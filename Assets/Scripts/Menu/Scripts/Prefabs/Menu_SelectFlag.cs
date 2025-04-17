using UnityEngine;

public class Menu_SelectFlag : MonoBehaviour
{
    public GameObject left_Obj;
    public GameObject right_Obj;

    float radius;   // 半径
    float runTime;
    float posx;

    public void Init(float width)
    {
        radius = width / 2;
        runTime = 0;
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        runTime += Time.deltaTime * 6;
        posx = (Mathf.Sin(runTime) + 1) * 1.5f + 6;
        left_Obj.transform.localPosition = new Vector3(-radius - posx, 0);
        right_Obj.transform.localPosition = new Vector3(radius + posx, 0);
    }
}
