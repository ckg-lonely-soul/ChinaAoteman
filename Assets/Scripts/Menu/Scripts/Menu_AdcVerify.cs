using UnityEngine;
using UnityEngine.UI;

public class Menu_AdcVerify : MonoBehaviour
{
    public GameObject text_Title;
    public GameObject cursor_Obj;
    public Text text_TipsTitle;
    public Text text_Tips;
    public Text text_ADC_LR;
    public Text text_ADC_UD;

    //
    Menu menu;

    enum en_CusorVerifySta
    {
        Left = 0,
        Right,
        Up,
        Down,
        OK,
    }
    en_CusorVerifySta statue;
    public int playerId;
    int playerNum = Main.MAX_PLAYER;

    //
    public void Awake0(Menu mm)
    {
        //
        menu = mm;
    }

    // Update is called once per frame
    void Update()
    {
        if (Menu.statue != en_MenuStatue.MenuSta_AdcVerify)
            return;

        if (playerId >= 0 && playerId < Main.MAX_PLAYER)
        {
            text_ADC_LR.text = "ADC_LR: " + Key.adValue[playerId * 2 + 1].ToString();
            text_ADC_UD.text = "ADC_UD: " + Key.adValue[playerId * 2 + 0].ToString();
        }

        switch (statue)
        {
            case en_CusorVerifySta.Left:     // 对准左上角
                CursorSmallChange(-Screen.width / 2, -Screen.width / 2 + 200, Screen.height / 2, Screen.height / 2 - 200);
                if (Key.KEYFJ_OkPressed(playerId) || Key.MENU_OkPressed())
                {
                    Key.SaveCursorPos_Left(playerId, (int)cursor_Obj.transform.localPosition.x);
                    Key.SetAdc_MinX(playerId);
                    Key.SetAdc_MidY(playerId);
                    ChagneStatue(en_CusorVerifySta.Right);
                }
                break;
            case en_CusorVerifySta.Right:     // 对准右下角
                CursorSmallChange(Screen.width / 2 - 200, Screen.width / 2, -Screen.height / 2 + 200, -Screen.height / 2);
                if (Key.KEYFJ_OkPressed(playerId) || Key.MENU_OkPressed())
                {
                    Key.SaveCursorPos_Right(playerId, (int)cursor_Obj.transform.localPosition.x);
                    Key.SetAdc_MaxX(playerId);
                    ChagneStatue(en_CusorVerifySta.Up);
                }
                break;
            case en_CusorVerifySta.Up:
                if (Key.KEYFJ_OkPressed(playerId) || Key.MENU_OkPressed())
                {
                    Key.SaveCursorPos_Top(playerId, (int)cursor_Obj.transform.localPosition.y);
                    Key.SetAdc_MaxY(playerId);
                    Key.SetAdc_MidX(playerId);
                    ChagneStatue(en_CusorVerifySta.Down);
                }
                break;
            case en_CusorVerifySta.Down:
                if (Key.KEYFJ_OkPressed(playerId) || Key.MENU_OkPressed())
                {
                    Key.SaveCursorPos_Button(playerId, (int)cursor_Obj.transform.localPosition.y);
                    Key.SetAdc_MinY(playerId);
                    playerId++;
                    if (playerId >= playerNum)
                    {
                        ChagneStatue(en_CusorVerifySta.OK);
                    }
                    else
                    {
                        ChagneStatue(en_CusorVerifySta.Left);
                    }
                }
                break;
            case en_CusorVerifySta.OK:     // 保存
                for (int i = 0; i < playerNum; i++)
                {
                    if (Key.KEYFJ_OkPressed(i))
                    {
                        menu.ChangeStatue(en_MenuStatue.MenuSta_SysSet);
                        return;
                    }
                }
                if (Key.MENU_OkPressed() || Key.KEYFJ_Menu_OkPressed())
                {
                    menu.ChangeStatue(en_MenuStatue.MenuSta_SysSet);
                }
                break;
        }
    }
    //
    void ChagneStatue(en_CusorVerifySta sta)
    {
        statue = sta;
        Key.Clear();
        switch (statue)
        {
            case en_CusorVerifySta.Left:     // 对准左上角
                if (Set.setVal.Language == (int)en_Language.Chinese)
                {
                    text_TipsTitle.text = "玩家 " + (playerId + 1).ToString() + " 枪校准";
                    text_Tips.text = "请将枪转到最左";
                }
                else
                {
                    text_TipsTitle.text = "Player " + (playerId + 1).ToString() + " Cursor Verify";
                    text_Tips.text = "";
                }
                cursor_Obj.transform.localPosition = new Vector3(-Screen.width / 2, 0, 0);
                break;
            case en_CusorVerifySta.Right:     // 对准右下角
                if (Set.setVal.Language == (int)en_Language.Chinese)
                {
                    text_TipsTitle.text = "玩家 " + (playerId + 1).ToString() + " 枪校准";
                    text_Tips.text = "请将枪转到最右";
                }
                else
                {
                    text_TipsTitle.text = "Player " + (playerId + 1).ToString() + " Cursor Verify";
                    text_Tips.text = "";
                }
                cursor_Obj.transform.localPosition = new Vector3(Screen.width / 2, 0, 0);
                break;
            case en_CusorVerifySta.Up:
                if (Set.setVal.Language == (int)en_Language.Chinese)
                {
                    text_TipsTitle.text = "玩家 " + (playerId + 1).ToString() + " 准心校准";
                    text_Tips.text = "请对准屏幕上边准心抠动班机";
                }
                else
                {
                    text_TipsTitle.text = "Player " + (playerId + 1).ToString() + " Cursor Verify";
                    text_Tips.text = "";
                }
                cursor_Obj.transform.localPosition = new Vector3(0, Screen.height / 2, 0);
                break;
            case en_CusorVerifySta.Down:
                if (Set.setVal.Language == (int)en_Language.Chinese)
                {
                    text_TipsTitle.text = "玩家 " + (playerId + 1).ToString() + " 准心校准";
                    text_Tips.text = "请对准屏幕下边准心抠动班机";
                }
                else
                {
                    text_TipsTitle.text = "Player " + (playerId + 1).ToString() + " Cursor Verify";
                    text_Tips.text = "";
                }
                cursor_Obj.transform.localPosition = new Vector3(0, -Screen.height / 2, 0);
                break;
            case en_CusorVerifySta.OK:     // 保存
                                           // 校准完成
                if (Set.setVal.Language == (int)en_Language.Chinese)
                {
                    text_TipsTitle.text = "准心校准完成";
                    text_Tips.text = "确认返回";
                }
                else
                {
                    text_TipsTitle.text = "Finish";
                    text_Tips.text = "Press Back";
                }
                cursor_Obj.transform.localPosition = new Vector3(0, 0, 0);
                break;
        }
    }

