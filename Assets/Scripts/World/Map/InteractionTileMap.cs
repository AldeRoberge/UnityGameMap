using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using World.Map.Tiles;

namespace World.Map
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

        public bool debugInteraction = true;

        public new void Start()
        {
            base.Start();
            
            for (int x = 0; x < SquaredMapSize; x++)
            {
                for (int y = 0; y < SquaredMapSize; y++)
                {
                    CreateTileObject(new TileLoc(x, y), UITileObjectTypes.CLEAR);
                }
            }
        }

        /**
         * Sets a tile to show the "selected" texture.
         */
        public void SetSelectedTile(TileLoc tileLoc)
        {
            Debug.Log("Set selected tile '" + tileLoc + "'.");

            UnselectTile();

            TileObject to;

            // Create if it doesn't exist
            if (!Tiles.ContainsKey(tileLoc))
            {
                to = CreateTileObject(tileLoc, UITileObjectTypes.SELECTED);
            }
            else
            {
                to = Tiles[tileLoc];
            }

            selectedTileObject = to;
            selectedTileObject.SetObjectType(UITileObjectTypes.SELECTED);
        }

        public void UnselectTile()
        {
            Debug.Log("Unselecting tile.");

            if (selectedTileObject != null)
            {
                //Hide previously selected tile. Replaces with grid if grid is shown.
                selectedTileObject.SetObjectType(isShowingGrid ? UITileObjectTypes.GRID : UITileObjectTypes.CLEAR);
            }
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

            Debug.Log("Is showing grid : " + isShowingGrid);

            foreach (TileObject to in GameMap.Instance.tileMap.Tiles.Values)
            {
                TileObject interactionTileMapTile;

                /**
                 * Check if tile exists, if not will create one.
                 */
                if (!Tiles.ContainsKey(to.tileLoc))
                {
                    interactionTileMapTile = CreateTileObject(to.tileLoc, UITileObjectTypes.GRID);
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
                    interactionTileMapTile.SetObjectType(UITileObjectTypes.GRID);
                }
                else
                {
                    interactionTileMapTile.SetObjectType(UITileObjectTypes.CLEAR);
                }
            }
        }
    }
}