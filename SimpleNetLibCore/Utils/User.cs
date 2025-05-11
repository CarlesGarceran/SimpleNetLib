using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetLibCore.Utils
{
    public class User
    {
        public string uid;
        public string name;
        public int ping;

        public Object enetPeer;

        public User(string name, Object peer)
        {
            this.name = name;
            this.enetPeer = peer;
        }

        public User()
        {
            
        }
    }
}
