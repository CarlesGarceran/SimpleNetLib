using Newtonsoft.Json;
using SimpleNetLibCore.Store;
using SimpleNetLibCore.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetLibCore.Packet
{
    public class FunctionPacket : Packet.Generic.NetworkPacket
    {
        public string methodName;
        public ReflectableObject[] args;

        [JsonConstructor]
        public FunctionPacket()
        {
            
        }

        public FunctionPacket(string methodName, params object[] args)
        {
            this.methodName = methodName;
            List<ReflectableObject> convertedArgs = new List<ReflectableObject>();

            foreach (object o in args)
            {
                var reflObj = new ReflectableObject();
                reflObj.Serialize(o);
                convertedArgs.Add(reflObj);
            }

            this.args = convertedArgs.ToArray();
        }

        public override void ClientExecute(Object ev)
        {
            MethodInstance methodInst = FunctionStore.Instance.GetClientFunction(methodName);
            object instance = methodInst.instance;
            MethodInfo methodInfo = methodInst.info;

            object[] deserializedArgs = new object[args.Length];
            for (int x = 0; x < args.Length; x++)
            {
                args[x].Deserialize();
                deserializedArgs[x] = args[x].Get();
            }

            methodInfo.Invoke(instance, deserializedArgs);
        }

        public override void Execute(Object ev)
        {

        }

        public override void ServerExecute(Object ev)
        {
            MethodInstance methodInst = FunctionStore.Instance.GetServerFunction(methodName);
            object instance = methodInst.instance;
            MethodInfo methodInfo = methodInst.info;

            object[] deserializedArgs = new object[args.Length];
            for(int x = 0; x < args.Length; x++)
            {
                args[x].Deserialize();
                deserializedArgs[x] = args[x].Get();
            }

            methodInfo.Invoke(instance, deserializedArgs);
        }
    }
}
