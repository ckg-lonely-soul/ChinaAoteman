using UnityEngine;
using UnityEngine.UI;


public enum en_TextMode
{
    Middle = 0,
    Left,
    Right,
}

public class StringText : MonoBehaviour
{

    public GameObject image_Prefab;
    public string path_CHAR;
    public string path_Num;
    public int Length = 6;
    // 显示格式:
    public en_TextMode mode = en_TextMode.Left;
    public bool sizeHandle = false;
    public float width;
    public bool sizeFollowSprite = false;
    public bool forUI = true;

    //
    GameObject[] image_Obj;

    Sprite[] sprite_CHAR;       // 字母（大写）
    Sprite[] sprite_Num;        // 数字
    //
    Image[] image;
    SpriteRenderer[] spriteRenderer;
    //
    float numWidth;


    // 
    void Awake()
    {
        // 圆形字母图片
        sprite_CHAR = Resources.LoadAll<Sprite>(path_CHAR);
        // 加载数字图片
        sprite_Num = Resources.LoadAll<Sprite>(path_Num);
        // 生成图片
        if (Length > 0)
        {
            image_Obj = new GameObject[Length];
            for (int i = 0; i < image_Obj.Length; i++)
            {
                image_Obj[i] = GameObject.Instantiate(image_Prefab, transform);
            }
            Destroy(image_Prefab);
            // 绑定组件 
            if (forUI)
            {
                // UI界面----
                image = new Image[Length];
                for (int i = 0; i < Length; i++)
                {
                    image[i] = image_Obj[i].GetComponent<Image>();
                }
                if (sizeHandle)
                {
                    numWidth = width;
                }
                else if (sizeFollowSprite)
                {
                    numWidth = 100 * sprite_Num[0].texture.width / sprite_Num[0].pixelsPerUnit;
                }
                else
                {
                    numWidth = image_Obj[0].GetComponent<RectTransform>().sizeDelta.x;
                }
            }
            else
            {
                // 精妙贴图----
                spriteRenderer = new SpriteRenderer[Length];
                for (int i = 0; i < Length; i++)
                {
                    spriteRenderer[i] = image_Obj[i].GetComponent<SpriteRenderer>();
                }
                numWidth = sprite_Num[0].texture.width / sprite_Num[0].pixelsPerUnit;
            }
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //
    public void UpdateText(string str)
    {
        //
        char[] chars;
        int len = 0;
        int i;

        chars = str.ToCharArray();
        if (forUI)
        {
            // UI界面----
            for (i = 0; i < str.Length && i < Length; i++)
            {
                if (chars[i] >= 'A' && chars[i] <= 'Z')
                {
                    image[len++].sprite = sprite_CHAR[chars[i] - 'A'];
                }
                else if (chars[i] >= '0' && chars[i] <= '9')
                {
                    image[len++].sprite = sprite_Num[chars[i] - '0'];
                }
            }
        }
        else
        {
            // 精灵贴图----
            for (i = 0; i < str.Length && i < Length; i++)
            {
                if (chars[i] >= 'A' && chars[i] <= 'Z')
                {
                    spriteRenderer[len++].sprite = sprite_CHAR[chars[i] - 'A'];
                }
                else if (chars[i] >= '0' && chars[i] <= '9')
                {
                    spriteRenderer[len++].sprite = sprite_Num[chars[i] - '0'];
                }
            }
        }
        //
        for (i = 0; i < image_Obj.Length; i++)
        {
            if (i < len)
            {
                image_Obj[i].SetActive(true);
            }
            else
            {
                image_Obj[i].SetActive(false);
            }
        }
        // 更新坐标 
        float start_x = 0;
        switch (mode)
        {
            case en_TextMode.Left:
                start_x = 0;
                break;
            case en_TextMode.Middle:
                start_x = (1 - len) * numWidth / 2;
                break;
            case en_TextMode.Right:
                start_x = (1 - len) * numWidth;
                break;
        }
        for (i = 0; i < len; i++)
        {
            image_Obj[i].transform.localPosition = new Vector3(start_x + i * numWidth, 0f, 0f);
        }
    }
}
