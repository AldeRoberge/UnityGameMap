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
        }

        public TileObject CreateTileObject(TileLoc loc, int type = -1)
        {
            GameObject tile = GameMap.Instance.CreateTileAt(loc, transform);

            TileObject c = tile.AddComponent<TileObject>();

            //TODO replace, this is the objectLibrary way of loading
            //tile.sprite = ObjectLibrary.getTileTexture(g.Type).texture;
            
            Texture2D tex = Resources.Load<Texture2D>("Sprites/Ground/Tiles/" + ObjectTypeToTexture(type));

            if (tex == null)
            {
                Debug.Log("Null tex with image " + ObjectTypeToTexture(type));
                Debug.Log("Path : 'Sprites/Ground/Tiles/" + ObjectTypeToTexture(type) + "'.");
            }

            c.SetTexture(tex);
            c.objectType = type;
            c.tileLoc = loc;
            Tiles.Add(loc, c);

            return c;
        }

        private string ObjectTypeToTexture(int objectType)
        {
            switch (objectType)
            {
                case -1:
                    return "Default";
                case 0:
                    return "Grass";
                case 1:
                    return "Clear";
                default:
                    return "";
            }
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