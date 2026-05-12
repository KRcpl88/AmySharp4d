#include "chess/engine/PVSaver.h"

#include <algorithm>

#include "chess/engine/ChessBoard.h"
#include "chess/engine/Move.h"

namespace tgreiner::amy::chess::engine {

int PVSaver::getPonderMove() const
{
    if (pvs_.empty() || pvs_[0].size() <= 1) {
        return 0;
    }

    return pvs_[0][1];
}

void PVSaver::ensureSize(int size)
{
    while (static_cast<int>(pvs_.size()) < size) {
        pvs_.emplace_back();
    }
}

void PVSaver::terminal(int ply)
{
    ensureSize(ply + 1);

    std::vector<int>& pv = pvs_[ply];
    if (static_cast<int>(pv.size()) != ply) {
        pv.assign(static_cast<std::size_t>(ply), 0);
    }
}

void PVSaver::move(int ply, int moveValue)
{
    ensureSize(ply + 2);

    const std::vector<int>& previousPV = pvs_[ply + 1];
    std::vector<int>& pv = pvs_[ply];

    const int len = std::max(static_cast<int>(previousPV.size()), ply + 1);
    if (static_cast<int>(pv.size()) != len) {
        pv.assign(static_cast<std::size_t>(len), 0);
    }

    pv[ply] = moveValue;

    if (static_cast<int>(previousPV.size()) > (ply + 1)) {
        std::copy(previousPV.begin() + ply + 1, previousPV.end(), pv.begin() + ply + 1);
    }
}

std::vector<int> PVSaver::getPV() const
{
    if (pvs_.empty()) {
        return {};
    }

    return pvs_[0];
}

std::string PVSaver::getPV(ChessBoard& board)
{
    static const std::vector<int> emptyPv;

    std::string result;
    int i = 0;
    const std::vector<int>& pv = pvs_.empty() ? emptyPv : pvs_[0];

    for (; i < static_cast<int>(pv.size()); ++i) {
        const int moveValue = pv[i];
        if (!board.isLegalMove(moveValue)) {
            break;
        }

        result += Move::toSAN(board, moveValue);
        result += ' ';
        board.doMove(moveValue);
    }

    for (int j = i - 1; j >= 0; --j) {
        board.undoMove();
    }

    return result;
}

} // namespace tgreiner::amy::chess::engine
