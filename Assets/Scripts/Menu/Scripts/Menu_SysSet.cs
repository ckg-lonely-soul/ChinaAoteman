using UnityEngine;
using UnityEngine.UI;

enum en_ButtonId : int
{
    GameSet = 0,
    //WinSet,
    GameSelect,
    Acc,
    AdcVerify,
    //UpdateVideo,
#if DEF_ENC
    Enc,
#endif
    IoCheck,
#if LANGUAGE_ALL
    Language,
#endif
    Back,
    Count,
}

public class Menu_SysSet : MonoBehaviour
{

    const int BUTTON_ID_GAMESET = (int)en_ButtonId.GameSet;
    //const int BUTTON_ID_WINSET = (int)en_ButtonId.WinSet;
    const int BUTTON_ID_GAMESELECT = (int)en_ButtonId.GameSelect;
    const int BUTTON_ID_ACC = (int)en_ButtonId.Acc;
    const int BUTTON_ID_ADCVERIFY = (int)en_ButtonId.AdcVerify;
    //const int BUTTON_ID_UPDATEVIDEO = (int)en_ButtonId.UpdateVideo;
#if DEF_ENC
    const int BUTTON_ID_ENC = (int)en_ButtonId.Enc;
#endif
    const int BUTTON_ID_IOCHECK = (int)en_ButtonId.IoCheck;
#if LANGUAGE_ALL
    const int BUTTON_ID_LANGUAGE = (int)en_ButtonId.Language;
#endif
    const int BUTTON_ID_BACK = (int)en_ButtonId.Back;
    const int MAX_BUTTON = (int)en_ButtonId.Count;


    // 要显示的内容(中英文切换)    
    public Text text_SysSet;
    public Menu_Button[] button;
    public Menu_Button button_Back;
    static int postIndex;

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
        for (int i = 0; i < button.Length; i++)
        {
            button[i].gameObject.SetActive(false);
        }
        button[BUTTON_ID_BACK] = button_Back;
        for (int i = 0; i < MAX_BUTTON; i++)
        {
            button[i].gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (Menu.statue != en_MenuStatue.MenuSta_SysSet)
            return;
        if (menuTips.gameObject.activeSelf)
            return;
        if (passwordManager.gameObject.activeSelf)
            return;
        if (passwordManager.waitCheck)
        {
            passwordManager.waitCheck = false;
            if (passwordManager.checkResult)
            {
                if (postIndex == BUTTON_ID_ACC)
                {
                    OnButton_Acc_Pressed();
                    return;
                }
            }
        }


        if (Key.MENU_LeftPressed() || Key.KEYFJ_Menu_LeftPressed())
        {
            postIndex = (postIndex + MAX_BUTTON - 1) % MAX_BUTTON;
            UpdateCursor();
        }
        if (Key.MENU_RightPressed() || Key.KEYFJ_Menu_RightPressed())
        {
            postIndex = (postIndex + 1) % MAX_BUTTON;
            UpdateCursor();
        }
        if (Key.MENU_OkPressed() || Key.KEYFJ_Menu_OkPressed())
        {
            switch (postIndex)
            {
                case BUTTON_ID_GAMESET: OnButton_GameSet_Pressed(); break;
                //case BUTTON_ID_WINSET: OnButton_WinSet_Pressed(); break;
                case BUTTON_ID_GAMESELECT: OnButton_GameSelect_Pressed(); break;
                case BUTTON_ID_ADCVERIFY: OnButton_CursorVerify_Pressed(); break;
                case BUTTON_ID_ACC:
                    passwordManager.GameStart(en_PasswordOptionType.Check);
                    break;
                //case BUTTON_ID_UPDATEVIDEO: OnButton_UpdateVideo_Pressed(); break;
#if DEF_ENC
            case BUTTON_ID_ENC: OnButton_Enc_Pressed(); break;
#endif
                case BUTTON_ID_IOCHECK: OnButton_IoCheck_Pressed(); break;
#if LANGUAGE_ALL
                case BUTTON_ID_LANGUAGE: OnButton_Language_Pressed(); break;
#endif
                case BUTTON_ID_BACK: OnButton_Back_Pressed(); break;
            }
        }
    }

