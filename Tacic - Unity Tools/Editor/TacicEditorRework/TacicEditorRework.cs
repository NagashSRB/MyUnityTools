using System;
using System.Collections.Generic;
using System.Linq;
using Tacic.Tacic___Unity_Tools.Scripts;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
//TODO : Izbaciti UnityEndigne.SceneManagement

namespace Tacic.Tacic___Unity_Tools.Editor.TacicEditorRework
{
    public class TacicEditor : EditorWindow
    {
        #region HelperFunctions


        [MenuItem("GameObject/Talenzzo/TempTest", false, 0)]
        static void TempTest()
        {
            //Debug.Log($"{Selection.activeGameObject.name}");
            
            //Debug.Log(EditorSceneManager.GetActiveScene().name);

            TacicEditorRooms.CloseZonesAndRooms(GameObject.Find("RoomsHolder").transform);
        }

        #endregion

        #region GameObjectMenu

        // -1 if not found
        [MenuItem("GameObject/Pokretanje levela - Tacic/TEST = Da li objekat ima hint priority", false, 0)]
        static int GetSelectedGameObjectHintPriorityAttribute()
        {
            GameObject[] selectedObjects = GetSelectedGameObjects();
            if (selectedObjects == null || selectedObjects.Length != 1)
            {
                Debug.LogError($"Select one object where you want to start level from");
                return -1;
            }

            GameObject selectedObject = selectedObjects[0];
            Component[] components = selectedObject.GetComponents(typeof(Component));
            foreach(Component component in components) 
            { 
                SerializedObject serializedComponent = new SerializedObject(component);
                serializedComponent.Update();
                string hintPriorityPropertyName = "hintPriority";
                SerializedProperty hintPriorityProperty = serializedComponent.FindProperty(hintPriorityPropertyName);
                if (hintPriorityProperty == null)
                {
                    Debug.Log($"HintPriority not found for object {component.name} / {component.ToString()}");
                    continue;
                }

                Debug.Log($"HintPriority :Component: {component.name} / {component.ToString()}, Value:{hintPriorityProperty.intValue}");
                return hintPriorityProperty.intValue;
                //serializedComponent.ApplyModifiedProperties();
            }

            return -1;
        }

        #endregion

        #region Editor Window

        private void OnEnable()
        {
            //ResetWindow();
            Instance = this;
            //previousState = EditorWindowState.None;
            //currentState = EditorWindowState.None;
        }

        public static TacicEditor Instance;

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.Space();

            SwitchWindowState();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            bool okButton = GUILayout.Button("OK", GUILayout.Width(140f));
            bool cancelButton = GUILayout.Button("Cancel", GUILayout.Width(140f));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            if (okButton)
            {
                OkButtonClicked();
            }

            if (cancelButton)
            {
                CancelButtonClicked();
            }
        }

        static void CancelButtonClicked()
        {
            Instance.Close();
        }

        static void OkButtonClicked()
        {
            okClicked();
            Instance.Close();
        }

        static void Show(EditorWindowState stt)
        {
            if (Instance == null)
            {
                previousWindowState = currentWindowState;
                currentWindowState = stt;
                if (previousWindowState != null && previousWindowState != currentWindowState)
                {
                    ResetWindow();
                }

                TacicEditor setObjectNamePopup = new TacicEditor();

                setObjectNamePopup.minSize = new Vector2(400f, 500f);
                setObjectNamePopup.maxSize = new Vector2(1000f, 1000f);
                setObjectNamePopup.titleContent.text = "Custom Editor Window u pokusaju ";

                setObjectNamePopup.ShowUtility();
            }
        }

        #endregion

        #region Custom Windows

        #region Custom Windows Settings

        static void SwitchWindowState()
        {
            switch (currentWindowState)
            {
                case EditorWindowState.TestWindow:
                    TestWindow();
                    break;
                case EditorWindowState.SearchByNameWindow:
                    SearchByNameWindow();
                    break;
                case EditorWindowState.ReferenceSelfComponentWindow:
                    ReferenceSelfComponentWindow();
                    break;
                case EditorWindowState.CreateMatrixWindow:
                    CreateMatrixWindow();
                    break;
                case EditorWindowState.EditPositionListWindow:
                    EditPositionListWindow();
                    break;
                case EditorWindowState.ReferenceMatrixWindow:
                    ReferenceMatrixWindow();
                    break;
                case EditorWindowState.CreateGameObjectsWindow:
                    CreateGameObjectsWindow();
                    break;
            }
        }

