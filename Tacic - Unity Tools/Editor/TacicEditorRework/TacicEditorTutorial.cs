using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.Editor.TacicEditorRework
{
    public class TacicEditorTutorial : EditorWindow
    {
        private void Explanations()
        {
            // SELECTION:
            // null if not selected, if selected one object, that is the one; if selected multiple ???
            // Use for testing only if something is selected (!= null)
            GameObject activeGameObject = Selection.activeGameObject;
        
            // Use this for accessing selected object (or objects)
            GameObject[] activeGameObjects = Selection.gameObjects;
        
        
            // UNDO:
            // Record gameObject or component
            // parent change, add component and create new/destroy are done differently, with specific functions:
            Undo.RecordObject(activeGameObject, "UndoActionGroupName" + activeGameObject.name);
            activeGameObject.name += "something";
            
            //TODO: Add specific undo functions
            
            
            // FIND:
            // Find all gameObjects lower in hierarchy than variable GameObject, and include inactive if you pass true
            activeGameObject.GetComponentsInChildren<SpriteRenderer>(true);

            // Gets component of an object, works with active and inactive gameObjects
            activeGameObject.GetComponent<SpriteRenderer>();

            //Find gameobject on scene/prefab mod with that name (WORKS ONLY FOR ACTIVE OBJECTS)
            GameObject.Find("name");

            //???Find transform of all children, not only level below, but all levels with that provided name.
            activeGameObject.transform.Find("name");
            // You can do it like a path in hierarchy. 
            activeGameObject.transform.Find("sphere2/sphere3");
        }

    }
}