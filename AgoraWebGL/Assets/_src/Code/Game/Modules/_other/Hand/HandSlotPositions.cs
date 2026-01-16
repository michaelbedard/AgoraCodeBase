using System;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Services;
using UnityEngine;
using Zenject;

namespace _src.Code.Game.Modules._other.Hand
{
    public class HandSlotPositions : MonoBehaviour
    {
        [Header("Options")]
        public bool isFaceUp;
        public float localScale;
        public float firstAngle = 0f;
        public float secondAngle = 0f;
        public float elevation = 0.5f;
        public float radius = 2f;
        public float angleIncrement = 5f;
        
        [Header("Dev")]
        public bool dev;
        public GameObject positionPrefab;
        private GameObject _centerOfRotationObject;
        private GameObject _handPositionObject;
        
        private Transform[] _slotTransforms;
        private ICameraPlaneService _cameraPlaneService;
        private IGameModuleService _gameModuleService;
        private Camera _camera;
        
        private float _viewPlaneWidth;
        private float _viewPlaneHeight;

        private int _handStartIndex = 0;
        private int _handEndIndex = 0;
        private int _numberOfSlots = 0;
        private bool _hasRemovedSlot = false;
        
        // PROPERTIES
        
        public Transform[] SlotTransforms
        {
            get { return _slotTransforms; }
        }
        
        // INIT
        
        [Inject]
        public void Construct(ICameraPlaneService cameraPlaneService, IGameModuleService gameModuleService)
        {
            _cameraPlaneService = cameraPlaneService;
            _gameModuleService = gameModuleService;
        }

        public void Start()
        {
            _camera = Camera.main;

            if (dev)
            {
                _centerOfRotationObject = Instantiate(positionPrefab);
                _handPositionObject = Instantiate(positionPrefab);
            }
            
            _cameraPlaneService = ServiceLocator.GetService<ICameraPlaneService>();
            _gameModuleService = ServiceLocator.GetService<IGameModuleService>();
            
            UpdateCameraViewDimensions();
            UpdateSlotPositions();

        }

        // METHODS
        
        public void AddSlot()
        {
            _numberOfSlots += 1;

            if (_numberOfSlots <= 10)
            {
                // we have no cards in stack
                _handStartIndex = 0;
                _handEndIndex = _numberOfSlots;
            }
            else
            {
                // there is some stack as we have more than 10 cards
                if (_handEndIndex - _handStartIndex > 9)
                {
                    // init index as it wasn't properly set
                    _handStartIndex = 0;
                    _handEndIndex = 9;
                }
            }
        }
        
        public void RemoveSlot()
        {
            if (_numberOfSlots == 0)
            {
                throw new Exception("Cannot remove a slot");
            }
            
            _numberOfSlots -= 1;

            if (_numberOfSlots < _handEndIndex)
            {
                // we were currently viewing last card
                _handEndIndex = _numberOfSlots;
                _handStartIndex = Mathf.Max(0, _handEndIndex - 9);
                
                if (_handStartIndex - 1 <= 0)
                {
                    _handStartIndex = 0;
                }
            }
        }
        
        public void MoveSlotsToTheLeft()
        {
            if (_numberOfSlots <= 10 || _handEndIndex == _numberOfSlots)
            {
                throw new Exception("Cannot move further to the right");
            }
            
            _handEndIndex = Mathf.Min(_numberOfSlots, _handEndIndex + 8); // +8 as we are seeing 8 at the time
            
            // check edge case where there is no more cards to the right
            if (_handEndIndex + 1 >= _numberOfSlots)
            {
                _handEndIndex = _numberOfSlots;
                _handStartIndex = _numberOfSlots - 9;
            }
            else
            {
                _handStartIndex = _handEndIndex - 8; // not 8 as its inclusive
            }
        }
        
        public void MoveSlotsToTheRight()
        {
            if (_numberOfSlots <= 10 || _handStartIndex == 0)
            {
                throw new Exception("Cannot move further to the left");
            }
            
            _handStartIndex = Mathf.Max(0, _handStartIndex - 8); // +8 as we are seeing 8 at the time
            
            // check edge case where there are no more card to the left
            if (_handStartIndex - 1 <= 0)
            {
                _handStartIndex = 0;
                _handEndIndex = 9;
            }
            else
            {
                _handEndIndex = _handStartIndex + 8; // not 8 as its inclusive
            }
        }

