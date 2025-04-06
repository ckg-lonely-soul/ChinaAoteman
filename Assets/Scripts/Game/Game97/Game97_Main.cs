using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum en_Game97_Sta
{
    Idle = 0,
    GameSelect,
    GameOver,
    TheEnd,
}
//选择场景
public class Game97_Main : MonoBehaviour
{
    public Game97_Idle gameIdle;
    public Game97_GameSel gameSel;
    public Game97_GameOver gameOver;
    public Image image_BackG;
    public Image image_BG; 
    public Image image_Cover;
    public Game00_PlayerUI[] playerUI;
    public AudioSource audioSource_BackG;//待机界面音乐
    public AudioClip m_pBackNusic = null;
    public GameObject qrCode_Obj;
    public GameObject updateFile_Obj;//更新视频ui
    public Text text_UpdateFileTips;

    Main main;
    en_Game97_Sta statue;
    float runTime;
    float time;
    int showTime;
    int scenceNum;
    int playerNum;
    public const int OpenSceneNum = 6;//展示六个场景
    readonly string[] strLanaguge = { "cn", "en" };
    int lanauage = -1;
    public void Awake0(Main mainf)
    {
        main = mainf;
        gameIdle.Awake0(this);
        if (lanauage != Set.setVal.Language)
        {
            lanauage = Set.setVal.Language;
        }
        if (Main.COMPANY_NUM == 4)
        {
            GameObject prefab = Resources.Load<GameObject>("Company_" + Main.COMPANY_NUM.ToString("D2") + "/Prefab/Game97/GameSelect");
            GameObject obj = Instantiate(prefab, gameSel.transform.parent);
            obj.transform.localPosition = Vector3.zero;
            Destroy(gameSel.gameObject);
            obj.transform.SetSiblingIndex(1);
            gameSel = obj.GetComponent<Game97_GameSel>();
        }
        else if (Main.COMPANY_NUM == 10)
        {
            image_BackG.sprite = Resources.Load<Sprite>("Company_" + Main.COMPANY_NUM.ToString("D2") + "/Pic/Game97/Idle/BackG");
        }

        gameSel.Awake0(this);
        gameOver.Awake0(this);

        for (int i = 0; i < playerUI.Length; i++)
        {
            //Debug.Log ("流"+2+i);

            playerUI[i].Awake0(null);
            playerUI[i].gameOut.Init(i);
        }
    }

