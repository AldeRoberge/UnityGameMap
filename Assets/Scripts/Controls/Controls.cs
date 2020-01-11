using UnityEngine;
using Utils;

namespace Controls
{
    [RequireComponent(typeof(MobileControls))]
    [RequireComponent(typeof(DesktopControls))]
    public class Controls : Singleton<Controls>
    {
        private MobileControls _mobileControls;
        private DesktopControls _desktopControls;

        public void Disable()
        {
            _mobileControls.Disable();
            _desktopControls.Disable();
        }

        public void Enable()
        {
            _mobileControls.Enable();
            _desktopControls.Enable();
        }

        public void Start()
        {
            if (gameObject.transform.position != Vector3.zero)
            {
                Debug.LogError("Fatal : The Controls script must be on a GameObject that is positioned at origin of the game world.");
                return;
            }

            if (Camera.main == null)
            {
                Debug.LogError("Fatal : No camera found.");
                return;
            }

            _mobileControls = GetComponent<MobileControls>();
            _desktopControls = GetComponent<DesktopControls>();

            if (_mobileControls == null || _desktopControls == null)
            {
                Debug.LogError("Fatal : Null mobile controls or desktop controls.");
                return;
            }
            
            _mobileControls.Enable();
            _desktopControls.Enable();
            
            
            
        }

        public Camera GetCamera()
        {
            return Camera.main;
        }
    }
}