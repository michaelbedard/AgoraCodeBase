using System.Collections.ObjectModel;
using System.Threading.Tasks;
using _src.Code.Core.Interfaces.GameModules.Other;
using UnityEngine;

namespace _src.Code.Core.Interfaces.GameModules
{
    public interface IToken : IBaseGameModule
    {
        IZone ParentZone { get; set; }
        
        public Transform Transform { get; }
        public void SetLabel(string label);
        public void SetColor(Color color);
    }
}