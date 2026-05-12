/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#include "chess\engine\recognizer\RecognizerMap.h"

#include "chess\engine\ChessBoard.h"
#include "chess\engine\recognizer\IRecognizer.h"
#include "chess\engine\recognizer\KBPKPRecognizer.h"
#include "chess\engine\recognizer\KBPKRecognizer.h"

namespace tgreiner::amy::chess::engine::recognizer {

RecognizerMap::RecognizerMap()
{
    init();
}

IRecognizer* RecognizerMap::getRecognizer(const tgreiner::amy::chess::engine::ChessBoard& board) const
{
    const auto it = map_.find(key(board.getMaterialSignature(true), board.getMaterialSignature(false)));
    return it == map_.end() ? nullptr : it->second.get();
}

int RecognizerMap::index(int p, int n, int b, int r, int q)
{
    return p | (n << 1) | (b << 2) | (r << 3) | (q << 4);
}

std::uint16_t RecognizerMap::key(int whiteSignature, int blackSignature)
{
    return static_cast<std::uint16_t>((whiteSignature << 8) | blackSignature);
}

void RecognizerMap::init()
{
    const auto kbpk = std::make_shared<KBPKRecognizer>();

    map_.emplace(key(index(0, 0, 0, 0, 0), index(1, 0, 1, 0, 0)), kbpk);
    map_.emplace(key(index(1, 0, 1, 0, 0), index(0, 0, 0, 0, 0)), kbpk);

    const auto kbpkp = std::make_shared<KBPKPRecognizer>();

    map_.emplace(key(index(1, 0, 0, 0, 0), index(1, 0, 1, 0, 0)), kbpkp);
    map_.emplace(key(index(1, 0, 1, 0, 0), index(1, 0, 0, 0, 0)), kbpkp);
}

} // namespace tgreiner::amy::chess::engine::recognizer
