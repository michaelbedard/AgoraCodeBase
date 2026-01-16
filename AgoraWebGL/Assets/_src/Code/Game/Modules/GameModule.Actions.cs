using System.Collections.Generic;
using System.Linq;
using _src.Code.Core.Actors;
using _src.Code.Game.Modules.Card;
using Agora.Core.Payloads.Hubs;
using UnityEngine;

namespace _src.Code.Game.Modules
{
    public abstract partial class GameModule<TModel>
    {
        private bool _disableActions;
        
        public void ResetActions()
        {
            Model.ClickActions = new List<ClickAction>();
            Model.DragActions = new List<DragAction>();
        }
        
        public virtual void EnableGlow()
        {
            if (Model.GlowImage == null)
                return;
            
            var currentAlpha = Model.GlowImage.color.a;
            var glowColor = Color.green;
            
            // TODO check if special color

            glowColor.a = currentAlpha;
            Model.GlowImage.gameObject.SetActive(true);
            Model.GlowImage.color = glowColor;
        }
        
        public virtual void DisableGlow()
        {
            if (Model.GlowImage != null)
                Model.GlowImage.gameObject.SetActive(false);
        }
        
        public virtual void UpdateGlowColor()
        {
            if (IsDropTarget)
            {
                EnableGlow();
            }
            else if (Model.ClickActions.Count > 0 || Model.DragActions.Count > 0)
            {
                EnableGlow();
            }
            else
            {
                DisableGlow();
            }
        }
        
        protected virtual void DoClickAction()
        {
            if (Model.ClickActions.Count > 0)
            {
                Debug.Log("Execute ClickAction with Id " + Model.ClickActions[0].Id);
                GameHubProxy.ExecuteAction(new ExecuteActionPayload()
                {
                    ActionId = Model.ClickActions[0].Id
                });
            }
        }

        protected virtual void OnDragStart(Vector2 position)
        {
            foreach (var dragAction in DragActions.Where(a=> !a.CanDropAnywhere))
            {
                foreach (var droppableModuleId in dragAction.TargetIds)
                {
                    Debug.Log("Can drop on " + droppableModuleId);
                
                    var droppableModule = GameModuleService.GetGameModuleById(droppableModuleId);
                    droppableModule.IsDropTarget = true;
                    droppableModule.UpdateGlowColor();
                }
            }
        }

        protected virtual void OnDragUpdate(InputObject objects, Vector2 position)
        {
            
        }

        // return whether or not is has trigger an action
        protected virtual bool OnDragEnd(List<InputObject> droppedOnObjects)
        {
            // check if we have made a drop action
            foreach (var droppedOnObject in droppedOnObjects)
            {
                var droppedOnModule = GameModuleService.GetGameModuleByInputObject(droppedOnObject);
                foreach (var dragAction in DragActions.Where(a => !a.CanDropAnywhere))
                {
                    foreach (var droppableModuleId in dragAction.TargetIds)
                    {
                        if (droppedOnModule.Id == droppableModuleId)
                        {
                            // we have drop on this.  Execute the action
                
                            Debug.Log("Do DragAction with Id " + dragAction.Id);
                            GameHubProxy.ExecuteAction(new ExecuteActionPayload()
                            {
                                ActionId = dragAction.Id
                            });
                            
                            // set IsDropTarget to false for every module 
                            foreach (var gameModule in GameModuleService.GetAllGameModules())
                            {
                                gameModule.IsDropTarget = false;
                                gameModule.UpdateGlowColor();
                            }
                        
                            return true;
                        }
                    }
                }
            }

            if (DragActions.Count(a => a.CanDropAnywhere) > 0)
            {
                // check if we have not dropped on a card in hand
                if (!droppedOnObjects.Any(d => d is CardModel card && card.Hand != null))
                {
                    // we have drop nothing.  Execute the action
                    var action = DragActions.First(a => a.CanDropAnywhere);
                
                    Debug.Log("Do DragAction with Id " + action.Id);
                    GameHubProxy.ExecuteAction(new ExecuteActionPayload()
                    {
                        ActionId = action.Id
                    });
                
                    // set IsDropTarget to false for every module 
                    foreach (var gameModule in GameModuleService.GetAllGameModules())
                    {
                        gameModule.IsDropTarget = false;
                        gameModule.UpdateGlowColor();
                    }
                        
                    return true;
                }
            }
            
            // set IsDropTarget to false for every module 
            foreach (var gameModule in GameModuleService.GetAllGameModules())
            {
                gameModule.IsDropTarget = false;
                gameModule.UpdateGlowColor();
            }
            
            return false;
        }
    }
}