using UnityEngine;

/// <summary>
/// 动态调整近战攻击y的位置
/// </summary>
public class NearAttackPosController : MonoBehaviour
{
    public float[] yOffsets;
    public float localOriginY = 0f;

    /// <summary>
    /// 初始化nearAttackPos的位置
    /// </summary>
    public void InitOriginY()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, localOriginY, transform.localPosition.z);
    }
}
