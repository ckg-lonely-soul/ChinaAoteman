using UnityEngine;
using UnityEngine.UI;

enum en_AccClearStaut
{
    Clear_None = 0,
    Clear_CurrAcc,
    Clear_TotalAcc,
    Clear_JL,
}
enum en_AccSelectStatue
{
    None = 0,
    ClearAcc,
    ClearTotalAcc,
}

public class Menu_Acc : MonoBehaviour
{
    const int MAX_ACC_ONEPAGE = 6;
    int MAX_ACC;
    const int MAX_SET_SEL = 0;
    const int MAX_BUTTON = 3;
    const int MAX_SEL = MAX_SET_SEL + MAX_BUTTON;

    const int MAX_PAGE = Main.MAX_PLAYER + 1;

    // 要显示的内容(中英文切换)    
    public Text text_Title;
    public Text text_PageTips;
    public Menu_Button[] button;
    // 账目
    public GameObject accData_Prefab;

    AccValue[] accVal = new AccValue[MAX_ACC_ONEPAGE];
    //
    int postIndex;
    int page;
    float clearTotalTime;

    //
    en_AccClearStaut statue;        // 状态: 0: 普通; 1: 清零账目; 2: 清零几率
    en_AccSelectStatue selectStatue;

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
        for (int i = 0; i < MAX_ACC_ONEPAGE; i++)
        {
            obj = Instantiate(accData_Prefab, transform);
            accVal[i] = obj.GetComponent<AccValue>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //
        if (Menu.statue != en_MenuStatue.MenuSta_Acc)
            return;
        if (menuTips.gameObject.activeSelf)
            return;
        if (passwordManager.gameObject.activeSelf)
            return;
        if (menuTips.waitTips)
        {
            menuTips.waitTips = false;
            if (menuTips.tipsType == en_MenuTipsType.Select)
            {
                if (menuTips.result)
                {
                    switch (selectStatue)
                    {
                        case en_AccSelectStatue.ClearAcc:
                            OnButton_AccClear_Pressed();
                            //	postIndex = MAX_SET_SEL + 1;
                            UpdateCursor();
                            break;
                        case en_AccSelectStatue.ClearTotalAcc:
                            FjData.ClearTotalStart();
                            statue = en_AccClearStaut.Clear_TotalAcc;
                            break;
                    }
                }
                else
                {
                    selectStatue = en_AccSelectStatue.None;
                }
            }
        }

        // 清零等待
        if (statue == en_AccClearStaut.Clear_CurrAcc)
        {
            // 清零当期账目中...
            FjData.Clear();
            statue = en_AccClearStaut.Clear_JL; //恢复
            postIndex = MAX_SET_SEL + 2;
            UpdateCursor();
            UpdateAccData();
            return;
        }
        else if (statue == en_AccClearStaut.Clear_TotalAcc)
        {
            // 清总账目
            FjData.ClearTotal();
            statue = en_AccClearStaut.Clear_JL; //恢复
            postIndex = MAX_SET_SEL + 2;
            UpdateCursor();
            UpdateAccData();
            return;
        }
        else if (statue == en_AccClearStaut.Clear_JL)
        {
            // 清零几率
            JL.Clear();
            statue = en_AccClearStaut.Clear_None; //恢复
            postIndex = MAX_SET_SEL + 2;
            UpdateCursor();
            // Tips
            switch (selectStatue)
            {
                case en_AccSelectStatue.ClearAcc:
                    if (Set.setVal.Language == (int)en_Language.Chinese)
                    {
                        menuTips.Init(en_MenuTipsType.Tips, "账目清零成功", 3);
                    }
                    else
                    {
                        menuTips.Init(en_MenuTipsType.Tips, "Account cleared successfully", 3);
                    }
                    break;
                case en_AccSelectStatue.ClearTotalAcc:
                    if (Set.setVal.Language == (int)en_Language.Chinese)
                    {
                        menuTips.Init(en_MenuTipsType.Tips, "历史总账清零成功", 3);
                    }
                    else
                    {
                        menuTips.Init(en_MenuTipsType.Tips, "Historical accounts cleared successfully", 3);
                    }
                    break;
            }
            selectStatue = en_AccSelectStatue.None;
            return;
        }

        // 同时长按左右按键，清总账目
        if ((Key.MENU_Statue_Left() && Key.MENU_Statue_Right()) || (Key.KEYFJ_Menu_Statue_Left() && Key.KEYFJ_Menu_Statue_Right()))
        {
            clearTotalTime += Time.deltaTime;
            if (clearTotalTime >= 5.0f)
            {
                clearTotalTime = 0;

                if (CanClear() == false)
                    return;
                //FjData.ClearTotalStart();
                //statue = en_AccClearStaut.Clear_TotalAcc;
                if (Set.setVal.Language == (int)en_Language.Chinese)
                {
                    menuTips.Init(en_MenuTipsType.Select, "是否清零历史总账?", 0);
                }
                else
                {
                    menuTips.Init(en_MenuTipsType.Select, "Clear Historical accounts?", 0);
                }
                selectStatue = en_AccSelectStatue.ClearTotalAcc;
                return;
            }
        }
        else
        {
            clearTotalTime = 0;
        }

        if (Key.MENU_LeftPressed() || Key.KEYFJ_Menu_LeftPressed())
        {
            postIndex = (postIndex + MAX_SEL - 1) % MAX_SEL;
            UpdateCursor();
        }
        if (Key.MENU_RightPressed() || Key.KEYFJ_Menu_RightPressed())
        {
            postIndex = (postIndex + 1) % MAX_SEL;
            UpdateCursor();
        }
        if (Key.MENU_OkPressed() || Key.KEYFJ_Menu_OkPressed())
        {
            if (postIndex == MAX_SET_SEL + 0)
            {
                if (CanClear() == false)
                    return;
                //清零
                if (Set.setVal.Language == (int)en_Language.Chinese)
                {
                    menuTips.Init(en_MenuTipsType.Select, "是否清零账目?", 0);
                }
                else
                {
                    menuTips.Init(en_MenuTipsType.Select, "Clear account?", 0);
                }
                selectStatue = en_AccSelectStatue.ClearAcc;

                //OnButton_AccClear_Pressed();
                ////	postIndex = MAX_SET_SEL + 1;
                //UpdateCursor();
                //} else if (postIndex == MAX_SET_SEL + 1) {
                //    // 下一页
                //    page = (page + 1) % MAX_PAGE;
                //    UpdateAccData();
            }
            else if (postIndex == MAX_SET_SEL + 1)
            {
                // 修改密码
                passwordManager.GameStart(en_PasswordOptionType.Modify);
            }
            else if (postIndex == MAX_SET_SEL + 2)
            {
                //返回
                OnButton_AccBack_Pressed();
            }
        }
    }

