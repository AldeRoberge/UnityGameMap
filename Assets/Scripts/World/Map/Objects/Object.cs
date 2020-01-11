using UnityEngine;
using World.Map.Tiles;

namespace World.Map.Objects
{
    public class Object : MonoBehaviour
    {
        public string objectType;

        public TileLoc tileLoc;



        public void Rotate()
        {
            var rotation = transform.rotation;
            transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y + 90f, rotation.eulerAngles.z);
        }

        public int GetRotation()
        {
            return (int) transform.rotation.y;
        }
    }
}