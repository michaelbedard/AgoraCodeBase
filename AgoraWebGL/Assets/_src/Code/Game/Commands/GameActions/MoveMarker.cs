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
//     public class MoveMarker: BaseAction<MoveMarkerActionDto, MoveMarkerAnimationDto>
//     {
//         private const float MoveTransitionTime = 0.7f;
//         private const float ParabolaHeight = 1.0f;
//         
//         [Inject]
//         public MoveMarker(
//             SignalBus signalBus, 
//             IAnimationQueueService animationQueueService, 
//             IGameModuleService gameModuleService) 
//             : base(signalBus, animationQueueService, gameModuleService)
//         {
//         }
//         
//         // allow
//         protected override void AllowCore(MoveMarkerActionDto action)
//         {
//             var marker = GameModuleService.GetGameModuleById(action.MarkerId);
//             var zone = GameModuleService.GetGameModuleById(action.ZoneId);
//             
//             marker.DragActions.Add(new DragAction(action, new [] { zone.Id }));
//         }
//         
//         // animate
//         protected override void AnimateCore(MoveMarkerAnimationDto animation)
//         {
//             AnimationQueueService.Push(async () =>
//             {
//                 var marker = GameModuleService.GetGameModuleById<IMarker>(animation.MarkerId);
//                 var path = animation.Path;
//
//                 var s = DOTween.Sequence();
//                 Vector3? lastPosition = null;
//                 foreach (var zoneId in path)
//                 {
//                     var parentZone = marker.ParentZone;
//                     var zone = GameModuleService.GetGameModuleById<IZone>(zoneId);
//                     
//                     // remove marker
//                     parentZone.RemoveMarker(marker);
//                         
//                     // add marker
//                     var targetTransform = zone.AddMarker(marker);
//
//                     // get transforms
//                     var markerTransform = marker.Transform;
//                     
//                     // Calculate parabolic path
//                     var startPosition = lastPosition ?? markerTransform.position;
//                     var targetPosition = targetTransform.position;
//                     var midPosition = new Vector3(
//                         (startPosition.x + targetPosition.x) / 2, 
//                         Mathf.Max(startPosition.y, targetPosition.y) + ParabolaHeight,
//                         (startPosition.z + targetPosition.z) / 2
//                     );
//                     
//                     // Append animations for this step
//                     s.Append(zone.UpdateObjectPositions(marker))
//                         .Join(parentZone.UpdateObjectPositions())
//                         .Join(markerTransform.DOPath(
//                                 new[] { startPosition, midPosition, targetPosition },
//                                 MoveTransitionTime,
//                                 PathType.CatmullRom
//                             )
//                         )
//                         .Join(markerTransform.DORotateQuaternion(targetTransform.rotation, MoveTransitionTime))
//                         .Join(markerTransform.DOScale(targetTransform.localScale, MoveTransitionTime))
//                         .JoinCallback(() =>
//                         {
//                             AudioService.PlaySoundEffectAsync(Globals.Instance.audioClipMovingPiece);
//                         });
//                     
//                     lastPosition = targetPosition;
//                 }
//                 
//                 s.OnComplete(() =>
//                 {
//                     AnimationQueueService.Next();
//                 });
//             });
//         }
//     }
// }