using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[Serializable]
public class SceneObjectData
{
    public string name;
    public string path; // 唯一路径标识（如 "Root/Child/GrandChild"）
    public Vector3 position; // 世界坐标
    public Quaternion rotation; // 世界旋转
    public Vector3 localScale; // 局部缩放
    public List<ComponentData> components = new List<ComponentData>(); // 组件数据
    public List<SceneObjectData> children = new List<SceneObjectData>(); // 子物体
}

[Serializable]
public class ComponentData
{
    public string type; // 组件简单名称（如 "MyScript"）
    public Dictionary<string, string> fields = new Dictionary<string, string>(); // 组件的字段数据
}

public class SceneDataImporter : EditorWindow
{
    [MenuItem("GunGameTool/导入场景数据")]
    static void ImportSceneData()
    {
        string path = EditorUtility.OpenFilePanel("Load Scene Data", "", "json");
        if (string.IsNullOrEmpty(path)) return;

        string json = File.ReadAllText(path);
        Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);
        Dictionary<string, GameObject> pathToObject = new Dictionary<string, GameObject>();

        // 第一遍：创建所有物体（无父子关系）
        foreach (SceneObjectData objData in wrapper.data)
        {
            CreateObject(objData, pathToObject);
        }

        // 第二遍：设置父子关系并挂载组件
        foreach (SceneObjectData objData in wrapper.data)
        {
            SetParentAndComponents(objData, pathToObject);
        }

        Debug.Log("场景数据导入完成！");
    }

    static void CreateObject(SceneObjectData objData, Dictionary<string, GameObject> pathToObject)
    {
        GameObject obj = new GameObject(objData.name);
        obj.transform.position = objData.position; // 设置世界坐标
        obj.transform.rotation = objData.rotation; // 设置世界旋转
        obj.transform.localScale = objData.localScale; // 设置局部缩放
        pathToObject[objData.path] = obj;

        // 递归创建子物体
        foreach (SceneObjectData childData in objData.children)
        {
            CreateObject(childData, pathToObject);
        }
    }

    static void SetParentAndComponents(SceneObjectData objData, Dictionary<string, GameObject> pathToObject)
    {
        GameObject obj = pathToObject[objData.path];
        int lastSlash = objData.path.LastIndexOf('/');
        if (lastSlash > 0)
        {
            string parentPath = objData.path.Substring(0, lastSlash);
            GameObject parent;
            if (pathToObject.TryGetValue(parentPath, out parent))
            {
                obj.transform.SetParent(parent.transform, true); // 保持子物体的相对坐标
            }
        }

        // 挂载组件并设置参数
        foreach (ComponentData compData in objData.components)
        {
            // 查找匹配的组件类型
            Type type = FindTypeByName(compData.type);
            if (type == null)
            {
                Debug.LogWarning($"未找到组件类型：{compData.type}");
                continue;
            }

            // 挂载组件
            Component comp = obj.AddComponent(type);

            // 设置组件字段值
            var fields = type.GetFields(
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance
            );

            foreach (var field in fields)
            {
                string value;
                if (compData.fields.TryGetValue(field.Name, out value))
                {
                    if (field.FieldType.IsPrimitive || field.FieldType == typeof(string))
                    {
                        field.SetValue(comp, Convert.ChangeType(value, field.FieldType));
                    }
                    else if (field.FieldType == typeof(Vector3) || field.FieldType == typeof(Quaternion))
                    {
                        field.SetValue(comp, JsonUtility.FromJson(value, field.FieldType));
                    }
                }
            }
        }

        // 递归处理子物体
        foreach (SceneObjectData childData in objData.children)
        {
            SetParentAndComponents(childData, pathToObject);
        }
    }

    static Type FindTypeByName(string typeName)
    {
        // 遍历所有已加载的程序集，查找匹配的类型
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type type = assembly.GetTypes().FirstOrDefault(t => t.Name == typeName);
            if (type != null)
            {
                return type;
            }
        }
        return null;
    }

    [Serializable]
    private class Wrapper { public List<SceneObjectData> data; }
}
