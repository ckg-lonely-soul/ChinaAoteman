using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
//using Fun;

public enum en_UpdateVideoSta {
    Idle = 0,
    UpdateVideo,
    DeleteVideo,
    Tishi,
}

enum en_OptioneWait {
    Idle = 0,
    Select,
    Optionning,
    Finish,
}

enum en_SelecetResult {
    Idle = 0,
    Yes,
    No,
}

public class Menu_UpdateVideo : MonoBehaviour {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    public const string srcPath = "F:/MP4/";
#elif UNITY_ANDROID
#if VER_A33
    public const string srcPath = "storage/usbhost1/";      // A33
#elif VER_3368
    //public const string srcPath = "mnt/external_sd/";      // 3368
    public const string srcPath = "mnt/media_rw/external_usb/";
#elif VER_H3
    public const string srcPath = "mnt/usbhost/Storage01/";      // H3
#else
    public const string srcPath = "mnt/usb_storage/USB_DISK1/udisk0/";
#endif    
#endif
    public Image image_Cursor;
    public GameObject tishi_Obj;
    public Text text_Title;
    public Text text_tishi;
    
    public Button[] button_Main;
    public Button[] button_Tishi;

    public en_UpdateVideoSta statue;
    en_OptioneWait optioneStatue;
    string diskPath;
    float runTime;
    int updateId;
    int updateNum;
    int updateFinished;
    //
    int selectId;

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
    }
    // Use this for initialization
    public void GameStart () {
        transform.localPosition = new Vector3(0, 0, 0);
        // 语言
        if(Set.setVal.Language == (int)en_Language.Chinese) {
            text_Title.text = "视频更新";
            button_Main[0].GetComponentInChildren<Text>().text = "更新视频";
            //button_Main[0].GetComponentInChildren<Text>().text = "更新礼品图片";
            button_Main[1].GetComponentInChildren<Text>().text = "删除视频文件";
            //button_Main[0].GetComponentInChildren<Text>().text = "删除礼品图片";
            button_Main[2].GetComponentInChildren<Text>().text = "返回";
            button_Tishi[0].GetComponentInChildren<Text>().text = "是";
            button_Tishi[1].GetComponentInChildren<Text>().text = "否";
        } else {
            text_Title.text = "Video Update";
            button_Main[0].GetComponentInChildren<Text>().text = "Update Video";
            //button_Main[0].GetComponentInChildren<Text>().text = "Update Gift Pic";
            button_Main[1].GetComponentInChildren<Text>().text = "Clear Video";
            //button_Main[0].GetComponentInChildren<Text>().text = "Clear Gift Pic";
            button_Main[2].GetComponentInChildren<Text>().text = "Back";
            button_Tishi[0].GetComponentInChildren<Text>().text = "YES";
            button_Tishi[1].GetComponentInChildren<Text>().text = "NO";
        }
        
        ChangeStatue(en_UpdateVideoSta.Idle);
	}
	
	// Update is called once per frame
	void Update () {
        if (menuTips.gameObject.activeSelf)
            return;
        if (passwordManager.gameObject.activeSelf)
            return;

        switch (statue) {
        case en_UpdateVideoSta.Idle:
            if (Key.MENU_LeftPressed() || Key.KEYFJ_Menu_LeftPressed()) {
                selectId = (selectId + 2) % 3;
                Update_Cursor();
            }
            if (Key.MENU_RightPressed() || Key.KEYFJ_Menu_RightPressed()) {
                selectId = (selectId + 1) % 3;
                Update_Cursor();
            }
            if (Key.MENU_OkPressed() || Key.KEYFJ_Menu_OkPressed()) {
                switch (selectId) {
                case 0:
                    OnClick_Update();
                    break;
                case 1:
                    OnClick_ClearVideo();
                    break;
                case 2:
                    OnClick_Back();
                    break;
                }
            }
            break;

        case en_UpdateVideoSta.UpdateVideo:
            switch (optioneStatue) {
            case en_OptioneWait.Idle:
                if (updateId < Game_VideoPlayer.MAX_VIDEO) {
                    string filename = Game_VideoPlayer.videoName + (updateId + 1).ToString("D4") + Game_VideoPlayer.videoExName;
                    string srcpath = diskPath + filename;
                    string targetpath = Application.persistentDataPath + "/" + filename;

                    if (File.Exists(srcpath)) {
                        updateNum++;
                        if (File.Exists(targetpath)) {
                            if (Set.setVal.Language == (int)en_Language.Chinese) {
                                text_tishi.text = filename + "已经存在，是否覆盖原有的视频?";
                            } else {
                                text_tishi.text = filename + "is already existed，does overwrite it?";
                            }                            
                            ChangeOptionStatue(en_OptioneWait.Select);
                        } else {
                            ChangeOptionStatue(en_OptioneWait.Optionning);
                        }
                    } else {
                        updateId++;
                    }
                } else {
                    if (updateNum == 0) {
                        if (Set.setVal.Language == (int)en_Language.Chinese) {
                            text_tishi.text = "没有要更新的视频文件";
                        } else {
                            text_tishi.text = "There is no video files to update";
                        }
                        ChangeStatue(en_UpdateVideoSta.Tishi);
                    } else if (updateFinished > 0) {
                        if (Set.setVal.Language == (int)en_Language.Chinese) {
                            text_tishi.text = "更新视频完成";
                        } else {
                            text_tishi.text = "video files update finish";
                        }
                        ChangeStatue(en_UpdateVideoSta.Tishi);
                    } else {
                        ChangeStatue(en_UpdateVideoSta.Idle);
                    }
                }
                break;
            case en_OptioneWait.Select:
                break;
            case en_OptioneWait.Optionning:
                if (updateId < Game_VideoPlayer.MAX_VIDEO) {
                    string filename = Game_VideoPlayer.videoName + (updateId + 1).ToString("D4") + Game_VideoPlayer.videoExName;
                    string srcpath = diskPath + filename;
                    string targetpath = Application.persistentDataPath + "/" + filename;
                    //
                    File.Copy(srcpath, targetpath, true);
                    updateFinished++;
                }
                ChangeOptionStatue(en_OptioneWait.Finish);
                break;
            case en_OptioneWait.Finish:
                updateId++;
                ChangeOptionStatue(en_OptioneWait.Idle);
                break;
            }
            break;

        case en_UpdateVideoSta.DeleteVideo:
            switch (optioneStatue) {
            case en_OptioneWait.Idle:
                int num = 0;
                string filename;
                for (int i = 0; i < Game_VideoPlayer.MAX_VIDEO; i++) {
                    filename = Application.persistentDataPath + "/" + Game_VideoPlayer.videoName + (i + 1).ToString("D4") + Game_VideoPlayer.videoExName;
                    if (File.Exists(filename)) {
                        num++;
                        break;
                    }
                }
                if (num > 0) {
                    if (Set.setVal.Language == (int)en_Language.Chinese) {
                        text_tishi.text = "是否确定删除视频文件?";
                    } else {
                        text_tishi.text = "Are you sure to delete the video files?";
                    }
                    ChangeOptionStatue(en_OptioneWait.Select);
                } else {
                    if (Set.setVal.Language == (int)en_Language.Chinese) {
                        text_tishi.text = "文件不存在";
                    } else {
                        text_tishi.text = "The file is not exist";
                    }
                    ChangeStatue(en_UpdateVideoSta.Tishi);
                }
                break;
            case en_OptioneWait.Select:
                break;
            case en_OptioneWait.Optionning:
                for (int i = 0; i < Game_VideoPlayer.MAX_VIDEO; i++) {
                    filename = Application.persistentDataPath + "/" + Game_VideoPlayer.videoName + (i + 1).ToString("D4") + Game_VideoPlayer.videoExName;
                    if (File.Exists(filename)) {
                        File.Delete(filename);
                    }
                }
                if (Set.setVal.Language == (int)en_Language.Chinese) {
                    text_tishi.text = "图片文件已清除";
                } else {
                    text_tishi.text = "The picture file is deleted";
                }
                ChangeStatue(en_UpdateVideoSta.Tishi);
                break;
            case en_OptioneWait.Finish:
                ChangeStatue(en_UpdateVideoSta.Idle);
                break;
            }
            break;

        case en_UpdateVideoSta.Tishi:
            runTime += Time.deltaTime;
            if (Key.MENU_OkPressed() || Key.KEYFJ_Menu_OkPressed() || runTime >= 5) {
                ChangeStatue(en_UpdateVideoSta.Idle);
            }
            break;
        }

        if (statue == en_UpdateVideoSta.UpdateVideo || statue == en_UpdateVideoSta.DeleteVideo) {
            if (optioneStatue == en_OptioneWait.Select) {
                if (Key.MENU_LeftPressed() || Key.KEYFJ_Menu_LeftPressed()) {
                    selectId = (selectId + 1) % 2;
                    Update_Cursor();
                }
                if (Key.MENU_RightPressed() || Key.KEYFJ_Menu_RightPressed()) {
                    selectId = (selectId + 1) % 2;
                    Update_Cursor();
                }
                if (Key.MENU_OkPressed() || Key.KEYFJ_Menu_OkPressed()) {
                    switch (selectId) {
                    case 0:
                        OnClick_CopyYES();
                        break;
                    case 1:
                        OnClick_CopyNO();
                        break;
                    }
                }
            }
        }
    }

    void ChangeStatue(en_UpdateVideoSta sta) {
        //print("UpdateVideoSta: " + sta);
        statue = sta;
        runTime = 0;
        updateId = 0;
        updateFinished = 0;
        Key.Clear();

        image_Cursor.gameObject.SetActive(false);
        ChangeOptionStatue(en_OptioneWait.Idle);
        switch (statue) {
        case en_UpdateVideoSta.Idle:
            image_Cursor.gameObject.SetActive(true);
            selectId = 2;
            Update_Cursor();
            break;

        case en_UpdateVideoSta.UpdateVideo:
            //case en_UpdateVideoSta.DeleteVideo:
#if (VER_H6) && !(UNITY_EDITOR || UNITY_STANDALONE_WIN)         // || VER_3368
            diskPath = GetDiskPath() + "/";
#else
            diskPath = srcPath;
#endif
            updateNum = 0;
            break;

        case en_UpdateVideoSta.Tishi:
            ShowTishi();
            break;
        }
    }
    void ChangeOptionStatue(en_OptioneWait sta) {
        optioneStatue = sta;

        tishi_Obj.SetActive(false);

        switch (optioneStatue) {
        case en_OptioneWait.Idle:
            break;

        case en_OptioneWait.Select:
            tishi_Obj.SetActive(true);
            button_Tishi[0].gameObject.SetActive(true);
            button_Tishi[1].gameObject.SetActive(true);
            image_Cursor.gameObject.SetActive(true);
            selectId = 0;
            Update_Cursor();
            optioneStatue = en_OptioneWait.Select;
            Key.Clear();
            break;

        case en_OptioneWait.Optionning:
            string filename = Game_VideoPlayer.videoName + (updateId + 1).ToString("D4") + Game_VideoPlayer.videoExName;
            if (statue == en_UpdateVideoSta.UpdateVideo) {
                // 提示：正在更新视频
                if (Set.setVal.Language == (int)en_Language.Chinese) {
                    text_tishi.text = filename + "\r\n正在更新视频...";
                } else {
                    text_tishi.text = filename + "\r\nThe video files is updating...";
                }
            } else if (statue == en_UpdateVideoSta.DeleteVideo) {
                // 提示：正在删除视频
                if (Set.setVal.Language == (int)en_Language.Chinese) {
                    text_tishi.text = "正在删除视频:" + filename + "...";
                } else {
                    text_tishi.text = "The video files is deleting " + filename + "...";
                }
            }
            ShowTishi();
            break;

        case en_OptioneWait.Finish:
            break;
        }
    }

    void ShowTishi() {
        image_Cursor.gameObject.SetActive(false);
        tishi_Obj.SetActive(true);
        button_Tishi[0].gameObject.SetActive(false);
        button_Tishi[1].gameObject.SetActive(false);
    }

    void Update_Cursor() {
        if(tishi_Obj.activeSelf == false) {
            image_Cursor.transform.SetParent(button_Main[selectId].transform);
            image_Cursor.rectTransform.sizeDelta = button_Main[selectId].GetComponent<RectTransform>().sizeDelta;
        } else {
            image_Cursor.transform.SetParent(button_Tishi[selectId].transform);
            image_Cursor.rectTransform.sizeDelta = button_Tishi[selectId].GetComponent<RectTransform>().sizeDelta;
        }
        image_Cursor.transform.SetSiblingIndex(0);
        image_Cursor.transform.localPosition = Vector3.zero;        
    }

    public void OnClick_TishiBackG() {
        if (statue == en_UpdateVideoSta.Tishi) {
            ChangeStatue(en_UpdateVideoSta.Idle);
        }
    }
    public void OnClick_Update() {
        if (statue == en_UpdateVideoSta.Idle) {
            ChangeStatue(en_UpdateVideoSta.UpdateVideo);
        }
    }


    public void OnClick_ClearVideo() {
        if (statue == en_UpdateVideoSta.Idle) {
            ChangeStatue(en_UpdateVideoSta.DeleteVideo);
        }
    }

    public void OnClick_CopyYES() {
        Main.Log("Yes");
        if (optioneStatue == en_OptioneWait.Select) {
            ChangeOptionStatue(en_OptioneWait.Optionning);
        }
        //if (tishi_Obj.activeSelf) {
        //    tishi_Obj.SetActive(false);
        //}
    }

    public void OnClick_CopyNO() {
        Main.Log("No");
        if (optioneStatue == en_OptioneWait.Select) {
            optioneStatue = en_OptioneWait.Finish;
        }
        tishi_Obj.SetActive(false);
    }

    public void OnClick_Back() {
        if(statue == en_UpdateVideoSta.Idle) {
            menu.ChangeStatue(en_MenuStatue.MenuSta_SysSet);
        }
    }

#if (VER_H6)        //  || VER_3368
    string GetDiskPath() {
        string path;
        try {
            AndroidJavaObject context = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass jc = new AndroidJavaClass("com.jm.jarpag.MainClass");
            path = jc.CallStatic<string>("GetStoragePath", context, true);
        } catch (System.Exception e) {
            path = "";
        }
        return path;
    }
#endif
}
