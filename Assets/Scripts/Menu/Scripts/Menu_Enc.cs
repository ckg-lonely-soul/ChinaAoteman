using UnityEngine;
using UnityEngine.UI;

public class Menu_Enc : MonoBehaviour
{
    // 坐标
    const int MAX_BITS = 6;
    const int MAX_SET_SEL = 2;
    const int MAX_SEL = MAX_SET_SEL + 2;

    // 要显示的内容(中英文切换)
    public GameObject planeMenu;
    public Text text_Enc;
    public Text cursor_Selected;        //光标选中标志
    public GameObject image_cursor;     //光标
    public GameObject[] button;


    // 传入的变量
    Menu menu;

    public AccValue[] accValue;
    public NumEdit[] numEdit;
    //
    static int postIndex;               // 光标所在行
    static int bitIndex;                // 光标所在位(打码数字)
    static bool selectSta;              // 行光标选中状态
    static float selectBlenTime;        // 光标闪烁时间

    public void Awake0(Menu mmenu)
    {
        //
        menu = mmenu;

        Init();
    }
    // Use this for initialization
    void Init()
    {
        //
        numEdit[0].SetName("C1:");
        numEdit[1].SetName("C2:");
        //
        accValue[0].SetName("P1");
        accValue[1].SetName("P2");
        accValue[2].SetName("P3");
        accValue[3].SetName("P4");
        //accValue[4].SetName("P5");
    }

    // Update is called once per frame
    void Update()
    {

        if (Main.statue != en_MainStatue.Game_98 || Menu.statue != en_MenuStatue.MenuSta_Enc)
            return;
        //if(Game_Enc.machineNo < 0) {
        //    menu.ChangeStatue(en_MenuStatue.MenuSta_SysSet);
        //    return;
        //}
        //选中标志闪烁
        if (postIndex < MAX_SET_SEL && selectSta == true)
        {
            if (Time.time - selectBlenTime >= 0.3)
            {
                selectBlenTime = Time.time;
                if (cursor_Selected.enabled == true)
                    cursor_Selected.enabled = false;
                else
                    cursor_Selected.enabled = true;
            }
        }

        //按键
        /*		if (Key.MENU_UpPressed ()) {
                    if (selectSta == false) {
                        postIndex = (postIndex + MAX_SEL - 1) % MAX_SEL;
                        UpdateCursor ();
                    } else if(postIndex < MAX_SET_SEL) {
                        numEdit [postIndex].BitValueAdd (bitIndex);
                    }
                }
                if (Key.MENU_DownPressed ()) {
                    if (selectSta == false) {
                        postIndex = (postIndex + 1) % MAX_SEL;
                        UpdateCursor ();
                    } else if(postIndex < MAX_SET_SEL) {
                        numEdit [postIndex].BitValueDec (bitIndex);
                    }
                }*/
        /*
if (Key.MENU_LeftPressed () || Key.KEYFJ_Menu_LeftPressed()) {
    if (postIndex < MAX_SET_SEL && selectSta == true) {
        bitIndex = (bitIndex + MAX_BITS - 1) % MAX_BITS; 
        UpdateCursor ();
    }
}
if (Key.MENU_RightPressed () || Key.KEYFJ_Menu_RightPressed()) {
    if (postIndex < MAX_SET_SEL && selectSta == true) {
        bitIndex = (bitIndex + 1) % MAX_BITS; 
        UpdateCursor ();
    }
}
if (Key.MENU_OkPressed () || Key.KEYFJ_Menu_OkPressed()) {			
    if (postIndex < MAX_SET_SEL) {
        //设置参数
        if (selectSta == true) {
            UpdataCursor_Select (false);
        } else {
            bitIndex = 0;
            UpdataCursor_Select (true);
            UpdateCursor ();
        }
    }
    //三个按键
    else if(postIndex == MAX_SET_SEL + 0){
        //确定保存
        OnButton_EncOk_Pressed();
    }
    else if(postIndex == MAX_SET_SEL + 1){
        //返回
        OnButton_EncBack_Pressed();
    //	Application.LoadLevel ("Loading");
    }
}
*/
        if (Key.MENU_LeftPressed() || Key.KEYFJ_Menu_LeftPressed())
        {
            if (selectSta)
            {
                if (postIndex < MAX_SET_SEL)
                {
                    bitIndex = (bitIndex + 1) % MAX_BITS;
                    UpdateCursor();
                }
            }
            else
            {
                postIndex = (postIndex + MAX_SEL - 1) % MAX_SEL;
                UpdateCursor();
            }
        }
        if (Key.MENU_RightPressed() || Key.KEYFJ_Menu_RightPressed())
        {
            if (selectSta)
            {
                if (postIndex < MAX_SET_SEL)
                {
                    numEdit[postIndex].BitValueAdd(bitIndex);
                }
            }
            else
            {
                postIndex = (postIndex + 1) % MAX_SEL;
                UpdateCursor();
            }
        }
        if (Key.MENU_OkPressed() || Key.KEYFJ_Menu_OkPressed())
        {
            if (postIndex < MAX_SET_SEL)
            {
                //设置参数
                if (selectSta == true)
                {
                    UpdataCursor_Select(false);
                }
                else
                {
                    bitIndex = 0;
                    UpdataCursor_Select(true);
                    UpdateCursor();
                }
            }
            //三个按键
            else if (postIndex == MAX_SET_SEL + 0)
            {
                //确定保存
                OnButton_EncOk_Pressed();
            }
            else if (postIndex == MAX_SET_SEL + 1)
            {
                //返回
                OnButton_EncBack_Pressed();
            }
        }
    }

