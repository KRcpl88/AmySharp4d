#pragma once

#include <string>

#include "common/engine/IntVector.h"

namespace tgreiner::amy::chess::engine {

class ChessBoard;

class GameEndRecognizer {
public:
    static constexpr const char* DRAW = "1/2-1/2";
    static constexpr const char* WHITE_WINS = "1-0";
    static constexpr const char* BLACK_WINS = "0-1";

    GameEndRecognizer() = default;

    const std::string& getResult() const;
    const std::string& getComment() const;
    bool isGameEnded(ChessBoard& board);

private:
    tgreiner::amy::common::engine::IntVector legalMoves_;
    std::string result_;
    std::string comment_;
};

} // namespace tgreiner::amy::chess::engine
