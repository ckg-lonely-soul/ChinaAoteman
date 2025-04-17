using UnityEngine;
using UnityEngine.UI;

public enum en_PasswordOptionType
{
    Check = 0,  // 验证
    Modify,     // 修改
}

enum en_PasswordOptionSta
{
    Check = 0,  // 验证
    NewPsw,     // 新密码
    AgainPsw,   // 
}

public class Menu_PasswordManager : MonoBehaviour
{
    public Text text_Title;
    public Text text_Tips;
    public Text text_WxTips;
    public AccValue accValue_Psw;
    public Menu_Button[] button;
    //
    const int BUTTON_ID_DEL = 10;
    const int BUTTON_ID_OK = 11;
    const int BUTTON_ID_BACK = 12;
    const int MAX_SEL = 13;
    const int PASSWORD_LEN = 6;
    readonly int[] tab_Key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, BUTTON_ID_DEL, 0, BUTTON_ID_OK, BUTTON_ID_BACK };
    //
    en_PasswordOptionType optionType;
    en_PasswordOptionSta statue;
    public bool waitCheck;
    public bool checkResult;
    int postIndex;
    int numLen;
    int password;
    int newPsw;

    Menu menu;
    Menu_Tips menuTips;
    public void Init(Menu mm)
    {
        menu = mm;
        menuTips = menu.menuTips;
    }

    int IndexOfArry(int[] arry, int value)
    {
        for (int i = 0; i < arry.Length; i++)
        {
            if (arry[i] == value)
            {
                return i;
            }
        }
        return -1;
    }
    public void UpdateLanguage()
    {
        if (Set.setVal.Language == (int)en_Language.Chinese)
        {
            //中文               
            if (optionType == en_PasswordOptionType.Modify)
            {
                text_Title.text = "修改密码";
            }
            else
            {
                text_Title.text = "密码验证";
            }
            text_WxTips.text = "温馨提示：出厂密码为 " + Set.DEF_PASSWORD.ToString("D6") + "，修改密码后请注意记住密码。";
            accValue_Psw.SetName("密码:");
            button[IndexOfArry(tab_Key, BUTTON_ID_DEL)].GetComponentInChildren<Text>().text = "删除";
            button[IndexOfArry(tab_Key, BUTTON_ID_OK)].GetComponentInChildren<Text>().text = "确认";
            button[IndexOfArry(tab_Key, BUTTON_ID_BACK)].GetComponentInChildren<Text>().text = "退出";
        }
        else
        {
            //英文     
            if (optionType == en_PasswordOptionType.Modify)
            {
                text_Title.text = "Modify Password";
            }
            else
            {
                text_Title.text = "Check Password";
            }
            text_WxTips.text = "Tips: the factory password is " + Set.DEF_PASSWORD.ToString("D6") + "，Please remember the password after changing the password.";
            accValue_Psw.SetName("Password:");
            button[IndexOfArry(tab_Key, BUTTON_ID_DEL)].GetComponentInChildren<Text>().text = "Del";
            button[IndexOfArry(tab_Key, BUTTON_ID_OK)].GetComponentInChildren<Text>().text = "Ok";
            button[IndexOfArry(tab_Key, BUTTON_ID_BACK)].GetComponentInChildren<Text>().text = "Back";
        }
    }
    // Use this for initialization
    public void GameStart(en_PasswordOptionType type)
    {
        //#if UNITY_EDITOR
        //        Set.setVal.Password = 888888;
        //#endif
        gameObject.SetActive(true);
        optionType = type;
        UpdateLanguage();
        checkResult = false;
        waitCheck = true;
        postIndex = BUTTON_ID_BACK;
        Update_SelectPos();
        ChangeStatue(en_PasswordOptionSta.Check);
        //if (Set.setVal.Password == Set.DEF_PASSWORD) {
        //    password = Set.setVal.Password;
        //    numLen = 6;
        //    Update_Psw();
        //}
    }

    void Exit()
    {
        gameObject.SetActive(false);
        Key.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (menuTips.gameObject.activeSelf)
            return;
        if (menuTips.waitTips)
        {
            menuTips.waitTips = false;
            ClearPsw();
        }

        if (Key.MENU_LeftPressed() || Key.KEYFJ_Menu_LeftPressed())
        {
            postIndex = (postIndex + MAX_SEL - 1) % MAX_SEL;
            Update_SelectPos();
        }
        if (Key.MENU_RightPressed() || Key.KEYFJ_Menu_RightPressed())
        {
            postIndex = (postIndex + 1) % MAX_SEL;
            Update_SelectPos();
        }
        if (Key.MENU_OkPressed() || Key.KEYFJ_Menu_OkPressed())
        {
            int key = tab_Key[postIndex];
            //
            if (key == BUTTON_ID_DEL)
            {
                // DEL
                if (numLen > 0)
                {
                    numLen--;
                    password /= 10;
                    Update_Psw();
                }
            }
            else if (key == BUTTON_ID_OK)
            {
                // ok
                if (numLen >= PASSWORD_LEN)
                {
                    switch (statue)
                    {
                        case en_PasswordOptionSta.Check:
                            if (password == Set.SUP_PASSWORD)
                            {
                                // 超级密码：恢复初始密码：                            
                                Set.DefaultPassword();
                                if (Set.setVal.Language == (int)en_Language.Chinese)
                                {
                                    menuTips.Init(en_MenuTipsType.Tips, "已恢复为初始密码", 1);
                                }
                                else
                                {
                                    menuTips.Init(en_MenuTipsType.Tips, "Restored to original password", 1);
                                }
                            }
                            else if (password == Set.setVal.Password)
                            {
                                // 密码正确
                                if (optionType == en_PasswordOptionType.Modify)
                                {
                                    ChangeStatue(en_PasswordOptionSta.NewPsw);
                                }
                                else
                                {
                                    checkResult = true;
                                    Exit();
                                }
                            }
                            else
                            {
                                // 密码错误
                                if (Set.setVal.Language == (int)en_Language.Chinese)
                                {
                                    menuTips.Init(en_MenuTipsType.Tips, "密码错误", 1);
                                }
                                else
                                {
                                    menuTips.Init(en_MenuTipsType.Tips, "Password error", 1);
                                }
                            }
                            break;
                        case en_PasswordOptionSta.NewPsw:
                            if (optionType == en_PasswordOptionType.Modify)
                            {
                                newPsw = password;
                                ChangeStatue(en_PasswordOptionSta.AgainPsw);
                            }
                            break;
                        case en_PasswordOptionSta.AgainPsw:
                            if (optionType == en_PasswordOptionType.Modify)
                            {
                                if (newPsw == password)
                                {
                                    Set.setVal.Password = password;
                                    Set.SavePassword();
                                    Exit();
                                    if (Set.setVal.Language == (int)en_Language.Chinese)
                                    {
                                        menuTips.Init(en_MenuTipsType.Tips, "修改密码成功", 1);
                                    }
                                    else
                                    {
                                        menuTips.Init(en_MenuTipsType.Tips, "Password modified successfully", 1);
                                    }
                                }
                                else
                                {
                                    if (Set.setVal.Language == (int)en_Language.Chinese)
                                    {
                                        menuTips.Init(en_MenuTipsType.Tips, "两次输入密码不一致，请重新输入", 1);
                                    }
                                    else
                                    {
                                        menuTips.Init(en_MenuTipsType.Tips, "The two passwords are inconsistent. Please re-enter", 1);
                                    }
                                }
                            }
                            break;
                    }
                }
                else
                {
                    if (Set.setVal.Language == (int)en_Language.Chinese)
                    {
                        menuTips.Init(en_MenuTipsType.Tips, "请输入6位数密码", 1);
                    }
                    else
                    {
                        menuTips.Init(en_MenuTipsType.Tips, "Please enter a 6-digit password", 1);
                    }
                }
            }
            else if (key == BUTTON_ID_BACK)
            {
                Exit();
            }
            else if (key < 10)
            {
                // 数字:
                if (numLen < 6)
                {
                    password *= 10;
                    password += key;
                    numLen++;
                    Update_Psw();
                }
            }
        }
    }

    void ChangeStatue(en_PasswordOptionSta sta)
    {
        statue = sta;
        switch (statue)
        {
            case en_PasswordOptionSta.Check:
                ClearPsw();
                if (optionType == en_PasswordOptionType.Modify)
                {
                    if (Set.setVal.Language == (int)en_Language.Chinese)
                    {
                        text_Tips.text = "请输入旧密码：";
                    }
                    else
                    {
                        text_Tips.text = "Enter old password：";
                    }
                }
                else
                {
                    if (Set.setVal.Language == (int)en_Language.Chinese)
                    {
                        text_Tips.text = "请输入密码：";
                    }
                    else
                    {
                        text_Tips.text = "Enter password：";
                    }
                }

                break;
            case en_PasswordOptionSta.NewPsw:
                ClearPsw();
                if (Set.setVal.Language == (int)en_Language.Chinese)
                {
                    text_Tips.text = "请输入新密码：";
                }
                else
                {
                    text_Tips.text = "Enter new password:";
                }
                break;
            case en_PasswordOptionSta.AgainPsw:
                ClearPsw();
                if (Set.setVal.Language == (int)en_Language.Chinese)
                {
                    text_Tips.text = "请再次输入新密码：";
                }
                else
                {
                    text_Tips.text = "Enter new password again：";
                }
                break;
        }
    }

    void Update_SelectPos()
    {
        for (int i = 0; i < button.Length; i++)
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
    void ClearPsw()
    {
        password = 0;
        numLen = 0;
        Update_Psw();
    }
    void Update_Psw()
    {
        if (numLen <= 0)
        {
            accValue_Psw.text_Data.text = "";
        }
        else
        {
            string format = "D" + numLen;
            accValue_Psw.text_Data.text = password.ToString(format);
        }
    }
}
