using Objects.Tiles;
using Objects.Tiles.ConnectedTiles;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameMapInteraction : MonoBehaviour
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

                    Debug.Log("Hit " + to + "!");

                    ConnectedTileObject cto = Hit.collider.gameObject.GetComponent<ConnectedTileObject>();

                    bool shouldPlace = true;

                    if (cto != null)
                    {
                        Debug.Log("Already a ConnectedTile in place. Removing.");
                        connectedTileMap.RemoveConnectedTileAt(cto.tileLoc);

                        if (cto.objectType == pathObjectTypeToBuild)
                        {
                            // Do not place new path.
                            shouldPlace = false;
                        }
                    }

                    if (shouldPlace)
                    {
                        Debug.Log("Placing a new tile at pos.");

                        ConnectedTileObject newCto =
                            connectedTileMap.CreateConnectedTileObject(to.tileLoc, pathObjectTypeToBuild);
                    }

                    // Update the surrounding tiles
                    UpdateNeigboursOf(to.tileLoc);
                }
            }
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