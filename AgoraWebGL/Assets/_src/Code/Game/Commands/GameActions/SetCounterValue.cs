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
//     public class SetCounterValue : BaseAction
//     {
//         private const float BounceAnimationTime = 0.5f;
//         
//         [Inject]
//         public SetCounterValue(SignalBus signalBus, IAnimationQueueService animationQueueService, IGameModuleService gameModuleService) 
//             : base(signalBus, animationQueueService, gameModuleService)
//         {
//         }
//
//         protected override void Allow(GameActionSignalSignal signalSignal)
//         {
//             if (signalSignal.Key != nameof(SetCounterValue)) return;
//             
//             throw new System.NotImplementedException();
//         }
//
//         protected override void Animate(GameAnimationSignal signal)
//         {
//             if (signal.Key != nameof(SetCounterValue)) return;
//             
//             AnimationQueueService.Push(async () =>
//             {
//                 var counter = GameModuleService.GetGameModuleById<ICounter>(signal.Args[0]);
//                 
//                 // options
//                 var value = 0;
//                 if (signal.Options.TryGetValue("Value", out var optionValue) && optionValue is int intValue)
//                 {
//                     value = intValue;
//                 }
//                 else
//                 {
//                     Debug.LogWarning("Missing or invalid 'Value' option in signal.");
//                 }
//                 
//                 var originalScale = counter.Transform.localScale;
//                 var targetScale = new Vector3(1.5f, 1.5f, 1.5f);
//
//                 var s = DOTween.Sequence();
//                 s.Join(counter.Transform.DOScale(targetScale, BounceAnimationTime).SetEase(Ease.OutBounce));
//                 s.OnComplete(() =>
//                 {
//                     counter.Value = (value);
//
//                     counter.Transform.DOScale(originalScale, BounceAnimationTime).SetEase(Ease.InBounce);
//                 });
//                 
//                 // will trigger immediately
//                 AnimationQueueService.Next();
//             });
//         }
//     }
// }