#pragma once

namespace tgreiner::amy::common::engine {
class NodeType;
}

namespace tgreiner::amy::chess::engine {

struct Searcher_Fields {
    static constexpr int MATE = 32768;
    static constexpr int MATE_LIMIT = MATE - 100;
};

class ISearcher {
public:
    virtual ~ISearcher() = default;

    virtual int getNodes() const = 0;
    virtual void reset() = 0;
    virtual int search(int alpha, int beta, int depth, const tgreiner::amy::common::engine::NodeType& nodeType) = 0;
};

} // namespace tgreiner::amy::chess::engine