    // 更新光标坐标和大小
    void UpdateCursor()
    {
        //三个按键
        for (int i = 0; i < MAX_BUTTON; i++)
        {
            if (i == postIndex)
            {
                button[i].SetSelect(true);
            }
            else
            {
                button[i].SetSelect(false);
            }
        }
    }

    // 1.进入初始化
    public void GameStart()
    {
        UpdateLanguage();
        //        
        page = 0;
        postIndex = MAX_SET_SEL + 2;		// 返回
        statue = en_AccClearStaut.Clear_None; //恢复
        selectStatue = en_AccSelectStatue.None;
        clearTotalTime = 0;
        UpdateCursor();
        // 更新账目
        UpdateAccData();
        //
        UpdateAccValPos();
    }

    //
    void UpdateAccValPos()
    {
        int index;
        for (int i = 0; i < MAX_ACC; i++)
        {
            if (i / MAX_ACC_ONEPAGE == page)
            {
                index = i % MAX_ACC_ONEPAGE;
                accVal[i].gameObject.SetActive(true);
                accVal[i].transform.localPosition = new Vector3((index / 3) * 400 - 200, 220 - (index % 3) * 60, 0);
            }
            else
            {
                accVal[i].gameObject.SetActive(false);
            }
        }
    }




    // 2.更新显示中英文
    public void UpdateLanguage()
    {
        if (Set.setVal.Language == (int)en_Language.Chinese)
        {
            //中文
            //按键
            text_Title.text = "账目查询";
            button[0].GetComponentInChildren<Text>().text = "清零";
            button[1].GetComponentInChildren<Text>().text = "修改密码";
            button[2].GetComponentInChildren<Text>().text = "返回";
        }
        else
        {
            //英文
            //按键
            text_Title.text = "Acc";
            button[0].GetComponentInChildren<Text>().text = "Clear";
            button[1].GetComponentInChildren<Text>().text = "Modify Password";
            button[2].GetComponentInChildren<Text>().text = "Back";
        }
        AccVal_UpdataLanguage();
    }

