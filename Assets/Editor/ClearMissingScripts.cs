using UnityEditor;
using UnityEngine;

public class ClearMissingScripts : MonoBehaviour {

    [MenuItem("GunGameTool/Clear Missing Scripts")]
    static void ClearMissingScriptsFromSelected()
    {
        // 获取当前选中的 GameObject
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("请先选中一个或多个 GameObject。");
            return;
        }

        int totalCleared = 0;

        // 遍历所有选中的 GameObject
        foreach (GameObject go in selectedObjects)
        {
            int cleared = ClearMissingScriptsFromGameObject(go);
            totalCleared += cleared;
            Debug.Log($"已从 {go.name} 中清除 {cleared} 个丢失的脚本。");
        }

        Debug.Log($"总共清除了 {totalCleared} 个丢失的脚本。");
    }

    static int ClearMissingScriptsFromGameObject(GameObject go)
    {
        int clearedCount = 0;

        // 使用 SerializedObject 来检查并移除丢失的脚本
        var components = go.GetComponents<Component>();
        var serializedObject = new SerializedObject(go);
        var prop = serializedObject.FindProperty("m_Component");

        // 遍历所有组件
        for (int i = components.Length - 1; i >= 0; i--)
        {
            if (components[i] == null) // 如果组件为 null，说明是丢失的脚本
            {
                prop.DeleteArrayElementAtIndex(i);
                clearedCount++;
            }
        }

        // 应用修改
        serializedObject.ApplyModifiedProperties();

        // 递归清除所有子物体上的丢失脚本
        foreach (Transform child in go.transform)
        {
            clearedCount += ClearMissingScriptsFromGameObject(child.gameObject);
        }

        return clearedCount;
    }
}
