#pragma once

#include <cstdint>
#include <memory>
#include <vector>

#include "bitboard/BitBoard.h"

namespace tgreiner::amy::chess::engine {

class ChessBoard;
class IEvaluator;
class NonLoosingCaptureMoveGenerator;
class PVSaver;

class QuiescenceSearch {
public:
    explicit QuiescenceSearch(ChessBoard& board);
    QuiescenceSearch(ChessBoard& board, PVSaver& pvsaver);
    ~QuiescenceSearch();

    std::int64_t getNodes() const;
    int search(int alpha, int beta, int depth, int ply);
    void reset();
    void resetStats();
    PVSaver& getPVSaver();

private:
    void initGenerators();

    ChessBoard& board_;
    IEvaluator* evaluator_;
    std::vector<std::unique_ptr<NonLoosingCaptureMoveGenerator>> generators_;
    std::unique_ptr<PVSaver> ownedPVSaver_;
    PVSaver* pvsaver_;
    std::int64_t nodes_ = 0;

    static constexpr int MAX_DEPTH = tgreiner::amy::bitboard::BitBoard::SIZE;
};

} // namespace tgreiner::amy::chess::engine