        public Transform GetTransformInFront(int index, float distance, bool faceUp, bool straight)
        {
            var inFrontTransform = new GameObject("InFrontTransform").transform;
            var centerOfRotation = GetCenterOfRotation();
            
            // in rad
            var angleIncrementRad = angleIncrement * Mathf.Deg2Rad;
            var totalAngleRad = angleIncrement * Mathf.Deg2Rad * (Mathf.Min(10, _numberOfSlots) - 1);
            var angleDifferenceRad = GetAngleDifferenceRad(angleIncrementRad, totalAngleRad, index, false);
            
            var x = centerOfRotation.x - Mathf.Cos(secondAngle * Mathf.Deg2Rad + angleDifferenceRad) * (radius + distance);
            var y = centerOfRotation.y - Mathf.Sin(secondAngle * Mathf.Deg2Rad + angleDifferenceRad) * (radius + distance);
            
            var normalRotation = straight ? 0 : secondAngle + 90;
            var tangentialRotation = faceUp ? 180 : 0;
                
            _cameraPlaneService.PositionElement(inFrontTransform, new Vector2(x, y), -normalRotation, tangentialRotation);
            inFrontTransform.localScale = new Vector3(localScale, localScale, localScale);

            return inFrontTransform;
        }
        
        public int UpdateSlotPositions(bool removeSlot = false, bool verbose = false)
        {
            // manage empty slots
            if (removeSlot && !_hasRemovedSlot)
            {
                RemoveSlot();
                _hasRemovedSlot = true;
            }

            if (!removeSlot && _hasRemovedSlot)
            {
                AddSlot();
                _hasRemovedSlot = false;
            }
            
            // logic
            InitializeSlotTransforms(_numberOfSlots);
            
            var centerOfRotation = GetCenterOfRotation();

            // in rad
            var angleIncrementRad = angleIncrement * Mathf.Deg2Rad;
            var totalAngleRad = angleIncrement * Mathf.Deg2Rad * (Mathf.Min(10, _numberOfSlots) - 1);
            
            if (verbose)
            {
                Debug.Log($"{_numberOfSlots} slots, with view at {_handStartIndex} : {_handEndIndex}");
            }
            
            // position each card accordingly
            for (var i = 0; i < _numberOfSlots; i++)
            {
                var angleDifferenceRad = GetAngleDifferenceRad(angleIncrementRad, totalAngleRad, i, verbose);
                
                if (verbose)
                {
                    Debug.Log($"angleIncrementDeg for {i} : {angleDifferenceRad * Mathf.Rad2Deg}");
                }
                
                var x = centerOfRotation.x - Mathf.Cos(secondAngle * Mathf.Deg2Rad + angleDifferenceRad) * radius;
                var y = centerOfRotation.y - Mathf.Sin(secondAngle * Mathf.Deg2Rad + angleDifferenceRad) * radius;

                var normalRotation = secondAngle + angleDifferenceRad * Mathf.Rad2Deg + 90;
                var tangentialRotation = isFaceUp ? 0 : 180;
                var offset = - i * 0.001f;
                
                _cameraPlaneService.PositionElement(_slotTransforms[i], new Vector2(x, y), -normalRotation, tangentialRotation, offset);
                _slotTransforms[i].localScale = new Vector3(localScale, localScale, localScale);

                if (dev)
                {
                    // foreach (var t in _slotTransforms)
                    // {
                    //     var gm = _gameModuleService.InstantiateGameModuleAsync<ICard>(null);
                    //     var gmTransform = gm.Transform;
                    //     
                    //     gmTransform.SetParent(t);
                    //     gmTransform.localPosition = new Vector3(0f, 0f, 0f);
                    //     gmTransform.eulerAngles = t.eulerAngles;
                    //     gmTransform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    // }
                }
            }

            return _numberOfSlots;
        }

