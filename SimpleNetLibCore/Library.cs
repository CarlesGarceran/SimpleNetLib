using SimpleNetLibCore.Local;
using SimpleNetLibCore.Store;
using SimpleNetLibCore.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetLibCore
{
    public static class Library
    {
        public static bool IsInitialized { get; private set; } = false;
        
        public static void Initialize()
        {
            // Library Initialization
            new FunctionStore();
            new NetworkStore();
            IsInitialized = true;
        }

        public static void CreateLocalUser(string name)
        {
            new LocalUser(name);
        }
    }
}
