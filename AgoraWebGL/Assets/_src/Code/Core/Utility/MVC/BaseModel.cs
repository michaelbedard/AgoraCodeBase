using System.ComponentModel;
using System.Runtime.CompilerServices;
using _src.Code.Core.Actors;
using UnityEngine;

namespace _src.Code.Core.Utility.MVC
{
    public class BaseModel : InputObject, INotifyPropertyChanged
    {
        public Transform Transform => transform;
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}