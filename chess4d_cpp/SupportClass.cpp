#include "SupportClass.h"

#include <algorithm>
#include <cstring>
#include <limits>
#include <stdexcept>

namespace tgreiner::amy {

namespace {
bool IsDelimiter(char ch, const std::string& delimiters)
{
    return delimiters.find(ch) != std::string::npos;
}

std::uint8_t AnyToByte(const std::any& value)
{
    if (value.type() == typeid(std::uint8_t)) {
        return std::any_cast<std::uint8_t>(value);
    }
    if (value.type() == typeid(std::int8_t)) {
        return static_cast<std::uint8_t>(std::any_cast<std::int8_t>(value));
    }
    if (value.type() == typeid(char)) {
        return static_cast<std::uint8_t>(std::any_cast<char>(value));
    }
    if (value.type() == typeid(int)) {
        return static_cast<std::uint8_t>(std::any_cast<int>(value));
    }
    if (value.type() == typeid(unsigned int)) {
        return static_cast<std::uint8_t>(std::any_cast<unsigned int>(value));
    }
    throw std::invalid_argument("Unsupported value in ToByteArray");
}
}

std::uint32_t SupportClass::URShift(std::uint32_t number, int bits)
{
    return number >> bits;
}

std::int32_t SupportClass::URShift(std::int32_t number, long bits)
{
    return static_cast<std::int32_t>(static_cast<std::uint32_t>(number) >> bits);
}

std::uint64_t SupportClass::URShift(std::uint64_t number, int bits)
{
    return number >> bits;
}

std::int64_t SupportClass::URShift(std::int64_t number, long bits)
{
    return static_cast<std::int64_t>(static_cast<std::uint64_t>(number) >> bits);
}

SupportClass::Tokenizer::Tokenizer(const std::string& source)
    : chars_(source)
{
}

SupportClass::Tokenizer::Tokenizer(const std::string& source, const std::string& delimiters)
    : Tokenizer(source)
{
    delimiters_ = delimiters;
}

SupportClass::Tokenizer::Tokenizer(const std::string& source, const std::string& delimiters, bool includeDelims)
    : Tokenizer(source, delimiters)
{
    includeDelims_ = includeDelims;
}

std::string SupportClass::Tokenizer::NextToken()
{
    return NextToken(delimiters_);
}

std::string SupportClass::Tokenizer::NextToken(const std::string& delimiters)
{
    delimiters_ = delimiters;

    if (currentPos_ >= chars_.size()) {
        throw std::out_of_range("No more tokens");
    }

    if (includeDelims_ && IsDelimiter(chars_[currentPos_], delimiters_)) {
        return std::string(1, chars_[currentPos_++]);
    }

    return nextToken(delimiters_);
}

std::string SupportClass::Tokenizer::nextToken(const std::string& delimiters)
{
    const std::size_t startPos = currentPos_;

    while (currentPos_ < chars_.size() && IsDelimiter(chars_[currentPos_], delimiters)) {
        ++currentPos_;
    }

    if (currentPos_ >= chars_.size()) {
        currentPos_ = startPos;
        throw std::out_of_range("No more tokens");
    }

    std::string token;
    while (currentPos_ < chars_.size() && !IsDelimiter(chars_[currentPos_], delimiters)) {
        token.push_back(chars_[currentPos_++]);
    }
    return token;
}

bool SupportClass::Tokenizer::HasMoreTokens()
{
    const std::size_t pos = currentPos_;
    try {
        NextToken();
        currentPos_ = pos;
        return true;
    } catch (const std::out_of_range&) {
        currentPos_ = pos;
        return false;
    }
}

int SupportClass::Tokenizer::Count()
{
    const std::size_t pos = currentPos_;
    int count = 0;
    try {
        while (true) {
            NextToken();
            ++count;
        }
    } catch (const std::out_of_range&) {
        currentPos_ = pos;
        return count;
    }
}

std::string SupportClass::Tokenizer::Current()
{
    return NextToken();
}

bool SupportClass::Tokenizer::MoveNext()
{
    return HasMoreTokens();
}

void SupportClass::Tokenizer::Reset()
{
}

SupportClass::StringIterator::StringIterator()
    : StringIterator("")
{
}

SupportClass::StringIterator::StringIterator(const std::string& text)
    : StringIterator(text, 0)
{
}

SupportClass::StringIterator::StringIterator(const std::string& text, int pos)
    : StringIterator(text, 0, static_cast<int>(text.size()), pos)
{
}

SupportClass::StringIterator::StringIterator(const std::string& text, int begin, int end, int pos)
    : begin_(begin), end_(end), position_(pos), source_(text)
{
    if (begin < 0 || begin > end || end > static_cast<int>(text.size()) || begin > static_cast<int>(text.size())) {
        throw std::invalid_argument("Invalid substring range");
    }
    if (pos < begin || pos > end) {
        throw std::invalid_argument("Invalid position");
    }
}

char SupportClass::StringIterator::Current() const
{
    if (position_ < end_) {
        return source_[position_];
    }
    return DONE;
}

char SupportClass::StringIterator::First()
{
    position_ = begin_;
    return Current();
}

int SupportClass::StringIterator::BeginIndex() const
{
    return begin_;
}

int SupportClass::StringIterator::EndIndex() const
{
    return end_;
}

int SupportClass::StringIterator::GetIndex() const
{
    return position_;
}

char SupportClass::StringIterator::SetIndex(int index)
{
    if (index == end_) {
        position_ = index;
        return DONE;
    }
    if (index < begin_ || index > end_) {
        throw std::invalid_argument("Invalid index");
    }
    position_ = index;
    return source_[position_];
}

char SupportClass::StringIterator::Last()
{
    position_ = end_ - 1;
    return source_[position_];
}

char SupportClass::StringIterator::Next()
{
    ++position_;
    if (position_ < end_) {
        return source_[position_];
    }
    return DONE;
}

char SupportClass::StringIterator::Previous()
{
    if (position_ > begin_) {
        --position_;
        return source_[position_];
    }
    return DONE;
}

std::int64_t SupportClass::NextLong(std::mt19937& random)
{
    const std::int64_t temporaryLong = (static_cast<std::int64_t>(random()) << 32) + random();
    return (random() & 1U) == 0 ? -temporaryLong : temporaryLong;
}

std::vector<std::uint8_t> SupportClass::ToByteArray(const std::vector<std::int8_t>& sbyteArray)
{
    std::vector<std::uint8_t> byteArray;
    byteArray.reserve(sbyteArray.size());
    for (std::int8_t value : sbyteArray) {
        byteArray.push_back(static_cast<std::uint8_t>(value));
    }
    return byteArray;
}

std::vector<std::uint8_t> SupportClass::ToByteArray(const std::string& sourceString)
{
    return std::vector<std::uint8_t>(sourceString.begin(), sourceString.end());
}

std::vector<std::uint8_t> SupportClass::ToByteArray(const std::vector<std::any>& tempObjectArray)
{
    std::vector<std::uint8_t> byteArray;
    byteArray.reserve(tempObjectArray.size());
    for (const auto& value : tempObjectArray) {
        byteArray.push_back(AnyToByte(value));
    }
    return byteArray;
}

std::vector<std::int8_t> SupportClass::ToSByteArray(const std::vector<std::uint8_t>& byteArray)
{
    std::vector<std::int8_t> sbyteArray;
    sbyteArray.reserve(byteArray.size());
    for (std::uint8_t value : byteArray) {
        sbyteArray.push_back(static_cast<std::int8_t>(value));
    }
    return sbyteArray;
}

SupportClass::BackReader::BackReader(std::istream& streamReader, std::size_t size)
    : stream_(streamReader), buffer_(size), position_(size)
{
}

SupportClass::BackReader::BackReader(std::istream& streamReader)
    : BackReader(streamReader, 1)
{
}

bool SupportClass::BackReader::MarkSupported() const
{
    return false;
}

void SupportClass::BackReader::Mark(int)
{
    throw std::ios_base::failure("Mark operations are not allowed");
}

void SupportClass::BackReader::Reset()
{
    throw std::ios_base::failure("Mark operations are not allowed");
}

int SupportClass::BackReader::Read()
{
    if (position_ < buffer_.size()) {
        return static_cast<unsigned char>(buffer_[position_++]);
    }
    return stream_.get();
}

std::streamsize SupportClass::BackReader::Read(char* array, std::streamsize count)
{
    if (count <= 0) {
        return 0;
    }

    std::streamsize readLimit = static_cast<std::streamsize>(buffer_.size() - position_);
    if (readLimit > count) {
        readLimit = count;
    }

    if (readLimit > 0) {
        std::memcpy(array, buffer_.data() + position_, static_cast<std::size_t>(readLimit));
        position_ += static_cast<std::size_t>(readLimit);
    }

    if (readLimit == count) {
        return readLimit;
    }

    stream_.read(array + readLimit, count - readLimit);
    const std::streamsize streamCount = stream_.gcount();
    if (streamCount == 0 && readLimit == 0) {
        return -1;
    }
    return readLimit + streamCount;
}

bool SupportClass::BackReader::IsReady() const
{
    return position_ < buffer_.size() || stream_.peek() != std::char_traits<char>::eof();
}

void SupportClass::BackReader::UnRead(int unReadChar)
{
    if (position_ == 0) {
        throw std::out_of_range("BackReader buffer is full");
    }
    buffer_[--position_] = static_cast<char>(unReadChar);
}

void SupportClass::BackReader::UnRead(const char* array, std::size_t count)
{
    Move(array, count);
}

void SupportClass::BackReader::UnRead(const std::vector<char>& array)
{
    if (!array.empty()) {
        Move(array.data(), array.size());
    }
}

void SupportClass::BackReader::Move(const char* array, std::size_t count)
{
    for (std::size_t index = count; index > 0; --index) {
        UnRead(array[index - 1]);
    }
}

std::uintmax_t SupportClass::FileLength(const std::filesystem::path& file)
{
    return std::filesystem::exists(file) ? std::filesystem::file_size(file) : 0;
}

std::unique_ptr<std::fstream> SupportClass::RandomAccessFileSupport::CreateRandomAccessFile(const std::filesystem::path& fileName, const std::string& mode)
{
    auto stream = std::make_unique<std::fstream>();
    if (mode == "rw") {
        stream->open(fileName, std::ios::in | std::ios::out | std::ios::binary);
        if (!*stream) {
            stream->clear();
            std::fstream creator(fileName, std::ios::out | std::ios::binary);
            creator.close();
            stream->open(fileName, std::ios::in | std::ios::out | std::ios::binary);
        }
    } else if (mode == "r") {
        stream->open(fileName, std::ios::in | std::ios::binary);
    } else {
        throw std::invalid_argument("Unsupported file mode");
    }

    if (!*stream) {
        throw std::ios_base::failure("Unable to open file");
    }
    return stream;
}

void SupportClass::RandomAccessFileSupport::WriteBytes(const std::string& data, std::ostream& fileStream)
{
    fileStream.write(data.data(), static_cast<std::streamsize>(data.size()));
}

void SupportClass::RandomAccessFileSupport::WriteChars(const std::string& data, std::ostream& fileStream)
{
    WriteBytes(data, fileStream);
}

void SupportClass::RandomAccessFileSupport::WriteRandomFile(const std::vector<std::int8_t>& sByteArray, std::ostream& fileStream)
{
    const auto byteArray = SupportClass::ToByteArray(sByteArray);
    fileStream.write(reinterpret_cast<const char*>(byteArray.data()), static_cast<std::streamsize>(byteArray.size()));
}

} // namespace tgreiner::amy