    public bool IsAllGamePass()
    {
        for (int i = 0; i < Main.gamePassed.Length; i++)
        {
            if (Set.GameSelect[i] == 0)
                continue;
            if (Main.gamePassed[i] == false)
            {
                return false;
            }
        }
        return true;
    }
    void OnEnable()
    {
        isChangeVideo = false;
        updateIndex = 0;
        updateFile_Obj.SetActive(false);
        if (qrCode_Obj != null)
        {
            if (Set.setVal.ShowQrCode == 0)
            {
                qrCode_Obj.SetActive(false);
            }
            else
            {
                qrCode_Obj.SetActive(true);
            }
        }
    }
    public void GameStart()
    {
        playerNum = Main.MAX_PLAYER;
        // playerUI[0].GameStart(0);
        if (Set.setVal.PlayerMode == (int)en_PlayerMode.One)
        {
            playerUI[1].gameObject.SetActive(false);
            playerNum = 1;
        }
        else if (Set.setVal.PlayerMode == (int)en_PlayerMode.Two)
        {
            playerUI[1].gameObject.SetActive(true);
        }

#if FPS_TEST
        FjData.g_Fj[0].Coins = 99;
        FjData.g_Fj[1].Coins = 99;
#endif
        if (Main.isRestart)
        {
            Main.isRestart = false;
            Main.gameOver = false;
            for (int i = 0; i < Main.gamePassed.Length; i++)
            {
                Main.gamePassed[i] = false;
            }
            // 玩家数据清零
            PlayerDataClear();
            //
            PAction.Init();
            for (int i = 0; i < Main.errorStatue.Length; i++)
            {
                Main.errorStatue[i] = 0;
            }
            //  Debug.Log("Update");
#if !IO_LOCAL//海燕一体机，5+1，5个游戏调用这个方法会有死循环
            UpdateBuffScence();
#endif
        }
        IO.Init();



        //#if UNITY_EDITOR
        //        for (int i = 0; i < Main.gamePassed.Length; i++)
        //        {
        //            Main.gamePassed[i] = true;
        //        }
        //#endif

        if (Main.gameOver || IsAllGamePass())
        {
            Main.gameOver = false;
            ChangeStatue(en_Game97_Sta.GameOver);
        }
        else if (Main.IsPlayerPlaying())
        {
            ChangeStatue(en_Game97_Sta.GameSelect);
        }
        else
        {
            if (Main.COMPANY_NUM == 1)
            {

                // 枪神
                //存土
                for (int i = 0; i < playerUI.Length; i++)
                {
                    playerUI[i].GameStart(i);
                    playerUI[i].image_Cursor.gameObject.SetActive(false);
                    playerUI[i].SetCheckPleaseCoinIn(true);
                }
                ChangeStatue(en_Game97_Sta.GameSelect);
            }
            else
            {
                ChangeStatue(en_Game97_Sta.Idle);
                if (Set.setVal.DeskMusic == 0 && IsCanPlay() == false)
                {
                    audioSource_BackG.Stop();
                }
                else
                {
                    audioSource_BackG.Play();
                }
            }
        }
        for (int i = 0; i < Main.MAX_PLAYER; i++)
        {
            if (FjData.g_Fj[i].Playing)
            {//每个玩家如果在游戏就不显示
                playerUI[i].SetCheckPleaseCoinIn(false);
            }
            else
            {
                playerUI[i].SetCheckPleaseCoinIn(true);
            }
        }
    }
    string diskPath;
    bool isChangeVideo;
    float videoCheckTime = 0;
    float updateTipsTime = 0;
    int updateIndex = 0;
    void ChangeVideoStart(int index)
    {
        isChangeVideo = true;
        updateIndex = index;
        updateFile_Obj.SetActive(true);
        updateTipsTime = 0;
        if (Set.setVal.Language == (int)en_Language.Chinese)
        {
            text_UpdateFileTips.text = "正在更新中文或英文视频: " + (updateIndex + 1).ToString("D4") + ".mp4";
        }
        else
        {
            text_UpdateFileTips.text = "Updating Chinese or English vedio: " + (updateIndex + 1).ToString("D4") + ".mp4";
        }
    }
    void UpdateBuffScence()
    {
        //for (int i = 0; i < 20; i++)
        //{
        //    Debug.Log(i+" : "+Set.GameSelect[i]);
        //}
        scenceNum = 0;
        // Main.buffScenes = new int[OpenSceneNum];
        for (int i = 0; i < Main.tab_GameId.Length; i++)
        {
            //Debug.Log (Set.GameSelect[Main.tab_GameId[i]]);
            if (Set.GameSelect[i] == 1 && scenceNum <= 5)//展示出来的是设置的六个场景
            {
                Main.buffScenes[scenceNum] = Main.tab_GameId[i];
                scenceNum++;
            }
        }
        if (scenceNum < OpenSceneNum)
        {
            //if (scenceNum == 0)
            //{
            //    int temp = Random.Range(0, tab_GameId.Length);
            //    while (isContains(Main.buffScenes, tab_GameId[temp]))
            //        temp = Random.Range(0, tab_GameId.Length);
            //    Main.buffScenes[scenceNum] = tab_GameId[temp];
            //}
            //if (scenceNum > 0)
            //{
            //    //if (Main.buffScenes[1] != 3)
            //    //{
            //    //    int no = Main.buffScenes[1];
            //    //    Main.buffScenes[1] = 3;
            //    //    Main.buffScenes[scenceNum] = no;
            //    //    scenceNum++;
            //    //}
            //    //if (scenceNum < 5 && Main.buffScenes[2] != 4)
            //    //{
            //    //    int no = Main.buffScenes[2];
            //    //    Main.buffScenes[2] = 4;
            //    //    Main.buffScenes[scenceNum] = no;
            //    //    scenceNum++;
            //    //}
            //}
            //else
            //{
            //Main.buffScenes[1] = 3;
            //Main.buffScenes[2] = 4;
            /* if (GetIndex(Main.buffScenes, scenceNum, 3) == -1)
             {
                 Main.buffScenes[scenceNum] = 3;
                 scenceNum++;
             }
             if (GetIndex(Main.buffScenes, scenceNum, 4) == -1 && scenceNum < 5)
             {
                 Main.buffScenes[scenceNum] = 4;
                 scenceNum++;
             }xiugai*/
            for (int i = scenceNum; i < OpenSceneNum; i++)
            {
                int temp = Random.Range(0, Main.tab_GameId.Length);
                while (GetIndex(Main.buffScenes, i, Main.tab_GameId[temp]) != -1)
                    temp = Random.Range(0, Main.tab_GameId.Length);
                Main.buffScenes[i] = Main.tab_GameId[temp];
                scenceNum++;

            }
        }
        int no = GetIndex(Main.buffScenes, scenceNum, 3);
        if (no != 1 && no >= 0)
        {
            int temp = Main.buffScenes[1];
            Main.buffScenes[1] = 3;
            Main.buffScenes[no] = temp;
        }

        no = GetIndex(Main.buffScenes, scenceNum, 4);
        if (no != 2 && no >= 0)
        {
            int temp = Main.buffScenes[2];
            Main.buffScenes[2] = 4;
            Main.buffScenes[no] = temp;
        }
        //for (int i = 0; i < Main.buffScenes.Length; i++)
        //{
        //    Debug.Log(Main.buffScenes[i]);
        //}
    }
    int GetIndex(int[] array, int len, int value)
    {
        for (int i = 0; i < array.Length && i < len; i++)
        {
            if (array[i] == value)
                return i;
        }
        return -1;
    }
    //bool isContains(int[] array, int value)
    //{
    //    for (int i = 0; i < array.Length; i++)
    //    {
    //        if (array[i] == value)
    //            return true;
    //    }
    //    return false;
    //}

