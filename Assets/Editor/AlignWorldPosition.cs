using UnityEditor;
using UnityEngine;

public class AlignWorldPosition : EditorWindow
{
    private GameObject _targetObject; // 拖入的目标物体

    [MenuItem("GunGameTool/对齐坐标工具")]
    public static void ShowWindow()
    {
        // 打开自定义窗口
        GetWindow<AlignWorldPosition>("对齐坐标工具");
    }

    private void OnGUI()
    {
        GUILayout.Label("对齐坐标工具", EditorStyles.boldLabel);

        // 拖拽目标物体
        _targetObject = (GameObject)EditorGUILayout.ObjectField(
            "拖入目标物体",
            _targetObject,
            typeof(GameObject),
            true);

        // 对齐按钮
        if (GUILayout.Button("对齐选中物体到目标"))
        {
            AlignSelectedToTarget();
        }
    }

    private void AlignSelectedToTarget()
    {
        if (_targetObject == null)
        {
            Debug.LogWarning("请先拖入一个目标物体！");
            return;
        }

        // 获取当前选中的物体
        GameObject[] selectedObjects = Selection.gameObjects;
        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("请先选中一个或多个物体！");
            return;
        }

        // 对齐选中物体到目标物体
        foreach (GameObject source in selectedObjects)
        {
            Undo.RecordObject(source.transform, "Align Position");
            source.transform.position = _targetObject.transform.position;
            Debug.Log($"已将 {source.name} 对齐到目标 {_targetObject.name}");
        }
    }
}