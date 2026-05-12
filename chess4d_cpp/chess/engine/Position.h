#pragma once

#include <vector>

#include "chess\engine\ChessConstants.h"

namespace tgreiner::amy::chess::engine {

class IPosition {
public:
    virtual ~IPosition() = default;

    virtual const std::vector<int>& getBoard() const = 0;
    virtual bool getWtm() const = 0;
    virtual int getEnPassantSquare() const = 0;
    virtual bool canWhiteCastleKingSide() const = 0;
    virtual bool canWhiteCastleQueenSide() const = 0;
    virtual bool canBlackCastleKingSide() const = 0;
    virtual bool canBlackCastleQueenSide() const = 0;
};

class InitialPosition final : public IPosition {
public:
    InitialPosition() : board_(ChessConstants::INITIAL_BOARD.begin(), ChessConstants::INITIAL_BOARD.end()) {}

    const std::vector<int>& getBoard() const override { return board_; }
    bool getWtm() const override { return true; }
    int getEnPassantSquare() const override { return 0; }
    bool canWhiteCastleKingSide() const override { return true; }
    bool canWhiteCastleQueenSide() const override { return true; }
    bool canBlackCastleKingSide() const override { return true; }
    bool canBlackCastleQueenSide() const override { return true; }

private:
    std::vector<int> board_;
};

struct Position_Fields {
    inline static const InitialPosition INITIAL_POSITION{};
};

} // namespace tgreiner::amy::chess::engine
