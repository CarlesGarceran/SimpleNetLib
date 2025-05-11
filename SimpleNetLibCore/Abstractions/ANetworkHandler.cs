using SimpleNetLibCore.Exceptions;
using SimpleNetLibCore.Packet;
using SimpleNetLibCore.Packet.Generic;
using SimpleNetLibCore.Packet.LowLevel;
using SimpleNetLibCore.Store;
using SimpleNetLibCore.Store.Attributes;
using SimpleNetLibCore.Utils;
using SimpleNetLibCore.Abstractions;
using SimpleNetLibCore.Authority;
using SimpleNetLibCore.Interfaces;
using SimpleNetLibCore.Store;
using System;
using System.Collections.Generic;
using System.Reflection;
using SimpleNetLibCore.GenericAPI.Flags;

namespace SimpleNetLibCore.Abstractions
{
    public abstract class ANetworkHandler
    {
        public bool IsServer { get; protected set; }
        public bool IsClient { get; protected set; }
        public byte CurrentChannel { get; protected set; } = 0;

        public int Timeout = 15;

        public static ANetworkHandler? Instance { get; protected set; }

        // GENERATE PASSKEYS FOR AUTHORIZATION
        private List<NetworkAuthority> networkAuthorities = new List<NetworkAuthority>();
        protected ANetworkHandler() => Instance = this;

        public bool CheckNetworkAuthorityCertificate(NetworkAuthority request)
        {
            return networkAuthorities.Find(
                networkAuthorities => networkAuthorities.AuthKey == request.AuthKey &&
                networkAuthorities.Instance == request.Instance
            ) != null;
        }

        public void PollNetworkAuthorityCertificate(NetworkAuthority request)
        {
            networkAuthorities.Add(request);
        }

        public virtual void SendToUser(NetworkPacket packet, User user, PacketFlags packetFlags = PacketFlags.None, byte channelId = 0) { }
        public virtual void SendToUser(NetworkPacket packet, Object peer, PacketFlags packetFlags = PacketFlags.None, byte channelId = 0) { }
        public virtual void SendToServer(NetworkPacket packet, PacketFlags packetFlags, byte channelID = 0) { }

        public void Broadcast(NetworkPacket packet, PacketFlags packetFlags = PacketFlags.None, byte channelId = 0)
        {
            /*
            SimpleNetLibCore.Wrapper.Packet p = new SimpleNetLibCore.Wrapper.Packet();
            byte[] buffer = new PacketWrap(packet).Serialize();
            p.Create(buffer, 0, buffer.Length, packetFlags);

            Broadcast(channelId, p);
            */

            ManualBroadcast(packet, channelId, packetFlags);
        }

        public void ServerRPC(INetworkObject networkObject, Delegate functionName, byte channelId, PacketFlags packetFlags, params object[] args)
        {
            if (functionName == null)
                return;

            MethodInfo methodInfo = functionName.GetMethodInfo();
            if (methodInfo == null) return;

            ServerFunctionAttribute? funcAttrib;

            if ((funcAttrib = methodInfo.GetCustomAttribute<ServerFunctionAttribute>()) != null)
            {
                string funcName = funcAttrib.functionName;

                ServerRPC(networkObject, funcName, channelId, packetFlags, args);
            }
            else
            {
                ALogHandler.Instance?.LogError("Function does not has ServerFunction Attribute");
                return;
            }
        }

        public void ClientRPC(INetworkObject networkObject, Delegate functionName, byte channelId, PacketFlags flags, params object[] args)
        {
            if (functionName == null)
                return;

            MethodInfo methodInfo = functionName.GetMethodInfo();

            if (methodInfo == null) return;

            ClientFunctionAttribute? funcAttrib;

            if ((funcAttrib = methodInfo.GetCustomAttribute<ClientFunctionAttribute>()) != null)
            {
                string funcName = funcAttrib.functionName;

                ClientRPC(networkObject, funcName, channelId, flags, args);
            }
            else
            {
                ALogHandler.Instance?.LogError("Function does not has ClientFunction Attribute");
                return;
            }
        }

