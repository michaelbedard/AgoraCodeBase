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
//     public class RemoveToken : BaseAction
//     {
//         private const float AnimationTime = 1.5f;
//         
//         public RemoveToken(SignalBus signalBus, IAnimationQueueService animationQueueService, IGameModuleService gameModuleService) 
//             : base(signalBus, animationQueueService, gameModuleService)
//         {
//         }
//
//         protected override void Allow(GameActionSignalSignal signalSignal)
//         {
//             if (signalSignal.Key != nameof(RemoveToken)) return;
//             
//             throw new System.NotImplementedException();
//         }
//
//         protected override void Animate(GameAnimationSignal signal)
//         {
//             if (signal.Key != nameof(RemoveToken)) return;
//             
//             AnimationQueueService.Push(async () =>
//             {
//                 var tokenPile = GameModuleService.GetGameModuleById<IToken>(signal.Args[0]);
//                 
//                 var lastTokenAdded = tokenPile.RemoveLastToken();
//                 var targetPosition = lastTokenAdded.Transform.position + new Vector3(0, 10, 0);
//
//                 // tween
//                 var s = DOTween.Sequence();
//                 s.Join(lastTokenAdded.Transform.DOMove(targetPosition, AnimationTime))
//                     .OnComplete(() => {
//                         // GameObject.Destroy(lastTokenAdded.);
//                     });
//                 
//                 s.AppendInterval(0.2f);
//                 s.OnComplete(() =>
//                 {
//                     AnimationQueueService.Next();
//                 });
//             });
//         }
//     }
// }