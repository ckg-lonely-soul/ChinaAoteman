using UnityEngine;
using UnityEngine.UI;

public class Menu_WinSet : MonoBehaviour
{
    const int MAX_SET_ONEPAGE = 12;
    int MAX_SET_SEL;// = 12;
    int MAX_SEL;// = MAX_SET_SEL + 3;

    // 要显示的内容(中英文切换)    
    public Text text_GameSet;
    public Text text_Tips;
    public Text[] text_DescriptTitle;
    public Text[] text_Descript;
    public Text text_DescriptTips;
    public GameObject setVal_Layer;
    public Menu_SelectFlag selectFlag;  //光标选中标志
    public Menu_Button[] button;
    public GameObject setVal_Prefab;
    // 设置项	
    //SetValue[] setVal = new SetValue[MAX_ALL_SET_SEL];
    //List<SetValue> list_SetVal = new List<SetValue>();
    ////
    //static int postIndex;
    //static bool selectSta;
    //float runTime_SaveText;

    //public static int SET_ID_StartCoins_sta;

    //// 传入的变量
    //Menu menu;
    //Menu_PasswordManager passwordManager;
    //Menu_Tips menuTips;
    ////
    //public void Awake0(Menu mmenu) {
    //    //
    //    menu = mmenu;
    //    passwordManager = menu.passwordManager;
    //    menuTips = menu.menuTips;

    //    //
    //    GameObject obj;
    //    for (int i = 0; i < MAX_ALL_SET_SEL; i++) {
    //        obj = Instantiate(setVal_Prefab, setVal_Layer.transform);
    //        setVal[i] = obj.GetComponent<SetValue>();
    //    }
    //    SetVal_Init();
    //}

    //// Update is called once per frame
    //void Update() {
    //    if (Menu.statue != en_MenuStatue.MenuSta_WinSet)
    //        return;

    //    if (menuTips.gameObject.activeSelf)
    //        return;
    //    // 等待确认提示
    //    if (menuTips.waitTips) {
    //        menuTips.waitTips = false;
    //        if (menuTips.tipsType == en_MenuTipsType.Select) {
    //            if (postIndex == MAX_SET_SEL + 0) {
    //                //保存并退出                
    //                if (menuTips.result) {
    //                    OnButton_SetSave_Pressed();
    //                    OnButton_SetBack_Pressed();
    //                    if (Set.setVal.Language == (int)en_Language.Chinese) {
    //                        menuTips.Init(en_MenuTipsType.Tips, "保存成功", 3);
    //                    } else {
    //                        menuTips.Init(en_MenuTipsType.Tips, "Saved successfully", 3);
    //                    }
    //                    return;
    //                }
    //            } else if (postIndex == MAX_SET_SEL + 1) {
    //                // 不保存退出
    //                if (menuTips.result) {
    //                    OnButton_SetBack_Pressed();
    //                    return;
    //                }
    //            } else if (postIndex == MAX_SET_SEL + 2) {
    //                //默认值
    //                if (menuTips.result) {
    //                    OnButton_SetDefault_Pressed();
    //                    //OnButton_SetBack_Pressed();                        
    //                    if (Set.setVal.Language == (int)en_Language.Chinese) {
    //                        menuTips.Init(en_MenuTipsType.Tips, "恢复默认值成功", 3);
    //                    } else {
    //                        menuTips.Init(en_MenuTipsType.Tips, "Restore default successfully", 3);
    //                    }
    //                    return;
    //                }
    //            }
    //        }
    //    }

