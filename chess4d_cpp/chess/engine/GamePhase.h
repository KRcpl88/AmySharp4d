#pragma once

#include <string>

namespace tgreiner::amy::chess::engine {

class GamePhase final {
public:
    static const GamePhase OPENING;
    static const GamePhase MIDDLEGAME;
    static const GamePhase ENDGAME;

    std::string toString() const;

    friend bool operator==(const GamePhase& lhs, const GamePhase& rhs) noexcept
    {
        return lhs.value_ == rhs.value_;
    }

    friend bool operator!=(const GamePhase& lhs, const GamePhase& rhs) noexcept
    {
        return !(lhs == rhs);
    }

private:
    enum class Value {
        Opening,
        Middlegame,
        Endgame
    };

    constexpr GamePhase(Value value, const char* name) noexcept
        : value_(value), name_(name)
    {
    }

    Value value_;
    const char* name_;
};

} // namespace tgreiner::amy::chess::engine
