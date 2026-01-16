using System;
using System.Collections.Generic;
using System.Linq;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Services;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _src.Code.App.Services
{
    public class CameraService : ICameraService
    {
        private readonly Camera _mainCamera;
        private readonly Transform _cameraRigTransform;

        [Inject]
        public CameraService(Camera mainCamera, Transform cameraRigTransform)
        {
            _mainCamera = mainCamera;
            _cameraRigTransform = cameraRigTransform;
        }

        /// <summary>
        /// GetMainCamera
        /// </summary>
        public Camera GetMainCamera()
        {
            return _mainCamera;
        }

        public void Zoom()
        {
            throw new NotImplementedException();
        }
        
        public void Center()
        {
            throw new NotImplementedException();
        }

        public List<InputObject> GetObjectsAtMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit[] hits = Physics.RaycastAll(ray);

            // Filter and sort the colliders by distance
            List<InputObject> inputObjects = hits
                .OrderBy(hit => hit.distance) // Sort by distance
                .Select(hit => GetInputObject(hit.collider)) // Select the inputObject
                .Where(obj => obj != null) // Non-null values
                .ToList();
            
            return inputObjects;
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