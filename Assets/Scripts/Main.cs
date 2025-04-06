
#define TEST_IN_WINDOW

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Profiling;


public enum en_MainStatue
{
    Restart = -1,       // 开机
    Game_00 = 0,
    Game_01,
    Game_02,
    Game_03,
    Game_04,
    Game_05,
    Game_06,
    Game_07,
    Game_08,
    Game_09,
    Game_10,
    Game_11,
    Game_12,
    Game_13,
    Game_14,
    Game_15,
    Game_16,
    Game_17,
    Game_18,
    Game_19,
    Game_20,
    Game_21,
    Game_22,
    Game_23,
    Game_24,
    Game_25,
    Game_26,
    Game_27,
    Game_28,
    Game_29,
    Game_30,
    Game_31,
    Game_32,
    Game_33,
    Game_34,
    Game_35,
    Game_36,
    Game_37,
    Game_38,
    Game_39,
    Game_40,
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
    // 分辨率设置
    public static bool FREE_SIZE = false;	// 自适应分辨率
    //默认分辨率
    public const float DEFAULT_SCREEN_WIDTH = 1280;
    public const float DEFAULT_SCREEN_HEIGHT = 720;
    //目标分辨率
    public const float TARGET_SCREEN_WIDTH = 1280;
    public const float TARGET_SCREEN_HEIGHT = 720;


