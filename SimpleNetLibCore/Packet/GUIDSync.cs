using SimpleNetLibCore.Abstractions;
using SimpleNetLibCore.Packet.Generic;
using SimpleNetLibCore.Interfaces;
using SimpleNetLibCore.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleNetLibCore.Utils;

namespace SimpleNetLibCore.Packet
{
    public class GUIDSync : NetworkPacket
    {
        public string tempUid = "";
        public string objectUid = "";
        
        public GUIDSync(string objectUid)
            : base(User.Instance)
        {
            this.tempUid = objectUid;
        }

        public override void ClientExecute(Object e)
        {
            NetworkStore.Instance.GetObjectInstance<INetworkObject>(tempUid).SetObjectID(objectUid,
                new Authority.NetworkAuthority(
                    ANetworkHandler.Instance,
                    objectUid
                )
            );
            NetworkStore.Instance.UpdateObjectInstance(tempUid, objectUid);
        }

        public override void Execute(Object e)
        {

        }

        public override void ServerExecute(Object e)
        {
            objectUid = Guid.NewGuid().ToString();
            NetworkStore.Instance.GetObjectInstance<INetworkObject>(tempUid).SetObjectID(objectUid, 
                new Authority.NetworkAuthority(
                    ANetworkHandler.Instance,
                    objectUid
                )
            );
            NetworkStore.Instance.UpdateObjectInstance(tempUid, objectUid);
        }
    }
}
