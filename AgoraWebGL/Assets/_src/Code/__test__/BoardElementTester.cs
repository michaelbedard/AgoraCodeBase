using _src.Code.Core.Interfaces.Services;
using UnityEngine;
using Zenject;

namespace _src.Code.__test__
{
    public class BoardElementTester : MonoBehaviour
    {
        public float x;
        public float y;
        public float normalRotation = 0f;
        public float tangentialRotation = 0f;
        public float offset = 0f;
    
        private IBoardPlaneService _boardPlaneService;

        [Inject]
        public void Construct(IBoardPlaneService boardPlaneService)
        {
            _boardPlaneService = boardPlaneService;
        }
    

        // Update is called once per frame
        private void Update()
        {
            // Call the PositionElement function
            _boardPlaneService.AddCommonElement(transform, new Vector2(x, y), normalRotation, tangentialRotation, offset);
        }
    }
}
