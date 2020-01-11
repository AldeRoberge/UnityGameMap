using System;
using System.Collections.Generic;
using Controls;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;
using Visuals;
using World.Map.ConnectedTiles;
using World.Map.Objects;
using World.Map.Tiles;
using Object = World.Map.Objects.Object;
using Random = UnityEngine.Random;
using TileLoc = World.Map.Tiles.TileLoc;

namespace World.Map
{
    /**
     * GameMap is the parent of the following types :
     
     *     TileMap : Tiles (ground)
     *     ConnectedTileMap : Connected Tiles (paths)
     *     ObjectMap : Objects (players, buildings, etc.)
     */
    public class GameMap : Singleton<GameMap>
    {
        public GameMapInput input;

        public TileMap tileMap;
        public ConnectedGenericTileMap connectedGenericTileMap;

        public ObjectMap objectMap;


        public void Start()
        {
            InitGameMapInteraction();

            InitConnectedTileMap();
            InitTileMap();

            InitObjectMap();
        }

        private void InitObjectMap()
        {
            objectMap = new GameObject("ObjectMap").AddComponent<ObjectMap>();
            objectMap.transform.parent = transform;
            objectMap.transform.position = new Vector3(0f, 0.1f, 0f);
        }

        private void InitGameMapInteraction()
        {
            input = new GameObject("GameMapInput").AddComponent<GameMapInput>();
            input.transform.parent = transform;
            input.transform.position = new Vector3(0f, 0.1f, 0f);
        }

        private void InitConnectedTileMap()
        {
            connectedGenericTileMap = new GameObject("ConnectedTileObjects").AddComponent<ConnectedGenericTileMap>();
            connectedGenericTileMap.transform.parent = transform;
            connectedGenericTileMap.transform.position = new Vector3(0f, 0.05f, 0f);
        }

        private void InitTileMap()
        {
            tileMap = new GameObject("TileObjects").AddComponent<TileMap>();
            tileMap.transform.parent = transform;
            tileMap.transform.position = new Vector3(0f, 0f, 0f);

            
        }

        // Converts a TileLoc to a Vector3
        public Vector3 GetWorldPosFromTileLoc(TileLoc tileLoc)
        {
            TileObject tileAt = tileMap.GetTileObjectAt(tileLoc);

            if (tileAt != null)
            {
                return tileAt.transform.position;
            }

            Debug.Log("tileMapTiles : " + tileMap.Tiles.Count);

            Debug.LogError("Fatal error : Could not find tile at '" + tileLoc + "'.");

            return new Vector3();
        }