    //    if (Key.MENU_LeftPressed() || Key.KEYFJ_Menu_LeftPressed()) {
    //        if (selectSta == true) {
    //            if (postIndex < MAX_SET_SEL) {
    //                GameSetVal_Left();
    //            }
    //        } else {
    //            if (--postIndex <= 0)
    //                postIndex = MAX_SEL - 1;
    //            UpdateSetValPos();
    //            UpdateCursor();                
    //        }
    //    }
    //    if (Key.MENU_RightPressed() || Key.KEYFJ_Menu_RightPressed()) {
    //        if (selectSta == true) {
    //            if (postIndex < MAX_SET_SEL) {
    //                GameSetVal_Right();
    //            }
    //        } else {
    //            if (++postIndex >= MAX_SEL)
    //                postIndex = 1;                
    //            UpdateSetValPos();
    //            UpdateCursor();                
    //        }
    //    }
    //    if (Key.MENU_OkPressed() || Key.KEYFJ_Menu_OkPressed()) {
    //        if (postIndex < MAX_SET_SEL) {
    //            //设置参数
    //            if (selectSta == true) {
    //                UpdataCursor_Select(false);
    //            } else {                    
    //                UpdataCursor_Select(true);
    //            }
    //        }
    //        //三个按键
    //        else if (postIndex == MAX_SET_SEL + 0) {
    //            //保存并退出
    //            if (Set.setVal.Language == (int)en_Language.Chinese) {
    //                menuTips.Init(en_MenuTipsType.Select, "是否保存并退出?", 0);
    //            } else {
    //                menuTips.Init(en_MenuTipsType.Select, "Save and exit?", 0);
    //            }
    //        } else if (postIndex == MAX_SET_SEL + 1) {
    //            // 不保存退出
    //            if (Set.setVal.Language == (int)en_Language.Chinese) {
    //                menuTips.Init(en_MenuTipsType.Select, "是否不保存退出?", 0);
    //            } else {
    //                menuTips.Init(en_MenuTipsType.Select, "Exit without saving?", 0);
    //            }
    //        } else if (postIndex == MAX_SET_SEL + 2) {
    //            //默认值
    //            if (Set.setVal.Language == (int)en_Language.Chinese) {
    //                menuTips.Init(en_MenuTipsType.Select, "是否恢复默认值?", 0);
    //            } else {
    //                menuTips.Init(en_MenuTipsType.Select, "Restore defaults?", 0);
    //            }
    //        }
    //    }
    //}

    //// 更新选中标志
    //void UpdataCursor_Select(bool sta) {
    //    selectSta = sta;
    //    selectFlag.gameObject.SetActive(sta);
    //    if (postIndex < list_SetVal.Count) {
    //        selectFlag.transform.position = list_SetVal[postIndex].image_ValueBackG.transform.position;
    //        selectFlag.Init(list_SetVal[postIndex].image_ValueBackG.rectTransform.sizeDelta.x);
    //    }
    //}
    //// 更新光标坐标和大小
    //void UpdateCursor() {
    //    //设置项
    //    for (int i = 0; i < list_SetVal.Count; i++) {
    //        if (i == postIndex) {
    //            list_SetVal[i].SetSelect(true);
    //        } else {
    //            list_SetVal[i].SetSelect(false);
    //        }
    //    }
    //    //三个按键
    //    for (int i = 0; i < button.Length; i++) {
    //        if (i + MAX_SET_SEL == postIndex) {
    //            button[i].SetSelect(true);
    //        } else {
    //            button[i].SetSelect(false);
    //        }
    //    }
    //}
    ////
    //void UpdateSetValPos() {
    //    int id = postIndex;
    //    if (id >= MAX_SET_SEL)
    //        id = MAX_SET_SEL - 1;
    //    int page = id / MAX_SET_ONEPAGE;
    //    int pindex;

    //    for (int i = 0; i < MAX_SET_SEL; i++) {
    //        if ((i / MAX_SET_ONEPAGE) == page) {
    //            list_SetVal[i].gameObject.SetActive(true);
    //            //list_SetVal[i].transform.localPosition = new Vector3(0, 220 - (i % MAX_SET_ONEPAGE) * 70, 0);
    //            pindex = i % MAX_SET_ONEPAGE;
    //            list_SetVal[i].transform.localPosition = new Vector3((pindex / 5) * 360 - 180, 200 - (pindex % 5) * 60, 0);
    //        } else {
    //            list_SetVal[i].gameObject.SetActive(false);
    //        }
    //    }
    //}


