using UnityEngine;

public class RemoveObj : MonoBehaviour
{
    public float destoryTime;//多久之后不显示
    void OnEnable()
    {
        Invoke("Remove", destoryTime);
    }

    void Remove()
    {
        PoolManager.instance.PushObj(gameObject);
    }
}
