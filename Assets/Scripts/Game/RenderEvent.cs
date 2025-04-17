using UnityEngine;

public class RenderEvent : MonoBehaviour
{
    public bool isRender = false;

    void OnWillRenderObject()
    {
        isRender = true;
    }

    public bool IsRender()
    {
        if (isRender)
        {
            isRender = false;
            return true;
        }
        return false;
    }
}
