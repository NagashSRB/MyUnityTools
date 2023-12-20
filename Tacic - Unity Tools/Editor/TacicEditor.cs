// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using UnityEditor;
// using UnityEditor.SceneManagement;
// using UnityEditorInternal;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using Tacic.Tacic___Unity_Tools.Scripts;
// using UnityEditor.Experimental.SceneManagement;
//
// namespace Tacic.Tacic___Unity_Tools.Editor
// {
//     public class TacicEditor : EditorWindow
//     {
//         #region HelperFunctions
//         
//         [MenuItem("GameObject/Talenzzo/Dodaj brojeve pored imena - Parent", false, 0)]
//         static void RenameObjects()
//         {
//             if (Selection.activeGameObject == null)
//                 return;
//
//             GameObject parent = Selection.gameObjects[0];
//             int i = 0;
//             foreach (Transform obj in parent.transform)
//             {
//                 obj.gameObject.name += i;
//                 i++;
//                 EditorUtility.SetDirty(obj);
//             }
//
//         }
//         
//         [MenuItem("Talenzzo/Promeni redosled objekta na osnovu pozicije - Select", false, 0)]
//         static void SortByPosition()
//         {
//             // TODO : Da ima editor window da se unesu dimenzije matrice/liste 
//             if (Selection.activeGameObject == null)
//                 return;
//             List<GameObject> objects = Selection.gameObjects.ToList();
//
//             Transform parent = objects[0].transform.parent;
//             foreach (GameObject selectedObject in objects)
//             {
//                 selectedObject.transform.parent = null;
//             }
//
//             objects.Sort((GameObject g1, GameObject g2) =>
//             {
//                 Vector3 t1 = g1.transform.position;
//                 Vector3 t2 = g2.transform.position;
//                 if (t1.y > t2.y || (Math.Round(t1.y, 2) == Math.Round(t2.y, 2) && t1.x < t2.x))
//                     return -1;
//                 return 1;
//             });
//
//             foreach (GameObject holder in objects)
//             {
//                 holder.transform.parent = parent;
//             }
//         }
//
//         [MenuItem("GameObject/Talenzzo/Napravi holdere za child objekte - Parent", false, 0)]
//         static void CreateHolders()
//         {
//             if (Selection.activeGameObject == null)
//                 return;
//             GameObject fields = Selection.gameObjects[0];
//             GameObject holders = new GameObject("PositionHolders")
//             {
//                 transform =
//                 {
//                     parent = fields.transform.parent,
//                     localPosition = Vector3.zero
//                 }
//             };
//
//             int i = 0;
//             foreach (Transform child in fields.transform)
//             {
//                 GameObject holder = new GameObject("PositionHolder" + i);
//                 holder.transform.parent = holders.transform;
//                 holder.transform.localPosition = child.localPosition;
//                 i++;
//             }
//
//             EditorUtility.SetDirty(holders);
//         }
//
//         [MenuItem("GameObject/Talenzzo/Zaokruzi poziciju na 4 decimale - Select", false, 0)]
//         static void RoundBy4()
//         {
//             if (Selection.activeGameObject == null)
//                 return;
//             foreach (GameObject gameObject in Selection.gameObjects)
//             {
//                 var localPosition = gameObject.transform.localPosition;
//                 gameObject.transform.localPosition = new Vector3(
//                     (float) Math.Round(localPosition.x, 4),
//                     (float) Math.Round(localPosition.y, 4),
//                     (float) Math.Round(localPosition.z, 4));
//                 EditorUtility.SetDirty(gameObject);
//             }
//         }
//
//
//
//         [MenuItem("GameObject/Talenzzo/Podesi pozicije childova kao u PositionHolders - Parent", false, 0)]
//         static void SetObjectPositionAsHolder()
//         {
//             if (Selection.activeGameObject == null)
//                 return;
//
//             Transform parent = Selection.gameObjects[0].transform;
//             Transform holders = parent.transform.parent.Find("PositionHolders");
//
//             if (parent.childCount != holders.childCount)
//                 return;
//             for (int i = 0; i < parent.childCount; i++)
//             {
//                 parent.GetChild(i).position = holders.GetChild(i).position;
//             }
//
//             EditorUtility.SetDirty(parent);
//         }
//
//         #endregion
//
//         [MenuItem("GameObject/Talenzzo/Add|Remove Solve", false, 0)]
//         static void AddRemoveSolve()
//         {
//             GameObject roomsHolder = GameObject.Find("RoomsHolder");
//             foreach (Transform room in roomsHolder.transform)
//             {
//                 foreach (Transform obj in room)
//                 {
//                     if (obj.gameObject.GetComponent<MiniGame>())
//                     {
//                         SolveMiniGameInEditorTacic solve = obj.gameObject.GetComponent<SolveMiniGameInEditorTacic>();
//                         if (!solve)
//                             obj.gameObject.AddComponent<SolveMiniGameInEditorTacic>();
//                         else
//                             DestroyImmediate(solve);
//                         EditorUtility.SetDirty(obj);
//                     }
//                 }
//             }
//
//             PrefabUtility.ApplyPrefabInstance(roomsHolder, InteractionMode.AutomatedAction);
//             EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
//         }
//
//         [MenuItem("GameObject/Talenzzo/In Test - Animation Curves", false, 0)]
//         static void AnimationCurves()
//         {
//             // Nadjemo AnimationClip
//             string assetPath = "Assets/Levels/Level213/Animations/TabletSortingMGAnimations/TabletSortingMGAnimation.anim";
//             AnimationClip animationClip = (AnimationClip) AssetDatabase.LoadAssetAtPath(
//                 assetPath, typeof(AnimationClip));
//             if (animationClip == null)
//             {
//                 Debug.Log("Animacija nije pronadjena");
//                 return;
//             }
//
//             // Nadjemo sve Curve Bindinge za animation clip
//             EditorCurveBinding[] bindingsArray = AnimationUtility.GetCurveBindings(animationClip);
//             EditorCurveBinding targetBinding = bindingsArray[0];
//             // Nadjemo binding sa propertyjem kojim zelimo
//             foreach (EditorCurveBinding binding in bindingsArray)
//             {
//                 if (binding.propertyName == "m_LocalPosition.x")
//                 {
//                     targetBinding = binding;
//                     break;
//                 }
//             }
//
//             // Nadjemo AnimationCurve za taj binding
//             AnimationCurve curve = AnimationUtility.GetEditorCurve(animationClip, targetBinding);
//
//             // Nadjemo mg objekat gde treba da se referencira
//             GameObject minigameGO = GameObject.Find("TabletSortingPuzzleMGMiniGame");
//             //TabletSortingGameplayManager manager = minigameGO.GetComponent<TabletSortingGameplayManager>();
//             // manager.curve = new AnimationCurve();
//             // manager.curve.keys = curve.keys;
//             // manager.curve.preWrapMode = curve.preWrapMode;
//             // manager.curve.postWrapMode = curve.postWrapMode;
//             //brisi komentare kad treba
//
//             GameObject roomsHolder = GameObject.Find("RoomsHolder");
//             //EditorUtility.SetDirty(manager);
//             EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
//             PrefabUtility.ApplyPrefabInstance(roomsHolder, InteractionMode.AutomatedAction);
//             EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
//             //PrefabUtility.RecordPrefabInstancePropertyModifications(roomsHolder);
//         }
//     
//
//         [MenuItem("Talenzzo/Swap 2 Selected object position", false, 0)]
//         static void SwapSelectedObjectTransform()
//         {
//             GameObject[] gameObjects = GetSelectedGameObjects();
//             if (gameObjects.Length == 2)
//             {
//                 (gameObjects[0].transform.position, gameObjects[1].transform.position) =
//                     (gameObjects[1].transform.position, gameObjects[0].transform.position);
//                 Debug.Log("Zamenjene pozicije");
//                 EditorUtility.SetDirty(gameObjects[0]);
//                 EditorUtility.SetDirty(gameObjects[1]);
//             }
//             else
//             {
//                 Debug.Log("Nisu zamenjene pozicije");
//             }
//         }
//     
//         // [MenuItem("Talenzzo/Temp Custom", false, 0)]
//         // static void TempCustom()
//         // {
//         //     GameObject[] gameObjects = GetSelectedGameObjects();
//         //     GameObject my = GameObject.Find("TrailCenter");
//         //     foreach (GameObject gameObject in gameObjects)
//         //     {
//         //         GameObject newObj = Instantiate(my);
//         //         newObj.transform.parent = gameObject.transform;
//         //         EditorUtility.SetDirty(newObj);
//         //     }
//         // }
//         
//
//         #region GameObjectMenu
//     
//         [MenuItem("GameObject/Pokretanje levela - Tacic/Pokreni level", false, 0)]
//         static void RunLevel()
//         {
//             ChangeEditorBuildSettingsForTest();
//             string roomsHolderAssetPath = GetRoomsHolderAssetPath();
//             SaveLevelEditScene();
//             if (roomsHolderAssetPath != "")
//             {
//                 UpdateRoomsHolder(roomsHolderAssetPath);
//                 SetUpTestLevelScene(roomsHolderAssetPath, false);
//             }
//             
//             EnterPlayMode();
//         }
//         
//         [MenuItem("GameObject/Pokretanje levela - Tacic/Pokreni prethodni level", false, 0)]
//         static void RunPreviousLevel()
//         {
//             EnterPlayMode();
//         }
//
//         [MenuItem("GameObject/Pokretanje levela - Tacic/IN TEST = Pokreni level od koraka", false, 0)]
//         static void RunLevelWithHint()
//         {
//             ChangeEditorBuildSettingsForTest();
//             int hintPriority = GetSelectedGameObjectHintPriorityAttribute();
//             if (hintPriority == -1)
//             {
//                 Debug.Log($"Object doesnt have hint priority attribute or it is not valid");
//                 return;
//             }
//             string roomsHolderAssetPath = GetRoomsHolderAssetPath();
//             SaveLevelEditScene();
//             if (roomsHolderAssetPath != "")
//             {
//                 UpdateRoomsHolder(roomsHolderAssetPath);
//                 SetUpTestLevelScene(roomsHolderAssetPath, true, hintPriority);
//             }
//             
//             EnterPlayMode();
//         }
//
//         static void TurnOnHotspots(Transform roomsHolder)
//         {
//             foreach (Transform room in roomsHolder.transform)
//             {
//                 var hotspots = room.GetComponentsInChildren<Hotspot>(true);
//                 Debug.Log($"Child counts: {hotspots.Length}");
//                 foreach (var hotspot in hotspots)
//                 {
//                     hotspot.gameObject.SetActive(true);
//                     hotspot.transform.GetChild(0).gameObject.SetActive(true);
//                 }
//             }
//         }
//         
//     
//         [MenuItem("GameObject/Pokretanje levela - Tacic/TEST = Vrati se na level", false, 0)]
//         static void GetBackToLevel()
//         {
//             // TODO: Nije islo, da se napravi da izadje iz play moda i udje na scenu. tako sto se nadje
//             GameObject roomsHolder = GameObject.Find("AudioObjects");
//             string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(roomsHolder);
//             //string path = AssetDatabase.GetAssetPath(roomsHolder);
//             Debug.Log(path);
//         
//             Debug.Log(SceneManager.GetActiveScene().name);
//             if (EditorApplication.isPlaying)
//             {
//                 EditorApplication.ExitPlaymode();
//             }
//             
//             
//             
//             // var parameters = new LoadSceneParameters(LoadSceneMode.Additive);
//             Scene scene = EditorSceneManager.OpenScene("Assets/Scenes/TestLevel.unity");
//             // //SceneManager.SetActiveScene(EditorSceneManager.OpenScene("Assets/Scenes/TestLevel.unity"));
//             // GameObject[] objects = scene.GetRootGameObjects();
//             // GameObject roomsHolder = null;
//             // foreach (GameObject gameObject in objects)
//             // {
//             //     if (gameObject.name == "RoomsHolder")
//             //     {
//             //         roomsHolder = gameObject;
//             //     }
//             // }
//             //
//             // if (roomsHolder == null)
//             // {
//             //     Debug.Log("Holder je null");
//             //     return;
//             // }
//             // string path = AssetDatabase.GetAssetPath(roomsHolder);// = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(roomsHolder);
//             // Debug.Log(path);
//             // SceneManager.UnloadSceneAsync(scene);
//         }
//         static void EnterPlayMode()
//         {
//             EditorSceneManager.OpenScene("Assets/Scenes/TestLevelSplash.unity");
//             //SceneManager.SetActiveScene();
//             EditorApplication.EnterPlaymode();
//         }
//
//         static void ChangeEditorBuildSettingsForTest()
//         {
//             ChangeEditorBuildSettings(new List<string>(2)
//             {
//                 "Assets/Scenes/TestLevel.unity",
//                 "Assets/Scenes/TestLevelSplash.unity"
//             });
//         }
//
//         static void ChangeEditorBuildSettings(List<string> scenePaths)
//         {
//             List<EditorBuildSettingsScene> sceneList = EditorBuildSettings.scenes.ToList();
//             foreach (string scenePath in scenePaths)
//             {
//                 EditorBuildSettingsScene scene = sceneList.Find(scene => scene.path == scenePath);
//                 if (scene != null)
//                 {
//                     scene.enabled = true;
//                 }
//                 else
//                 {
//                     Debug.LogError("Ta scena ne postoji!");
//                 }
//             }
//
//             EditorBuildSettings.scenes = sceneList.ToArray();
//         }
//
//         static void SaveLevelEditScene()
//         {
//             // If we are on the scene, save scene and override prefab first
//             GameObject sceneRoomsHolder = GetRoomsHolderOnScene();
//             if (sceneRoomsHolder)
//             {
//                 PrefabUtility.ApplyPrefabInstance(sceneRoomsHolder, InteractionMode.AutomatedAction);
//                 EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
//             }
//         }
//
//         static void UpdateRoomsHolder(string roomsHolderAssetPath)
//         {
//             GameObject roomsHolderPrefabRoot = PrefabUtility.LoadPrefabContents(roomsHolderAssetPath);
//             
//             CloseZoomZonesAndRooms(roomsHolderPrefabRoot);
//             TurnOnHotspots(roomsHolderPrefabRoot.transform);
//
//             PrefabUtility.SaveAsPrefabAsset(roomsHolderPrefabRoot, roomsHolderAssetPath);
//             PrefabUtility.UnloadPrefabContents(roomsHolderPrefabRoot);
//         }
//
//         static void CloseZoomZonesAndRooms(GameObject roomsHolder)
//         {
//             int zoomElementLayer = 8;
//             foreach (Transform room in roomsHolder.transform)
//             {
//                 foreach (Transform obj in room)
//                 {
//                     if (obj.gameObject.GetComponent<ZoomZone>() || obj.gameObject.GetComponent<MiniGame>())
//                     {
//                         if(obj.GetChild(0).gameObject.activeSelf)
//                         {
//                             obj.GetChild(0).gameObject.SetActive(false);
//                             Debug.Log($"Disabled holder: " + obj.name);
//                         }
//                         
//                         SetLayerAllChildren(obj.transform, zoomElementLayer);
//                     }
//                 }
//
//                 if (room.gameObject.name != "AudioObjects")
//                     room.gameObject.SetActive(false);
//             }
//
//             roomsHolder.transform.GetChild(0).gameObject.SetActive(true);
//         }
//         
//         static void SetLayerAllChildren(Transform root, int layer)
//         {
//             var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
//             foreach (var child in children)
//             {
//                 child.gameObject.layer = layer;
//             }
//         }
//
//         static void SetUpTestLevelScene(string newRoomsHolderAssetPath, bool shouldStartFromHint, int hintOrder = -1)
//         {
//             //SceneManager.SetActiveScene(EditorSceneManager.OpenScene("Assets/Scenes/TestLevel.unity"));
//             
//             Scene testLevelScene = EditorSceneManager.OpenScene("Assets/Scenes/TestLevel.unity");
//             GameObject oldRoomsHolder = GetRoomsHolderOnScene();
//             GameObject newRoomsHolder = ChangeRoomsHolderInTestLevelScene(oldRoomsHolder, newRoomsHolderAssetPath);
//             
//             if (shouldStartFromHint)
//             {
//                 SetLevelProgressController(newRoomsHolder, hintOrder);
//             }
//             
//             EditorSceneManager.SaveScene(testLevelScene);
//         }
//
//         // Promena RoomsHoldera u TestLevel sceni iz scene u kojoj se pravi level
//         static GameObject ChangeRoomsHolderInTestLevelScene(GameObject oldRoomsHolder, string roomsHolderAssetPath)
//         {
//             //string sceneName = SceneManager.GetActiveScene().name;
//             // if (sceneName == "TestLevel" || sceneName == "TestLevelSplash") return;
//             
//             //string holderLocation = roomsHolderAssetPath;
//             // //string holderLocation = "Assets/Prefabs/Levels/" + sceneName;
//             // DirectoryInfo dirInfo = new DirectoryInfo(holderLocation);
//             // if (!dirInfo.Exists)
//             //     return;
//             
//             if (oldRoomsHolder != null)
//             {
//                 string oldPrefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(oldRoomsHolder);
//                 if (oldPrefabPath == roomsHolderAssetPath)
//                 {
//                     Debug.Log($"Isti level je pokrenut");
//                     return oldRoomsHolder;
//                 }
//                 
//                 DestroyImmediate(oldRoomsHolder);
//             }
//
//             GameObject roomsHolderPrefabAsset =
//                 AssetDatabase.LoadAssetAtPath(roomsHolderAssetPath, typeof(GameObject)) as GameObject;
//             GameObject roomsHolderPrefabRoot = (GameObject) PrefabUtility.InstantiatePrefab(roomsHolderPrefabAsset, SceneManager.GetActiveScene());
//             roomsHolderPrefabRoot.transform.SetAsLastSibling();
//             return roomsHolderPrefabRoot;
//         }
//         
//         static void SetLevelProgressController(GameObject roomsHolder, int hintOrder)
//         {
//             LevelProgressController levelProgressController =
//                 GameObject.Find("LevelProgressController")?.GetComponent<LevelProgressController>();
//
//             if (levelProgressController == null)
//             {
//                 Debug.LogError($"Cannot find level progress controller component");
//                 return;
//             }
//             
//             SerializedObject serializedLevelProgress = new SerializedObject(levelProgressController);
//             string hintOrderPropertyName = "numberOfHintsToTestInclusive";
//             string targetRoomsHolderPropertyName = "roomsHolderFromTestLevelScene";
//             SerializedProperty hintOrderProperty = serializedLevelProgress.FindProperty(hintOrderPropertyName);
//             SerializedProperty targetRoomsHolderProperty = serializedLevelProgress.FindProperty(targetRoomsHolderPropertyName);
//             serializedLevelProgress.Update();
//
//             targetRoomsHolderProperty.objectReferenceValue = roomsHolder;
//             hintOrderProperty.intValue = hintOrder;
//             serializedLevelProgress.ApplyModifiedProperties();
//         }
//
//         static string GetRoomsHolderAssetPath()
//         {
//             PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();
//             if (stage && stage.prefabContentsRoot.name == "RoomsHolder")
//             {
//                 return stage.assetPath;
//             }
//             
//             GameObject sceneRoomsHolder = GameObject.Find("RoomsHolder");
//             if (sceneRoomsHolder)
//             {
//                 return PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(sceneRoomsHolder);
//             }
//
//             return "";
//         }
//
//         // -1 if not found
//         [MenuItem("GameObject/Pokretanje levela - Tacic/TEST = Da li objekat ima hint priority", false, 0)]
//         static int GetSelectedGameObjectHintPriorityAttribute()
//         {
//             GameObject[] selectedObjects = GetSelectedGameObjects();
//             if (selectedObjects == null || selectedObjects.Length != 1)
//             {
//                 Debug.LogError($"Select one object where you want to start level from");
//                 return -1;
//             }
//
//             GameObject selectedObject = selectedObjects[0];
//             Component[] components = selectedObject.GetComponents(typeof(Component));
//             foreach(Component component in components) 
//             { 
//                 SerializedObject serializedComponent = new SerializedObject(component);
//                 serializedComponent.Update();
//                 string hintPriorityPropertyName = "hintPriority";
//                 SerializedProperty hintPriorityProperty = serializedComponent.FindProperty(hintPriorityPropertyName);
//                 if (hintPriorityProperty == null)
//                 {
//                     Debug.Log($"HintPriority not found for object {component.name} / {component.ToString()}");
//                     continue;
//                 }
//
//                 Debug.Log($"HintPriority :Component: {component.name} / {component.ToString()}, Value:{hintPriorityProperty.intValue}");
//                 return hintPriorityProperty.intValue;
//                 //serializedComponent.ApplyModifiedProperties();
//             }
//
//             return -1;
//         }
//         
//         
//         [MenuItem("GameObject/Pokretanje levela - Tacic/TEST = Set up level progress", false, 0)]
//         static void PrintRoomsAssetPath()
//         {
//             string roomsHolderAssetPath = GetRoomsHolderAssetPath();
//             //UpdateRoomsHolder(roomsHolderAssetPath);
//             SetUpTestLevelScene(roomsHolderAssetPath, true, 5);
//         }
//
//         #endregion
//
//         #region Editor Window
//
//         private void OnEnable()
//         {
//             //ResetWindow();
//             Instance = this;
//             //previousState = EditorWindowState.None;
//             //currentState = EditorWindowState.None;
//         }
//
//         public static TacicEditor Instance;
//
//         private void OnGUI()
//         {
//             EditorGUILayout.Space();
//             EditorGUILayout.BeginVertical("box");
//             EditorGUILayout.Space();
//
//             SwitchWindowState();
//
//             EditorGUILayout.BeginHorizontal();
//             EditorGUILayout.Space();
//             EditorGUILayout.Space();
//             bool okButton = GUILayout.Button("OK", GUILayout.Width(140f));
//             bool cancelButton = GUILayout.Button("Cancel", GUILayout.Width(140f));
//             EditorGUILayout.EndHorizontal();
//             EditorGUILayout.EndVertical();
//
//             if (okButton)
//             {
//                 OkButtonClicked();
//             }
//
//             if (cancelButton)
//             {
//                 CancelButtonClicked();
//             }
//         }
//
//         static void CancelButtonClicked()
//         {
//             Instance.Close();
//         }
//
//         static void OkButtonClicked()
//         {
//             okClicked();
//             Instance.Close();
//         }
//
//         static void Show(EditorWindowState stt)
//         {
//             if (Instance == null)
//             {
//                 previousWindowState = currentWindowState;
//                 currentWindowState = stt;
//                 if (previousWindowState != null && previousWindowState != currentWindowState)
//                 {
//                     ResetWindow();
//                 }
//
//                 TacicEditor setObjectNamePopup = new TacicEditor();
//
//                 setObjectNamePopup.minSize = new Vector2(400f, 500f);
//                 setObjectNamePopup.maxSize = new Vector2(1000f, 1000f);
//                 setObjectNamePopup.titleContent.text = "Custom Editor Window u pokusaju ";
//
//                 setObjectNamePopup.ShowUtility();
//             }
//         }
//
//         #endregion
//
//         #region Custom Windows
//
//         #region Custom Windows Settings
//
//         static void SwitchWindowState()
//         {
//             switch (currentWindowState)
//             {
//                 case EditorWindowState.TestWindow:
//                     TestWindow();
//                     break;
//                 case EditorWindowState.SearchByNameWindow:
//                     SearchByNameWindow();
//                     break;
//                 case EditorWindowState.ReferenceSelfComponentWindow:
//                     ReferenceSelfComponentWindow();
//                     break;
//                 case EditorWindowState.CreateMatrixWindow:
//                     CreateMatrixWindow();
//                     break;
//                 case EditorWindowState.EditPositionListWindow:
//                     EditPositionListWindow();
//                     break;
//                 case EditorWindowState.ReferenceMatrixWindow:
//                     ReferenceMatrixWindow();
//                     break;
//                 case EditorWindowState.CreateGameObjectsWindow:
//                     CreateGameObjectsWindow();
//                     break;
//             }
//         }
//
//         static void ResetWindow()
//         {
//             inputString = "";
//             inputString2 = "";
//             inputString3 = "";
//             inputBool1 = false;
//             inputBool2 = false;
//             inputFloat = 0;
//             inputFloat2 = 0;
//             inputInt = 0;
//             inputInt2 = 0;
//             inputVector2 = Vector2.zero;
//         }
//
//         public enum EditorWindowState
//         {
//             None,
//             TestWindow,
//             SearchByNameWindow,
//             ReferenceSelfComponentWindow,
//             CreateMatrixWindow,
//             EditPositionListWindow,
//             ReferenceMatrixWindow,
//             CreateGameObjectsWindow
//         }
//
//         static Action okClicked;
//         public static EditorWindowState currentWindowState;
//         public static EditorWindowState previousWindowState;
//         private static string inputString;
//         private static string inputString2;
//         private static string inputString3;
//         private static bool inputBool1;
//         private static bool inputBool2;
//         private static float inputFloat;
//         private static float inputFloat2;
//         private static int inputInt;
//         private static int inputInt2;
//         private static GameObject inputGameObject;
//         private static Component inputComponent;
//         private static Vector2 inputVector2;
//     
//
//         #endregion
//     
//         #region TemplateWindow
//     
//         static void TemplateWindow()
//         {
//             inputString = EditorGUILayout.TextField("String za ispisivanje:", inputString);
//             TemplateWindowUpdate();
//         }
//
//         // [MenuItem("GameObject/Talenzzo/In Test - Editor Window", false, 0)]
//         static void TemplateOption()
//         {
//             okClicked = OnTestFinished;
//             Show(EditorWindowState.TestWindow);
//         }
//
//         static void OnTemplateFinished()
//         {
//             Debug.Log(inputString);
//         }
//
//         static void TemplateWindowUpdate()
//         {
//         
//         }
//     
//         #endregion
//
//         #region TestWindow
//
//         static void TestWindow()
//         {
//             inputString = EditorGUILayout.TextField("String za ispisivanje:", inputString);
//             // toggle / checkbox
//             // boolX = EditorGUILayout.Toggle("Open", boolX, GUILayout.ExpandWidth(false), GUILayout.Height(60f));
//
//             // int field
//             // intX = EditorGUILayout.IntField("Broj stanja", intX);
//
//             // object reference, example: sound. Mozda podesavanje da moze i sa scene
//             // soundX = (AudioClip)EditorGUILayout.ObjectField("Zvuk", soundX, typeof(AudioClip), false, GUILayout.ExpandWidth(true));
//
//             // EditorGUILayout.DropdownButton(new GUIContent("Choose component"), FocusType.Keyboard);
//         
//             // inputFloat = EditorGUILayout.Slider("Tekst", inputFloat, 0, 5);
//         }
//
//         // [MenuItem("GameObject/Talenzzo/In Test - Editor Window", false, 0)]
//         static void TestMenuOption()
//         {
//             okClicked = OnTestFinished;
//             Show(EditorWindowState.TestWindow);
//         }
//
//         static void OnTestFinished()
//         {
//             Debug.Log(inputString);
//         }
//
//         #endregion
//
//         #region SearchByNameWindow
//
//         static void SearchByNameWindow()
//         {
//             inputString = EditorGUILayout.TextField("Tacno ime objekta (case sensitive) :", inputString);
//         }
//
//         [MenuItem("GameObject/Talenzzo/Search selected objects by exact name", false, 0)]
//         static void SeachByNameOption()
//         {
//             okClicked = OnSearchByNameWindowFinished;
//             Show(EditorWindowState.SearchByNameWindow);
//         }
//
//         static void OnSearchByNameWindowFinished()
//         {
//             GameObject[] gameObjects = GetSelectedGameObjects();
//             if (gameObjects == null)
//             {
//                 return;
//             }
//
//             foreach (GameObject gameObject in gameObjects)
//             {
//                 if (gameObject.name != inputString)
//                 {
//                     ArrayUtility.Remove(ref gameObjects, gameObject);
//                     //gameObjects.Remove(gameObject);
//                 }
//             }
//
//             Selection.objects = gameObjects;
//         }
//
//         #endregion
//
//         #region ReferenceSelfComponentWindow
//
//
//         static void ReferenceSelfComponentWindow()
//         {
//             inputString = EditorGUILayout.TextField("Ime klase: ", inputString);
//             inputString2 = EditorGUILayout.TextField("Ime atributa: ", inputString2);
//             inputString3 = EditorGUILayout.TextField("Tip koji se referencira: ", inputString3);
//
//             EditorGUILayout.Space();
//             EditorGUILayout.BeginHorizontal();
//             inputBool1 = EditorGUILayout.Toggle("Get component from parent", inputBool1, GUILayout.ExpandWidth(false),
//                 GUILayout.Height(60f));
//             inputBool2 = EditorGUILayout.Toggle("Get component from self ", inputBool2, GUILayout.ExpandWidth(false),
//                 GUILayout.Height(60f));
//             EditorGUILayout.EndHorizontal();
//         }
//
//         [MenuItem("GameObject/Talenzzo/In Test - Reference components - Select", false, 0)]
//         static void ReferenceSelfComponentOption()
//         {
//             okClicked = OnReferenceSelfComponentWindowFinished;
//             Show(EditorWindowState.ReferenceSelfComponentWindow);
//         }
//
//
//         static void OnReferenceSelfComponentWindowFinished()
//         {
//             GameObject[] gameObjects = GetSelectedGameObjects();
//             foreach (GameObject gameObject in gameObjects)
//             {
//                 // inputString - Ime klase, Symemtry, inputString2 - ime atributa, inputString3 - komponenta za referenciranje
//                 var myClassComponent = gameObject.GetComponent(inputString);
//                 if (!myClassComponent)
//                 {
//                     Debug.Log("Ne postoji ta komponenta");
//                     return;
//                 }
//
//                 var myType = myClassComponent.GetType();
//                 var targetField = myType.GetField(inputString2);
//                 // Self
//                 var componentForReferencing = gameObject.GetComponent(inputString3);
//
//                 if (inputBool1)
//                 {
//                     // Get From Parent
//                     componentForReferencing = gameObject.transform.parent.GetComponent(inputString3);
//                 }
//                 else if (inputBool2)
//                 {
//                     // Get From Self
//                 }
//                 else
//                 {
//                     return;
//                 }
//
//                 if (targetField == null)
//                 {
//                     Debug.Log("Ne postoji taj atribut ili je private");
//                     return;
//                 }
//
//                 if (targetField.FieldType.Name != componentForReferencing.GetType().Name)
//                 {
//                     Debug.Log("Nisu isti tipovi");
//                     return;
//                 }
//
//                 targetField.SetValue(myClassComponent, componentForReferencing);
//                 EditorUtility.SetDirty(gameObject);
//                 ApplyOverrides();
//             }
//         }
//
//         #endregion
//     
//         #region CreateMatrixWindow
//     
//         static void CreateMatrixWindow()
//         {
//             EditorGUILayout.BeginHorizontal();
//             inputInt = EditorGUILayout.IntField("Number of rows", inputInt);
//             inputInt2 = EditorGUILayout.IntField("Number of columns", inputInt2);
//             EditorGUILayout.EndHorizontal();
//             inputFloat = EditorGUILayout.Slider("X osa", inputFloat, 0, 5);
//             inputFloat2 = EditorGUILayout.Slider("Y osa", inputFloat2, 0, 5);
//             CreateMatrixWindowUpdate(inputFloat, inputFloat2);
//         }
//
//         private static bool isCreateMatrixWindowActive;
//         private static GameObject[] createMatrixWindowSelectedObjects;
//
//         static void CreateMatrixWindowUpdate(float value, float value2)
//         {
//             value = (float) Math.Round(value, 3);
//             value2 = (float) Math.Round(value2, 3);
//             if (createMatrixWindowSelectedObjects == null || createMatrixWindowSelectedObjects.Length != inputInt * inputInt2)
//             {
//                 return;
//             }
//
//         
//             for (int i = 0; i < inputInt; i++)
//             {
//                 var position = createMatrixWindowSelectedObjects[0].transform.localPosition;
//                 for (int j = 0; j < inputInt2; j++)
//                 {
//                     if (j == 0)
//                         continue;
//                     int index = i * inputInt2 + j;
//                     position.x = position.x + value;
//                     var localPosition = createMatrixWindowSelectedObjects[index].transform.localPosition;
//                     createMatrixWindowSelectedObjects[index].transform.localPosition = new Vector3(position.x, localPosition.y, localPosition.z);
//                 }
//             }
//
//             for (int j = 0; j < inputInt2; j++)
//             {
//                 var position = createMatrixWindowSelectedObjects[0].transform.localPosition;
//                 for (int i = 0; i < inputInt; i++)
//                 {
//                     if (i == 0)
//                         continue;
//                     int index = i * inputInt2 + j;
//                     position.y = position.y + value2;
//                     var localPosition = createMatrixWindowSelectedObjects[index].transform.localPosition;
//                     createMatrixWindowSelectedObjects[index].transform.localPosition = new Vector3(localPosition.x, position.y, localPosition.z);
//                 }
//             }
//         }
//
//         [MenuItem("Talenzzo/Set Distance Between GameObject - Matrix", false, 0)]
//         static void CreateMatrixOption()
//         {
//             okClicked = OnCreateMatrixFinished;
//             isCreateMatrixWindowActive = true;
//             inputFloat = 0.2f;
//             inputFloat2 = 0.2f;
//             inputInt = 1;
//             inputInt2 = 1;
//             createMatrixWindowSelectedObjects = GetSelectedGameObjects();
//             Show(EditorWindowState.CreateMatrixWindow);
//         }
//
//         static void OnCreateMatrixFinished()
//         {
//             isCreateMatrixWindowActive = false;
//         }
//     
//         #endregion
//     
//         #region EditPositionListWindow
//     
//         static void EditPositionListWindow()
//         {
//             inputString = EditorGUILayout.TextField("Ime skripte", inputString);
//             inputString2 = EditorGUILayout.TextField("Ime liste pozicija", inputString2);
//             EditPositionListWindowUpdate();
//         }
//
//         [MenuItem("GameObject/Talenzzo/In Test - Edit Position List", false, 0)]
//         static void EditPositionListOption()
//         {
//             okClicked = OnEditPositionListFinished;
//             selectedMarkers = null;
//             GameObject[] selectedGameObjects = GetSelectedGameObjects();
//             if (selectedGameObjects == null || selectedGameObjects.Length != 1)
//             {
//                 return;
//             }
//
//             inputGameObject = selectedGameObjects[0];
//             Show(EditorWindowState.EditPositionListWindow);
//         }
//
//         static void OnEditPositionListFinished()
//         {
//             Debug.Log(inputString);
//             inputGameObject = null;
//         }
//
//         private static ReorderableList _vector2List;
//         private static List<GameObject> selectedMarkers;
//         static void EditPositionListWindowUpdate()
//         { 
//             if (inputComponent == null)
//             {
//                 // if (inputGameObject == null)
//                 //     return;
//                 inputComponent = inputGameObject.GetComponent(inputString);
//                 if (inputComponent == null)
//                 {
//                     return;
//                 }
//             }
//             SerializedObject serializedObject = new SerializedObject(inputComponent);
//             SerializedProperty myProperty = serializedObject.FindProperty(inputString2);
//
//             if (myProperty == null)
//             {
//                 return;
//             }
//     
//             inputBool1 = EditorGUILayout.Foldout(inputBool1, "Vector2 List");
//             if (inputBool1)
//             {
//                 EditorGUIUtility.editingTextField = false;
//                 // Create a ReorderableList for the vector2List SerializedProperty
//     
//                 _vector2List = new ReorderableList(serializedObject, myProperty, true, true, true, true);
//     
//                 // Wrap the ReorderableList inside a scroll view
//                 inputVector2 = EditorGUILayout.BeginScrollView(inputVector2, GUILayout.Height(300));
//         
//                 // Draw the ReorderableList
//                 _vector2List.drawHeaderCallback = (Rect rect) => {
//                     EditorGUI.LabelField(rect, "Vector2 List");
//                 };
//
//                 _vector2List.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
//                     SerializedProperty element = _vector2List.serializedProperty.GetArrayElementAtIndex(index);
//                     rect.y += 2;
//                     rect.height = EditorGUIUtility.singleLineHeight;
//                     EditorGUI.LabelField(rect, "Point " + index);
//                     rect.x += 70;
//                     rect.width -= 70;
//                     EditorGUI.PropertyField(rect, element, GUIContent.none);
//                 };
//
//                 _vector2List.onAddCallback = (ReorderableList list) => {
//                     list.serializedProperty.arraySize++;
//                     list.index = list.serializedProperty.arraySize - 1;
//                     SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(list.index);
//                     element.vector2Value = Vector2.zero;
//                 };
//
//                 _vector2List.onRemoveCallback = (ReorderableList list) => {
//                     if (EditorUtility.DisplayDialog("Delete Vector2", "Are you sure you want to delete this vector2?", "Yes", "No"))
//                     {
//                         ReorderableList.defaultBehaviours.DoRemoveButton(list);
//                     }
//                 };
//
//                 _vector2List.DoLayoutList();
//                 EditorGUILayout.EndScrollView();
//                 EditorGUIUtility.editingTextField = true;
//                 serializedObject.Update();
//                 // Apply any changes made to the SerializedObject
//                 serializedObject.ApplyModifiedProperties();
//
//                 EditPositionListWindowCreateObjects(myProperty, inputGameObject.transform);
//             }
//     
//         }
//
//         static void EditPositionListWindowCreateObjects(SerializedProperty property, Transform parent)
//         {
//             if (selectedMarkers == null || selectedMarkers.Count == 0)
//             {
//                 selectedMarkers = new List<GameObject>();
//                 for (int i = 0; i < property.arraySize; i++)
//                 {
//                     SerializedProperty element = property.GetArrayElementAtIndex(i);
//                     Vector2 v2element = element.vector2Value;
//                     GameObject marker = new GameObject();
//                     //Instantiate(marker, parent);
//                     marker.name = "Marker" + i;
//                     marker.transform.parent = parent;
//                     marker.transform.localScale = new Vector3(0.05f, 0.05f, 1);
//                     marker.transform.localPosition = new Vector3(v2element.x, v2element.y, 0);
//                     SpriteRenderer renderer = marker.AddComponent<SpriteRenderer>();
//                     renderer.sortingOrder = 30;
//                     //renderer.sprite = 
//                     EditorUtility.SetDirty(marker);
//                     selectedMarkers.Add(marker);
//                 }
//             }
//         }
//
//
//         #endregion
//     
//         #region ReferenceMatrixWindow
//
//
//         private static int[,] integerMatrix;
//         static void ReferenceMatrixWindow()
//         {
//             // - component validate
//             inputString = EditorGUILayout.TextField("Ime skripte", inputString);
//             inputString2 = EditorGUILayout.TextField("Ime liste pozicija", inputString2);
//         
//             if (inputComponent == null)
//             {
//                 if (inputGameObject == null)
//                     return;
//                 inputComponent = inputGameObject.GetComponent(inputString);
//                 if (inputComponent == null)
//                 {
//                     return;
//                 }
//             }
//             SerializedObject serializedObject = new SerializedObject(inputComponent);
//             SerializedProperty matrixIntArrayProperty = serializedObject.FindProperty(inputString2);
//
//             if (matrixIntArrayProperty == null)
//             {
//                 return;
//             }
//         
//             //Matrix
//
//             EditorGUILayout.BeginHorizontal();
//             inputInt = EditorGUILayout.IntField("Number of rows", inputInt);
//             inputInt2 = EditorGUILayout.IntField("Number of columns", inputInt2);
//             EditorGUILayout.EndHorizontal();
//
//             // Create matrix if valid
//             if (inputInt <= 0 || inputInt2 <= 0)
//             {
//                 return;
//             }
//
//             if (inputInt * inputInt2 != matrixIntArrayProperty.arraySize)
//             {
//                 return;
//             }
//
//             //boolX = EditorGUILayout.Toggle("Open", boolX, GUILayout.ExpandWidth(false), GUILayout.Height(60f));
//             if (integerMatrix == null || integerMatrix.GetLength(0) != inputInt || integerMatrix.GetLength(1) != inputInt2)
//             {
//                 integerMatrix = new int[inputInt, inputInt2];
//                 if (matrixIntArrayProperty.arraySize != 0 && matrixIntArrayProperty.arraySize >= inputInt * inputInt2)
//                 {
//                     for (int i = 0; i < integerMatrix.GetLength(0); i++)
//                     {
//                         for (int j = 0; j < integerMatrix.GetLength(1); j++)
//                         {
//                             integerMatrix[i, j] = matrixIntArrayProperty
//                                 .GetArrayElementAtIndex(i * integerMatrix.GetLength(1) + j).intValue;
//                         }
//                     }
//                 }
//             }
//
//             // Resize the serialized property if the matrix size has changed. NE RADI ZA SAD
//             // TODO: handle resizing matrix
//             // if (matrixIntArrayProperty.arraySize != integerMatrix.Length)
//             // {
//             //     matrixIntArrayProperty.arraySize = integerMatrix.Length;
//             // }
//         
//             serializedObject.Update();
//         
//             GUILayout.Space(20);
//         
//             inputVector2 = EditorGUILayout.BeginScrollView(inputVector2, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
//         
//             GUILayout.BeginHorizontal();
//             GUILayout.Space(60);
//             for (int j = 0; j < integerMatrix.GetLength(1); j++)
//             {
//                 GUILayout.Label(j.ToString(), GUILayout.Width(50));
//             }
//             GUILayout.EndHorizontal();
//         
//             GUILayout.BeginVertical();
//             for (int i = 0; i < integerMatrix.GetLength(0); i++)
//             {
//                 GUILayout.BeginHorizontal();
//                 GUILayout.Label(i.ToString(), GUILayout.Width(50));
//                 for (int j = 0; j < integerMatrix.GetLength(1); j++)
//                 {
//                     integerMatrix[i, j] = EditorGUILayout.IntField(integerMatrix[i, j], GUILayout.Width(50));
//                     // Update the serialized property value
//                     int index = i * integerMatrix.GetLength(1) + j;
//                     matrixIntArrayProperty.GetArrayElementAtIndex(index).intValue = integerMatrix[i, j];
//                 }
//                 GUILayout.EndHorizontal();
//             }
//             GUILayout.EndVertical();
//             EditorGUILayout.EndScrollView();
//         
//             serializedObject.ApplyModifiedProperties();
//         
//             ReferenceMatrixWindowUpdate();
//         }
//
//         [MenuItem("GameObject/Talenzzo/Edit Matrix - List<int>", false, 0)]
//         static void ReferenceMatrixOption()
//         {
//             GameObject[] selectedGameObjects = GetSelectedGameObjects();
//             if (selectedGameObjects == null || selectedGameObjects.Length != 1)
//             {
//                 return;
//             }
//
//             inputGameObject = selectedGameObjects[0];
//             okClicked = OnReferenceMatrixFinished;
//             Show(EditorWindowState.ReferenceMatrixWindow);
//         }
//
//         static void OnReferenceMatrixFinished()
//         {
//             integerMatrix = null;
//             Debug.Log(inputString);
//         }
//
//         static void ReferenceMatrixWindowUpdate()
//         {
//         
//         }
//     
//         #endregion
//         
//         #region CreateGameObjectsWindow
//         
//         static void CreateGameObjectsWindow()
//         {
//             GUILayout.Label("Number of GameObjects to create");
//             inputInt = EditorGUILayout.IntField(inputInt, GUILayout.Width(50));
//             GUILayout.Label("Name of objects");
//             inputString = EditorGUILayout.TextField(inputString, GUILayout.Width(250));
//             
//             
//             CreateGameObjectsWindowUpdate();
//         }
//
//         [MenuItem("GameObject/Talenzzo/Create GameObjects", false, 0)]
//         static void CreateGameObjectsOption()
//         {
//             okClicked = OnCreateGameObjectsFinished;
//             Show(EditorWindowState.CreateGameObjectsWindow);
//         }
//
//         static void OnCreateGameObjectsFinished()
//         {
//             GameObject[] selectedGameObjects = GetSelectedGameObjects();
//             if (selectedGameObjects.Length != 1)
//             {
//                 Debug.LogError("Select parent object");
//             }
//
//             Transform parent = selectedGameObjects[0].transform;
//
//             for (int i = 0; i < inputInt; i++)
//             {
//                 GameObject gameObject = new GameObject();
//                 Undo.RegisterCreatedObjectUndo(gameObject, "Create New GameObject" + i);
//
//                 gameObject.transform.parent = parent;
//                 gameObject.transform.localPosition = Vector3.zero;
//                 gameObject.transform.localRotation = Quaternion.identity;
//                 gameObject.transform.localScale = Vector3.one;
//                 gameObject.name = inputString;
//                 gameObject.layer = parent.gameObject.layer;
//                 EditorUtility.SetDirty(gameObject);
//             }
//             
//             RenameObjects();
//         }
//
//         static void CreateGameObjectsWindowUpdate()
//         {
//         
//         }
//         
//         #endregion
//
//         #endregion
//
//         #region Editor Script Utillities
//
//         private static GameObject[] GetSelectedGameObjects()
//         {
//             if (Selection.activeGameObject == null)
//                 return null;
//
//             return Selection.gameObjects;
//         }
//
//         private static GameObject GetRoomsHolderOnScene()
//         {
//             return GameObject.Find("RoomsHolder");
//         }
//
//         private static void ApplyOverrides()
//         {
//             EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
//             PrefabUtility.ApplyPrefabInstance(GetRoomsHolderOnScene(), InteractionMode.AutomatedAction);
//             EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
//         }
//
//         #endregion
//     }
// }
// // TODO - Test Undo.RecordObject(gameObject, gameObject.name);  snima sve posle te naredbe. Ne radi sa Referenciranjem kao gore