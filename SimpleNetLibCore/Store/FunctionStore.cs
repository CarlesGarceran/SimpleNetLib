using SimpleNetLibCore.Packet.Generic;
using SimpleNetLibCore.Store.Attributes;
using SimpleNetLibCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimpleNetLibCore.Store
{
    public sealed class MethodInstance
    {
        public MethodInfo info;
        public object instance;

        public MethodInstance(object instance, MethodInfo info)
        {
            this.info = info;
            this.instance = instance;
        }
    }

    public class FunctionStore
    {
        private Dictionary<string, MethodInstance> serverMethods;
        private Dictionary<string, MethodInstance> clientMethods;
        private Dictionary<Type, MethodInstance> packetReceivedAttributes;

        public FunctionStore() 
        {
            Instance = this;

            serverMethods = new Dictionary<string, MethodInstance>();
            clientMethods = new Dictionary<string, MethodInstance>();
            packetReceivedAttributes = new Dictionary<Type, MethodInstance>();
        }

        public void AddServerFunction(string functionName, MethodInstance methodInst)
        {
            if (methodInst.info.GetCustomAttribute<ClientFunctionAttribute>() != null)
                throw new Exception("Attempt to register ClientFunction as a ServerFunction");

            if (methodInst.info.GetCustomAttribute<ServerFunctionAttribute>() == null)
                throw new Exception("Attempt to register a function without the ServerFunction attribute");

            serverMethods.Add(functionName, methodInst);
        }

        public void AddClientFunction(string functionName, MethodInstance methodInst)
        {
            if (methodInst.info.GetCustomAttribute<ServerFunctionAttribute>() != null)
                throw new Exception("Attempt to register ServerFunction as a ClientFunction");

            if (methodInst.info.GetCustomAttribute<ClientFunctionAttribute>() == null)
                throw new Exception("Attempt to register a function without the ServerFunction attribute");

            clientMethods.Add(functionName, methodInst);
        }

        public void AddPacketReceivedFunction(Type type, MethodInstance methodInst)
        {
            if (methodInst.info.GetCustomAttribute<PacketReceivedAttribute>() != null)
                throw new Exception("Attempt to register ServerFunction as a ClientFunction");

            packetReceivedAttributes.Add(type, methodInst);
        }

        public MethodInstance? GetServerFunction(string functionName)
        {
            if (serverMethods.ContainsKey(functionName))
                return serverMethods[functionName];
            else
                return null;
        }

        public MethodInstance? GetClientFunction(string functionName)
        {
            if (clientMethods.ContainsKey(functionName))
                return clientMethods[functionName];
            else
                return null;
        }

        public MethodInstance? GetFunctionFromPacket(Type packet)
        {
            if (packetReceivedAttributes.ContainsKey(packet))
                return packetReceivedAttributes[packet];
            else
                return null;
        }

        public static FunctionStore? Instance { get; private set; }

        public static void RegisterFunctions(INetworkObject Instance)
        {
            string instanceId = Instance.ObjectID;

            Type t = Instance.GetType();

            MethodInfo[] methodInfos = t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (MethodInfo methodInfo in methodInfos)
            {
                ServerFunctionAttribute? serverFuncAttrib = null;
                ClientFunctionAttribute? clientFuncAttrib = null;
                PacketReceivedAttribute? packetReceivedAttrib = null;
                if ((serverFuncAttrib = methodInfo.GetCustomAttribute<ServerFunctionAttribute>()) != null)
                {
                    FunctionStore.Instance?.AddServerFunction(instanceId + "_" + serverFuncAttrib.functionName, new MethodInstance(Instance, methodInfo));
                }

                if ((clientFuncAttrib = methodInfo.GetCustomAttribute<ClientFunctionAttribute>()) != null)
                {
                    FunctionStore.Instance?.AddClientFunction(instanceId + "_" + clientFuncAttrib.functionName, new MethodInstance(Instance, methodInfo));
                }

                if ((packetReceivedAttrib = methodInfo.GetCustomAttribute<PacketReceivedAttribute>()) != null)
                {
                    FunctionStore.Instance?.AddPacketReceivedFunction(packetReceivedAttrib.packetType, new MethodInstance(Instance, methodInfo));
                }
            }
        }
    }
}
