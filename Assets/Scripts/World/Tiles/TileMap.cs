using UnityEngine;

namespace World.Tiles
{
    public class TileMap : GenericTileMap<TileObject>
    {
        public const int SquaredMapSize = 10;

        public new void Start()
        {
            base.Start();

            for (int x = 0; x < SquaredMapSize; x++)
            {
                for (int y = 0; y < SquaredMapSize; y++)
                {
                    CreateTileObject(new TileLoc(x, y), UITileObjectTypes.GRASS);
                }
            }
        }

        public TileObject CreateTileObject(TileLoc loc, string type = UITileObjectTypes.DEFAULT)
        {
            if (GetTileObjectAt(loc) != null)
            {
                Debug.Log("Fatal : Tile already at '" + loc + "'.");
                return null;
            }

            GameObject tile = GameMap.Instance.CreateTileAt(loc, transform);

            TileObject c = tile.AddComponent<TileObject>();

            c.SetObjectType(type);
            c.tileLoc = loc;

            Tiles.Add(loc, c);

            return c;
        }
    }
}