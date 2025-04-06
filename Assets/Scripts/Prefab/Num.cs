using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum en_NumMode
{
    NUM_M_Middle = 0,
    NUM_M_Left,
    NUM_M_Right,
}

public class Num : MonoBehaviour
{
    const int NUM_SIZE = 10;
    // 
    public string spritePath;           //0-9 数字的路径
                                        // 显示格式:
    public en_NumMode mode = en_NumMode.NUM_M_Middle;
    public int minBitNum = 0;
    public bool sizeHandle = false;
    public float width;
    public bool sizeFollowSprite = false;
    public bool forUI = true;

    // 自用组件
    Sprite[] sprite_Num;
    Image[] image;
    SpriteRenderer[] spriteRenderer;
    //
    float numWidth;
    // 附加字符
    int boundsChar = -1;    // 没有

    // 重新加载数字图片(路径)
    public void ReloadSprites(string path)
    {
        sprite_Num = Resources.LoadAll<Sprite>(path);
        if (forUI == false)
        {
            numWidth = sprite_Num[0].texture.width / sprite_Num[0].pixelsPerUnit;
        }
    }

    public void SetOrderInLayer(int value)
    {
        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].sortingOrder = value;
        }
    }

    public void SetBoundsChar(int charid)
    {
        boundsChar = charid;
    }
    public float UpdateShow(int dat)
    {
        // 首次加载
        int len;
        // 加载数字图片
        if (sprite_Num == null)
        {
            sprite_Num = Resources.LoadAll<Sprite>(spritePath);
        }
        // 绑定组件 
        if (forUI)
        {
            // UI界面----
            if (image == null)
            {
                image = GetComponentsInChildren<Image>();
            }
            if (sizeFollowSprite)
            {
                for (int i = 0; i < image.Length; i++)
                {
                    image[i].rectTransform.sizeDelta = new Vector2(sprite_Num[0].texture.width, sprite_Num[0].texture.height);
                }
                numWidth = 100 * sprite_Num[0].texture.width / sprite_Num[0].pixelsPerUnit;
            }
            if (sizeHandle)
            {
                numWidth = width;
            }
            else
            {
                numWidth = image[0].rectTransform.sizeDelta.x;
            }
            // 开始 ---------
            len = 0;
            if (dat < 0)
                dat = 0;
            // 更新数字
            for (int i = 0; i < image.Length; i++)
            {
                if (dat > 0 || i == 0 || i < minBitNum)
                {
                    image[i].gameObject.SetActive(true);
                    image[i].sprite = sprite_Num[dat % 10];
                    dat /= 10;
                    len++;
                }
                else
                {
                    image[i].gameObject.SetActive(false);
                }
            }
            if (boundsChar >= 0 && boundsChar < sprite_Num.Length)
            {
                image[len].gameObject.SetActive(true);
                image[len].sprite = sprite_Num[boundsChar];
                len++;
            }
        }
        else
        {
            // 精灵贴图----
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
                numWidth = sprite_Num[0].texture.width / sprite_Num[0].pixelsPerUnit;
            }
            // 开始 ---------
            len = 0;
            if (dat < 0)
                dat = 0;
            // 更新数字
            for (int i = 0; i < spriteRenderer.Length; i++)
            {
                if (dat > 0 || i == 0 || i < minBitNum)
                {
                    spriteRenderer[i].gameObject.SetActive(true);
                    spriteRenderer[i].sprite = sprite_Num[dat % 10];
                    dat /= 10;
                    len++;
                }
                else
                {
                    spriteRenderer[i].gameObject.SetActive(false);
                }
            }
            if (boundsChar >= 0 && boundsChar < sprite_Num.Length)
            {
                spriteRenderer[len].gameObject.SetActive(true);
                spriteRenderer[len].sprite = sprite_Num[boundsChar];
                len++;
            }
        }
        // 更新坐标 
        float start_x = 0;
        switch (mode)
        {
            case en_NumMode.NUM_M_Left:
                start_x = (len - 1) * numWidth;// - numWidth / 2;
                break;
            case en_NumMode.NUM_M_Middle:
                start_x = (len - 1) * numWidth / 2;
                break;
            case en_NumMode.NUM_M_Right:
                start_x = 0;    //-numWidth / 2;
                break;
        }
        for (int i = 0; i < len; i++)
        {
            if (forUI)
            {
                image[i].transform.localPosition = new Vector3(start_x - i * numWidth, 0f, 0f);
            }
            else
            {
                spriteRenderer[i].transform.localPosition = new Vector3(start_x - i * numWidth, 0f, 0f);
            }
        }
        return numWidth * len;
    }
}