    //// 1.进入初始化 : 放到中英文切换前
    //int oldOutMode;
    //public void GameStart() {
    //    oldOutMode = Set.setVal.OutMode;
    //    UpdateLanguage();
    //    UpdataGameSet();
    //    UpdateOrder();
    //    postIndex = MAX_SEL - 1;    //返回        
    //    UpdataCursor_Select(false);
    //    UpdateCursor();
    //    UpdateDescript();
    //}
    //// 2.更新显示中英文
    //public void UpdateLanguage() {
    //    if (Set.setVal.Language == (int)en_Language.Chinese) {
    //        //中文
    //        text_GameSet.text = "礼品设置";
    //        text_Tips.text = "以100局计算";
    //        button[0].GetComponentInChildren<Text>().text = "保存并退出";
    //        button[1].GetComponentInChildren<Text>().text = "不保存退出";
    //        button[2].GetComponentInChildren<Text>().text = "恢复默认值";
    //    } else {
    //        //英文
    //        text_GameSet.text = "Gift Set";
    //        text_Tips.text = "Calculated by 100 games";
    //        button[0].GetComponentInChildren<Text>().text = "Save Back";
    //        button[1].GetComponentInChildren<Text>().text = "Not Save Back";
    //        button[2].GetComponentInChildren<Text>().text = "Default";
    //    }
    //    SetVal_UpdataLanguage();
    //}


    //public void OnButton_SetDefault_Pressed() {
    //    Set.DefaultWinSet();
    //    GameStart();
    //}
    //public void OnButton_SetBack_Pressed() {
    //    menu.ChangeStatue(en_MenuStatue.MenuSta_SysSet);
    //}


    //// 右键设置++
    //void GameSetVal_Right() {
    //    if (postIndex < list_SetVal.Count) {
    //        if (CanAddWinCnt() == false)
    //            return;
    //        list_SetVal[postIndex].ValueAdd();
    //        CheckTotalWins();
    //        UpdateDescript();
    //        ModeCheck();
    //    }
    //}

    //// 左键设置--
    //void GameSetVal_Left() {
    //    if (postIndex < list_SetVal.Count) {
    //        if (CanDecWinCnt(postIndex) == false)
    //            return;
    //        list_SetVal[postIndex].ValueDec();
    //        CheckTotalWins();
    //        UpdateDescript();
    //        ModeCheck();
    //    }
    //}


    //// 需要修改的部分 --------------------------------------------------------------------------------------------------------------------------------------------
    //// 设置项名字
    ////const int SET_ID_SmallGameWinMode = 13;
    //const int SET_ID_WinCnt1 = 0;
    //const int SET_ID_WinCnt2 = 1;
    //const int SET_ID_WinCnt3 = 2;
    //const int SET_ID_WinCnt4 = 3;
    //const int SET_ID_WinCnt5 = 4;
    //const int SET_ID_BoxWinCnt1 = 5;
    //const int SET_ID_BoxWinCnt2 = 6;
    //const int SET_ID_BoxWinCnt3 = 7;
    //const int SET_ID_BoxWinGiftCnt1 = 8;
    //const int MAX_ALL_SET_SEL = 9;


