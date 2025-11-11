using SimpleNetLibCore.Store;
using SimpleNetLibCore.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleNetLibCore.Encryption;
using SimpleNetLibCore.Utils;

namespace SimpleNetLibCore
{
    /// <summary>
    /// SimpleNetLib Library Startup
    /// </summary>
    public static class Library
    {
        public static Settings Settings = new Settings();

        public static bool IsInitialized { get; private set; } = false;
        
        /// <summary>
        /// Initializes the Library
        /// </summary>
        public static void Initialize()
        {
            // Library Initialization
            new FunctionStore();
            new NetworkStore();
            IsInitialized = true;
        }

        /// <summary>
        /// Creates the game user, which later will get an identifier by the server
        /// </summary>
        /// <param name="name"></param>
        public static void CreateUser(string name)
        {
            new User(name);
        }
    }
}
