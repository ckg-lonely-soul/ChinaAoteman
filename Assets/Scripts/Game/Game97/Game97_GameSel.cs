using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum en_GameSelSta
{
    Idle = 0,
    SelectPlayer,
    Selecting,
    EnterGame,
    NextPage,    //翻页
    PreviousPage
}
public class Game97_GameSel : MonoBehaviour
{
    public Num num_Time;
    public Image image_TitleBox;//标题框
    public Image image_PressToStart;
    public Image image_LogoCN;
    public Image image_LogoEN;
    public Image image_Ok;
    public Image image_Ok2;
    public GameObject image_LogoTitle;
    //public Image StartGame;

    public GameObject gamePageGroup;
    public GameObject selectKuang_Obj;
    public GameObject[] gamePage;
    public GameObject gameSelOne_Prefab;
    // 声音
    public AudioSource audioSource_BackG;       // 背景音乐
    public Image nextPage, previousPage;
    public RectTransform gameSels;

    Game97_Main game97_Main;
    Game97_GameSelOne[] gameSelOne; //总的
    List<Game97_GameSelOne> list_GameSelOne = new List<Game97_GameSelOne>();

    en_GameSelSta statue;
    int playerId;
    int selectId;
    int timeOut;
    float runTime;
    float selectTime;
    float shootWaterTime;
    float scale;
    //
    float pageTimer;

    int scenceNum;
    int currentScene;
    //  public List<int> buffScence = new List<int>();  //选择的5个场景的索引
    public float minX, maxX, movePosX;      //最左边的坐标 最右边的坐标（小图片）  移动的坐标
    // float[] currentPosX;
    public Sprite leftSelected, rightSelected, leftGray, rightGray;//左右箭头的图片  修改处
    public Transform Dot;
    public Transform[] dot_s;   //5个点的数组
                                // int lastDotIndex;
                                //public float gameSelX;
                                //
    public Image Button_NextPage;
    public Image Button_PreviousPage;
    int arrowSelectSta = 0;
    //-1左，0没选,1右

    //修改
    public Image Image_Title;
    public Text text_Time;

    Sprite sprite_Title;
    int maxGamePage;
    int maxCurrGameSel;
    int currGamePage;