    //// 退卡片模式ID列表
    //int[] tab_IdBuf_WinCard = {
    //    SET_ID_WinCnt1,
    //    SET_ID_WinCnt2,
    //    SET_ID_WinCnt3,
    //    SET_ID_WinCnt4,
    //    SET_ID_WinCnt5,
    //    SET_ID_BoxWinCnt1,
    //    SET_ID_BoxWinCnt2,
    //    SET_ID_BoxWinCnt3,
    //};
    //int[] tab_IdBuf_WinGift = {
    //    SET_ID_WinCnt1,
    //    SET_ID_WinCnt2,
    //    SET_ID_WinCnt3,
    //    SET_ID_WinCnt4,
    //    SET_ID_WinCnt5,
    //    SET_ID_BoxWinGiftCnt1,
    //};
    ////
    //void SetVal_Init() {
    //    //** 1.设置项初始化(代替在界面里直接真写) : 初始化全部可选择列表
    //    setVal[SET_ID_WinCnt1].SetValueInit(Set.SET_c_WinCnt);
    //    setVal[SET_ID_WinCnt2].SetValueInit(Set.SET_c_WinCnt);
    //    setVal[SET_ID_WinCnt3].SetValueInit(Set.SET_c_WinCnt);
    //    setVal[SET_ID_WinCnt4].SetValueInit(Set.SET_c_WinCnt);
    //    setVal[SET_ID_WinCnt5].SetValueInit(Set.SET_c_WinCnt);
    //    setVal[SET_ID_BoxWinCnt1].SetValueInit(Set.SET_c_BoxWinCnt);
    //    setVal[SET_ID_BoxWinCnt2].SetValueInit(Set.SET_c_BoxWinCnt);
    //    setVal[SET_ID_BoxWinCnt3].SetValueInit(Set.SET_c_BoxWinCnt);
    //    setVal[SET_ID_BoxWinGiftCnt1].SetValueInit(Set.SET_c_BoxWinCnt);
    //}
    //// 模式检测，当模式发生变化时，设置项目排列不同
    //void ModeCheck() {
    //    //if (list_SetVal[postIndex] == setVal[SET_ID_OutMode]) {
    //    //    UpdateOrder();
    //    //}
    //}
    //// 排序， 不同模式
    //void UpdateOrder() {
    //    int[] tab_IdBuf = tab_IdBuf_WinCard;
    //    if (Set.setVal.SmallGameWinMode == (int)en_SmallGameWinMode.Gift) {
    //        tab_IdBuf = tab_IdBuf_WinGift;
    //    }
    //    MAX_SET_SEL = tab_IdBuf.Length;
    //    MAX_SEL = MAX_SET_SEL + 3;
    //    // 当前要操作的选项
    //    list_SetVal.Clear();
    //    for (int i = 0; i < tab_IdBuf.Length; i++) {
    //        list_SetVal.Add(setVal[tab_IdBuf[i]]);
    //    }
    //    // 关闭所有选项
    //    for (int i = 0; i < setVal.Length; i++) {
    //        setVal[i].gameObject.SetActive(false);
    //    }
    //    // 重新排列
    //    UpdateSetValPos();
    //}
    //// 2.更新显示中英文
    //void SetVal_UpdataLanguage() {
    //    if (Set.setVal.Language == (int)en_Language.Chinese) {
    //        // 要显示的设置项 名称
    //        //setVal[SET_ID_SmallGameWinMode].SetSelectName("转盘奖励");
    //        // 娱乐
    //        setVal[SET_ID_WinCnt1].SetSelectName("1张卡局数");
    //        setVal[SET_ID_WinCnt2].SetSelectName("2张卡局数");
    //        setVal[SET_ID_WinCnt3].SetSelectName("3张卡局数");
    //        setVal[SET_ID_WinCnt4].SetSelectName("4张卡局数");
    //        setVal[SET_ID_WinCnt5].SetSelectName("5张卡局数");
    //        setVal[SET_ID_BoxWinCnt1].SetSelectName("转盘1张卡局数");
    //        setVal[SET_ID_BoxWinCnt2].SetSelectName("转盘2张卡局数");
    //        setVal[SET_ID_BoxWinCnt3].SetSelectName("转盘3张卡局数");
    //        setVal[SET_ID_BoxWinGiftCnt1].SetSelectName("转盘1扭蛋局数");

    //        // 设置项值:        
    //        //
    //        //setVal[SET_ID_SmallGameWinMode].SetValueName((int)en_SmallGameWinMode.Card, "卡片");
    //        //setVal[SET_ID_SmallGameWinMode].SetValueName((int)en_SmallGameWinMode.Gift, "扭蛋");

    //        // Descript:
    //        text_DescriptTitle[0].text = "100局出卡片数：";
    //        text_DescriptTitle[1].text = "100局出扭蛋数：";
    //        text_DescriptTitle[2].text = "游戏次数：";
    //        //text_DescriptTitle[3].text = "卡片概率：";
    //    } else {
    //        // 要显示的设置项 名称
    //        //setVal[SET_ID_SmallGameWinMode].SetSelectName("Small Game Win");
    //        setVal[SET_ID_WinCnt1].SetSelectName("1 card games");
    //        setVal[SET_ID_WinCnt2].SetSelectName("2 card games");
    //        setVal[SET_ID_WinCnt3].SetSelectName("3 card games");
    //        setVal[SET_ID_WinCnt4].SetSelectName("4 card games");
    //        setVal[SET_ID_WinCnt5].SetSelectName("5 card games");
    //        setVal[SET_ID_BoxWinCnt1].SetSelectName("Box 1 card games");
    //        setVal[SET_ID_BoxWinCnt2].SetSelectName("Box 2 card games");
    //        setVal[SET_ID_BoxWinCnt3].SetSelectName("Box 3 card games");
    //        setVal[SET_ID_BoxWinGiftCnt1].SetSelectName("Box 1 gift games");
    //        //
    //        //setVal[SET_ID_SmallGameWinMode].SetValueName((int)en_SmallGameWinMode.Card, "Card");
    //        //setVal[SET_ID_SmallGameWinMode].SetValueName((int)en_SmallGameWinMode.Gift, "Gift");

