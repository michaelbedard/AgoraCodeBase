using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.UI;
using UnityEngine.UIElements;

namespace _src.Code.UI.Scenes.Game
{
    public class GameScreen : CustomVisualElement, IVisualElement
    {
        
        // factory
        public new class UxmlFactory : UxmlFactory<GameScreen, UxmlTraits> { }
        
        private VisualElement _loadingView;
        private VisualElement _endGameView;
        
        /// <summary>
        /// Initialize
        /// </summary>
        protected override void InitializeCore()
        {
            _loadingView = Get("LoadingView");
            _endGameView = Get("EndGameView");
            
            ShowLoadingView();
            HideEndGameView();
        }
        
        public void ShowLoadingView()
        {
            _loadingView.style.display = DisplayStyle.Flex;
        }

        public void HideLoadingView()
        {
            _loadingView.style.display = DisplayStyle.None;
        }
        
        public void ShowEndGameView()
        {
            _endGameView.style.display = DisplayStyle.Flex;
        }

        private void HideEndGameView()
        {
            _endGameView.style.display = DisplayStyle.None;
        }
    }
}