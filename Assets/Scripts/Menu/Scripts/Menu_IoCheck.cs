using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_IoCheck : MonoBehaviour {
    const int MAX_ACC_ONEPAGE = 6;
    int MAX_ACC;
    const int MAX_SET_SEL = 8;
    const int MAX_BUTTON = 1;
    const int MAX_SEL = MAX_SET_SEL + MAX_BUTTON;

    const int MAX_PAGE = Main.MAX_PLAYER + 1;

    // 要显示的内容(中英文切换)    
    public Text text_Title;    
    public Menu_Button[] button;
    public Menu_Button[] button_Out;        
    public AccValue[] accValue_In;
    public AccValue[] accValue_Adc;

    float[] runTime = new float[MAX_SET_SEL];
    int[] runCnt;
    //
    int postIndex;
    int page;
    float clearTotalTime;
    bool pressedLeft;
    bool pressedRight;
    bool pressedOK;
    //
    // 传入的变量
    Menu menu;
    Menu_PasswordManager passwordManager;
    Menu_Tips menuTips;
    //
    public void Awake0(Menu mmenu) {
        //
        menu = mmenu;
        passwordManager = menu.passwordManager;
        menuTips = menu.menuTips;

        runCnt = new int[accValue_In.Length];
    }

    // Update is called once per frame
    void Update() {
        //
        if (Menu.statue != en_MenuStatue.MenuSta_IoCheck)
            return;
        if (menuTips.gameObject.activeSelf)
            return;
        if (passwordManager.gameObject.activeSelf)
            return;

        // 输入检测
        if (Key.KEYFJ_CinPressed(0)) {
            runCnt[0]++;
            UpdateAccData(0);
        }
        if (Key.KEYFJ_CoutPressed(0)) {
            runCnt[1]++;
            UpdateAccData(1);
        }
        if (Key.KEYFJ_OkPressed(0)) {
            runCnt[2]++;
            UpdateAccData(2);
        }
        if (Key.KEYFJ_AddPressed(0)) {
            runCnt[3]++;
            UpdateAccData(3);
        }
        if (Key.KEYFJ_CinPressed(1)) {
            runCnt[4]++;
            UpdateAccData(4);
        }
        if (Key.KEYFJ_CoutPressed(1)) {
            runCnt[5]++;
            UpdateAccData(5);
        }
        if (Key.KEYFJ_OkPressed(1)) {
            runCnt[6]++;
            UpdateAccData(6);
        }
        if (Key.KEYFJ_AddPressed(1)) {
            runCnt[7]++;
            UpdateAccData(7);
        }
        if (Key.KEYFJ_ResetPressed(0)) {
            runCnt[8]++;
            UpdateAccData(8);
        }
        if (Key.MENU_OkPressed()) {
            runCnt[9]++;
            UpdateAccData(9);
            pressedOK = true;
        }
        if (Key.MENU_LeftPressed()) {
            runCnt[10]++;
            UpdateAccData(10);
            pressedLeft = true;
        }
        if (Key.MENU_RightPressed()) {
            runCnt[11]++;
            UpdateAccData(11);
            pressedRight = true;
        }

        for (int i = 0; i < accValue_Adc.Length && i < Main.MAX_PLAYER * 2; i++) {
            accValue_Adc[i].SetData(Key.adValue[i]);
        }

        for (int i = 0; i < runTime.Length; i++) {
            if (runTime[i] > 0) {
                runTime[i] -= Time.deltaTime;
                if (runTime[i] <= 0) {
                    switch (i) {
                    case 0: //SSR1
                        IO.Out_SSR(0, 0);
                        break;
                    case 1: //SSR2
                        IO.Out_SSR(1, 0);
                        break;
                    case 2: //LED1
                        IO.Out_LED(0, 0);
                        break;
                    case 3: //LED2
                        IO.Out_LED(1, 0);
                        break;
                    case 4: //LED3
                        IO.Out_LED(2, 0);
                        break;
                    case 5: //LED4
                        IO.Out_LED(3, 0);
                        break;
                    case 6: //B_LED1
                        IO.ButtonLED(0, 0);
                        break;
                    case 7: //B_LED2
                        IO.ButtonLED(1, 0);
                        break;
                    }
                    button_Out[i].GetComponent<Image>().color = Color.white;
                }
            }
        }

        if (Key.MENU_LeftPressed() || Key.KEYFJ_Menu_LeftPressed() || pressedLeft) {
            pressedLeft = false;
            postIndex = (postIndex + MAX_SEL - 1) % MAX_SEL;
            UpdateCursor();
        }
        if (Key.MENU_RightPressed() || Key.KEYFJ_Menu_RightPressed() || pressedRight) {
            pressedRight = false;
            postIndex = (postIndex + 1) % MAX_SEL;
            UpdateCursor();
        }
        if (Key.MENU_OkPressed() || Key.KEYFJ_Menu_OkPressed() || pressedOK) {
            pressedOK = false;
            switch (postIndex) {
            case 0: //SSR1
                IO.Out_SSR(0, 1);
                break;
            case 1: //SSR2
                IO.Out_SSR(1, 1);
                break;
            case 2: //LED1
                IO.Out_LED(0, 1);
                break;
            case 3: //LED2
                IO.Out_LED(1, 1);
                break;
            case 4: //LED3
                IO.Out_LED(2, 1);
                break;
            case 5: //LED4
                IO.Out_LED(3, 1);
                break;
            case 6: //B_LED1
                IO.ButtonLED(0, 1);
                break;
            case 7: //B_LED2
                IO.ButtonLED(1, 1);
                break;

            case MAX_SET_SEL:
                //返回
                OnButton_AccBack_Pressed();
                break;
            }
            if (postIndex < MAX_SET_SEL) {
                runTime[postIndex] = 0.6f;
                button_Out[postIndex].image_BackG.color = Color.blue;
            }
        }
    }

    // 更新光标坐标和大小
    void UpdateCursor() {
        //设置项
        for (int i = 0; i < button_Out.Length; i++) {
            if (i == postIndex) {
                button_Out[i].SetSelect(true);
            } else {
                button_Out[i].SetSelect(false);
            }
        }
        //三个按键
        for (int i = 0; i < button.Length; i++) {
            if (i + MAX_SET_SEL == postIndex) {
                button[i].SetSelect(true);
            } else {
                button[i].SetSelect(false);
            }
        }
    }

    // 1.进入初始化
    public void GameStart() {
        UpdateLanguage();
        //
        pressedLeft = false;
        pressedRight = false;
        pressedOK = false;
        page = 0;
        postIndex = MAX_SET_SEL;		// 下一页
        for (int i = 0; i < runCnt.Length; i++) {
            runCnt[i] = 0;
            UpdateAccData(i);
        }
      
        for (int i = 0; i < button_Out.Length; i++) {
            button_Out[i].image_BackG.color = Color.white;
        }
        clearTotalTime = 0;
        UpdateCursor();
    }


    // 2.更新显示中英文
    public void UpdateLanguage() {
        if (Set.setVal.Language == (int)en_Language.Chinese) {
            //中文
            //按键
            text_Title.text = "IO检测";
            button[0].GetComponentInChildren<Text>().text = "返回";
        } else {
            //英文
            //按键
            text_Title.text = "IO Check";
            button[0].GetComponentInChildren<Text>().text = "Back";
        }
        AccVal_UpdataLanguage();
    }

    //按键 ------
    public void OnButton_AccBack_Pressed() {
        IO.Init();
        menu.ChangeStatue(en_MenuStatue.MenuSta_SysSet);
    }

    // 需要修改的内容 -------------------------------------------------------------------------------------------------------

    // 2.更新显示中英文
    void AccVal_UpdataLanguage() {
        if (Set.setVal.Language == (int)en_Language.Chinese) {
            //中文
            //账目

        } else {
            //英文
            //账目

        }
    }

    // 更新账目
    void UpdateAccData(int no) {
        if (no >= accValue_In.Length)
            return;
        accValue_In[no].SetData(runCnt[no]);
    }
}
