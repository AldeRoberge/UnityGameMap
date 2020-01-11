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
            UnityEngine.Object pPrefab = Resources.Load("3D/Pots_type_4"); // note: not .prefab!

            Clone(pPrefab, new TileLoc(0, 0));
            Clone(pPrefab, new TileLoc(1, 1));
            Clone(pPrefab, new TileLoc(2, 2));
            Clone(pPrefab, new TileLoc(3, 3));
        }

        private void Clone(UnityEngine.Object pPrefab, TileLoc tileLoc)
        {
            GameObject pNewObject = (GameObject) Instantiate(pPrefab);
            pNewObject.AddComponent<MeshCollider>();

            Object o = pNewObject.AddComponent<Object>();
            o.MoveTo(tileLoc);

            Selecteable selecteable = pNewObject.AddComponent<Selecteable>();
            selecteable.onSelection.AddListener(() =>
            {
                
            });
        }
    }
}