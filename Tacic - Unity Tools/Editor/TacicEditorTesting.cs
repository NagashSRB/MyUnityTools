using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.Editor
{
    public class EditorTestComponent : MonoBehaviour
    {
        
    }
    
    public class TacicEditorTesting : EditorWindow
    {
        #region OldEditorScript
        
        [MenuItem("Talenzzo/Test/PrefabMode", false, 0)]
        static void PrefabModeTest()
        {
            //t1
            //bool isInPrefabMode = PrefabUtility.IsPartOfPrefabInstance(EditorUtility.GetActiveGameObject());
            
            
            //t2
            // GameObject rootGameObject = PrefabUtility.GetPrefabInstanceHandle(Selection.activeGameObject);
            // if (rootGameObject != null)
            // {
            //     string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(rootGameObject);
            //     Debug.Log("Prefab Mode is active. Prefab asset path: " + prefabPath);
            // }
            // else
            // {
            //     Debug.Log("Not in Prefab Mode.");
            // }
            
            //t3
            if (PrefabStageUtility.GetCurrentPrefabStage())
            {
                Debug.Log("nije null");
            }
            else
            {
                Debug.Log("null");
            }
            
            //t4
            PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();
            Debug.Log(stage.assetPath);
        }
        
        
        
        //[MenuItem("Title", false, 0)]
        static void DoSomething()
        {
            GameObject editorTestGameObject = null;
            //Find loaded objects of type - very slow
            //Gets a list of all loaded objects of Type type. Objects attached to inactive GameObjects are only included if inactiveObjects is set to true
            //public static T[] FindObjectsOfType(bool includeInactive);
            List<EditorTestComponent> list = GameObject.FindObjectsOfType<EditorTestComponent>().ToList();
            
            //Initialize new gameobject
            //Or just set it afterwards
            GameObject holders = new GameObject("Holders")
            {
                transform =
                {
                    parent = editorTestGameObject.transform.parent,
                    localPosition = Vector3.zero
                }
            };
        }
        
        // // Legacy - Pokusaj referenciranja iz inspectora ovog niza, ali to izgleda radi samo za runtime. 
        // // Da se isproba u runtimeu. Umesto toga, treba koristiti SerializedFields ili sl klase za zeljene rezultate
        // [MenuItem("GameObject/Talenzzo/ReferenceBoard", false, 0)]
        // static void ReferenceBoard()
        // {
        //     GameObject mg = GameObject.Find("SymmetryPuzzleMGMiniGame");
        //     var arr = mg.GetComponent<SymmetryPuzzleManager>();
        //     string input =
        //         "0000010000001110101111000001000000101011111000000101000010101010100100010001001010101111000001000000111011111000000100000010101110110000010001001010101110010001000100101010101000000120010";
        //
        //     for (int i = 0; i < input.Length; i++)
        //     {
        //         arr.board.Add(input[i] - '0');
        //     }
        //
        //     EditorUtility.SetDirty(arr);
        // }
        
        // // Legacy - Sortiranje objekata po redosledu selektovanja. Izgleda da unityeditor ne pamti redosled
        // // selektovanja, pa ovaj kod ne radi kako treba. Ostaviti istrazivanje za drugi put, da li postoji nacin
        // [MenuItem("Talenzzo/Promeni redosled objekata po redosledu selektovanja - Select", false, 0)]
        // static void SortBySelectionOrder()
        // {
        //     GameObject[] gameObjects = GetSelectedGameObjects();
        //     if (gameObjects.Length == 0 || !IsParentSameForObjects(gameObjects))
        //     {
        //         Debug.Log("Not sorted");
        //         return;
        //     }
        //
        //     Transform parent = gameObjects[0].transform.parent;
        //
        //     for (int i = 0; i < gameObjects.Length; i++)
        //     {
        //         gameObjects[i].transform.SetSiblingIndex(i);
        //     }
        // }
        #endregion
    }

    // Property Drawer -> Custom Inspector ([curestomeditor(typeof(Component))]); oninspectorgui
    // Editor Script -> Editor Window
    // Editor
    
    //EditorGUIUtility
    
    //editorguilayout ima kao param labelu,, guilayout i ne bas
    public class TacicTestEditorWindow : EditorWindow
    {
        [MenuItem("Talenzzo/Test/TestFunctionInTestScript", false, 0)]
        static void TestFunctionInTestScript()
        {
                        
            TacicTestEditorWindow testEditorWindow = CreateInstance<TacicTestEditorWindow>();
            testEditorWindow.minSize = new Vector2(600f, 500f);
            //Max ne mora
            testEditorWindow.ShowUtility();
            
            // // Ovakav window ima izgleda maximize, moze da se zakaci sa strane za unity. Mozda to nije lose ako 
            // // mogu npr i tabovi da se koriste pa da budue kao konstanto prisutan editor tool
            // TacicTestEditorWindow testEditorWindow = (TacicTestEditorWindow) GetWindow(typeof(TacicTestEditorWindow));
            // testEditorWindow.minSize = new Vector2(600f, 500f);
            // testEditorWindow.Show();
        }

        private Texture2D texture;
        private Rect textureRect;

        public enum MyEnum
        {
            Test1,Test2,Test3,Test4,Test5,Test6,Test7
        }
        public MyEnum myEnum = MyEnum.Test1;

        private void OnGUI()
        {
            //DrawLayouts(), custom function. Draw region basically.
            DrawLayouts();
            DrawHeader();

            // EditorGUILayout.BeginHorizontal();
            // EditorGUILayout.EndHorizontal();
            using (new EditorGUILayout.HorizontalScope())
            {
                myEnum = (MyEnum) EditorGUILayout.EnumPopup(myEnum);
                EditorGUILayout.Toggle(false);
            }
        }

        private void DrawHeader()
        {
            GUILayout.BeginArea(headerSection);
            
            GUILayout.Label("Text");
            //EditorGUILayout.LabelField();
            //razlika guilayout i editorguilayiou?  layout je wrapper za rectove valjda
            
            GUILayout.EndArea();
            
            //EditorGUILayout.BeginHorizontal();
        }

        private void DrawLayouts()
        {
            headerSection.x = 0;
            headerSection.y = 0;
            headerSection.width = Screen.width;
            headerSection.height = 50;       
            
            projectSection.x = 0;
            projectSection.y = 50;
            //projectSection.width = Screen.width / 3f;
            //projectSection.height = Screen.width - 50;            
            projectSection.width = 150;
            projectSection.height = 150;
            
            GUI.DrawTexture(headerSection, headerSectionTexture);
            GUI.DrawTexture(projectSection, projectTexture);
        }

        private Texture2D headerSectionTexture;
        private Rect headerSection;
        
        private Texture2D projectTexture;
        private Rect projectSection;

        private void OnEnable()
        {
            Debug.Log("Test editor window on enable");
            Color color = new Color(13f / 255f, 32f / 255f,  44f / 255f, 1f);
            headerSectionTexture = new Texture2D(1, 1);
            headerSectionTexture.SetPixel(0, 0, color);
            headerSectionTexture.Apply();


            projectTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/Levels/Level1/FlowerShop1.png");
            
        }


        // // Tacic Editor. Slicno i ja radim, makar deo za kreiranje i singleton
        // EditorGUILayout.Space();
        // EditorGUILayout.BeginVertical("box");
        // EditorGUILayout.Space();
        //     
        // //switch window state
        //     
        // EditorGUILayout.BeginHorizontal();
        // EditorGUILayout.Space();
        // EditorGUILayout.Space();
        // bool okButton = GUILayout.Button("OK", GUILayout.Width(140f));
        // bool cancelButton = GUILayout.Button("Cancel", GUILayout.Width(140f));
        // EditorGUILayout.EndHorizontal();
        // EditorGUILayout.EndVertical();
        //
        // if (okButton)
        // {
        //     //OkButtonClicked();
        // }
        //
        // if (cancelButton)
        // {
        //     //CancelButtonClicked();
        // }
            
        // Rooms and Exits - GameObjectMenu
        // if (Instance == null)
        // {
        //     GameObjectMenu setObjectNamePopup = new GameObjectMenu();
        //
        //     setObjectNamePopup.minSize = new Vector2(600f, 500f);
        //     setObjectNamePopup.maxSize = new Vector2(600f, 500f);
        //     setObjectNamePopup.titleContent.text = "Unesite ime objekta: ";
        //
        //     setObjectNamePopup.ShowUtility();
        // }

            
            
        //layouti
        
    }

}
