using System.Collections.ObjectModel;
using _src.Code.Core.Interfaces.GameModules.Other;
using _src.Code.Core.Utility.MVC;
using DG.Tweening;
using UnityEngine;

namespace _src.Code.Core.Interfaces.GameModules
{
    public interface IZone : IBaseGameModule
    {
        // Properties
        Canvas Canvas { get; }
        ObservableCollection<ICard> Cards { get; set; }
        ObservableCollection<IMarker> Markers { get; set; }
        ObservableCollection<IToken> Tokens { get; set; }
        
        // Methods
        Transform AddCard(ICard card);
        void RemoveCard(ICard card);
        Transform AddMarker(IMarker marker);
        void RemoveMarker(IMarker marker);
        Transform AddToken(IToken token);
        void RemoveToken(IToken token);

        Sequence UpdateObjectPositions(IBaseController objectToIgnore = null, bool instantaneous = false);
    }
}