using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetLibCore.Utils
{
    public class NetworkWriter : BinaryWriter
    {
        public NetworkWriter(Stream output, Encoding encoding) : base(output, encoding) { }
        public NetworkWriter(Stream output) : base(output, Encoding.UTF8) { }

        public void Write(Object obj)
        {
            base.Write(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
        }
    }
}
