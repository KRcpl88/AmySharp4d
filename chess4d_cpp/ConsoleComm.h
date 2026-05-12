#pragma once

#include <string>

#include "common/engine/IComm.h"

namespace AmySharp {

class ConsoleComm : public tgreiner::amy::common::engine::IComm {
public:
    ConsoleComm() = default;
    ~ConsoleComm() override = default;

    void OnResponse(const std::string& response) override;
};

} // namespace AmySharp
