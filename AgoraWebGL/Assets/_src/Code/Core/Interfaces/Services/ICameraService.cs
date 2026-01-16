using System.Collections.Generic;
using _src.Code.Core.Actors;
using UnityEngine;

namespace _src.Code.Core.Interfaces.Services
{
    public interface ICameraService
    {
        Camera GetMainCamera();
        void Zoom();
        void Center();
        List<InputObject> GetObjectsAtMousePosition();
    }
}