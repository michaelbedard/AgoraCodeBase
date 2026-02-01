using _src.Code.Game._other.Hand;
using UnityEditor;
using UnityEngine;

namespace _src.Editor
{
    [CustomEditor(typeof(HandSlotPositions))]
    public class HandSlotPositionsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            // Reference to the HandSlotPositions script
            var handSlotPositions = (HandSlotPositions)target;
            
            if (GUILayout.Button("AddSlot"))
            {
                handSlotPositions.AddSlot();
                handSlotPositions.UpdateSlotPositions();
            }
            
            if (GUILayout.Button("RemoveSlot"))
            {
                handSlotPositions.RemoveSlot();
                handSlotPositions.UpdateSlotPositions();
            }
            
            if (GUILayout.Button("MoveRight"))
            {
                handSlotPositions.MoveSlotsToTheRight();
                handSlotPositions.UpdateSlotPositions();
            }
            
            if (GUILayout.Button("MoveLeft"))
            {
                handSlotPositions.MoveSlotsToTheLeft();
                handSlotPositions.UpdateSlotPositions();
            }

            if (GUILayout.Button("Update"))
            {
                handSlotPositions.UpdateSlotPositions();
            }
        }
    }
}