    //        // Descript:
    //        text_DescriptTitle[0].text = "Get cards per 100：";
    //        text_DescriptTitle[1].text = "Get gifts per 100：";
    //        text_DescriptTitle[2].text = "Number of games：";
    //        //text_DescriptTitle[3].text = "Card ratio：";
    //    }
    //}
    //bool CanAddWinCnt() {
    //    int winCnt = 0;
    //    //winCnt += setVal[SET_ID_WinCnt1].GetValue();
    //    winCnt += setVal[SET_ID_WinCnt2].GetValue();
    //    winCnt += setVal[SET_ID_WinCnt3].GetValue();
    //    winCnt += setVal[SET_ID_WinCnt4].GetValue();
    //    winCnt += setVal[SET_ID_WinCnt5].GetValue();
    //    if (Set.setVal.SmallGameWinMode == (int)en_SmallGameWinMode.Card) {
    //        winCnt += setVal[SET_ID_BoxWinCnt1].GetValue();
    //        winCnt += setVal[SET_ID_BoxWinCnt2].GetValue();
    //        winCnt += setVal[SET_ID_BoxWinCnt3].GetValue();
    //    } else {
    //        winCnt += setVal[SET_ID_BoxWinGiftCnt1].GetValue();
    //    }
    //    if (winCnt >= 100)
    //        return false;
    //    return true;
    //}
    //bool CanDecWinCnt(int index) {
    //    if (index >= list_SetVal.Count)
    //        return false;
    //    if (list_SetVal[index].GetValue() <= 0)
    //        return false;
    //    return true;
    //}
    //void CheckTotalWins() {
    //    int winCnt = 0;
    //    //winCnt += setVal[SET_ID_WinCnt1].GetValue();
    //    winCnt += setVal[SET_ID_WinCnt2].GetValue();
    //    winCnt += setVal[SET_ID_WinCnt3].GetValue();
    //    winCnt += setVal[SET_ID_WinCnt4].GetValue();
    //    winCnt += setVal[SET_ID_WinCnt5].GetValue();
    //    if (Set.setVal.SmallGameWinMode == (int)en_SmallGameWinMode.Card) {
    //        winCnt += setVal[SET_ID_BoxWinCnt1].GetValue();
    //        winCnt += setVal[SET_ID_BoxWinCnt2].GetValue();
    //        winCnt += setVal[SET_ID_BoxWinCnt3].GetValue();
    //    } else {
    //        winCnt += setVal[SET_ID_BoxWinGiftCnt1].GetValue();
    //    }
    //    if (winCnt > 100) {
    //        Set.DefaultWinSet();
    //        UpdataGameSet();
    //    } else {
    //        setVal[SET_ID_WinCnt1].UpdateValue(100 - winCnt);
    //    }
    //}
    //void UpdateDescript() {
    //    int winCard = 0;
    //    int winGift = 0;

    //    winCard += setVal[SET_ID_WinCnt1].GetValue() * 1;
    //    winCard += setVal[SET_ID_WinCnt2].GetValue() * 2;
    //    winCard += setVal[SET_ID_WinCnt3].GetValue() * 3;
    //    winCard += setVal[SET_ID_WinCnt4].GetValue() * 4;
    //    winCard += setVal[SET_ID_WinCnt5].GetValue() * 5;
    //    if (Set.setVal.SmallGameWinMode == (int)en_SmallGameWinMode.Card) {
    //        winCard += setVal[SET_ID_BoxWinCnt1].GetValue() * 1;
    //        winCard += setVal[SET_ID_BoxWinCnt2].GetValue() * 2;
    //        winCard += setVal[SET_ID_BoxWinCnt3].GetValue() * 3;
    //    } else {
    //        winGift += setVal[SET_ID_BoxWinGiftCnt1].GetValue() * 1;
    //    }

