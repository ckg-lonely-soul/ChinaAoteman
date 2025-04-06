using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour 
{
    public static PoolManager instance;//单例
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }
    //总对象池，可以存储多个物体池子。键是物体名字，值是物体池子，这里用栈实现
    public Dictionary<string, Stack<GameObject>> poolDic = new Dictionary<string, Stack<GameObject>>();
    
    public GameObject GetObj(GameObject prefab)//要用物体时拿物体池子中的物体
    {
        GameObject obj;
        //如果有这个物体池子并且物体池子里有已经生成出来的物体，就直接获取该物体，然后显示
        if (poolDic.ContainsKey(prefab.name) && poolDic[prefab.name].Count > 0)
        {
            obj = poolDic[prefab.name].Pop();
            if(obj == null)
            {
                obj = Instantiate(prefab);
                obj.name = prefab.name;
            }
            obj.SetActive(true);
        }
        else//没有就生成一个物体来获取，因为生成出来的物体名字会有（clone），和预制体名字不一致，所以要改变名字
        {
            obj = Instantiate(prefab);
            obj.name = prefab.name;
        }
        return obj;
    }

    public void PushObj(GameObject obj)//将物体给回对象池的该物体池子
    {
        obj.SetActive(false);//不显示
        if(!poolDic.ContainsKey(obj.name))//如果没有这个物体的物体池子，就创建一个物体池子加入到总对象池
        {
            poolDic.Add(obj.name, new Stack<GameObject>());
        }
        poolDic[obj.name].Push(obj);//将用完的物体给回物体池子
    }

    public void PoolClear()//清空总物体池，切换场景时用，防止内存泄漏和一些错误
    {
        poolDic.Clear();
    }
}
