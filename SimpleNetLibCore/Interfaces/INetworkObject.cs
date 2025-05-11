using SimpleNetLibCore.Abstractions;
using SimpleNetLibCore.Authority;
using SimpleNetLibCore.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetLibCore.Authority
{
    public class NetworkAuthority
    {
        public string AuthKey;
        public object Instance;

        public NetworkAuthority(object instance, string Key)
        {
            this.Instance = instance;
            this.AuthKey = Key;

            if (ANetworkHandler.Instance == null)
                return;

            ANetworkHandler.Instance.PollNetworkAuthorityCertificate(this);
        }
    }
}

namespace SimpleNetLibCore.Interfaces
{
    public interface INetworkObject
    {
        public string ObjectID { get; set; }
        public string OwnerID { get; set; }

        public void SetObjectID(string id, NetworkAuthority authoritativeRequest);

        public bool IsMine();
        public bool isMine();

        public bool IsServer();
        public bool isServer();
        public bool IsClient();
        public bool isClient();
    }
}
