using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using _src.Code.Core.Actors;
using _src.Code.Core.Utility.MVC;
using Agora.Core.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace _src.Code.Game.Modules
{
    public class GameModuleModel : BaseModel
    {

        [Header("Game Module")]
        [SerializeField] 
        private Image glowImage;
        
        private string _id;
        private string _name;
        private GameModuleType _type;
        private string _description;
        private bool _isDropTarget;
        private List<ClickAction> _clickActions = new List<ClickAction>();
        private List<DragAction> _dragActions = new List<DragAction>();

        /// <summary>
        /// View Properties
        /// </summary>

        public Image GlowImage => glowImage;

        /// <summary>
        /// Controller Properties
        /// </summary>

        public string Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public GameModuleType Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public bool IsDropTarget
        {
            get => _isDropTarget;
            set
            {
                if (_isDropTarget != value)
                {
                    _isDropTarget = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<ClickAction> ClickActions
        {
            get => _clickActions;
            set
            {
                if (_clickActions != value)
                {
                    _clickActions = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public List<DragAction> DragActions
        {
            get => _dragActions;
            set
            {
                if (_dragActions != value)
                {
                    _dragActions = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}