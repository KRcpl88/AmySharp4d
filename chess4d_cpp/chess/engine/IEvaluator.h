/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

namespace tgreiner::amy::chess::engine {

/// Interface for position evaluators.
class IEvaluator {
public:
    virtual ~IEvaluator() = default;

    virtual int getWhiteMaterial() const = 0;
    virtual int getBlackMaterial() const = 0;
    virtual int evaluate(int alpha, int beta) = 0;
    virtual void init() = 0;
    virtual void move(int from, int to, int type, bool whiteToMove) = 0;
    virtual void capture(int square, int type, bool whiteToMove) = 0;
    virtual void add(int square, int type, bool whiteToMove) = 0;
    virtual int  getMaterialValue(int piece) const = 0;
};

} // namespace tgreiner::amy::chess::engine
