// using _src.Scripts.Core.Interfaces.GameModules;
// using _src.Scripts.Core.Interfaces.Services;
// using Agora.Core.Dtos.GameDtos.CommandDtos;
// using DG.Tweening;
// using UnityEngine;
// using Zenject;
//
// namespace _src.Scripts.Game.GameCommands.GameActions
// {
//     public class AddToken : BaseAction<>
//     {
//         private const float AnimationTime = 1.5f;
//         
//         public AddToken(SignalBus signalBus, IAnimationQueueService animationQueueService, IGameModuleService gameModuleService) : base(signalBus, animationQueueService, gameModuleService)
//         {
//         }
//
//         protected override void Allow(CommandDto signalSignal)
//         {
//             if (signalSignal.Key != nameof(AddToken)) return;
//             
//             throw new System.NotImplementedException();
//         }
//
//         protected override void Animate(GameAnimationSignal signal)
//         {
//             if (signal.Key != nameof(AddToken)) return;
//
//             AnimationQueueService.Push(() =>
//             {
//                 var tokenPile = GameModuleService.GetGameModuleById<IToken>(signal.Args[0]);
//
//                 var token = tokenPile.AddToken();
//                 var tokenTransform = token.Transform;
//                 var targetTransform = tokenPile.GetNextTokenTransform();
//
//                 tokenTransform.position = targetTransform.position + new Vector3(0, 10, 0);
//                 tokenTransform.rotation = targetTransform.rotation;
//                 tokenTransform.localScale = targetTransform.localScale;
//
//                 // tween
//                 var s = DOTween.Sequence();
//                 s.Join(token.Transform.DOMove(targetTransform.position, AnimationTime));
//                 s.AppendInterval(0.2f);
//                 s.OnComplete(() =>
//                 {
//                     GameObject.Destroy(targetTransform.gameObject);
//                     AnimationQueueService.Next();
//                 });
//             });
//         }
//     }
// }