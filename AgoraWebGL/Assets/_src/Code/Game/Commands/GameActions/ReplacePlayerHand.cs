// using System;
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
//     public class ReplacePlayerHand : BaseAction
//     {
//         private ICameraPlaneService _cameraPlaneService;
//         
//         private const float CardTransitionTime = 1f;
//         
//         [Inject]
//         public ReplacePlayerHand(
//             SignalBus signalBus, 
//             IAnimationQueueService animationQueueService, 
//             IGameModuleService gameModuleService, 
//             ICameraPlaneService cameraPlaneService) 
//             : base(signalBus, animationQueueService, gameModuleService)
//         {
//             _cameraPlaneService = cameraPlaneService;
//         }
//         
//         // allow
//         protected override void Allow(GameActionSignalSignal signalSignal)
//         {
//             if (signalSignal.Key != nameof(ReplacePlayerHand)) return;
//
//             throw new NotImplementedException();
//         }
//         
//         // animate
//         protected override void Animate(GameAnimationSignal signal)
//         {
//             if (signal.Key != nameof(ReplacePlayerHand)) return;
//             
//             AnimationQueueService.Push(async () =>
//             {
//                 var player = GameModuleService.GetGameModuleById<IPlayer>(signal.Args[0]);
//                 var cards = signal.Args.Skip(1).Select(arg => GameModuleService.GetGameModuleById<ICard>(arg)).ToList();
//
//                 var centerOfRotationTransform = new GameObject("CenterOfRotationTransform").transform;
//                 var centerOfRotation = player.Hand.GetCenterOfRotation();
//                 
//                 _cameraPlaneService.PositionElement(centerOfRotationTransform, centerOfRotation);
//                 
//                 // remove cards from hand
//                 var previousCards = player.Hand.RemoveAllCardInternal();
//                 
//                 // instantaneously position other hand cards at the center of rotation
//                 foreach (var card in cards)
//                 {
//                     card.Transform.position = centerOfRotationTransform.position;
//                     player.Hand.AddCardInternal(card, 0);
//                 }
//
//                 // tween
//                 var s = DOTween.Sequence();
//
//                 // place previous card to center of rotation
//                 foreach (var previousCard in previousCards)
//                 {
//                     s.Join(previousCard.Transform.DOMove(centerOfRotationTransform.position, CardTransitionTime));
//                     s.Join(previousCard.Transform.DORotateQuaternion(centerOfRotationTransform.rotation, CardTransitionTime));
//                     s.Join(previousCard.Transform.DOScale(centerOfRotationTransform.localScale, CardTransitionTime));
//                 }
//                 
//                 // place new cards on their spot
//                 s.Join(player.Hand.UpdateCardPositions(CardTransitionTime));
//                 
//                 s.OnComplete(() =>
//                 {
//                     GameObject.Destroy(centerOfRotationTransform.gameObject);
//                     AnimationQueueService.Next();
//                 });
//             });
//         }
//     }
// }