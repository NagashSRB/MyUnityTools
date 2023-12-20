using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Tacic.Tacic___Unity_Tools.Editor
{
    public class ReferenceFinder : EditorWindow
    {
        private string guidToFind = string.Empty;

        private Object searchedObject;

        private Object oldSearchedObject;

        private Object replacementObject;

        private Dictionary<Object, int> referenceObjects = new Dictionary<Object, int>();

        private int referenceObjectsCount;

        private Vector2 scrollPosition;

        private Stopwatch searchTimer = new Stopwatch();

        private bool searchInAssets;

        private bool searchInScenes;

        private bool searchInPrefabs;

        private bool autoRefresh;

        [MenuItem("MidvaTools/Reference Finder", false, 50)]
        static void Init()
        {
            GetWindow(typeof(ReferenceFinder), false, "Reference Finder");
        }

        void OnGUI()
        {
            if (EditorSettings.serializationMode == SerializationMode.ForceText)
            {
                DisplayMainMenu();

                Search();

                if (referenceObjects.Count != 0)
                {
                    DisplayReferenceObjectList(ref referenceObjects);
                }
            }
        }

        private void DisplayMainMenu()
        {

            oldSearchedObject = searchedObject;

            searchedObject = EditorGUILayout.ObjectField(searchedObject != null ? searchedObject.name : "Drag & Drop Asset", searchedObject, typeof(Object), false);

            guidToFind = EditorGUILayout.TextField("GUID", guidToFind);

            if (searchedObject != oldSearchedObject)
            {
                if (searchedObject != null)
                {
                    guidToFind = GetGUID(searchedObject);
                }
                else
                {
                    guidToFind = string.Empty;
                }
            }

            DisplaySearchCategoryMenu();
        }

        private void DisplaySearchCategoryMenu()
        {
            searchInAssets = EditorGUILayout.Toggle("*.asset", searchInAssets);

            searchInPrefabs = EditorGUILayout.Toggle("*.prefab", searchInPrefabs);

            searchInScenes = EditorGUILayout.Toggle("*.unity", searchInScenes);
        }

        private void Search()
        {
            if (GUILayout.Button("Search"))
            {
                if (string.IsNullOrEmpty(guidToFind))
                {
                    DisplayDialog("Please select the object you are searching for.");

                    return;
                }

                searchTimer.Reset();

                searchTimer.Start();

                referenceObjects.Clear();

                var pathToAsset = AssetDatabase.GUIDToAssetPath(guidToFind);

                if (!string.IsNullOrEmpty(pathToAsset))
                {
                    searchedObject = AssetDatabase.LoadAssetAtPath<Object>(pathToAsset);

                    var allPathToAssetsList = GetAssetsWithSearchedReferencePaths();

                    string assetPath;

                    for (int i = 0; i < allPathToAssetsList.Count; i++)
                    {
                        assetPath = allPathToAssetsList[i];

                        var text = File.ReadAllText(assetPath);

                        var lines = text.Split('\n');

                        for (int j = 0; j < lines.Length; j++)
                        {
                            var line = lines[j];

                            if (line.Contains("guid:"))
                            {
                                if (line.Contains(guidToFind))
                                {
                                    var pathToReferenceAsset = assetPath.Replace(Application.dataPath, string.Empty);

                                    pathToReferenceAsset = pathToReferenceAsset.Replace(".meta", string.Empty);

                                    var path = pathToReferenceAsset;

                                    var asset = AssetDatabase.LoadAssetAtPath<Object>(path);

                                    if (asset != null)
                                    {
                                        if (!referenceObjects.ContainsKey(asset))
                                        {
                                            referenceObjects.Add(asset, 0);
                                        }
                                        referenceObjects[asset]++;
                                    }
                                    else
                                    {
                                        Debug.LogError(path + " could not be loaded");
                                    }

                                    referenceObjectsCount = referenceObjects.Count;
                                }
                            }
                        }
                    }
                    searchTimer.Stop();
                }
                else
                {
                    Debug.Log("Invalid asset path");
                }

                if (referenceObjects.Count == 0)
                {
                    DisplayDialog("No references were found in your selection!");
                }
            }
        }

        private List<string> GetAssetsWithSearchedReferencePaths()
        {
            var allPathToAssetsList = new List<string>();

            string selectionPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (searchInAssets)
            {
                var allAssets = Directory.GetFiles(selectionPath, "*.asset", SearchOption.AllDirectories);
                allPathToAssetsList.AddRange(allAssets);
            }

            if (searchInPrefabs)
            {
                var allPrefabs = Directory.GetFiles(selectionPath, "*.prefab", SearchOption.AllDirectories);
                allPathToAssetsList.AddRange(allPrefabs);
            }

            if (searchInScenes)
            {
                var allScenes = Directory.GetFiles(selectionPath, "*.unity", SearchOption.AllDirectories);
                allPathToAssetsList.AddRange(allScenes);
            }

            return allPathToAssetsList;
        }

        private void DisplayReferenceObjectList(ref Dictionary<Object, int> referenceObjectsDictionary)
        {
            GUILayout.Label("Referenced by: " + referenceObjectsCount + " assets. (Last search duration: " + searchTimer.Elapsed + ")");

            GUILayout.Label("Currently in list: " + referenceObjectsDictionary.Count);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            Object objectToRemove = null;

            foreach (var referenceObject in referenceObjectsDictionary)
            {
                var referencingObject = referenceObject.Key;

                var referenceCount = referenceObject.Value;

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.ObjectField(" Reference count: " + referenceCount, referencingObject, typeof(Object), false);

                if (GUILayout.Button("Replace", GUILayout.Width(100)))
                {
                    if (replacementObject == null)
                    {
                        DisplayDialog("Please select the replacement object.");
                    }
                    else
                    {
                        objectToRemove = referencingObject;

                        Replace(GetGUID(replacementObject), referencingObject);

                    }
                }

                if (GUILayout.Button("Clear", GUILayout.Width(100)))
                {
                    objectToRemove = referencingObject;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (objectToRemove != null)
            {
                referenceObjectsDictionary.Remove(objectToRemove); //remove the object from list after reference is replaced or object is cleared

                if (autoRefresh)
                {
                    RefreshAssets();
                }
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();

            replacementObject = EditorGUILayout.ObjectField("Replace With: ", replacementObject, searchedObject.GetType(), false);

            ReplaceAllButton();

            ClearAllButton();

            RefreshButton();

            AutoRefreshToggle();

            EditorGUILayout.EndHorizontal();
        }

        private void ReplaceAllButton()
        {
            if (GUILayout.Button("Replace All", GUILayout.Width(100)))
            {
                if (replacementObject == null)
                {
                    DisplayDialog("Please select the replacement object.");

                    return;
                }

                foreach (KeyValuePair<Object, int> _object in referenceObjects)
                {
                    Replace(GetGUID(replacementObject), _object.Key);
                }

                referenceObjects.Clear();

                if (autoRefresh)
                {
                    RefreshAssets();
                }
            }
        }

        private void ClearAllButton()
        {
            if (GUILayout.Button("Clear All", GUILayout.Width(100)))
            {
                referenceObjects.Clear();
            }
        }

        private void RefreshButton()
        {
            if (GUILayout.Button("Refresh", GUILayout.Width(100)))
            {
                RefreshAssets();
            }
        }

        private void RefreshAssets()
        {
            AssetDatabase.Refresh();
        }

        private void AutoRefreshToggle()
        {
            autoRefresh = EditorGUILayout.Toggle("Auto Refresh", autoRefresh);
        }

        private void Replace(string replacementGUID, Object objectWithSearchedReference)
        {
            StringWriter stringWriter = new StringWriter();

            string assetPath = GetAssetPath(objectWithSearchedReference);

            var text = File.ReadAllText(assetPath);

            var lines = text.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                if (line.Contains("guid:"))
                {
                    if (line.Contains(guidToFind))
                    {
                        string replaced = line.Replace(guidToFind, replacementGUID);

                        stringWriter.WriteLine(replaced);

                        continue;
                    }
                }
                stringWriter.WriteLine(line);
            }
            File.WriteAllText(assetPath, stringWriter.ToString());
        }

        private string GetAssetPath(Object _object)
        {
            return AssetDatabase.GetAssetPath(_object);
        }

        private string GetGUID(Object _object)
        {
            return AssetDatabase.AssetPathToGUID(GetAssetPath(_object));
        }

        private void DisplayDialog(string message)
        {
            EditorUtility.DisplayDialog("Message", message, "OK");
        }
    }    
}