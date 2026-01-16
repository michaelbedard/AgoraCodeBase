using Agora.Core.Enums;
using UnityEngine;

namespace _src.Code.Core
{
    public class Globals : MonoBehaviour
    {
        [SerializeField]
        private Texture2D cursorTextureDefault;
        [SerializeField]
        private Texture2D cursorTextureClickable;
        [SerializeField]
        private Texture2D cursorTextureDraggable;
        [SerializeField]
        private Texture2D cursorTextureDragging;
        [SerializeField]
        private Texture2D cursorTextureInput;
        
        [Header("Sound Effects")]
        public AudioClip audioClipShuffleDeck;
        public AudioClip audioClipDrawingCard;
        public AudioClip audioClipMovingPiece;
        public AudioClip audioClipPlayingCard;
        public AudioClip audioClipRollingDice;
        
        public static Globals Instance;

        public Globals()
        {
            Instance = this;
        }
        
        // Properties
        public string ServerUrl
        {
            get
            {
                #if !UNITY_EDITOR
                    return "https://api.agoraboardgames.com";
                #endif

                return "http://localhost:5039";
            }
        }

        public Texture2D CursorTextureDefault => cursorTextureDefault;
        public Texture2D CursorTextureClickable => cursorTextureClickable;
        public Texture2D CursorTextureDraggable => cursorTextureDraggable;
        public Texture2D CursorTextureDragging => cursorTextureDragging;
        public Texture2D CursorTextureInput => cursorTextureInput;

        [HideInInspector]
        public int BlurCount { get; set; } = 0;
        [HideInInspector]
        public int MouseOverCount { get; set; } = 0;
    }
}