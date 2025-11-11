using SimpleNetLibCore.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetLibCore
{
    /// <summary>
    /// SimpleNetLib Library Configuration
    /// </summary>
    public class Settings
    {
        public IPacketEncrypter PacketEncrypter = new NoEncryption();
    }
}