        static void ResetWindow()
        {
            inputString = "";
            inputString2 = "";
            inputString3 = "";
            inputBool1 = false;
            inputBool2 = false;
            inputFloat = 0;
            inputFloat2 = 0;
            inputInt = 0;
            inputInt2 = 0;
            inputVector2 = Vector2.zero;
        }

        public enum EditorWindowState
        {
            None,
            TestWindow,
            SearchByNameWindow,
            ReferenceSelfComponentWindow,
            CreateMatrixWindow,
            EditPositionListWindow,
            ReferenceMatrixWindow,
            CreateGameObjectsWindow
        }

        static Action okClicked;
        public static EditorWindowState currentWindowState;
        public static EditorWindowState previousWindowState;
        private static string inputString;
        private static string inputString2;
        private static string inputString3;
        private static bool inputBool1;
        private static bool inputBool2;
        private static float inputFloat;
        private static float inputFloat2;
        private static int inputInt;
        private static int inputInt2;
        private static GameObject inputGameObject;
        private static Component inputComponent;
        private static Vector2 inputVector2;
    

        #endregion
    
        #region TemplateWindow
    
        static void TemplateWindow()
        {
            inputString = EditorGUILayout.TextField("String za ispisivanje:", inputString);
            TemplateWindowUpdate();
        }

        // [MenuItem("GameObject/Talenzzo/In Test - Editor Window", false, 0)]
        static void TemplateOption()
        {
            okClicked = OnTestFinished;
            Show(EditorWindowState.TestWindow);
        }

        static void OnTemplateFinished()
        {
            Debug.Log(inputString);
        }

        static void TemplateWindowUpdate()
        {
        
        }
    
        #endregion

        #region TestWindow

        static void TestWindow()
        {
            inputString = EditorGUILayout.TextField("String za ispisivanje:", inputString);
            // toggle / checkbox
            // boolX = EditorGUILayout.Toggle("Open", boolX, GUILayout.ExpandWidth(false), GUILayout.Height(60f));

            // int field
            // intX = EditorGUILayout.IntField("Broj stanja", intX);

            // object reference, example: sound. Mozda podesavanje da moze i sa scene
            // soundX = (AudioClip)EditorGUILayout.ObjectField("Zvuk", soundX, typeof(AudioClip), false, GUILayout.ExpandWidth(true));

            // EditorGUILayout.DropdownButton(new GUIContent("Choose component"), FocusType.Keyboard);
        
            // inputFloat = EditorGUILayout.Slider("Tekst", inputFloat, 0, 5);
        }

        // [MenuItem("GameObject/Talenzzo/In Test - Editor Window", false, 0)]
        static void TestMenuOption()
        {
            okClicked = OnTestFinished;
            Show(EditorWindowState.TestWindow);
        }

        static void OnTestFinished()
        {
            Debug.Log(inputString);
        }

        #endregion

        #region SearchByNameWindow

        static void SearchByNameWindow()
        {
            inputString = EditorGUILayout.TextField("Tacno ime objekta (case sensitive) :", inputString);
        }

        [MenuItem("GameObject/Talenzzo/Search selected objects by exact name", false, 0)]
        static void SeachByNameOption()
        {
            okClicked = OnSearchByNameWindowFinished;
            Show(EditorWindowState.SearchByNameWindow);
        }

