#pragma once

#include <cstdint>
#include <memory>
#include <vector>

#include "bitboard/BitBoard.h"
#include "chess/engine/ISearcher.h"

namespace tgreiner::amy::common::engine {
class NodeType;
}

namespace tgreiner::amy::chess::engine {

class ChessBoard;
class HistoryTable;
class ITransTable;
class MoveGenerator;
class PVSaver;
class QuiescenceSearch;

class AlphaBeta : public ISearcher {
public:
    AlphaBeta(ChessBoard& board, ITransTable& ttable, HistoryTable& history);
    AlphaBeta(ChessBoard& board, ITransTable& ttable, PVSaver& pvsaver);
    ~AlphaBeta();

    int getNodes() const override;
    void reset() override;
    int search(int alpha, int beta, int depth, const tgreiner::amy::common::engine::NodeType& nodeType) override;

    PVSaver& getPVSaver();

private:
    void initGenerators();
    int search(int alpha, int beta, int depth, int ply);

    ChessBoard& board_;
    ITransTable& ttable_;
    HistoryTable* history_;
    std::unique_ptr<HistoryTable> ownedHistory_;
    std::unique_ptr<PVSaver> ownedPVSaver_;
    PVSaver* pvsaver_;
    std::unique_ptr<QuiescenceSearch> qsearch_;
    std::vector<std::unique_ptr<MoveGenerator>> generators_;
    std::int64_t nodes_ = 0;

    static constexpr int MAX_DEPTH = tgreiner::amy::bitboard::BitBoard::SIZE;
};

} // namespace tgreiner::amy::chess::engine
