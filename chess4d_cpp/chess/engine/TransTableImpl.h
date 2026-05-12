#pragma once

#include <vector>

#include "chess/engine/ITransTable.h"
#include "chess/engine/TTEntry.h"

namespace tgreiner::amy::chess::engine {

class TransTableImpl : public ITransTable {
public:
    explicit TransTableImpl(int bits);

    TTEntry* get_Renamed(std::int64_t hashkey) override;
    void store(std::int64_t hashkey, int move, int depth, int score, int alpha, int beta) override;
    void clear() override;

private:
    int size_;
    int mask_;
    std::vector<TTEntry> table_;
};

} // namespace tgreiner::amy::chess::engine
