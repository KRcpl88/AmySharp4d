#include "chess/engine/SearchOutputXBoard.h"

#include <sstream>

#include "chess/engine/ISearcher.h"

namespace tgreiner::amy::chess::engine {

SearchOutputXBoard::SearchOutputXBoard(tgreiner::amy::common::engine::IComm& comm)
    : comm_(comm)
{
}

void SearchOutputXBoard::setPost(bool post)
{
    post_ = post;
}

bool SearchOutputXBoard::getPost() const
{
    return post_;
}

void SearchOutputXBoard::header()
{
}

void SearchOutputXBoard::pv(int iteration, int time, int score, const std::string& pvValue, std::int64_t nodes)
{
    if (!post_) {
        return;
    }

    if (time >= threshold_ || score > Searcher_Fields::MATE_LIMIT || score < -Searcher_Fields::MATE_LIMIT) {
        std::ostringstream line;
        line << iteration << ' ' << score << ' ' << (time / 10) << ' ' << nodes << ' ' << pvValue;
        comm_.OnResponse(line.str());
    }
}

void SearchOutputXBoard::move(int, int, const std::string&, int, int)
{
}

void SearchOutputXBoard::failHigh(int, int, const std::string&)
{
}

void SearchOutputXBoard::failLow(int, int, const std::string&)
{
}

} // namespace tgreiner::amy::chess::engine
