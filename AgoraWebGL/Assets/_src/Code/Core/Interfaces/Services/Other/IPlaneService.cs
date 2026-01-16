using UnityEngine;

namespace _src.Code.Core.Interfaces.Services.Other
{
    public interface IPlaneService
    {
        Vector2 GetTransformPlaneCoordinate(Vector3 worldPosition);
        Vector3 TransformPoint(Vector2 planeCoordinates);
    }
}