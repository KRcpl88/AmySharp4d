#pragma once

namespace tgreiner::amy::bitboard {

class Lfr {
public:
    int Level{0};
    int Rank{0};
    int File{0};

    Lfr() = default;
    Lfr(int level, int file, int rank);
    explicit Lfr(int offset);

    void Validate() const;
    bool IsValid() const;

    static bool IsValid(int level, int file, int rank);
    static bool IsValid(int offset);

    explicit operator int() const;

private:
    static void ValidateOffset(int offset);
};

} // namespace tgreiner::amy::bitboard
