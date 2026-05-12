#pragma once

#include <any>
#include <cstddef>
#include <cstdint>
#include <filesystem>
#include <fstream>
#include <iosfwd>
#include <istream>
#include <memory>
#include <random>
#include <string>
#include <vector>

namespace tgreiner::amy {

class IThreadRunnable {
public:
    virtual ~IThreadRunnable() = default;
    virtual void Run() = 0;
};

class SupportClass final {
public:
    static std::uint32_t URShift(std::uint32_t number, int bits);
    static std::int32_t URShift(std::int32_t number, long bits);
    static std::uint64_t URShift(std::uint64_t number, int bits);
    static std::int64_t URShift(std::int64_t number, long bits);

    class SetSupport {
    public:
        virtual ~SetSupport() = default;
        virtual bool Add(const std::any& obj) = 0;
        virtual bool AddAll(const std::vector<std::any>& collection) = 0;
    };

    class Tokenizer {
    public:
        explicit Tokenizer(const std::string& source);
        Tokenizer(const std::string& source, const std::string& delimiters);
        Tokenizer(const std::string& source, const std::string& delimiters, bool includeDelims);

        std::string NextToken();
        std::string NextToken(const std::string& delimiters);
        bool HasMoreTokens();
        int Count();
        std::string Current();
        bool MoveNext();
        void Reset();

    private:
        std::string nextToken(const std::string& delimiters);

        std::size_t currentPos_ = 0;
        bool includeDelims_ = false;
        std::string chars_;
        std::string delimiters_ = " \t\n\r\f";
    };

    class StringIterator {
    public:
        static constexpr char DONE = static_cast<char>(0xFF);

        StringIterator();
        explicit StringIterator(const std::string& text);
        StringIterator(const std::string& text, int pos);
        StringIterator(const std::string& text, int begin, int end, int pos);

        char Current() const;
        char First();
        int BeginIndex() const;
        int EndIndex() const;
        int GetIndex() const;
        char SetIndex(int index);
        char Last();
        char Next();
        char Previous();

    private:
        int begin_ = 0;
        int end_ = 0;
        int position_ = 0;
        std::string source_;
    };

    static std::int64_t NextLong(std::mt19937& random);
    static std::vector<std::uint8_t> ToByteArray(const std::vector<std::int8_t>& sbyteArray);
    static std::vector<std::uint8_t> ToByteArray(const std::string& sourceString);
    static std::vector<std::uint8_t> ToByteArray(const std::vector<std::any>& tempObjectArray);
    static std::vector<std::int8_t> ToSByteArray(const std::vector<std::uint8_t>& byteArray);

    class BackReader {
    public:
        BackReader(std::istream& streamReader, std::size_t size);
        explicit BackReader(std::istream& streamReader);

        bool MarkSupported() const;
        void Mark(int position);
        void Reset();
        int Read();
        std::streamsize Read(char* array, std::streamsize count);
        bool IsReady() const;
        void UnRead(int unReadChar);
        void UnRead(const char* array, std::size_t count);
        void UnRead(const std::vector<char>& array);

    private:
        void Move(const char* array, std::size_t count);

        std::istream& stream_;
        std::vector<char> buffer_;
        std::size_t position_ = 0;
    };

    static std::uintmax_t FileLength(const std::filesystem::path& file);

    class RandomAccessFileSupport {
    public:
        static std::unique_ptr<std::fstream> CreateRandomAccessFile(const std::filesystem::path& fileName, const std::string& mode);
        static void WriteBytes(const std::string& data, std::ostream& fileStream);
        static void WriteChars(const std::string& data, std::ostream& fileStream);
        static void WriteRandomFile(const std::vector<std::int8_t>& sByteArray, std::ostream& fileStream);
    };

private:
    SupportClass() = delete;
};

} // namespace tgreiner::amy
