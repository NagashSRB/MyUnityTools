// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// using UnityEditor.SceneManagement;
// using System.IO;
// using System.Linq;
// using UnityEditor.Experimental.SceneManagement;
// using UnityEngine.SceneManagement;
//
// namespace x{
//     public class GameObjectMenu : EditorWindow
//     {
//         private bool animationObjectHasIdle;
//         private bool animationObjectHasOpen;
//         private bool animationObjectHasClose;
//         private bool animationObjectHasItemUsed;
//
//         private int numberOfStatesForStatesObject;
//
//         // Funkcija za kreiranje sobe
//         [MenuItem("GameObject/Room Escape/Kreiraj sobu", false, 0)]
//         static void CreateRoomAction()
//         {
//             createAction = new System.Action(CreateRoomObject);
//             SetObjectNameWindow();
//         }
//
//         static void CreateRoomObject()
//         {
//             GameObject roomPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/Room.prefab", typeof(GameObject)) as GameObject;
//             GameObject roomObject = Instantiate(roomPrefab, Vector3.zero, roomPrefab.transform.rotation) as GameObject;
//
//             if (Selection.activeGameObject != null)
//             {
//                 roomObject.transform.SetParent(Selection.activeGameObject.transform);
//                 roomObject.transform.localPosition = Vector3.zero;
//             }
//
//             if (creatingObjectName != "" && creatingObjectName != null)
//                 roomObject.name = creatingObjectName;
//         }
//
//         // Funkcija za kreiranje itema za promenu sobe
//         [MenuItem("GameObject/Room Escape/Kreiraj objekat za promenu sobe", false, 0)]
//         static void CreateChangeRoomObjectAction()
//         {
//             createAction = new System.Action(CreateChangeRoomObject);
//             SetObjectNameWindow();
//         }
//
//         static void CreateChangeRoomObject()
//         {
//             GameObject changeRoomPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/ChangeRoomPrefab.prefab", typeof(GameObject)) as GameObject;
//             GameObject changeRoomPrefabObject = Instantiate(changeRoomPrefab, Vector3.zero, changeRoomPrefab.transform.rotation) as GameObject;
//
//             if (Selection.activeGameObject != null)
//             {
//                 changeRoomPrefabObject.transform.SetParent(Selection.activeGameObject.transform);
//                 changeRoomPrefabObject.transform.localPosition = Vector3.zero;
//             }
//
//             if (creatingObjectName != "" && creatingObjectName != null)
//                 changeRoomPrefabObject.name = creatingObjectName;
//         }
//
//         // Funkcija za kreiranje objekta koji moze da se pokupi
//         [MenuItem("GameObject/Room Escape/Kreiraj item objekat", false, 0)]
//         static void CreateItemObjectAction()
//         {
//             createAction = new System.Action(CreateItemObject);
//             SetObjectNameWindow();
//         }
//
//         static void CreateItemObject()
//         {
//             GameObject itemObjectPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/ItemPrefab.prefab", typeof(GameObject)) as GameObject;
//             GameObject itemObject = Instantiate(itemObjectPrefab, Vector3.zero, itemObjectPrefab.transform.rotation) as GameObject;
//
//             if (Selection.activeGameObject != null)
//             {
//                 itemObject.transform.SetParent(Selection.activeGameObject.transform);
//                 itemObject.transform.localPosition = Vector3.zero;
//             }
//
//             if (creatingObjectName != "" && creatingObjectName != null)
//                 itemObject.name = creatingObjectName;
//
//             // Kreiramo inventory objekat za ovaj objekat
//             GameObject inventoryItem = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/InventoryItem.prefab", typeof(GameObject)) as GameObject;
//             GameObject inventoryItemObject = Instantiate(inventoryItem, GameObject.Find("Canvas").transform) as GameObject;
//
//             inventoryItemObject.name = creatingObjectName + "InventoryItem";
//
//             inventoryItemObject.GetComponent<InventoryItem>().itemIndex = GameObjectMenu.Instance.CalculateLastCreatedItemIndex() + 1;
//
//             // Proveravamo da li folder za inventory iteme za ovaj level postoji
//             DirectoryInfo dirInfo = new DirectoryInfo("Assets/Levels/" + EditorSceneManager.GetActiveScene().name);
//
//             if (!dirInfo.Exists)
//             {
//                 AssetDatabase.CreateFolder("Assets/Levels", EditorSceneManager.GetActiveScene().name);
//             }
//
//             string inventoryItemLocalPath = "Assets/Levels/" + EditorSceneManager.GetActiveScene().name + "/" + inventoryItemObject.name + ".prefab";
//             inventoryItemLocalPath = AssetDatabase.GenerateUniqueAssetPath(inventoryItemLocalPath);
//
//             PrefabUtility.SaveAsPrefabAsset(inventoryItemObject, inventoryItemLocalPath);
//
//             itemObject.GetComponent<ActiveItem>().itemForInventory = AssetDatabase.LoadAssetAtPath(inventoryItemLocalPath, typeof(GameObject)) as GameObject;
//
//             // Setujemo index za hint na defaultni (-1)
//             itemObject.GetComponent<ActiveItem>().hintPriority = -1;
//
//             DestroyImmediate(inventoryItemObject);
//         }
//
//         // Funkcija za kreiranje objekta za inventory
//         [MenuItem("GameObject/Room Escape/Kreiraj inventory item objekat", false, 0)]
//         static void CreateInventoryItemObjectAction()
//         {
//             createAction = new System.Action(CreateInventoryItemObject);
//             SetObjectNameWindow();
//         }
//
//         static void CreateInventoryItemObject()
//         {
//             // Kreiramo inventory objekat za ovaj objekat
//             GameObject inventoryItem = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/InventoryItem.prefab", typeof(GameObject)) as GameObject;
//             GameObject inventoryItemObject = Instantiate(inventoryItem, GameObject.Find("Canvas").transform) as GameObject;
//
//             if (creatingObjectName != "" && creatingObjectName != null)
//             {
//                 inventoryItemObject.name = creatingObjectName + "InventoryItem";
//             }
//
//             inventoryItemObject.GetComponent<InventoryItem>().itemIndex = GameObjectMenu.Instance.CalculateLastCreatedItemIndex() + 1;
//
//             // Proveravamo da li folder za inventory iteme za ovaj level postoji
//             DirectoryInfo dirInfo = new DirectoryInfo("Assets/Levels/" + EditorSceneManager.GetActiveScene().name);
//
//             if (!dirInfo.Exists)
//             {
//                 AssetDatabase.CreateFolder("Assets/Levels", EditorSceneManager.GetActiveScene().name);
//             }
//
//             string inventoryItemLocalPath = "Assets/Levels/" + EditorSceneManager.GetActiveScene().name + "/" + inventoryItemObject.name + ".prefab";
//             inventoryItemLocalPath = AssetDatabase.GenerateUniqueAssetPath(inventoryItemLocalPath);
//
//             PrefabUtility.SaveAsPrefabAsset(inventoryItemObject, inventoryItemLocalPath);
//
//             DestroyImmediate(inventoryItemObject);
//         }
//
//         // Funkcija za kreiranje objekta na koji se koristi neki od objekata
//         [MenuItem("GameObject/Room Escape/Kreiraj target objekat", false, 0)]
//         static void CreateTargetObjectAction()
//         {
//             createAction = new System.Action(CreateTargetObject);
//             SetObjectNameWindow();
//         }
//
//         static void CreateTargetObject()
//         {
//             GameObject targetObjectPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/TargetObjectPrefab.prefab", typeof(GameObject)) as GameObject;
//             GameObject targetObject = Instantiate(targetObjectPrefab, Vector3.zero, targetObjectPrefab.transform.rotation) as GameObject;
//
//             if (Selection.activeGameObject != null)
//             {
//                 targetObject.transform.SetParent(Selection.activeGameObject.transform);
//                 targetObject.transform.localPosition = Vector3.zero;
//             }
//
//             if (creatingObjectName != "" && creatingObjectName != null)
//                 targetObject.name = creatingObjectName;
//
//             // Setujemo prioritet za hint
//             targetObject.GetComponent<TargetItem>().hintPriority = -1;
//
//             // Proveravamo da li smo selektovali neku animaciju
//             if (Instance.animationObjectHasIdle || Instance.animationObjectHasOpen || Instance.animationObjectHasClose || Instance.animationObjectHasItemUsed)
//             {
//                 // Prvo proveravamo da li postoji direktorijum za level
//                 DirectoryInfo di = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1]);
//                 if (!di.Exists)
//                 {
//                     AssetDatabase.CreateFolder("Assets/Levels", EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1]);
//                 }
//
//                 // Prvo proveravamo da li postoji direktorijum za objekat
//                 DirectoryInfo di2 = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations");
//                 if (!di2.Exists)
//                 {
//                     AssetDatabase.CreateFolder("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1], "Animations");
//                 }
//
//                 DirectoryInfo di3 = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + creatingObjectName);
//                 if (!di3.Exists)
//                 {
//                     AssetDatabase.CreateFolder("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations", creatingObjectName);
//                 }
//
//                 // Kreiramo novi animator sa stanjima koja su odabrana
//                 var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + creatingObjectName + "/" + creatingObjectName + ".controller");
//
//                 // Kreiramo stanja za novi animator
//                 var rootStateMachine = controller.layers[0].stateMachine;
//
//                 var idleState = rootStateMachine.AddState("Idle");
//                 var itemUsedState = rootStateMachine.AddState("ItemUsed");
//                 var openState = rootStateMachine.AddState("Open");
//                 var closeState = rootStateMachine.AddState("Close");
//                 // Sedtujemo idle stanje na default stanje
//                 rootStateMachine.defaultState = idleState;
//
//                 // Kreiramo i same animacije
//                 if (Instance.animationObjectHasIdle)
//                 {
//                     AnimationClip idleAnimation = new AnimationClip();
//                     AssetDatabase.CreateAsset(idleAnimation, "Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + creatingObjectName + "/" + creatingObjectName + "_Idle.anim");
//                     AnimationClipSettings idleAnimationSettings = AnimationUtility.GetAnimationClipSettings(idleAnimation);
//                     idleAnimationSettings.loopTime = false;
//                     AnimationUtility.SetAnimationClipSettings(idleAnimation, idleAnimationSettings);
//                     idleState.motion = idleAnimation;
//                 }
//
//                 if (Instance.animationObjectHasItemUsed)
//                 {
//                     AnimationClip itemUsedAnimation = new AnimationClip();
//                     AssetDatabase.CreateAsset(itemUsedAnimation, "Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + creatingObjectName + "/" + creatingObjectName + "_ItemUsed.anim");
//                     AnimationClipSettings itemUsedAnimationSettings = AnimationUtility.GetAnimationClipSettings(itemUsedAnimation);
//                     itemUsedAnimationSettings.loopTime = false;
//                     AnimationUtility.SetAnimationClipSettings(itemUsedAnimation, itemUsedAnimationSettings);
//                     itemUsedState.motion = itemUsedAnimation;
//                 }
//
//                 if (Instance.animationObjectHasOpen)
//                 {
//                     AnimationClip openAnimation = new AnimationClip();
//                     AssetDatabase.CreateAsset(openAnimation, "Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + creatingObjectName + "/" + creatingObjectName + "_Open.anim");
//                     AnimationClipSettings openAnimationSettings = AnimationUtility.GetAnimationClipSettings(openAnimation);
//                     openAnimationSettings.loopTime = false;
//                     AnimationUtility.SetAnimationClipSettings(openAnimation, openAnimationSettings);
//                     openState.motion = openAnimation;
//                 }
//
//                 if (Instance.animationObjectHasClose)
//                 {
//                     AnimationClip closeAnimation = new AnimationClip();
//                     AssetDatabase.CreateAsset(closeAnimation, "Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + creatingObjectName + "/" + creatingObjectName + "_Close.anim");
//                     AnimationClipSettings closeAnimationSettings = AnimationUtility.GetAnimationClipSettings(closeAnimation);
//                     closeAnimationSettings.loopTime = false;
//                     AnimationUtility.SetAnimationClipSettings(closeAnimation, closeAnimationSettings);
//                     closeState.motion = closeAnimation;
//                 }
//
//                 // Kreiramo animator komponentu i dodajemo novi kreirani animator
//                 if (targetObject.transform.Find("AnimationHolder").GetComponent<Animator>() == null)
//                     targetObject.transform.Find("AnimationHolder").gameObject.AddComponent<Animator>();
//                 targetObject.transform.Find("AnimationHolder").gameObject.GetComponent<Animator>().runtimeAnimatorController = controller;
//                 targetObject.transform.GetChild(0).gameObject.AddComponent<KeepAnimatorState>();
//
//                 Instance.animationObjectHasIdle = false;
//                 Instance.animationObjectHasOpen = false;
//                 Instance.animationObjectHasClose = false;
//                 Instance.animationObjectHasItemUsed = false;
//                 EditorUtility.SetDirty(targetObject);
//             }
//         }
//
//         // Funkcija za kreiranje objekta na koji se koristi neki od objekata
//         [MenuItem("GameObject/Room Escape/Kreiraj target objekat na koji se koristi vise itema", false, 0)]
//         static void CreateMultipleItemsTargetObjectAction()
//         {
//             createAction = new System.Action(CreateMultipleItemsTargetObject);
//             SetObjectNameWindow();
//         }
//
//         static void CreateMultipleItemsTargetObject()
//         {
//             GameObject targetObjectPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/MultipleItemsTargetItem.prefab", typeof(GameObject)) as GameObject;
//             GameObject targetObject = Instantiate(targetObjectPrefab, Vector3.zero, targetObjectPrefab.transform.rotation) as GameObject;
//
//             if (Selection.activeGameObject != null)
//             {
//                 targetObject.transform.SetParent(Selection.activeGameObject.transform);
//                 targetObject.transform.localPosition = Vector3.zero;
//             }
//
//             if (creatingObjectName != "" && creatingObjectName != null)
//                 targetObject.name = creatingObjectName;
//
//             // Setujemo prioritet za hint
//             targetObject.GetComponent<MultipleItemsTargetItem>().hintPriority = -1;
//
//             // Proveravamo da li smo selektovali neku animaciju
//             if (Instance.animationObjectHasIdle || Instance.animationObjectHasOpen || Instance.animationObjectHasClose || Instance.animationObjectHasItemUsed)
//             {
//                 // Prvo proveravamo da li postoji direktorijum za level
//                 DirectoryInfo di = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1]);
//                 if (!di.Exists)
//                 {
//                     AssetDatabase.CreateFolder("Assets/Levels", EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1]);
//                 }
//
//                 // Prvo proveravamo da li postoji direktorijum za objekat
//                 DirectoryInfo di2 = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations");
//                 if (!di2.Exists)
//                 {
//                     AssetDatabase.CreateFolder("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1], "Animations");
//                 }
//
//                 DirectoryInfo di3 = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + creatingObjectName);
//                 if (!di3.Exists)
//                 {
//                     AssetDatabase.CreateFolder("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations", creatingObjectName);
//                 }
//
//                 // Kreiramo novi animator sa stanjima koja su odabrana
//                 var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + creatingObjectName + "/" + creatingObjectName + ".controller");
//
//                 // Kreiramo stanja za novi animator
//                 var rootStateMachine = controller.layers[0].stateMachine;
//
//                 var idleState = rootStateMachine.AddState("Idle");
//                 var itemUsedState = rootStateMachine.AddState("ItemUsed");
//                 var openState = rootStateMachine.AddState("Open");
//                 var closeState = rootStateMachine.AddState("Close");
//                 // Sedtujemo idle stanje na default stanje
//                 rootStateMachine.defaultState = idleState;
//
//                 // Kreiramo i same animacije
//                 if (Instance.animationObjectHasIdle)
//                 {
//                     AnimationClip idleAnimation = new AnimationClip();
//                     AssetDatabase.CreateAsset(idleAnimation, "Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + creatingObjectName + "/" + creatingObjectName + "_Idle.anim");
//                     AnimationClipSettings idleAnimationSettings = AnimationUtility.GetAnimationClipSettings(idleAnimation);
//                     idleAnimationSettings.loopTime = false;
//                     AnimationUtility.SetAnimationClipSettings(idleAnimation, idleAnimationSettings);
//                     idleState.motion = idleAnimation;
//                 }
//
//                 if (Instance.animationObjectHasItemUsed)
//                 {
//                     AnimationClip itemUsedAnimation = new AnimationClip();
//                     AssetDatabase.CreateAsset(itemUsedAnimation, "Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + creatingObjectName + "/" + creatingObjectName + "_ItemUsed.anim");
//                     AnimationClipSettings itemUsedAnimationSettings = AnimationUtility.GetAnimationClipSettings(itemUsedAnimation);
//                     itemUsedAnimationSettings.loopTime = false;
//                     AnimationUtility.SetAnimationClipSettings(itemUsedAnimation, itemUsedAnimationSettings);
//                     itemUsedState.motion = itemUsedAnimation;
//                 }
//
//                 if (Instance.animationObjectHasOpen)
//                 {
//                     AnimationClip openAnimation = new AnimationClip();
//                     AssetDatabase.CreateAsset(openAnimation, "Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + creatingObjectName + "/" + creatingObjectName + "_Open.anim");
//                     AnimationClipSettings openAnimationSettings = AnimationUtility.GetAnimationClipSettings(openAnimation);
//                     openAnimationSettings.loopTime = false;
//                     AnimationUtility.SetAnimationClipSettings(openAnimation, openAnimationSettings);
//                     openState.motion = openAnimation;
//                 }
//
//                 if (Instance.animationObjectHasClose)
//                 {
//                     AnimationClip closeAnimation = new AnimationClip();
//                     AssetDatabase.CreateAsset(closeAnimation, "Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + creatingObjectName + "/" + creatingObjectName + "_Close.anim");
//                     AnimationClipSettings closeAnimationSettings = AnimationUtility.GetAnimationClipSettings(closeAnimation);
//                     closeAnimationSettings.loopTime = false;
//                     AnimationUtility.SetAnimationClipSettings(closeAnimation, closeAnimationSettings);
//                     closeState.motion = closeAnimation;
//                 }
//
//                 // Kreiramo animator komponentu i dodajemo novi kreirani animator
//                 if (targetObject.transform.Find("AnimationHolder").GetComponent<Animator>() == null)
//                     targetObject.transform.Find("AnimationHolder").gameObject.AddComponent<Animator>();
//                 targetObject.transform.Find("AnimationHolder").gameObject.GetComponent<Animator>().runtimeAnimatorController = controller;
//
//                 Instance.animationObjectHasIdle = false;
//                 Instance.animationObjectHasOpen = false;
//                 Instance.animationObjectHasClose = false;
//                 Instance.animationObjectHasItemUsed = false;
//             }
//         }
//
//         // Funkcija za kreiranje objekta koji ima vise targeta
//         [MenuItem("GameObject/Room Escape/Kreiraj target objekat sa vise targeta", false, 0)]
//         static void CreateMultipleTagetsObjectAction()
//         {
//             createAction = new System.Action(CreateMultipleTargetsObject);
//             SetObjectNameWindow();
//         }
//
//         static void CreateMultipleTargetsObject()
//         {
//             GameObject multipleTargetsItemPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/MultipleTargetsItem.prefab", typeof(GameObject)) as GameObject;
//             GameObject multipleTargetsItemPrefabObject = Instantiate(multipleTargetsItemPrefab, Vector3.zero, multipleTargetsItemPrefab.transform.rotation) as GameObject;
//
//             if (Selection.activeGameObject != null)
//             {
//                 multipleTargetsItemPrefabObject.transform.SetParent(Selection.activeGameObject.transform);
//                 multipleTargetsItemPrefabObject.transform.localPosition = Vector3.zero;
//             }
//
//             if (creatingObjectName != "" && creatingObjectName != null)
//                 multipleTargetsItemPrefabObject.name = creatingObjectName;
//
//             // Setujemo prioritet za hint
//             multipleTargetsItemPrefabObject.GetComponent<MultipleTargetsItem>().hintPriority = -1;
//         }
//
//         // Funkcija za kreiranje objekta na koji moze da se iskoristi vise od jednog itema
//         [MenuItem("GameObject/Room Escape/Kreiraj target objekat na koji moze da se iskoristi vise itema", false, 0)]
//         static void CreateMultipleIndicesTagetObjectAction()
//         {
//             createAction = new System.Action(CreateMultipleIndicesTargetObject);
//             SetObjectNameWindow();
//         }
//
//         static void CreateMultipleIndicesTargetObject()
//         {
//             GameObject multipleIndicesTargetItemPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/MultipleIndicesTarget.prefab", typeof(GameObject)) as GameObject;
//             GameObject multipleIndicesTargetItemPrefabObject = Instantiate(multipleIndicesTargetItemPrefab, Vector3.zero, multipleIndicesTargetItemPrefab.transform.rotation) as GameObject;
//
//             if (Selection.activeGameObject != null)
//             {
//                 multipleIndicesTargetItemPrefabObject.transform.SetParent(Selection.activeGameObject.transform);
//                 multipleIndicesTargetItemPrefabObject.transform.localPosition = Vector3.zero;
//             }
//
//             if (creatingObjectName != "" && creatingObjectName != null)
//                 multipleIndicesTargetItemPrefabObject.name = creatingObjectName;
//
//             // FIXME za sada sam zakomentarisao - Setujemo prioritet za hint
//             // multipleIndicesTargetItemPrefabObject.GetComponent<MultipleIndicesTarget>().hintPriority = -1;
//         }
//
//         // Funkcija za kreiranje zoom zone objekta i zoom zone za njega
//         [MenuItem("GameObject/Room Escape/Kreiraj zoom zone objekat", false, 0)]
//         static void CreateZoomObjectAction()
//         {
//             createAction = new System.Action(CreateZoomObject);
//             SetObjectNameWindow();
//         }
//
//         static void CreateZoomObject()
//         {
//             GameObject zoomItemObjectPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/ZoomItemPrefab.prefab", typeof(GameObject)) as GameObject;
//             GameObject zoomItemObject = Instantiate(zoomItemObjectPrefab, Vector3.zero, zoomItemObjectPrefab.transform.rotation) as GameObject;
//
//             if (Selection.activeGameObject != null)
//             {
//                 zoomItemObject.transform.SetParent(Selection.activeGameObject.transform);
//                 zoomItemObject.transform.localPosition = Vector3.zero;
//             }
//
//             if (creatingObjectName != "" && creatingObjectName != null)
//                 zoomItemObject.name = creatingObjectName;
//
//             GameObject zoomZonePrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/ZoomZonePrefab.prefab", typeof(GameObject)) as GameObject;
//             GameObject zoomZoneObject = Instantiate(zoomZonePrefab, Vector3.zero, zoomZonePrefab.transform.rotation) as GameObject;
//
//             if (Selection.activeGameObject != null)
//             {
//                 // Trazimo parent objekat za zoom zonu
//                 bool parentFound = false;
//                 Transform appropriateParent = Selection.activeGameObject.transform;
//                 while (!parentFound)
//                 {
//                     if (appropriateParent.parent.name == "RoomsHolder")
//                     {
//                         parentFound = true;
//                     }
//                     else
//                     {
//                         appropriateParent = appropriateParent.parent;
//                     }
//                 }
//
//                 zoomZoneObject.transform.SetParent(appropriateParent);
//                 zoomZoneObject.transform.localPosition = Vector3.zero;
//             }
//
//             if (creatingObjectName != "" && creatingObjectName != null)
//                 zoomZoneObject.name = creatingObjectName + "ZoomZone";
//
//             zoomItemObject.GetComponent<ZoomZoneItem>().zoomZone = zoomZoneObject;
//             zoomZoneObject.GetComponent<ZoomZone>().zoomZoneItem = zoomItemObject.GetComponent<ZoomZoneItem>();
//         }
//
//         //[MenuItem("GameObject/Room Escape/Kreiraj zoom zonu", false, 0)]
//         //static void CreateZoomZoneAction()
//         //{
//         //    createAction = new System.Action(CreateZoomZone);
//         //    SetObjectNameWindow();
//         //}
//
//         //static void CreateZoomZone()
//         //{
//         //    GameObject zoomZonePrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/ZoomZonePrefab.prefab", typeof(GameObject)) as GameObject;
//         //    GameObject zoomZoneObject = Instantiate(zoomZonePrefab, Vector3.zero, zoomZonePrefab.transform.rotation) as GameObject;
//
//         //    if (Selection.activeGameObject != null)
//         //    {
//         //        zoomZoneObject.transform.SetParent(Selection.activeGameObject.transform);
//         //        zoomZoneObject.transform.localPosition = Vector3.zero;
//         //    }
//
//         //    if (creatingObjectName != "" && creatingObjectName != null)
//         //        zoomZoneObject.name = creatingObjectName + "ZoomZone";
//         //}
//
//         // Funkcija za kreiranje mini game objekta i mini igre
//         [MenuItem("GameObject/Room Escape/Kreiraj mini game objekat", false, 0)]
//         static void CreateMiniGameObjectAction()
//         {
//             createAction = new System.Action(CreateMiniGameObject);
//             SetObjectNameWindow();
//         }
//
//         static void CreateMiniGameObject()
//         {
//             GameObject miniGameItemPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/MiniGameItemPrefab.prefab", typeof(GameObject)) as GameObject;
//             GameObject miniGameItemObject = Instantiate(miniGameItemPrefab, Vector3.zero, miniGameItemPrefab.transform.rotation) as GameObject;
//
//             if (Selection.activeGameObject != null)
//             {
//                 miniGameItemObject.transform.SetParent(Selection.activeGameObject.transform);
//                 miniGameItemObject.transform.localPosition = Vector3.zero;
//             }
//
//             if (creatingObjectName != "" && creatingObjectName != null)
//                 miniGameItemObject.name = creatingObjectName;
//
//             GameObject miniGamePrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/MiniGamePrefab.prefab", typeof(GameObject)) as GameObject;
//             GameObject miniGameObject = Instantiate(miniGamePrefab, Vector3.zero, miniGamePrefab.transform.rotation) as GameObject;
//
//             if (Selection.activeGameObject != null)
//             {
//                 // Trazimo parent objekat za zoom zonu
//                 bool parentFound = false;
//                 Transform appropriateParent = Selection.activeGameObject.transform;
//                 while (!parentFound)
//                 {
//                     if (appropriateParent.parent.name == "RoomsHolder")
//                     {
//                         parentFound = true;
//                     }
//                     else
//                     {
//                         appropriateParent = appropriateParent.parent;
//                     }
//                 }
//
//                 miniGameObject.transform.SetParent(appropriateParent);
//                 miniGameObject.transform.localPosition = Vector3.zero;
//             }
//
//             if (creatingObjectName != "" && creatingObjectName != null)
//                 miniGameObject.name = creatingObjectName + "MiniGame";
//
//             miniGameItemObject.GetComponent<MiniGameItem>().miniGame = miniGameObject;
//             miniGameObject.GetComponent<MiniGame>().miniGameItem = miniGameItemObject.GetComponent<MiniGameItem>();
//         }
//
//         // Funkcija za kreiranje dokument itema
//         [MenuItem("GameObject/Room Escape/Kreiraj dokument item", false, 0)]
//         static void CreateDocumentObjectAction()
//         {
//             createAction = new System.Action(CreateDocumentObject);
//             SetObjectNameWindow();
//         }
//
//         static void CreateDocumentObject()
//         {
//             GameObject documentItemPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/DocumentItem.prefab", typeof(GameObject)) as GameObject;
//             GameObject documentItemPrefabObject = Instantiate(documentItemPrefab, Vector3.zero, documentItemPrefab.transform.rotation) as GameObject;
//
//             if (Selection.activeGameObject != null)
//             {
//                 documentItemPrefabObject.transform.SetParent(Selection.activeGameObject.transform);
//                 documentItemPrefabObject.transform.localPosition = Vector3.zero;
//             }
//
//             if (creatingObjectName != "" && creatingObjectName != null)
//                 documentItemPrefabObject.name = creatingObjectName;
//         }
//
//         [MenuItem("GameObject/Room Escape/Kreiraj Interactive objekat - Artist", false, 0)]
//         static void CreateArtistInteractiveObjectAction()
//         {
//             createAction = new System.Action(CreateArtistInteractiveObject);
//
//             SetObjectNameWindow();
//         }
//
//         private bool isChangingPosition;
//
//         static void CreateArtistInteractiveObject()
//         {
//             string prefabSelectorString = "ArtistInteractiveObject.prefab";
//
//             GameObject animObjectPrefab = AssetDatabase.LoadAssetAtPath($"Assets/Prefabs/GameElements/{prefabSelectorString}", typeof(GameObject)) as GameObject;
//             GameObject animObject = Instantiate(animObjectPrefab, Vector3.zero, animObjectPrefab.transform.rotation) as GameObject;
//
//             if(Selection.activeGameObject != null)
//             {
//                 animObject.transform.SetParent(Selection.activeGameObject.transform);
//                 animObject.transform.localPosition = Vector3.zero;
//             }
//
//             string artistAnimObjName = creatingObjectName + "ArtistAnim";
//
//             if (creatingObjectName != "" && creatingObjectName != null)
//                 animObject.name = artistAnimObjName;
//
//             #region Folder Creation
//             // Check if directory for this level exists/creates one, Assets/Levels/Levelxyz
//             DirectoryInfo di = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1]);
//             if (!di.Exists)
//             {
//                 AssetDatabase.CreateFolder("Assets/Levels", EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1]);
//             }
//
//             // Check if Animations folder exists/creates one Assets/Levels/Levelxyz/Animations
//             DirectoryInfo di2 = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations");
//             if (!di2.Exists)
//             {
//                 AssetDatabase.CreateFolder("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1], "Animations");
//             }
//
//             // When checks are done and folders are created, create specific folder for this animation
//             DirectoryInfo di3 = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + artistAnimObjName);
//             if (!di3.Exists)
//             {
//                 AssetDatabase.CreateFolder("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations", artistAnimObjName);
//             }
//             #endregion
//
//             // Creating animator controller
//             var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + artistAnimObjName + "/" + artistAnimObjName + ".controller");
//             var rootStateMachine = controller.layers[0].stateMachine;
//
//             var idleState = rootStateMachine.AddState("Idle");
//             var lastCreatedState = idleState;
//
//             #region Create Idle anim
//             AnimationClip idleAnim = new AnimationClip();
//             AssetDatabase.CreateAsset(idleAnim, $"Assets/Levels/{EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1]}" +
//                                                 $"/Animations/{artistAnimObjName}/{artistAnimObjName}_Idle.anim");
//             AnimationClipSettings idleStateAnimSettings = AnimationUtility.GetAnimationClipSettings(idleAnim);
//             idleStateAnimSettings.loopTime = true;
//             AnimationUtility.SetAnimationClipSettings(idleAnim, idleStateAnimSettings);
//             idleState.motion = idleAnim;
//
//             #endregion
//
//             // Create trigger parameter
//             AnimatorControllerParameter changeStateTrigger = new AnimatorControllerParameter();
//             changeStateTrigger.name = "ChangeState";
//             changeStateTrigger.type = AnimatorControllerParameterType.Trigger;
//
//             #region Create state 0 anim
//             controller.AddParameter(changeStateTrigger);
//
//             var newState = rootStateMachine.AddState("State0");
//
//             AnimationClip newAnimation = new AnimationClip();
//             AssetDatabase.CreateAsset(newAnimation, $"Assets/Levels/{EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1]}" +
//                                                     $"/Animations/{artistAnimObjName}/{artistAnimObjName}_State0.anim");
//
//             AnimationEvent animFinishedEvent = new AnimationEvent();
//             animFinishedEvent.functionName = "OnAnimationFinished";
//             AnimationUtility.SetAnimationEvents(newAnimation, new AnimationEvent[] {animFinishedEvent});
//
//             AnimationClipSettings newStateAnimSettings = AnimationUtility.GetAnimationClipSettings(newAnimation);
//             newStateAnimSettings.loopTime = false;
//             AnimationUtility.SetAnimationClipSettings(newAnimation, newStateAnimSettings);
//             newState.motion = newAnimation;
//
//             UnityEditor.Animations.AnimatorConditionMode conditionMode = UnityEditor.Animations.AnimatorConditionMode.If;
//
//             var transition = lastCreatedState.AddTransition(newState, true);
//             transition.AddCondition(conditionMode, 0, "ChangeState");
//             lastCreatedState = newState;
//             #endregion
//
//             rootStateMachine.defaultState = idleState;
//
//             #region Adding components to objects
//
//             animObject.AddComponent<ArtistsSpecific.ArtistInteractiveItem>();
//
//             Animator thisAnimator;
//
//             // If object is multiple clicking, add animator to the root object so that it can access its collider
//             if(Instance.isChangingPosition)
//             {
//                 animObject.AddComponent<Animator>();
//
//                 thisAnimator = animObject.GetComponent<Animator>();
//
//                 animObject.AddComponent<AudioAnimationEvents>();
//             }
//             else
//             {
//                 GameObject animationHolder = animObject.transform.GetChild(0).gameObject;
//
//                 animationHolder.AddComponent<Animator>();
//
//                 thisAnimator = animationHolder.GetComponent<Animator>();
//
//                 animationHolder.AddComponent<ArtistsSpecific.ArtistAnimEventsInvoker>();
//                 animationHolder.GetComponent<ArtistsSpecific.ArtistAnimEventsInvoker>().artistAnimItem = animObject.GetComponent<ArtistsSpecific.ArtistInteractiveItem>();
//
//                 animationHolder.AddComponent<AudioAnimationEvents>();
//             }
//
//             thisAnimator.runtimeAnimatorController = controller;
//             thisAnimator.gameObject.AddComponent<KeepAnimatorState>();
//
//             #endregion
//             //thisAnimator.gameObject.AddComponent<ArtistsSpecific.ArtistAnimEventsInvoker>();
//
//             //animObject.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = controller;
//             //animObject.transform.GetChild(0).gameObject.AddComponent<KeepAnimatorState>();
//
//             Instance.isChangingPosition = false;
//             EditorUtility.SetDirty(animObject);
//         }
//
//         // Funkcija za kreiranje objekta za animaciju
//         [MenuItem("GameObject/Room Escape/Kreiraj animation object", false, 0)]
//         static void CreateAnimationObjectAction()
//         {
//             createAction = new System.Action(CreateAnimationObject);
//             SetObjectNameWindow();
//         }
//
//         static void CreateAnimationObject()
//         {
//             GameObject animationObjectPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/AnimationObject.prefab", typeof(GameObject)) as GameObject;
//             GameObject animationObject = Instantiate(animationObjectPrefab, Vector3.zero, animationObjectPrefab.transform.rotation) as GameObject;
//
//             if (Selection.activeGameObject != null)
//             {
//                 animationObject.transform.SetParent(Selection.activeGameObject.transform);
//                 animationObject.transform.localPosition = Vector3.zero;
//             }
//
//             if (creatingObjectName != "" && creatingObjectName != null)
//                 animationObject.name = creatingObjectName;
//
//             // Proveravamo da li smo selektovali neku animaciju
//             if (Instance.animationObjectHasIdle || Instance.animationObjectHasOpen || Instance.animationObjectHasClose)
//             {
//                 // Prvo proveravamo da li postoji direktorijum za level
//                 DirectoryInfo di = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1]);
//                 if (!di.Exists)
//                 {
//                     AssetDatabase.CreateFolder("Assets/Levels", EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1]);
//                 }
//
//                 // Prvo proveravamo da li postoji direktorijum za objekat
//                 DirectoryInfo di2 = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations");
//                 if (!di2.Exists)
//                 {
//                     AssetDatabase.CreateFolder("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1], "Animations");
//                 }
//
//                 DirectoryInfo di3 = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + creatingObjectName);
//                 if (!di3.Exists)
//                 {
//                     AssetDatabase.CreateFolder("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations", creatingObjectName);
//                 }
//
//                 // Kreiramo novi animator sa stanjima koja su odabrana
//                 var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + creatingObjectName + "/" + creatingObjectName + ".controller");
//
//                 // Kreiramo stanja za novi animator
//                 var rootStateMachine = controller.layers[0].stateMachine;
//
//                 var idleState = rootStateMachine.AddState("Idle");
//                 var openState = rootStateMachine.AddState("Open");
//                 var closeState = rootStateMachine.AddState("Close");
//                 // Sedtujemo idle stanje na default stanje
//                 rootStateMachine.defaultState = idleState;
//
//                 // Kreiramo i same animacije
//                 if (Instance.animationObjectHasIdle)
//                 {
//                     AnimationClip idleAnimation = new AnimationClip();
//                     AssetDatabase.CreateAsset(idleAnimation, "Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + creatingObjectName + "/" + creatingObjectName + "_Idle.anim");
//                     AnimationClipSettings idleAnimationSettings = AnimationUtility.GetAnimationClipSettings(idleAnimation);
//                     idleAnimationSettings.loopTime = false;
//                     AnimationUtility.SetAnimationClipSettings(idleAnimation, idleAnimationSettings);
//                     idleState.motion = idleAnimation;
//                 }
//
//                 if (Instance.animationObjectHasOpen)
//                 {
//                     AnimationClip openAnimation = new AnimationClip();
//                     AssetDatabase.CreateAsset(openAnimation, "Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + creatingObjectName + "/" + creatingObjectName + "_Open.anim");
//                     AnimationClipSettings openAnimationSettings = AnimationUtility.GetAnimationClipSettings(openAnimation);
//                     openAnimationSettings.loopTime = false;
//                     AnimationUtility.SetAnimationClipSettings(openAnimation, openAnimationSettings);
//                     openState.motion = openAnimation;
//                 }
//
//                 if (Instance.animationObjectHasClose)
//                 {
//                     AnimationClip closeAnimation = new AnimationClip();
//                     AssetDatabase.CreateAsset(closeAnimation, "Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + creatingObjectName + "/" + creatingObjectName + "_Close.anim");
//                     AnimationClipSettings closeAnimationSettings = AnimationUtility.GetAnimationClipSettings(closeAnimation);
//                     closeAnimationSettings.loopTime = false;
//                     AnimationUtility.SetAnimationClipSettings(closeAnimation, closeAnimationSettings);
//                     closeState.motion = closeAnimation;
//                 }
//
//                 // Kreiramo animator komponentu i dodajemo novi kreirani animator
//                 if (animationObject.transform.Find("AnimationHolder").GetComponent<Animator>() == null)
//                     animationObject.transform.Find("AnimationHolder").gameObject.AddComponent<Animator>();
//                 animationObject.transform.Find("AnimationHolder").gameObject.GetComponent<Animator>().runtimeAnimatorController = controller;
//
//                 Instance.animationObjectHasIdle = false;
//                 Instance.animationObjectHasOpen = false;
//                 Instance.animationObjectHasClose = false;
//                 Instance.animationObjectHasItemUsed = false;
//             }
//         }
//
//         // Funkcija za kreiranje feedback on tap objekta
//         [MenuItem("GameObject/Room Escape/Kreiraj feedback on tap objekat", false, 0)]
//         static void CreateFeedbackOnTapObjectAction()
//         {
//             createAction = new System.Action(CreateFeedbackOnTapObject);
//             SetObjectNameWindow();
//         }
//
//         static void CreateFeedbackOnTapObject()
//         {
//             GameObject feedbackOnTapItemPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/FeedbackOnTapPrefab.prefab", typeof(GameObject)) as GameObject;
//             GameObject feedbackOnTapItemPrefabObject = Instantiate(feedbackOnTapItemPrefab, Vector3.zero, feedbackOnTapItemPrefab.transform.rotation) as GameObject;
//
//             if (Selection.activeGameObject != null)
//             {
//                 feedbackOnTapItemPrefabObject.transform.SetParent(Selection.activeGameObject.transform);
//                 feedbackOnTapItemPrefabObject.transform.localPosition = Vector3.zero;
//             }
//
//             if (creatingObjectName != "" && creatingObjectName != null)
//                 feedbackOnTapItemPrefabObject.name = creatingObjectName + "FeedbackOnTap";
//
//             feedbackOnTapItemPrefabObject.GetComponent<HelperObject>().id = creatingObjectName;
//         }
//
//         // Funkcija za kreiranje animatora za selktovani objekat
//         [MenuItem("GameObject/Room Escape/Kreiraj animator za selektovani object", false, 0)]
//         static void CreateAnimatorForSelectedObjectAction()
//         {
//             createAction = new System.Action(CreateAnimatorForSelectedObject);
//             SetObjectNameWindow();
//         }
//
//         static void CreateAnimatorForSelectedObject()
//         {
//             if (Selection.activeGameObject != null)
//             {
//                 // Prvo proveravamo da li postoji direktorijum za level
//                 DirectoryInfo di = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1]);
//                 if (!di.Exists)
//                 {
//                     AssetDatabase.CreateFolder("Assets/Levels", EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1]);
//                 }
//
//                 // Prvo proveravamo da li postoji direktorijum za objekat
//                 DirectoryInfo di2 = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations");
//                 if (!di2.Exists)
//                 {
//                     AssetDatabase.CreateFolder("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1], "Animations");
//                 }
//
//                 DirectoryInfo di3 = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + Selection.activeGameObject.name);
//                 if (!di3.Exists)
//                 {
//                     AssetDatabase.CreateFolder("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations", Selection.activeGameObject.name);
//                 }
//
//                 // Kreiramo novi animator sa stanjima koja su odabrana
//                 var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + Selection.activeGameObject.name + "/" + Selection.activeGameObject.name + ".controller");
//
//                 // Kreiramo stanja za novi animator
//                 var rootStateMachine = controller.layers[0].stateMachine;
//
//                 var idleState = rootStateMachine.AddState("Idle");
//             
//                 // Sedtujemo idle stanje na default stanje
//                 rootStateMachine.defaultState = idleState;
//
//                 // Kreiramo i same animacije
//                 if (Instance.animationObjectHasIdle)
//                 {
//                     AnimationClip idleAnimation = new AnimationClip();
//                     AssetDatabase.CreateAsset(idleAnimation, "Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + Selection.activeGameObject.name + "/" + Selection.activeGameObject.name + "_Idle.anim");
//                     AnimationClipSettings idleAnimationSettings = AnimationUtility.GetAnimationClipSettings(idleAnimation);
//                     idleAnimationSettings.loopTime = false;
//                     AnimationUtility.SetAnimationClipSettings(idleAnimation, idleAnimationSettings);
//                     idleState.motion = idleAnimation;
//                 }
//
//                 if (Instance.animationObjectHasItemUsed)
//                 {
//                     var itemUsedState = rootStateMachine.AddState("ItemUsed");
//                     AnimationClip itemUsedAnimation = new AnimationClip();
//                     AssetDatabase.CreateAsset(itemUsedAnimation, "Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + Selection.activeGameObject.name + "/" + Selection.activeGameObject.name + "_ItemUsed.anim");
//                     AnimationClipSettings itemUsedAnimationSettings = AnimationUtility.GetAnimationClipSettings(itemUsedAnimation);
//                     itemUsedAnimationSettings.loopTime = false;
//                     AnimationUtility.SetAnimationClipSettings(itemUsedAnimation, itemUsedAnimationSettings);
//                     itemUsedState.motion = itemUsedAnimation;
//                 }
//
//                 if (Instance.animationObjectHasOpen)
//                 {
//                     var openState = rootStateMachine.AddState("Open");
//                     AnimationClip openAnimation = new AnimationClip();
//                     AssetDatabase.CreateAsset(openAnimation, "Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + Selection.activeGameObject.name + "/" + Selection.activeGameObject.name + "_Open.anim");
//                     AnimationClipSettings openAnimationSettings = AnimationUtility.GetAnimationClipSettings(openAnimation);
//                     openAnimationSettings.loopTime = false;
//                     AnimationUtility.SetAnimationClipSettings(openAnimation, openAnimationSettings);
//                     openState.motion = openAnimation;
//                 }
//
//                 if (Instance.animationObjectHasClose)
//                 {
//                     var closeState = rootStateMachine.AddState("Close");
//                     AnimationClip closeAnimation = new AnimationClip();
//                     AssetDatabase.CreateAsset(closeAnimation, "Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + Selection.activeGameObject.name + "/" + Selection.activeGameObject.name + "_Close.anim");
//                     AnimationClipSettings closeAnimationSettings = AnimationUtility.GetAnimationClipSettings(closeAnimation);
//                     closeAnimationSettings.loopTime = false;
//                     AnimationUtility.SetAnimationClipSettings(closeAnimation, closeAnimationSettings);
//                     closeState.motion = closeAnimation;
//                 }
//
//                 Transform animationHolderObject = Selection.activeGameObject.transform.Find("AnimationHolder");
//
//                 #region Old
//                 // Kreiramo animator komponentu i dodajemo novi kreirani animator
//                 //if (Selection.activeGameObject.transform.Find("AnimationHolder").GetComponent<Animator>() == null)
//                 //  Selection.activeGameObject.transform.Find("AnimationHolder").gameObject.AddComponent<Animator>();
//
//                 //Selection.activeGameObject.transform.Find("AnimationHolder").gameObject.GetComponent<Animator>().runtimeAnimatorController = controller;
//                 #endregion
//
//                 #region ArtistUpdate
//                 if (animationHolderObject != null && animationHolderObject.gameObject.GetComponent<Animator>() == null)
//                 {
//                     animationHolderObject.gameObject.AddComponent<Animator>();
//                     animationHolderObject.gameObject.GetComponent<Animator>().runtimeAnimatorController = controller;
//                 }
//                 else
//                 {
//                     Selection.activeGameObject.AddComponent<Animator>();
//                     Selection.activeGameObject.GetComponent<Animator>().runtimeAnimatorController = controller;
//                 }
//                 #endregion
//
//
//                 Instance.animationObjectHasIdle = false;
//                 Instance.animationObjectHasOpen = false;
//                 Instance.animationObjectHasClose = false;
//                 Instance.animationObjectHasItemUsed = false;
//
//                 EditorUtility.SetDirty(Selection.activeGameObject);
//             }
//         }
//
//         // Dodatanje hotspot objekta za selektovani objekat
//         [MenuItem("GameObject/Room Escape/Dodaj Hotspot za selektovani objekat", false, 12)]
//         public static void AddHotspotToSelectedObject()
//         {
//             if (Selection.activeGameObject != null)
//             {
//                 GameObject hotspotPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/HotspotHolder.prefab", typeof(GameObject)) as GameObject;
//                 GameObject hotspotObject = Instantiate(hotspotPrefab, Vector3.zero, hotspotPrefab.transform.rotation) as GameObject;
//
//                 hotspotObject.transform.SetParent(Selection.activeGameObject.transform);
//                 hotspotObject.transform.localScale = Vector3.one;
//                 hotspotObject.transform.localPosition = Vector3.zero;
//
//                 hotspotObject.name = "HotspotHolder";
//             }
//         }
//
//         //[MenuItem("GameObject/Room Escape/Kreiraj mini game", false, 0)]
//         //static void CreateMiniGameAction()
//         //{
//         //    createAction = new System.Action(CreateMiniGame);
//         //    SetObjectNameWindow();
//         //}
//
//         //static void CreateMiniGame()
//         //{
//         //    GameObject miniGamePrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/MiniGamePrefab.prefab", typeof(GameObject)) as GameObject;
//         //    GameObject miniGameObject = Instantiate(miniGamePrefab, Vector3.zero, miniGamePrefab.transform.rotation) as GameObject;
//
//         //    if (Selection.activeGameObject != null)
//         //    {
//         //        miniGameObject.transform.SetParent(Selection.activeGameObject.transform);
//         //        miniGameObject.transform.localPosition = Vector3.zero;
//         //    }
//
//         //    if (creatingObjectName != "" && creatingObjectName != null)
//         //        miniGameObject.name = creatingObjectName + "MiniGame";
//         //}
//
//         public static GameObjectMenu Instance;
//         private static string creatingObjectName;
//         public static System.Action createAction;
//
//         public static AudioClip creatingAmbientSound;
//
//         // Prozor za unosenje imena objekta
//         public static void SetObjectNameWindow()
//         {
//             if (Instance == null)
//             {
//                 GameObjectMenu setObjectNamePopup = new GameObjectMenu();
//
//                 setObjectNamePopup.minSize = new Vector2(600f, 500f);
//                 setObjectNamePopup.maxSize = new Vector2(600f, 500f);
//                 setObjectNamePopup.titleContent.text = "Unesite ime objekta: ";
//
//                 setObjectNamePopup.ShowUtility();
//             }
//         }
//
//         private void OnGUI()
//         {
//             EditorGUILayout.Space();
//             EditorGUILayout.BeginVertical("box");
//             EditorGUILayout.Space();
//
//             if (createAction != CreateAnimatorForSelectedObject)
//             {
//                 creatingObjectName = EditorGUILayout.TextField("Ime objekta", creatingObjectName);
//
//                 EditorGUILayout.Space();
//             }
//
//             if (createAction == CreateAnimationObject)
//             {
//                 EditorGUILayout.LabelField("Odaberite animacije:");
//                 EditorGUILayout.Space();
//
//                 EditorGUILayout.BeginHorizontal();
//                 animationObjectHasIdle = EditorGUILayout.Toggle("Idle", animationObjectHasIdle, GUILayout.ExpandWidth(false), GUILayout.Height(60f));
//                 EditorGUILayout.EndHorizontal();
//
//                 EditorGUILayout.BeginHorizontal();
//                 animationObjectHasOpen = EditorGUILayout.Toggle("Open", animationObjectHasOpen, GUILayout.ExpandWidth(false), GUILayout.Height(60f));
//                 EditorGUILayout.EndHorizontal();
//
//                 EditorGUILayout.BeginHorizontal();
//                 animationObjectHasClose = EditorGUILayout.Toggle("Close", animationObjectHasClose, GUILayout.ExpandWidth(false), GUILayout.Height(60f));
//                 EditorGUILayout.EndHorizontal();
//             }
//             else if (createAction == CreateTargetObject || createAction == CreateMultipleItemsTargetObject)
//             {
//                 EditorGUILayout.BeginHorizontal();
//                 animationObjectHasIdle = EditorGUILayout.Toggle("Idle", animationObjectHasIdle, GUILayout.ExpandWidth(false), GUILayout.Height(60f));
//                 EditorGUILayout.EndHorizontal();
//
//                 EditorGUILayout.BeginHorizontal();
//                 animationObjectHasItemUsed = EditorGUILayout.Toggle("ItemUsed", animationObjectHasItemUsed, GUILayout.ExpandWidth(false), GUILayout.Height(60f));
//                 EditorGUILayout.EndHorizontal();
//
//                 EditorGUILayout.BeginHorizontal();
//                 animationObjectHasOpen = EditorGUILayout.Toggle("Open", animationObjectHasOpen, GUILayout.ExpandWidth(false), GUILayout.Height(60f));
//                 EditorGUILayout.EndHorizontal();
//
//                 EditorGUILayout.BeginHorizontal();
//                 animationObjectHasClose = EditorGUILayout.Toggle("Close", animationObjectHasClose, GUILayout.ExpandWidth(false), GUILayout.Height(60f));
//                 EditorGUILayout.EndHorizontal();
//             }
//             else if (createAction == CreateAnimatorForSelectedObject)
//             {
//                 EditorGUILayout.LabelField("Odaberite animacije:");
//                 EditorGUILayout.Space();
//
//                 EditorGUILayout.BeginHorizontal();
//                 animationObjectHasIdle = EditorGUILayout.Toggle("Idle", animationObjectHasIdle, GUILayout.ExpandWidth(false), GUILayout.Height(60f));
//                 EditorGUILayout.EndHorizontal();
//
//                 if (Selection.activeGameObject.GetComponent<TargetItem>() != null)
//                 {
//                     EditorGUILayout.BeginHorizontal();
//                     animationObjectHasItemUsed = EditorGUILayout.Toggle("ItemUsed", animationObjectHasItemUsed, GUILayout.ExpandWidth(false), GUILayout.Height(60f));
//                     EditorGUILayout.EndHorizontal();
//                 }
//
//                 if (Selection.activeGameObject.GetComponent<TargetItem>() != null || Selection.activeGameObject.GetComponent<AnimationItem>())
//                 {
//                     EditorGUILayout.BeginHorizontal();
//                     animationObjectHasOpen = EditorGUILayout.Toggle("Open", animationObjectHasOpen, GUILayout.ExpandWidth(false), GUILayout.Height(60f));
//                     EditorGUILayout.EndHorizontal();
//                 }
//
//                 if (Selection.activeGameObject.GetComponent<TargetItem>() != null || Selection.activeGameObject.GetComponent<AnimationItem>())
//                 {
//                     EditorGUILayout.BeginHorizontal();
//                     animationObjectHasClose = EditorGUILayout.Toggle("Close", animationObjectHasClose, GUILayout.ExpandWidth(false), GUILayout.Height(60f));
//                     EditorGUILayout.EndHorizontal();
//                 }
//             }
//             else if(createAction == CreateArtistInteractiveObject)
//             {
//                 EditorGUILayout.BeginHorizontal();
//                 isChangingPosition = EditorGUILayout.Toggle("Should Move?", isChangingPosition, GUILayout.ExpandWidth(false));
//                 EditorGUILayout.EndHorizontal();
//             }
//             else if (createAction == CreateObjectWithStates)
//             {
//                 numberOfStatesForStatesObject = EditorGUILayout.IntField("Broj stanja", numberOfStatesForStatesObject);
//
//                 EditorGUILayout.Space();
//             }
//             else if (createAction == CreateAmbientSound)
//             {
//                 creatingAmbientSound = (AudioClip)EditorGUILayout.ObjectField("Zvuk", creatingAmbientSound, typeof(AudioClip), false, GUILayout.ExpandWidth(true));
//
//                 EditorGUILayout.Space();
//             }
//
//             EditorGUILayout.EndVertical();
//
//             EditorGUILayout.BeginHorizontal();
//             bool createButton = GUILayout.Button("Kreiraj", GUILayout.Width(140f));
//
//             if (createButton)
//             {
//                 CreateButtonClicked();
//             }
//
//             bool cancelButton = GUILayout.Button("Odustani", GUILayout.Width(140f));
//
//             if (cancelButton)
//             {
//                 CancelButtonClicked();
//             }
//
//             EditorGUILayout.EndHorizontal();
//         }
//
//         public void CreateButtonClicked()
//         {
//             createAction();
//             Instance.Close();
//         }
//
//         public void CancelButtonClicked()
//         {
//             creatingObjectName = "";
//             Instance.Close();
//         }
//
//         private void OnEnable()
//         {
//             animationObjectHasIdle = false;
//             animationObjectHasOpen = false;
//             animationObjectHasClose = false;
//             animationObjectHasItemUsed = false;
//             numberOfStatesForStatesObject = 0;
//             Instance = this;
//         }
//
//         // Funkcija za kreiranje objekta na sceni koji menja stanje
//         [MenuItem("GameObject/Room Escape/Kreiraj objekat koji menja stanje (mini objekti na sceni)", false, 0)]
//         static void CreateObjectWithStatesAction()
//         {
//             createAction = new System.Action(CreateObjectWithStates);
//             SetObjectNameWindow();
//         }
//
//         static void CreateObjectWithStates()
//         {
//             GameObject objectPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/ObjectWithStates.prefab", typeof(GameObject)) as GameObject;
//             GameObject statesObject = Instantiate(objectPrefab, Vector3.zero, objectPrefab.transform.rotation) as GameObject;
//
//             if (Selection.activeGameObject != null)
//             {
//                 statesObject.transform.SetParent(Selection.activeGameObject.transform);
//                 statesObject.transform.localPosition = Vector3.zero;
//             }
//
//             if (creatingObjectName != "" && creatingObjectName != null)
//                 statesObject.name = creatingObjectName;
//
//             // Prvo proveravamo da li postoji direktorijum za level
//             DirectoryInfo di = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1]);
//             if (!di.Exists)
//             {
//                 AssetDatabase.CreateFolder("Assets/Levels", EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1]);
//             }
//
//             // Prvo proveravamo da li postoji direktorijum za objekat
//             DirectoryInfo di2 = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations");
//             if (!di2.Exists)
//             {
//                 AssetDatabase.CreateFolder("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1], "Animations");
//             }
//
//             DirectoryInfo di3 = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + creatingObjectName);
//             if (!di3.Exists)
//             {
//                 AssetDatabase.CreateFolder("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations", creatingObjectName);
//             }
//
//             // Kreiramo novi animator sa stanjima koja su odabrana
//             var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + creatingObjectName + "/" + creatingObjectName + ".controller");
//
//             // FIXME ovo napraviti
//             // Kreiramo stanja za novi animator
//             var rootStateMachine = controller.layers[0].stateMachine;
//
//             var idleState = rootStateMachine.AddState("Idle");
//             var lastCreatedState = idleState;
//
//             // Kreiramo trigger parametar
//             AnimatorControllerParameter changeStateTrigger = new AnimatorControllerParameter();
//             changeStateTrigger.name = "ChangeState";
//             changeStateTrigger.type = AnimatorControllerParameterType.Trigger;
//
//             controller.AddParameter(changeStateTrigger);
//
//             // Za svako stanje kreiramo animaciju i dodajemo je stanju
//             for (int i = 0; i < Instance.numberOfStatesForStatesObject; i++)
//             {
//                 var newState = rootStateMachine.AddState("State" + (i + 1).ToString());
//
//                 AnimationClip newAnimation = new AnimationClip();
//                 AssetDatabase.CreateAsset(newAnimation, "Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + creatingObjectName + "/" + creatingObjectName + "State" + (i + 1).ToString() + ".anim");
//                 AnimationClipSettings newStateAnimationSettings = AnimationUtility.GetAnimationClipSettings(newAnimation);
//                 newStateAnimationSettings.loopTime = false;
//                 AnimationUtility.SetAnimationClipSettings(newAnimation, newStateAnimationSettings);
//                 newState.motion = newAnimation;
//
//
//                 UnityEditor.Animations.AnimatorConditionMode conditionMode = UnityEditor.Animations.AnimatorConditionMode.If;
//
//                 var transition = lastCreatedState.AddTransition(newState, true);
//                 transition.AddCondition(conditionMode, 0, "ChangeState");
//                 lastCreatedState = newState;
//             }
//
//             // Sedtujemo idle stanje na default stanje
//             rootStateMachine.defaultState = idleState;
//
//             statesObject.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = controller;
//             statesObject.transform.GetChild(0).gameObject.AddComponent<KeepAnimatorState>();
//             EditorUtility.SetDirty(statesObject);
//         }
//
//         // Funkcija za kreiranje animiranog objekta koji se kreira prilikom kombinovanja dva ili vise elemenata
//         [MenuItem("GameObject/Room Escape/Kreiraj animirani objekat za kombinovanje dva ili vise objekata", false, 0)]
//         static void CreateAnimatedCombinigObjectAction()
//         {
//             createAction = new System.Action(CreateAnimatedCombiningObject);
//             SetObjectNameWindow();
//         }
//
//         static void CreateAnimatedCombiningObject()
//         {
//             // Kreiramo inventory objekat za ovaj objekat
//             GameObject animatedObject = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/CombineAnimationObject.prefab", typeof(GameObject)) as GameObject;
//             GameObject combineAnimationObject = Instantiate(animatedObject, GameObject.Find("Canvas").transform) as GameObject;
//
//             if (creatingObjectName != "" && creatingObjectName != null)
//                 combineAnimationObject.name = creatingObjectName + "CombineAnimation";
//
//             // Prvo proveravamo da li postoji direktorijum za level
//             DirectoryInfo di = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1]);
//             if (!di.Exists)
//             {
//                 AssetDatabase.CreateFolder("Assets/Levels", EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1]);
//             }
//
//             // Prvo proveravamo da li postoji direktorijum za objekat
//             DirectoryInfo di2 = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations");
//             if (!di2.Exists)
//             {
//                 AssetDatabase.CreateFolder("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1], "Animations");
//             }
//
//             DirectoryInfo di3 = new DirectoryInfo("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + combineAnimationObject.name);
//             if (!di3.Exists)
//             {
//                 AssetDatabase.CreateFolder("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations", combineAnimationObject.name);
//             }
//
//             // Kreiramo novi animator sa stanjima koja su odabrana
//             var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath("Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + combineAnimationObject.name + "/" + combineAnimationObject.name + ".controller");
//
//             // Kreiramo stanja za novi animator
//             var rootStateMachine = controller.layers[0].stateMachine;
//
//             var idleState = rootStateMachine.AddState("Idle");
//             var combineState = rootStateMachine.AddState("CombineItems");
//
//             // Sedtujemo idle stanje na default stanje
//             rootStateMachine.defaultState = idleState;
//
//             // Kreiramo i same animacije
//             AnimationClip combineAnimation = new AnimationClip();
//             AssetDatabase.CreateAsset(combineAnimation, "Assets/Levels/" + EditorApplication.currentScene.Split('.')[0].Split('/')[EditorApplication.currentScene.Split('.')[0].Split('/').Length - 1] + "/Animations/" + combineAnimationObject.name + "/" + combineAnimationObject.name + "_CombineItems.anim");
//             AnimationClipSettings combineAnimationSettings = AnimationUtility.GetAnimationClipSettings(combineAnimation);
//             combineAnimationSettings.loopTime = false;
//             AnimationUtility.SetAnimationClipSettings(combineAnimation, combineAnimationSettings);
//             combineState.motion = combineAnimation;
//
//             combineAnimationObject.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = controller;
//
//             // Proveravamo da li folder za inventory iteme za ovaj level postoji
//             DirectoryInfo dirInfo = new DirectoryInfo("Assets/Levels/" + EditorSceneManager.GetActiveScene().name);
//
//             if (!dirInfo.Exists)
//             {
//                 AssetDatabase.CreateFolder("Assets/Levels", EditorSceneManager.GetActiveScene().name);
//             }
//
//             DirectoryInfo dirInfo2 = new DirectoryInfo("Assets/Levels/" + EditorSceneManager.GetActiveScene().name + "/CombinedItemsAnimationPrefabs");
//
//             if (!dirInfo2.Exists)
//             {
//                 AssetDatabase.CreateFolder("Assets/Levels/" + EditorSceneManager.GetActiveScene().name, "CombinedItemsAnimationPrefabs");
//             }
//
//             string inventoryItemLocalPath = "Assets/Levels/" + EditorSceneManager.GetActiveScene().name + "/CombinedItemsAnimationPrefabs/" + combineAnimationObject.name + ".prefab";
//             inventoryItemLocalPath = AssetDatabase.GenerateUniqueAssetPath(inventoryItemLocalPath);
//
//             PrefabUtility.SaveAsPrefabAsset(combineAnimationObject, inventoryItemLocalPath);
//
//             DestroyImmediate(combineAnimationObject);
//         }
//
//         // Funkcija za kreiranje sobe
//         [MenuItem("GameObject/Room Escape/Kreiraj level finish objekat", false, 0)]
//         static void LevelFinishAction()
//         {
//             //createAction = new System.Action(CreateLevelFinishObject);
//             //SetObjectNameWindow();
//             CreateLevelFinishObject();
//         }
//
//         static void CreateLevelFinishObject()
//         {
//             GameObject levelFinishPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/LevelFinishedObject.prefab", typeof(GameObject)) as GameObject;
//             GameObject levelFinishObject = Instantiate(levelFinishPrefab, Vector3.zero, levelFinishPrefab.transform.rotation) as GameObject;
//
//             if (Selection.activeGameObject != null)
//             {
//                 levelFinishObject.transform.SetParent(Selection.activeGameObject.transform);
//                 levelFinishObject.transform.localPosition = Vector3.zero;
//             }
//
//             levelFinishObject.name = "LevelFinishedObject";
//             //if (creatingObjectName != "" && creatingObjectName != null)
//             //    levelFinishObject.name = creatingObjectName;
//         }
//
//         // Funkcija za kreiranje itema koji skida zivot
//         [MenuItem("GameObject/Room Escape/Kreiraj objekat koji oduzima zivot", false, 0)]
//         static void CreateDangerousObjectAction()
//         {
//             createAction = new System.Action(CreateDangerousObject);
//             SetObjectNameWindow();
//         }
//
//         static void CreateDangerousObject()
//         {
//             GameObject dangerousPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameElements/DangerousItem.prefab", typeof(GameObject)) as GameObject;
//             GameObject dangerousPrefabObject = Instantiate(dangerousPrefab, Vector3.zero, dangerousPrefab.transform.rotation) as GameObject;
//
//             if (Selection.activeGameObject != null)
//             {
//                 dangerousPrefabObject.transform.SetParent(Selection.activeGameObject.transform);
//                 dangerousPrefabObject.transform.localPosition = Vector3.zero;
//             }
//
//             if (creatingObjectName != "" && creatingObjectName != null)
//                 dangerousPrefabObject.name = creatingObjectName;
//         }
//
//         // Funkcija za kreiranje ambijentalnog zvuka
//         [MenuItem("GameObject/Room Escape/Kreiraj ambijentalni zvuk", false, 0)]
//         static void CreateAmbientSoundAction()
//         {
//             createAction = new System.Action(CreateAmbientSound);
//             SetObjectNameWindow();
//         }
//
//         static void CreateAmbientSound()
//         {
//             // Prvo proveravamo na koju je sobu kliknuo i samo ukolio 
//             // jeste na neku onda kreiramo ambijentalne zvukove za tu sobu
//             GameObject currentObject;
//
//             if (Selection.activeGameObject != null)
//             {
//                 currentObject = Selection.activeGameObject;
//
//                 while (currentObject.transform.parent != null)
//                 {
//                     if (currentObject.transform.parent.name == "RoomsHolder")
//                         break;
//                     else
//                         currentObject = currentObject.transform.parent.gameObject;
//                 }
//
//                 // Proveravamo koji od gornja dva uslova je ispunjen, tj. da li smo bili u nekoj sobi ili smo kliknuli u prazan prostor
//                 if (currentObject.transform.parent != null && currentObject.transform.parent.name == "RoomsHolder") // Dobar objekat
//                 {
//                     if (currentObject.transform.Find("AmbientSounds") == null)
//                     {
//                         GameObject ambientSounds = new GameObject("AmbientSounds");
//                         ambientSounds.transform.SetParent(currentObject.transform);
//                         ambientSounds.transform.localScale = Vector3.one;
//                         ambientSounds.transform.localPosition = Vector3.zero;
//                     }
//
//                     GameObject newAmbientSound = new GameObject(creatingObjectName);
//                     newAmbientSound.AddComponent<AmbientSound>();
//                     newAmbientSound.transform.parent = currentObject.transform.Find("AmbientSounds").transform;
//
//                     newAmbientSound.GetComponent<AudioSource>().playOnAwake = false;
//
//                     if (creatingAmbientSound != null)
//                         newAmbientSound.GetComponent<AudioSource>().clip = creatingAmbientSound;
//                 }
//             }
//
//         
//
//             //if (GameObject.Find("RoomsHolder") == null)
//             //{
//             //    GameObject roomsHolder = new GameObject("RoomsHolder");
//             //}
//
//             //if (GameObject.Find("AmbientSounds") == null)
//             //{
//             //    GameObject ambientSounds = new GameObject("AmbientSounds");
//             //    ambientSounds.transform.SetParent(GameObject.Find("RoomsHolder").transform);
//             //}
//
//             //GameObject newAmbientSound = new GameObject(creatingObjectName);
//             //newAmbientSound.AddComponent<AmbientSound>();
//             //newAmbientSound.transform.parent = GameObject.Find("RoomsHolder/AmbientSounds").transform;
//
//             //newAmbientSound.GetComponent<AudioSource>().playOnAwake = false;
//
//             //if (creatingAmbientSound != null)
//             //    newAmbientSound.GetComponent<AudioSource>().clip = creatingAmbientSound;
//         }
//
//         // Pomocna funkcija za dobijanje indeksa za item koji trebamo da kreiramo
//         public int CalculateLastCreatedItemIndex()
//         {
//             int currentLastIndex = -1;
//
//             DirectoryInfo dirInfo = new DirectoryInfo("Assets/Levels/" + EditorSceneManager.GetActiveScene().name);
//             FileInfo[] fileInfo = dirInfo.GetFiles("*.prefab");
//
//             if (fileInfo.Length > 0)
//             {
//                 for (int i = 0; i < fileInfo.Length; i++)
//                 {
//                     Object objAsset = AssetDatabase.LoadAssetAtPath("Assets/Levels/" + EditorSceneManager.GetActiveScene().name + "/" + fileInfo[i].Name, typeof(Object));
//
//                     GameObject temp = Instantiate(objAsset) as GameObject;
//
//                     if (temp.GetComponent<InventoryItem>().itemIndex > currentLastIndex)
//                         currentLastIndex = temp.GetComponent<InventoryItem>().itemIndex;
//
//                     DestroyImmediate(temp);
//                 }
//             }
//
//             GameObject[] invItemsOnScene = GameObject.FindObjectsOfTypeAll(typeof(InventoryItem)) as GameObject[];
//
//             if (invItemsOnScene != null)
//                 for (int i = 0; i < invItemsOnScene.Length; i++)
//                     DestroyImmediate(invItemsOnScene[i]);
//
//             if (currentLastIndex != -1)
//                 return currentLastIndex;
//             else
//                 return 0;
//         }
//         
//         // -------------------------------------------------------------------------------------
//         
//   
//         [MenuItem("Level Tools/Promeni rooms holder i pokreni level", false, 0)]
//         static void SwitchRoomsHolder()
//         {
//             ChangeEditorBuildSettingsForTest();
//             string roomsHolderAssetPath = GetRoomsHolderAssetPath();
//             SaveLevelEditScene();
//             if (roomsHolderAssetPath != "")
//             {
//                 UpdateRoomsHolder(roomsHolderAssetPath);
//                 ChangeRoomsHolderInTestLevelScene(roomsHolderAssetPath);
//             }
//             EnterPlayMode();
//         }
//         
//         [MenuItem("Level Tools/Pokreni level", false, 0)]
//         static void EnterPlayMode()
//         {
//             EditorSceneManager.SetActiveScene(EditorSceneManager.OpenScene("Assets/Scenes/TestLevelSplash.unity"));
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
//                     Debug.LogError("Build Settings: Scene does not exist");
//                 }
//             }
//
//             EditorBuildSettings.scenes = sceneList.ToArray();
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
//         static void SaveLevelEditScene()
//         {
//             // If we are on the scene, save scene and override prefab first
//             GameObject sceneRoomsHolder = GameObject.Find("RoomsHolder");
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
//                 foreach (Transform zone in room)
//                 {
//                     if (zone.gameObject.GetComponent<ZoomZone>() || zone.gameObject.GetComponent<MiniGame>())
//                     {
//                         if(zone.GetChild(0).gameObject.activeSelf)
//                         {
//                             zone.GetChild(0).gameObject.SetActive(false);
//                             Debug.Log($"Turned off zone: " + zone.name);
//                         }
//                         
//                         SetLayerAllChildren(zone.transform, zoomElementLayer);
//                     }
//                 }
//
//                 if (room.gameObject.name != "AudioObjects")
//                 {
//                     room.gameObject.SetActive(false);
//                 }
//             }
//
//             roomsHolder.transform.GetChild(0).gameObject.SetActive(true);
//         }
//         
//         static void SetLayerAllChildren(Transform root, int layer)
//         {
//             Transform[] children = root.GetComponentsInChildren<Transform>(true);
//             foreach (Transform child in children)
//             {
//                 child.gameObject.layer = layer;
//             }
//         }
//         
//         static void ChangeRoomsHolderInTestLevelScene(string roomsHolderAssetPath)
//         {
//             SceneManager.SetActiveScene(EditorSceneManager.OpenScene("Assets/Scenes/TestLevel.unity"));
//             GameObject oldRoomsHolder = GameObject.Find("RoomsHolder");
//             if (oldRoomsHolder != null)
//             {
//                 string oldPrefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(oldRoomsHolder);
//                 if (oldPrefabPath == roomsHolderAssetPath)
//                 {
//                     Debug.Log($"Isti level je pokrenut");
//                     return;
//                 }
//                 
//                 DestroyImmediate(oldRoomsHolder);
//             }
//
//             GameObject roomsHolderPrefabAsset =
//                 AssetDatabase.LoadAssetAtPath(roomsHolderAssetPath, typeof(GameObject)) as GameObject;
//             GameObject roomsHolderPrefabRoot = (GameObject) PrefabUtility.InstantiatePrefab(roomsHolderPrefabAsset, SceneManager.GetActiveScene());
//             roomsHolderPrefabRoot.transform.SetAsLastSibling();
//             EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
//         }
//     }
// }