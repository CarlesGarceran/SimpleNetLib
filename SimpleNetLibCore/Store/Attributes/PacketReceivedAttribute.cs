using SimpleNetLibCore.Packet.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetLibCore.Store.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PacketReceivedAttribute : Attribute
    {
        public Type packetType;

        public PacketReceivedAttribute(Type packet)
        {
            if (!typeof(NetworkPacket).IsAssignableTo(packet))
                throw new ArgumentException($"The type must be a subclass of {nameof(NetworkPacket)}.", nameof(packet));
        
            packetType = packet;
        }
    }
}
