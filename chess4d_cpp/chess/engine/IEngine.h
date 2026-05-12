#pragma once

#include <string>

namespace tgreiner::amy::chess::engine {

class IEngine {
public:
    virtual ~IEngine() = default;

    virtual void setPosition(const std::string& epd) = 0;
    virtual void doMove(int move) = 0;
};

} // namespace tgreiner::amy::chess::engine
