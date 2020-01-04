using System.Collections.Generic;
using Map.Objects.Tiles;
using Map.Objects.Tiles.ConnectedTiles;
using UnityEngine;

namespace Map
{
    public class ConnectedTileMap : MonoBehaviour
    {
        public Dictionary<TileLoc, ConnectedTileObject> ConnectedTiles;
        
        public void Start()
        {
            this.transform.position += new Vector3(0f, 0.05f,0f);
            ConnectedTiles = new Dictionary<TileLoc, ConnectedTileObject>();
        }

        public ConnectedTileObject CreateConnectedTileObject(TileLoc loc, int objectType = -1)
        {
            GameObject tile = GameMap.Instance.CreateTileAt(loc, transform);

            ConnectedTileObject cto = tile.AddComponent<ConnectedTileObject>();
            ConnectedTiles[loc] = cto;
            cto.objectType = objectType;
            cto.tileLoc = loc;
            cto.UpdateConnection();

            return cto;
        }


        public ConnectedTileObject GetConnectedObjectAt(TileLoc tileLoc)
        {
            return !ConnectedTiles.ContainsKey(tileLoc) ? null : ConnectedTiles[tileLoc];
        }
        
        
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