using UnityEditor;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.Editor
{
    [InitializeOnLoad]
    public class DecoratingHierarchyEditor
    {
        private static readonly Color Red = new Color(1, 0, 0, 0.3f);
        private static readonly Color Green = new Color(0, 1, 0, 0.3f);
        private static readonly Color Blue = new Color(0, 0, 1, 0.3f);
        private static readonly Color Cyan = new Color(0, 1, 1, 0.3f);
        private static readonly Color Magenta = new Color(1, 0, 1, 0.3f);
        private static readonly Color Yellow = new Color(1, 1, 0, 0.3f);
        private static readonly Color Orange = new Color(1, 0.5f, 0, 0.3f);
        
        static DecoratingHierarchyEditor()
        {
            EditorApplication.hierarchyWindowItemOnGUI += DrawDecoration;
        }

        private static void DrawDecoration(int instanceId, Rect selectionRect)
        {
            GameObject go = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
            if (go != null)
            {
                if (go.GetComponent<ActiveItem>())
                {
                    EditorGUI.DrawRect(selectionRect, Blue);
                }
                else if (go.GetComponent<ZoomZone>() || go.GetComponent<ZoomZoneItem>())
                {
                    EditorGUI.DrawRect(selectionRect, Green);
                }
                else if (go.GetComponent<MiniGame>() || go.GetComponent<MiniGameItem>())
                {
                    EditorGUI.DrawRect(selectionRect, Yellow);
                }
                else if (go.GetComponent<StatesAnimationItem>())
                {
                    EditorGUI.DrawRect(selectionRect, Magenta);
                }
                else if (go.GetComponent<TargetItem>())
                {
                    EditorGUI.DrawRect(selectionRect, Orange);
                }
            }
        }
    }
}
