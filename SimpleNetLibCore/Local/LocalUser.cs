using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetLibCore.Local
{
    public class LocalUser
    {
        public readonly string uid = Guid.NewGuid().ToString();
        public string userName;
        public Object peer;

        public LocalUser(string name)
        {
            Instance = this;
            this.userName = name;
        }


        public static LocalUser? Instance;
    }
}
