using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum en_MainStatue
{
    Restart = -1,       // 开机
    Game_00 = 0,
    Game_96,
    Game_97 = 97,
    Game_98,

    LoadScene,          // 加载游戏场景中..
    Menu,
    Game,
}

enum en_LoadStatue
{
    Load_FjData = 0,
    Load_JL,
    Load_Dna,
    Load_Enc,
}

public enum en_GameStatue
{
    Idle = 0,
    Play,
    Gameover,
}

public enum en_ErrorMask
{
    ERROR_NONE = 0,
    OUT_TIMEOUT = (1 << 0),
}

public enum en_Scene
{
    GameMain = 97,       // 游戏选择界面
    Menu = 98,       // 菜单界面
}


public class Main : MonoBehaviour
{
    #region 字段
    // 分辨率设置
    public static bool FREE_SIZE = false;	// 自适应分辨率
    //默认分辨率
    public const float DEFAULT_SCREEN_WIDTH = 1280;
    public const float DEFAULT_SCREEN_HEIGHT = 720;
    //目标分辨率
    public const float TARGET_SCREEN_WIDTH = 1280;
    public const float TARGET_SCREEN_HEIGHT = 720;

    // 枪版本
#if INFRARED_SCREEN_BEAD    // 
    const string GUN_VER = "InfraredScreen.";
#elif SHOOT_BEAD      // 弹珠机
    const string GUN_VER = "bead.";
#elif SHOOT_WATER   // 水枪机
    const string GUN_VER = "water.";
#else               // 普通版
    const string GUN_VER = "";
#endif
    public static string VERSION = GUN_VER + "01.00.00";

    public const bool VER_DNA = false;
    public const bool VER_ENC = false;

    public const bool KEY_ESC_QUITE = false; // 按"ESC"键可退出程序
    public const int COMPANY_NUM = 3;
    //3:中华超人

#if NEW_IO
    public readonly static int[] tab_GameId = { 0 };
#elif IO_LOCAL
    public readonly static int[] tab_GameId = { 1 };
#endif

    public const int MAX_PLAYER = 2;
    public const int MAX_MONSTER = 20;
    public const int MAX_USER_VIDEO = 1;

    public Canvas gameUI_Panel;
    public GameObject camera_Obj;
    public GameObject loading_Obj;
    public GameObject game97_Obj;
    public Menu menu;
    public GameObject volumeControl_Prefab;
    Game97_Main game97_Main;

    Game_LoadScene game_LoadScene;
    //
    public Image image_LoadingBackG;
    public Image image_ProgressBarValue;//image 的fillAmount实现进度条
    public Slider image_ProgressBarValue1;//slider实现进度条

    public Num num_Rocket;//小火箭数字
    public Image[] image_Rocket;//去掉

    public Sprite[] sprite_SmallLogo;
    // sound
    public GameObject sound_CoinIn;
    public static AudioSource audioSource_CoinIn;

    // 组件, 类
#if UNITY_EDITOR
    public static Uart_Windows uartIO = new Uart_Windows();
#elif UNITY_ANDROID
    public static Uart_Android uartIO = new Uart_Android ();
#endif

    // 全局变量 
    // 主程序状态
    public static en_MainStatue statue = en_MainStatue.Restart;
    public static en_MainStatue nextStatue = en_MainStatue.Restart;

    // 游戏状态
    [HideInInspector]
    public static ulong[] errorStatue = new ulong[MAX_PLAYER];
    [HideInInspector]
    public static float[] errorOutTimeoutTime = new float[MAX_PLAYER];
    public static bool[] errorChanged = new bool[MAX_PLAYER];
    public static int[] score = new int[MAX_PLAYER];
    // 游戏ID（场景）
    public static bool isRestart = true;  // 重启动标志
    public static int gameId = -1;      // 开机加载界面
    public static bool IsDemo = false;
    public static float soundVolume;    // 游戏音量
    public static bool[] coinInChanged = new bool[MAX_PLAYER];
    public static bool[] outNumChanged = new bool[MAX_PLAYER];   // 退数字已改变
    public static bool[] bombNumChanged = new bool[MAX_PLAYER];  // 炸弹数改变
    public static bool[] gameStarted = new bool[MAX_PLAYER];    // 游戏开始标志（计算奖励用到）
    public static int destroyId;

    public static bool[] gamePassed = new bool[tab_GameId.Length];
    public static bool gameOver = false;
    public static bool isFirstPlayVideo;//第一次播放视频
    public static bool isSelectLevel;//是否在选关中，用来设置投币位置