    //------------------------------------------------------------------------------------------
    //    public static bool TestPC = false;
    // 版本(固定语言)
    /*
    v07:    1. 修改数据保存为铁电.
            2. 修改几外英文问题
            3. 点击小地图进入游戏。 

    v09:    1. 修改，SSR1和SSR2反过来.
            2. 修改, 单投模式，只显示一个"请投币"和投币数.
            3. 修改, 娱乐模式，币转化成时间数时，赠送5个炸弹.

            4. 游戏过关时，随机下一关，改为固定一下关，
            5. (娱乐模式)炸弹炸死BOSS时，其它小怪没有一起死。
            6. (娱乐模式)在DEMO模式下，不出扣血数字，暴击数字，得分数字。
            7. (娱乐模式)在游戏时，不出扣血数字，暴击数字。

    v10:    1. 加回v09去掉的数字.
            2. 所有2D精灵，UI等预制体，脚本绑定的对象都补齐，为空时可能会增加内存。
            3. 数字脚本：不显示的数字图片，SetActive(false)

    v11:    1. 其中一人游戏结束就开始退礼品
            2. 扭蛋比率最大上调到3000。（原来是1000）
           *3. (CGW) 1P和2P 退票信号调换。
          
    v12:    1. 修改：娱乐版：在切换游戏时分数清零的问题；
            2. 修改：娱乐版：在游戏过关时，没有玩的玩家也加了过关分数的问题； 
            3. 修改：加载数据时，LOGO没有显示语言的问题。
            4. 修改：娱乐版：炸弹或箱子炸死BOSS时，小怪全死时出错的问题；   
                 
    v14:    1. 添加设置项“背景音乐”,背景音乐不受音量大小控制
            2. 添加：无代理LOGO版本。

    v02.01  (上一版本：01.14)
            1. 修改：游戏加载慢的问题：多个场景合成一个场景，每个游戏单独做一个资源Prefab.

    v02.04  1. 添加更换游戏Logo的方法

    v02.05  1. 修改cmdIO 指令解析越界出错的问题.
            2. 水枪机模式下，炸弹1秒后自动扔出。

    v01.02.06   1. 增加P1 、P2功能
    
    v01.02.07   1. 修改：除了普通枪之外的其它枪，在枪模式时，都加震动

    v01.02.08   1. 修改：其它枪的震动速度

    v01.02.10   1. 修改：待机音乐开关无效的问题
                2. 修改：参数设置、游戏选择界面，默认按键时，会清零数据的问题
                3. 添加：中英文可调功能
                4. 修改：清总账目没更新显示的问题；清总账目没清几率的问题

    v01.02.11   1. 《枪神》修改：选择界面作为待机界面

    v01.02.12   1. 《枪神》修改：退彩票/扭蛋，放在玩家死亡时开始退。

    v01.02.13   1. 《枪神》修改：游戏结束后，回到待机界面的问题(应该回到选择界面)
    v01.02.15    1.《寸土必争》场景读秒结束卡界面  不随机进入场景
    v01.02.15      1.09场景   卡点问题
    01.02.17           1.修改设置中 背景音乐 为 待机音乐
    01.02.17      1.桌面待机音乐 首次进入游戏正常  后面默认开启问题      2.默认音量为50  修改重启后恢复50
    01.02.18       破解版

    01.02.19    (2020-5-12)1.修改：吃到血包增加的时间由原来的1/5改为固定10秒；以解决设置为5分钟以上时间时，可以大量延长游戏时间的问题；
                2.修改：没选择游戏（可能因为没校枪）自动进入游戏时，可能会闪退的问题;

    01.02.20    (2020-5-20)1.修改：（射珠版）打中宝箱时，停止射珠；

    01.02.21    (2020-6-13)1.添加《神枪降临》
    01.02.22    (2020-6-20)1.添加《枪圣》(9合1)
                    2.修改：游戏加载完成后，先激活场景，再切换游戏；
                    3.修改：游戏选择，第一次没有时，默认改为1.

    01.02.23    (2020-07-29)1.修改：所有专版，都加上，玩家死亡才退奖励

    01.02.27    (2020-12-14)
                1.修改：声音文件修改为流播放
                2.修改：“荒野小妖”地图卡怪的问题: 近程怪获取不到攻击点造成的；在生成怪前，生判断是否可以获取攻击点，再生成;gameMain 初始化时获取对应怪的属性

    01.02.28    (2020-12-16)
                1.修改：“动物森林”怪贴图大小为512x512
                2.恢复 v01.02.07-2的修改；修改为:近程怪在RunOut状态下，可重新找回攻击点；

    01.02.29    (2020-12-16)
                1. 添加：游戏选择全关时提示，加默认游戏；

    01.02.30    (2021-03-04)
                1.修改：枪校准，添加实时显示ADC值
                2.修改：枪校准，添加ADC中间值

    01.02.31    (2021-03-15)
                1.修改：寸土必争，游戏选择设置编号不对的问题；
                2.添加：专版“神枪手”
                2.添加：专版“CX-战警”

    01.02.32    (2021-03-31)
                1.修改：专版“CX-战警”LOGO坐标，底图尺寸；请投币坐标

    01.04.01    (2021-08-03)
                1.修改：寸土必争：删除“冰雪机械”地图；
                2.修改：选择界面，地图排列的逻辑；
                3.修改：过关标志逻辑；
                4.修改：整理地图图片和名字图片；

    01.04.02    (2021-11-26)
                1.修改：设置界面风格；
                2.修改：怪看不见卡住的问题（超时强制到下一路径点）;

    01.04.03    (2021-12-04)
                1.修改：增加出厂密码提示；
                2.修改：参数设置/游戏选择/礼品设置，初始化排序的问题；
                3.修改：IO检测，按键信息出错的问题；

    01.04.04    (2021-12-07)
                1.修改：单人模式，开始币数为0时，2P光标显示的问题；
    01.04.05    (2022-02-15)
                1.添加：专版“枪王”；

    01.04.06    (2022-03-10)
                1.修改：（枪王专版）默认值改为：玩家模式=单人模式; 游戏时间=5分钟；
                2.修改：（枪王专版）更换普通枪开火声音

    01.04.07    (2022-05-31)
                1.添加：设置项“震动力度”(每次震动，电磁铁打开的时间(ms))；
                2.添加：设置项“震动时间”(震动周期，电磁铁打开+间隔的时间(ms))；

    01.04.08    (2023-03-08)
                1.添加：寸土必争-扫码版；
    01.04.09    (2024-1-8)
                1.修改：射球精灵logo和背景
    01.04.10    (2024-05-20)
                1.添加：红外枪控制[GUN_HW]，只有普通枪模式;
    01.04.11    (2024-08-06)
                1.添加：海燕枪神5加1一体机(IO_LOCAL,IO_GUN1)
    01.04.12    (2024-08-15)
                1.修改：按枪确定切换视频
                2.修改：第一次进mp5时，视频随机
                3.修改：最少从0001播放视频，修复播0000会报错
    01.04.13    (2024-08-16)
                1.修改：修复按枪确定切换视频
    01.04.14    (2024-08-16)
                1.修改：按切枪切换视频
                2.添加：后台设置mp5关中枪震动开关，MP5中枪震动功能
    01.04.15    (2024-08-16)
                1.修改：海燕枪神5+1一体机未投币时选择界面标题改成枪神6合1，去除左上角小logo
    01.04.16    (2024-09-18)
                1.修改：初始化音量，修复关机不能保存音量的bug
    01.04.17    (2024-09-27)
                1.添加：寸土必争换皮星际射击，更换ui。
                2.修改：更换关卡顺序，修复game01和game02关卡ui未更换的问题。
                3.修改：选择关卡背景音乐。
    01.04.18    (2024-10-11)
                1.添加：寸土必争换皮未来战线，更换ui
                2.添加：后台设置4种枪的震动力度和震动时间
    01.04.19    (2024-10-29)
                1.添加：未来战线英文版
    01.04.20    (2024-11-27)
                1.修改：修改单投单退投币进游戏逻辑，当币数够玩时，需要玩家按下确认键后才扣币进行游戏。
                2.添加：关卡内中途加入
                3.修改：单投单退投币位置
    01.04.21    (2025-1-3)
                1.修改：海燕一体机声音调小，和视频音量差不多
    01.04.22    (2025-1-11)
                1.修改：海燕一体机添加儿歌，后台设置开关儿歌
    01.04.23    (2025-1-13)
                1.修改：海燕一体机儿歌声音调小，宏定义开IO_GUN1

    */



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
    public static string VERSION = GUN_VER + "01.04.23";
    //
    public const bool VER_DNA = false;
    public const bool VER_ENC = false;

