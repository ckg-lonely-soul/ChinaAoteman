using UnityEditor;
using UnityEngine;

public class MeshColliderTool : EditorWindow
{

    [MenuItem("GunGameTool/Add MeshCollider to Children")]
    public static void ShowWindow()
    {
        GetWindow<MeshColliderTool>("MeshCollider Tool");
    }

    private GameObject targetObject; // Ŀ������

    private void OnGUI()
    {
        GUILayout.Label("Add MeshCollider to Children", EditorStyles.boldLabel);

        // ѡ��Ŀ������
        targetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true);

        // ��Ӱ�ť
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

    // �ݹ�����Ӷ������ MeshCollider
    private void AddMeshCollidersToChildren(GameObject parent)
    {
        if (parent == null) return;

        // ���������Ӷ���
        foreach (Transform child in parent.transform)
        {
            // ����Ӷ����� MeshFilter ��û�� MeshCollider
            MeshFilter meshFilter = child.GetComponent<MeshFilter>();
            if (meshFilter != null && child.GetComponent<MeshCollider>() == null)
            {
                // ��� MeshCollider
                MeshCollider meshCollider = child.gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = meshFilter.sharedMesh;
                meshCollider.convex = false; // ���� false����������
                Debug.Log("Added MeshCollider to: " + child.name);
            }

            // �ݹ����Ӷ�����Ӷ���
            AddMeshCollidersToChildren(child.gameObject);
        }
    }
}
