// using _src.Code.Core;
// using _src.Code.Core.Actors;
// using _src.Code.Core.Interfaces.GameModules;
// using _src.Code.Core.Interfaces.Services;
// using Agora.Core.Dtos.Game.Commands.Actions;
//
// using DG.Tweening;
// using UnityEngine;
// using Zenject;
//
// namespace _src.Code.Game.Commands.GameActions
// {
//     public class ReturnCardToDeck: BaseAction<ReturnCardToDeckActionDto, ReturnCardToDeckAnimationDto>
//     {
//         private const float CardTransitionTime = 0.3f;
//         
//         [Inject]
//         public ReturnCardToDeck(SignalBus signalBus, IAnimationQueueService animationQueueService, IGameModuleService gameModuleService) 
//             : base(signalBus, animationQueueService, gameModuleService)
//         {
//         }
//         
//         // allow
//         protected override void AllowCore(ReturnCardToDeckActionDto action)
//         {
//             var card = GameModuleService.GetGameModuleById(action.CardId);
//             card.DragActions.Add(new DragAction(action, new [] { action.DeckId }));
//         }
//         
//         // animate
//         protected override void AnimateCore(ReturnCardToDeckAnimationDto animation)
//         {
//             AnimationQueueService.Push(async () =>
//             {
//                 var deck = GameModuleService.GetGameModuleById<IDeck>(animation.DeckId);
//                 var card = GameModuleService.GetGameModuleById<ICard>(animation.CardId);
//                 
//                 // add to zone
//                 if (card.ParentZone != null)
//                 {
//                     var parentZone = card.ParentZone;
//                     parentZone.RemoveCard(card);
//                     parentZone.UpdateObjectPositions();
//                 }
//
//                 // get transforms
//                 var topDeckTransform = deck.GetTopDeckTransform();
//                 var cardTransform = card.Transform;
//                 
//                 // sound
//                 AudioService.PlaySoundEffectAsync(Globals.Instance.audioClipPlayingCard);
//                 
//                 // tween
//                 var s = DOTween.Sequence();
//                 s.Join(cardTransform.DOMove(topDeckTransform.position, CardTransitionTime));
//                 s.Join(cardTransform.DORotateQuaternion(topDeckTransform.rotation, CardTransitionTime));
//                 s.Join(cardTransform.DOScale(topDeckTransform.localScale, CardTransitionTime));
//                 
//                 s.OnComplete(() =>
//                 {
//                     // default position, somewhere far away
//                     cardTransform.position = new Vector3(0, 0, 40f);
//                     
//                     AnimationQueueService.Next();
//                 });
//             });
//         }
//     }
// }