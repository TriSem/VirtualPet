using System;
using System.Collections.Generic;
using UnityEngine;

public sealed partial class WorldObject : MonoBehaviour
{
    /// <summary>
    /// Try to retrieve the object with the specified id.
    /// </summary>
    /// <param name="instanceId"></param>
    /// <returns>WorldObject reference or null, if it was not registered.</returns>
    public static WorldObject FindObject(ulong instanceId) => WorldDatabase.Get.FindObject(instanceId);

    sealed class WorldDatabase
    {
        Dictionary<ulong, WorldObject> worldObjects = new Dictionary<ulong, WorldObject>();
        static ulong count = 0;

        static readonly Lazy<WorldDatabase> lazy = new Lazy<WorldDatabase>(() => new WorldDatabase());
        public static WorldDatabase Get => lazy.Value;

        public WorldObject FindObject(ulong instanceId)
        {
            try
            {
                return worldObjects[instanceId];
            }
            catch
            {
                Debug.LogError("Id not found. Has the object been deleted?");
                return null;
            }
        }

        private WorldDatabase() { }

        public ulong AddWithKey(WorldObject worldObject)
        {
            worldObjects.Add(count, worldObject);
            return count++;
        }
    }
}