    //按键 ------
    public void OnButton_AccClear_Pressed()
    {

        FjData.ClearStart();

        //JL.Clear ();
        //postIndex = MAX_SET_SEL + 1;
        UpdateAccData();
        statue = en_AccClearStaut.Clear_CurrAcc;
    }
    public void OnButton_AccBack_Pressed()
    {
        menu.ChangeStatue(en_MenuStatue.MenuSta_SysSet);
    }

    bool CanClear()
    {
        if (Main.VER_ENC)
        {
            //if (Game_Enc.IsActive() == false)
            //    return false;
        }
        return true;
    }

    // 需要修改的内容 -------------------------------------------------------------------------------------------------------

    // 2.更新显示中英文
    void AccVal_UpdataLanguage()
    {
        if (Set.setVal.Language == (int)en_Language.Chinese)
        {
            //中文
            //账目
            if (Set.setVal.GameMode == (int)en_GameMode.Yule)
            {
                accVal[0].SetName("总投币");
                accVal[1].SetName("总退票");
                accVal[2].SetName("总退扭蛋");
                accVal[3].SetName("历史投币");
                accVal[4].SetName("历史退票");
                accVal[5].SetName("历史退扭蛋");
            }
            else
            {
                accVal[0].SetName("总投币");
                accVal[1].SetName("总退票");
                accVal[2].SetName("历史投币");
                accVal[3].SetName("历史退票");
            }


            if (page == 0)
            {
                // 机台总账
                text_PageTips.text = "机台总账";
            }
            else
            {
                text_PageTips.text = "玩家" + page.ToString() + "账目";
            }
        }
        else
        {
            //英文
            //账目
            if (Set.setVal.GameMode == (int)en_GameMode.Yule)
            {
                accVal[0].SetName("Coin Input");
                accVal[1].SetName("Ticket Output");
                accVal[2].SetName("Gift Output");
                accVal[3].SetName("Total Coin Input");
                accVal[4].SetName("Total Ticket Output");
                accVal[5].SetName("Total Gift Output");
            }
            else
            {
                accVal[0].SetName("Coin Inpu");
                accVal[1].SetName("Ticket Output");
                accVal[2].SetName("Total Coin Input");
                accVal[3].SetName("Total Gift Output");
            }
            if (page == 0)
            {
                // 机台总账
                text_PageTips.text = "Total Acc";
            }
            else
            {
                text_PageTips.text = "Player" + page.ToString() + " Acc";
            }
        }
    }
    // 更新账目
    void UpdateAccData()
    {
        int i;
        int dat;

        for (i = 0; i < accVal.Length; i++)
        {
            accVal[i].gameObject.SetActive(false);
        }
        MAX_ACC = 6;
        AccVal_UpdataLanguage();
        if (page == 0)
        {
            // 机台总账
            dat = 0;
            for (i = 0; i < Main.MAX_PLAYER; i++)
                dat += FjData.acc[i].CoinIn;
            accVal[0].SetData(dat);
            //
            dat = 0;
            for (i = 0; i < Main.MAX_PLAYER; i++)
                dat += FjData.acc[i].TicketOut;
            accVal[1].SetData(dat);
            //
            dat = 0;
            for (i = 0; i < Main.MAX_PLAYER; i++)
                dat += FjData.acc[i].GiftOut;
            accVal[2].SetData(dat);
            ////
            dat = 0;
            for (i = 0; i < Main.MAX_PLAYER; i++)
                dat += FjData.totalAcc[i].CoinIn;
            accVal[3].SetData(dat);
            //
            dat = 0;
            for (i = 0; i < Main.MAX_PLAYER; i++)
                dat += FjData.totalAcc[i].TicketOut;
            accVal[4].SetData(dat);
            //
            dat = 0;
            for (i = 0; i < Main.MAX_PLAYER; i++)
                dat += FjData.totalAcc[i].GiftOut;
            accVal[5].SetData(dat);

        }
        else
        {
            // 分机账目

            accVal[0].SetData(FjData.acc[page - 1].CoinIn);
            accVal[1].SetData(FjData.acc[page - 1].TicketOut);
            accVal[2].SetData(FjData.acc[page - 1].GiftOut);
            //
            accVal[3].SetData(FjData.totalAcc[page - 1].CoinIn);
            accVal[4].SetData(FjData.totalAcc[page - 1].TicketOut);
            accVal[5].SetData(FjData.totalAcc[page - 1].GiftOut);
        }
        //
        UpdateAccValPos();
    }
}