    // 更新光标坐标和大小
    void UpdateCursor()
    {
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
        postIndex = 0;
        UpdateCursor();
    }
    // 2.更新显示中英文
    public void UpdateLanguage()
    {
        if (Set.setVal.Language == (int)en_Language.Chinese)
        {
            //中文
            text_SysSet.text = "系统设置";
            button[BUTTON_ID_GAMESET].GetComponentInChildren<Text>().text = "参数设置";
            //button[BUTTON_ID_WINSET].GetComponentInChildren<Text>().text = "礼品设置";
            button[BUTTON_ID_GAMESELECT].GetComponentInChildren<Text>().text = "游戏选择";
            button[BUTTON_ID_ADCVERIFY].GetComponentInChildren<Text>().text = "ADC校准";
            button[BUTTON_ID_ACC].GetComponentInChildren<Text>().text = "账目查询";
            //button[BUTTON_ID_UPDATEVIDEO].GetComponentInChildren<Text>().text = "更新视频";
#if DEF_ENC
            button[BUTTON_ID_ENC].GetComponentInChildren<Text>().text = "报账";
#endif
            button[BUTTON_ID_IOCHECK].GetComponentInChildren<Text>().text = "IO检测";
#if LANGUAGE_ALL
            button[BUTTON_ID_LANGUAGE].GetComponentInChildren<Text>().text = "语言";
#endif
            button[BUTTON_ID_BACK].GetComponentInChildren<Text>().text = "返回";
        }
        else
        {
            //英文
            text_SysSet.text = "System Set";
            button[BUTTON_ID_GAMESET].GetComponentInChildren<Text>().text = "Game Set";
            //button[BUTTON_ID_WINSET].GetComponentInChildren<Text>().text = "Gift Set";
            button[BUTTON_ID_GAMESELECT].GetComponentInChildren<Text>().text = "Game Select";
            button[BUTTON_ID_ADCVERIFY].GetComponentInChildren<Text>().text = "ADC Verify";
            button[BUTTON_ID_ACC].GetComponentInChildren<Text>().text = "Acc";
            //button[BUTTON_ID_UPDATEVIDEO].GetComponentInChildren<Text>().text = "Update Video";
#if DEF_ENC
            button[BUTTON_ID_ENC].GetComponentInChildren<Text>().text = "Report";
#endif
            button[BUTTON_ID_IOCHECK].GetComponentInChildren<Text>().text = "IO Check";
#if LANGUAGE_ALL
            button[BUTTON_ID_LANGUAGE].GetComponentInChildren<Text>().text = "Language";
#endif
            button[BUTTON_ID_BACK].GetComponentInChildren<Text>().text = "Back";
        }
    }

    //按键 ------
    public void OnButton_GameSet_Pressed()
    {
        menu.ChangeStatue(en_MenuStatue.MenuSta_GameSet);
    }
    public void OnButton_WinSet_Pressed()
    {
        menu.ChangeStatue(en_MenuStatue.MenuSta_WinSet);
    }
    public void OnButton_GameSelect_Pressed()
    {
        menu.ChangeStatue(en_MenuStatue.MenuSta_GameSelect);
    }
    public void OnButton_CursorVerify_Pressed()
    {
        menu.ChangeStatue(en_MenuStatue.MenuSta_AdcVerify);
    }
    public void OnButton_Acc_Pressed()
    {
        menu.ChangeStatue(en_MenuStatue.MenuSta_Acc);
    }
    public void OnButton_UpdateVideo_Pressed()
    {
        menu.ChangeStatue(en_MenuStatue.MenuSta_UpdateVideo);
    }
    public void OnButton_IoCheck_Pressed()
    {
        menu.ChangeStatue(en_MenuStatue.MenuSta_IoCheck);
    }
    public void OnButton_Enc_Pressed()
    {
        menu.ChangeStatue(en_MenuStatue.MenuSta_Enc);
    }
    public void OnButton_Language_Pressed()
    {
        menu.ChangeStatue(en_MenuStatue.MenuSta_Language);
    }
    public void OnButton_Back_Pressed()
    {
        //	Application.LoadLevel ("Loading");
        menu.Quit();
    }
}