    void SortBuffScence()
    {
        //for (int i = 1; i < length; i++)
        //{

        //}
    }
    void Update()
    {
        UDiskUpdateVideo();
        Upate_CursorPos();

        switch (statue)
        {
            case en_Game97_Sta.Idle:
                if (runTime < 2)
                {
                    runTime += Time.deltaTime;
                    break;
                }
                for (int i = 0; i < playerNum; i++)
                {
                    if (Main.IsCanGamePlay(i))
                    {
                        ChangeStatue(en_Game97_Sta.GameSelect);
                    }
                }
                break;

            case en_Game97_Sta.GameSelect:

                break;

            case en_Game97_Sta.TheEnd:

                break;
        }
    }

    public void ChangeStatue(en_Game97_Sta sta)
    {

        Key.Clear();
        statue = sta;
        runTime = 0;

        gameIdle.gameObject.SetActive(false);
        gameSel.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(false);
        //		image_Cover.gameObject.SetActive(true);
        //Debug.Log (statue);
        switch (statue)
        {
            case en_Game97_Sta.Idle:
                gameIdle.gameObject.SetActive(true);
                gameIdle.GameStart();
                for (int i = 0; i < playerUI.Length; i++)
                {
                    playerUI[i].GameStart(i);
                    playerUI[i].image_Cursor.gameObject.SetActive(false);
                    playerUI[i].SetCheckPleaseCoinIn(true);
                }
                break;

            case en_Game97_Sta.GameSelect:
                gameSel.gameObject.SetActive(true);
                gameSel.GameStart();
                if (Set.setVal.InOutMode == (int)en_InOutMode.OneInOneOut)//单投单退投币数位置
                {
                    playerUI[0].coinIn.transform.localPosition = new Vector3(-150f, -320f, 0);
                }
                Main.isSelectLevel = true;
                break;

            case en_Game97_Sta.GameOver:
                gameOver.gameObject.SetActive(true);
                gameOver.GameStart();
                for (int i = 0; i < Main.gamePassed.Length; i++)
                {
                    Main.gamePassed[i] = false;
                }
                PlayerDataClear();
                break;
        }
    }

