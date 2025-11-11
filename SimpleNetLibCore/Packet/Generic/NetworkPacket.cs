using SimpleNetLibCore.Reflection;
using SimpleNetLibCore.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JSON = Newtonsoft.Json.JsonConvert;

namespace SimpleNetLibCore.Packet.Generic
{
    public abstract partial class NetworkPacket
    {
        private MemoryStream memoryStream;
        public Buffer<byte> memoryBuffer;
        public ReflectableObject packetOwner;

        public NetworkPacket(User owner)
        {
            memoryBuffer = new Buffer<byte>();
            memoryStream = new MemoryStream(memoryBuffer.data);
            packetOwner = new ReflectableObject();
            packetOwner.Serialize(owner);
        }

        public string Serialize()
        {
            return JSON.SerializeObject(this);
        }

        public abstract void Execute(Object e);

        public abstract void ServerExecute(Object e);
        public abstract void ClientExecute(Object e);

        public NetworkWriter GetNetworkWriter() { return new NetworkWriter(memoryStream); }

        public override string ToString()
        {
            return Serialize();
        }
    }
}
