using System.Collections.Generic;
using UnityEngine;
using World.Tiles;

namespace World.ConnectedTiles
{
    public class ConnectedTileObject : TileObject
    {
        private static readonly Dictionary<int, List<int>> OriginalBlobAndRotations = new Dictionary<int, List<int>>()
        {
            {0, new List<int>()},
            {1, new List<int>() {4, 16, 64}},
            {5, new List<int>() {20, 80, 65}},
            {7, new List<int>() {28, 112, 193}},
            {17, new List<int>() {68}},
            {21, new List<int>() {84, 81, 69}},
            {23, new List<int>() {92, 113, 197}},
            {29, new List<int>() {116, 209, 71}},
            {31, new List<int>() {124, 241, 199}},
            {85, new List<int>()},
            {87, new List<int>() {93, 117, 213}},
            {95, new List<int>() {125, 245, 215}},
            {119, new List<int>() {221}},
            {127, new List<int>() {253, 247, 223}},
            {255, new List<int>()},
        };

        public bool IsOfSameType(TileLoc otherTile)
        {
            ConnectedTileObject cto = GameMap.Instance.connectedGenericTileMap.GetConnectedTileAt(otherTile);

            if (cto == null)
            {
                return false;
            }

            return cto.objectType == objectType;
        }

        /**
    	    The following is pseudocode taken from :
    		    http://www.cr31.co.uk/stagecast/wang/blob.html
    		    https://www.boristhebrave.com/2013/07/14/tileset-roundup/
    		    
    	    Note : It has been converted from "top" convention to "north" and etc.
    
    	    if not West or not North:     NorthWest = False
    	    if not West or not South: SouthWest = False
    	    if not East or not North:    NorthEast = False
    	    if not East or not South: SouthEast = False
    
    	    neighbour_index = (
    	        NorthWest + 2*North + 4*NorthEast +  
    	        8*West + 16*East + 
    	        32*SouthWest + 64*South + 128*SouthEast)
    
            Where each variable is 0 or 1 according to if that corner is solid or not.
        */
        public int NeighbourTilesWeight()
        {
            bool northWest = IsOfSameType(ConnectedTileUtils.NorthWest.RelativeTo(tileLoc));
            bool north = IsOfSameType(ConnectedTileUtils.North.RelativeTo(tileLoc));
            bool northEast = IsOfSameType(ConnectedTileUtils.NorthEast.RelativeTo(tileLoc));

            bool west = IsOfSameType(ConnectedTileUtils.West.RelativeTo(tileLoc));
            // center is omitted
            bool east = IsOfSameType(ConnectedTileUtils.East.RelativeTo(tileLoc));

            bool southWest = IsOfSameType(ConnectedTileUtils.SouthWest.RelativeTo(tileLoc));
            bool south = IsOfSameType(ConnectedTileUtils.South.RelativeTo(tileLoc));
            bool southEast = IsOfSameType(ConnectedTileUtils.SouthEast.RelativeTo(tileLoc));

            if (!west || !north)
                northWest = false;

            if (!west || !south)
                southWest = false;

            if (!north || !east)
                northEast = false;

            if (!south || !east)
                southEast = false;

            int iNorthWest = northWest ? 1 : 0;
            int iNorth = north ? 1 : 0;
            int iNorthEast = northEast ? 1 : 0;

            int iWest = west ? 1 : 0;
            // Center is omitted
            int iEast = east ? 1 : 0;

            int iSouthWest = southWest ? 1 : 0;
            int iSouth = south ? 1 : 0;
            int iSouthEast = southEast ? 1 : 0;

            return iNorth + 2 * iNorthEast + 4 * iEast + 8 * iSouthEast + 16 * iSouth + 32 * iSouthWest + 64 * iWest +
                   128 * iNorthWest;
        }

        public void UpdateConnection()
        {
            if (objectType == UITileObjectTypes.CLEAR)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                return;
            }

            gameObject.GetComponent<MeshRenderer>().enabled = true;

            int weight = NeighbourTilesWeight();

            Debug.Log("Updating tile at " + tileLoc.x + ", " + tileLoc.y + "Weight : " + weight);

            int rotation = 180;
            string textureName = null;

            foreach (KeyValuePair<int, List<int>> blobAndRotationVariant in OriginalBlobAndRotations)
            {
                // Tile without rotation
                if (weight == blobAndRotationVariant.Key)
                {
                    textureName = blobAndRotationVariant.Key.ToString();
                    break;
                }

                // Tile with rotation
                foreach (int i in blobAndRotationVariant.Value)
                {
                    if (i == weight)
                    {
                        textureName = blobAndRotationVariant.Key.ToString();

                        Debug.Log("Fount path of rotated blob : '" + textureName + "'.");

                        switch (blobAndRotationVariant.Value.IndexOf(i))
                        {
                            case 0:
                                rotation = 270;
                                break;
                            case 1:
                                rotation = 0;
                                break;
                            case 2:
                                rotation = 90;
                                break;
                            default:
                                Debug.LogError("Fatal error : Index '" + blobAndRotationVariant.Value.IndexOf(i) +
                                               "' of vartiant of blob '" + blobAndRotationVariant.Key +
                                               "' is not supported.");
                                break;
                        }

                        break;
                    }
                }
            }

            string actualResourcePath =
                "Sprites/Ground/ConnectedTiles/" + objectType + "/" + textureName;

            Texture2D texture = Resources.Load<Texture2D>(actualResourcePath);

            if (texture == null)
            {
                Debug.LogError("Texture for path '" + actualResourcePath + "' is null.");
            }

            SetTexture(texture);

            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.Rotate(new Vector3(0, rotation, 0));
        }
    }

    public static class ConnectedTileUtils
    {
        /**
	
	Defines the relative x,y for the following locations :
	
	[NorthWest, North,   NorthEast]
	[West,      Center,  East] 
	[SouthWest, South,   SouthEast]
	*/
        public static readonly TileLoc NorthWest = new TileLoc(-1, 1);

        public static readonly TileLoc North = new TileLoc(0, 1);
        public static readonly TileLoc NorthEast = new TileLoc(1, 1);

        public static readonly TileLoc West = new TileLoc(-1, 0);
        public static readonly TileLoc Center = new TileLoc(0, 0);
        public static readonly TileLoc East = new TileLoc(1, 0);

        public static readonly TileLoc SouthWest = new TileLoc(-1, -1);
        public static readonly TileLoc South = new TileLoc(0, -1);
        public static readonly TileLoc SouthEast = new TileLoc(1, -1);

        public static TileLoc[] Cardinal =
        {
            NorthWest, North, NorthEast,
            West, Center, East,
            SouthWest, South, SouthEast
        };

        public static TileLoc[] ImmediateCardinal =
        {
            North, West, East, South
        };
    }
}