using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dna : MonoBehaviour {
	// 控件坐标

	// Code[2]
	float DNA_CODE_X = Menu.SCREEN_WIDTH / 2;				// 
	float DNA_CODE_Y = Screen.height * 2 / 5;			// 
	float DNA_CODE_DISTANCE_Y = Menu.SCREEN_WIDTH / 10;		// 

	//
	const int MAX_BITS = 6;
	const int MAX_SET_SEL = 2;
	const int MAX_SEL = MAX_SET_SEL + 1;

	// 要显示的内容(中英文切换)
	public GameObject planeMenu;

	public Text text_Dna;
	public Text cursor_Selected;		//光标选中标志
	public GameObject image_cursor;		//光标
	public GameObject button;			//
	//
	public GameObject accData;
	public GameObject[] NumEidt_Code = new GameObject[2];
	//
	AccValue accData_Script;
	NumEdit[] numEdit_Scritp = new NumEdit[2];

	// 传入的变量
	Menu menu;

	static int postIndex;				// 光标所在行
	static int bitIndex;				// 光标所在位(打码数字)
	static bool selectSta;				// 行光标选中状态
	static float selectBlenTime;		// 光标闪烁时间

	int statue;

    //
    public void Awake0(Menu mmenu) {
        //
        menu = mmenu;
        accData_Script = accData.GetComponent<AccValue> ();
		//
		numEdit_Scritp [0] = NumEidt_Code [0].GetComponent<NumEdit> ();
		numEdit_Scritp [1] = NumEidt_Code [1].GetComponent<NumEdit> ();

        Init();
    }

	// Use this for initialization
	void Init () {
		//
		Key.Clear ();
		//
		//
		numEdit_Scritp [0].SetName ("C1:");
		numEdit_Scritp [1].SetName ("C2:");
		//
		UpdateLanguage();
		//
		postIndex = 0;
		bitIndex = 0;
		selectSta = false;
		UpdataCursor_Select (false);
		//
		UpdateCursor ();
		//
		GameStart ();

		statue = 0;
	}

	// Update is called once per frame
	void Update () {
		//
		if (Main.statue != en_MainStatue.Game_98 || Menu.statue != en_MenuStatue.MenuSta_Dna)
			return;
        if (Game_Enc.MACHINE_NO >= 0) {
            menu.Enter();
            return;
        }
		//选中标志闪烁
		if (postIndex < MAX_SET_SEL && selectSta == true) {
			if (Time.time - selectBlenTime >= 0.3) {
				selectBlenTime = Time.time;
				if (cursor_Selected.enabled == true)
					cursor_Selected.enabled = false;
				else
					cursor_Selected.enabled = true;
			}
		}

        //按键
        /*		if (Key.MENU_UpPressed ()) {
                    if (selectSta == false) {
                        postIndex = (postIndex + MAX_SEL - 1) % MAX_SEL;
                        UpdateCursor ();
                    } else if(postIndex < MAX_SET_SEL) {
                        numEdit_Scritp [postIndex].BitValueAdd (bitIndex);
                    }
                }
                if (Key.MENU_DownPressed ()) {
                    if (selectSta == false) {
                        postIndex = (postIndex + 1) % MAX_SEL;
                        UpdateCursor ();
                    } else if(postIndex < MAX_SET_SEL) {
                        numEdit_Scritp [postIndex].BitValueDec (bitIndex);
                    }
                }

                if (Key.MENU_LeftPressed () || Key.KEYFJ_Menu_LeftPressed()) {
                    if (postIndex < MAX_SET_SEL && selectSta == true) {
                        bitIndex = (bitIndex + MAX_BITS - 1) % MAX_BITS; 
                        UpdateCursor ();
                    }
                }
                if (Key.MENU_RightPressed () || Key.KEYFJ_Menu_RightPressed()) {
                    if (postIndex < MAX_SET_SEL && selectSta == true) {
                        bitIndex = (bitIndex + 1) % MAX_BITS; 
                        UpdateCursor ();
                    }
                }
                if (Key.MENU_OkPressed () || Key.KEYFJ_Menu_OkPressed()) {			
                    if (postIndex < MAX_SET_SEL) {
                        //设置参数
                        if (selectSta == true) {
                            UpdataCursor_Select (false);
                        } else {
                            bitIndex = 0;
                            UpdataCursor_Select (true);
                            UpdateCursor ();
                        }
                    }
                    //三个按键
                    else if(postIndex == MAX_SET_SEL + 0){
                        //确定保存
                        OnButton_DnaOk_Pressed();
                    }
                }        
    */
        if (Key.MENU_LeftPressed() || Key.KEYFJ_Menu_LeftPressed()) {
            if (selectSta) {
                if (postIndex < MAX_SET_SEL) {
                    bitIndex = (bitIndex + 1) % MAX_BITS;
                    UpdateCursor();
                }
            } else {
                postIndex = (postIndex + MAX_SEL - 1) % MAX_SEL;
                UpdateCursor();
            }
        }
        if (Key.MENU_RightPressed() || Key.KEYFJ_Menu_RightPressed()) {
            if (selectSta) {
                if (postIndex < MAX_SET_SEL) {
                    numEdit_Scritp[postIndex].BitValueAdd(bitIndex);
                }
            } else {
                postIndex = (postIndex + 1) % MAX_SEL;
                UpdateCursor();
            }            
        }
        if (Key.MENU_OkPressed() || Key.KEYFJ_Menu_OkPressed()) {
            if (postIndex < MAX_SET_SEL) {
                //设置参数
                if (selectSta == true) {
                    UpdataCursor_Select(false);
                } else {
                    bitIndex = 0;
                    UpdataCursor_Select(true);
                    UpdateCursor();
                }
            }
            //三个按键
            else if (postIndex == MAX_SET_SEL + 0) {
                //确定保存
                OnButton_DnaOk_Pressed();
            }
        }
    }
	//
	void OnDestroy()
	{

	}
	// 更新选中标志
	void UpdataCursor_Select(bool sta)
	{
		cursor_Selected.enabled = sta;
		selectBlenTime = Time.time;
		selectSta = sta;
	}
	// 更新光标坐标和大小
	void UpdateCursor()
	{		
		if (postIndex < MAX_SET_SEL) {
			//C1, C2
			cursor_Selected.enabled = selectSta;
            image_cursor.GetComponent<RectTransform>().sizeDelta = NumEidt_Code[postIndex].GetComponent<RectTransform>().sizeDelta;
			image_cursor.transform.localPosition = NumEidt_Code [postIndex].transform.localPosition;
			cursor_Selected.transform.localPosition = NumEidt_Code[postIndex].transform.localPosition + new Vector3 (130 - 87 + bitIndex * 36, 8, 0);
		} else {			
			cursor_Selected.enabled = false;
            //三个按键
            image_cursor.GetComponent<RectTransform>().sizeDelta = button.GetComponent<RectTransform>().sizeDelta;
            image_cursor.transform.localPosition = button.transform.localPosition;
		}
	}

	// 1.进入初始化
	public void GameStart()
	{
        UpdateLanguage();
        postIndex = 0;
		UpdataCursor_Select (false);
		UpdateCursor (); 
		accData_Script.SetData (Game_Dna.GDNA_MyDna);
	}
	// 2.更新显示中英文
	public void UpdateLanguage()
	{
		if (Set.setVal.Language == (int)en_Language.Chinese) {
            //中文
            //DNA
            text_Dna.text = "DNA";
			accData_Script.SetName("DNA:");
			button.GetComponentInChildren<Text> ().text = "确认";
		} else {
			//英文
			//DNA
			text_Dna.text = "DNA";
			accData_Script.SetName("DNA:");
			button.GetComponentInChildren<Text> ().text = "OK";
		}
	}

	//按键 ------
	public void OnButton_DnaOk_Pressed()
	{
		ushort[] code = new ushort[2];

		code [0] = (ushort)numEdit_Scritp [0].GetValue ();
		code [1] = (ushort)numEdit_Scritp [1].GetValue ();
		if (Game_Dna.SetDna (code)) {
            menu.Enter();
		}
	}
}
