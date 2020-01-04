using System.Collections.Generic;
using Map.Objects.Tiles;
using UnityEngine;

namespace Map
{
    
    /**
     * A math of tiles (ground).
     */
    public class TileMap : MonoBehaviour
    {
        public Dictionary<TileLoc, TileObject> Tiles;

        public const int SquaredMapSize = 10;

        public Texture2D gridOverlay;

        public void Start()
        {
            Tiles = new Dictionary<TileLoc, TileObject>();
            InitMap();

            gridOverlay = Resources.Load<Texture2D>("Sprites/Ground/Overlay");

            ShowGrid();
        }

        private void InitMap()
        {

            for (int x = 0; x < SquaredMapSize; x++)
            {
                for (int y = 0; y < SquaredMapSize; y++)
                {
                    CreateTileObject(new TileLoc(x, y), 0);
                }
            }
        }


        private bool isShowingGrid;

        public void ToggleGrid()
        {
            if (isShowingGrid)
            {
                HideGrid();
            }
            else
            {
                ShowGrid();
            }
        }

        public void ShowGrid()
        {
            isShowingGrid = true;

            foreach (TileObject to in Tiles.Values)
            {
                Material m = to.GetComponent<MeshRenderer>().material;

                m.EnableKeyword("_DETAIL_MULX2");
                m.SetTexture("_DetailMask", gridOverlay);
                m.SetTexture("_DetailAlbedoMap", gridOverlay);
            }
        }

        public void HideGrid()
        {
            isShowingGrid = false;

            foreach (TileObject to in Tiles.Values)
            {
                to.GetComponent<MeshRenderer>().material.SetTexture("_DetailAlbedoMap", null);
            }
        }


        public TileObject CreateTileObject(TileLoc loc, int type = -1)
        {
            GameObject tile = GameMap.Instance.CreateTileAt(loc, transform);

            TileObject c = tile.AddComponent<TileObject>();

            //TODO replace, this is the objectLibrary way of loading
            //tile.sprite = ObjectLibrary.getTileTexture(g.Type).texture;

            Debug.Log("Creating object at " + loc + " with image " + ObjectTypeToTexture(type));

            Debug.Log("Sprites/Ground/Tiles/" + ObjectTypeToTexture(type));

            c.SetTexture(Resources.Load<Texture2D>("Sprites/Ground/Tiles/" + ObjectTypeToTexture(type)));

            c.objectType = type;
            c.tileLoc = loc;
            Tiles[loc] = c;

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