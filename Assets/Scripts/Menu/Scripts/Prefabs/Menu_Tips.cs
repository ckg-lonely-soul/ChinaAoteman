using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum en_MenuTipsType {
    Tips = 0,
    Select,
}

public class Menu_Tips : MonoBehaviour {
    const int MAX_SEL = 2;

    public Text text_Tips;
    public Text text_Time;
    public Menu_Button[] button;
    //
    public en_MenuTipsType tipsType;
    public bool result;
    public bool waitTips;
    int remainTime;
    float runTime;
    int postIndex;

    public void UpdataLanguage() {
        if (Set.setVal.Language == (int)en_Language.Chinese) {
            //中文                        
            button[0].GetComponentInChildren<Text>().text = "是";
            button[1].GetComponentInChildren<Text>().text = "否";            
        } else {
            //英文            
            button[0].GetComponentInChildren<Text>().text = "Yes";
            button[1].GetComponentInChildren<Text>().text = "No";
        }        
    }
    // Use this for initialization
    public void Init (en_MenuTipsType type, string tips, int time) {
        gameObject.SetActive(true);
        UpdataLanguage();
        tipsType = type;
        runTime = time + 0.9f;
        remainTime = time;
        postIndex = 0;
        result = false;
        waitTips = true;
        text_Time.gameObject.SetActive(false);
        text_Tips.text = tips;
        if (type == en_MenuTipsType.Select) {
            for (int i = 0; i < button.Length; i++) {
                button[i].gameObject.SetActive(true);
            }
            Update_SelectPos();
        } else {
            for (int i = 0; i < button.Length; i++) {
                button[i].gameObject.SetActive(false);
            }
            if (time > 0) {
                text_Time.gameObject.SetActive(true);
                Update_Time(remainTime);
            }
        }
	}

    void Exit() {
        gameObject.SetActive(false);
        Key.Clear();
    }
	
	// Update is called once per frame
	void Update () {
        if (tipsType == en_MenuTipsType.Select) {
            if (Key.MENU_LeftPressed() || Key.KEYFJ_Menu_LeftPressed()) {
                postIndex = (postIndex + MAX_SEL - 1) % MAX_SEL;
                Update_SelectPos();
            }
            if (Key.MENU_RightPressed() || Key.KEYFJ_Menu_RightPressed()) {
                postIndex = (postIndex + MAX_SEL - 1) % MAX_SEL;
                Update_SelectPos();
            }
            if (Key.MENU_OkPressed() || Key.KEYFJ_Menu_OkPressed()) {
                if (postIndex == 0) {
                    // YES
                    result = true;
                } else {
                    // NO
                    result = false;
                }
                Exit();
            }
        } else {
            if (runTime > 0) {
                runTime -= Time.deltaTime;
                if (remainTime != (int)runTime) {
                    remainTime = (int)runTime;
                    Update_Time(remainTime);
                }
            } else {
                Exit();
            }
        }
	}

    void Update_SelectPos() {
        for (int i = 0; i < button.Length; i++) {
            if (i == postIndex) {
                button[i].SetSelect(true);
            } else {
                button[i].SetSelect(false);
            }
        }
    }

    void Update_Time(int value) {
        text_Time.text = "(" + value + ")";
    }
}
