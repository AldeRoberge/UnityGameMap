using UnityEngine;
using World.Tiles;

namespace World.ConnectedTiles
{
    /**
     * A map of connected tiles (paths).
     */
    public class ConnectedGenericTileMap : GenericTileMap<ConnectedTileObject>
    {
        /**
         * Creates a ConnectedTileObject at position loc.
         */
        public ConnectedTileObject CreateConnectedTileObject(TileLoc loc, string objectType = UITileObjectTypes.DEFAULT)
        {
            GameObject tile = GameMap.Instance.CreateTileAt(loc, transform);

            ConnectedTileObject cto = tile.AddComponent<ConnectedTileObject>();
            Tiles[loc] = cto;
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
            return !Tiles.ContainsKey(tileLoc) ? null : Tiles[tileLoc];
        }

        /**
         * Removes a ConnectedTileObject for a given tileLoc.
         */
        public void RemoveConnectedTileAt(TileLoc tileLoc)
        {
            if (Tiles.ContainsKey(tileLoc))
            {
                Destroy(Tiles[tileLoc].gameObject);
                Tiles.Remove(tileLoc);
            }
            else
            {
                Debug.Log("No tileLoc at pos.");
            }
        }
    }
}