    // 
    public const bool KEY_ESC_QUITE = false; // 按"ESC"键可退出程序
    //	public const bool 
    //公司类别 
    public const int COMPANY_NUM = 3;
    //0:公版:logo:《枪皇》,{0,3,4,5,7,8,9,10}
    //1:海燕电子,logo:《枪神》,{0,3,4,5,8,10,12,16}
    //2:王军,logo:《枪圣》,{0,3,4,5,7,8,9,10}
    //3.寸土必争
    //4.神枪降临
    //5.枪圣(9合1)
    //6.神枪手(漫威科技)(加了一个开机画面)
    //7.CX-战警(漫威科技)(加了一个开机画面)
    //8.枪王
    //9.寸土必争（请扫码）
    //10.射球精灵
    //11.寸土必争换皮（星际射击）
    //12.寸土必争换皮（未来战线）

    //20合1
    //public readonly static int[] tab_GameId = { 0, 3, 4, 5, 7, 8, 9, 10, 13, 14, 12, 16, 19, 1, 2, 6, 11, 15, 17, 18 };
    //公版（枪皇）,枪圣,
    //public readonly static int[] tab_GameId = { 0, 3, 4, 5, 7, 8, 9, 10, };
    //枪神
    //public readonly static int[] tab_GameId = { 0, 3, 4, 5, 8, 10, 12, 16};
    //寸土必争；6.神枪手；7.CX-战警；8.枪王
    //public readonly static int[] tab_GameId_Test = { 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20 };
#if NEW_IO
    //public readonly static int[] tab_GameId = { 2, 6, 11, 15, 17, 18, 20, 3, 4, 5, 7, 8, 9, 10, 13, 14, 12, 16, 19, 1 };
    public readonly static int[] tab_GameId = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 };
