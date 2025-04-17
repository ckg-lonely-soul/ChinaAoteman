using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackTarget : MonoBehaviour {

    public int Id;
    public Image cursor;
    public Canvas canvas;
    public Vector3 worldPos;
    public GameObject attackLight;
    public AudioSource attackSound;
    private bool soundIsPlaying;
    public bool canFire;

    [SerializeField] private float distanceFromCamera = 6f;

    void Update()
    {
        // 获取鼠标屏幕位置
        Vector3 mousePos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, cursor.transform.position);

        // 对于透视相机，使用固定距离
        mousePos.z = distanceFromCamera;

        // 转换到世界坐标
        worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // 更新物体位置
        transform.position = worldPos;

        if (attackLight.activeSelf)
        {
            if (!soundIsPlaying)
            {
                attackSound.Play();
                soundIsPlaying = true;
            }
        }
        else if (soundIsPlaying)
        {
            attackSound.Stop();
            soundIsPlaying = false;
        }
    }
}
