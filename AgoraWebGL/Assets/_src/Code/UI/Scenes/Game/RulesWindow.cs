using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.UI;
using _src.Code.UI.Common;
using _src.Code.UI.Common.IconButtons;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace _src.Code.UI.Scenes.Game
{
    public class RulesWindow : CustomVisualElement, IVisualElement
    {
        public new class UxmlFactory : UxmlFactory<RulesWindow, UxmlTraits> { }
        public string Path => "Assets/_src/UI/Windows/RulesWindow.uxml";
        
        private Title _title;
        private VisualElement _rulesContent;
        private VisualElement _creditsContent;
        private VisualElement _leftPage;
        private VisualElement _rightPage;
        private Button _leftBtn;
        private Button _rightBtn;
        private CloseIcon _closeBtn;
        private SubTitle _creditsSubTitle;
        private Label _creditsLabel;
        private SubTitle _mainSubTitle;
        private Label _mainLabel;
        private SecondaryButton _switchBtn;
        
        private List<string> _rulesAddresses;
        private int _currentIndex;
        private bool _isShowingCredits;
        
        protected override void InitializeCore()
        {
            _title = Get<Title>();
            _rulesContent = Get("Rules");
            _creditsContent = Get("Credits");
            _leftPage = Get("LeftPage");
            _rightPage = Get("RightPage");
            _leftBtn = Get<Button>("LeftButton");
            _rightBtn = Get<Button>("RightButton");
            _creditsSubTitle = Get<SubTitle>("GameCredits");
            _creditsLabel = Get<Label>("GameCreditsLabel");
            _mainSubTitle = Get<SubTitle>("MainCredits");
            _mainLabel = Get<Label>("MainCreditsLabel");
            _switchBtn = Get<SecondaryButton>();
            _closeBtn = Get<CloseIcon>();
            
            _creditsSubTitle.Label.text = "Music Attributions";
            _creditsLabel.text = "Work: Goodhaven \n" +
                                 "Author: Tim  \n" +
                                 "Source: https://tabletopaudio.com/  \n" +
                                 "License: CC BY-NC-ND 4.0 \n \n" +
                                 "Work: River Town \n" +
                                 "Author: Tim  \n" +
                                 "Source: https://tabletopaudio.com/  \n" +
                                 "License: CC BY-NC-ND 4.0 \n \n" +
                                 "Work: Lord of Bones \n" +
                                 "Author: Tim  \n" +
                                 "Source: https://tabletopaudio.com/  \n" +
                                 "License: CC BY-NC-ND 4.0 \n \n";
            _mainSubTitle.Label.text = "Assets Attributions";
            _mainLabel.text = "Work: Reigns Inspired Deck Of Playing Cards \n" +
                              "Author: kato  \n" +
                              "Source: https://underscorekato.itch.io/reigns-inspired-deck-of-playing-cards \n" +
                              "License: CC BY-SA 4.0 \n \n" +
                              "Work: Short Tables \n" +
                              "Author: Law C. Esper  \n" +
                              "Source: https://monchop.itch.io/short-tales \n" +
                              "License: CC-BY 4.0 \n \n";
            _leftBtn.clicked += MoveLeft;
            _rightBtn.clicked += MoveRight;
            _closeBtn.Clicked += Hide;
            _switchBtn.Clicked += Switch;
            
            ShowRules();
            
            _currentIndex = 0;
        }

        public async Task SetRules(string title, List<string> rulesAddresses)
        {
            _rulesAddresses = rulesAddresses;
            _title.Label.text = $"{title}'s Rules";
            
            UpdatePages();
            UpdateButtonsVisibility();
        }

        private void Switch()
        {
            if (_isShowingCredits)
            {
                ShowRules();
            } 
            else
            {
                ShowCredits();
            }
        }

        private void ShowRules()
        {
            _rulesContent.style.display = DisplayStyle.Flex;
            _creditsContent.style.display = DisplayStyle.None;
            _switchBtn.Text = "Credits";
            _isShowingCredits = false;
        }
        
        private void ShowCredits()
        {
            _rulesContent.style.display = DisplayStyle.None;
            _creditsContent.style.display = DisplayStyle.Flex;
            _switchBtn.Text = "Rules";
            _isShowingCredits = true;
        }
        
        private void MoveRight()
        {
            if (_currentIndex < _rulesAddresses.Count - 2)
            {
                _currentIndex += 2;
                UpdatePages();
                UpdateButtonsVisibility();
            }
        }

        private void MoveLeft()
        {
            if (_currentIndex > 0)
            {
                _currentIndex -= 2;
                UpdatePages();
                UpdateButtonsVisibility();
            }
        }

        private async void UpdatePages()
        {
            try
            {
                if (_currentIndex < _rulesAddresses.Count)
                {
                    _leftPage.style.backgroundImage = new StyleBackground(
                        await Addressables.LoadAssetAsync<Sprite>(_rulesAddresses[_currentIndex]).Task
                    );
                }
                else
                {
                    _leftPage.style.backgroundImage = null;
                }

                if (_currentIndex + 1 < _rulesAddresses.Count)
                {
                    _rightPage.style.backgroundImage = new StyleBackground(
                        await Addressables.LoadAssetAsync<Sprite>(_rulesAddresses[_currentIndex + 1]).Task
                    );
                }
                else
                {
                    _rightPage.style.backgroundImage = null;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private void UpdateButtonsVisibility()
        {
            _leftBtn.SetEnabled(_currentIndex > 0);
            _rightBtn.SetEnabled(_currentIndex + 2 < _rulesAddresses.Count);
        }
    }
}