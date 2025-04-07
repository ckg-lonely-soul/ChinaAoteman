using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AlignChildrenToParent : MonoBehaviour
{
    [MenuItem("GunGameTool/Align Children to Parent", false, 10)]
    static void AlignAllChildren()
    {
        // ��ȡ��ǰѡ�еĶ���
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject != null)
        {
            // ��¼����������ı任״̬���Ա���Գ�������
            Undo.RecordObjects(selectedObject.GetComponentsInChildren<Transform>(), "Align All Children to Parent");

            // ��ȡ������������������ת
            Vector3 parentPosition = selectedObject.transform.position;
            Quaternion parentRotation = selectedObject.transform.rotation;

            // �ݹ��������������
            AlignChildrenRecursive(selectedObject.transform, parentPosition, parentRotation);

            Debug.Log("All children aligned to parent successfully.");
        }
        else
        {
            Debug.LogWarning("Please select a GameObject.");
        }
    }

    // �ݹ鷽�����������в㼶��������
    static void AlignChildrenRecursive(Transform parent, Vector3 position, Quaternion rotation)
    {
        // ��������������
        foreach (Transform child in parent)
        {
            // ����������������������ת
            child.position = position;
            child.rotation = rotation;

            // �ݹ鴦���������������
            AlignChildrenRecursive(child, position, rotation);
        }
    }
}
