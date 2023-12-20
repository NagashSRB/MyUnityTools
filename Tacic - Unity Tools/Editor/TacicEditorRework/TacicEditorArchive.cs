using Tacic.Tacic___Unity_Tools.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Tacic.Tacic___Unity_Tools.Editor.TacicEditorRework
{
    public class TacicEditorArchive : EditorWindow
    {
        // Removed reason: MiniGameManager class now have solve on 'SPACE'. Every mini game inherits it now.
        // Not Reworked: Undo, UnityEngine.SceneManagenent
        //[MenuItem("GameObject/Talenzzo/Add|Remove Solve", false, 0)]
        static void AddRemoveSolve()
        {
            GameObject roomsHolder = GameObject.Find("RoomsHolder");
            foreach (Transform room in roomsHolder.transform)
            {
                foreach (Transform obj in room)
                {
                    if (obj.gameObject.GetComponent<MiniGame>())
                    {
                        SolveMiniGameInEditorTacic solve = obj.gameObject.GetComponent<SolveMiniGameInEditorTacic>();
                        if (!solve)
                            obj.gameObject.AddComponent<SolveMiniGameInEditorTacic>();
                        else
                            DestroyImmediate(solve);
                        EditorUtility.SetDirty(obj);
                    }
                }
            }

            SceneManager.GetActiveScene();
            PrefabUtility.ApplyPrefabInstance(roomsHolder, InteractionMode.AutomatedAction);
            //EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }
        
        // static void CloseZoomZonesAndRooms(GameObject roomsHolder)
        // {
        //     Type type = typeof(ZoomZone);
        //     inputGameObject.GetComponent(type);
        //     int zoomElementLayer = 8;
        //     foreach (Transform room in roomsHolder.transform)
        //     {
        //         foreach (Transform obj in room)
        //         {
        //             if (obj.gameObject.GetComponent<ZoomZone>() || obj.gameObject.GetComponent<MiniGame>())
        //             {
        //                 if(obj.GetChild(0).gameObject.activeSelf)
        //                 {
        //                     obj.GetChild(0).gameObject.SetActive(false);
        //                     Debug.Log($"Disabled holder: " + obj.name);
        //                 }
        //                 
        //                 TacicEditorUtils.SetLayerToChildren(obj.transform, zoomElementLayer, true);
        //             }
        //         }
        //
        //         if (room.gameObject.name != "AudioObjects")
        //             room.gameObject.SetActive(false);
        //     }
        //
        //     roomsHolder.transform.GetChild(0).gameObject.SetActive(true);
        // }
    }
}