        static void OnSearchByNameWindowFinished()
        {
            GameObject[] gameObjects = GetSelectedGameObjects();
            if (gameObjects == null)
            {
                return;
            }

            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.name != inputString)
                {
                    ArrayUtility.Remove(ref gameObjects, gameObject);
                    //gameObjects.Remove(gameObject);
                }
            }

            Selection.objects = gameObjects;
        }

        #endregion

        #region ReferenceSelfComponentWindow


        static void ReferenceSelfComponentWindow()
        {
            inputString = EditorGUILayout.TextField("Ime klase: ", inputString);
            inputString2 = EditorGUILayout.TextField("Ime atributa: ", inputString2);
            inputString3 = EditorGUILayout.TextField("Tip koji se referencira: ", inputString3);

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            inputBool1 = EditorGUILayout.Toggle("Get component from parent", inputBool1, GUILayout.ExpandWidth(false),
                GUILayout.Height(60f));
            inputBool2 = EditorGUILayout.Toggle("Get component from self ", inputBool2, GUILayout.ExpandWidth(false),
                GUILayout.Height(60f));
            EditorGUILayout.EndHorizontal();
        }

        [MenuItem("GameObject/Talenzzo/In Test - Reference components - Select", false, 0)]
        static void ReferenceSelfComponentOption()
        {
            okClicked = OnReferenceSelfComponentWindowFinished;
            Show(EditorWindowState.ReferenceSelfComponentWindow);
        }


        static void OnReferenceSelfComponentWindowFinished()
        {
            GameObject[] gameObjects = GetSelectedGameObjects();
            foreach (GameObject gameObject in gameObjects)
            {
                // inputString - Ime klase, Symemtry, inputString2 - ime atributa, inputString3 - komponenta za referenciranje
                var myClassComponent = gameObject.GetComponent(inputString);
                if (!myClassComponent)
                {
                    Debug.Log("Ne postoji ta komponenta");
                    return;
                }

                var myType = myClassComponent.GetType();
                var targetField = myType.GetField(inputString2);
                // Self
                var componentForReferencing = gameObject.GetComponent(inputString3);

                if (inputBool1)
                {
                    // Get From Parent
                    componentForReferencing = gameObject.transform.parent.GetComponent(inputString3);
                }
                else if (inputBool2)
                {
                    // Get From Self
                }
                else
                {
                    return;
                }

                if (targetField == null)
                {
                    Debug.Log("Ne postoji taj atribut ili je private");
                    return;
                }

                if (targetField.FieldType.Name != componentForReferencing.GetType().Name)
                {
                    Debug.Log("Nisu isti tipovi");
                    return;
                }

                targetField.SetValue(myClassComponent, componentForReferencing);
                EditorUtility.SetDirty(gameObject);
                ApplyOverrides();
            }
        }

        #endregion
    
        #region CreateMatrixWindow
    
        static void CreateMatrixWindow()
        {
            EditorGUILayout.BeginHorizontal();
            inputInt = EditorGUILayout.IntField("Number of rows", inputInt);
            inputInt2 = EditorGUILayout.IntField("Number of columns", inputInt2);
            EditorGUILayout.EndHorizontal();
            inputFloat = EditorGUILayout.Slider("X osa", inputFloat, 0, 5);
            inputFloat2 = EditorGUILayout.Slider("Y osa", inputFloat2, 0, 5);
            CreateMatrixWindowUpdate(inputFloat, inputFloat2);
        }

        private static bool isCreateMatrixWindowActive;
        private static GameObject[] createMatrixWindowSelectedObjects;

        static void CreateMatrixWindowUpdate(float value, float value2)
        {
            value = (float) Math.Round(value, 3);
            value2 = (float) Math.Round(value2, 3);
            if (createMatrixWindowSelectedObjects == null || createMatrixWindowSelectedObjects.Length != inputInt * inputInt2)
            {
                return;
            }

        
            for (int i = 0; i < inputInt; i++)
            {
                var position = createMatrixWindowSelectedObjects[0].transform.localPosition;
                for (int j = 0; j < inputInt2; j++)
                {
                    if (j == 0)
                        continue;
                    int index = i * inputInt2 + j;
                    position.x = position.x + value;
                    var localPosition = createMatrixWindowSelectedObjects[index].transform.localPosition;
                    createMatrixWindowSelectedObjects[index].transform.localPosition = new Vector3(position.x, localPosition.y, localPosition.z);
                }
            }

            for (int j = 0; j < inputInt2; j++)
            {
                var position = createMatrixWindowSelectedObjects[0].transform.localPosition;
                for (int i = 0; i < inputInt; i++)
                {
                    if (i == 0)
                        continue;
                    int index = i * inputInt2 + j;
                    position.y = position.y + value2;
                    var localPosition = createMatrixWindowSelectedObjects[index].transform.localPosition;
                    createMatrixWindowSelectedObjects[index].transform.localPosition = new Vector3(localPosition.x, position.y, localPosition.z);
                }
            }
        }

        [MenuItem("Talenzzo/Set Distance Between GameObject - Matrix", false, 0)]
        static void CreateMatrixOption()
        {
            okClicked = OnCreateMatrixFinished;
            isCreateMatrixWindowActive = true;
            inputFloat = 0.2f;
            inputFloat2 = 0.2f;
            inputInt = 1;
            inputInt2 = 1;
            createMatrixWindowSelectedObjects = GetSelectedGameObjects();
            Show(EditorWindowState.CreateMatrixWindow);
        }

        static void OnCreateMatrixFinished()
        {
            isCreateMatrixWindowActive = false;
        }
    
        #endregion
    
        #region EditPositionListWindow
    
        static void EditPositionListWindow()
        {
            inputString = EditorGUILayout.TextField("Ime skripte", inputString);
            inputString2 = EditorGUILayout.TextField("Ime liste pozicija", inputString2);
            EditPositionListWindowUpdate();
        }

        [MenuItem("GameObject/Talenzzo/In Test - Edit Position List", false, 0)]
        static void EditPositionListOption()
        {
            okClicked = OnEditPositionListFinished;
            selectedMarkers = null;
            GameObject[] selectedGameObjects = GetSelectedGameObjects();
            if (selectedGameObjects == null || selectedGameObjects.Length != 1)
            {
                return;
            }

            inputGameObject = selectedGameObjects[0];
            Show(EditorWindowState.EditPositionListWindow);
        }

        static void OnEditPositionListFinished()
        {
            Debug.Log(inputString);
            inputGameObject = null;
        }

        private static ReorderableList _vector2List;
        private static List<GameObject> selectedMarkers;
        static void EditPositionListWindowUpdate()
        { 
            if (inputComponent == null)
            {
                // if (inputGameObject == null)
                //     return;
                inputComponent = inputGameObject.GetComponent(inputString);
                if (inputComponent == null)
                {
                    return;
                }
            }
            SerializedObject serializedObject = new SerializedObject(inputComponent);
            SerializedProperty myProperty = serializedObject.FindProperty(inputString2);

            if (myProperty == null)
            {
                return;
            }
    
            inputBool1 = EditorGUILayout.Foldout(inputBool1, "Vector2 List");
            if (inputBool1)
            {
                EditorGUIUtility.editingTextField = false;
                // Create a ReorderableList for the vector2List SerializedProperty
    
                _vector2List = new ReorderableList(serializedObject, myProperty, true, true, true, true);
    
                // Wrap the ReorderableList inside a scroll view
                inputVector2 = EditorGUILayout.BeginScrollView(inputVector2, GUILayout.Height(300));
        
                // Draw the ReorderableList
                _vector2List.drawHeaderCallback = (Rect rect) => {
                    EditorGUI.LabelField(rect, "Vector2 List");
                };

                _vector2List.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                    SerializedProperty element = _vector2List.serializedProperty.GetArrayElementAtIndex(index);
                    rect.y += 2;
                    rect.height = EditorGUIUtility.singleLineHeight;
                    EditorGUI.LabelField(rect, "Point " + index);
                    rect.x += 70;
                    rect.width -= 70;
                    EditorGUI.PropertyField(rect, element, GUIContent.none);
                };

                _vector2List.onAddCallback = (ReorderableList list) => {
                    list.serializedProperty.arraySize++;
                    list.index = list.serializedProperty.arraySize - 1;
                    SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(list.index);
                    element.vector2Value = Vector2.zero;
                };

                _vector2List.onRemoveCallback = (ReorderableList list) => {
                    if (EditorUtility.DisplayDialog("Delete Vector2", "Are you sure you want to delete this vector2?", "Yes", "No"))
                    {
                        ReorderableList.defaultBehaviours.DoRemoveButton(list);
                    }
                };

                _vector2List.DoLayoutList();
                EditorGUILayout.EndScrollView();
                EditorGUIUtility.editingTextField = true;
                serializedObject.Update();
                // Apply any changes made to the SerializedObject
                serializedObject.ApplyModifiedProperties();

                EditPositionListWindowCreateObjects(myProperty, inputGameObject.transform);
            }
    
        }

        static void EditPositionListWindowCreateObjects(SerializedProperty property, Transform parent)
        {
            if (selectedMarkers == null || selectedMarkers.Count == 0)
            {
                selectedMarkers = new List<GameObject>();
                for (int i = 0; i < property.arraySize; i++)
                {
                    SerializedProperty element = property.GetArrayElementAtIndex(i);
                    Vector2 v2element = element.vector2Value;
                    GameObject marker = new GameObject();
                    //Instantiate(marker, parent);
                    marker.name = "Marker" + i;
                    marker.transform.parent = parent;
                    marker.transform.localScale = new Vector3(0.05f, 0.05f, 1);
                    marker.transform.localPosition = new Vector3(v2element.x, v2element.y, 0);
                    SpriteRenderer renderer = marker.AddComponent<SpriteRenderer>();
                    renderer.sortingOrder = 30;
                    //renderer.sprite = 
                    EditorUtility.SetDirty(marker);
                    selectedMarkers.Add(marker);
                }
            }
        }


        #endregion
    
        #region ReferenceMatrixWindow


        private static int[,] integerMatrix;
        static void ReferenceMatrixWindow()
        {
            // - component validate
            inputString = EditorGUILayout.TextField("Ime skripte", inputString);
            inputString2 = EditorGUILayout.TextField("Ime liste pozicija", inputString2);
        
            if (inputComponent == null)
            {
                if (inputGameObject == null)
                    return;
                inputComponent = inputGameObject.GetComponent(inputString);
                if (inputComponent == null)
                {
                    return;
                }
            }
            SerializedObject serializedObject = new SerializedObject(inputComponent);
            SerializedProperty matrixIntArrayProperty = serializedObject.FindProperty(inputString2);

            if (matrixIntArrayProperty == null)
            {
                return;
            }
        
            //Matrix

            EditorGUILayout.BeginHorizontal();
            inputInt = EditorGUILayout.IntField("Number of rows", inputInt);
            inputInt2 = EditorGUILayout.IntField("Number of columns", inputInt2);
            EditorGUILayout.EndHorizontal();

            // Create matrix if valid
            if (inputInt <= 0 || inputInt2 <= 0)
            {
                return;
            }

            if (inputInt * inputInt2 != matrixIntArrayProperty.arraySize)
            {
                return;
            }

            //boolX = EditorGUILayout.Toggle("Open", boolX, GUILayout.ExpandWidth(false), GUILayout.Height(60f));
            if (integerMatrix == null || integerMatrix.GetLength(0) != inputInt || integerMatrix.GetLength(1) != inputInt2)
            {
                integerMatrix = new int[inputInt, inputInt2];
                if (matrixIntArrayProperty.arraySize != 0 && matrixIntArrayProperty.arraySize >= inputInt * inputInt2)
                {
                    for (int i = 0; i < integerMatrix.GetLength(0); i++)
                    {
                        for (int j = 0; j < integerMatrix.GetLength(1); j++)
                        {
                            integerMatrix[i, j] = matrixIntArrayProperty
                                .GetArrayElementAtIndex(i * integerMatrix.GetLength(1) + j).intValue;
                        }
                    }
                }
            }

            // Resize the serialized property if the matrix size has changed. NE RADI ZA SAD
            // TODO: handle resizing matrix
            // if (matrixIntArrayProperty.arraySize != integerMatrix.Length)
            // {
            //     matrixIntArrayProperty.arraySize = integerMatrix.Length;
            // }
        
            serializedObject.Update();
        
            GUILayout.Space(20);
        
            inputVector2 = EditorGUILayout.BeginScrollView(inputVector2, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        
            GUILayout.BeginHorizontal();
            GUILayout.Space(60);
            for (int j = 0; j < integerMatrix.GetLength(1); j++)
            {
                GUILayout.Label(j.ToString(), GUILayout.Width(50));
            }
            GUILayout.EndHorizontal();
        
            GUILayout.BeginVertical();
            for (int i = 0; i < integerMatrix.GetLength(0); i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(i.ToString(), GUILayout.Width(50));
                for (int j = 0; j < integerMatrix.GetLength(1); j++)
                {
                    integerMatrix[i, j] = EditorGUILayout.IntField(integerMatrix[i, j], GUILayout.Width(50));
                    // Update the serialized property value
                    int index = i * integerMatrix.GetLength(1) + j;
                    matrixIntArrayProperty.GetArrayElementAtIndex(index).intValue = integerMatrix[i, j];
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        
            serializedObject.ApplyModifiedProperties();
        
            ReferenceMatrixWindowUpdate();
        }

        [MenuItem("GameObject/Talenzzo/Edit Matrix - List<int>", false, 0)]
        static void ReferenceMatrixOption()
        {
            GameObject[] selectedGameObjects = GetSelectedGameObjects();
            if (selectedGameObjects == null || selectedGameObjects.Length != 1)
            {
                return;
            }

            inputGameObject = selectedGameObjects[0];
            okClicked = OnReferenceMatrixFinished;
            Show(EditorWindowState.ReferenceMatrixWindow);
        }

        static void OnReferenceMatrixFinished()
        {
            integerMatrix = null;
            Debug.Log(inputString);
        }

        static void ReferenceMatrixWindowUpdate()
        {
        
        }
    
        #endregion
        
        #region CreateGameObjectsWindow
        
        static void CreateGameObjectsWindow()
        {
            GUILayout.Label("Number of GameObjects to create");
            inputInt = EditorGUILayout.IntField(inputInt, GUILayout.Width(50));
            GUILayout.Label("Name of objects");
            inputString = EditorGUILayout.TextField(inputString, GUILayout.Width(250));
            
            
            CreateGameObjectsWindowUpdate();
        }

        [MenuItem("GameObject/Talenzzo/Create GameObjects", false, 0)]
        static void CreateGameObjectsOption()
        {
            okClicked = OnCreateGameObjectsFinished;
            Show(EditorWindowState.CreateGameObjectsWindow);
        }

        static void OnCreateGameObjectsFinished()
        {
            GameObject[] selectedGameObjects = GetSelectedGameObjects();
            if (selectedGameObjects.Length != 1)
            {
                Debug.LogError("Select parent object");
            }

            Transform parent = selectedGameObjects[0].transform;

            for (int i = 0; i < inputInt; i++)
            {
                GameObject gameObject = new GameObject();
                Undo.RegisterCreatedObjectUndo(gameObject, "Create New GameObject" + i);

                gameObject.transform.parent = parent;
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localRotation = Quaternion.identity;
                gameObject.transform.localScale = Vector3.one;
                gameObject.name = inputString;
                gameObject.layer = parent.gameObject.layer;
                EditorUtility.SetDirty(gameObject);
            }
            
            TacicEditorUtils.AddNumbersToChildsGameObjectName(parent);
        }

        static void CreateGameObjectsWindowUpdate()
        {
        
        }
        
        #endregion

        #endregion

        #region Editor Script Utillities

        private static GameObject[] GetSelectedGameObjects()
        {
            if (Selection.activeGameObject == null)
                return null;

            return Selection.gameObjects;
        }

        private static GameObject GetRoomsHolderOnScene()
        {
            return GameObject.Find("RoomsHolder");
        }

        private static void ApplyOverrides()
        {
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            PrefabUtility.ApplyPrefabInstance(GetRoomsHolderOnScene(), InteractionMode.AutomatedAction);
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }

        #endregion
    }
}
// TODO - Test Undo.RecordObject(gameObject, gameObject.name);  snima sve posle te naredbe. Ne radi sa Referenciranjem kao gore