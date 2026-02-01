using System.Collections.Generic;
using System.Threading.Tasks;
using _src.Code.Core.Interfaces.Managers;
using _src.Code.Core.Interfaces.Services;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _src.Code.Game
{
    public class TurnManager: MonoBehaviour, ITurnManager
    {
        [Inject] private IClientDataService _clientDataService;
        [Inject] private ICharacterService _characterService;
        
        private Transform _characterTransform;
        private string _currentPlayerId;
        private GameObject _currentCharacterObj;
        
        private const float MoveDistance = 5f;
        private const float Duration = 0.5f;
        
        [Inject]
        public void Construct(Transform characterTransform)
        {
            this._characterTransform = characterTransform;
        }

        public async Task UpdateVisual(List<string> playersTakingTurnId)
        {
            var newPlayerId = playersTakingTurnId[0]; // KEEP FOR NOW
            
            if (_currentPlayerId == newPlayerId) return;

            _currentPlayerId = newPlayerId;

            // 2. Handle Old Character Removal
            if (_currentCharacterObj != null)
            {
                var oldChar = _currentCharacterObj;
                // Move down and destroy
                oldChar.transform.DOLocalMoveY(-MoveDistance, Duration)
                    .SetEase(Ease.InBack, 0.5f) // Light bounce on exit
                    .OnComplete(() => Destroy(oldChar));
            }

            // 3. Fetch and Setup New Character
            var player = _clientDataService.Players.Find(p => p.Id == newPlayerId);
            var character = await _characterService.GetCharacterById(player.Avatar, true);
            
            _currentCharacterObj = character;
            character.transform.SetParent(_characterTransform, false);

            // Start position (below screen)
            character.transform.localPosition = new Vector3(0, -MoveDistance, 0);

            // 4. Bouncy Entrance
            character.transform.DOLocalMoveY(0, Duration)
                .SetEase(Ease.OutBack, 1.2f); // "Light" bouncy effect
        }
    }
}