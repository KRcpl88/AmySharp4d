/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

#include "chess\engine\recognizer\KBPKRecognizer.h"

namespace tgreiner::amy::chess::engine {
class ChessBoard;
}

namespace tgreiner::amy::chess::engine::recognizer {

class KBPKPRecognizer : public KBPKRecognizer {
public:
    int probe(const tgreiner::amy::chess::engine::ChessBoard& board) override;
};

} // namespace tgreiner::amy::chess::engine::recognizer
