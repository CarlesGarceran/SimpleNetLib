using Newtonsoft.Json;
using SimpleNetLibCore.Packet.Generic;
using SimpleNetLibCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetLibCore.Packet.LowLevel
{
    public class PacketWrap
    {
        public string packetPayload;
        public Type packetType;

        public PacketWrap(NetworkPacket p)
        {
            packetPayload = p.Serialize();
            packetType = p.GetType();
        }

        [JsonConstructor]
        public PacketWrap()
        {
            packetPayload = "";
            packetType = GetType();
        }

        public byte[] Serialize()
        {
            byte[] jsonBytes = Library.Settings.PacketEncrypter.EncryptBuffer(Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(this)));
            return Compressor.Compress(jsonBytes);
        }

        public NetworkPacket? Deserialize()
        {
            return (NetworkPacket?)JsonConvert.DeserializeObject(packetPayload, packetType);
        }

        public T? Deserialize<T>()
        {
            return (T?)JsonConvert.DeserializeObject(packetPayload, packetType);
        }
    }
}
