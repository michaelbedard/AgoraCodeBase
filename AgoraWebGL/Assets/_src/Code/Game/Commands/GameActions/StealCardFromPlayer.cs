// using _src.Scripts.Core.Interfaces.GameModules;
// using _src.Scripts.Core.Interfaces.Services;
// using _src.Scripts.Core.Signals.Game;
// using _src.Scripts.Game.GameModules;
// using DG.Tweening;
// using Zenject;
//
// namespace _src.Scripts.Game.GameCommands.GameActions
// {
//     public class StealCardFromPlayer : BaseAction
//     {
//         [Inject]
//         public StealCardFromPlayer(SignalBus signalBus, IAnimationQueueService animationQueueService, IGameModuleService gameModuleService) 
//             : base(signalBus, animationQueueService, gameModuleService)
//         {
//         }
//
//         protected override void Allow(GameActionSignalSignal signalSignal)
//         {
//             if (signalSignal.Key != nameof(StealCardFromPlayer)) return;
//             
//             throw new System.NotImplementedException();
//         }
//
//         protected override void Animate(GameAnimationSignal signal)
//         {
//             if (signal.Key != nameof(StealCardFromPlayer)) return;
//             
//             AnimationQueueService.Push(async () =>
//             {
//                 var player = GameModuleService.GetGameModuleById<IPlayer>(signal.Args[0]);
//                 var target = GameModuleService.GetGameModuleById<IPlayer>(signal.Args[1]);
//                 var card = GameModuleService.GetGameModuleById<ICard>(signal.Args[2]);
//
//                 player.Hand.AddCardInternal(card, 0);
//                 target.Hand.RemoveCardInternal(card);
//                 
//                 var s = DOTween.Sequence();
//                 s.Join(player.Hand.UpdateCardPositions());
//                 s.Join(target.Hand.UpdateCardPositions());
//                 
//                 s.OnComplete(() =>
//                 {
//                     AnimationQueueService.Next();
//                 });
//             });
//         }
//     }
// }