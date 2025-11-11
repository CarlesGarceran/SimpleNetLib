using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetLibCore.Encryption
{
    public class NoEncryption : IPacketEncrypter
    {
        public byte[] EncryptBuffer(byte[] data)
        {
            return data;
        }

        public byte[] DecryptBuffer(byte[] data)
        {
            return data;
        }

        public string DecryptString(string data)
        {
            return data;
        }

        public string EncryptString(string data)
        {
            return data;
        }
    }
}
