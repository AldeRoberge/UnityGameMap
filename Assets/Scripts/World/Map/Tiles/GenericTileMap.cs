using System;
using System.Collections.Generic;
using UnityEngine;

namespace World.Map.Tiles
{
    /**
     * A GenericTileMap. It is used by :
     *
     *     - TileMap : Ground tiles
     *     - ConnectedTileMap : Paths
     *     - InteractionTileMap : Selection, Grid overlays
     *     - ObjectMap : Objects
     */
    public class GenericTileMap<T> : MonoBehaviour where T : TileObject
    {
        public Dictionary<TileLoc, T> Tiles;

        public void Start()
        {
            Tiles = new Dictionary<TileLoc, T>();
        }

        public T GetTileObjectAt(TileLoc tileLoc)
        {
            if (tileLoc == null)
            {
                Debug.LogError("Fatal : TileLoc should not be null.");
                return null;
            }

            return !Tiles.ContainsKey(tileLoc) ? null : Tiles[tileLoc];
        }

        public void RemoveTileObjectAt(TileLoc tileLoc)
        {
            if (tileLoc == null)
            {
                Debug.LogError("Fatal : TileLoc should not be null.");
                return;
            }

            if (Tiles.ContainsKey(tileLoc))
            {
                Tiles.Remove(tileLoc);
            }
        }
    }
}