using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tgreiner.amy.chess.engine;
namespace AmySharp
{
    class Program
    {
        static void Main(string[] args)
        {
            XBoardEngine e = new XBoardEngine(new ConsoleComm());
            string cmd = Console.ReadLine();

            while (!cmd.ToUpper().Equals("QUIT"))
            {
                e.Process(cmd);
                cmd = Console.ReadLine();
            }

            // Send quit
            e.Process("quit");
        }
    }
}
