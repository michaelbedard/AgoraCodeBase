using _src.Code.Core.Interfaces.Services;
using UnityEngine;
using Zenject;
using Vector2 = UnityEngine.Vector2;

namespace _src.Code.__test__
{
    public class CameraElementTester : MonoBehaviour
    {
        public float x;
        public float y;
        public float normalRotation = 0f;
        public float tangentialRotation = 0f;
        public float offset = 0f;
    
        private ICameraPlaneService _camneraPlaneService;

        [Inject]
        public void Construct(ICameraPlaneService camneraPlaneService)
        {
            _camneraPlaneService = camneraPlaneService;
        }
    

        // Update is called once per frame
        private void Update()
        {
            // Call the PositionElement function
            _camneraPlaneService.PositionElement(transform, new Vector2(x, y), normalRotation, tangentialRotation, offset);
        }
    }
}