        // Converts a Vector3 to a TileLoc
        public TileLoc GetTileLocFromWorldPos(Vector3 worldPos)
        {
            foreach (TileObject to in tileMap.Tiles.Values)
            {
                if (to.transform.position == worldPos)
                {
                    return to.tileLoc;
                }
            }

            Debug.Log("Could not find TilePos for WorldPos " + worldPos + ".");

            return new TileLoc(0, 0);
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

    public enum EditType
    {
        Path,
        Objects
    }

    public class GameMapInput : MonoBehaviour
    {
        public string pathObjectTypeToBuild = "Path";

        public InteractionTileMap interactionMap;

        public TileMap tileMap;
        public ConnectedGenericTileMap connectedGenericTileMap;
        public ObjectMap objectMap;

        public EditType editing = EditType.Objects;

        public List<Object> selectedObjects; //This is a list, for in the future for multiple object selection.

        public void Start()
        {
            tileMap = GameMap.Instance.tileMap;
            connectedGenericTileMap = GameMap.Instance.connectedGenericTileMap;
            interactionMap = gameObject.AddComponent<InteractionTileMap>();

            objectMap = GameMap.Instance.objectMap;

            selectedObjects = new List<Object>();
        }

        private Object moving;
        private GameObject tileMouseClickOrigin;
        private GameObject tileMousePosition;

        public bool isMovingObject = false;

        public bool ShowMouseInteraction = false;

        /**
         * Registers events done to objects and tiles.
         */
        void Update()
        {
            // Toggle Grid
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Showing grid...");
                interactionMap.ToggleGrid();
            }

            // Mouse was released, mouse did not move more than one tile. Selecting object/tile.
            if (Input.GetMouseButtonUp(0))
            {
                if (!isMovingObject)
                {
                    Debug.Log("Mouse released. Was not carrying an object.");

                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit Hit;

                    if (Physics.Raycast(ray, out Hit))
                    {
                        if (editing == EditType.Path)
                        {
                            Debug.Log("Currently editing paths.");

                            TileObject to = Hit.collider.gameObject.GetComponent<TileObject>();

                            if (to == null)
                            {
                                Debug.Log("No tile selected.");

                                interactionMap.UnselectTile();
                                Controls.Controls.Instance.Enable();
                            }
                            else
                            {
                                Debug.Log("Selected new tile.");

                                interactionMap.SetSelectedTile(to.tileLoc);
                                PlaceObjectOfTypeAt(pathObjectTypeToBuild, to.tileLoc);
                            }
                        }
                        else if (editing == EditType.Objects)
                        {
                            Debug.Log("Currently editing objects.");

                            Object to = Hit.collider.gameObject.GetComponent<Object>();

                            if (to == null)
                            {
                                Debug.Log("No object selected");

                                UnselectObjects();
                                Controls.Controls.Instance.Enable();
                            }
                            else
                            {
                                Debug.Log("Selected new object.");

                                SelectObject(to);
                            }
                        }
                    }
                }

                ResetObjectMovement();
            }

            // Mouse is pressed, create the mouse position helpers.
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log("Is over game object, skipping.");
                }
                else
                {
                    tileMouseClickOrigin = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    tileMouseClickOrigin.GetComponent<Renderer>().material.color = Color.red;
                    tileMouseClickOrigin.GetComponent<Renderer>().enabled = ShowMouseInteraction;
                    tileMouseClickOrigin.transform.position = new Vector3(0f, 0f, 0f);

                    tileMousePosition = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    tileMousePosition.GetComponent<Renderer>().material.color = Color.blue;
                    tileMousePosition.GetComponent<Renderer>().enabled = ShowMouseInteraction;
                    tileMousePosition.transform.position = new Vector3(0f, 0f, 0f);

                    // Init origin pos
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out var Hit))
                    {
                        tileMouseClickOrigin.transform.position = Hit.collider.gameObject.transform.position;
                        moving = Hit.collider.gameObject.GetComponent<Object>();
                        SelectObject(moving);

                        if (moving is TileObject)
                        {
                            Debug.Log("Moving tile object, not moving.");

                            ResetObjectMovement();
                            UnselectObjects();
                            Controls.Controls.Instance.Enable();
                        }
                        else
                        {
                            Debug.Log("Moving object.");

                            Controls.Controls.Instance.Disable();
                        }
                    }
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (tileMouseClickOrigin != null)
                {
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out var Hit))
                    {
                        tileMousePosition.transform.position = Hit.collider.gameObject.transform.position;

                        // Calculate distance between 
                        if (Vector3.Distance(tileMouseClickOrigin.transform.position, tileMousePosition.transform.position) > 0.01)
                        {
                            isMovingObject = true;
                        }

                        if (isMovingObject && moving != null)
                        {
                            if (!objectMap.HasTileObjectAt(tileMousePosition.transform.position))
                            {
                                moving.transform.position = tileMousePosition.transform.position;
                            }
                            else
                            {
                                Debug.Log("Object already at this position.");
                            }
                        }
                    }
                }
            }

            return;
        }

        private void ResetObjectMovement()
        {
            if (tileMouseClickOrigin != null)
            {
                Destroy(tileMousePosition);
                Destroy(tileMouseClickOrigin);
            }

            isMovingObject = false;
            moving = null;
        }

        private void SelectObject(Object to)
        {
            Selecteable s = to.gameObject.GetComponent<Selecteable>();

            if (s != null)
            {
                UnselectObjects();

                selectedObjects.Clear();

                // Set selected to true
                s.SetSelected(true);

                if (!selectedObjects.Contains(to))
                {
                    selectedObjects.Add(to);
                }

                UIManager.Instance.ShowOptionsFor(to);
            }
        }

        private void UnselectObjects()
        {
            // Remove other selected objects
            foreach (Object o in selectedObjects)
            {
                o.GetComponent<Selecteable>().SetSelected(false);
            }

            UIManager.Instance.ClearButtons();
        }

        private void PlaceObjectOfTypeAt(string id, TileLoc loc)
        {
            ConnectedTileObject cto = connectedGenericTileMap.GetConnectedTileAt(loc);

            bool shouldPlace = true;

            if (cto != null)
            {
                Debug.Log("Removing underlying tile of type " + id + " at " + loc + ".");
                connectedGenericTileMap.RemoveConnectedTileAt(cto.tileLoc);

                if (cto.GetObjectType() == pathObjectTypeToBuild)
                {
                    // Do not place new path.
                    shouldPlace = false;
                }
            }

            if (shouldPlace)
            {
                Debug.Log("Placing a new tile of type " + id + " at " + loc + ".");
                connectedGenericTileMap.CreateConnectedTileObject(loc, pathObjectTypeToBuild);
            }

            // Update the surrounding tiles
            UpdateNeigboursOf(loc);
        }

        private void UpdateNeigboursOf(TileLoc tileLoc)
        {
            // Update neighbours
            foreach (TileLoc pos in ConnectedTileUtils.Cardinal)
            {
                ConnectedTileObject c =
                    GameMap.Instance.connectedGenericTileMap.GetConnectedTileAt(pos.RelativeTo(tileLoc));

                if (c != null)
                {
                    Debug.Log("Updating " + tileLoc + "...");
                    c.UpdateConnection();
                }
            }
        }
    }
}