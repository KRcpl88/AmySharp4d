#pragma once

#include <array>
#include <cstdint>
#include <memory>
#include <vector>

#include "bitboard/BitBoard.h"
#include "chess/engine/ISearcher.h"

namespace tgreiner::amy::common::engine {
class NodeType;
}

namespace tgreiner::amy::common::timer {
class IChessTimer;
}

namespace tgreiner::amy::chess::engine {

class CheckEvasionMoveGenerator;
class ChessBoard;
class ExtendedQuiescenceSearch;
class HistoryTable;
class IEvaluator;
class ISelectivity;
class ITransTable;
class MoveGenerator2;
class PVSaver;
class QuiescenceSearch;
class SelectivityImpl;

namespace recognizer {
class RecognizerMap;
}

class NegaScout : public ISearcher {
public:
    static constexpr int DEPTH_STEP = 16;

    NegaScout(ChessBoard& board, ITransTable& ttable, PVSaver& pvsaver, tgreiner::amy::common::timer::IChessTimer& timer);
    NegaScout(ChessBoard& board, ITransTable& ttable, HistoryTable& history, ISelectivity& selectivity, recognizer::RecognizerMap* recognizerMap, tgreiner::amy::common::timer::IChessTimer& timer);
    ~NegaScout();

    int getNodes() const override;
    void reset() override;
    int search(int alpha, int beta, int depth, const tgreiner::amy::common::engine::NodeType& nodeType) override;

    PVSaver& getPVSaver();

private:
    void initGenerators();
    int search(int alpha, int beta, int depth, int ply, const tgreiner::amy::common::engine::NodeType& nodeType);

    ChessBoard& board_;
    ITransTable& ttable_;
    HistoryTable* history_;
    std::unique_ptr<HistoryTable> ownedHistory_;
    std::unique_ptr<PVSaver> ownedPVSaver_;
    PVSaver* pvsaver_;
    ISelectivity* selectivity_;
    std::unique_ptr<SelectivityImpl> ownedSelectivity_;
    recognizer::RecognizerMap* recognizerMap_;
    std::unique_ptr<recognizer::RecognizerMap> ownedRecognizerMap_;
    tgreiner::amy::common::timer::IChessTimer& timer_;
    IEvaluator* evaluator_ = nullptr;
    std::unique_ptr<QuiescenceSearch> qsearch_;
    std::unique_ptr<ExtendedQuiescenceSearch> extQsearch_;
    std::vector<std::unique_ptr<MoveGenerator2>> generators_;
    std::vector<std::unique_ptr<CheckEvasionMoveGenerator>> checkEvasionGenerators_;
    std::array<int, tgreiner::amy::bitboard::BitBoard::SIZE> extensions_{};
    std::array<bool, tgreiner::amy::bitboard::BitBoard::SIZE> inCheckTab_{};
    std::int64_t nodes_ = 0;
    int nullTried_ = 0;
    int nullCutOffs_ = 0;

    static constexpr int MAX_DEPTH = tgreiner::amy::bitboard::BitBoard::SIZE;
    static constexpr int REDUCE_NULLMOVE = 3 * DEPTH_STEP;
};

} // namespace tgreiner::amy::chess::engine
