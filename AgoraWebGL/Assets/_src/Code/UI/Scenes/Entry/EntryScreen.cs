using System;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.UI;
using _src.Code.UI.Common;
using _src.Code.UI.Common.IconButtons;
using UnityEngine.UIElements;

namespace _src.Code.UI.Scenes.Entry
{
    public class EntryScreen : CustomVisualElement, IVisualElement
    {
        public new class UxmlFactory : UxmlFactory<EntryScreen, UxmlTraits> { }
        
        // 1. The three new events
        public event Action PlayWithFriendsClicked;
        public event Action PlayOnlineClicked;
        public event Action ShopClicked;
        
        // Fields
        private VisualElement _btnContainer;
        
        // 2. Distinct buttons for each action
        private PrimaryButton _playFriendsBtn;
        private PrimaryButton _playOnlineBtn;
        private PrimaryButton _shopBtn;
        
        // Dynamic elements
        private LobbyView _lobbyView;
        private LoadingWidget _loadingWidget; 
        
        protected override void InitializeCore()
        {
            _btnContainer = Get("BtnsContainer");; 

            _playFriendsBtn = Get<PrimaryButton>("PlayWithFriendsBtn");
            _playOnlineBtn = Get<PrimaryButton>("PlayOnlineBtn");
            _shopBtn = Get<PrimaryButton>("ShopBtn");
            
            // Setup Text and Event Listeners
            _playFriendsBtn.Text = "Play with Friends";
            _playFriendsBtn.Clicked += () => PlayWithFriendsClicked?.Invoke();

            _playOnlineBtn.Text = "Play Online";
            _playOnlineBtn.Clicked += () => PlayOnlineClicked?.Invoke();

            _shopBtn.Text = "Shop";
            _shopBtn.Clicked += () => ShopClicked?.Invoke();
        }

        public void ShowLobbyView(LobbyView lobbyView)
        {
            _lobbyView = lobbyView;
            style.display = DisplayStyle.None;

            _lobbyView.AddToRootElement();
        }
        
        public void HideLobbyView()
        {
            if (_lobbyView != null)
            {
                _lobbyView.RemoveFromHierarchy(); 
                _lobbyView = null;
            }
            
            style.display = DisplayStyle.Flex;
        }

        public void ShowLoadingWidget(LoadingWidget loadingWidget)
        {
            _loadingWidget = loadingWidget;
            
            style.display = DisplayStyle.None;
            _loadingWidget.AddToRootElement();
        }

        public void HideLoadingWidget()
        {
            if (_loadingWidget != null)
            {
                _loadingWidget.RemoveFromHierarchy();
                _loadingWidget = null;
            }

            style.display = DisplayStyle.Flex;
        }
        
        // HELPERS
        
        private void SetButtonsDisplay(DisplayStyle btnStyle)
        {
            if (_playFriendsBtn != null) _playFriendsBtn.style.display = btnStyle;
            if (_playOnlineBtn != null) _playOnlineBtn.style.display = btnStyle;
            if (_shopBtn != null) _shopBtn.style.display = btnStyle;
        }
    }
}