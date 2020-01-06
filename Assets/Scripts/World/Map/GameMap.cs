using Map.Objects.Tiles;
using Map.Objects.Tiles.ConnectedTiles;
using UnityEngine;
using Utils;
using World.Objects.Tiles.ConnectedTiles;
using TileLoc = Map.Objects.Tiles.TileLoc;

namespace Map
{
    /**
     * GameMap is a parent of the following types :
     
     *     TileMap : Tiles (ground)
     *     ConnectedTileMap : Connected Tiles (paths)
     *     ObjectMap : Objects (players, buildings, etc.)
     */
    public class GameMap : Singleton<GameMap>
    {
        public GameMapInput input;

        public TileMap tileMap;
        public ConnectedTileMap connectedTileMap;

        public const int SquaredMapSize = 10;

        public void Start()
        {
            InitGameMapInteraction();

            InitConnectedTileMap();
            InitTileMap();

            GenerateFakeObjectAtRandomPost();
        }

        private void InitGameMapInteraction()
        {
            input = new GameObject("GameMapInput").AddComponent<GameMapInput>();
            input.transform.parent = transform;
            input.transform.position = new Vector3(0f, 0.1f, 0f);
        }

        private void InitConnectedTileMap()
        {
            connectedTileMap = new GameObject("ConnectedTileObjects").AddComponent<ConnectedTileMap>();
            connectedTileMap.transform.parent = transform;
            connectedTileMap.transform.position = new Vector3(0f, 0.05f, 0f);
        }

        private void InitTileMap()
        {
            tileMap = new GameObject("TileObjects").AddComponent<TileMap>();
            tileMap.transform.parent = transform;
            tileMap.transform.position = new Vector3(0f, 0f, 0f);
        }

        private void GenerateFakeObjectAtRandomPost()
        {
            TileLoc loc = new TileLoc(Random.Range(0, SquaredMapSize), Random.Range(0, SquaredMapSize));

            //GameObject donut = Resources.I TODO
        }

        /**
     * Utility method used by both ConnectedTileObjects (paths) and TileObjects (other) to create a basic tile.
     */
        public GameObject CreateTileAt(TileLoc loc, Transform parent)
        {
            GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Plane);
            tile.name = "Ground Tile (" + loc.x + ", " + loc.y + ")";
            tile.transform.parent = parent;
            tile.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            tile.transform.localPosition = new Vector3(loc.x, 0, loc.y);

            return tile;
        }
    }

    public class GameMapInput : MonoBehaviour
    {
        public int pathObjectTypeToBuild = 0;

        public InteractionTileMap interactionMap;

        public TileMap tileMap;
        public ConnectedTileMap connectedTileMap;

        public void Start()
        {
            tileMap = GameMap.Instance.tileMap;
            connectedTileMap = GameMap.Instance.connectedTileMap;
            interactionMap = this.gameObject.AddComponent<InteractionTileMap>();
        }

        void Update()
        {
            CheckIfPaletteChanged();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Showing grid...");
                interactionMap.ToggleGrid();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                for (int x = 0; x < GameMap.SquaredMapSize; x++)
                {
                    for (int y = 0; y < GameMap.SquaredMapSize; y++)
                    {
                        tileMap.CreateTileObject(new TileLoc(x, y), 0);
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit Hit;

                if (Physics.Raycast(ray, out Hit))
                {
                    TileObject to = Hit.collider.gameObject.GetComponent<TileObject>();

                    if (to == null) return;

                    interactionMap.SetSelectedTile(to.tileLoc);
                    PlaceObjectOfTypeAt(pathObjectTypeToBuild, to.tileLoc);
                }
            }
        }

        private void PlaceObjectOfTypeAt(int objectType, TileLoc loc)
        {
            ConnectedTileObject cto = connectedTileMap.GetConnectedTileAt(loc);

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
            foreach (TileLoc pos in TilesUtils.Cardinal)
            {
                ConnectedTileObject c =
                    GameMap.Instance.connectedTileMap.GetConnectedTileAt(pos.RelativeTo(tileLoc));

                if (c != null)
                {
                    Debug.Log("Updating " + tileLoc + "...");
                    c.UpdateConnection();
                }
            }
        }
    }
}