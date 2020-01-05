using UnityEngine;
using UnityEngine.Tilemaps;
using World;

namespace Map.Objects.Tiles
{
    /**
 * A TileObject is an object that is on a tile.
 * It is bound to its TileLoc position.
 */
    public class TileObject : SelecteableObject
    {
        public TileLoc tileLoc;

        public void SetRotation(int rotation)
        {
            this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, rotation, 0));
        }
    }

    public class SelecteableObject : Object
    {
        public static Texture2D gridOverlay;
        public static Texture2D selectedOverlay;
        
        public static Texture2D clearOverlay;

        public bool showGrid;
        public bool isSelected;
        public bool showArrow;

        public Material material;

        public void Start()
        {
            gridOverlay = Resources.Load<Texture2D>("Sprites/Ground/Overlay/Grid");
            selectedOverlay = Resources.Load<Texture2D>("Sprites/Ground/Overlay/Selected");
            clearOverlay = Resources.Load<Texture2D>("Sprites/Ground/Overlay/Clear");
        }

        public void SetOverlay(Texture2D overlay)
        {

            if (material == null)
            {
                material = gameObject.GetComponent<MeshRenderer>().material;
            }

            if (overlay == null)
            {
                Debug.LogError("Null overlay.");
            }
            
            material.EnableKeyword("_DETAIL_MULX2");
            material.SetTexture("_DetailMask", overlay);
            material.SetTexture("_DetailAlbedoMap", overlay);
        }

        public void HideOverlay()
        {
            if (material == null)
            {
                material = gameObject.GetComponent<MeshRenderer>().material;
            }
            
            //to.GetComponent<MeshRenderer>().material.SetTexture("_DetailAlbedoMap", null);
            material.DisableKeyword("_DETAIL_MULX2");
        }

        public void ShowArrow(Orientation orientation)
        {
            showArrow = true;
        }

        public void EnableGrid(bool enabled)
        {
            if (enabled)
            {
                SetOverlay(gridOverlay);
            }
            else
            {
                SetOverlay(clearOverlay);
            }
        }


        public void ShowSelected()
        {
            SetOverlay(selectedOverlay);
        }


        public void HideSelected()
        {
            SetOverlay(clearOverlay);
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
        
    }
}