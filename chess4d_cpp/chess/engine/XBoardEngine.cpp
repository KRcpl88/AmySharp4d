#include "chess/engine/XBoardEngine.h"

#include <iostream>
#include <sstream>
#include <stdexcept>

#include "bitboard/LevelRankFile.h"
#include "chess/engine/Driver.h"
#include "chess/engine/Move.h"
#include "chess/engine/PonderThread.h"
#include "common/timer/QuotaTimeControl.h"
#include "common/timer/SuddenDeathTimeControl.h"

namespace tgreiner::amy::chess::engine {

using tgreiner::amy::bitboard::Lfr;
using tgreiner::amy::common::timer::QuotaTimeControl;
using tgreiner::amy::common::timer::SuddenDeathTimeControl;

XBoardEngine::XBoardEngine(tgreiner::amy::common::engine::IComm& comm)
    : comm_(comm),
      board_(),
      ttable_(16),
      searchOutput_(comm),
      timerAlgorithm_(0, 0),
      timer_(timerAlgorithm_)
{
    worker_ = std::thread(&XBoardEngine::run, this);

    std::unique_lock<std::mutex> lock(commandMutex_);
    startedCv_.wait(lock, [this] { return started_; });
}

XBoardEngine::~XBoardEngine()
{
    {
        std::lock_guard<std::mutex> lock(commandMutex_);
        command_ = "quit";
        hasCommand_ = true;
    }
    inputCv_.notify_one();

    if (worker_.joinable()) {
        worker_.join();
    }
}

void XBoardEngine::run()
{
    {
        std::lock_guard<std::mutex> lock(commandMutex_);
        started_ = true;
    }
    startedCv_.notify_one();

    sendFeatures();
    commandLoop();
}

void XBoardEngine::Process(const std::string& cmd)
{
    {
        std::lock_guard<std::mutex> lock(commandMutex_);
        command_ = cmd;
        hasCommand_ = true;
    }
    inputCv_.notify_one();
}

void XBoardEngine::commandLoop()
{
    bool respond = true;
    const std::regex levelPattern(R"(level (\d+) (\d+)(:(\d+))? (\d+))");

    for (;;) {
        std::string currentCommand;
        {
            std::unique_lock<std::mutex> lock(commandMutex_);
            inputCv_.wait(lock, [this] { return hasCommand_; });
            currentCommand = command_;
            hasCommand_ = false;
        }

        if (currentCommand == "quit") {
            if (ponderThread_) {
                ponderThread_->abort();
                ponderThread_.reset();
            }
            return;
        }

        if (currentCommand.starts_with("log ")) {
            setLogFile(currentCommand.substr(4));
            continue;
        }

        if (currentCommand.starts_with("new")) {
            if (currentCommand.size() > 4) {
                board_ = ChessBoard(currentCommand.substr(4));
            } else {
                board_ = ChessBoard();
            }
            timerAlgorithm_.setDuration(0, 0);
            ponderThread_.reset();
            timeControl_.reset();
            PrintBoard(printHex);
            respond = true;
            continue;
        }

        if (currentCommand.starts_with("printhex")) {
            printHex = true;
            PrintBoard(printHex);
            continue;
        }

        if (currentCommand.starts_with("printsquare")) {
            printHex = false;
            PrintBoard(printHex);
            continue;
        }

        if (currentCommand.starts_with("usermove ")) {
            const std::string moveStr = currentCommand.substr(9);
            try {
                const int move = Move::parseSAN(board_, moveStr);
                handleMove(respond, move);
            } catch (const std::exception& ex) {
                logError(std::string("Got bad move ") + moveStr + ": " + ex.what());
            }
            continue;
        }

        if (currentCommand.starts_with("showmoves ")) {
            const std::string squareStr = currentCommand.substr(10);
            try {
                if (squareStr.size() < 3) {
                    throw std::out_of_range("square too short");
                }
                const Lfr lfr(squareStr[0] - 'a', squareStr[1] - 'a', squareStr[2] - '1');
                const int square = static_cast<int>(lfr);
                if (board_.getPieceAt(square) != 0) {
                    std::cout << board_.toString(square) << std::endl;
                }
            } catch (const std::exception&) {
                logError(std::string("Invalid square ") + squareStr);
            }
            continue;
        }

        if (currentCommand == "force") {
            if (ponderThread_) {
                ponderThread_->abort();
                ponderThread_.reset();
            }
            respond = false;
            continue;
        }

        if (currentCommand == "hard") {
            ponder_ = true;
            continue;
        }

        if (currentCommand == "easy") {
            ponder_ = false;
            continue;
        }

        if (currentCommand == "go") {
            go();
            respond = true;
            continue;
        }

        if (currentCommand == "undo") {
            board_.undoMove();
            continue;
        }

        if (currentCommand == "post") {
            searchOutput_.setPost(true);
            continue;
        }

        if (currentCommand == "nopost") {
            searchOutput_.setPost(false);
            continue;
        }

        if (currentCommand.starts_with("setboard ")) {
            const std::string fen = currentCommand.substr(9);
            try {
                board_ = ChessBoard(fen);
            } catch (const std::exception&) {
                comm_.OnResponse("tellusererror Illegal position");
            }
            continue;
        }

        if (currentCommand.starts_with("time")) {
            if (timeControl_) {
                const int remaining = std::stoi(currentCommand.substr(5));
                logDebug("Time remaining is " + std::to_string(remaining));
                timeControl_->setRemainingTime(10 * remaining);
                timerAlgorithm_.setDuration(timeControl_->getSoftLimit(), timeControl_->getHardLimit());
            }
            continue;
        }

        if (currentCommand.starts_with("level ")) {
            std::smatch levelMatch;
            if (std::regex_match(currentCommand, levelMatch, levelPattern)) {
                const int moves = std::stoi(levelMatch[1].str());
                const int time = std::stoi(levelMatch[2].str()) * 60;
                if (moves != 0) {
                    logDebug(std::to_string(moves) + " moves in " + std::to_string(time) + " seconds");
                    timeControl_ = std::make_unique<QuotaTimeControl>(moves, time);
                } else {
                    logDebug("All moves in " + std::to_string(time) + " seconds");
                    timeControl_ = std::make_unique<SuddenDeathTimeControl>(time);
                }
            } else {
                logError("Got bad move or command : " + currentCommand);
            }
            continue;
        }
    }
}

void XBoardEngine::PrintBoard(bool hex) const
{
    if (hex) {
        std::cout << board_.toStringHex(-1) << std::endl;
    } else {
        std::cout << board_.toString(-1) << std::endl;
    }
}

void XBoardEngine::handleMove(bool respond, int move)
{
    board_.doMove(move);
    checkGameEnd();

    if (respond) {
        if (ponder_ && ponderThread_) {
            if (ponderThread_->getPonderMove() == move) {
                ponderThread_->ponderHit();
            } else {
                ponderThread_->abort();
                ponderThread_.reset();
            }
        }

        go();
    }
}

void XBoardEngine::go()
{
    if (ponderThread_) {
        const int bestMove = ponderThread_->getBestMove();
        const int ponderMove = ponderThread_->getNextPonderMove();
        ponderThread_.reset();

        if (bestMove == 0) {
            return;
        }

        comm_.OnResponse("move " + Move::toSAN(board_, bestMove));
        board_.doMove(bestMove);
        PrintBoard(printHex);
        checkGameEnd();

        if (ponder_ && board_.isLegalMove(ponderMove)) {
            ponderThread_ = std::make_unique<PonderThread>(board_, ponderMove, ttable_, timer_, searchOutput_, maxDepth_);
        }
        return;
    }

    ChessBoard searchBoard(board_);
    Driver driver(searchBoard, ttable_, timer_);
    driver.setSearchOutput(&searchOutput_);
    const int move = driver.search(maxDepth_);

    if (move == 0) {
        return;
    }

    comm_.OnResponse("move " + Move::toSAN(board_, move));
    board_.doMove(move);
    PrintBoard(printHex);
    checkGameEnd();

    const int ponderMove = driver.getPonderMove();
    if (ponder_ && board_.isLegalMove(ponderMove)) {
        ponderThread_ = std::make_unique<PonderThread>(board_, ponderMove, ttable_, timer_, searchOutput_, maxDepth_);
    }
}

void XBoardEngine::checkGameEnd()
{
    if (gameEndRecognizer_.isGameEnded(board_)) {
        comm_.OnResponse(gameEndRecognizer_.getResult() + " {" + gameEndRecognizer_.getComment() + "}");
    }
}

void XBoardEngine::sendFeatures()
{
    comm_.OnResponse("feature done=0");
    comm_.OnResponse("feature myname=\"AmyJ 0.1\"");
    comm_.OnResponse("feature usermove=1");
    comm_.OnResponse("feature san=1");
    comm_.OnResponse("feature sigint=0");
    comm_.OnResponse("feature sigterm=0");
    comm_.OnResponse("feature setboard=1");
    comm_.OnResponse("feature done=1");
}

void XBoardEngine::logDebug(const std::string& message)
{
    if (logStream_) {
        logStream_ << "DEBUG " << message << std::endl;
    }
}

void XBoardEngine::logError(const std::string& message)
{
    if (logStream_) {
        logStream_ << "ERROR " << message << std::endl;
    }
}

void XBoardEngine::setLogFile(const std::string& filename)
{
    logStream_.close();
    logStream_.open(filename, std::ios::out | std::ios::app);
    if (!logStream_) {
        comm_.OnResponse("tellusererror Unable to open log file");
    }
}

} // namespace tgreiner::amy::chess::engine
