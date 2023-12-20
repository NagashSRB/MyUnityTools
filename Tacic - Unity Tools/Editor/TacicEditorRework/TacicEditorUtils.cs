using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.Editor.TacicEditorRework
{
    public class TacicEditorUtils : EditorWindow
    {
        #region ChildObjects
        
        // TODO: DirectChildren i AllChildren, mozda kao parametar da se prosledjuje nekako. boolean ili enum 
        // TODO: Concrete functions, helper functions, bool question functions
        public static void AddNumbersToChildsGameObjectName(Transform parent)
        {
            int i = 0;
            List<Transform> children = GetChildrenTransforms(parent, false);
            foreach (Transform child in children)
            {
                Undo.RecordObject(child.gameObject, "Add Numbers To GameObject Name");
                child.name += i;
                i++;
            }
        }

        public static void RemoveParentForAllDirectChildren(Transform parent)
        {
            List<Transform> children = GetChildrenTransforms(parent, false);
            foreach (Transform child in children)
            {
                child.parent = null;
            }
        }
        
        public static void SetLayerToChildren(Transform root, int layer, bool allChildren)
        {
            List<Transform> children = GetChildrenTransforms(root, allChildren);
            foreach (Transform child in children)
            {
                child.gameObject.layer = layer;
            }
        }

        /// <summary>
        /// Get children of an object
        /// </summary>
        /// <param name="parent">Target parent object</param>
        /// <param name="allChildren">If true return all children, if not only direct children (1. layer)</param>
        /// <param name="includeInactive">Should include inactive gameobjects</param>
        public static List<Transform> GetChildrenTransforms(Transform parent, bool allChildren, bool includeInactive = true)
        {
            if (allChildren)
            {
                return parent.GetComponentsInChildren<Transform>(includeInactive).ToList();
            }

            List<Transform> children = new List<Transform>();
            foreach (Transform child in parent)
            {
                if (includeInactive || child.gameObject.activeInHierarchy)
                {
                    children.Add(child);
                }
            }

            return children;
        }
        
        /// <summary>
        /// Checks if parent is same for all children
        /// </summary>
        public static bool IsParentSameForObjects(List<GameObject> gameObjects)
        {
            Transform parent = gameObjects[0].transform.parent;
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.transform.parent != parent)
                {
                    Debug.Log("Objects don't have same parents");
                    return false;
                }
            }

            return true;
        }
        
        #endregion

        #region Transform manipulation

        public static void RoundPositionOfGameObjects(List<GameObject> gameObjects, int digits)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                Undo.RecordObject(gameObject.transform, "Round Position Of GameObjects");
                Vector3 localPosition = gameObject.transform.localPosition;
                gameObject.transform.localPosition = new Vector3(
                    (float) Math.Round(localPosition.x, digits),
                    (float) Math.Round(localPosition.y, digits),
                    (float) Math.Round(localPosition.z, digits));
            }
        }

        public static void CopyChildPositions(Transform parentToCopyFrom, Transform parentToCopyTo)
        {
            List<Transform> childrenToCopyFrom = GetChildrenTransforms(parentToCopyFrom, false);
            for (int i = 0; i < parentToCopyTo.childCount; i++)
            {
                Transform childToCopyTo = parentToCopyTo.GetChild(i);
                Undo.RecordObject(childToCopyTo, "Copy Child Positions");
                childToCopyTo.position = childrenToCopyFrom[i].position;
            }
        }

        #endregion

        #region Sort
        
        public static void SortObjectsByPosition(List<GameObject> objectOrder)
        {
            int decimalsPrecision = 2;
            List<GameObject> newObjectsOrder = new List<GameObject>(objectOrder);

            SortGameObjectListByPosition(newObjectsOrder, decimalsPrecision);
            
            List<Vector3> newPositions = new List<Vector3>();
            foreach (GameObject newObject in newObjectsOrder)
            {
                newPositions.Add(newObject.transform.position);
            }

            for (int i = 0; i < objectOrder.Count; i++)
            {
                Undo.RecordObject(objectOrder[i].transform, "Sort Objects By Position");
                objectOrder[i].transform.position = newPositions[i];
            }
        }
        
        public static void SortGameObjectListBySiblingIndex(List<GameObject> gameObjects)
        {
            gameObjects.Sort((gameObject1, gameObject2) =>
            {
                int siblingIndex1 = gameObject1.transform.GetSiblingIndex();
                int siblingIndex2 = gameObject2.transform.GetSiblingIndex();
                if (siblingIndex1 < siblingIndex2)
                {
                    return -1;
                }
                
                return 1;
            });
        }

        public static void SortGameObjectListByPosition(List<GameObject> gameObjects, int decimalsPrecision)
        {
            float tolerance = Mathf.Pow(10, -decimalsPrecision);
            gameObjects.Sort((gameObject1, gameObject2) =>
            {
                Vector3 t1 = gameObject1.transform.position;
                Vector3 t2 = gameObject2.transform.position;
                if (t1.y > t2.y || (Math.Abs(Math.Round(t1.y, decimalsPrecision) - Math.Round(t2.y, decimalsPrecision)) < tolerance && t1.x < t2.x))
                {
                    return -1;
                }
                
                return 1;
            });
        }

        #endregion

        #region Selection

        public static GameObject GetOneSelectedObject()
        {
            if (Selection.activeGameObject == null || Selection.gameObjects.Length != 1)
            {
                Debug.LogError("Select one object");
                return null;
            }

            return Selection.gameObjects[0];
        }
        
        public static List<GameObject> GetSelectedGameObjects()
        {
            if (Selection.activeGameObject == null)
            {
                Debug.LogError("Select one or more object");
                return null;
            }

            return Selection.gameObjects.ToList();
        }
        
        #endregion

        #region Create

        public static List<GameObject> CreateGameObjects(string name = "GameObject", int gameObjectsCount = 1, Transform parent = null)
        {
            List<GameObject> gameObjects = new List<GameObject>();

            for (int i = 0; i < gameObjectsCount; i++)
            {
                gameObjects.Add(CreateGameObject(name, parent));
            }

            return gameObjects;
        }
        
        public static GameObject CreateGameObject(string name = "GameObject", Transform parent = null)
        {
            GameObject gameObject = new GameObject();
            Undo.RegisterCreatedObjectUndo(gameObject, "Create New GameObject");

            gameObject.transform.parent = parent;
            gameObject.name = name;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = Vector3.one;
            if (parent != null)
            {
                gameObject.layer = parent.gameObject.layer;
            }

            return gameObject;
        }

        #endregion

        #region Swapping

        public static void SwapTwoTransforms(Transform transform1, Transform transform2)
        {
            Undo.RecordObject(transform1, "Swap Two Transforms");
            Undo.RecordObject(transform2, "Swap Two Transforms");
            (transform1.position, transform2.position) = (transform2.transform.position, transform1.position);
            (transform1.localScale, transform2.localScale) = (transform2.transform.localScale, transform1.localScale);
            (transform1.rotation, transform2.rotation) = (transform2.transform.rotation, transform1.rotation);
        }

        #endregion
        
        #region Editor Settings
        
        /// <summary>
        /// Include scenes in build settings
        /// </summary>
        /// <param name="scenePaths">Provided paths to scenes</param>
        public static void ChangeEditorBuildSettings(List<string> scenePaths)
        {
            List<EditorBuildSettingsScene> sceneList = EditorBuildSettings.scenes.ToList();
            foreach (string scenePath in scenePaths)
            {
                EditorBuildSettingsScene scene = sceneList.Find(scene => scene.path == scenePath);
                if (scene != null)
                {
                    scene.enabled = true;
                }
                else
                {
                    Debug.LogError($"Scene {scenePath} doesn't exist");
                }
            }

            EditorBuildSettings.scenes = sceneList.ToArray();
        }
        
        #endregion
        
        #region Prefabs

        public static GameObject EnablePrefabEditingAndGetRoot(string prefabAssetPath)
        {
            return PrefabUtility.LoadPrefabContents(prefabAssetPath);
        }

        public static void DisablePrefabEditing(GameObject prefabRoot, string prefabAssetPath)
        {
            PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabAssetPath);
            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }
        
        static string GetPrefabAssetPathFromScene(string prefabRootName)
        {
            // If it is prefab mode
            PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();
            if (stage && stage.prefabContentsRoot.name == prefabRootName)
            {
                return stage.assetPath;
            }
            
            // If it is scene
            GameObject sceneRoomsHolder = GameObject.Find(prefabRootName);
            if (sceneRoomsHolder)
            {
                return PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(sceneRoomsHolder);
            }

            // Not found
            return "";
        }
        
        #endregion
    }
}
