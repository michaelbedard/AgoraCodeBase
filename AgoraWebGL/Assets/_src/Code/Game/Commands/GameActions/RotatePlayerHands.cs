// using System.Collections.Generic;
// using System.Linq;
// using _src.Scripts.Core.Interfaces.GameModules;
// using _src.Scripts.Core.Interfaces.Services;
// using _src.Scripts.Core.Signals.Game;
// using _src.Scripts.Game.GameModules;
// using DG.Tweening;
// using UnityEngine;
// using Zenject;
//
// namespace _src.Scripts.Game.GameCommands.GameActions
// {
//     public class RotatePlayerHands : BaseAction
//     {
//         [Inject]
//         public RotatePlayerHands(SignalBus signalBus, IAnimationQueueService animationQueueService, IGameModuleService gameModuleService) 
//             : base(signalBus, animationQueueService, gameModuleService)
//         {
//         }
//
//         protected override void Allow(GameActionSignalSignal signalSignal)
//         {
//             if (signalSignal.Key != nameof(RotatePlayerHands)) return;
//             
//             throw new System.NotImplementedException();
//         }
//
//         protected override void Animate(GameAnimationSignal signal)
//         {
//             if (signal.Key != nameof(RotatePlayerHands)) return;
//             
//             AnimationQueueService.Push(async () =>
//             {
//                 var players = signal.Args.Select(arg => GameModuleService.GetGameModuleById<IPlayer>(arg)).ToList();
//
//                 // options
//                 var rotation = -1;
//                 if (signal.Options.TryGetValue("Rotation", out var rotationObject))
//                 {
//                     if (rotationObject is int validRotation && (validRotation == 1 || validRotation == -1))
//                     {
//                         rotation = validRotation;
//                     }
//                     else
//                     {
//                         // Handle invalid rotation value
//                         Debug.LogWarning("Invalid rotation value, defaulting to -1.");
//                     }
//                 }
//                 
//                 // build dictionary and remove hand
//                 var playerToCardsInHand = new Dictionary<IPlayer, List<ICard>>();
//                 foreach (var player in players)
//                 {
//                     playerToCardsInHand[player] = player.Hand.RemoveAllCardInternal();
//                 }
//                 
//                 var s = DOTween.Sequence();
//                 foreach (var player in players)
//                 {
//                     var nextPlayer = GetNextPlayer(player, players, rotation);
//                     
//                     foreach (var card in playerToCardsInHand[nextPlayer])
//                     {
//                         player.Hand.AddCardInternal(card, 0);
//                     }
//
//                     s.Join(player.Hand.UpdateCardPositions());
//                 }
//                 
//                 s.OnComplete(() =>
//                 {
//                     AnimationQueueService.Next();
//                 });
//             });
//         }
//         
//         private IPlayer GetNextPlayer(IPlayer player, IList<IPlayer> players, int rotation)
//         {
//             // Find the index of the current player
//             var currentIndex = players.IndexOf(player);
//
//             // Calculate the next index based on the rotation direction
//             int nextIndex;
//             if (rotation == 1) // Clockwise
//             {
//                 nextIndex = (currentIndex + 1) % players.Count;
//             }
//             else // Counter-clockwise
//             {
//                 nextIndex = (currentIndex - 1 + players.Count) % players.Count;
//             }
//
//             // Return the next players in the rotation
//             return players[nextIndex];
//         }
//     }
// }