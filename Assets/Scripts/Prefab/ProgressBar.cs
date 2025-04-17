using UnityEngine;

public enum en_ProgressBar_Start
{
    Left = 0,
    Right,
}
public class ProgressBar : MonoBehaviour
{
    public GameObject rogressValue_Obj;
    public en_ProgressBar_Start startPos = en_ProgressBar_Start.Left;

    float width;

    void Awake()
    {
        width = rogressValue_Obj.GetComponent<RectTransform>().sizeDelta.x;
    }

    public void SetProgress(float value)
    {
        if (value >= 0 && value <= 1)
        {
            rogressValue_Obj.transform.localScale = new Vector3(value, 1, 1);
            if (startPos == en_ProgressBar_Start.Left)
            {
                rogressValue_Obj.transform.localPosition = new Vector3(-width * (1 - value) / 2, 0, 0);
            }
            else if (startPos == en_ProgressBar_Start.Right)
            {
                rogressValue_Obj.transform.localPosition = new Vector3(width * (1 - value) / 2, 0, 0);
            }
        }
    }
}