    //    // 100局出卡数：
    //    text_Descript[0].text = winCard.ToString();
    //    // 100局出扭蛋数：
    //    text_Descript[1].text = winGift.ToString();
    //    // 游戏次数：
    //    text_Descript[2].text = JL.ttl.ToString();
    //    // 卡片概率：
    //    //text_Descript[3].text = "Get cards per 100：";
    //}

    //// 更新设置内容 :
    //void UpdataGameSet() {
    //    //setVal[SET_ID_SmallGameWinMode].UpdateValue(Set.setVal.SmallGameWinMode);
    //    //
    //    setVal[SET_ID_WinCnt1].UpdateValue(Set.setVal.WinCnt1);
    //    setVal[SET_ID_WinCnt2].UpdateValue(Set.setVal.WinCnt2);
    //    setVal[SET_ID_WinCnt3].UpdateValue(Set.setVal.WinCnt3);
    //    setVal[SET_ID_WinCnt4].UpdateValue(Set.setVal.WinCnt4);
    //    setVal[SET_ID_WinCnt5].UpdateValue(Set.setVal.WinCnt5);
    //    setVal[SET_ID_BoxWinCnt1].UpdateValue(Set.setVal.BoxWinCnt1);
    //    setVal[SET_ID_BoxWinCnt2].UpdateValue(Set.setVal.BoxWinCnt2);
    //    setVal[SET_ID_BoxWinCnt3].UpdateValue(Set.setVal.BoxWinCnt3);
    //    setVal[SET_ID_BoxWinGiftCnt1].UpdateValue(Set.setVal.BoxWinGiftCnt1);
    //}

    ////按键 ------
    //public void OnButton_SetSave_Pressed() {
    //    // 计算数字是否超100
    //    int winCnt = 0;
    //    winCnt += setVal[SET_ID_WinCnt1].GetValue();
    //    winCnt += setVal[SET_ID_WinCnt2].GetValue();
    //    winCnt += setVal[SET_ID_WinCnt3].GetValue();
    //    winCnt += setVal[SET_ID_WinCnt4].GetValue();
    //    winCnt += setVal[SET_ID_WinCnt5].GetValue();
    //    if (Set.setVal.SmallGameWinMode == (int)en_SmallGameWinMode.Card) {
    //        winCnt += setVal[SET_ID_BoxWinCnt1].GetValue();
    //        winCnt += setVal[SET_ID_BoxWinCnt2].GetValue();
    //        winCnt += setVal[SET_ID_BoxWinCnt3].GetValue();
    //    } else {
    //        winCnt += setVal[SET_ID_BoxWinGiftCnt1].GetValue();
    //    }
    //    if (winCnt > 100) {
    //        // 超过100提示
    //        if (Set.setVal.Language == (int)en_Language.Chinese) {
    //            menuTips.Init(en_MenuTipsType.Tips, "总局数超过100，请重新设置", 1);
    //        } else {
    //            menuTips.Init(en_MenuTipsType.Tips, "Restore defaults?", 0);
    //        }
    //        return;
    //    }

    //    //保存设置
    //    //Set.setVal.SmallGameWinMode = setVal[SET_ID_SmallGameWinMode].GetValue();
    //    //
    //    Set.setVal.WinCnt1 = setVal[SET_ID_WinCnt1].GetValue();
    //    Set.setVal.WinCnt2 = setVal[SET_ID_WinCnt2].GetValue();
    //    Set.setVal.WinCnt3 = setVal[SET_ID_WinCnt3].GetValue();
    //    Set.setVal.WinCnt4 = setVal[SET_ID_WinCnt4].GetValue();
    //    Set.setVal.WinCnt5 = setVal[SET_ID_WinCnt5].GetValue();
    //    Set.setVal.BoxWinCnt1 = setVal[SET_ID_BoxWinCnt1].GetValue();
    //    Set.setVal.BoxWinCnt2 = setVal[SET_ID_BoxWinCnt2].GetValue();
    //    Set.setVal.BoxWinCnt3 = setVal[SET_ID_BoxWinCnt3].GetValue();
    //    Set.setVal.BoxWinGiftCnt1 = setVal[SET_ID_BoxWinGiftCnt1].GetValue();
    //    //
    //    Set.SaveWinSet();
    //}
}

