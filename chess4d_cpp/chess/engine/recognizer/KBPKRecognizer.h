/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

#include "chess\engine\recognizer\IRecognizer.h"

namespace tgreiner::amy::chess::engine {
class ChessBoard;
}

namespace tgreiner::amy::chess::engine::recognizer {

class KBPKRecognizer : public IRecognizer {
public:
    int getValue() const override;
    int probe(const tgreiner::amy::chess::engine::ChessBoard& board) override;

protected:
    virtual bool blackKingDefendsH8(const tgreiner::amy::chess::engine::ChessBoard& board) const;
    virtual bool blackKingDefendsA8(const tgreiner::amy::chess::engine::ChessBoard& board) const;
    virtual bool whiteKingDefendsH1(const tgreiner::amy::chess::engine::ChessBoard& board) const;
    virtual bool whiteKingDefendsA1(const tgreiner::amy::chess::engine::ChessBoard& board) const;
};

} // namespace tgreiner::amy::chess::engine::recognizer
