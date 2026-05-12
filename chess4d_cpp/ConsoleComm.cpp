#include "ConsoleComm.h"

#include <iostream>

namespace AmySharp {

void ConsoleComm::OnResponse(const std::string& response)
{
    std::cout << response << std::endl;
}

} // namespace AmySharp
