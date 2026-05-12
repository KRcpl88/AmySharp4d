/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

#include <cstdint>
#include <memory>
#include <unordered_map>

namespace tgreiner::amy::chess::engine {
class ChessBoard;
}

namespace tgreiner::amy::chess::engine::recognizer {

class IRecognizer;

class RecognizerMap {
public:
    RecognizerMap();

    IRecognizer* getRecognizer(const tgreiner::amy::chess::engine::ChessBoard& board) const;

private:
    using RecognizerPtr = std::shared_ptr<IRecognizer>;

    static int index(int p, int n, int b, int r, int q);
    static std::uint16_t key(int whiteSignature, int blackSignature);
    void init();

    std::unordered_map<std::uint16_t, RecognizerPtr> map_;
};

} // namespace tgreiner::amy::chess::engine::recognizer
