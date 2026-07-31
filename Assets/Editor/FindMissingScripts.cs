using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class FindMissingScripts : EditorWindow
{
    [MenuItem("Tools/Find Missing Scripts In All Prefabs And Scenes")]
    static void Find()
    {
        int count = 0;

        // Check prefabs (only in Assets, not Packages)
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            Component[] components = prefab.GetComponentsInChildren<Component>(true);
            foreach (var c in components)
            {
                if (c == null)
                {
                    count++;
                    Debug.LogWarning($"[PREFAB] Missing script in: {path}", prefab);
                }
            }
        }

        // Check scenes (only in Assets, not Packages)
        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets" });
        foreach (string guid in sceneGuids)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);

            foreach (GameObject go in scene.GetRootGameObjects())
            {
                Component[] components = go.GetComponentsInChildren<Component>(true);
                foreach (var c in components)
                {
                    if (c == null)
                    {
                        count++;
                        Debug.LogWarning($"[SCENE: {scenePath}] Missing script on GameObject: {c}", go);
                    }
                }
            }

            EditorSceneManager.CloseScene(scene, true);
        }

        Debug.Log($"Scan complete. Total missing scripts found: {count}");
    }

    [MenuItem("Tools/Find Missing Scripts In ScriptableObjects")]
    static void FindInScriptableObjects()
    {
        int count = 0;
        string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets" });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            if (obj == null)
            {
                count++;
                Debug.LogWarning($"[SCRIPTABLEOBJECT] Missing/broken script at: {path}");
            }
        }
        Debug.Log($"ScriptableObject scan complete. Total broken: {count}");
    }
}