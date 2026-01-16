using _src.Code.Core.Interfaces.GameModules.Other;

namespace _src.Code.Core.Interfaces.GameModules
{
    public interface IMarker: IBaseGameModule
    {
        // Properties
        IZone ParentZone { get; set; }
        
        // Methods
    }
}