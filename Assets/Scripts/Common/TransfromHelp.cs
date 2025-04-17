using UnityEngine;



/// <summary>
/// transFrom类助手
/// </summary>
public static class TransfromHelp
{
    /// <summary>
    /// 获取子物体
    /// </summary>
    /// <param name="tf"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
    public static Transform GetChild(this Transform tf, string childName)
    {
        Transform trans = tf.Find(childName);
        if (trans != null)
            return trans;
        for (int i = 0; i < tf.childCount; i++)
        {

            if (trans != null)
            {
                return trans;
            }
            trans = GetChild(tf.GetChild(i), childName);
        }
        return trans;
    }
    /// <summary>
    /// 设置物体的激活状态
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tf"></param>
    /// <param name="active"></param>
    public static void SetActive<T>(this T tf, bool active) where T : Component
    {
        if (tf == null)
            return;
        tf.gameObject.SetActive(active);
    }
    /// <summary>
    /// 获取或者添加组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tf"></param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this Transform tf) where T : Component
    {
        T component = tf.GetComponent<T>();
        if (component == null)
        {
            component = tf.gameObject.AddComponent<T>();
        }
        return component;
    }

    public static bool AnimatorIsPlayEnd(this Animator anim, string name, float value = 0.99f)
    {
        AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateinfo.IsName(name) && (stateinfo.normalizedTime > value))
        {
            return true;
        }
        return false;
    }
}