    public void EnterGame(int gameno)//选的场景
    {
        for (int i = 0; i < Main.MAX_PLAYER; i++)
        {
            IO.GunRunStop(i);
        }
        //gameno = 03;
        if(gameno == 96)
        {
            main.ChangeScene(en_MainStatue.Game_96);
        }
        else
        {
            //main.ChangeScene((en_MainStatue)gameno);
            main.ChangeScene((en_MainStatue)0);
        }
        if (Set.setVal.InOutMode == (int)en_InOutMode.OneInOneOut)//单投单退投币数位置
        {
            playerUI[0].coinIn.transform.localPosition = new Vector3(320f, -320f, 0);
        }
    }

    public void Upate_CursorPos()
    {
        for (int i = 0; i < playerUI.Length; i++)
        {
#if GUN_HW
            playerUI[i].image_Cursor.transform.position = new Vector3(Key.GetAd_LeftRight(i) * Screen.width, Key.GetAd_UpDown(i) * Screen.height, 0);
#else
            playerUI[i].image_Cursor.transform.localPosition = new Vector3(Key.cursorPos_Left[i] + Key.GetAd_LeftRight(i) * Key.screen_Width[i], Key.cursorPos_Down[i] + Key.GetAd_UpDown(i) * Screen.height, 0);
#endif
        }
    }
    public static void PlayerDataClear()
    {
        for (int i = 0; i < Main.MAX_PLAYER; i++)
        {
            FjData.g_Fj[i].Scores = 0;
            FjData.g_Fj[i].JsScores = 0;
            FjData.g_Fj[i].Blood = 0;
            FjData.g_Fj[i].GameTime = 0;
            FjData.g_Fj[i].Playing = false;
            FjData.g_Fj[i].Played = false;
        }
    }

    public bool IsCanPlay()
    {

        for (int i = 0; i < Main.MAX_PLAYER; i++)
        {
            if (Main.IsCanGamePlay(i))
                return true;
            if (FjData.g_Fj[i].Blood > 0)
                return true;
        }
        return false;
    }

