using System;
using System.Threading.Tasks;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Logic;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace _src.Code.App.Logic.Entry
{
    public partial class EntryLogic : IEntryLogic, IInitializable, IDisposable
    {
        private readonly IVisualElementService _uiService;
        private readonly ILobbyHttpProxy _lobbyHttpProxy;
        private readonly IHubProxy _hubProxy;
        private readonly IClientDataService _clientDataService;
        private readonly ISceneService _sceneService;
        private readonly IGameHubProxy _gameHubProxy;
        private readonly ICharacterService _characterService;
        private readonly Transform _avatarTransform;

        public EntryLogic(
            IVisualElementService uiService,
            ILobbyHttpProxy lobbyHttpProxy,
            IHubProxy hubProxy,
            IClientDataService clientDataService,
            ISceneService sceneService,
            IHubController hubController,
            IGameHubProxy gameHubProxy,
            ICharacterService characterService,
            Transform avatarTransform)
        {
            _uiService = uiService;
            _lobbyHttpProxy = lobbyHttpProxy;
            _hubProxy = hubProxy;
            _clientDataService = clientDataService;
            _sceneService = sceneService;
            _gameHubProxy = gameHubProxy;
            _characterService = characterService;
            _avatarTransform = avatarTransform;
            
            // for now...
            hubController.RegisterEntryLogic(this);
        }

        public async Task DisplayAvatar()
        {
            var character = await _characterService.GetCharacterById(_clientDataService.AvatarId, true);
            character.transform.SetParent(_avatarTransform, false);
            character.transform.localPosition = Vector3.zero;
            
            var animatorController = character.GetComponentInChildren<AnimatorController>();
            animatorController.DoJumpAnimation();
        }
    }
}