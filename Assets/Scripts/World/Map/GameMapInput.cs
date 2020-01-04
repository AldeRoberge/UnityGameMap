using Map.Objects.Tiles;
using Map.Objects.Tiles.ConnectedTiles;
using UnityEngine;

namespace Map
{
    public class GameMapInput : MonoBehaviour
    {
        public int pathObjectTypeToBuild = 0;

        public TileMap tileMap;
        public ConnectedTileMap connectedTileMap;

        void Update()
        {
            CheckIfPaletteChanged();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                tileMap.ToggleGrid();
            }

            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit Hit;

                if (Physics.Raycast(ray, out Hit))
                {
                    TileObject to = Hit.collider.gameObject.GetComponent<TileObject>();

                    if (to == null) return;

                    PlaceObjectOfTypeAt(pathObjectTypeToBuild, to.tileLoc);
                }
            }
        }

        private void PlaceObjectOfTypeAt(int objectType, TileLoc loc)
        {
            ConnectedTileObject cto = connectedTileMap.GetConnectedObjectAt(loc);

            bool shouldPlace = true;

            if (cto != null)
            {
                Debug.Log("Removing underlying tile of type " + objectType + " at " + loc + ".");
                connectedTileMap.RemoveConnectedTileAt(cto.tileLoc);

                if (cto.objectType == pathObjectTypeToBuild)
                {
                    // Do not place new path.
                    shouldPlace = false;
                }
            }

            if (shouldPlace)
            {
                Debug.Log("Placing a new tile of type " + objectType + " at " + loc + ".");
                connectedTileMap.CreateConnectedTileObject(loc, pathObjectTypeToBuild);
            }

            // Update the surrounding tiles
            UpdateNeigboursOf(loc);
        }

        private void CheckIfPaletteChanged()
        {
            bool updated = false;

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                pathObjectTypeToBuild++;

                if (pathObjectTypeToBuild > 1)
                {
                    pathObjectTypeToBuild = 0;
                }

                updated = true;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                pathObjectTypeToBuild++;

                if (pathObjectTypeToBuild < 0)
                {
                    pathObjectTypeToBuild = 1;
                }

                updated = true;
            }

            if (updated)
            {
                Debug.Log("Newly selected palette : " + pathObjectTypeToBuild);
            }
        }

        private void UpdateNeigboursOf(TileLoc tileLoc)
        {
            // Update neighbours
            foreach (TileLoc pos in ConnectedObjectUtils.Around)
            {
                ConnectedTileObject c =
                    GameMap.Instance.connectedTileMap.GetConnectedObjectAt(pos.RelativeTo(tileLoc));

                if (c != null)
                {
                    Debug.Log("Updating " + tileLoc + "...");
                    c.UpdateConnection();
                }
            }
        }
    }
}