    public void UDiskUpdateVideo()//u盘自动更新
    {
        if (isChangeVideo)
        {
#if (VER_H6) && !(UNITY_EDITOR || UNITY_STANDALONE_WIN)         // || VER_3368
            diskPath = Menu_UpdateVideo.GetDiskPath() + "/";
#else
            diskPath = Menu_UpdateVideo.srcPath;
#endif
            // 视频
            if (updateIndex < Main.MAX_USER_VIDEO)
            {
                string filename = "CN" + (updateIndex + 1).ToString("D4") + ".mp4";
                string filename1 = "EN" + (updateIndex + 1).ToString("D4") + ".mp4";
                string srcpath = diskPath + filename;
                string srcpath1 = diskPath + filename1;
                if (File.Exists(srcpath))
                {
                    string targetpath = Application.persistentDataPath + "/" + filename;
                    File.Copy(srcpath, targetpath, true);
                }
                if (File.Exists(srcpath1))
                {
                    string targetpath1 = Application.persistentDataPath + "/" + filename1;
                    File.Copy(srcpath1, targetpath1, true);
                }
                videoCheckTime = 10;
            }
            else
            {
                videoCheckTime = 0;
            }
            //for (int i = 0; i < MyDefine.MAX_USER_VIDEO; i++) {
            //    string filename = (i + 1).ToString("D4") + ".mp4";
            //    string srcpath = diskPath + filename;
            //    if (File.Exists(srcpath)) {
            //        string targetpath = Application.persistentDataPath + "/" + filename;
            //        File.Copy(srcpath, targetpath, true);
            //    }
            //}
            //// 声音
            //for (int i = 0; i < 10; i++) {
            //    string filename = (i + 1).ToString("D4") + ".mp3";
            //    string srcpath = diskPath + filename;
            //    if (File.Exists(srcpath)) {
            //        string targetpath = Application.persistentDataPath + "/" + filename;
            //        File.Copy(srcpath, targetpath, true);
            //    }
            //}       
            if (Set.setVal.Language == (int)en_Language.Chinese)
            {
                text_UpdateFileTips.text = "更新完成";
            }
            else
            {
                text_UpdateFileTips.text = "Update finish";
            }

            updateTipsTime = 1.5f;
            isChangeVideo = false;
        }

        if (updateTipsTime > 0)
        {
            updateTipsTime -= Time.deltaTime;
            if (updateTipsTime <= 0)
            {
                updateFile_Obj.SetActive(false);
            }
        }

        // 检测是否有更新
        videoCheckTime += Time.deltaTime;
        if (videoCheckTime >= 2 && updateTipsTime <= 1.0f)
        {
            videoCheckTime = 0;
#if (VER_H6) && !(UNITY_EDITOR || UNITY_STANDALONE_WIN)         // || VER_3368
                diskPath = Menu_UpdateVideo.GetDiskPath() + "/";
#else
            diskPath = Menu_UpdateVideo.srcPath;
#endif
            // 视频
            for (int i = 0; i < Main.MAX_USER_VIDEO; i++)
            {
                string filename = "CN" + (i + 1).ToString("D4") + ".mp4";
                string srcpath = diskPath + filename;
                string targetpath = Application.persistentDataPath + "/" + filename;

                FileInfo fileInfo_Src = new FileInfo(srcpath);
                if (fileInfo_Src != null && fileInfo_Src.Exists)
                {
                    //if (File.Exists(srcpath)) {
                    FileInfo fileInfo_Target = new FileInfo(targetpath);
                    if (fileInfo_Target != null && fileInfo_Target.Exists)
                    {
                        //if (File.Exists(targetpath)) {
                        if (fileInfo_Target.Length != fileInfo_Src.Length)
                        {
                            ChangeVideoStart(i);
                            return;
                        }
                    }
                    else
                    {
                        ChangeVideoStart(i);
                        return;
                    }
                }
            }
            for (int i = 0; i < Main.MAX_USER_VIDEO; i++)
            {
                string filename = "EN" + (i + 1).ToString("D4") + ".mp4";
                string srcpath = diskPath + filename;
                string targetpath = Application.persistentDataPath + "/" + filename;

                FileInfo fileInfo_Src = new FileInfo(srcpath);
                if (fileInfo_Src != null && fileInfo_Src.Exists)
                {
                    //if (File.Exists(srcpath)) {
                    FileInfo fileInfo_Target = new FileInfo(targetpath);
                    if (fileInfo_Target != null && fileInfo_Target.Exists)
                    {
                        //if (File.Exists(targetpath)) {
                        if (fileInfo_Target.Length != fileInfo_Src.Length)
                        {
                            ChangeVideoStart(i);
                            return;
                        }
                    }
                    else
                    {
                        ChangeVideoStart(i);
                        return;
                    }
                }
            }
            //// 声音
            //for (int i = 0; i < 10; i++) {
            //    string filename = (i + 1).ToString("D4") + ".mp3";
            //    string srcpath = diskPath + filename;
            //    string targetpath = Application.persistentDataPath + "/" + filename;

            //    FileInfo fileInfo_Src = new FileInfo(srcpath);
            //    if (fileInfo_Src != null && fileInfo_Src.Exists) {
            //        //if (File.Exists(srcpath)) {
            //        FileInfo fileInfo_Target = new FileInfo(targetpath);
            //        if (fileInfo_Target != null && fileInfo_Target.Exists) {
            //            //if (File.Exists(targetpath)) {
            //            if (fileInfo_Target.Length != fileInfo_Src.Length) {
            //                ChangeVideoStart();
            //                return;
            //            }
            //        } else {
            //            ChangeVideoStart();
            //            return;
            //        }
            //    }
            //}
        }
    }
}
