using UnityEditor;
using UnityEngine;

public class MeshColliderTool : EditorWindow
{

    [MenuItem("GunGameTool/Add MeshCollider to Children")]
    public static void ShowWindow()
    {
        GetWindow<MeshColliderTool>("MeshCollider Tool");
    }

    private GameObject targetObject; // 目标物体

    private void OnGUI()
    {
        GUILayout.Label("Add MeshCollider to Children", EditorStyles.boldLabel);

        // 选择目标物体
        targetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true);

        // 添加按钮
        if (GUILayout.Button("Add MeshCollider to Children"))
        {
            if (targetObject != null)
            {
                AddMeshCollidersToChildren(targetObject);
            }
            else
            {
                Debug.LogWarning("Please assign a target object.");
            }
        }
    }

    // 递归遍历子对象并添加 MeshCollider
    private void AddMeshCollidersToChildren(GameObject parent)
    {
        if (parent == null) return;

        // 遍历所有子对象
        foreach (Transform child in parent.transform)
        {
            // 如果子对象有 MeshFilter 但没有 MeshCollider
            MeshFilter meshFilter = child.GetComponent<MeshFilter>();
            if (meshFilter != null && child.GetComponent<MeshCollider>() == null)
            {
                // 添加 MeshCollider
                MeshCollider meshCollider = child.gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = meshFilter.sharedMesh;
                meshCollider.convex = false; // 或者 false，根据需求
                Debug.Log("Added MeshCollider to: " + child.name);
            }

            // 递归检查子对象的子对象
            AddMeshCollidersToChildren(child.gameObject);
        }
    }
}
