/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

namespace tgreiner::amy::chess::engine {
class ChessBoard;
}

namespace tgreiner::amy::chess::engine::recognizer {

class IRecognizer {
public:
    static constexpr int USELESS = 0;
    static constexpr int EXACT = 1;
    static constexpr int LOWER_BOUND = 2;
    static constexpr int UPPER_BOUND = 3;

    virtual ~IRecognizer() = default;

    virtual int getValue() const = 0;
    virtual int probe(const tgreiner::amy::chess::engine::ChessBoard& board) = 0;
};

} // namespace tgreiner::amy::chess::engine::recognizer