    // 更新选中标志
    void UpdataCursor_Select(bool sta)
    {
        cursor_Selected.enabled = sta;
        selectBlenTime = Time.time;
        selectSta = sta;
    }
    // 更新光标坐标和大小
    void UpdateCursor()
    {
        if (postIndex < MAX_SET_SEL)
        {
            //C1, C2
            cursor_Selected.enabled = selectSta;
            image_cursor.GetComponent<RectTransform>().sizeDelta = numEdit[postIndex].GetComponent<RectTransform>().sizeDelta;
            image_cursor.transform.localPosition = numEdit[postIndex].transform.localPosition;
            cursor_Selected.transform.localPosition = numEdit[postIndex].transform.localPosition + new Vector3(130 - 87 + bitIndex * 36, 8, 0);
        }
        else
        {
            cursor_Selected.enabled = false;
            //三个按键
            image_cursor.GetComponent<RectTransform>().sizeDelta = button[postIndex - MAX_SET_SEL].GetComponent<RectTransform>().sizeDelta;
            image_cursor.transform.localPosition = button[postIndex - MAX_SET_SEL].transform.localPosition;
        }
    }

    // 1.进入初始化
    public void GameStart()
    {
        UpdateLanguage();
        Game_Enc.Init();
        //
        numEdit[0].SetValue(0);
        numEdit[1].SetValue(0);
        //
        accValue[0].SetData(Game_Enc.MACHINE_NO);
        accValue[1].SetData(Game_Enc.GENC_Zyf);
        accValue[2].SetData(Game_Enc.GENC_Zjf);
        accValue[3].SetData(Game_Enc.GENC_JiaoYan);
        accValue[4].SetData(Game_Enc.mactime);
        //
        postIndex = 0;
        UpdataCursor_Select(false);
        UpdateCursor();
    }
    // 2.更新显示中英文
    public void UpdateLanguage()
    {
        if (Set.setVal.Language == (int)en_Language.Chinese)
        {
            //中文
            //报码
            text_Enc.text = "报码";
            accValue[4].SetName("剩余时间");
            button[0].GetComponentInChildren<Text>().text = "确认";
            button[1].GetComponentInChildren<Text>().text = "返回";
        }
        else
        {
            //英文
            //报码
            text_Enc.text = "Code";
            accValue[4].SetName("Remain Time");
            button[0].GetComponentInChildren<Text>().text = "OK";
            button[1].GetComponentInChildren<Text>().text = "Back";
        }
    }

    //按键 ------
    public void OnButton_EncOk_Pressed()
    {
        ushort[] code = new ushort[2];

        code[0] = (ushort)numEdit[0].GetValue();
        code[1] = (ushort)numEdit[1].GetValue();
        if (Game_Enc.SetCode(code))
        {
            accValue[4].SetData(Game_Enc.mactime);
            menu.ChangeStatue(en_MenuStatue.MenuSta_SysSet);
        }
    }
    public void OnButton_EncBack_Pressed()
    {
        if (Game_Enc.mactime > 0)
        {
            menu.ChangeStatue(en_MenuStatue.MenuSta_SysSet);
        }
    }
}
