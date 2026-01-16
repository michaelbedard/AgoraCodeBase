namespace _src.Code.Core.Utility.Plane
{
    // [ExecuteInEditMode]
    // public class PlaneElement : PlanePositionController
    // {
    //     [Header("Parameters")]
    //     public float xPositionOnPlane;
    //     public float yPositionOnPlane;
    //     public float rotationAroundNormal;
    //     public float rotationPerpendicularToNormal;
    //     public float offset;
    //     public float scale;
    //
    //     [Header("Options")] 
    //     public bool makeChild;
    //     public bool dev;
    //
    //     private void Awake()
    //     {
    //         if (planeTransform == null) return;
    //         
    //         PlaceElementOnPlane(transform, new Vector2(xPositionOnPlane, yPositionOnPlane), rotationAroundNormal, rotationPerpendicularToNormal, offset, scale, false);
    //         if (Application.isPlaying && makeChild)
    //         {
    //             transform.SetParent(planeTransform);
    //         }
    //     }
    //
    //     private void Update()
    //     {
    //         if (!dev) return;
    //         
    //         PlaceElementOnPlane(transform, new Vector2(xPositionOnPlane, yPositionOnPlane), rotationAroundNormal, rotationPerpendicularToNormal, offset, scale, false);
    //     }
    // }
}