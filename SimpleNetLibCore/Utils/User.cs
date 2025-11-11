using Newtonsoft.Json;
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
        public string uid { get; private set; } = "UNDEFINED";
        public string name { get; private set; }
        public int ping { get; private set; }

        [JsonIgnore] public object socket { get; private set; }

        public User(string name)
        {
            Instance = this;
            this.name = name;
        }

        public void SetSocket(object socket) => this.socket = socket;
        public void SetPing(int ping) => this.ping = ping;
        public void SetUID(string uid) => this.uid = uid;

        public static User? Instance;
    }
}
