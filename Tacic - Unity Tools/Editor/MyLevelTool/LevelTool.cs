using System;
using System.IO;
using System.Text.RegularExpressions;
using Tacic.Tacic___Unity_Tools.Editor.TacicEditorRework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tacic.Tacic___Unity_Tools.Editor.MyLevelTool
{
    public class LevelTool : EditorWindow
    {
        #region Level Path
        //TODO:  Remove region if moved this data from Level Creation Tool to LevelData
        static readonly string levelAssetsPath = "Assets/Levels";
        static readonly string levelTexturesPath = "Assets/Textures/Levels";
        static readonly string levelPrefabsPath = "Assets/Prefabs/Levels";
        static readonly string levelScenesPath = "Assets/Scenes";
        static readonly string levelTemplateScenePath = "Assets/Scenes/Level.unity";

        public string levelName;
        public string currentAssetsPath;
        public string currentScenePath;
        public string currentPrefabPath;
        public string currentTexturesPath;
        public string roomsHolderPath;

        #endregion

        
        private string pathToScriptableObject = "Assets/Tacic/Tacic - Unity Tools/Editor/MyLevelTool/LevelScriptableObject.asset";
        private ScriptableObjectCreator SOcreator = new ScriptableObjectCreator();
        private LevelScriptableObject levelData;
        private bool hideSettings;
        
        // GUI settings and styles
        private GUIStyle horizontalLine;
        private Vector2 scrollPosition = Vector2.zero;
        
        // TODO: Check if all assets are used.
        // TODO: Check if all sprites are used.
        // TODO: Jumping to saved checkpoint objects in hieararchy while collapsing others. Visible/clickable

        [MenuItem("Level Tools/Level Tool", false, 0)]
        static void CreateLevel()
        {
            GetWindow<LevelTool>("Level Tool").Show();
        }
        
        // TODO: Save on exit

        private void OnEnable()
        {
            horizontalLine = CreateHorizontalLineStyle();
            levelData = AssetDatabase.LoadAssetAtPath<LevelScriptableObject>(pathToScriptableObject);

            if (levelData != null)
            {
                //Load level (maybe separate function)
                LoadLevelData();
            }
        }

        private void LoadLevelData()
        {
            LoadData();
            SetUpLevelStrings();
            if (!IsLevelDataValid())
            {
                Debug.LogError("Level cannot be found. Rooms holder does not exists or folders missing");
            }
        }

        private bool IsLevelDataValid()
        {
            bool roomsHolderExists = AssetDatabase.LoadAssetAtPath<GameObject>(roomsHolderPath) != null;
            return roomsHolderExists;
        }

        private GUIStyle CreateHorizontalLineStyle()
        {
            horizontalLine = new GUIStyle();
            horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
            horizontalLine.fixedHeight = 2;
            horizontalLine.margin.top = horizontalLine.margin.bottom = 2;
            horizontalLine.stretchWidth = true;
            return horizontalLine;
        }

        private void SetUpLevelStrings()
        {
            currentAssetsPath = levelAssetsPath + "/" + levelName;
            currentTexturesPath = levelTexturesPath + "/" + levelName;
            currentPrefabPath = levelPrefabsPath + "/" + levelName;
            currentScenePath = levelScenesPath + "/" + levelName + ".unity";
            roomsHolderPath = currentPrefabPath + "/" + "RoomsHolder.prefab";
        }

        private void OnGUI()
        {
            using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPosition))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    using (new EditorGUILayout.VerticalScope())
                    {
                        //showButtons = GUILayout.Toggle(hideSettings, "Hide Settings");
                        ShowNavigatingButtons();
                    }
                    using (new EditorGUILayout.VerticalScope())
                    {
                        hideSettings = GUILayout.Toggle(hideSettings, "Hide Settings");
                        if (!hideSettings)
                        {
                            ShowSettings();
                        }

                        ShowAssetButtons();
                    }
                }
                
                scrollPosition = scrollView.scrollPosition;
            }
        }

        private void ShowAssetButtons()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                if (GUILayout.Button("TODO: Find unused assets"))
                {
                    
                }
            }
        }

        private void ShowNavigatingButtons()
        {
            GUILayout.Space(20);
            using (new EditorGUILayout.VerticalScope())
            {
                if (GUILayout.Button("Open Level Prefab"))
                {
                    // Open level prefab (mby function)
                    EditorSceneManager.OpenScene(currentScenePath);
                    AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<GameObject>(roomsHolderPath));
                    // TODO: Hide canvas
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Assets"))
                    {
                        Object assetObject = AssetDatabase.LoadAssetAtPath<Object>(currentAssetsPath);

                        // Check if the asset exists before pinging
                        if (assetObject != null)
                        {
                            Selection.activeObject = assetObject;
                            EditorGUIUtility.PingObject(assetObject);
                        }
                    }
                    
                    if (GUILayout.Button("Prefab"))
                    {
                        Object assetObject = AssetDatabase.LoadAssetAtPath<Object>(roomsHolderPath);

                        // Check if the asset exists before pinging
                        if (assetObject != null)
                        {
                            Selection.activeObject = assetObject;
                            EditorGUIUtility.PingObject(assetObject);
                        }
                    }
                    
                    if (GUILayout.Button("Textures"))
                    {
                        Object assetObject = AssetDatabase.LoadAssetAtPath<Object>(currentTexturesPath);

                        // Check if the asset exists before pinging
                        if (assetObject != null)
                        {
                            Selection.activeObject = assetObject;
                            EditorGUIUtility.PingObject(assetObject);
                        }
                    }
                    
                    if (GUILayout.Button("Scene"))
                    {
                        Object assetObject = AssetDatabase.LoadAssetAtPath<Object>(currentScenePath);

                        // Check if the asset exists before pinging
                        if (assetObject != null)
                        {
                            Selection.activeObject = assetObject;
                            EditorGUIUtility.PingObject(assetObject);
                        }
                    }
                }

                // Hotspots:
                EditorGUILayout.LabelField("Hotspots:");
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Turn hotspots ON"))
                    {
                        TacicEditorRooms.SetActiveHotspots(true, roomsHolderPath);
                    }
                    if (GUILayout.Button("Turn hotspots OFF"))
                    {
                        TacicEditorRooms.SetActiveHotspots(false, roomsHolderPath);
                    }
                }
                
                // Layers:
                EditorGUILayout.LabelField("Layers:");
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Set zoom element layers"))
                    {
                        TacicEditorRooms.SetLayersToAllZonesMenu(8, roomsHolderPath);
                    }
                }          
                
                // Rooms and zoom zones
                if (GUILayout.Button("Close zoom zones and set first room open"))
                {
                    TacicEditorRooms.CloseRoomsAndMiniGames(roomsHolderPath);
                }
                if (GUILayout.Button("Set zone size"))
                {
                    TacicEditorRooms.SetSizeForAllZones(roomsHolderPath);
                }
                GUILayout.Space(20);
                DrawHorizontalLine();
            }
        }

        private void ShowSettings()
        {
            GUILayout.Space(20);
            using (new EditorGUILayout.VerticalScope())
            {
                //pathToScriptableObject = EditorGUILayout.TextField("Scriptable object path:", pathToScriptableObject);
                levelName = EditorGUILayout.TextField("Level name:", levelName);
                GUILayout.Space(20);
                //GUILayout.Label("Save String to ScriptableObject", EditorStyles.boldLabel);
                if (GUILayout.Button("Save parameters"))
                {
                    string levelNamePattern = @"^Level\d{1,5}$";
                    if (!Regex.IsMatch(levelName, levelNamePattern))
                    {
                        Debug.LogError("Name must start with 'Level', and contain numbers after word 'Level'");
                        return;
                    }
                    
                    SaveDataToScriptableObject();
                    //Update class variables
                    LoadLevelData();
                }
                
                GUILayout.Space(20);
                DrawHorizontalLine();
            }
        }

        private void DrawHorizontalLine()
        {
            GUILayout.Box(GUIContent.none, horizontalLine);
        }

        private void SaveDataToScriptableObject()
        {
            levelData = File.Exists(pathToScriptableObject)
                ? AssetDatabase.LoadAssetAtPath<LevelScriptableObject>(pathToScriptableObject)
                : SOcreator.CreateScriptableObject<LevelScriptableObject>(pathToScriptableObject);

            if (levelData == null)
            {
                Debug.LogError("LevelData SO doesn't exist");
                return;
                // Mozda ne treba
            }
                    
            // Fill up data
            SaveData();

            // Finalize
            EditorUtility.SetDirty(levelData);
            AssetDatabase.SaveAssets();
        }

        private void SaveData()
        {
            levelData.name = levelName;
            levelData.hideSettings = hideSettings;
        }

        private void LoadData()
        {
            levelName = levelData.name;
            hideSettings = levelData.hideSettings;
        }
    }
}