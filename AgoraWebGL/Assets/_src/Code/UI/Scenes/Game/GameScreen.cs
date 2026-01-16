using System.Linq;
using System.Threading.Tasks;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Services;
using _src.Code.Core.Interfaces.UI;
using _src.Code.UI.Common;
using _src.Code.UI.Shared;
using UnityEngine.UIElements;

namespace _src.Code.UI.Scenes.Game
{
    public class GameScreen : CustomVisualElement, IVisualElement
    {
        public string Path { get; } = string.Empty;
        
        // factory
        public new class UxmlFactory : UxmlFactory<GameScreen, UxmlTraits> { }
        
        // fields
        private IGameDataService _gameDataService;
        
        private CircularButton _changeLobbyBtn;
        private CircularButton _changeGameBtn;
        private CircularButton _rulesBtn;

        public SubTitle TopTag { get; set; }
        public SubTitle RightTag { get; set; }
        public SubTitle LeftTag { get; set; }
        public SubTitle TopRightTag { get; set; }
        public SubTitle TopLeftTag { get; set; }
        
        public Description Description { get; set; }
        
        /// <summary>
        /// Initialize
        /// </summary>
        protected override void InitializeCore()
        {
            _gameDataService = ServiceLocator.GetService<IGameDataService>();
            
            _changeLobbyBtn = Get<CircularButton>("ChangeLobbyBtn");
            _changeGameBtn = Get<CircularButton>("ChangeGameBtn");
            _rulesBtn = Get<CircularButton>("RulesBtn");

            TopTag = Get<SubTitle>("TopTag");
            RightTag = Get<SubTitle>("RightTag");
            LeftTag = Get<SubTitle>("LeftTag");
            TopRightTag = Get<SubTitle>("TopRightTag");
            TopLeftTag = Get<SubTitle>("TopLeftTag");

            Description = Get<Description>();

            _changeLobbyBtn.IconAddress = "Assets/_src/Sprites/Icons/icon_speed.png";
            _changeGameBtn.IconAddress = "Assets/_src/Sprites/Icons/icon_treasure.png";
            _rulesBtn.IconAddress = "Assets/_src/Sprites/Icons/icon_scroll.png";

            _changeLobbyBtn.Text = "Exit\nGame";
            _changeLobbyBtn.Clicked += async () => await OnChangeLobbyBtnClick();
            _rulesBtn.Text = "Rules";
            _rulesBtn.Clicked += async () => await OnRulesBtnClick();
            
            // SignalBus.Subscribe<EndGameSignal>(ShowEndGameScreen);
            
            ShowStartGameScreen();
            HideEndGameScreen();
            HideTags();
        }
        
        public void ShowStartGameScreen()
        {
            // _startGameScreen.parent.style.display = DisplayStyle.Flex;
        }

        public void HideStartGameScreen()
        {
            // _startGameScreen.parent.style.display = DisplayStyle.None;
        }
        
        public void ShowEndGameScreen()
        {
            // _endGameScreen.parent.style.display = DisplayStyle.Flex;
            
            // hide btns
            _changeLobbyBtn.parent.style.display = DisplayStyle.None;
            _changeGameBtn.parent.style.display = DisplayStyle.None;
            _rulesBtn.parent.style.display = DisplayStyle.None;
            TopTag.parent.style.display = DisplayStyle.None;
            RightTag.parent.style.display = DisplayStyle.None;
            LeftTag.parent.style.display = DisplayStyle.None;
            TopRightTag.parent.style.display = DisplayStyle.None;
            TopLeftTag.parent.style.display = DisplayStyle.None;
        }

        private void HideEndGameScreen()
        {
            // _endGameScreen.parent.style.display = DisplayStyle.None;
            
            // show btns
            _changeLobbyBtn.parent.style.display = DisplayStyle.Flex;
            _changeGameBtn.parent.style.display = DisplayStyle.Flex;
            _rulesBtn.parent.style.display = DisplayStyle.Flex;
        }
        
        private async Task OnChangeLobbyBtnClick()
        {
            if (_gameDataService.IsAgainstComputer)
            {
                // await SignalBus.Send(new ExitLobbyRequest());
            }
            else
            {
                var yesNoPopup = await ServiceLocator.GetService<IVisualElementService>().GetOrCreate<YesNoPopup>();
                yesNoPopup.Title.Label.text = "Exit Game?";
                yesNoPopup.Message.text = $"This will end the game for all.";
                yesNoPopup.YesButton.Text = "Yes";
                yesNoPopup.NoButton.Text = "No";
                yesNoPopup.YesButton.Clicked += async () =>
                {
                    // await ServiceLocator.GetService<IGameHubProxy>().ExitGame();
                    // await SignalBus.Send(new LoadMainSceneRequest());
                };
                yesNoPopup.NoButton.Button.clicked += async () =>
                {
                    yesNoPopup.Hide();
                };
                
                yesNoPopup.Show();
            }
        }
        
        private async Task OnRulesBtnClick()
        {
            var rulesWindow = await VisualElementService.GetOrCreate<RulesWindow>();
            await rulesWindow.SetRules(_gameDataService.GameTitle, _gameDataService.CompleteRulesAddresses.ToList());
            rulesWindow.Show();
        }


        private void HideTags()
        {
            TopTag.parent.style.display = DisplayStyle.None;
            RightTag.parent.style.display = DisplayStyle.None;
            LeftTag.parent.style.display = DisplayStyle.None;
            TopRightTag.parent.style.display = DisplayStyle.None;
            TopLeftTag.parent.style.display = DisplayStyle.None;
        }
    }
}