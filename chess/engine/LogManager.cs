using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AmySharp.chess.engine.logger
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
        StreamWriter logfile = null;

        void ILog.Error(string msg)
        {
            if (logfile != null)
            {
                logfile.WriteLine($"ERROR: {msg}");
            }
        }

        void ILog.Debug(string msg)
        {
            if (logfile != null)
            {
                logfile.WriteLine($"DEBUG: {msg}");
            }
        }
        void ILog.Fatal(string msg)
        {
            if (logfile != null)
            {
                logfile.WriteLine($"FATAL: {msg}");
            }
        }
        void ILog.Info(string msg)
        {
            if (logfile != null)
            {
                logfile.WriteLine($"INFO: {msg}");
            }
        }

        public void SetLogFile(string filename)
        {
            if (logfile != null)
            {
                logfile.Close();
                logfile = null;
            }

            logfile = new StreamWriter(filename);
        }

        public void Close()
        {
            if (logfile != null)
            {
                logfile.Close();
                logfile = null;
            }
        }
    }

    class LogManager
    {
        static Logger logger = new Logger();
        public static ILog GetLogger()
        {
            return logger;

        }

        public static ILog GetLogger(System.Type nonesuch)
        {
            return GetLogger();

        }

        public static void SetLogFile(string filename)
        {
            logger.SetLogFile(filename);
        }

        public static void Close()
        {
            logger.Close();
        }
    }


}
