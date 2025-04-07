using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AlignChildrenToParent : MonoBehaviour
{
    [MenuItem("GunGameTool/Align Children to Parent", false, 10)]
    static void AlignAllChildren()
    {
        // 获取当前选中的对象
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject != null)
        {
            // 记录所有子物体的变换状态，以便可以撤销操作
            Undo.RecordObjects(selectedObject.GetComponentsInChildren<Transform>(), "Align All Children to Parent");

            // 获取父物体的世界坐标和旋转
            Vector3 parentPosition = selectedObject.transform.position;
            Quaternion parentRotation = selectedObject.transform.rotation;

            // 递归对齐所有子物体
            AlignChildrenRecursive(selectedObject.transform, parentPosition, parentRotation);

            Debug.Log("All children aligned to parent successfully.");
        }
        else
        {
            Debug.LogWarning("Please select a GameObject.");
        }
    }

    // 递归方法：对齐所有层级的子物体
    static void AlignChildrenRecursive(Transform parent, Vector3 position, Quaternion rotation)
    {
        // 遍历所有子物体
        foreach (Transform child in parent)
        {
            // 设置子物体的世界坐标和旋转
            child.position = position;
            child.rotation = rotation;

            // 递归处理子物体的子物体
            AlignChildrenRecursive(child, position, rotation);
        }
    }
}
