using UnityEngine;
using Utils;
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
        public TileMap tileMap;
        public ConnectedTileMap connectedTileMap;

        public void Start()
        {
            InitTileMap();
            InitConnectedTileMap();
            
            
            InitGameMapInteraction();

            GenerateFakeObjectAtRandomPost();
        }

        
        
        private void InitTileMap()
        {
            tileMap = new GameObject("TileObjects").AddComponent<TileMap>();
            tileMap.transform.parent = transform;
        }

        private void InitConnectedTileMap()
        {
            connectedTileMap = new GameObject("ConnectedTileObjects").AddComponent<ConnectedTileMap>();
            connectedTileMap.transform.parent = transform;
        }
        
        
        
        private void GenerateFakeObjectAtRandomPost()
        {
            TileLoc loc = new TileLoc(Random.Range(0, TileMap.SquaredMapSize), Random.Range(0, TileMap.SquaredMapSize));
        
            //GameObject donut = Resources.I TODO
        
        
        }

        private void InitGameMapInteraction()
        {
            GameMapInput gameMapInput = gameObject.AddComponent<GameMapInput>();
            gameMapInput.tileMap = tileMap;
            gameMapInput.connectedTileMap = connectedTileMap;
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
}