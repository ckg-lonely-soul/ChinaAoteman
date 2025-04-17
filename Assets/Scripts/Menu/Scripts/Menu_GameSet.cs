using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_GameSet : MonoBehaviour
{
    const int MAX_SET_ONEPAGE = 12;
    int MAX_SET_SEL;// = 12;
    int MAX_SEL;// = MAX_SET_SEL + 3;

    // 要显示的内容(中英文切换)    
    public Text text_GameSet;
    public Text text_TiShi;
    public Text text_DescriptTitle;
    public Text text_Descript;
    public GameObject setVal_Layer;
    public Menu_SelectFlag selectFlag;  //光标选中标志
    public Menu_Button[] button;
    public GameObject setVal_Prefab;
    // 设置项	
    SetValue[] setVal = new SetValue[MAX_ALL_SET_SEL];
    List<SetValue> list_SetVal = new List<SetValue>();
    //
    static int postIndex;
    static bool selectSta;
    float runTime_SaveText;

    //public static int SET_ID_StartCoins_sta;

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
        for (int i = 0; i < MAX_ALL_SET_SEL; i++)
        {
            obj = Instantiate(setVal_Prefab, setVal_Layer.transform);
            setVal[i] = obj.GetComponent<SetValue>();
        }
        SetVal_Init();
    }

    void Update()
    {
        if (Menu.statue != en_MenuStatue.MenuSta_GameSet)
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
                    if (CanChange() == false)
                        return;
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
        if (postIndex < list_SetVal.Count)
        {
            selectFlag.transform.position = list_SetVal[postIndex].image_ValueBackG.transform.position;
            selectFlag.Init(list_SetVal[postIndex].image_ValueBackG.rectTransform.sizeDelta.x);
        }
    }

    // 更新光标坐标和大小
    void UpdateCursor()
    {
        //设置项
        for (int i = 0; i < list_SetVal.Count; i++)
        {
            if (i == postIndex)
            {
                list_SetVal[i].SetSelect(true);
            }
            else
            {
                list_SetVal[i].SetSelect(false);
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
        UpdateDescript();
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
                list_SetVal[i].gameObject.SetActive(true);
                pindex = i % MAX_SET_ONEPAGE;
                list_SetVal[i].transform.localPosition = new Vector3((pindex % 3) * 300 - 300, 180 - (pindex / 3) * 70, 0);
            }
            else
            {
                list_SetVal[i].gameObject.SetActive(false);
            }
        }
    }


    // 1.进入初始化 : 放到中英文切换前
    int oldOutMode;
    public void GameStart()
    {
        oldOutMode = Set.setVal.OutMode;
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
            text_GameSet.text = "游戏设置";
            text_TiShi.text = "温馨提示:修改奖励模式后,请清零账目!";
            button[0].GetComponentInChildren<Text>().text = "保存并退出";
            button[1].GetComponentInChildren<Text>().text = "恢复默认值";
            button[2].GetComponentInChildren<Text>().text = "不保存退出";
        }
        else
        {
            //英文
            //通用
            text_GameSet.text = "Game Set";
            text_TiShi.text = "Tips: please clear the account after modifying the Out Mode!";
            button[0].GetComponentInChildren<Text>().text = "Save Back";
            button[1].GetComponentInChildren<Text>().text = "Default";
            button[2].GetComponentInChildren<Text>().text = "Not Save Back";
        }
        SetVal_UpdataLanguage();
    }


    public void OnButton_SetDefault_Pressed()
    {
        Set.Default();
        GameStart();
    }
    public void OnButton_SetBack_Pressed()
    {
        menu.ChangeStatue(en_MenuStatue.MenuSta_SysSet);
    }


    // 右键设置++
    void GameSetVal_Right()
    {
        if (postIndex < list_SetVal.Count)
        {
            if (CanChange() == false)
                return;
            list_SetVal[postIndex].ValueAdd();
            UpdateDescript();
            ModeCheck();
        }
    }

    // 左键设置--
    void GameSetVal_Left()
    {
        if (postIndex < list_SetVal.Count)
        {
            if (CanChange() == false)
                return;
            list_SetVal[postIndex].ValueDec();
            UpdateDescript();
            ModeCheck();
        }
    }

    bool CanChange()
    {
        return true;
    }

    // 需要修改的部分 --------------------------------------------------------------------------------------------------------------------------------------------
    // 设置项名字
    const int SET_ID_GameMode = 0;
    const int SET_ID_PlayerMode = 1;
    const int SET_ID_GunMode = 2;
    const int SET_ID_InOutMode = 3;
    const int SET_ID_AttackPower = 4;
    //	
    const int SET_ID_OutMode = 5;
    const int SET_ID_StartCoins = 6;
    const int SET_ID_TicketBl = 7;
    const int SET_ID_GiftBl = 8;
    const int SET_ID_GameTime = 9;
    const int SET_ID_ScoreTtl = 10;
    //
    const int SET_ID_TicketsOneCoin = 11;
    const int SET_ID_EditerOneCoin = 12;
    const int SET_ID_MinTickets = 13;
    const int SET_ID_OutDcTime = 14;
    //
    const int SET_ID_DeskMusic = 15;
    const int SET_ID_MainSoundVolume = 16;
    const int SET_ID_Gun1ShakePower = 17;
    const int SET_ID_Gun1ShakeTime = 18;
    const int SET_ID_Gun2ShakePower = 19;
    const int SET_ID_Gun2ShakeTime = 20;
    const int SET_ID_Gun3ShakePower = 21;
    const int SET_ID_Gun3ShakeTime = 22;
    const int SET_ID_Gun4ShakePower = 23;
    const int SET_ID_Gun4ShakeTime = 24;
    const int SET_ID_ShowQrCode = 25;
    const int SET_ID_MP5GunShake = 26;
    const int SET_ID_ChildrenSong = 27;
    const int MAX_ALL_SET_SEL = 28;


    // 娱乐模式ID列表
    int[] tab_IdBuf_PlayerOne = {
        SET_ID_PlayerMode,
        SET_ID_AttackPower,
#if !GUN_HW || UNITY_EDITOR
        SET_ID_GunMode,
#endif
        SET_ID_OutMode,
        SET_ID_StartCoins,
        SET_ID_TicketBl,
        SET_ID_GiftBl,
        SET_ID_GameTime,
        SET_ID_OutDcTime,
        SET_ID_DeskMusic,
#if IO_LOCAL
        SET_ID_ShowQrCode,
        SET_ID_MP5GunShake,
        SET_ID_ChildrenSong,
#endif
#if SHAKE_POWER
        SET_ID_Gun1ShakePower,
        SET_ID_Gun1ShakeTime,
        SET_ID_Gun2ShakePower,
        SET_ID_Gun2ShakeTime,
        SET_ID_Gun3ShakePower,
        SET_ID_Gun3ShakeTime,
        SET_ID_Gun4ShakePower,
        SET_ID_Gun4ShakeTime,
#endif
    };
    int[] tab_IdBuf_PlayerTwo = {
        SET_ID_PlayerMode,
        SET_ID_AttackPower,
        SET_ID_InOutMode,
#if !GUN_HW || UNITY_EDITOR
        SET_ID_GunMode,
#endif
        SET_ID_OutMode,
        SET_ID_StartCoins,
        SET_ID_TicketBl,
        SET_ID_GiftBl,
        SET_ID_GameTime,
        SET_ID_OutDcTime,
        SET_ID_DeskMusic,
#if IO_LOCAL
        SET_ID_ShowQrCode,
        SET_ID_MP5GunShake,
        SET_ID_ChildrenSong,
#endif
#if SHAKE_POWER
        SET_ID_Gun1ShakePower,
        SET_ID_Gun1ShakeTime,
        SET_ID_Gun2ShakePower,
        SET_ID_Gun2ShakeTime,
        SET_ID_Gun3ShakePower,
        SET_ID_Gun3ShakeTime,
        SET_ID_Gun4ShakePower,
        SET_ID_Gun4ShakeTime,
#endif
    };
    //
    void SetVal_Init()
    {
        //** 1.设置项初始化(代替在界面里直接真写) : 初始化全部可选择列表
        //setVal[SET_ID_InOutMode].SetValueInit(Set.SET_c_InOutMode);
        setVal[SET_ID_GameMode].SetValueInit(Set.SET_c_GameMode);
        setVal[SET_ID_InOutMode].SetValueInit(Set.SET_c_InOutMode);
        setVal[SET_ID_AttackPower].SetValueInit(Set.SET_c_AttackPower);
        setVal[SET_ID_Gun1ShakePower].SetValueInit(Set.SET_c_ShakePower);
        setVal[SET_ID_Gun1ShakeTime].SetValueInit(Set.SET_c_ShakeTime);
        setVal[SET_ID_Gun2ShakePower].SetValueInit(Set.SET_c_ShakePower);
        setVal[SET_ID_Gun2ShakeTime].SetValueInit(Set.SET_c_ShakeTime);
        setVal[SET_ID_Gun3ShakePower].SetValueInit(Set.SET_c_ShakePower);
        setVal[SET_ID_Gun3ShakeTime].SetValueInit(Set.SET_c_ShakeTime);
        setVal[SET_ID_Gun4ShakePower].SetValueInit(Set.SET_c_ShakePower);
        setVal[SET_ID_Gun4ShakeTime].SetValueInit(Set.SET_c_ShakeTime);
        setVal[SET_ID_GunMode].SetValueInit(Set.SET_c_GunMode);
        setVal[SET_ID_PlayerMode].SetValueInit(Set.SET_c_PlayerMode);
        //
        setVal[SET_ID_OutMode].SetValueInit(Set.SET_c_OutMode);
        setVal[SET_ID_TicketBl].SetValueInit(Set.SET_c_TicketBl);
        setVal[SET_ID_GiftBl].SetValueInit(Set.SET_c_GiftBl);
        setVal[SET_ID_StartCoins].SetValueInit(Set.SET_c_StartCoins);
        setVal[SET_ID_GameTime].SetValueInit(Set.SET_c_GameTime);
        //
        setVal[SET_ID_ScoreTtl].SetValueInit(Set.SET_c_ScoreTtl);
        setVal[SET_ID_DeskMusic].SetValueInit(Set.SET_c_DeskMusic);
        setVal[SET_ID_MainSoundVolume].SetValueInit(Set.SET_c_MainSoundVolume);
        //
        setVal[SET_ID_TicketsOneCoin].SetValueInit(Set.SET_c_TicketsOneCoin);
        setVal[SET_ID_EditerOneCoin].SetValueInit(Set.SET_c_EditerOneCoin);
        setVal[SET_ID_MinTickets].SetValueInit(Set.SET_c_MinTickets);
        setVal[SET_ID_OutDcTime].SetValueInit(Set.SET_c_OutDcTime);
        setVal[SET_ID_ShowQrCode].SetValueInit(Set.SET_c_ShowQrCode);
        setVal[SET_ID_MP5GunShake].SetValueInit(Set.SET_c_MP5GunShake);
        setVal[SET_ID_ChildrenSong].SetValueInit(Set.SET_c_ChildrenSong);
    }
    // 模式检测，当模式发生变化时，设置项目排列不同
    void ModeCheck()
    {
        if (list_SetVal[postIndex] == setVal[SET_ID_PlayerMode])
        {
            UpdateOrder();
        }
    }
    // 排序， 不同模式
    void UpdateOrder()
    {
        int[] tab_IdBuf;

        if (setVal[SET_ID_PlayerMode].GetValue() == (int)en_PlayerMode.One)
        {
            // 娱乐模式
            tab_IdBuf = tab_IdBuf_PlayerOne;
        }
        else
        {
            // 中性模式 
            tab_IdBuf = tab_IdBuf_PlayerTwo;
        }

        MAX_SET_SEL = tab_IdBuf.Length;
        MAX_SEL = MAX_SET_SEL + 3;
        // 当前要操作的选项
        list_SetVal.Clear();
        for (int i = 0; i < tab_IdBuf.Length; i++)
        {
            list_SetVal.Add(setVal[tab_IdBuf[i]]);
        }
        // 关闭所有选项
        for (int i = 0; i < setVal.Length; i++)
        {
            setVal[i].gameObject.SetActive(false);
        }
        // 重新排列
        UpdateSetValPos();
    }
    // 2.更新显示中英文
    void SetVal_UpdataLanguage()
    {
        if (Set.setVal.Language == (int)en_Language.Chinese)
        {
            // 要显示的设置项 名称
            setVal[SET_ID_GameMode].SetSelectName("游戏模式");

            setVal[SET_ID_InOutMode].SetSelectName("投退模式");
            setVal[SET_ID_AttackPower].SetSelectName("攻击威力");
            setVal[SET_ID_Gun1ShakePower].SetSelectName("枪1震动力度");
            setVal[SET_ID_Gun1ShakeTime].SetSelectName("枪1震动时间");
            setVal[SET_ID_Gun2ShakePower].SetSelectName("枪2震动力度");
            setVal[SET_ID_Gun2ShakeTime].SetSelectName("枪2震动时间");
            setVal[SET_ID_Gun3ShakePower].SetSelectName("枪3震动力度");
            setVal[SET_ID_Gun3ShakeTime].SetSelectName("枪3震动时间");
            setVal[SET_ID_Gun4ShakePower].SetSelectName("枪4震动力度");
            setVal[SET_ID_Gun4ShakeTime].SetSelectName("枪4震动时间");
            setVal[SET_ID_GunMode].SetSelectName("枪模式");
            setVal[SET_ID_PlayerMode].SetSelectName("玩家模式");
            // 娱乐
            setVal[SET_ID_OutMode].SetSelectName("奖励模式");
            setVal[SET_ID_StartCoins].SetSelectName("几币一玩");
            setVal[SET_ID_TicketBl].SetSelectName("彩票比率");
            setVal[SET_ID_GiftBl].SetSelectName("扭蛋比率");
            setVal[SET_ID_GameTime].SetSelectName("游戏时间");
            setVal[SET_ID_ScoreTtl].SetSelectName("是否积累");
            setVal[SET_ID_DeskMusic].SetSelectName("待机音乐");
            setVal[SET_ID_ShowQrCode].SetSelectName("显示二维码");
            setVal[SET_ID_MP5GunShake].SetSelectName("MP5枪震动");
            setVal[SET_ID_ChildrenSong].SetSelectName("播放儿歌");

            // 中性模式
            setVal[SET_ID_TicketsOneCoin].SetSelectName("1币兑票数");
            setVal[SET_ID_EditerOneCoin].SetSelectName("1币分数");
            setVal[SET_ID_MinTickets].SetSelectName("安慰票数");
            setVal[SET_ID_OutDcTime].SetSelectName("退检测时间");
            // 共用
            setVal[SET_ID_MainSoundVolume].SetSelectName("游戏音量");

            // 设置项值: 
            // 游戏模式
            setVal[SET_ID_GameMode].SetValueName(0, "娱乐模式");
            setVal[SET_ID_GameMode].SetValueName(1, "中性模式");
            // 投退模式
            setVal[SET_ID_InOutMode].SetValueName(0, "双投双退");
            setVal[SET_ID_InOutMode].SetValueName(1, "双投单退");
            setVal[SET_ID_InOutMode].SetValueName(2, "单投单退");

            // 枪模式
            setVal[SET_ID_GunMode].SetValueName(0, "枪");
            setVal[SET_ID_GunMode].SetValueName(1, "射珠");
            setVal[SET_ID_GunMode].SetValueName(2, "射水");

            // 玩家模式
            setVal[SET_ID_PlayerMode].SetValueName(0, "单人");
            setVal[SET_ID_PlayerMode].SetValueName(1, "双人");

            // 奖励模式
            setVal[SET_ID_OutMode].SetValueName(0, "无奖励");
            setVal[SET_ID_OutMode].SetValueName(1, "退彩票");
            setVal[SET_ID_OutMode].SetValueName(2, "退扭蛋");
            //
            setVal[SET_ID_ScoreTtl].SetValueName(0, "否");
            setVal[SET_ID_ScoreTtl].SetValueName(1, "是");
            //
            setVal[SET_ID_DeskMusic].SetValueName(0, "关");
            setVal[SET_ID_DeskMusic].SetValueName(1, "开");
            //
            setVal[SET_ID_ShowQrCode].SetValueName(0, "关");
            setVal[SET_ID_ShowQrCode].SetValueName(1, "开");
            //
            setVal[SET_ID_MP5GunShake].SetValueName(0, "关");
            setVal[SET_ID_MP5GunShake].SetValueName(1, "开");
            //
            setVal[SET_ID_ChildrenSong].SetValueName(0, "否");
            setVal[SET_ID_ChildrenSong].SetValueName(1, "是");
        }
        else
        {
            // 要显示的设置项 名称
            setVal[SET_ID_GameMode].SetSelectName("Game Mode");
            setVal[SET_ID_InOutMode].SetSelectName("Input And Output Mode");
            setVal[SET_ID_AttackPower].SetSelectName("Attack Power");
            setVal[SET_ID_Gun1ShakePower].SetSelectName("Gun 1 Shake Power");
            setVal[SET_ID_Gun1ShakeTime].SetSelectName("Gun 1 Shake Time");
            setVal[SET_ID_Gun2ShakePower].SetSelectName("Gun 2 Shake Power");
            setVal[SET_ID_Gun2ShakeTime].SetSelectName("Gun 2 Shake Time");
            setVal[SET_ID_Gun3ShakePower].SetSelectName("Gun 3 Shake Power");
            setVal[SET_ID_Gun3ShakeTime].SetSelectName("Gun 3 Shake Time");
            setVal[SET_ID_Gun4ShakePower].SetSelectName("Gun 4 Shake Power");
            setVal[SET_ID_Gun4ShakeTime].SetSelectName("Gun 4 Shake Time");
            setVal[SET_ID_GunMode].SetSelectName("Gun Mode");
            setVal[SET_ID_PlayerMode].SetSelectName("Player Mode");
            setVal[SET_ID_StartCoins].SetSelectName("Number Of Coins Play Once");
            setVal[SET_ID_GameTime].SetSelectName("Game Time");
            setVal[SET_ID_ScoreTtl].SetSelectName("Wheher The Cumulative");
            setVal[SET_ID_DeskMusic].SetSelectName("Dest Music");
            setVal[SET_ID_ShowQrCode].SetSelectName("Show QR-Code");
            setVal[SET_ID_MP5GunShake].SetSelectName("MP5GunShake");
            setVal[SET_ID_ChildrenSong].SetSelectName("ChildrenSong");
            setVal[SET_ID_MainSoundVolume].SetSelectName("Sound Volume");

            setVal[SET_ID_OutMode].SetSelectName("Out Mode");
            setVal[SET_ID_TicketBl].SetSelectName("Ticket Ratio");
            setVal[SET_ID_GiftBl].SetSelectName("Gift Ratio");

            // 中性模式
            setVal[SET_ID_TicketsOneCoin].SetSelectName("Tickets Per Coin");
            setVal[SET_ID_EditerOneCoin].SetSelectName("Editers Per Coin");
            setVal[SET_ID_MinTickets].SetSelectName("Min Tickets");
            setVal[SET_ID_OutDcTime].SetSelectName("Out Check Time");

            // 设置项值: 
            // 游戏模式
            setVal[SET_ID_GameMode].SetValueName(0, "Entertain Mode");
            setVal[SET_ID_GameMode].SetValueName(1, "Neutral Mode");
            // 投退模式
            setVal[SET_ID_InOutMode].SetValueName(0, "Double Inupt Double Return");
            setVal[SET_ID_InOutMode].SetValueName(1, "Double Inupt Single Return");
            setVal[SET_ID_InOutMode].SetValueName(2, "Single Input Single Return");

            // 枪模式
            setVal[SET_ID_GunMode].SetValueName(0, "Gun");
            setVal[SET_ID_GunMode].SetValueName(1, "Shoot Bead");
            setVal[SET_ID_GunMode].SetValueName(2, "Shoot Water");
            // 投退模式
            setVal[SET_ID_PlayerMode].SetValueName(0, "One Player");
            setVal[SET_ID_PlayerMode].SetValueName(1, "Two Player");
            // 奖励模式
            setVal[SET_ID_OutMode].SetValueName(0, "None");
            setVal[SET_ID_OutMode].SetValueName(1, "Return Ticket");
            setVal[SET_ID_OutMode].SetValueName(2, "Retrun Gift");
            //
            setVal[SET_ID_ScoreTtl].SetValueName(0, "No");
            setVal[SET_ID_ScoreTtl].SetValueName(1, "Yes");
            //
            setVal[SET_ID_DeskMusic].SetValueName(0, "OFF");
            setVal[SET_ID_DeskMusic].SetValueName(1, "ON");
            //
            setVal[SET_ID_ShowQrCode].SetValueName(0, "OFF");
            setVal[SET_ID_ShowQrCode].SetValueName(1, "ON");
            //
            setVal[SET_ID_MP5GunShake].SetValueName(0, "OFF");
            setVal[SET_ID_MP5GunShake].SetValueName(1, "ON");
            //
            setVal[SET_ID_ChildrenSong].SetValueName(0, "No");
            setVal[SET_ID_ChildrenSong].SetValueName(1, "Yes");
        }

        setVal[SET_ID_AttackPower].SetValueName(0, "1");
        setVal[SET_ID_AttackPower].SetValueName(1, "1.5");
        setVal[SET_ID_AttackPower].SetValueName(2, "2");
        setVal[SET_ID_AttackPower].SetValueName(3, "2.5");
        setVal[SET_ID_AttackPower].SetValueName(4, "3");
    }

    void UpdateDescript()
    {
        text_DescriptTitle.gameObject.SetActive(false);
        text_Descript.gameObject.SetActive(false);
        //
        if (postIndex >= list_SetVal.Count)
            return;
        for (int i = 0; i < setVal.Length; i++)
        {
            if (list_SetVal[postIndex] == setVal[i])
            {
                text_DescriptTitle.gameObject.SetActive(true);
                text_Descript.gameObject.SetActive(true);
                text_DescriptTitle.text = setVal[i].text_Name.text + "：";
                switch (i)
                {
                    case SET_ID_OutDcTime:
                        text_Descript.text = "退礼品信号有效间隔时间：" + setVal[i].text_SetValue.text + "毫秒。";
                        break;
                    default:
                        text_DescriptTitle.gameObject.SetActive(false);
                        text_Descript.gameObject.SetActive(false);
                        break;
                }
                break;
            }
        }
    }

    // 更新设置内容 :
    void UpdataGameSet()
    {
        //setVal[SET_ID_InOutMode].UpdateValue(Set.setVal.InOutMode);
        setVal[SET_ID_GameMode].UpdateValue(Set.setVal.GameMode);
        setVal[SET_ID_InOutMode].UpdateValue(Set.setVal.InOutMode);
        setVal[SET_ID_AttackPower].UpdateValue(Set.setVal.AttackPower);
        setVal[SET_ID_Gun1ShakePower].UpdateValue(Set.setVal.Gun1ShakePower);
        setVal[SET_ID_Gun1ShakeTime].UpdateValue(Set.setVal.Gun1ShakeTime);
        setVal[SET_ID_Gun2ShakePower].UpdateValue(Set.setVal.Gun2ShakePower);
        setVal[SET_ID_Gun2ShakeTime].UpdateValue(Set.setVal.Gun2ShakeTime);
        setVal[SET_ID_Gun3ShakePower].UpdateValue(Set.setVal.Gun3ShakePower);
        setVal[SET_ID_Gun3ShakeTime].UpdateValue(Set.setVal.Gun3ShakeTime);
        setVal[SET_ID_Gun4ShakePower].UpdateValue(Set.setVal.Gun4ShakePower);
        setVal[SET_ID_Gun4ShakeTime].UpdateValue(Set.setVal.Gun4ShakeTime);
        setVal[SET_ID_GunMode].UpdateValue(Set.setVal.GunMode);
        setVal[SET_ID_PlayerMode].UpdateValue(Set.setVal.PlayerMode);
        //
        setVal[SET_ID_OutMode].UpdateValue(Set.setVal.OutMode);
        setVal[SET_ID_TicketBl].UpdateValue(Set.setVal.TicketBl);
        setVal[SET_ID_GiftBl].UpdateValue(Set.setVal.GiftBl);
        setVal[SET_ID_StartCoins].UpdateValue(Set.setVal.StartCoins);
        setVal[SET_ID_GameTime].UpdateValue(Set.setVal.GameTime);
        setVal[SET_ID_ScoreTtl].UpdateValue(Set.setVal.ScoreTtl);
        setVal[SET_ID_DeskMusic].UpdateValue(Set.setVal.DeskMusic);
        setVal[SET_ID_ShowQrCode].UpdateValue(Set.setVal.ShowQrCode);
        setVal[SET_ID_MP5GunShake].UpdateValue(Set.setVal.MP5GunShake);
        setVal[SET_ID_ChildrenSong].UpdateValue(Set.setVal.ChildrenSong);
        //
        setVal[SET_ID_TicketsOneCoin].UpdateValue(Set.setVal.TicketsOneCoin);
        setVal[SET_ID_EditerOneCoin].UpdateValue(Set.setVal.EditerOneCoin);
        setVal[SET_ID_MinTickets].UpdateValue(Set.setVal.MinTickets);
        setVal[SET_ID_OutDcTime].UpdateValue(Set.setVal.OutDcTime);
        //
        setVal[SET_ID_MainSoundVolume].UpdateValue(Set.setVal.MainSoundVolume);
    }

    //按键 ------
    public void OnButton_SetSave_Pressed()
    {
        // 切换模式要清零
        if (Set.setVal.OutMode != setVal[SET_ID_OutMode].GetValue())
        {
            FjData.Clear();    // 清零当前账目
        }
        // 中性模式：切换兑票比率时，清零账目(当前账目)和几率
        if (setVal[SET_ID_GameMode].GetValue() == (int)en_GameMode.Zhongxing)
        {
            if (Set.setVal.TicketsOneCoin != setVal[SET_ID_TicketsOneCoin].GetValue())
            {
                FjData.Clear();    // 清零当前账目
                JL.Clear();
            }
        }

        //保存设置
        //Set.setVal.InOutMode = setVal[SET_ID_InOutMode].GetValue();
        Set.setVal.GameMode = setVal[SET_ID_GameMode].GetValue();
        Set.setVal.InOutMode = setVal[SET_ID_InOutMode].GetValue();
        Set.setVal.AttackPower = setVal[SET_ID_AttackPower].GetValue();
        Set.setVal.Gun1ShakePower = setVal[SET_ID_Gun1ShakePower].GetValue();
        Set.setVal.Gun1ShakeTime = setVal[SET_ID_Gun1ShakeTime].GetValue();
        Set.setVal.Gun2ShakePower = setVal[SET_ID_Gun2ShakePower].GetValue();
        Set.setVal.Gun2ShakeTime = setVal[SET_ID_Gun2ShakeTime].GetValue();
        Set.setVal.Gun3ShakePower = setVal[SET_ID_Gun3ShakePower].GetValue();
        Set.setVal.Gun3ShakeTime = setVal[SET_ID_Gun3ShakeTime].GetValue();
        Set.setVal.Gun4ShakePower = setVal[SET_ID_Gun4ShakePower].GetValue();
        Set.setVal.Gun4ShakeTime = setVal[SET_ID_Gun4ShakeTime].GetValue();
        Set.setVal.GunMode = setVal[SET_ID_GunMode].GetValue();
        Set.setVal.PlayerMode = setVal[SET_ID_PlayerMode].GetValue();
        //
        Set.setVal.OutMode = setVal[SET_ID_OutMode].GetValue();
        Set.setVal.TicketBl = setVal[SET_ID_TicketBl].GetValue();
        Set.setVal.GiftBl = setVal[SET_ID_GiftBl].GetValue();
        Set.setVal.StartCoins = setVal[SET_ID_StartCoins].GetValue();
        Set.setVal.GameTime = setVal[SET_ID_GameTime].GetValue();
        Set.setVal.ScoreTtl = setVal[SET_ID_ScoreTtl].GetValue();
        Set.setVal.DeskMusic = setVal[SET_ID_DeskMusic].GetValue();
        Set.setVal.ShowQrCode = setVal[SET_ID_ShowQrCode].GetValue();
        Set.setVal.MP5GunShake = setVal[SET_ID_MP5GunShake].GetValue();
        Set.setVal.ChildrenSong = setVal[SET_ID_ChildrenSong].GetValue();
        //
        Set.setVal.TicketsOneCoin = setVal[SET_ID_TicketsOneCoin].GetValue();
        Set.setVal.EditerOneCoin = setVal[SET_ID_EditerOneCoin].GetValue();
        Set.setVal.MinTickets = setVal[SET_ID_MinTickets].GetValue();
        Set.setVal.OutDcTime = setVal[SET_ID_OutDcTime].GetValue();
        //
        Set.setVal.MainSoundVolume = setVal[SET_ID_MainSoundVolume].GetValue();

        Set.SaveAll();
    }
}
