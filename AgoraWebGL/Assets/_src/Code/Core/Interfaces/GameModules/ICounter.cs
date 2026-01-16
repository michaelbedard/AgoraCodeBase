using System.Threading.Tasks;
using _src.Code.Core.Interfaces.GameModules.Other;

namespace _src.Code.Core.Interfaces.GameModules
{
    public interface ICounter : IBaseGameModule        

    {
        // Properties
        int Value { get; set; }
        
        // Methods
    }
}