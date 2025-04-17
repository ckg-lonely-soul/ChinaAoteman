using UnityEngine;
public class Game97_NextPage : MonoBehaviour
{
    public RectTransform gameSels;
    public void OnClick()
    {
        float x = gameSels.anchoredPosition.x;
        gameSels.anchoredPosition = new Vector2(x - 300, 0);
    }
}
