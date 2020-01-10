using UnityEngine;
using UnityEngine.Tilemaps;
using World;

namespace Map.Objects.Tiles
{
    /**
 * A TileObject is an object that is on a tile.
 * It is bound to its TileLoc position.
 */
    public class TileObject : MonoBehaviour
    {
        public TileLoc tileLoc;

        private int objectType { get; set; }

        public int GetObjectType()
        {
            return objectType;
        }

        public void SetObjectType(int objectType)
        {
            this.objectType = objectType;
        }


        public void UpdateTexture()
        {
            //TODO replace, this is the objectLibrary way of loading
            //tile.sprite = ObjectLibrary.getTileTexture(g.Type).texture;
            
            Texture2D tex = Resources.Load<Texture2D>("Sprites/Ground/Tiles/" + ObjectTypeToTexture(objectType));

            if (tex == null)
            {
                Debug.Log("Null tex with image " + ObjectTypeToTexture(objectType));
                Debug.Log("Path : 'Sprites/Ground/Tiles/" + ObjectTypeToTexture(objectType) + "'.");
            }

            SetTexture(tex);
        }
        
        public void SetTexture(Texture2D texture)
        {
            //TODO This material should be cached. (And cloned)
            Material material = new Material(Resources.Load<Material>("Materials/GroundMaterial"));

            if (material == null)
            {
                Debug.LogError("Fatal error : Could not load material.");
            }

            if (texture == null)
            {
                Debug.LogError("Fatal error : Texture is null.");
            }

            material.SetTexture("_MainTex", texture);
            material.mainTexture = texture;
            gameObject.GetComponent<MeshRenderer>().material = material;
        }
        
        
        
        private string ObjectTypeToTexture(int objectType)
        {
            switch (objectType)
            {
                case -1:
                    return "Default";
                case 0:
                    return "Grass";
                case 1:
                    return "Clear";
                case 2:
                    return "Grid";
                default:
                    return "";
            }
        }
        
    }

    public static class UITileObjectTypes
    {

        public const int CLEAR = 0;
        public const int DEFAULT = 1;
        public const int GRASS = 2;
        public const int GRID = 3;
        public const int SELECTED = 4;

    }
}