using _src.Code.Core;
using _src.Code.Core.Actors;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace _src.Code.UI.Common.IconButtons
{
    public class CloseIcon: CustomButton
    {
        public new class UxmlFactory : UxmlFactory<CloseIcon, UxmlTraits> { }

        
        protected override void InitializeCore()
        {
            base.InitializeCore();
            
            Clicked += () =>
            {
                Cursor.SetCursor(Globals.Instance.CursorTextureDefault, Vector2.zero, CursorMode.Auto);
            };
        }
    }
}