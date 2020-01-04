using UnityEngine;

namespace Map.Objects.Tiles
{
    public static class TileUtils
    {


        public static void SetTexture(this TileObject tileObject, Texture2D texture)
        {
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
            tileObject.gameObject.GetComponent<MeshRenderer>().material = material;
        }
        
    }
}