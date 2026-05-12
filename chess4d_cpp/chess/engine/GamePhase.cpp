#include "chess/engine/GamePhase.h"

namespace tgreiner::amy::chess::engine {

const GamePhase GamePhase::OPENING(GamePhase::Value::Opening, "Opening");
const GamePhase GamePhase::MIDDLEGAME(GamePhase::Value::Middlegame, "Middlegame");
const GamePhase GamePhase::ENDGAME(GamePhase::Value::Endgame, "Endgame");

std::string GamePhase::toString() const
{
    return name_;
}

} // namespace tgreiner::amy::chess::engine
