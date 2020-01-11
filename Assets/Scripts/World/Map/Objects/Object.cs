using UnityEngine;
using World.Map.Tiles;

namespace World.Map.Objects
{
    public class Object : MonoBehaviour
    {
        public string objectType;

        public Orientation orientation;

        
        public TileLoc tileLoc;


        public void MoveTo(TileLoc tileLoc)
        {
            this.tileLoc = tileLoc;
            this.transform.position = GameMap.Instance.GetWorldPos(tileLoc);
        }
        

        public void SetRotation(Orientation rotation)
        {
            orientation = rotation;
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, (int) rotation, 0));
        }

        public void SetTileLoc()
        {
            
        }
        
    }
    
    public enum Orientation
    {
        Up = 0,
        Right = 90,
        Down = 180,
        Left = 270
    }
}