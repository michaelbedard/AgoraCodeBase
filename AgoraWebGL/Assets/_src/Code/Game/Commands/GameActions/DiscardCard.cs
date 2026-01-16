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
//     public class DiscardCard : BaseAction
//     {
//         private ICameraPlaneService _cameraPlaneService;
//         
//         private const float CardDistanceAnimation = 1.0f;
//         private const float CardTimeAnimation = 1.0f;
//         
//         [Inject]
//         public DiscardCard(SignalBus signalBus, IAnimationQueueService animationQueueService, IGameModuleService gameModuleService,
//             ICameraPlaneService cameraPlaneService) 
//             : base(signalBus, animationQueueService, gameModuleService)
//         {
//             _cameraPlaneService = cameraPlaneService;
//         }
//
//         protected override void Allow(GameActionSignalSignal signalSignal)
//         {
//             if (signalSignal.Key != nameof(DiscardCard)) return;
//             
//             throw new System.NotImplementedException();
//         }
//
//         protected override void Animate(GameAnimationSignal signal)
//         {
//             if (signal.Key != nameof(DiscardCard)) return;
//             
//             AnimationQueueService.Push(async () =>
//             {
//                 var player = GameModuleService.GetGameModuleById<IPlayer>(signal.Args[0]);
//                 var card = GameModuleService.GetGameModuleById<ICard>(signal.Args[1]);
//
//                 // get target position
//                 var cardCoordinatesOnPlane = _cameraPlaneService.GetTransformPlaneCoordinate(card.Transform);
//                 var centerOfRotation = player.Hand.GetCenterOfRotation();
//                 
//                 var direction = (cardCoordinatesOnPlane - centerOfRotation).normalized;
//                 var targetCoordinates = cardCoordinatesOnPlane + direction * CardDistanceAnimation;
//                 
//                 var targetWorldPosition = _cameraPlaneService.TransformPoint(targetCoordinates);
//                 
//                 // logic
//                 player.Hand.RemoveCardInternal(card);
//                 
//                 // tween
//                 var s = DOTween.Sequence();
//                 s.Join(card.Transform.DOMove(targetWorldPosition, CardTimeAnimation));
//                 // s.Join(card.CanvasGroup.DOFade(0, CardTimeAnimation));
//                 s.Join(player.Hand.UpdateCardPositions());
//                 
//                 s.OnComplete(() =>
//                 {
//                     // reset
//                     card.CanvasGroup.alpha = 1f;
//                     card.Transform.position = Vector3.zero;
//                     card.Transform.rotation = new Quaternion();
//                     
//                     AnimationQueueService.Next();
//                 });
//             });
//         }
//     }
// }