using UnityEngine;

/// <summary>
/// ��̬������ս����y��λ��
/// </summary>
public class NearAttackPosController : MonoBehaviour
{
    public float[] yOffsets;
    public float localOriginY = 0f;

    /// <summary>
    /// ��ʼ��nearAttackPos��λ��
    /// </summary>
    public void InitOriginY()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, localOriginY, transform.localPosition.z);
    }
}
