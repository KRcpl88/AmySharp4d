#pragma once

#include <cstdint>
#include <iosfwd>
#include <string>

#include "chess/engine/ISearchOutput.h"

namespace tgreiner::amy::chess::engine {

class SearchOutputTextUI : public ISearchOutput {
public:
    SearchOutputTextUI();
    explicit SearchOutputTextUI(std::ostream& out);

    void header() override;
    void pv(int iteration, int time, int score, const std::string& pv, std::int64_t nodes) override;
    void move(int iteration, int time, const std::string& move, int count, int total) override;
    void failHigh(int iteration, int time, const std::string& move) override;
    void failLow(int iteration, int time, const std::string& move) override;

private:
    std::string formatIteration(int iter) const;
    std::string formatTime(int time) const;
    std::string formatScore(int score) const;
    std::string formatCnt(int count, int total) const;
    std::string formatNodes(std::int64_t nodes) const;
    std::string formatPV(const std::string& pv) const;

    std::ostream& out_;
    int threshold_ = 300;

    static constexpr int WIDTH = 80;
};

} // namespace tgreiner::amy::chess::engine
