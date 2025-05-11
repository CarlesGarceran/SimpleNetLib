using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetLibCore.Utils
{
    public class Compressor
    {
        public static byte[] Compress(byte[] jsonBytes)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (DeflateStream deflater = new DeflateStream(stream, CompressionLevel.SmallestSize))
                {
                    deflater.Write(jsonBytes, 0, jsonBytes.Length);
                }

                return stream.ToArray();
            }
        }

        public static T Decompress<T>(byte[] compressedData)
        {
            using (var inputStream = new MemoryStream(compressedData))
            using (var deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress))
            using (var outputStream = new MemoryStream())
            {
                deflateStream.CopyTo(outputStream);
                string json = Encoding.UTF8.GetString(outputStream.ToArray());
                return JsonConvert.DeserializeObject<T>(json);
            }
        }
    }
}