    public void Awake0(Game97_Main game)
    {
        game97_Main = game;

        gameSelOne = new Game97_GameSelOne[Main.tab_GameId.Length];
        Sprite sprite;
        GameObject obj;
        for (int i = 0; i < gameSelOne.Length; i++)
        {
            obj = Instantiate(gameSelOne_Prefab, gamePageGroup.transform);
            gameSelOne[i] = obj.GetComponent<Game97_GameSelOne>();
            sprite = Resources.Load<Sprite>("Pic/Game97/GameSelect/GamePic/" + Main.tab_GameId[i].ToString("D4"));
            gameSelOne[i].Update_GamePic(sprite);
        }
        gameSelOne = GetComponentsInChildren<Game97_GameSelOne>();
        dot_s = Dot.GetComponentsInChildren<Transform>();

        if (Main.COMPANY_NUM == 6 || Main.COMPANY_NUM == 7 || Main.COMPANY_NUM == 10)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Company_" + Main.COMPANY_NUM.ToString("D2") + "/Pic/Game97/GameSelect/BackG");
        }
        else if (Main.COMPANY_NUM == 8)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Company_" + Main.COMPANY_NUM.ToString("D2") + "/Pic/Game97/Idle/BackG");
        }
        else if (Main.COMPANY_NUM == 11)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Company_" + Main.COMPANY_NUM.ToString("D2") + "/Pic/Game97/GameSelect/BackG");
            image_TitleBox.gameObject.SetActive(false);
        }
        else if (Main.COMPANY_NUM == 12)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Company_" + Main.COMPANY_NUM.ToString("D2") + "/Pic/Game97/GameSelect/BackG");
            image_TitleBox.gameObject.SetActive(false);
        }
    }

    readonly string[] strLanaguge = { "CN", "EN" };
    int lanauage = -1;
    void CheckLanguage()
    {
        if (lanauage != Set.setVal.Language)
        {
            lanauage = Set.setVal.Language;
            //
            string strCompany = "";
            if (Main.COMPANY_NUM == 4 || Main.COMPANY_NUM == 5)
            {
                strCompany = "Company_" + Main.COMPANY_NUM.ToString("D2") + "/";
            }
            Sprite sprite;
            for (int i = 0; i < gameSelOne.Length; i++)
            {
                sprite = Resources.Load<Sprite>(strCompany + "Pic/Game97/GameSelect/GameName/" + Main.tab_GameId[i].ToString("D3") + "_" + strLanaguge[lanauage]);
                gameSelOne[i].Update_GameName(sprite);
            }
            //
            if (Set.setVal.Language == (int)en_Language.Chinese)
            {
                image_LogoCN.gameObject.SetActive(true);
                if (Main.COMPANY_NUM == 1)
                {
                    image_LogoCN.gameObject.SetActive(false);
                }
                image_LogoEN.gameObject.SetActive(false);
                image_Ok.gameObject.SetActive(true);
                image_Ok2.gameObject.SetActive(false);
                //if (Main.COMPANY_NUM == 1 || Main.COMPANY_NUM == 10)
                //{
                //    image_LogoCN.sprite = Resources.Load<Sprite>("Company_" + Main.COMPANY_NUM.ToString("D2") + "/Pic/Game97/Idle/BackG_cn/0000");
                //}
                if(Main.COMPANY_NUM == 7 || Main.COMPANY_NUM == 6 || Main.COMPANY_NUM == 8)
                {
                    image_LogoCN.sprite = Resources.Load<Sprite>("Company_" + Main.COMPANY_NUM.ToString("D2") + "/Pic/Game97/GameSelect/Logo/Logo_CN");
                    image_LogoCN.SetNativeSize();
                    image_LogoCN.transform.localScale = Vector3.one * 0.75f;
                    image_LogoCN.transform.localPosition = new Vector3(-490, 295);
                }
                else
                {
                    image_LogoCN.sprite = Resources.Load<Sprite>("Company_" + Main.COMPANY_NUM.ToString("D2") + "/Pic/Game97/Idle/BackG_cn/0000");
                    if(Main.COMPANY_NUM == 11)
                    {
                        image_LogoCN.transform.localPosition = new Vector3(-480f, 280f);
                    }
                    if (Main.COMPANY_NUM == 12)
                    {
                        image_LogoCN.SetNativeSize();
                        image_LogoCN.transform.localScale = Vector3.one * 0.4f;
                        image_LogoCN.transform.localPosition = new Vector3(-480f, 285f);
                    }
                }
                if (Main.COMPANY_NUM == 4)
                {
                    sprite_Title = Resources.Load<Sprite>("Company_04/Pic/Game97/GameSelect/Title_CN");
                    Image_Title.transform.localScale = Vector3.one;
                }
                else if(Main.COMPANY_NUM == 11)
                {
                    sprite_Title = Resources.Load<Sprite>("Company_11/Pic/Game97/GameSelect/Title/Title_CN");
                    Image_Title.transform.localScale = Vector3.one;
                }
                else if (Main.COMPANY_NUM == 12)
                {
                    sprite_Title = Resources.Load<Sprite>("Company_12/Pic/Game97/GameSelect/Title/Title_CN");
                    Image_Title.transform.localScale = Vector3.one;
                    image_Ok.transform.parent.GetComponent<Image>().enabled = false;
                    image_Ok.sprite = Resources.Load<Sprite>("Company_12/Pic/Game97/GameSelect/Queding/Queding_CN");
                    image_Ok.SetNativeSize();
                }
                else
                {
                    sprite_Title = Resources.Load<Sprite>("Company_00/Common/GameUI/cn/0000");
                }
            }
            else
            {
                image_Ok.gameObject.SetActive(false);
                image_Ok2.gameObject.SetActive(true);
                image_LogoCN.gameObject.SetActive(false);
                image_LogoEN.gameObject.SetActive(true);
                if (Main.COMPANY_NUM == 1)
                {
                    image_LogoEN.gameObject.SetActive(false);
                }
                //if (Main.COMPANY_NUM == 1 || Main.COMPANY_NUM == 10)
                //{
                //    image_LogoEN.sprite = Resources.Load<Sprite>("Company_" + Main.COMPANY_NUM.ToString("D2") + "/Pic/Game97/Idle/BackG_en/0000");
                //}
                if(Main.COMPANY_NUM == 7 || Main.COMPANY_NUM == 6 || Main.COMPANY_NUM == 8)
                {
                    image_LogoEN.sprite = Resources.Load<Sprite>("Company_" + Main.COMPANY_NUM.ToString("D2") + "/Pic/Game97/GameSelect/Logo/Logo_EN");
                    image_LogoEN.SetNativeSize();
                    image_LogoEN.transform.localScale = Vector3.one * 0.75f;
                    image_LogoEN.transform.localPosition = new Vector3(-475, 295);
                }
                else
                {
                    image_LogoEN.sprite = Resources.Load<Sprite>("Company_" + Main.COMPANY_NUM.ToString("D2") + "/Pic/Game97/Idle/BackG_en/0000");
                    if (Main.COMPANY_NUM == 12)
                    {
                        image_LogoEN.SetNativeSize();
                        image_LogoEN.transform.localScale = Vector3.one * 0.3f;
                        image_LogoEN.transform.localPosition = new Vector3(-480f, 285f);
                    }
                }
                if (Main.COMPANY_NUM == 4)
                {
                    sprite_Title = Resources.Load<Sprite>("Company_04/Pic/Game97/GameSelect/Title_EN");
                    Image_Title.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                }
                else if (Main.COMPANY_NUM == 11)
                {
                    sprite_Title = Resources.Load<Sprite>("Company_11/Pic/Game97/GameSelect/Title/Title_EN");
                    Image_Title.transform.localScale = Vector3.one;
                }
                else if (Main.COMPANY_NUM == 12)
                {
                    sprite_Title = Resources.Load<Sprite>("Company_12/Pic/Game97/GameSelect/Title/Title_EN");
                    Image_Title.transform.localScale = Vector3.one;
                    image_Ok.transform.parent.GetComponent<Image>().enabled = false;
                    image_Ok2.sprite = Resources.Load<Sprite>("Company_12/Pic/Game97/GameSelect/Queding/Queding_EN");
                    image_Ok2.SetNativeSize();
                }
                else
                {
                    sprite_Title = Resources.Load<Sprite>("Company_00/Common/GameUI/en/0000");
                }
            }
            Image_Title.sprite = sprite_Title;
            Image_Title.SetNativeSize();
        }
    }

    public void GameStart()
    {
        //修改图标--语言
        CheckLanguage();
        //
        selectId = -1;
        // 确定选择游戏的玩家，玩家1优先
        playerId = -1;
        scenceNum = 0;
        selectTime = 20f;
        shootWaterTime = 0;
        currentScene = 2;
        currGamePage = 0;
        // buffScence = new List<int>();
        UpdateGameSelectInit();
        //UpdateCurrentSelect();
        Update_GameSelOneSta();

        if (maxGamePage <= 1)
        {
            previousPage.gameObject.SetActive(false);
            nextPage.gameObject.SetActive(false);
        }
        else
        {
            previousPage.gameObject.SetActive(true);
            nextPage.gameObject.SetActive(true);
        }

        //
        if (Main.COMPANY_NUM == 1)
        {
            // 枪神
            ChangeStatue(en_GameSelSta.Idle);
            image_LogoTitle.SetActive(true);
#if IO_LOCAL
            if (Set.setVal.ChildrenSong == 0)
            {
                audioSource_BackG.clip = SoundManager.Instance.levelBgm[Random.Range(0, SoundManager.Instance.levelBgm.Length)];
            }
            else
            {
                if (Set.setVal.Language == (int)en_Language.Chinese)
                {
                    audioSource_BackG.clip = SoundManager.Instance.gunGodBgmCN[Random.Range(0, SoundManager.Instance.gunGodBgmCN.Length)];
                }
                else
                {
                    audioSource_BackG.clip = SoundManager.Instance.gunGodBgmEN[Random.Range(0, SoundManager.Instance.gunGodBgmEN.Length)];
                }
            }
            audioSource_BackG.volume = 0.6f;
#endif
            if (Set.setVal.DeskMusic == 0 && game97_Main.IsCanPlay() == false)
            {
                audioSource_BackG.Stop();

            }
            else
            {
                audioSource_BackG.Play();
            }
            return;
        }
        StartSelect();
    }

    void StartSelect()
    {
        audioSource_BackG.Play();

        if (Set.setVal.PlayerMode == (int)en_PlayerMode.Two)
        {
            for (int i = 0; i < game97_Main.playerUI.Length; i++)
            {
                game97_Main.playerUI[i].image_Cursor.gameObject.SetActive(true);
            }
        }
        else if (Set.setVal.PlayerMode == (int)en_PlayerMode.One)
        {
            game97_Main.playerUI[0].image_Cursor.gameObject.SetActive(true);
        }
        Image_Title.gameObject.SetActive(true);
        image_LogoTitle.SetActive(false);
        for (int i = 0; i < Main.MAX_PLAYER; i++)
        {
            if (FjData.g_Fj[i].Playing)
            {
                playerId = i;
                break;
            }
        }
        if (Set.setVal.PlayerMode == (int)en_PlayerMode.One)
        {
            playerId = 0;
        }
        if (playerId < 0)
        {
            if (Set.setVal.PlayerMode == (int)en_PlayerMode.Two && Set.setVal.InOutMode == (int)en_InOutMode.OneInOneOut)
            {
                ChangeStatue(en_GameSelSta.SelectPlayer);

                return;
            }
            for (int i = 0; i < Main.MAX_PLAYER; i++)
            {
                if (Main.IsCanGamePlay(i))
                {
                    playerId = i;
                    break;
                }
            }
        }
        if (playerId < 0)
        {
            playerId = 0;
        }

        for (int i = 1; i < dot_s.Length; i++)
        {
            if (i != (currentScene + 1))
                dot_s[i].gameObject.SetActive(false);
        }
        ChangeStatue(en_GameSelSta.Selecting);

        runTime = 20;
        timeOut = 20;

        //#if UNITY_EDITOR
        //        runTime = 4;
        //        timeOut = 4;
        //#endif
    }

    void Update()
    {
        switch (statue)
        {
            case en_GameSelSta.Idle:
                if (game97_Main.IsCanPlay())
                {
                    StartSelect();
                }
                break;

            case en_GameSelSta.SelectPlayer:
                if (runTime > 0)
                {
                    runTime -= Time.deltaTime;
                    if (timeOut != (int)runTime)
                    {
                        timeOut = (int)runTime;
                        num_Time.UpdateShow(timeOut);
                        text_Time.text = (timeOut / 60).ToString("D2") + ":" + (timeOut % 60).ToString("D2");
                        if (timeOut == 0)//选择时间到了自动扣玩家1币开始
                        {
                            if (Main.DecStartCoin(0))
                            {
                                playerId = 0;
                                FjData.g_Fj[playerId].Playing = true;
                                FjData.g_Fj[playerId].Played = true;
                                ChangeStatue(en_GameSelSta.Selecting);
                                break;
                            }
                        }
                    }
                }
                for (int i = 0; i < Main.MAX_PLAYER; i++)
                {
                    if (Key.KEYFJ_OkPressed(i))
                    {
                        if (Main.DecStartCoin(i))
                        {
                            playerId = i;
                            FjData.g_Fj[playerId].Playing = true;
                            FjData.g_Fj[playerId].Played = true;
                            ChangeStatue(en_GameSelSta.Selecting);
                            break;
                        }
                    }
                }
                break;
            case en_GameSelSta.Selecting:
                if(Set.setVal.InOutMode == (int)en_InOutMode.OneInOneOut)
                {
                    for (int i = 0; i < Main.MAX_PLAYER; i++)
                    {
                        if (FjData.g_Fj[i].Playing)
                        {
                            IO.GunMotorStart(i);
                        }
                        else
                        {
                            if (Key.KEYFJ_OkPressed(i))
                            {
                                if (Main.DecStartCoin(i))
                                {
                                    playerId = i;
                                    FjData.g_Fj[playerId].Playing = true;
                                    FjData.g_Fj[playerId].Played = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Main.MAX_PLAYER; i++)
                    {
                        if (!FjData.g_Fj[i].Playing)
                        {
                            if (FjData.g_Fj[i].Coins >= Set.setVal.StartCoins)
                            {
                                IO.GunMotorStart(i);
                            }
                        }
                    }
                }
                if (runTime > 0)
                {
                    runTime -= Time.deltaTime;
                    if (timeOut != (int)runTime)
                    {
                        timeOut = (int)runTime;
                        num_Time.UpdateShow(timeOut);
                        text_Time.text = (timeOut / 60).ToString("D2") + ":" + (timeOut % 60).ToString("D2");
                    }
                }
                if (Set.setVal.GunMode == (int)en_GunMode.ShootWater)
                {
                    shootWaterTime += Time.deltaTime;
                }
#if FPS_TEST
            shootWaterTime += Time.deltaTime;
#endif
                if (Key.KEYFJ_OkPressed(playerId) || timeOut == 0 || shootWaterTime >= 1f)
                {
                    shootWaterTime = 0;
                    if (selectId < 0 && timeOut == 0)
                    {
                        selectId = GetRandomGameId();
                    }
                    if (selectId >= 0)
                    {
                        ChangeStatue(en_GameSelSta.EnterGame);
                        break;
                    }
                }

                SelectGameCheck();
                if (maxGamePage <= 1)
                {
                    break;
                }

                if (arrowSelectSta == -1)
                {
                    ChangeStatue(en_GameSelSta.PreviousPage);
                    break;
                }
                if (arrowSelectSta == 1)
                {
                    ChangeStatue(en_GameSelSta.NextPage);
                    break;
                }// 新

                break;


            case en_GameSelSta.EnterGame:
                #region
                //gameSelOne[selectId].transform.localPosition = Vector3.MoveTowards(gameSelOne[selectId].transform.localPosition, new Vector3(0, 0, 0), 1000 * Time.deltaTime);
                //if (scale < 1)
                //{
                //    scale += Time.deltaTime;
                //    if (scale > 1)
                //    {
                //        scale = 1;
                //    }
                //    gameSelOne[selectId].transform.localScale = new Vector3(scale, scale, scale);
                //}
                //else
                //{
                #endregion
                runTime += Time.deltaTime;
                if (runTime >= 0.5f)
                {
                    //Debug.Log("EnterGame: " + selectId);
                    game97_Main.EnterGame(Main.tab_GameId[selectId]);
                }
                //}
                break;
            case en_GameSelSta.PreviousPage:
            case en_GameSelSta.NextPage:
                if (runTime > 0)
                {
                    runTime -= Time.deltaTime;
                    if (timeOut != (int)runTime)
                    {
                        timeOut = (int)runTime;
                        num_Time.UpdateShow(timeOut);
                        text_Time.text = (timeOut / 60).ToString("D2") + ":" + (timeOut % 60).ToString("D2");
                    }
                }

                SelectGameCheck();

                if (Vector3.Distance(gamePageGroup.transform.localPosition, new Vector3(0, 0, 0)) > 5)
                {
                    gamePageGroup.transform.localPosition = Vector3.Lerp(gamePageGroup.transform.localPosition, new Vector3(0, 0, 0), 4f * Time.deltaTime);
                }
                else
                {
                    gamePageGroup.transform.localPosition = new Vector3(0, 0, 0);
                    ChangeStatue(en_GameSelSta.Selecting);
                }
                break;

        }
    }

    void ChangeStatue(en_GameSelSta sta)
    {
        statue = sta;
#if UNITY_EDITOR
        Debug.Log(sta);
#endif
        //runTime = 0;
        Key.Clear();

        image_PressToStart.gameObject.SetActive(false);

        switch (statue)
        {
            case en_GameSelSta.Idle:
                Image_Title.gameObject.SetActive(false);
                break;

            case en_GameSelSta.SelectPlayer:
                image_PressToStart.gameObject.SetActive(true);
                runTime = selectTime;
                timeOut = (int)selectTime;
                num_Time.UpdateShow(timeOut);
                text_Time.text = (timeOut / 60).ToString("D2") + ":" + (timeOut % 60).ToString("D2");
                break;
            case en_GameSelSta.Selecting:
                //image_NextPage = disGameSelone[0].image_BackG;
                //image_PreviousPage = disGameSelone[0].image_BackG;
                //if (runTime == 0)
                //{
                //    runTime = 20;
                //    timeOut = 20;
                //}
                if (Set.setVal.GunMode == (int)en_GunMode.ShootWater)
                {
                    IO.GunMotorStart(playerId);
                }
                else if (Set.setVal.GunMode == (int)en_GunMode.Gun)
                {
#if GUN_HW
                    for (int i = 0; i < Main.MAX_PLAYER; i++)
                    {
                        if (FjData.g_Fj[i].Playing)
                        {
                            IO.GunMotorStart(i);
                        }
                    }
#else
                    IO.GunMotorStart(playerId);
#endif
                }
                
                previousPage.GetComponent<Image>().sprite = rightGray;//左右调换
                nextPage.GetComponent<Image>().sprite = leftGray;
                maxX = (list_GameSelOne.Count - 6) * 20;//420
                minX = -maxX;

                for (int i = 0; i < Main.MAX_PLAYER; i++)
                {
                    if (FjData.g_Fj[i].Playing)
                    {//每个玩家如果在游戏就不显示
                        game97_Main.playerUI[i].SetCheckPleaseCoinIn(false);
                    }
                    else
                    {
                        game97_Main.playerUI[i].SetCheckPleaseCoinIn(true);
                    }
                }

#if FPS_TEST //|| UNITY_EDITOR
            runTime = 2;
            timeOut = 2;
#endif
                // num_Time.UpdateShow(timeOut);
                //UpdateGameSelect();
                //Update_GameSelOneSta();
                break;
            case en_GameSelSta.EnterGame:

                break;

            case en_GameSelSta.NextPage://右键  按钮
                                        //Debug.Log("NextPage");
                currentScene++;
                currGamePage++;
                if (currGamePage >= maxGamePage)
                    currGamePage = 0;
                for (int i = 0; i < maxGamePage; i++)
                {
                    gamePage[i].transform.localPosition = new Vector3(((i + currGamePage) % maxGamePage) * 1280, 0, 0);
                }
                gamePageGroup.transform.localPosition = new Vector3(-1280, 0, 0);
                break;

            case en_GameSelSta.PreviousPage://左键  按钮
                                            //Debug.Log("PreviousPage");
                currentScene--;
                currGamePage--;
                if (currGamePage < 0)
                    currGamePage = maxGamePage - 1;
                for (int i = 0; i < maxGamePage; i++)
                {
                    gamePage[i].transform.localPosition = new Vector3(((currGamePage + i - maxGamePage) % maxGamePage) * 1280, 0, 0);
                }
                gamePageGroup.transform.localPosition = new Vector3(1280, 0, 0);
                break;
        }
    }

    void SelectGameCheck()
    {
        //光标在左右图标上，继续检测
        for (int i = 0; i < gameSelOne.Length; i++)
        {
            if (Main.gamePassed[i])
                continue;
            if (gameSelOne[i].gameObject.activeSelf == false)
                continue;
            if (Main.IsOnButton(game97_Main.playerUI[playerId].image_Cursor, gameSelOne[i].image_Pic))
            {
                selectId = i;
                Update_GameSelOneSta();
                break;
            }
        }

        //光标检测
        if (Main.IsOnButton(game97_Main.playerUI[playerId].image_Cursor, previousPage))
        {
            if (arrowSelectSta != -1)
            {
                Update_ArrowSelectStatue(-1);
            }
        }
        else if (Main.IsOnButton(game97_Main.playerUI[playerId].image_Cursor, nextPage))
        {
            if (arrowSelectSta != 1)
            {
                Update_ArrowSelectStatue(1);
            }
        }
        else if (arrowSelectSta != 0)
        {
            Update_ArrowSelectStatue(0);
        }

    }

    void Update_ArrowSelectStatue(int no)
    {
        arrowSelectSta = no;
        if (arrowSelectSta == -1)
        {
            previousPage.GetComponent<Image>().sprite = rightSelected;//修改处
            nextPage.GetComponent<Image>().sprite = leftGray;
        }
        else if (arrowSelectSta == 0)
        {
            nextPage.GetComponent<Image>().sprite = leftGray;
            previousPage.GetComponent<Image>().sprite = rightGray;
        }
        else if (arrowSelectSta == 1)
        {
            previousPage.GetComponent<Image>().sprite = rightGray;
            nextPage.GetComponent<Image>().sprite = leftSelected;
        }
    }

    void UpdateGameSelectInit()
    {
        int no = 0;		//0~5
        int id;
        int maxOnePage = 6;
        int starty = 100;
        int disx = 340;
        int disy = 230;
        if (Main.COMPANY_NUM == 5)
        {
            maxOnePage = 9;
            starty = 160;
            disx = 320;
            disy = 160;
        }
        maxCurrGameSel = 0; //show
        maxGamePage = 0;
        //Debug.Log(gameSelOne.Length);

        list_GameSelOne.Clear();
        for (int i = 0; i < gameSelOne.Length; i++)
        {
            if (i >= Main.tab_GameId.Length)
            {
                gameSelOne[i].gameObject.SetActive(false);
                continue;
            }
            if (Main.gamePassed[i])
            {
                gameSelOne[i].Update_Statue(true);
            }
            else
            {
                gameSelOne[i].Update_Statue(false);
            }
            if (Set.GameSelect[i] == 1)
            {
                gameSelOne[i].gameObject.SetActive(true);
                list_GameSelOne.Add(gameSelOne[i]);
                if (no >= maxOnePage)
                {
                    no = 0;
                    maxGamePage++;
                    //Debug.Log(maxGamePage);
                }
                gameSelOne[i].transform.SetParent(gamePage[maxGamePage].transform);
                gameSelOne[i].transform.localPosition = new Vector3(((no % 3) - 1) * disx, starty - (no / 3) * disy, 0);
                no++;
                maxCurrGameSel++;
            }
            else
            {
                gameSelOne[i].gameObject.SetActive(false);
            }
        }
        maxGamePage++;

        for (int i = 0; i < maxGamePage; i++)
        {
            gamePage[i].transform.localPosition = new Vector3(i * 1280, 0, 0);
        }
        //Debug.Log (gameSelOne.Length);		
        currGamePage = 0;
    }

    int GetRandomGameId()    //随机进入场景
    {
        int[] idBuf = new int[Main.tab_GameId.Length];

        int len = 0;
        for (int i = 0; i < Main.tab_GameId.Length; i++)
        {
            if (Set.GameSelect[i] == 0)
                continue;
            if (Main.gamePassed[i] == false)
            {
                idBuf[len] = i;
                len++;
            }
        }
        if (len > 0)
        {
            return idBuf[Random.Range(0, len)];
        }
        return -1;
    }

    void Update_GameSelOneSta()             //选中的图标会变大
    {
        selectKuang_Obj.SetActive(false);

        for (int i = 0; i < Main.tab_GameId.Length; i++)
        {
            if (i == selectId)
            {
                gameSelOne[i].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                selectKuang_Obj.SetActive(true);
                selectKuang_Obj.transform.SetParent(gameSelOne[i].transform.parent);
                selectKuang_Obj.transform.localPosition = gameSelOne[i].transform.localPosition;
                selectKuang_Obj.transform.localScale = gameSelOne[i].transform.localScale;
                gameSelOne[i].image_NoSelectBox.gameObject.SetActive(false);
            }
            else
            {
                gameSelOne[i].transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);//0.85
                gameSelOne[i].image_NoSelectBox.gameObject.SetActive(true);
            }
        }
    }

}
