using UnityEngine;
using Object = World.Map.Objects.Object;

namespace World.Map.Tiles
{
    /**
 * A TileObject is an object that is on a tile.
 * It is bound to its TileLoc position.
 */
    public class TileObject : Object
    {


        public string GetObjectType()
        {
            return objectType;
        }

        public void SetObjectType(string objectType)
        {
            this.objectType = objectType;
            UpdateTexture();
        }

        public void UpdateTexture()
        {
            //TODO replace, this is the objectLibrary way of loading
            //tile.sprite = ObjectLibrary.getTileTexture(g.Type).texture;
            
            Texture2D tex = Resources.Load<Texture2D>("Sprites/Ground/Tiles/" + objectType);

            if (tex == null)
            {
                Debug.Log("Null tex with image " + objectType);
                Debug.Log("Path : 'Sprites/Ground/Tiles/" + objectType + "'.");
            }

            SetTexture(tex);
        }
        
        protected void SetTexture(Texture2D texture)
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

    }

    public static class UITileObjectTypes
    {

        public const string CLEAR = "Clear";
        public const string DEFAULT = "Default";
        public const string GRASS = "Grass";
        public const string GRID = "Grid";
        public const string SELECTED = "Selected";

    }
}