#if UNITY_EDITOR

using Fries.Interior_01.Utility;
using UnityEditor;
using UnityEngine;

namespace Fries.Interior_01.Setup {
    public static class Setup {
        
        [MenuItem("Tools/Fries/Interior_01 Setup Built-in Rendering Pipeline")]
        public static void setup() {
            // 创建 Yurei Initializer
            GameObject yureiManager = GameObject.Find("Yurei Manager");
            if (yureiManager == null) {
                yureiManager = new GameObject("Yurei Manager");
                yureiManager.AddComponent<YureiManagerBRP>();
            }
            
            if (!yureiManager.GetComponent<YureiManagerBRP>()) 
                Debug.LogError("You have an invalid Yurei Manager in the scene. Please delete it and try again.");
            
            // 清除当前的选择
            Selection.activeGameObject = null;
            // 设置当前对象为选中状态
            Selection.activeGameObject = yureiManager;
            // 也可以将视图聚焦到该对象上
            EditorGUIUtility.PingObject(yureiManager);
        }
        
        [MenuItem("Tools/Fries/Interior_01 Setup Universal Rendering Pipeline")]
        public static void setup1() {
            // 创建 Yurei Initializer
            GameObject yureiManager = GameObject.Find("Yurei Manager");
            if (yureiManager == null) {
                yureiManager = new GameObject("Yurei Manager");
                yureiManager.AddComponent<YureiManagerURP>();
            }
            
            if (!yureiManager.GetComponent<YureiManagerURP>()) 
                Debug.LogError("You have an invalid Yurei Manager in the scene. Please delete it and try again.");
            
            // 清除当前的选择
            Selection.activeGameObject = null;
            // 设置当前对象为选中状态
            Selection.activeGameObject = yureiManager;
            // 也可以将视图聚焦到该对象上
            EditorGUIUtility.PingObject(yureiManager);
        }
    }
}

#endif

namespace Fries.Interior_01.Setup {
    public static class Config {
        public static readonly int DefaultLayer = 18;
    }
}