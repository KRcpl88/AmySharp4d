#pragma once

#include <exception>
#include <string>
#include <utility>

namespace tgreiner::amy::common::timer
{
    class TimeOutException : public std::exception
    {
    public:
        TimeOutException()
            : message_("Time allotted to the operation has expired.")
        {
        }

        explicit TimeOutException(std::string message)
            : message_(std::move(message))
        {
        }

        const char* what() const noexcept override
        {
            return message_.c_str();
        }

    private:
        std::string message_;
    };
}
