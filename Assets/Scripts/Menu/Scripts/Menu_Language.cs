using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Language : MonoBehaviour {
	const int MAX_SET_SEL = 1;
	const int MAX_SEL = MAX_SET_SEL + 3;

	// 要显示的内容(中英文切换)	
	public Text text_Title;    	
    public SetValue setVal;
    public Menu_SelectFlag selectFlag;  //光标选中标志
    public Menu_Button[] button;
    
    //
    static int postIndex;
	static bool selectSta;

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
        //
        setVal.SetValueInit(Set.SET_c_Language);
    }

	// Update is called once per frame
	void Update () {
		if (Menu.statue != en_MenuStatue.MenuSta_Language)
			return;
        if (menuTips.gameObject.activeSelf)
            return;
        if (passwordManager.gameObject.activeSelf)
            return;

        /*	if (Key.MENU_UpPressed ()) {
                if (selectSta == false) {
                    if (postIndex >= MAX_SET_SEL) {
                        postIndex = MAX_SET_SEL - 1;
                    } else {
                        postIndex = (postIndex + MAX_SEL - 1) % MAX_SEL;
                    }
                    UpdateCursor ();
                }
            }
            if (Key.MENU_DownPressed ()) {
                if (selectSta == false) {
                    postIndex = (postIndex + 1) % MAX_SEL;
                    UpdateCursor ();
                }
            } */
        if (Key.MENU_LeftPressed () || Key.KEYFJ_Menu_LeftPressed()) {
			if (selectSta == true) {
				if (postIndex < MAX_SET_SEL) {
                    setVal.ValueDec ();
				}
			} else {
				postIndex = (postIndex + MAX_SEL - 1) % MAX_SEL;
				UpdateCursor ();
			}
		}
		if (Key.MENU_RightPressed () || Key.KEYFJ_Menu_RightPressed()) {
			if (selectSta == true) {
				if (postIndex < MAX_SET_SEL) {
					setVal.ValueAdd ();
				}
			} else {
				postIndex = (postIndex + 1) % MAX_SEL;
				UpdateCursor ();
			}
		}
		if (Key.MENU_OkPressed () || Key.KEYFJ_Menu_OkPressed()) {
			if (postIndex < MAX_SET_SEL) {
				//设置参数
				if (selectSta == true) {
					UpdataCursor_Select (false);
				} else {
					UpdataCursor_Select (true);
				}
			}
			//三个按键
			else if(postIndex == MAX_SET_SEL + 0){
				//保存
				OnButton_LanguageSave_Pressed();
				postIndex = MAX_SET_SEL + 2;
				UpdateCursor ();
			}
			else if(postIndex == MAX_SET_SEL + 1){
				//默认值
				OnButton_LanguageDefault_Pressed();
				postIndex = MAX_SET_SEL + 2;
				UpdateCursor ();
			}
			else if(postIndex == MAX_SET_SEL + 2){
                //回返               
                OnButton_LanguageBack_Pressed();
			}
		}
	}

    // 更新选中标志
    void UpdataCursor_Select(bool sta) {
        selectSta = sta;
        selectFlag.gameObject.SetActive(sta);
        if (postIndex < MAX_SET_SEL) {
            selectFlag.transform.position = setVal.image_ValueBackG.transform.position;
            selectFlag.Init(setVal.image_ValueBackG.rectTransform.sizeDelta.x);
        }
    }

    // 更新光标坐标和大小
    void UpdateCursor() {
        //设置项
        for (int i = 0; i < MAX_SET_SEL; i++) {
            if (i == postIndex) {
                setVal.SetSelect(true);
            } else {
                setVal.SetSelect(false);
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
	public void GameStart()
	{
        UpdateLanguage();

		postIndex = MAX_SEL - 1;	//返回
		UpdataCursor_Select(false);
		UpdateCursor ();

	}
	// 2.更新显示中英文
	public void UpdateLanguage()
	{
		if (Set.setVal.Language == (int)en_Language.Chinese) {
            //中文
            //语言            
            text_Title.text = "语言";
			setVal.SetSelectName ("语言");
            setVal.SetValueName (0, "中文");
            setVal.SetValueName (1, "英文");
			button [0].GetComponentInChildren<Text> ().text = "保存";
			button [1].GetComponentInChildren<Text> ().text = "默认值";
			button [2].GetComponentInChildren<Text> ().text = "返回";
		} else {
            //英文
            //语言            
            text_Title.text = "Language";
            setVal.SetSelectName ("Language");
            setVal.SetValueName (0, "Chinese");
            setVal.SetValueName (1, "English");
			button [0].GetComponentInChildren<Text> ().text = "Save";
			button [1].GetComponentInChildren<Text> ().text = "Default";
			button [2].GetComponentInChildren<Text> ().text = "Back";
		}
        UpdataSetValue();
    }

	// 更新设置内容
	void UpdataSetValue()
	{
        setVal.UpdateValue(Set.setVal.Language);
	}

	//按键 ------
	public void OnButton_LanguageSave_Pressed()
	{        
        int language = setVal.GetValue ();
        
		if (Set.setVal.Language == language)
			return;
		Set.setVal.Language = language;
		Set.SaveLanguage ();
        UpdateLanguage();
	}
	public void OnButton_LanguageDefault_Pressed()
	{
		Set.DefaultLanguage ();
        UpdateLanguage();
	}
	public void OnButton_LanguageBack_Pressed()
	{
		menu.ChangeStatue (en_MenuStatue.MenuSta_SysSet);
	}
}
