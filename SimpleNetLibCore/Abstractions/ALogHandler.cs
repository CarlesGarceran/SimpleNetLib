using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetLibCore.Abstractions
{
    /// <summary>
    /// Abstract network handler for piping into GameEngines.
    /// 
    /// </summary>
    public abstract class ALogHandler
    {
        public static ALogHandler? Instance { get; private set; }
        protected List<string> messages;
        protected List<string> info;
        protected List<string> warnings;
        protected List<string> errors;

        protected ALogHandler()
        {
            Instance = this;
            messages = new List<string>();
            info = new List<string>();
            warnings = new List<string>();
            errors = new List<string>();
        }

        public abstract void Log(string message);
        public abstract void LogInfo(string message);
        public abstract void LogWarning(string message);
        public abstract void LogError(string message);


        public List<string> GetMessages()
        {
            return messages;
        }

        public List<string> GetInfo()
        {
            return info;
        }

        public List<string> GetWarnings()
        {
            return warnings;
        }

        public List<string> GetErrors()
        {
            return errors;
        }
    }
}
