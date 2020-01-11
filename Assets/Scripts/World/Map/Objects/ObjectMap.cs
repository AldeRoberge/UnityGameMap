using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Visuals;
using World.Map.Tiles;

namespace World.Map.Objects
{
    public class ObjectMap : MonoBehaviour
    {
        public Dictionary<TileLoc, Object> Objects;

        public void Start()
        {
            UnityEngine.Object pPrefab1 = Resources.Load("3D/Pots_type_5"); // note: not .prefab!
            UnityEngine.Object pPrefab2 = Resources.Load("3D/Pots_type_4"); // note: not .prefab!
            UnityEngine.Object pPrefab3 = Resources.Load("3D/Pots_type_4"); // note: not .prefab!
            UnityEngine.Object pPrefab4 = Resources.Load("3D/Pots_type_5"); // note: not .prefab!

            Clone(pPrefab1, new TileLoc(0, 0));
            Clone(pPrefab2, new TileLoc(1, 1));
            Clone(pPrefab3, new TileLoc(2, 2));
            Clone(pPrefab4, new TileLoc(3, 3));
        }

        private void Clone(UnityEngine.Object pPrefab, TileLoc tileLoc)
        {
            GameObject pNewObject = (GameObject) Instantiate(pPrefab);
            MeshCollider collider = pNewObject.AddComponent<MeshCollider>();
            collider.convex = true;

            Object o = pNewObject.AddComponent<Object>();
            o.MoveTo(tileLoc);

            Selecteable selecteable = pNewObject.AddComponent<Selecteable>();
            selecteable.onSelection.AddListener(() => { });
        }
    }
}