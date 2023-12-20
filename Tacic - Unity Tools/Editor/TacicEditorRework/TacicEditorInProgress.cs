using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tacic.Tacic___Unity_Tools.Editor.TacicEditorRework
{
    public class TacicEditorInProgress : MonoBehaviour
    {
        // Setting parameters from animation to animation curve attribute in some script. Maybe add window for referencing script
        // or name and selection. See how can you use that
        // [MenuItem("GameObject/Talenzzo/In Test - Animation Curves", false, 0)]
        static void AnimationCurves()
        {
            // Nadjemo AnimationClip
            string assetPath = "Assets/Levels/Level213/Animations/TabletSortingMGAnimations/TabletSortingMGAnimation.anim";
            AnimationClip animationClip = (AnimationClip) AssetDatabase.LoadAssetAtPath(
                assetPath, typeof(AnimationClip));
            if (animationClip == null)
            {
                Debug.Log("Animacija nije pronadjena");
                return;
            }

            // Nadjemo sve Curve Bindinge za animation clip
            EditorCurveBinding[] bindingsArray = AnimationUtility.GetCurveBindings(animationClip);
            EditorCurveBinding targetBinding = bindingsArray[0];
            // Nadjemo binding sa propertyjem kojim zelimo
            foreach (EditorCurveBinding binding in bindingsArray)
            {
                if (binding.propertyName == "m_LocalPosition.x")
                {
                    targetBinding = binding;
                    break;
                }
            }

            // Nadjemo AnimationCurve za taj binding
            AnimationCurve curve = AnimationUtility.GetEditorCurve(animationClip, targetBinding);

            // Nadjemo mg objekat gde treba da se referencira
            GameObject minigameGO = GameObject.Find("TabletSortingPuzzleMGMiniGame");
            //TabletSortingGameplayManager manager = minigameGO.GetComponent<TabletSortingGameplayManager>();
            // manager.curve = new AnimationCurve();
            // manager.curve.keys = curve.keys;
            // manager.curve.preWrapMode = curve.preWrapMode;
            // manager.curve.postWrapMode = curve.postWrapMode;
            //brisi komentare kad treba

            GameObject roomsHolder = GameObject.Find("RoomsHolder");
            //EditorUtility.SetDirty(manager);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            PrefabUtility.ApplyPrefabInstance(roomsHolder, InteractionMode.AutomatedAction);
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            //PrefabUtility.RecordPrefabInstancePropertyModifications(roomsHolder);
        }
        

        
    }
}

