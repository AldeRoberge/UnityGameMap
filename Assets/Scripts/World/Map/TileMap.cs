using System.Collections.Generic;
using Map.Objects.Tiles;
using Map.Objects.Tiles.ConnectedTiles;
using UnityEngine;
using World.Objects.Tiles.ConnectedTiles;

namespace Map
{
    /**
     * A math of tiles (ground).
     */
    public class TileMap : MonoBehaviour
    {
        public Dictionary<TileLoc, TileObject> Tiles;

        public void Start()
        {
            Debug.Log("Hey");
            Tiles = new Dictionary<TileLoc, TileObject>();

            for (int x = 0; x < GameMap.SquaredMapSize; x++)
            {
                for (int y = 0; y < GameMap.SquaredMapSize; y++)
                {
                    CreateTileObject(new TileLoc(x, y), UITileObjectTypes.DEFAULT);
                }
            }
        }

        public TileObject CreateTileObject(TileLoc loc, string type = UITileObjectTypes.DEFAULT)
        {
            GameObject tile = GameMap.Instance.CreateTileAt(loc, transform);

            TileObject c = tile.AddComponent<TileObject>();
                
            c.SetObjectType(type);
            c.tileLoc = loc;
            Tiles.Add(loc, c);

            return c;
        }

        public TileObject GetTileAt(TileLoc tileLoc)
        {
            return !Tiles.ContainsKey(tileLoc) ? null : Tiles[tileLoc];
        }

        public void RemoveTileAt(TileLoc tileLoc)
        {
            if (Tiles.ContainsKey(tileLoc))
            {
                Tiles.Remove(tileLoc);
            }
        }
    }
}