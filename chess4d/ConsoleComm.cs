using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tgreiner.amy.common.engine;
namespace AmySharp
{
    class ConsoleComm : IComm
    {
        public ConsoleComm()
        {
        }

        public void OnResponse(string response)
        {
            Console.WriteLine(response);
        }
    }
}
