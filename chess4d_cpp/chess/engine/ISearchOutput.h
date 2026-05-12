#pragma once

#include <cstdint>
#include <string>

namespace tgreiner::amy::chess::engine {

class ISearchOutput {
public:
    virtual ~ISearchOutput() = default;

    virtual void header() = 0;
    virtual void pv(int iteration, int time, int score, const std::string& pv, std::int64_t nodes) = 0;
    virtual void move(int iteration, int time, const std::string& move, int count, int total) = 0;
    virtual void failHigh(int iteration, int time, const std::string& move) = 0;
    virtual void failLow(int iteration, int time, const std::string& move) = 0;
};

} // namespace tgreiner::amy::chess::engine
