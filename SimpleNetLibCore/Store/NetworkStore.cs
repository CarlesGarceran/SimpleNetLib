using SimpleNetLibCore.Store;
using SimpleNetLibCore.Abstractions;
using SimpleNetLibCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimpleNetLibCore.Store
{
    public class NetworkStore
    {
        public static NetworkStore Instance { get; private set; }

        private Dictionary<string, INetworkObject> NetworkInstances; // (ObjectID, ObjectInstance)

        public NetworkStore()
        {
            Instance = this;
            this.NetworkInstances = new Dictionary<string, INetworkObject>();
        }

        public bool UpdateObjectInstance(string objectID, string newObjectId)
        {
            if (!NetworkInstances.ContainsKey(objectID))
            {
                ALogHandler.Instance?.LogError("Cannot Add NetworkInstance (Reason: Missing NetworkID)");
                return false;
            }

            INetworkObject tmp = NetworkInstances[objectID];
            NetworkInstances.Remove(objectID);
            NetworkInstances.Add(newObjectId, tmp);

            return true;
        }

        public bool AddObjectInstance(string objectID, INetworkObject instance)
        {
            if (NetworkInstances.ContainsKey(objectID))
            {
                ALogHandler.Instance?.LogError("Cannot Add NetworkInstance (Reason: Duplicate NetworkID)");
                return false;
            }

            NetworkInstances.Add(objectID, instance);
            return true;
        }

        public bool RemoveObjectInstance(string objectID)
        {
            if (!NetworkInstances.ContainsKey(objectID))
            {
                ALogHandler.Instance?.LogError("Cannot Remove NetworkInstance (Reason: NetworkID not found)");
                return false;
            }

            NetworkInstances.Remove(objectID);
            return true;
        }

        public INetworkObject? GetObjectInstance(string objectID)
        {
            if (!NetworkInstances.ContainsKey(objectID))
            {
                ALogHandler.Instance?.LogError("Cannot Get NetworkInstance (Reason: NetworkID not found)");
                return null;
            }

            return NetworkInstances[objectID];
        }

        public T? GetObjectInstance<T>(string objectID) where T : class 
        {
            if (!NetworkInstances.ContainsKey(objectID))
            {
                ALogHandler.Instance?.LogError("Cannot Get NetworkInstance (Reason: NetworkID not found)");
                return null;
            }

            return NetworkInstances[objectID] as T;
        }
    }
}
