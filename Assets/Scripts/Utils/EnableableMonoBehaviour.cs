using UnityEngine;

namespace Utils
{
    public class EnableableMonoBehaviour : MonoBehaviour
    {
        public bool IsEnabled;

        protected internal void Disable()
        {
            IsEnabled = false;
        }

        protected internal void Enable()
        {
            IsEnabled = true;
        }
    }
}