    float runTime;
    int sceneNum = 0;
    string sceneName = "";

    // 界面切换
    [HideInInspector]
    public bool panelChanging = false;
    [HideInInspector]
    public int panelChangeMode;					// 切换模式
    [HideInInspector]
    public GameObject panel_Old;				// 老界面
    [HideInInspector]
    public GameObject panel_New;                // 新界面

    public static int receiveDataNum = 0;       // 接收数据个数
    public static byte readAddr;
    public static byte addr;
    GameObject volumeControl;
    #endregion

    void Awake()
    {
        menu.Awake0(this);
        VolumeControl.Init();
        game97_Main = game97_Obj.GetComponentInChildren<Game97_Main>();
        game97_Main.Awake0(this);
        audioSource_CoinIn = sound_CoinIn.GetComponent<AudioSource>();
#if IO_LOCAL
        audioSource_CoinIn.volume = 0.6f;
#endif
        isFirstPlayVideo = true;
    }

    void Awake0()
    {
        // 声音
        audioSource_CoinIn = sound_CoinIn.GetComponent<AudioSource>();
        //
        if (gameId == -1)
        {
            Set.LoadAll();
            JL.Init();
        }
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        Set.LoadAll();
        Key.Init();
        IO.Init();
        LeYaoYao.LYY_Init();
#if NEW_IO||IO_GUN1
        AudioListener.volume = (float)Set.setVal.SysVolume / 100;
#endif
        if (nextStatue == en_MainStatue.Restart)
        {
            statue = en_MainStatue.Game_97;
            ChangeStatue(en_MainStatue.Game_97);
        }
        else
        {
            ChangeStatue(nextStatue);
        }
#if UNITY_EDITOR || FPS_TEST
#else
        
#endif
    }

    int testTime = 0;
    int testTimeCnt = 0;
    void FixedUpdate()
    {
        //500帧之后testTime+1。
        testTimeCnt++;
        if (testTimeCnt >= 500)
        {
            testTimeCnt = 0;
            testTime++;
        }
    }

    int fpscnt;
    int fps;
    float fpstime;

    public static long AllMemory;
    public static long UseMemory;
    public static long UnuseMemory;

