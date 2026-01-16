using UnityEngine;

namespace _src.Code.Game.Modules
{
    public partial class GameModule<TModel>
    {
        public void SetParent(Transform transform)
        {
            Model.transform.SetParent(transform);
        }

        public void DisableCollider()
        {
            foreach (var collider in Colliders)
            {
                collider.enabled = false;
            }
        }
        
        public void EnableCollider()
        {
            foreach (var collider in Colliders)
            {
                collider.enabled = true;
            }
        }
        
        public void Destroy()
        {
            // unsubscribe
            Model.PropertyChanged -= OnModelPropertyChanged;
            UnsubscribeFromSignals();
            
            // destroy game object
            if (Model != null)
            {
                Object.Destroy(Model.Transform.gameObject);
            }
        }
    }
}