#pragma once

#include <array>
#include <cstdint>
#include <memory>
#include <vector>

#include "common/engine/NodeType.h"

namespace tgreiner::amy::chess::engine {

class CheckEvasionMoveGenerator;
class ChessBoard;
class ExtendedQuiescenceMoveGenerator;
class HistoryTable;
class IEvaluator;
class ITransTable;
class NonLoosingCaptureMoveGenerator;
class PVSaver;

class ExtendedQuiescenceSearch {
public:
    static constexpr int DEPTH_EXT_QSEARCH = -16;
    static constexpr int DEPTH_QSEARCH = -32;

    ExtendedQuiescenceSearch(ChessBoard& board, ITransTable& transTable, HistoryTable& history);
    ExtendedQuiescenceSearch(ChessBoard& board, PVSaver& pvsaver, ITransTable& transTable, HistoryTable& history);
    ~ExtendedQuiescenceSearch();

    std::int64_t getNodes() const;
    int search(int alpha, int beta, int depth, int ply);
    int search(int alpha, int beta, int depth, int ply, const tgreiner::amy::common::engine::NodeType& nodeType);
    void reset();
    void resetStats();
    PVSaver& getPVSaver();

private:
    void initGenerators(HistoryTable& history);
    int evadeCheck(int alpha, int beta, int depth, int ply);
    int extendedQ(int alpha, int beta, int ply);

    ChessBoard& board_;
    IEvaluator* evaluator_;
    std::vector<std::unique_ptr<NonLoosingCaptureMoveGenerator>> generators_;
    std::array<std::unique_ptr<CheckEvasionMoveGenerator>, 2> checkEvasionGenerators_;
    std::unique_ptr<ExtendedQuiescenceMoveGenerator> extendedGenerator_;
    std::unique_ptr<PVSaver> ownedPVSaver_;
    PVSaver* pvsaver_;
    ITransTable& transTable_;
    std::int64_t nodes_ = 0;

    static constexpr int MAX_DEPTH = 64;
};

} // namespace tgreiner::amy::chess::engine