    void Update()
    {
#if IO_LOCAL && !UNITY_EDITOR
        Key.CheckByInput();
#endif
        #region Test
        //Debug.Log(FjData.g_Fj[0].Coins);
        //cpuRatio = Profiler.GetTotalAllocatedMemory();
        //gpuRatio = Profiler.GetTotalAllocatedMemory();
        AllMemory = Profiler.GetTotalReservedMemory() / 1000000;
        UseMemory = Profiler.GetTotalAllocatedMemory() / 1000000;
        UnuseMemory = Profiler.GetTotalUnusedReservedMemory() / 1000000;
        //memGpu = Profiler.GetTotalAllocatedMemory ();
        //帧数检测
        fpscnt++;
        fpstime += Time.deltaTime;
        if (fpstime >= 1.0f)
        {
            fpstime -= 1.0f;
            fps = fpscnt;
            fpscnt = 0;

            //DebugConsole.instance.AddMessage("this is instance AddMessage test by cartzhang", "warning");
        }
        // 输出口测试
        // OutIO_Test();
        //
        ChangePanelRun();

        IO.CheckSend();

        #endregion

        switch (statue)
        {
            case en_MainStatue.LoadScene:
                if (async == null || async.isDone)
                {
                    if (nextStatue == en_MainStatue.Game_97 || nextStatue == en_MainStatue.Game_98)
                    {
                        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game_97"));
                    }
                    else if (nextStatue == en_MainStatue.Game_96)
                    {
                        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game_96"));
                    }
                    else
                    {
                        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game_" + ((int)nextStatue).ToString("D2")));
                    }
                    ChangeStatue(nextStatue);
                    break;
                }

                image_ProgressBarValue.fillAmount = async.progress;
                image_ProgressBarValue1.value = async.progress;
                float b = async.progress;
                float a = 0;
                a = 100 * b;
                num_Rocket.UpdateShow((int)a);//小火箭百分数
                break;

            case en_MainStatue.Game_98:
                break;

            default:
                //设置
                if (Key.MENU_OkPressed() || Key.KEYFJ_Menu_OkPressed())
                {
                    IO.Init();
                    PAction.Init();
                    ChangeScene(en_MainStatue.Game_98);
                }
#if NEW_IO || IO_GUN1
                if (Key.MENU_Statue_Left() || Key.MENU_Statue_Right())
                {
                    if (volumeControl == null)
                    {
                        volumeControl = GameObject.Instantiate(volumeControl_Prefab, gameUI_Panel.transform);
                        volumeControl.transform.localPosition = new Vector3(0, 0, 0);
                    }
                }
#endif
                break;

        }

        //按ESC退出
        if (KEY_ESC_QUITE)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
                return;
            }
        }
    }

    static AsyncOperation async;
    public void ChangeScene(en_MainStatue gameno)   //切换场景
    {
        Resources.UnloadUnusedAssets();
        PoolManager.instance.PoolClear();
        switch (gameno)
        {
            case en_MainStatue.Game_00:
                isSelectLevel = false;
                async = SceneManager.LoadSceneAsync("Game_" + ((int)gameno).ToString("D2"), LoadSceneMode.Additive);
                break;
            case en_MainStatue.Game_96:
                async = SceneManager.LoadSceneAsync("Game_96", LoadSceneMode.Additive);
                isSelectLevel = false;
                break;
            case en_MainStatue.Game_97:
            case en_MainStatue.Game_98:
                Scene scene;
                async = null;
                for (int i = 0; i < tab_GameId.Length; i++)
                {
                    scene = SceneManager.GetSceneByName("Game_" + tab_GameId[i].ToString("D2"));
                    if (scene.IsValid())
                    {
                        async = SceneManager.UnloadSceneAsync("Game_" + tab_GameId[i].ToString("D2"));
                    }
                }
                isSelectLevel = false;
                break;
        }

        image_ProgressBarValue.fillAmount = 0;
        nextStatue = gameno;
        ChangeStatue(en_MainStatue.LoadScene);
    }

    public void ChangeStatue(en_MainStatue sta)
    {

        statue = sta;
        Key.Clear();
#if GUN_HW
        IO.GunMotorStop(0);
        IO.GunMotorStop(1);
#endif
#if IO_LOCAL
        IO.MotorOld_Out(false);
#endif
        if (camera_Obj != null)
        {
            camera_Obj.SetActive(false);
        }
        if (loading_Obj != null)
        {
            loading_Obj.SetActive(false);
        }
        if (game97_Obj != null)
        {
            game97_Obj.SetActive(false);
        }
        if (menu != null)
        {
            menu.gameObject.SetActive(false);
        }

        Scene[] scenes = SceneManager.GetAllScenes();
        sceneNum = scenes.Length;
        sceneName = SceneManager.GetActiveScene().name;

        switch (statue)
        {
            case en_MainStatue.Game_00:
                Game00_Main game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_96:
                Game96_Main game96_Main = GameObject.Find("Game96Main").GetComponentInChildren<Game96_Main>();
                game96_Main.Awake0(this);
                game96_Main.GameStart();
                break;
            case en_MainStatue.Game_97:
                camera_Obj.SetActive(true);
                game97_Obj.SetActive(true);
                game97_Main.GameStart();
                break;
            case en_MainStatue.Game_98:
                camera_Obj.SetActive(true);
                menu.gameObject.SetActive(true);
                menu.Enter();
                isRestart = true;
                for (int i = 0; i < MAX_PLAYER; i++)
                {
                    FjData.g_Fj[i].Playing = false;
                }
                break;
            case en_MainStatue.LoadScene:
                loading_Obj.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// 改变场景到待机状态
    /// </summary>
    public void ChangeStatue_To_GameIdle()
    {
        ChangeStatue(en_MainStatue.Game_97);
    }

    /// <summary>
    /// 无参ChangeScene
    /// </summary>
    public void ChangeScene()
    {
        print("ChangeScene: " + gameId.ToString());
        ChangeStatue(en_MainStatue.LoadScene);
    }

    public static void PlaySound_CoinIn()
    {
        if (audioSource_CoinIn != null)
        {
            audioSource_CoinIn.Play();
        }
    }

    // Load data ---------------------------------------------------------------------------------
    // 用在加载进度条之前，读出所有数据
    // 加载数据程序状态
    static en_LoadStatue loadStatue;
    public static void Load_Start()
    {
        if (VER_DNA)
        {
            Load_ChangeStatue(en_LoadStatue.Load_Dna);
        }
        else if (VER_ENC)
        {
            Load_ChangeStatue(en_LoadStatue.Load_Enc);
        }
        else
        {
            Load_ChangeStatue(en_LoadStatue.Load_FjData);
        }
    }

    public static bool Load_Run()
    {
        //#if UNITY_EDITOR
        //        return true;
        //#endif
        switch (loadStatue)
        {
            case en_LoadStatue.Load_Dna:
                if (Game_Dna.Load())
                {
                    Game_Dna.Init();
                    Game_Dna.CheckDnaCode();
                    if (Game_Enc.MACHINE_NO <= 0)
                    {
                        return true;    // 打码失败:加载完备，直接进入打码
                    }
                    if (VER_ENC)
                    {
                        Load_ChangeStatue(en_LoadStatue.Load_Enc);
                    }
                    else
                    {
                        Load_ChangeStatue(en_LoadStatue.Load_FjData);
                    }
                }
                break;
            case en_LoadStatue.Load_Enc:
                if (Game_Enc.Load())
                {
                    Load_ChangeStatue(en_LoadStatue.Load_FjData);
                }
                break;
            case en_LoadStatue.Load_FjData:
                if (FjData.Load())
                {
                    Load_ChangeStatue(en_LoadStatue.Load_JL);
                }
                break;
            case en_LoadStatue.Load_JL:
                if (JL.Load())
                {
                    return true;    // 加载完备
                }
                break;
        }
        return false;
    }

    static void Load_ChangeStatue(en_LoadStatue sta)
    {
        loadStatue = sta;
        switch (loadStatue)
        {
            case en_LoadStatue.Load_Dna:
                Game_Dna.LoadStart();
                break;
            case en_LoadStatue.Load_Enc:
                Game_Enc.LoadStart();
                break;
            case en_LoadStatue.Load_FjData:
                FjData.LoadStart();
                break;
            case en_LoadStatue.Load_JL:
                JL.LoadStart();
                break;
        }

    }

    public static bool IsReady()
    {
        if (statue == en_MainStatue.Game)
            return true;
        if (statue == en_MainStatue.LoadScene)
            return true;
        return false;
    }

    float ChangePanelSpeed;

    // 切换界面
    public void ChangePanelStart(GameObject panelOld, GameObject panelNew, int mode)
    {
        // 界面切换
        if (panelOld == null && panelNew == null)
            return;
        panelChanging = true;

        panelChangeMode = mode;
        panel_Old = panelOld;
        panel_New = panelNew;
        //
        ChangePanelSpeed = 600f;
        if (panel_Old != null)
        {
            panel_Old.transform.localPosition = new Vector3(0, 0, 0);
        }
        if (panel_New != null)
        {
            panel_New.transform.localPosition = new Vector3(Screen.width, 0, 0);
        }
    }
    //
    void ChangePanelRun()
    {
        if (panelChanging)
        {
            if (panel_Old != null)
            {
                panel_Old.transform.Translate(Vector3.right * Time.deltaTime * ChangePanelSpeed);
                if (panel_Old.transform.localPosition.x <= -Screen.width)
                {
                    panelChanging = false;
                    panel_Old.transform.localPosition = new Vector3(Screen.width, 0, 0);
                    if (panel_New != null)
                    {
                        panel_New.transform.localPosition = new Vector3(0, 0, 0);
                    }
                    return;
                }
            }
            if (panel_New != null)
            {
                panel_New.transform.Translate(Vector3.right * Time.deltaTime * ChangePanelSpeed);
                if (panel_New.transform.localPosition.x <= 0)
                {
                    panelChanging = false;
                    panel_New.transform.localPosition = new Vector3(0, 0, 0);
                    if (panel_Old != null)
                    {
                        panel_Old.transform.localPosition = new Vector3(Screen.width, 0, 0);
                    }
                    return;
                }
            }
            if (ChangePanelSpeed > -38000f)
            {
                ChangePanelSpeed -= Time.deltaTime * 38000f;
                if (ChangePanelSpeed < -38000f)
                {
                    ChangePanelSpeed = -38000f;
                }
            }
        }
    }

    public static bool IsOnButton(Image cursor, Image image)
    {
        if (cursor.transform.position.x < image.rectTransform.position.x - image.rectTransform.sizeDelta.x * image.transform.lossyScale.x / 2)
            return false;
        if (cursor.transform.position.x > image.rectTransform.position.x + image.rectTransform.sizeDelta.x * image.transform.lossyScale.x / 2)
            return false;
        if (cursor.transform.position.y < image.rectTransform.position.y - image.rectTransform.sizeDelta.y * image.transform.lossyScale.y / 2)
            return false;
        if (cursor.transform.position.y > image.rectTransform.position.y + image.rectTransform.sizeDelta.y * image.transform.lossyScale.y / 2)
            return false;
        return true;
    }

#if DEBUG_TEST || FPS_TEST //|| UNITY_EDITOR
    void OnGUI()//打印桌面
    {
        GUI.color = Color.red;
        GUI.Label(new Rect(300, 0, 300, 20), "分辨率: " + Screen.width.ToString() + "x" + Screen.height.ToString());
        GUI.Label(new Rect(50, 20, 300, 20), "VER: " + VERSION);
        GUI.Label(new Rect(50, 50, 200, 20), "FPS: " + fps.ToString());
        GUI.Label(new Rect(300, 50, 200, 20), "AllMemory: " + AllMemory.ToString());
        GUI.Label(new Rect(300, 80, 200, 20), "UseMemory: " + UseMemory.ToString());
        GUI.Label(new Rect(300, 110, 200, 20), "UnuseMemory: " + UnuseMemory.ToString());

        GUI.Label(new Rect(50, 110, 200, 20), "KEY_1: " + Key.Key_Old.ToString("X"));

        GUI.Label(new Rect(50, 150, 200, 20), "AD_0: " + Key.adValue[0].ToString());
        GUI.Label(new Rect(50, 180, 200, 20), "AD_1: " + Key.adValue[1].ToString());

        GUI.Label(new Rect(50, 220, 200, 20), "MinX_1: " + Key.adcMinXValue[0].ToString());
        GUI.Label(new Rect(50, 250, 200, 20), "MidX_1: " + Key.adcMidXValue[0].ToString());
        GUI.Label(new Rect(50, 280, 200, 20), "MaxX_1: " + Key.adcMaxXValue[0].ToString());

        for (int i = 0; i < JL.sero.Length; i++)
        {
            GUI.Label(new Rect(1000, 80 + i * 30, 300, 20), "JL_Sero_" + i.ToString() + ": " + JL.sero[i].ToString());
        }

    }
#endif
    public static void FormatImageSizeFollowSprite(Image image)
    {
        image.rectTransform.sizeDelta = new Vector2(image.sprite.texture.width, image.sprite.texture.height);
    }

    public static void Log(string str)
    {
        print(str);
    }

    public static int IndexOfArray(int[] array, int value)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == value)
            {
                return i;
            }
        }
        return -1;
    }

    // 判断分数是否够玩
    public static bool IsCanGamePlay(int playerno)
    {
        if (Set.setVal.PlayerMode == (int)en_PlayerMode.One && playerno > 0)
            return false;
        if (Set.setVal.InOutMode == (int)en_InOutMode.OneInOneOut && Set.setVal.PlayerMode == (int)en_PlayerMode.Two)
        {
            // 单投模式
            playerno = 0;
        }

        if (FjData.g_Fj[playerno].Coins >= Set.setVal.StartCoins)
        {
            return true;
        }

        return false;
    }

    // 扣币是否成功
    public static bool DecStartCoin(int playerno)
    {
        if (Set.setVal.PlayerMode == (int)en_PlayerMode.One && playerno > 0)
            return false;
        int no;
        if (Set.setVal.InOutMode == (int)en_InOutMode.OneInOneOut && Set.setVal.PlayerMode == (int)en_PlayerMode.Two)
        {
            // 单投模式
            no = 0;
        }
        else
        {
            no = playerno;
        }
        if (FjData.g_Fj[no].Coins >= Set.setVal.StartCoins)
        {
            FjData.g_Fj[no].Coins -= Set.setVal.StartCoins;
            FjData.SaveData_Coins(no);
            int coin = Set.setVal.StartCoins;
            if (Set.setVal.OutMode == (int)en_OutMode.OutTicket)
            {
                coin *= Set.setVal.TicketBl;
            }
            JL.PushCoin(playerno, coin);
            return true;
        }
        return false;
    }

    // 结算奖励
    public static void JieSuanScore(int playerno)
    {
        FjData.g_Fj[playerno].Scores = 0;
    }

    public static bool IsPlayerPlaying()
    {
        for (int i = 0; i < MAX_PLAYER; i++)
        {
            if (FjData.g_Fj[i].Playing)
            {
                return true;
            }
        }
        return false;
    }

}
