using System.Collections.Generic;
using Map.Objects.Tiles;
using UnityEngine;
using World.Objects.Tiles.ConnectedTiles;

namespace Map
{
    /**
     * The interaction tile map is a tilemap that floats above the actual tilemap,
     * showing the user the currently selected tile and a grid.
     *
     * See GameMapInput.
     */
    public class InteractionTileMap : TileMap
    {
        public TileObject selectedTileObject;

        public Texture2D arrow;

        public static Texture2D gridTexture;
        public static Texture2D selectedTexture;
        public static Texture2D clearTexture;

        public new void Start()
        {
            base.Start();
            gridTexture = Resources.Load<Texture2D>("Sprites/Ground/Tiles/Grid");
            selectedTexture = Resources.Load<Texture2D>("Sprites/Ground/Tiles/Selected");
            clearTexture = Resources.Load<Texture2D>("Sprites/Ground/Tiles/Clear");
        }

        /**
         * Sets a tile to show the "selected" texture.
         */
        public void SetSelectedTile(TileLoc tileLoc)
        {
            if (selectedTileObject != null)
            {
                //Hide previously selected tile
                selectedTileObject.SetTexture(clearTexture);
            }

            TileObject to;

            // Create if it doesn't exist
            if (!Tiles.ContainsKey(tileLoc))
            {
                to = CreateTileObject(tileLoc, 1);
            }
            else
            {
                to = Tiles[tileLoc];
            }

            selectedTileObject = to;
            selectedTileObject.SetTexture(selectedTexture);
        }

        private bool isShowingGrid;

        public void ToggleGrid()
        {
            if (!isShowingGrid)
            {
                isShowingGrid = true;
            }
            else
            {
                isShowingGrid = false;
            }

            foreach (TileObject to in GameMap.Instance.tileMap.Tiles.Values)
            {
                TileObject interactionTileMapTile;

                /**
                 * Check if tile exists, if not will create one.
                 */
                if (!Tiles.ContainsKey(to.tileLoc))
                {
                    interactionTileMapTile = CreateTileObject(to.tileLoc, 2);
                }
                else
                {
                    interactionTileMapTile = Tiles[to.tileLoc];
                }


                if (interactionTileMapTile == selectedTileObject)
                {
                    // Skip selected tile
                    continue;
                }
                

                /**
                 * Set the texture to grid if enabled, nothing is disabled.
                 */
                if (isShowingGrid)
                {
                    interactionTileMapTile.SetTexture(gridTexture);
                }
                else
                {
                    interactionTileMapTile.SetTexture(clearTexture);
                }
            }
        }
    }
}