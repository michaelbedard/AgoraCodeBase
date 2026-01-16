using System;
using System.Collections.Generic;
using System.Text;
using _src.Code.Core.Actors;
using _src.Code.UI.Common;
using UnityEngine.UIElements;

namespace _src.Code.UI.Scenes.Game
{
    public class EndGameWindow : CustomVisualElement
    {
        // 1. No Path string here.
        // 2. No ServiceLocator.
        
        // UI Elements
        private Label _endGameMessage;
        private Label _winners;
        private PrimaryButton _playAgainBtn;
        private ClickableText _exitBtn;

        // Events for the Logic (Handler) to listen to
        public event Action OnPlayAgainClicked;
        public event Action OnExitClicked;

        protected override void InitializeCore()
        {
            _endGameMessage = Get<Label>("EndGameMessage");
            _winners = Get<Label>("Winners");
            _playAgainBtn = Get<PrimaryButton>();
            _exitBtn = Get<ClickableText>();

            // Setup static text
            Get<Title>().Label.text = "Game Over";
            Get<SubTitle>("WinnersSubTitle").Label.text = "Winners";
            _playAgainBtn.Text = "Play Again";
            _exitBtn.Button.text = "Go back to Main Menu";

            // Forward clicks to events
            _playAgainBtn.Clicked += () => OnPlayAgainClicked?.Invoke();
            _exitBtn.Button.clicked += () => OnExitClicked?.Invoke();
        }

        // Pure Visual Logic
        public void Setup(string message, List<string> winnerUsernames)
        {
            _endGameMessage.text = message;
            
            var builder = new StringBuilder();
            for (var i = 0; i < winnerUsernames.Count; i++)
            {
                var position = (i + 1) switch { 1 => "1st", 2 => "2nd", 3 => "3rd", _ => $"{i + 1}th" };
                builder.AppendLine($"{position} : {winnerUsernames[i]}");
            }
            _winners.text = builder.ToString();
        }
    }
}