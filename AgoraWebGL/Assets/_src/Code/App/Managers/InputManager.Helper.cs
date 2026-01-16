using System.Collections.Generic;
using System.Linq;
using _src.Code.Core;
using _src.Code.Core.Actors;
using _src.Code.Core.Signals.Inputs;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _src.Code.App.Managers
{
    public partial class InputManager
    {
        // state
        private InputObject _mouseOverObject;
        private InputObject _targetObject;
        private Vector2 _startPosition;
        private bool _mouseDown;
        private bool _isDragging;
        private bool _isHovering;
        
        // coroutine
        private Coroutine _draggingCoroutine;
        private Coroutine _hoverCoroutine;
        
        // update
        private Vector2 _mousePosition;
        private InputObject _objectAtMousePosition;
        private List<InputObject> _objectListAtMousePosition;
            
        private void UpdateObjectAtMousePosition()
        {
            _mousePosition = Mouse.current.position.ReadValue();

            // objectListAtMousePosition
            if (Camera.main == null)
            {
                _objectListAtMousePosition = new List<InputObject>();
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(_mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray);

                // Filter and sort the colliders by distance
                List<InputObject> inputObjects = hits
                    .OrderBy(hit => hit.distance) // Sort by distance
                    .Select(hit => GetInputObject(hit.collider)) // Select the inputObject
                    .Where(obj => obj != null) // Non-null values
                    .ToList();

                _objectListAtMousePosition = inputObjects; // Returns an empty list if no colliders are found
            }
            
            // objectAtMousePosition
            _objectAtMousePosition = _objectListAtMousePosition.Count > 0 
                ? _objectListAtMousePosition[0]
                : null;
            
            // mouseOverObject
            if (MouseOverUI)
            {
                if (_mouseOverObject != null)
                {
                    // exit
                    _mouseOverObject.OnExit?.Invoke();
                    _mouseOverObject = null;
                }
            }
            else
            {
                if (_mouseOverObject != _objectAtMousePosition)
                {
                    // exit
                    if (_mouseOverObject != null)
                    {
                        _mouseOverObject.OnExit?.Invoke();
                    }
                
                    // enter
                    if (_objectAtMousePosition != null)
                    {
                        _objectAtMousePosition.OnEnter?.Invoke();
                    }

                    _mouseOverObject = _objectAtMousePosition;
                }
            }
        }
        
        private void UpdateCursor()
        {
            // dragging overrite all
            if (_isDragging)
            {
                Cursor.SetCursor(Globals.Instance.CursorTextureDragging, Vector2.zero, CursorMode.Auto);
                return;
            }
            
            // UI
            if (MouseOverUI)
            {
                return;
            }
            
            // mouse over object
            if (_mouseOverObject != null)
            {
                if (_mouseOverObject.CanBeClick)
                {
                    // clickable cursor
                    Cursor.SetCursor(Globals.Instance.CursorTextureClickable, Vector2.zero, CursorMode.Auto);
                } 
                else if (_mouseOverObject.CanBeDrag)
                {
                    // draggable cursor
                    Cursor.SetCursor(Globals.Instance.CursorTextureDraggable, Vector2.zero, CursorMode.Auto);
                }
                else
                {
                    // default cursor
                    Cursor.SetCursor(Globals.Instance.CursorTextureDefault, Vector2.zero, CursorMode.Auto);
                }
            }
            else
            {
                // default cursor
                Cursor.SetCursor(Globals.Instance.CursorTextureDefault, Vector2.zero, CursorMode.Auto);
            }
        }
        
        private void OnClickStarted()
        {
            _targetObject = _objectAtMousePosition;
            _startPosition = _mousePosition;
            _mouseDown = true;
        }
        
        private void OnClickUpdate()
        {
            if (_isDragging)
            {
                var draggedOverObject = GetObjectAtMousePositionExcept(_targetObject);
                
                // drag update
                if (_targetObject != null && _targetObject.CanBeDrag)
                {
                    _targetObject.OnDragUpdate?.Invoke(draggedOverObject, _mousePosition);
                }
                
                SignalBus.Fire(new DragUpdateSignal(_targetObject, draggedOverObject, _mousePosition));
            }
            else
            {
                // return if over UI
                if (MouseOverUI)
                {
                    return;
                }
                
                // check if we should consider a drag
                if (Vector2.Distance(_mousePosition, _startPosition) > 5f)
                {
                    _isDragging = true;
                    
                    // drag start
                    if (_targetObject != null && _targetObject.CanBeDrag)
                    {
                        _targetObject.OnDragStart?.Invoke(_startPosition);
                        _targetObject.IsBeingDrag = true;
                    }
                    
                    SignalBus.Fire(new DragStartSignal(_targetObject, _mousePosition));
                }
            }
        }
        
        private void OnClickCanceled()
        {
            if (_isDragging)
            {
                var droppedOnObjects = GetInputObjectListAtMousePositionExcept(_targetObject);

                // drag end
                if (_targetObject != null && _targetObject.CanBeDrag)
                {
                    _targetObject.OnDragEnd?.Invoke(droppedOnObjects, _mousePosition);
                    _targetObject.IsBeingDrag = false;
                }
                
                SignalBus.Fire(new DragEndSignal(_targetObject, droppedOnObjects, _mousePosition));
            }
            else
            {
                // if NOT over UI
                if (!MouseOverUI)
                {
                    // check if we are over the same collider as the start
                    if (_targetObject != null && _targetObject == _objectAtMousePosition)
                    {
                        // click
                        if (_targetObject.CanBeClick)
                        {
                            _targetObject.OnClick?.Invoke();
                        }
                    
                        SignalBus.Fire(new ClickSignal(_targetObject));
                    } 
                    else
                    {
                        SignalBus.Fire(new ClickSignal(null));
                    }
                }
            }
            
            // Reset state
            _targetObject = null;
            _mouseDown = false;
            _isDragging = false;
        }

        private void OnMouseMove(Vector2 mousePosition)
        {
            if (MouseOverUI || _mouseDown) return;

            if (_targetObject != _objectAtMousePosition)
            {
                // hover end
                if (_targetObject != null && _targetObject.CanBeHover)
                {
                    _targetObject.OnHoverEnd?.Invoke();
                }
                
                SignalBus.Fire(new HoverEndSignal(_targetObject));
                
                // hover start
                if (_objectAtMousePosition != null && _objectAtMousePosition.CanBeHover)
                {
                    _objectAtMousePosition.OnHoverStart?.Invoke();
                }
                
                SignalBus.Fire(new HoverStartSignal(_objectAtMousePosition));
                
                // set target object
                _targetObject = _objectAtMousePosition;
            }
        }
        
        private void OnScroll(Vector2 scrollDelta)
        {
            // return if over UI
            if (MouseOverUI)
            {
                return;
            }
            
            // scroll
            if (_objectAtMousePosition != null && _objectAtMousePosition.CanBeScroll)
            {
                _objectAtMousePosition.OnScroll?.Invoke(scrollDelta);
            }
            
            SignalBus.Fire(new ScrollSignal(_objectAtMousePosition, scrollDelta));
        }
        
        private InputObject GetObjectAtMousePositionExcept(InputObject objectToIgnore)
        {
            if (_objectAtMousePosition == objectToIgnore)
            {
                return _objectListAtMousePosition.FirstOrDefault(o => o != objectToIgnore);
            }
            
            return _objectAtMousePosition;
        }
        
        private List<InputObject> GetInputObjectListAtMousePositionExcept(InputObject objectToIgnore)
        {
            return _objectListAtMousePosition.Where(o => o != objectToIgnore).ToList();
        }

        [CanBeNull]
        private InputObject GetInputObject(Collider collider)
        {
            return collider.gameObject.GetComponent<InputObject>() 
                        ?? collider.gameObject.GetComponentInChildren<InputObject>() 
                        ?? collider.gameObject.GetComponentInParent<InputObject>();
        }
    }
}