        private float GetAngleDifferenceRad(float angleIncrementRad, float totalAngleRad, int i, bool verbose)
        {
            float angleDifferenceRad;
            if (i < _handStartIndex)
            {
                // stacked card left-side
                var numberOfCardInStack = _handStartIndex;
                var angleIncrementInStackRad = angleIncrementRad / numberOfCardInStack;

                angleDifferenceRad = angleIncrementInStackRad * i  - totalAngleRad / 2;

            }
            else if (i >= _handEndIndex)
            {
                // stacked card right-side
                var numberOfCardInStack = _numberOfSlots - _handEndIndex;
                var angleIncrementInStackRad = angleIncrementRad / numberOfCardInStack;
                var indexInStack = i - _handEndIndex;
                    
                angleDifferenceRad = 8 * angleIncrementRad + angleIncrementInStackRad * (indexInStack + 1)  - totalAngleRad / 2;

                if (verbose)
                {
                    Debug.Log("over : " + i + " with diff " + angleDifferenceRad * Mathf.Rad2Deg);
                }
            }
            else
            {
                // is inside hand view
                    
                var indexInHand = i - _handStartIndex;

                // check if there is cards to the left
                if (_handStartIndex > 0)
                {
                    indexInHand += 1;
                }
                    
                angleDifferenceRad = angleIncrementRad * indexInHand  - totalAngleRad / 2;
            }

            return angleDifferenceRad;
        }
        
        public Vector2 GetCenterOfRotation()
        {
            // find the position of the hand
            var r = GetDistanceFromCenterToEdge(
                _viewPlaneWidth - 2 * elevation, 
                _viewPlaneHeight - 2 * elevation, 
                firstAngle * Mathf.Deg2Rad
            );

            var handPosition = new Vector2(
                Mathf.Cos(firstAngle * Mathf.Deg2Rad) * r,
                Mathf.Sin(firstAngle * Mathf.Deg2Rad) * r
            );
            
            var centerOfRotation = new Vector2(
                handPosition.x + Mathf.Cos(secondAngle * Mathf.Deg2Rad) * radius, 
                handPosition.y + Mathf.Sin(secondAngle * Mathf.Deg2Rad) * radius
            );
            
            if (dev)
            {
                _cameraPlaneService.PositionElement(_handPositionObject.transform, new Vector2(handPosition.x, handPosition.y), 0, 0);
                _cameraPlaneService.PositionElement(_centerOfRotationObject.transform, new Vector2(centerOfRotation.x, centerOfRotation.y), 0, 0);
            }

            return centerOfRotation;
        }

        // SLOT UTILS
        
        private void InitializeSlotTransforms(int numberOfSlot)
        {

            if (_slotTransforms != null)
            {
                foreach (var slot in _slotTransforms)
                {
                    Destroy(slot.gameObject);
                }
            }
            
            // add fresh ones
            _slotTransforms = new Transform[numberOfSlot]; 
            
            for (var i = 0; i < numberOfSlot; i++)
            {
                var slotObject = new GameObject("Slot_" + i);
                _slotTransforms[i] = slotObject.transform;
                _slotTransforms[i].SetParent(transform);
            }
        }
        
        private float GetDistanceFromCenterToEdge(float width, float height, float angleRad)
        {
            var horizontalIntersect = float.MaxValue;
            var verticalIntersect = float.MaxValue;

            if (Mathf.Abs(Mathf.Cos(angleRad)) > 0.0001) // Checking for very small values
                horizontalIntersect = Mathf.Abs((width/2) / Mathf.Cos(angleRad));
            if (Mathf.Abs(Mathf.Sin(angleRad)) > 0.0001) // Checking for very small values
                verticalIntersect = Mathf.Abs((height/2) / Mathf.Sin(angleRad));

            return Mathf.Min(horizontalIntersect, verticalIntersect);
        }
        
        private void UpdateCameraViewDimensions()
        {
            var halfFovRad = (_camera.fieldOfView * 0.5f) * Mathf.Deg2Rad; // Convert half FOV to radians
            var aspectRatio = _camera.aspect; // Get the aspect ratio of the camera
            
            _viewPlaneHeight = 2 * _cameraPlaneService.GetDistanceFromClippingPlane() * Mathf.Tan(halfFovRad);
            _viewPlaneWidth = _viewPlaneHeight * aspectRatio;
        }
    }
}