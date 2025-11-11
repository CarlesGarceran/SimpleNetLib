using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetLibCore.Encryption
{
    public interface IPacketEncrypter
    {
        public byte[] EncryptBuffer(byte[] data);
        public byte[] DecryptBuffer(byte[] data);

        public string EncryptString(string data);
        public string DecryptString(string data);
    }
}
