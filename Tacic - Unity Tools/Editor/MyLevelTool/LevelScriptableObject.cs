using UnityEditor;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.Editor.MyLevelTool
{
    public class ScriptableObjectCreator
    {
        private string path;
        public T CreateScriptableObject<T>(string scriptableObjectPath) where T : ScriptableObject
        {
            path = scriptableObjectPath;
            T scriptableObject = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(scriptableObject, path);
            return scriptableObject;
        }
        
        public T LoadScriptableObject<T>() where T : ScriptableObject
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
    }
    
    
    
    public class LevelScriptableObject : ScriptableObject
    {
        public string name;
        public bool hideSettings;
    }
}