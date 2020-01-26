using UnityEngine;
using World.Tiles;

namespace World.Objects
{
    public class Object : MonoBehaviour
    {
        public string objectType;

        public TileLoc tileLoc;

        public void Rotate90()
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