using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JSON = Newtonsoft.Json.JsonConvert;

namespace SimpleNetLibCore.Reflection
{
    public class Reflected
    {
        public string instance = "";
        public string typeRef = "";
    }

    public class ReflectableObject
    {
        public Reflected serialized;

        public Type? objectType;
        private object? instance;

        public ReflectableObject()
        {
            serialized = new Reflected();
            instance = null;
            objectType = null;
        }

        public void Serialize(object inst)
        {
            instance = inst;
            objectType = inst.GetType();
            serialized.instance = Library.Settings.PacketEncrypter.EncryptString(JSON.SerializeObject(instance));
            serialized.typeRef = objectType.FullName;
        }

        public void Deserialize()
        {
            instance = JSON.DeserializeObject(Library.Settings.PacketEncrypter.DecryptString(serialized.instance), objectType);
        }

        public void DeserializeAs<T>()
        {
            instance = JSON.DeserializeObject(Library.Settings.PacketEncrypter.DecryptString(serialized.instance), typeof(T));
        }

        public object? Get()
        {
            return instance;
        }

        public T GetAs<T>()
        {
            return (T)instance;
        }
    }
}
