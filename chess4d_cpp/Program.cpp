#include <algorithm>
#include <cctype>
#include <iostream>
#include <string>

#include "ConsoleComm.h"
#include "chess/engine/XBoardEngine.h"

namespace {
std::string ToUpper(std::string value)
{
    std::transform(value.begin(), value.end(), value.begin(), [](unsigned char ch) {
        return static_cast<char>(std::toupper(ch));
    });
    return value;
}
}

int main()
{
    AmySharp::ConsoleComm comm;
    tgreiner::amy::chess::engine::XBoardEngine engine(comm);

    std::string cmd;
    while (std::getline(std::cin, cmd)) {
        if (ToUpper(cmd) == "QUIT") {
            break;
        }
        engine.Process(cmd);
    }

    engine.Process("quit");
    return 0;
}
