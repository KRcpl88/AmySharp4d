#pragma once

#include <string>

namespace tgreiner::amy::common::engine
{
    class IComm
    {
    public:
        virtual ~IComm() = default;

        virtual void OnResponse(const std::string& response) = 0;
    };
}