    //
    public void GameStart()
    {
        UpdataLanguage();
        playerNum = Main.MAX_PLAYER;
        playerId = 0;
        ChagneStatue(en_CusorVerifySta.Left);
    }

    public void UpdataLanguage()
    {
        if (Set.setVal.Language == (int)en_Language.Chinese)
        {
            //中文
            text_Title.GetComponent<Text>().text = "准心校准";
        }
        else
        {
            //英文
            text_Title.GetComponent<Text>().text = "Cursor Verify";
        }
    }

    // 光标微调
    void CursorSmallChange(float limitLeft, float limitRight, float limitTop, float limitButtom)
    {
        if (Key.MENU_Statue_Left())
        {
            if (cursor_Obj.transform.localPosition.x > limitLeft)
            {
                cursor_Obj.transform.Translate(-50 * Time.deltaTime, 0, 0);
                if (cursor_Obj.transform.localPosition.x < limitLeft)
                {
                    cursor_Obj.transform.localPosition = new Vector3(limitLeft, cursor_Obj.transform.localPosition.y, 0);
                }
            }
        }
        if (Key.MENU_Statue_Right())
        {
            if (cursor_Obj.transform.localPosition.x < limitRight)
            {
                cursor_Obj.transform.Translate(50 * Time.deltaTime, 0, 0);
                if (cursor_Obj.transform.localPosition.x > limitRight)
                {
                    cursor_Obj.transform.localPosition = new Vector3(limitRight, cursor_Obj.transform.localPosition.y, 0);
                }
            }
        }
    }

}
