#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.Scripts.In_Progress.BoardGame.SpecificTypes.BoardStructure.GraphBoard.Editor
{
    public class SceneViewMouseInteraction : EditorWindow
    {
    private static Vector2 mousePosition;
    private static bool isMouseDown;
    private static bool customInteractionEnabled = true;

    [MenuItem("Window/Scene View Mouse Interaction")]
    public static void ShowWindow()
    {
        GetWindow<SceneViewMouseInteraction>("Scene View Mouse Interaction");
    }

    private void OnEnable()
    {
        SceneView.onSceneGUIDelegate += SceneGUI;
    }

    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= SceneGUI;
    }

    private void SceneGUI(SceneView sceneView)
    {
        if (!customInteractionEnabled)
        {
            //HandleUtility.Repaint();
            
            return;
        }

        Event currentEvent = Event.current;
        //Debug.Log(currentEvent.mousePosition);
        Debug.Log(sceneView.camera.ScreenToWorldPoint(mousePosition));

        switch (currentEvent.type)
        {
            case EventType.MouseDown:
                isMouseDown = true;
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                Ray mouseRay = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(mouseRay, out hitInfo))
                {
                    Vector3 worldPosition = hitInfo.point;
                    Debug.Log("Pos:" + worldPosition);
                    // Calculate distances from predefined objects
                    // foreach (GameObject obj in predefinedObjects)
                    // {
                    //     float distance = Vector3.Distance(obj.transform.position, worldPosition);
                    //     Debug.Log("Distance to " + obj.name + ": " + distance);
                    // }
                }

                currentEvent.Use();
                break;

            case EventType.MouseDrag:
                if (isMouseDown)
                {
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                }
                break;

            case EventType.MouseUp:
                isMouseDown = false;
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                break;

            case EventType.Repaint:
            case EventType.Layout:
                break;

            default:
                // Store mouse position for other event types
                mousePosition = currentEvent.mousePosition;

                // Prevent default scene interactions for other event types
                currentEvent.Use();
                break;
        }
    }

    private void OnGUI()
    {
        customInteractionEnabled = GUILayout.Toggle(customInteractionEnabled, "Custom Interaction Enabled");

        GUILayout.Label("Mouse Position: " + mousePosition);
    }
    }
}
#endif