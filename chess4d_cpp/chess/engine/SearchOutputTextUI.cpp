#include "chess/engine/SearchOutputTextUI.h"

#include <algorithm>
#include <iostream>
#include <ostream>
#include <sstream>
#include <vector>

#include "chess/engine/Formatter.h"
#include "chess/engine/ISearcher.h"

namespace tgreiner::amy::chess::engine {

namespace {
std::string leftPad(std::string value, std::size_t width)
{
    if (value.size() >= width) {
        return value;
    }
    return std::string(width - value.size(), ' ') + value;
}
}

SearchOutputTextUI::SearchOutputTextUI()
    : out_(std::cout)
{
}

SearchOutputTextUI::SearchOutputTextUI(std::ostream& out)
    : out_(out)
{
}

void SearchOutputTextUI::header()
{
    out_ << "It    Time   Score  principal Variation" << std::endl;
}

void SearchOutputTextUI::pv(int iteration, int time, int score, const std::string& pvValue, std::int64_t nodes)
{
    if (time >= threshold_ || score > Searcher_Fields::MATE_LIMIT || score < -Searcher_Fields::MATE_LIMIT) {
        out_ << formatIteration(iteration)
             << formatTime(time)
             << formatScore(score)
             << "  "
             << formatPV(pvValue + " [" + formatNodes(nodes) + "]")
             << std::endl;
    }
}

void SearchOutputTextUI::move(int iteration, int time, const std::string& moveValue, int count, int total)
{
    if (false && time >= threshold_) {
        out_ << formatIteration(iteration)
             << formatTime(time)
             << formatCnt(count, total)
             << "  "
             << moveValue
             << "     \r";
        out_.flush();
    }
}

void SearchOutputTextUI::failHigh(int iteration, int time, const std::string& moveValue)
{
    if (time >= threshold_) {
        out_ << formatIteration(iteration)
             << formatTime(time)
             << "     +++  "
             << moveValue
             << "     "
             << std::endl;
    }
}

void SearchOutputTextUI::failLow(int iteration, int time, const std::string& moveValue)
{
    if (time >= threshold_) {
        out_ << formatIteration(iteration)
             << formatTime(time)
             << "     ---  "
             << moveValue
             << "     "
             << std::endl;
    }
}

std::string SearchOutputTextUI::formatIteration(int iter) const
{
    std::string result = std::to_string(iter);
    if (iter < 10) {
        result = ' ' + result;
    }
    return result;
}

std::string SearchOutputTextUI::formatTime(int time) const
{
    return leftPad(Formatter::timeToString(time), 8);
}

std::string SearchOutputTextUI::formatScore(int score) const
{
    return leftPad(Formatter::scoreToString(score), 8);
}

std::string SearchOutputTextUI::formatCnt(int count, int total) const
{
    return leftPad(std::to_string(count + 1) + "/" + std::to_string(total), 8);
}

std::string SearchOutputTextUI::formatNodes(std::int64_t nodes) const
{
    if (nodes >= 1000) {
        return std::to_string(nodes / 1000) + " kN";
    }
    return "0." + std::to_string(nodes / 100) + " kN";
}

std::string SearchOutputTextUI::formatPV(const std::string& thePV) const
{
    const std::string prefix = "                    ";
    const int maxLen = WIDTH - static_cast<int>(prefix.size()) - 1;
    if (static_cast<int>(thePV.size()) < maxLen) {
        return thePV;
    }

    std::istringstream stream(thePV);
    std::vector<std::string> tokens;
    std::string token;
    while (stream >> token) {
        tokens.push_back(token);
    }

    std::string result;
    std::string line;
    bool withPrefix = false;

    for (const std::string& current : tokens) {
        if (static_cast<int>(line.size() + current.size() + 1) < maxLen) {
            if (!line.empty()) {
                line += ' ';
            }
            line += current;
        } else {
            if (withPrefix) {
                result += prefix;
            }
            result += line;
            result += '\n';
            line = current;
            withPrefix = true;
        }
    }

    if (!line.empty()) {
        if (withPrefix) {
            result += prefix;
        }
        result += line;
    }

    return result;
}

} // namespace tgreiner::amy::chess::engine
