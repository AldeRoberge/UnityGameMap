using System.Collections.Generic;
using UnityEngine;
using World.Map.Tiles;

namespace World.Map.ConnectedTiles
{
    /**
     * A map of connected tiles (paths).
     */
    public class ConnectedTileMap : MonoBehaviour
    {
        public Dictionary<TileLoc, ConnectedTileObject> ConnectedTiles;

        public void Start()
        {
            ConnectedTiles = new Dictionary<TileLoc, ConnectedTileObject>();
        }

        /**
         * Creates a ConnectedTileObject at position loc.
         */
        public ConnectedTileObject CreateConnectedTileObject(TileLoc loc, string objectType = UITileObjectTypes.DEFAULT)
        {
            GameObject tile = GameMap.Instance.CreateTileAt(loc, transform);

            ConnectedTileObject cto = tile.AddComponent<ConnectedTileObject>();
            ConnectedTiles[loc] = cto;
            cto.SetObjectType(objectType);
            cto.tileLoc = loc;
            cto.UpdateConnection();

            return cto;
        }

        /**
         * Returns a ConnectedTileObject for a given tileLoc. Null if not found.
         */
        public ConnectedTileObject GetConnectedTileAt(TileLoc tileLoc)
        {
            return !ConnectedTiles.ContainsKey(tileLoc) ? null : ConnectedTiles[tileLoc];
        }

        /**
         * Removes a ConnectedTileObject for a given tileLoc.
         */
        public void RemoveConnectedTileAt(TileLoc tileLoc)
        {
            if (ConnectedTiles.ContainsKey(tileLoc))
            {
                Destroy(ConnectedTiles[tileLoc].gameObject);
                ConnectedTiles.Remove(tileLoc);
            }
            else
            {
                Debug.Log("No tileLoc at pos.");
            }
        }
    }
}