#elif IO_LOCAL
    public readonly static int[] tab_GameId = { 4, 7, 12, 15, 19, 96 };
#endif
    //public readonly static int[] tab_GameId_v32 = { 2, 6, 11, 15, 17, 18, 20, 0, 3, 4, 5, 7, 8, 9, 10, 13, 14, 12, 16, 19, 1 };    
    //public readonly static int[] tab_GameId = { 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40 };
    //神枪降临/神枪手
    //public readonly static int[] tab_GameId = { 3, 1, 20, 6, 8, 9, 12, 16, 0, 4 };
    // 枪圣(9合一)
    //public readonly static int[] tab_GameId = { 2, 5, 7, 10, 11, 13, 14, 15, 17 };


    public const int MAX_PLAYER = 2;
    public const int MAX_MONSTER = 20;
    public const int MAX_USER_VIDEO = 1;
    // -----------
    public Canvas gameUI_Panel;
    public GameObject camera_Obj;
    public GameObject loading_Obj;
    public GameObject game97_Obj;
    public Menu menu;
    public GameObject volumeCotroy_Prefab;
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

    //
    // 其它脚本    
    //	public static GameUI gameUI;


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
    //public static bool[] enableAutoOut = new bool[MAX_PLAYER];   //
    //public static bool[] enableAutoOutOld = new bool[MAX_PLAYER];    // 切换场景前的状态
    public static bool[] coinInChanged = new bool[MAX_PLAYER];   //
    public static bool[] outNumChanged = new bool[MAX_PLAYER];   // 退数字已改变
    public static bool[] bombNumChanged = new bool[MAX_PLAYER];  // 炸弹数改变
    public static bool[] gameStarted = new bool[MAX_PLAYER];    // 游戏开始标志（计算奖励用到）
    public static int destroyId;

    public static bool[] gamePassed = new bool[tab_GameId.Length];
    public static bool gameOver = false;
    public static int[] buffScenes = new int[tab_GameId.Length];//22
    public static bool isFirstPlayVideo;//第一次播放视频
    public static bool isSelectLevel;//是否在选关中，用来设置投币位置
    //
    float runTime;
    int sceneNum = 0;
    string sceneName = "";

    // 界面切换
    [HideInInspector]
    public bool panelChangeing = false;
    [HideInInspector]
    public int panelChangeMode;					// 切换模式
    [HideInInspector]
    public GameObject panel_Old;				// 老界面
    [HideInInspector]
    public GameObject panel_New;                // 新界面

    public static int receiveDataNum = 0;       // 接收数据个数
    public static byte readAddr;
    public static byte addr;
    GameObject volumeCotroy;
    //
    void Awake()
    {
        menu.Awake0(this);
        VolumeCotroy.Init();
        //
        GameObject prefab;
        if (COMPANY_NUM == 4 || COMPANY_NUM == 5)
        {
            Destroy(loading_Obj);
            prefab = Resources.Load<GameObject>("Company_" + COMPANY_NUM.ToString("D2") + "/Prefab/LoadingScene");
            loading_Obj = Instantiate(prefab);
            loading_Obj.transform.localPosition = Vector3.zero;
            game_LoadScene = loading_Obj.GetComponent<Game_LoadScene>();
        }
        if (COMPANY_NUM == 5)
        {
            // game97
            Destroy(game97_Obj);
            prefab = Resources.Load<GameObject>("Company_" + COMPANY_NUM.ToString("D2") + "/Prefab/Game97");
            game97_Obj = Instantiate(prefab);
        }
        if (COMPANY_NUM == 8)
        {
            image_LoadingBackG.sprite = Resources.Load<Sprite>("Company_" + COMPANY_NUM.ToString("D2") + "/Pic/Game97/Idle/BackG");
        }
        if (COMPANY_NUM == 10)
        {
            image_LoadingBackG.sprite = Resources.Load<Sprite>("Company_" + COMPANY_NUM.ToString("D2") + "/Pic/Game97/Idle/BackG");
        }
        if (COMPANY_NUM == 11)//加载界面背景
        {
            image_LoadingBackG.sprite = Resources.Load<Sprite>("Company_" + COMPANY_NUM.ToString("D2") + "/Pic/Game97/Idle/BackG");
        }
        if (COMPANY_NUM == 12)//加载界面背景
        {
            image_LoadingBackG.sprite = Resources.Load<Sprite>("Company_" + COMPANY_NUM.ToString("D2") + "/Pic/Game97/Idle/BackG");
        }
        //Debug.Log ("流"+1);
        game97_Main = game97_Obj.GetComponentInChildren<Game97_Main>();
        //	Debug.Log ("流"+2);
        game97_Main.Awake0(this);
        //Debug.Log ("流"+3);
        audioSource_CoinIn = sound_CoinIn.GetComponent<AudioSource>();
#if IO_LOCAL
        audioSource_CoinIn.volume = 0.6f;
#endif
        isFirstPlayVideo = true;
    }
    //
    void Awake0()
    {
        //	gameUI = gameUI_Panel.GetComponent<GameUI> ();
        // 声音
        audioSource_CoinIn = sound_CoinIn.GetComponent<AudioSource>();
        //
        if (gameId == -1)
        {
            Set.LoadAll();
            JL.Init();
        }
        //
        //statue = en_MainStatue.Loading;

    }


    void Start()//开始事件
    {
        //print("main Start!");
        //print("fxa: " + Vector3.Cross(Vector3.forward, new Vector3(-1, 0, 1)));
        //print("fxb: " + Vector3.Cross(Vector3.forward, new Vector3(1, 0, 1)));
        //print("fxc: " + Vector3.Cross(Vector3.forward, new Vector3(-1, 0, -1)));
        //print("fxd: " + Vector3.Cross(Vector3.forward, new Vector3(1, 0, -1)));

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
        //FjData.g_Fj[0].Wins = 20;
        //FjData.g_Fj[0].Coins = 20;
        //FjData.g_Fj[1].Coins = 20;

        //FjData.g_Fj[0].Wins = 30;
        //FjData.g_Fj[1].Wins = 30;
#else
         //FjData.g_Fj[0].Coins = 20;
#endif


        //#if UNITY_EDITOR
        // ChangeScene(en_MainStatue.Game_01);
        //#endif
    }

    int testTime = 0;
    int testTimeCnt = 0;
    void FixedUpdate()
    {
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
        //Debug.Log(FjData.g_Fj[0].Coins);
        //	cpuRatio = Profiler.GetTotalAllocatedMemory();
        //	gpuRatio = Profiler.GetTotalAllocatedMemory();
        AllMemory = Profiler.GetTotalReservedMemory() / 1000000;
        UseMemory = Profiler.GetTotalAllocatedMemory() / 1000000;
        UnuseMemory = Profiler.GetTotalUnusedReservedMemory() / 1000000;
        //	memGpu = Profiler.GetTotalAllocatedMemory ();
        //帧数检测
        fpscnt++;
        fpstime += Time.deltaTime;
        if (fpstime >= 1.0f)
        {
            fpstime -= 1.0f;
            fps = fpscnt;
            fpscnt = 0;

            //     DebugConsole.instance.AddMessage("this is instance AddMessage test by cartzhang", "warning");
        }
        // 输出口测试
        //     OutIO_Test();
        //
        ChagnePaneRun();
        //
        IO.CheckSend();
        //
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
                if (Main.COMPANY_NUM == 4 || Main.COMPANY_NUM == 5)
                {
                    // 重新做的整个界面
                    game_LoadScene.Update_ProgressValue(async.progress);
                }
                else
                {
                    image_ProgressBarValue.fillAmount = async.progress;
                    image_ProgressBarValue1.value = async.progress;
                    Debug.Log("流" + 3 + statue);
                    //	image_Rocket [0].transform.localPosition = new Vector2 (0, 0);
                    float b = async.progress;
                    float a = 0;
                    a = 100 * b;
                    num_Rocket.UpdateShow((int)a);//小火箭百分数
                                                  //image_Rocket [0].transform.localPosition = new Vector2 ((int)a, 0);
                }
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
                    //if (gameId >= 0 && gameId < MAX_GAME) {
                    //    // 游戏中: 先清怪，清刷怪点，0.5秒再切换场景
                    //    gamePlay.EnterMenuStart();
                    //} else {
                    //    ChangeStatue (en_MainStatue.Game_98);
                    //}
                }
