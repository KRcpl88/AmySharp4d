#pragma once

#include <string>
#include <vector>

namespace tgreiner::amy::chess::engine {

class ChessBoard;

class PVSaver {
public:
    int getPonderMove() const;
    void terminal(int ply);
    void move(int ply, int move);
    std::vector<int> getPV() const;
    std::string getPV(ChessBoard& board);

private:
    void ensureSize(int size);

    std::vector<std::vector<int>> pvs_;
};

} // namespace tgreiner::amy::chess::engine
