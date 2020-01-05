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


        public void SetSelectedTile(TileLoc tileLoc)
        {
            if (selectedTileObject != null)
            {
                selectedTileObject.HideSelected();
            }

            TileObject to;

            if (!Tiles.ContainsKey(tileLoc))
            {
                to = CreateTileObject(tileLoc, 0);
            }
            else
            {
                to = Tiles[tileLoc];
            }

            selectedTileObject = to;
            selectedTileObject.ShowSelected();


            foreach (TileObject tileObject in GameMap.Instance.tileMap.Tiles.Values)
            {
                tileObject.HideOverlay();
            }

            // Update neighbours
            foreach (TileLoc pos in TilesUtils.ImmediateCardinal)
            {
                TileObject c = GameMap.Instance.tileMap.GetTileAt(pos.RelativeTo(selectedTileObject.tileLoc));

                if (c != null)
                {
                    int rotation = 0;

                    if (arrow == null)
                    {
                        arrow = Resources.Load<Texture2D>("Sprites/Ground/Interaction/Arrow");
                    }

                    if (pos == TilesUtils.North)
                    {
                        rotation = 180;
                    }
                    else if (pos == TilesUtils.East)
                    {
                        rotation = 270;
                    }
                    else if (pos == TilesUtils.South)
                    {
                        rotation = 0;
                    }
                    else if (pos == TilesUtils.West)
                    {
                        rotation = 90;
                    }


                    c.SetOverlay(arrow);
                    c.SetRotation(rotation);

                    //c.ShowArrow();
                }
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

            foreach (TileObject to in GameMap.Instance.tileMap.Tiles.Values)
            {
                TileObject interactionTileMapTile;

                if (!Tiles.ContainsKey(to.tileLoc))
                {
                    // Will create a new interaction tile for every tiles in the GameMap tilemap.
                    interactionTileMapTile = CreateTileObject(to.tileLoc, 1);
                }
                else
                {
                    interactionTileMapTile = Tiles[to.tileLoc];
                }

                if (interactionTileMapTile == selectedTileObject)
                {
                    //Skip the "selected" tile.
                    continue;
                }

                interactionTileMapTile.EnableGrid(isShowingGrid);
            }
        }
    }
}