        public void TargetRPC(INetworkObject networkObject, User target, Delegate functionName, byte channelId, PacketFlags packetFlags, params object[] args)
        {
            if (functionName == null)
                return;

            MethodInfo methodInfo = functionName.GetMethodInfo();

            if (methodInfo == null) return;

            ServerFunctionAttribute? funcAttrib;

            if ((funcAttrib = methodInfo.GetCustomAttribute<ServerFunctionAttribute>()) != null)
            {
                string funcName = funcAttrib.functionName;

                TargetRPC(networkObject, target, funcName, channelId, packetFlags, args);
            }
            else
            {
                ALogHandler.Instance?.LogError("Function does not has ServerFunction Attribute");
                return;
            }
        }

        public void ServerRPC(INetworkObject networkObject, string functionName, byte channelId, PacketFlags flags, params object[] args)
        {
            INetworkObject? owner = NetworkStore.Instance.GetObjectInstance(networkObject.ObjectID);

            if (owner == null)
                return;

            MethodInstance instance = FunctionStore.Instance.GetServerFunction(owner.ObjectID + "_" + functionName);

            MethodInfo mInfo = instance.info;

            if(IsServer)
            {
                mInfo.Invoke(owner, args);
                return;
            }

            FunctionPacket packet = new FunctionPacket(owner.ObjectID + "_" + functionName, args);

            this.SendToServer(packet, flags, channelId);
        }

        public void ClientRPC(INetworkObject networkObject, string functionName, byte channelId, PacketFlags flags, params object[] args)
        {
            INetworkObject? owner = NetworkStore.Instance.GetObjectInstance(networkObject.ObjectID);

            if (owner == null)
                return;

            MethodInstance instance = FunctionStore.Instance.GetClientFunction(owner.ObjectID + "_" + functionName);

            MethodInfo mInfo = instance.info;

            if(IsClient)
            {
                mInfo.Invoke(owner, args);
                return;
            }

            FunctionPacket packet = new FunctionPacket(owner.ObjectID + "_" + functionName, args);

            this.Broadcast(packet, flags, channelId);
        }

        public void TargetRPC(INetworkObject networkObject, User target, string functionName, byte channelId, PacketFlags flags, params object[] args)
        {
            INetworkObject? owner = NetworkStore.Instance.GetObjectInstance(networkObject.ObjectID);

            if (owner == null)
                return;

            MethodInstance instance = FunctionStore.Instance.GetClientFunction(owner.ObjectID + "_" + functionName);

            MethodInfo mInfo = instance.info;

            if (IsClient)
            {
                mInfo.Invoke(owner, args);
                return;
            }

            FunctionPacket packet = new FunctionPacket(owner.ObjectID + "_" + functionName, args);

            this.SendToUser(packet, target, flags, channelId);
        }


        protected void ClientExecute(NetworkPacket p, Object @event)
        {
            ProcessPacket(p, @event);
            p.ClientExecute(@event);
        }

        protected void ServerExecute(NetworkPacket p, Object @event)
        {
            ProcessPacket(p, @event);
            p.ServerExecute(@event);
        }

        public abstract void Disconnect();

        public abstract void Listen();

        protected abstract void ProcessNetworkEvent(Object @event);
        protected abstract void OnConnected(Object @event);
        protected virtual void ProcessDisconnectionPacket(Object @e) => throw new NonImplementedMethodException();
        protected virtual void ProcessTimeoutPacket(Object @e) => throw new NonImplementedMethodException();

        protected abstract void ManualBroadcast(NetworkPacket packet, byte channelId, PacketFlags packetFlags);
        protected abstract void Broadcast(byte channelId, Object packet);

        private void ProcessPacket(NetworkPacket p, Object @event)
        {
            p.Execute(@event);

            MethodInstance? methodInstance = null;
            if ((methodInstance = FunctionStore.Instance.GetFunctionFromPacket(p.GetType())) != null)
            {
                methodInstance.info.Invoke(methodInstance.instance, new object[] { p });
            }
        }
    }
}
