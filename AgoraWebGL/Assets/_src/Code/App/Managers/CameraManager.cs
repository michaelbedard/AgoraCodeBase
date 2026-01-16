using _src.Code.Core.Interfaces.Managers;
using _src.Code.Core.Signals.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _src.Code.App.Managers
{
    // Meant to be attached to the camera rig
    public class CameraManager : MonoBehaviour, ICameraManager
    {
        // variables
        private Vector3 _targetPosition;
        private float _zoomHeight;
        private Vector3 _horizontalVelocity;
        private Vector3 _lastPosition;
        private Vector3 _startDrag;
        private float _speed;
        
        // initial position
        private Vector3 _initialCameraRigPosition;
        private Vector3 _initialCameraPosition;
        
        // horizontal motion const - moving
        [Header("moving")]
        [SerializeField]
        private float MaxSpeed = 5f;
        [SerializeField]
        private float Acceleration = 10f;
        [SerializeField]
        private float Damping = 15f;
        [SerializeField]
        private float MaxLeft = 10f;
        [SerializeField]
        private float MaxRight = 10f;
        [SerializeField]
        private float MaxTop = 10f;
        [SerializeField]
        private float MaxBottom = 10f;

        // Vertical motion const - zooming
        [Header("zooming")]
        [SerializeField]
        private float StepSize = 1f;
        [SerializeField]
        private float ZoomDampening = 7.5f;
        [SerializeField]
        private float MinHeight = 5f;
        [SerializeField]
        private float MaxHeight = 50f;
        [SerializeField]
        private float ZoomSpeed = 2f;
        
        // services
        private SignalBus _signalBus;
        
        // other
        private bool _isInitialized;
        private Camera _mainCamera;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        // Init

        public void Initialize(Vector3 initialPosition)
        {
            _mainCamera = Camera.main;
            _mainCamera.transform.localPosition = initialPosition;
            
            _initialCameraRigPosition = transform.position;
            
            _zoomHeight = initialPosition.y;
            _initialCameraPosition = initialPosition;
            
            if (!_isInitialized)
            {
                _signalBus.Subscribe<DragStartSignal>(OnStartDragAction);
                _signalBus.Subscribe<DragUpdateSignal>(OnUpdateDragAction);
                _signalBus.Subscribe<ScrollSignal>(OnScrollAction);
            
                _isInitialized = true;
            }
        }
        
        // Mono
        
        // private void Awake()
        // {
        //     Instance = this;
        // }

        private void Update()
        {
           if (!_isInitialized) return;
            
            UpdateVelocity();
            UpdateCameraPosition();
            UpdateBasePosition();
        }
        
        // UTILS
        
        private void OnStartDragAction(DragStartSignal signal)
        {
            if (signal.InputObject != null && signal.InputObject.CanBeDrag) return; // should be smt we cant drag
            
            var plane = new Plane(Vector3.up, Vector3.zero);
            var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            
            if (plane.Raycast(ray, out var distance))
            {
                _startDrag = ray.GetPoint(distance);
            }
        }
        
        private void OnUpdateDragAction(DragUpdateSignal signal)
        {
            if (signal.InputObject != null && signal.InputObject.CanBeDrag) return; // should be smt we cant drag
            
            var plane = new Plane(Vector3.up, Vector3.zero);
            var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            
            if (plane.Raycast(ray, out var distance))
            {
                _targetPosition += _startDrag - ray.GetPoint(distance);
            }
        }

        private void OnScrollAction(ScrollSignal signal)
        {
            var value = -signal.ScrollDelta.y / 100f;

            if (Mathf.Abs(value) > 0.1f)
            {
                _zoomHeight = _mainCamera.transform.localPosition.y + value * StepSize;
                if (_zoomHeight < MinHeight)
                    _zoomHeight = MinHeight;
                else if (_zoomHeight > MaxHeight)
                    _zoomHeight = MaxHeight;
            }
        }

        private void UpdateVelocity()
        {
            var position = transform.position;
            _horizontalVelocity = (position - _lastPosition) / Time.deltaTime;
            _horizontalVelocity.y = 0;
            _lastPosition = position;
        }
        
        private void UpdateCameraPosition()
        {
            var cameraLocalPosition = _mainCamera.transform.localPosition;
            
            var zoomTarget = new Vector3(cameraLocalPosition.x, _zoomHeight,
                cameraLocalPosition.z);

            zoomTarget -= ZoomSpeed * (_zoomHeight - cameraLocalPosition.y) * Vector3.forward;

            _mainCamera.transform.localPosition = Vector3.Lerp(cameraLocalPosition, zoomTarget, Time.deltaTime * ZoomDampening);
        }

        private void UpdateBasePosition()
        {
            if (_targetPosition.sqrMagnitude > 0.1f)
            {
                _speed = Mathf.Lerp(_speed, MaxSpeed, Time.deltaTime * Acceleration);
                transform.position += _targetPosition * _speed * Time.deltaTime;
            }
            else
            {
                _horizontalVelocity = Vector3.Lerp(_horizontalVelocity, Vector3.zero, Time.deltaTime * Damping);
                transform.position += _horizontalVelocity * Time.deltaTime;
            }

            // Clamp the camera's position within the defined boundaries
            ClampCameraPosition();

            _targetPosition = Vector3.zero;
        }
        
        private void ClampCameraPosition()
        {
            // Get the current position of the camera rig
            Vector3 currentPosition = transform.position;

            // Clamp the X position (left and right boundaries)
            currentPosition.x = Mathf.Clamp(currentPosition.x, -MaxLeft, MaxRight);

            // Clamp the Z position (top and bottom boundaries)
            currentPosition.z = Mathf.Clamp(currentPosition.z, -MaxBottom, MaxTop);

            // Apply the clamped position back to the camera rig
            transform.position = currentPosition;
        }
        
        private Vector3 GetCameraRight()
        {
            var right = _mainCamera.transform.right;
            right.y = 0;
            return right;
        }
        
        private Vector3 GetCameraForward()
        {
            var forward = _mainCamera.transform.forward;
            forward.y = 0;
            return forward;
        }
        
        public void ResetCameraPosition()
        {
            _lastPosition = Vector3.zero;
            _targetPosition = Vector3.zero;
            _horizontalVelocity = Vector3.zero;
            _speed = 0;
            
            transform.position = _initialCameraRigPosition;
            _mainCamera.transform.localPosition = _initialCameraPosition;
            _zoomHeight = _initialCameraPosition.y;
        }
    }
}