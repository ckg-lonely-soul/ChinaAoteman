using UnityEngine;
[ExecuteInEditMode]
public class SkyRoatate : MonoBehaviour
{
    public float RotateSpeed = 0.01f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RotateSky();
    }
    /// <summary>
    /// 天空盒旋转
    /// </summary>
    public void RotateSky()
    {
        //（前提 摄像机标签为MainCamera）
        float num = Camera.main.GetComponent<Skybox>().material.GetFloat("_Rotation");
        Camera.main.GetComponent<Skybox>().material.SetFloat("_Rotation", num + RotateSpeed);
    }
}

