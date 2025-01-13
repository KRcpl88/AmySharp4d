using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmySharp.chess.engine.log4net
{
    interface ILog
    {
        void Error(string msg);
        void Debug(string msg);
        void Fatal(string msg);
        void Info(string msg);

        bool IsDebugEnabled { get; }
    }

    class Logger : ILog
    {
        public bool IsDebugEnabled {get { return false; } }

        void ILog.Error(string msg)
        {
           
        }

        void ILog.Debug(string msg)
        {

        }
        void ILog.Fatal(string msg)
        {

        }
        void ILog.Info(string msg)
        {

        }

    }

    class LogManager
    {
        static ILog logger = new Logger();
        public static ILog GetLogger()
        {
            return logger;

        }

        public static ILog GetLogger(System.Type nonesuch)
        {
            return GetLogger();

        }
    }


}
