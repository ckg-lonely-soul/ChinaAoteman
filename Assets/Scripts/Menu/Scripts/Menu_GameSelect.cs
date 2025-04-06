using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_GameSelect : MonoBehaviour
{
    const int MAX_SET_ONEPAGE = 24;
    //int MAX_ALL_SET_SEL = Main.tab_GameId.Length;
    int MAX_SET_SEL = 20;
    int MAX_SEL = 8;// = MAX_SET_SEL + 3;

    // 要显示的内容(中英文切换)    
    public Text text_GameSet;
    public GameObject setVal_Layer;
    public Menu_SelectFlag selectFlag;  //光标选中标志
    public Menu_Button[] button;
    public GameObject setVal_Prefab;
    // 设置项    
    SetValue[] setVal = new SetValue[Main.tab_GameId.Length];
    //
    static int postIndex;
    static bool selectSta;

    // 传入的变量
    Menu menu;
    Menu_PasswordManager passwordManager;
    Menu_Tips menuTips;
    //
    public void Awake0(Menu mmenu)
    {
        //
        menu = mmenu;
        passwordManager = menu.passwordManager;
        menuTips = menu.menuTips;
        //
        GameObject obj;
        for (int i = 0; i < Main.tab_GameId.Length; i++)
        {
            obj = Instantiate(setVal_Prefab, setVal_Layer.transform);
            setVal[i] = obj.GetComponent<SetValue>();
        }
        SetVal_Init();
        // UpdateSetValPos();
    }

    void Update()
    {
        if (Menu.statue != en_MenuStatue.MenuSta_GameSelect)
            return;

        if (menuTips.gameObject.activeSelf)
            return;
        // 等待确认提示
        if (menuTips.waitTips)
        {
            menuTips.waitTips = false;
            if (menuTips.tipsType == en_MenuTipsType.Select)
            {
                if (postIndex == MAX_SET_SEL + 0)
                {
                    //保存并退出                
                    if (menuTips.result)
                    {
                        OnButton_SetSave_Pressed();
                        OnButton_SetBack_Pressed();
                        if (Set.setVal.Language == (int)en_Language.Chinese)
                        {
                            menuTips.Init(en_MenuTipsType.Tips, "保存成功", 3);
                        }
                        else
                        {
                            menuTips.Init(en_MenuTipsType.Tips, "Saved successfully", 3);
                        }
                        return;
                    }
                }
                else if (postIndex == MAX_SET_SEL + 1)
                {
                    //默认值
                    if (menuTips.result)
                    {
                        OnButton_SetDefault_Pressed();
                        //OnButton_SetBack_Pressed();                        
                        if (Set.setVal.Language == (int)en_Language.Chinese)
                        {
                            menuTips.Init(en_MenuTipsType.Tips, "恢复默认值成功", 3);
                        }
                        else
                        {
                            menuTips.Init(en_MenuTipsType.Tips, "Restore default successfully", 3);
                        }
                        return;
                    }
                }
                else if (postIndex == MAX_SET_SEL + 2)
                {
                    // 不保存退出
                    if (menuTips.result)
                    {
                        OnButton_SetBack_Pressed();
                        return;
                    }
                }
            }
        }



        /*	if (Key.MENU_UpPressed ()) {
                if (selectSta == false) {
                    if (postIndex >= MAX_SET_SEL) {
                        postIndex = MAX_SET_SEL - 1;
                    } else {
                        postIndex = (postIndex + MAX_SEL - 1) % MAX_SEL;
                        UpdateSetValPos();
                    }
                    UpdateCursor ();
                }
            }
            if (Key.MENU_DownPressed ()) {
                if (selectSta == false) {
                    postIndex = (postIndex + 1) % MAX_SEL;
                    UpdateSetValPos();
                    UpdateCursor ();
                }
            } */
        if (Key.MENU_LeftPressed() || Key.KEYFJ_Menu_LeftPressed())
        {
            if (selectSta == true)
            {
                if (postIndex < MAX_SET_SEL)
                {
                    GameSetVal_Left();
                }
            }
            else
            {
                postIndex = (postIndex + MAX_SEL - 1) % MAX_SEL;
                UpdateSetValPos();
                UpdateCursor();
                //	if (postIndex > MAX_SET_SEL) {
                //		postIndex--;
                //		UpdateSetValPos();
                //		UpdateCursor ();
                //	}
            }
        }
        if (Key.MENU_RightPressed() || Key.KEYFJ_Menu_RightPressed())
        {
            if (selectSta == true)
            {
                if (postIndex < MAX_SET_SEL)
                {
                    GameSetVal_Right();
                }
            }
            else
            {
                postIndex = (postIndex + 1) % MAX_SEL;
                UpdateSetValPos();
                UpdateCursor();
                //	if (postIndex >= MAX_SET_SEL) {
                //		postIndex = (postIndex + 1) % MAX_SEL;
                //		UpdateSetValPos();
                //		UpdateCursor ();
                //	}
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
                    UpdataCursor_Select(true);
                }
            }
            //三个按键
            else if (postIndex == MAX_SET_SEL + 0)
            {
                //保存并退出
                if (Set.setVal.Language == (int)en_Language.Chinese)
                {
                    menuTips.Init(en_MenuTipsType.Select, "是否保存并退出?", 0);
                }
                else
                {
                    menuTips.Init(en_MenuTipsType.Select, "Save and exit?", 0);
                }
            }
            else if (postIndex == MAX_SET_SEL + 1)
            {
                //默认值
                if (Set.setVal.Language == (int)en_Language.Chinese)
                {
                    menuTips.Init(en_MenuTipsType.Select, "是否恢复默认值?", 0);
                }
                else
                {
                    menuTips.Init(en_MenuTipsType.Select, "Restore defaults?", 0);
                }
            }
            else if (postIndex == MAX_SET_SEL + 2)
            {
                // 不保存退出
                if (Set.setVal.Language == (int)en_Language.Chinese)
                {
                    menuTips.Init(en_MenuTipsType.Select, "是否不保存退出?", 0);
                }
                else
                {
                    menuTips.Init(en_MenuTipsType.Select, "Exit without saving?", 0);
                }
            }
        }
    }

    // 更新选中标志
    void UpdataCursor_Select(bool sta)
    {
        selectSta = sta;
        selectFlag.gameObject.SetActive(sta);
        if (postIndex < setVal.Length)
        {
            selectFlag.transform.position = setVal[postIndex].image_ValueBackG.transform.position;
            selectFlag.Init(setVal[postIndex].image_ValueBackG.rectTransform.sizeDelta.x);
        }
    }
    // 更新光标坐标和大小
    void UpdateCursor()
    {
        //设置项
        for (int i = 0; i < setVal.Length; i++)
        {
            if (i == postIndex)
            {
                setVal[i].SetSelect(true);
            }
            else
            {
                setVal[i].SetSelect(false);
            }
        }
        //三个按键
        for (int i = 0; i < button.Length; i++)
        {
            if (i + MAX_SET_SEL == postIndex)
            {
                button[i].SetSelect(true);
            }
            else
            {
                button[i].SetSelect(false);
            }
        }
    }
    //
    void UpdateSetValPos()
    {
        int id = postIndex;
        if (id >= MAX_SET_SEL)
            id = MAX_SET_SEL - 1;
        int page = id / MAX_SET_ONEPAGE;
        int pindex;

        for (int i = 0; i < MAX_SET_SEL; i++)
        {
            if ((i / MAX_SET_ONEPAGE) == page)
            {
                setVal[i].gameObject.SetActive(true);
                //setVal[i].transform.localPosition = new Vector3(0, 220 - (i % MAX_SET_ONEPAGE) * 70, 0);
                pindex = i % MAX_SET_ONEPAGE;
                setVal[i].transform.localPosition = new Vector3((pindex % 3) * 340 - 340, 190 - (pindex / 3) * 60, 0);
            }
            else
            {
                setVal[i].gameObject.SetActive(false);
            }
        }
    }

    // 1.进入初始化 : 放到中英文切换前
    public void GameStart()
    {
        MAX_SET_SEL = Main.tab_GameId.Length;
        MAX_SEL = MAX_SET_SEL + 3;

        UpdateLanguage();
        UpdataGameSet();
        UpdateOrder();
        postIndex = MAX_SEL - 1;    //返回        
        UpdataCursor_Select(false);
        UpdateCursor();
    }
    // 2.更新显示中英文
    public void UpdateLanguage()
    {
        if (Set.setVal.Language == (int)en_Language.Chinese)
        {
            //中文
            //通用
            text_GameSet.text = "游戏选择";
            button[0].GetComponentInChildren<Text>().text = "保存并退出";
            button[1].GetComponentInChildren<Text>().text = "恢复默认值";
            button[2].GetComponentInChildren<Text>().text = "不保存退出";
        }
        else
        {
            //英文
            //通用
            text_GameSet.text = "Game Select";
            button[0].GetComponentInChildren<Text>().text = "Save Back";
            button[1].GetComponentInChildren<Text>().text = "Default";
            button[2].GetComponentInChildren<Text>().text = "Not Save Back";
        }
        SetVal_UpdataLanguage();
    }


    public void OnButton_SetDefault_Pressed()
    {
        Set.DefaultGameSelect();
        GameStart();
    }
    public void OnButton_SetBack_Pressed()
    {
        menu.ChangeStatue(en_MenuStatue.MenuSta_SysSet);
    }


    // 右键设置++
    void GameSetVal_Right()
    {
        if (postIndex < setVal.Length)
        {
            setVal[postIndex].ValueAdd();
        }
    }

    // 左键设置--
    void GameSetVal_Left()
    {
        if (postIndex < setVal.Length)
        {
            setVal[postIndex].ValueDec();
        }
    }

    // 需要修改的部分 --------------------------------------------------------------------------------------------------------------------------------------------
    //
    void SetVal_Init()
    {
        //** 1.设置项初始化(代替在界面里直接真写) : 初始化全部可选择列表
        for (int i = 0; i < setVal.Length; i++)
        {
            setVal[i].SetValueInit(Set.SET_c_GameSelect);
        }
    }
    // 排序， 不同模式
    void UpdateOrder()
    {
        //MAX_SET_SEL = MAX_ALL_SET_SEL;
        //MAX_SEL = MAX_SET_SEL + 3;
        // 关闭所有选项
        for (int i = 0; i < setVal.Length; i++)
        {
            setVal[i].gameObject.SetActive(false);
        }
        // 重新排列
        UpdateSetValPos();
    }

    string[] tab_GameName_CN = {
        "冰雪机械",     // 00
        "异域战士",     // 01
        "冰封魔族",     // 02
        "动物森林",     // 03
        "军事基地",     // 04
        "未来街道",     // 05
        "枪手突围",     // 06
        "城市突击",     // 07
        "魔方世界",     // 08
        "西部危机",     // 09
        "丛林探索",     // 10
        "魔幻天空",     // 11
        "突击街道",     // 12
        "荒野小妖",     // 13
        "机关迷宫",     // 14
        "埃及传说",     // 15
        "魔兽争霸",     // 16
        "疯狂动物城",   // 17
        "黑暗领域",     // 18
        "皇室领地",     // 19
        "天空城堡",     // 20
        "太空站",       // 21
    };
    string[] tab_GameName04_CN = {
        "冰雪朋克",    // 00     
        "外域勇士",    // 01
        "冰封魔族",    // 02
        "动物之森",    // 03
        "军事堡垒",    // 04
        "未来街道",    // 05
        "士兵突击",    // 06
        "城市突击",    // 07
        "我的世界",    // 08
        "西部枪手",    // 09
        "丛林探索",    // 10
        "魔幻天空",    // 11
        "街道突围",    // 12
        "荒野小妖",    // 13
        "机关迷宫",    // 14
        "埃及传说",    // 15
        "魔兽争霸",    // 16
        "疯狂动物城",  // 17,
        "黑暗领域",    // 18
        "皇室领地",    // 19
        "天空之城",    // 20
    };
    string[] tab_GameName05_CN = {
        "冰雪朋克",    // 00
        "外域勇士",    // 01
        "冰川魔域",    // 02
        "动物之森",    // 03
        "军事堡垒",    // 04
        "空中街道",    // 05
        "枪手突击",    // 06
        "城市突围",    // 07
        "我的世界",    // 08
        "西部枪手",    // 09
        "森林探索",    // 10
        "魔幻世界",    // 11
        "街道突围",    // 12
        "精灵小妖",    // 13
        "突围迷宫",    // 14
        "埃及机关",    // 15
        "魔兽争霸",    // 16
        "动物乐园",    // 17
        "未知领域",    // 18
        "皇者领地",    // 19
        "天空之城",    // 20
    };
    string[] tab_GameName01YT_CN = {//海燕枪神5+1一体机
        "冰雪朋克",    // 00
        "外域勇士",    // 01
        "冰川魔域",    // 02
        "动物之森",    // 03
        "军事堡垒",    // 04
        "空中街道",    // 05
        "枪手突击",    // 06
        "城市突围",    // 07
        "我的世界",    // 08
        "西部枪手",    // 09
        "森林探索",    // 10
        "魔幻世界",    // 11
        "街道突围",    // 12
        "精灵小妖",    // 13
        "突围迷宫",    // 14
        "埃及机关",    // 15
        "魔兽争霸",    // 16
        "动物乐园",    // 17
        "未知领域",    // 18
        "皇者领地",    // 19
        "天空之城",    // 20
        "MP5",         // 96
    };
    // 2.更新显示中英文
    void SetVal_UpdataLanguage()
    {
        if (Set.setVal.Language == (int)en_Language.Chinese)
        {
            // 要显示的设置项 名称
            string[] tab_GameName;
            if (Main.COMPANY_NUM == 1)//枪神
            {
#if IO_LOCAL
                tab_GameName = tab_GameName01YT_CN;
#else
                tab_GameName = tab_GameName_CN;
#endif
            }
            else if (Main.COMPANY_NUM == 4)
            {
                tab_GameName = tab_GameName04_CN;
            }
            else if (Main.COMPANY_NUM == 5)
            {
                tab_GameName = tab_GameName05_CN;
            }
            else
            {
                tab_GameName = tab_GameName_CN;
            }
            // 设置项值: 
            for (int i = 0; i < setVal.Length; i++)
            {
                if (i < tab_GameName.Length)
                {
#if IO_LOCAL
                    if(Main.COMPANY_NUM == 1)
                    {
                        if(Main.tab_GameId[i] == 96)//mp5
                        {
                            setVal[i].SetSelectName(tab_GameName[21]); //(i + 1).ToString("D2") + "." + 
                        }
                        else
                        {
                            setVal[i].SetSelectName(tab_GameName[Main.tab_GameId[i]]); //(i + 1).ToString("D2") + "." + 
                        }
                    }
#else
                    setVal[i].SetSelectName(tab_GameName[Main.tab_GameId[i]]); //(i + 1).ToString("D2") + "." + 
#endif
                }
                setVal[i].SetValueName(0, "关");
                setVal[i].SetValueName(1, "开");
            }
        }
        else
        {
            // 英文
            for (int i = 0; i < setVal.Length; i++)
            {
#if IO_LOCAL
                if(Main.COMPANY_NUM == 1)
                {
                    if(i == setVal.Length - 1)//最后一个是mp5
                    {
                        setVal[i].SetSelectName("MP5");
                    }
                    else
                    {
                        setVal[i].SetSelectName("Game " + (i + 1));
                    }
                }
#else
                setVal[i].SetSelectName("Game " + (i + 1));
#endif
            }
            // 设置项值: 
            for (int i = 0; i < setVal.Length; i++)
            {
                setVal[i].SetValueName(0, "OFF");
                setVal[i].SetValueName(1, "ON");
            }
        }
    }
    // 更新设置内容 :
    void UpdataGameSet()
    {
        for (int i = 0; i < setVal.Length; i++)
        {
            setVal[i].UpdateValue(Set.GameSelect[i]);
        }
    }
    bool IsCloseAllGame()
    {
        for (int i = 0; i < Main.tab_GameId.Length; i++)
        {
            if (Set.GameSelect[i] != 0)
            {
                return false;
            }
        }
        return true;
    }
    //按键 ------
    public void OnButton_SetSave_Pressed()
    {

        for (int i = 0; i < setVal.Length; i++)
        {
            Set.GameSelect[i] = setVal[i].GetValue();
            //Debug.Log("GameSelect_" + i + ":" + Set.GameSelect[i]);
        }
        if (IsCloseAllGame())
        {
            if (Set.setVal.Language == (int)en_Language.Chinese)
            {
                menuTips.Init(en_MenuTipsType.Tips, "全部游戏已关闭，将打开默认游戏", 2);
            }
            else
            {
                menuTips.Init(en_MenuTipsType.Tips, "All games are closed. Default game will be opened", 2);
            }
            Set.GameSelect[0] = 1;
            Set.GameSelect[1] = 1;
        }
        Set.SaveGameSelect();
    }
}
