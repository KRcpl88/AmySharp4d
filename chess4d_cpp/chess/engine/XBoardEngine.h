#pragma once

#include <condition_variable>
#include <fstream>
#include <memory>
#include <mutex>
#include <regex>
#include <string>
#include <thread>

#include "chess/engine/ChessBoard.h"
#include "chess/engine/GameEndRecognizer.h"
#include "chess/engine/SearchOutputXBoard.h"
#include "chess/engine/TransTableImpl2.h"
#include "common/timer/AlgorithmBasedTimer.h"
#include "common/timer/ExtendOnFailLowTimerAlgorithm.h"
#include "common/timer/TimeControl.h"

namespace tgreiner::amy::common::engine {
class IComm;
}

namespace tgreiner::amy::chess::engine {

class PonderThread;

class XBoardEngine {
public:
    explicit XBoardEngine(tgreiner::amy::common::engine::IComm& comm);
    ~XBoardEngine();

    void run();
    void Process(const std::string& cmd);

    bool printHex = false;

private:
    void commandLoop();
    void PrintBoard(bool hex) const;
    void handleMove(bool respond, int move);
    void go();
    void checkGameEnd();
    void sendFeatures();
    void logDebug(const std::string& message);
    void logError(const std::string& message);
    void setLogFile(const std::string& filename);

    tgreiner::amy::common::engine::IComm& comm_;
    ChessBoard board_;
    TransTableImpl2 ttable_;
    SearchOutputXBoard searchOutput_;
    tgreiner::amy::common::timer::ExtendOnFailLowTimerAlgorithm timerAlgorithm_;
    tgreiner::amy::common::timer::AlgorithmBasedTimer timer_;
    std::unique_ptr<PonderThread> ponderThread_;
    GameEndRecognizer gameEndRecognizer_;
    std::unique_ptr<tgreiner::amy::common::timer::TimeControl> timeControl_;
    int maxDepth_ = 50;
    bool ponder_ = false;

    std::thread worker_;
    mutable std::mutex commandMutex_;
    std::condition_variable inputCv_;
    std::condition_variable startedCv_;
    std::string command_;
    bool hasCommand_ = false;
    bool started_ = false;
    std::ofstream logStream_;
};

} // namespace tgreiner::amy::chess::engine