#if NEW_IO || IO_GUN1
                if (Key.MENU_Statue_Left() || Key.MENU_Statue_Right())
                {
                    if (volumeCotroy == null)
                    {
                        volumeCotroy = GameObject.Instantiate(volumeCotroy_Prefab, gameUI_Panel.transform);
                        volumeCotroy.transform.localPosition = new Vector3(0, 0, 0);
                    }
                }
#endif
                break;

        }

        //
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
        //Debug.Log ("流"+gameno);
        Resources.UnloadUnusedAssets();
        PoolManager.instance.PoolClear();
        switch (gameno)
        {
            case en_MainStatue.Game_00:
            case en_MainStatue.Game_01:
            case en_MainStatue.Game_02:
            case en_MainStatue.Game_03:
            case en_MainStatue.Game_04:
            case en_MainStatue.Game_05:
            case en_MainStatue.Game_06:
            case en_MainStatue.Game_07:
            case en_MainStatue.Game_08:
            case en_MainStatue.Game_09:
            case en_MainStatue.Game_10:
            case en_MainStatue.Game_11:
            case en_MainStatue.Game_12:
            case en_MainStatue.Game_13:
            case en_MainStatue.Game_14:
            case en_MainStatue.Game_15:
            case en_MainStatue.Game_16:
            case en_MainStatue.Game_17:
            case en_MainStatue.Game_18:
            case en_MainStatue.Game_19:
            case en_MainStatue.Game_20:
            case en_MainStatue.Game_21:
            case en_MainStatue.Game_22:
            case en_MainStatue.Game_23:
            case en_MainStatue.Game_24:
            case en_MainStatue.Game_25:
            case en_MainStatue.Game_26:
            case en_MainStatue.Game_27:
            case en_MainStatue.Game_28:
            case en_MainStatue.Game_29:
            case en_MainStatue.Game_30:
            case en_MainStatue.Game_31:
            case en_MainStatue.Game_32:
            case en_MainStatue.Game_33:
            case en_MainStatue.Game_34:
            case en_MainStatue.Game_35:
            case en_MainStatue.Game_36:
            case en_MainStatue.Game_37:
            case en_MainStatue.Game_38:
            case en_MainStatue.Game_39:
            case en_MainStatue.Game_40:
                isSelectLevel = false;
                async = SceneManager.LoadSceneAsync("Game_" + ((int)gameno).ToString("D2"), LoadSceneMode.Additive);
                //Debug.Log("Game_" + ((int)gameno).ToString("D2"));
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
                // Game_97 常驻内存
                //async = SceneManager.LoadSceneAsync("Game_97", LoadSceneMode.Single);
                break;
        }

        if (Main.COMPANY_NUM == 4 || Main.COMPANY_NUM == 5)
        {
            game_LoadScene.Update_ProgressValue(0);
        }
        else
        {
            image_ProgressBarValue.fillAmount = 0;
        }
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
            case en_MainStatue.Game_01:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_02:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_03:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_04:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_05:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_06:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_07:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_08:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_09:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_10:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_11:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_12:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_13:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_14:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_15:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_16:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_17:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_18:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_19:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_20:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_21:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_22:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_23:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_24:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_25:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_26:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_27:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_28:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_29:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_30:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_31:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_32:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_33:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_34:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_35:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_36:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_37:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_38:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_39:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_40:
                game00_Main = GameObject.Find("GameMain").GetComponentInChildren<Game00_Main>();
                game00_Main.Awake0(this);
                game00_Main.GameStart();
                break;
            case en_MainStatue.Game_96:
                Game96_Main game96_Main = GameObject.Find("Game96Main").GetComponentInChildren<Game96_Main>();
                game96_Main.Awake0(this);
                game96_Main.GameStart();
                break;
            case en_MainStatue.Game_97:
                //Debug.Log (111);
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
                if (Main.COMPANY_NUM == 4 || Main.COMPANY_NUM == 5)
                {
                    // 重新做的整个界面
                    game_LoadScene.GameStart();
                }
                break;
        }
    }


    public void ChangeStatue_To_GameIdle()
    {
        ChangeStatue(en_MainStatue.Game_97);
        //ChangeStatue(en_MainStatue.Game);
        //gameMain.ChangeStatue(en_GameStatue.Idle);
    }

    //
    public void ChangeScene()
    {
        print("ChangeScene: " + gameId.ToString());
        //    SceneManager.LoadScene ("Loading");
        // 切换场景前的状态
        //for(int i = 0; i < MAX_PLAYER; i++) {
        //    enableAutoOutOld[i] = enableAutoOut[i];
        //}
        ChangeStatue(en_MainStatue.LoadScene);
    }
    public static void PlaySound_CoinIn()
    {
        if (audioSource_CoinIn != null)
        {
            audioSource_CoinIn.Play();
        }
    }
    public static void UpdateOutNum(int playerno)
    {

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
        //print("Load_ChangeStatue : " + loadStatue.ToString());
    }



    //
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
        panelChangeing = true;

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
    void ChagnePaneRun()
    {
        if (panelChangeing)
        {
            if (panel_Old != null)
            {
                panel_Old.transform.Translate(Vector3.right * Time.deltaTime * ChangePanelSpeed);
                if (panel_Old.transform.localPosition.x <= -Screen.width)
                {
                    panelChangeing = false;
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
                    panelChangeing = false;
                    panel_New.transform.localPosition = new Vector3(0, 0, 0);
                    if (panel_Old != null)
                    {
                        panel_Old.transform.localPosition = new Vector3(Screen.width, 0, 0);
                    }
                    return;
                }
            }
            //
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
    void OnGUI()//打印桌面  修改
    {
        GUI.color = Color.red;
        GUI.Label(new Rect(300, 0, 300, 20), "分辨率: " + Screen.width.ToString() + "x" + Screen.height.ToString());
        GUI.Label(new Rect(50, 20, 300, 20), "VER: " + VERSION);
        GUI.Label(new Rect(50, 50, 200, 20), "FPS: " + fps.ToString());
        ////	GUI.Label (new Rect(50, 100, 200, 20), "exeStatue: " + exeStatue.ToString() );
        GUI.Label(new Rect(300, 50, 200, 20), "AllMemory: " + AllMemory.ToString());
        GUI.Label(new Rect(300, 80, 200, 20), "UseMemory: " + UseMemory.ToString());
        GUI.Label(new Rect(300, 110, 200, 20), "UnuseMemory: " + UnuseMemory.ToString());

        //GUI.Label(new Rect(300, 150, 200, 20), "testTime : " + testTime.ToString());
        //GUI.Label(new Rect(300, 200, 200, 20), "sceneNum : " + sceneNum.ToString());
        //GUI.Label(new Rect(300, 230, 200, 20), "sceneName : " + sceneName);

        //// Uart
        ////GUI.Label(new Rect(50, 80, 400, 20), "Uart: " + uartIO.portFd.ToString() + 
        ////    " ReceveCount: "+ uartIO.receiveCnt.ToString() +
        ////    " cmdCnt: "+ cmdIO.cmdReceiveCnt.ToString());

        //KEY:`
        //
        GUI.Label(new Rect(50, 110, 200, 20), "KEY_1: " + Key.Key_Old.ToString("X"));

        GUI.Label(new Rect(50, 150, 200, 20), "AD_0: " + Key.adValue[0].ToString());
        GUI.Label(new Rect(50, 180, 200, 20), "AD_1: " + Key.adValue[1].ToString());

        GUI.Label(new Rect(50, 220, 200, 20), "MinX_1: " + Key.adcMinXValue[0].ToString());
        GUI.Label(new Rect(50, 250, 200, 20), "MidX_1: " + Key.adcMidXValue[0].ToString());
        GUI.Label(new Rect(50, 280, 200, 20), "MaxX_1: " + Key.adcMaxXValue[0].ToString());

        //GUI.Label(new Rect(50, 250, 200, 20), "Win_0: " + FjData.g_Fj[0].Wins);
        //GUI.Label(new Rect(50, 280, 200, 20), "Win_0: " + FjData.g_Fj[1].Wins);

        //if (gamePlay != null) {
        //    GUI.Label(new Rect(50, 210, 200, 20), "AliveMonsterNum: " + gamePlay.aliveMonsterNum.ToString());
        //    GUI.Label(new Rect(50, 240, 200, 20), "exsitMonsterNum: " + gamePlay.exsitMonsterNum.ToString());
        //    GUI.Label(new Rect(50, 270, 200, 20), "monsterFreshPosId: " + gamePlay.monsterFreshPosId_Out.ToString());
        //}

        //JL:
        //GUI.Label(new Rect(1000, 50, 300, 20), "JL_TTL : " + JL.ttl.ToString());
        for (int i = 0; i < JL.sero.Length; i++)
        {
            GUI.Label(new Rect(1000, 80 + i * 30, 300, 20), "JL_Sero_" + i.ToString() + ": " + JL.sero[i].ToString());
        }
        //GUI.Label(new Rect(1000, 170, 300, 20), "JL_waveJk : " + JL.waveJk.ToString());
        //GUI.Label(new Rect(1000, 200, 300, 20), "JL_fpbl : " + JL.fpbl.ToString());
        //GUI.Label(new Rect(1000, 230, 300, 20), "JL_killJL : " + JL.killJL.ToString());

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
    public static int IndexOfArry(int[] arry, int value)
    {
        for (int i = 0; i < arry.Length; i++)
        {
            if (arry[i] == value)
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
        //		Debug.Log ("StartCoins:"+S
        //		Debug.Log ("Coins:"+FjData.g_Fj [playerno].Coins);
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
            //
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

        //if (Set.setVal.OutMode == (int)en_OutMode.OutTicket)
        //{
        //    if (FjData.g_Fj[playerno].Scores >= Set.setVal.TicketBl && Set.setVal.TicketBl > 0)
        //    {
        //        FjData.g_Fj[playerno].Wins += FjData.g_Fj[playerno].Scores / Set.setVal.TicketBl;
        //        FjData.SaveData_Wins(playerno);
        //    }
        //}
        //else if (Set.setVal.OutMode == (int)en_OutMode.OutGift)
        //{
        //    if (FjData.g_Fj[playerno].Scores >= Set.setVal.GiftBl && Set.setVal.GiftBl > 0)
        //    {
        //        int win = FjData.g_Fj[playerno].Scores / Set.setVal.GiftBl;
        //        if (win > 1)
        //            win = 1;
        //        FjData.g_Fj[playerno].Wins += win;
        //        FjData.SaveData_Wins(playerno);
        //    }
        //}

        //if (FjData.g_Fj[playerno].Scores > 0)
        //{
        //    FjData.g_Fj[playerno].Scores = 0;
        //    //FjData.SaveData_Scores(playerno);
        //}
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
