using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Game97_GameSelOne : MonoBehaviour
{
    public Image image_Pic;
    public Image image_GameName;
    public Image image_Pass;
    public Image image_NoSelectBox;

    public int scenceIndex;
    public int arrayIndex;

    void Start()
    {
        image_GameName.transform.localPosition = new Vector3(0, -80f, 0);
    }
    public void Update_Statue(bool ispassed)
    {
        if (ispassed)
        {
            image_Pic.color = new Color(0.4f, 0.4f, 0.4f, 1);
            image_Pass.gameObject.SetActive(true);
        }
        else
        {
            image_Pic.color = new Color(1f, 1f, 1f, 1);
            image_Pass.gameObject.SetActive(false);
        }
    }
    public void Update_GamePic(Sprite sprite)
    {
        image_Pic.sprite = sprite;
    }

    public void Update_GameName(Sprite sprite)
    {
        image_GameName.sprite = sprite;
        image_GameName.SetNativeSize();
    }
}
