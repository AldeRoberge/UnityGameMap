using UnityEngine;
using Visuals;
using World.Tiles;

namespace World.Objects
{
    public class ObjectMap : GenericTileMap<ThreeDimensionObject>
    {
        public new void Start()
        {
            base.Start();

            UnityEngine.Object pPrefab1 = Resources.Load("3D/Pots_type_5"); // note: not .prefab!
            UnityEngine.Object pPrefab2 = Resources.Load("3D/Pots_type_4"); // note: not .prefab!
            UnityEngine.Object pPrefab3 = Resources.Load("3D/Pots_type_4"); // note: not .prefab!
            UnityEngine.Object pPrefab4 = Resources.Load("3D/Pots_type_5"); // note: not .prefab!

            Clone(pPrefab1, new TileLoc(0, 0));
            Clone(pPrefab2, new TileLoc(1, 1));
            Clone(pPrefab3, new TileLoc(2, 2));
            Clone(pPrefab4, new TileLoc(3, 3));
        }

        public void MoveObjectTo(ThreeDimensionObject obj, TileLoc newPos)
        {
            if (!Tiles.ContainsValue(obj))
            {
                Debug.Log("Fatal error : Could not move object '" + obj + "' because it is not found in the tilemap.");
                return;
            }

            Tiles.Remove(obj.tileLoc);
            Tiles.Add(newPos, obj);

            obj.tileLoc = newPos;
            obj.transform.position = GameMap.Instance.GetWorldPosFromTileLoc(newPos);
        }

        private void Clone(UnityEngine.Object pPrefab, TileLoc tileLoc)
        {
            if (GetTileObjectAt(tileLoc))
            {
                Debug.Log("Existing object at '" + tileLoc + "'.");
                return;
            }

            GameObject pNewObject = (GameObject) Instantiate(pPrefab, transform, true);
            MeshCollider collider = pNewObject.AddComponent<MeshCollider>();
            collider.convex = true;

            ThreeDimensionObject o = pNewObject.AddComponent<ThreeDimensionObject>();
            Selecteable selecteable = pNewObject.AddComponent<Selecteable>();
            selecteable.onSelection.AddListener(() => { });

            Tiles.Add(tileLoc, o);
            o.tileLoc = tileLoc;
            MoveObjectTo(o, tileLoc);
        }

        

        
    }
}