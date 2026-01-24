using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.UI;
using _src.Code.UI.Common;
using _src.Code.UI.Common.IconButtons;
using Agora.Core.Dtos;
using Agora.Core.Enums;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace _src.Code.UI.Scenes.Entry
{
    public class LobbyView : CustomVisualElement
    {
        public new class UxmlFactory : UxmlFactory<LobbyView, UxmlTraits> { }
        
        public event Action<GameKey> GameSelection;
        public event Action StartGameClicked;
        public event Action CancelClicked;
        
        // fields
        private Title _title;
        private EnumField _enumField;
        private ScrollView _scrollView;
        private PrimaryButton _startGameButton;
        private CloseIcon _cancelBtn;
        private Label _lobbyId;
        
        /// <summary>
        /// Initialize
        /// </summary>
        protected override void InitializeCore()
        {
            _title = Get<Title>();
            _enumField = Get<EnumField>();
            _scrollView = Get<ScrollView>();
            _startGameButton = Get<PrimaryButton>();
            _cancelBtn = Get<CloseIcon>();
            _lobbyId = Get<Label>("LobbyId");
            
            // setup
            _startGameButton.Text = "Start Game";
            _startGameButton.Clicked += () => StartGameClicked?.Invoke();
            _cancelBtn.Clicked += () => CancelClicked?.Invoke();
            
            _enumField.RegisterValueChangedCallback(evt => 
            {
                GameKey selectedKey = (GameKey)evt.newValue;
                GameSelection?.Invoke(selectedKey);
            });
        }

        public void SetGame(GameKey gameKey)
        {
            _title.Label.text = gameKey.ToString();
            _enumField.value = gameKey;
        }
        
        public void SetLobbyId(string lobbyId)
        {
            _lobbyId.text = lobbyId;
        }
        
        public async Task SetPlayers(List<UserDto> players)
        {
            _scrollView.Clear();
            
            foreach (var player in players)
            {
                await AddPlayer(player);
            }
        }

        public async Task AddPlayer(UserDto player)
        {
            var playerRow = await VisualElementService.Create<PlayerRow>();
            playerRow.SetUsername(player.Username);
    
            // IMPORTANT: Set the element name to the ID. 
            // This acts like an HTML ID, allowing O(1) lookups.
            playerRow.name = player.Id;
        
            _scrollView.Add(playerRow);
        }
        
        public void RemovePlayer(string id)
        {
            var playerRow = _scrollView.Q<PlayerRow>(name: id);
    
            if (playerRow != null)
            {
                playerRow.RemoveFromHierarchy();
            }
        }
    }
}