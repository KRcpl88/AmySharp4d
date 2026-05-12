#pragma once

#include <cstdint>
#include <string>

#include "chess/engine/ISearchOutput.h"
#include "common/engine/IComm.h"

namespace tgreiner::amy::chess::engine {

class SearchOutputXBoard : public ISearchOutput {
public:
    explicit SearchOutputXBoard(tgreiner::amy::common::engine::IComm& comm);

    void setPost(bool post);
    bool getPost() const;

    void header() override;
    void pv(int iteration, int time, int score, const std::string& pv, std::int64_t nodes) override;
    void move(int iteration, int time, const std::string& move, int count, int total) override;
    void failHigh(int iteration, int time, const std::string& move) override;
    void failLow(int iteration, int time, const std::string& move) override;

private:
    int threshold_ = 300;
    bool post_ = true;
    tgreiner::amy::common::engine::IComm& comm_;
};

} // namespace tgreiner::amy::chess::engine
