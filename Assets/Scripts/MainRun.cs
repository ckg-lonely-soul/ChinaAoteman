using System.Diagnostics;
using UnityEngine;


public class MainRun : MonoBehaviour
{
    //  串口
#if UNITY_EDITOR
    public static Uart_Windows uartIO = new Uart_Windows();
    public static Uart_Windows uartLYY = new Uart_Windows();
#elif UNITY_ANDROID
    public static Uart_Android uartIO = new Uart_Android();
    public static Uart_Android uartLYY = new Uart_Android();
    //public static Uart_Android uartIO01 = new Uart_Android ();
    public static Uart_Android uartGyro = new Uart_Android();
#elif UNITY_STANDALONE_WIN
    public static Uart_Windows uartIO = new Uart_Windows();
    //public static Uart_Windows uartIO01 = new Uart_Windows();
#endif


    Process[] processPro;
    float proTime = 0;
    bool isFocus = true;

#if FPS_TEST
    static void UartSendData(byte[] buf, int len) {

    }
#endif
    public static MainRun instance;
    //
    void Awake()
    {
        if (instance)
            instance = this;
        // 串口和协议初始化: 
        // 串口初始化


        SetCode setCode_IO;

        // 协议初始化: -----------------------------
#if IO_BBM
        setCode_IO =  CmdIO_BBM.LAN_SetCode;
        CmdIO_BBM.Init(uartIO.SendData);
#elif IO_CR
        setCode_IO =  CmdIO_CR.LAN_SetCode;
        CmdIO_CR.Init(uartIO.SendData);
#elif IO_GUN1
        setCode_IO = CmdIO_GUN1.LAN_SetCode;
        CmdIO_GUN1.Init(uartIO.SendData);
#else
        setCode_IO = CmdIO.LAN_SetCode;
        CmdIO.Init(uartIO.SendData);

        //#error  "*****未初始化IO指令*****"
#endif
#if FPS_TEST

#elif UNITY_EDITOR
#if !VER_3288
        uartIO.Init(4, 38400, setCode_IO);
        uartLYY.Init(3, 38400, LeYaoYao.SetCode);

#endif

        // uartIO01.Init(1, 115200, CmdIOGift.LAN_SetCode);


#elif VER_A33
        uartIO.Init(2, 38400, setCode_IO);
        uartLYY.Init(1, 38400, LeYaoYao.SetCode);
       // uartIO01.Init(1, 115200, CmdIOGift.LAN_SetCode);

        CmdIO.Init(uartIO.SendData);
#elif VER_3368
        uartIO.Init(3, 38400, CmdIO.LAN_SetCode);
       // uartIO01.Init(0, 115200, CmdIOGift.LAN_SetCode);
       
        CmdIO.Init(uartIO.SendData);
#elif VER_H6
        uartIO.Init(2, 38400, setCode_IO);
        uartLYY.Init(0, 38400, LeYaoYao.SetCode);
       // uartIO01.Init(0, 115200, CmdIOGift.LAN_SetCode);
#elif VER_S812
        uartIO.Init(2, 9600, CmdIO.LAN_SetCode);
        uartIO01.Init(3, 115200, CmdIOGift.LAN_SetCode);
      //  CmdIOGift.Init(uartIO01.SendData);

#elif VER_3288
        //uartIO.Init(2, 9600, CmdIO.LAN_SetCode);
      //  uartIO01.Init(1, 115200, CmdIOGift.LAN_SetCode);
        CmdIOGift.Init(uartIO01.SendData);

#else
        // #error  "*****未初始化串口*****"
#endif


#if FPS_TEST
        CmdIO.Init(UartSendData);
#else
#if !VER_3288
        CmdIO.Init(uartIO.SendData);
#endif
        // CmdIOGift.Init(uartIO01.SendData);
#endif
        // Main.Log("串口初始化");
    }

    void Start()
    {
        isFocus = true;
    }
    void OnDestroy()
    {
        //关闭串口
        uartIO.Close();
        uartLYY.Close();

        //   uartIO01.Close();
        //
        Resources.UnloadUnusedAssets();
    }
    // 2ms 一次
    //void FixedUpdate() {
    //    // IO板串口

    //}

    void Update()
    {

        // 加密板串口
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        //        Main.uartData.CheckRead();
#endif

#if FPS_TEST

#else
        uartIO.CheckRead();
        uartLYY.CheckRead();

        //    uartIO01.CheckRead();
#endif
#if VER_3288
        IO.IO_Check();         // RK3288
#endif
        //IO.CheckSend();
        //	pAction.CodeTableCheck ();
        //
#if UNITY_EDITOR
        Key.KeyTest_ForKeybord();
#endif
        //if (Main.IsReady()) {
        PAction.Check();
        //}
        //   CmdIOGift.LinkCheck();

        LeYaoYao.LYY_Check();
        //// 连接超时检测
        //// 串口1连接--------
        //if ((Main.mainErrorStatue & (1 << (int)en_MainErrorMask.CONNECT_TIMEOUT_BYTE_1)) == 0) {
        //    if (Main.connectByteTimeoutTime[0] > 0) {
        //        Main.connectByteTimeoutTime[0] -= Time.deltaTime;
        //    } else {
        //        Main.mainErrorStatue |= (1 << (int)en_MainErrorMask.CONNECT_TIMEOUT_BYTE_1);
        //        Key.Init();     // 断开连接，按键重置
        //    }
        //} else {
        //    if (Main.connectByteTimeoutTime[0] > 0) {
        //        Main.mainErrorStatue &= ~(uint)(1 << (int)en_MainErrorMask.CONNECT_TIMEOUT_BYTE_1);
        //    }
        //}
        //// 指令检测
        //if ((Main.mainErrorStatue & (1 << (int)en_MainErrorMask.CONNECT_TIMEOUT_CMD_1)) == 0) {
        //    if (Main.connectCmdTimeoutTime[0] > 0) {
        //        Main.connectCmdTimeoutTime[0] -= Time.deltaTime;
        //    } else {
        //        Main.mainErrorStatue |= (1 << (int)en_MainErrorMask.CONNECT_TIMEOUT_CMD_1);
        //    }
        //} else {
        //    if (Main.connectCmdTimeoutTime[0] > 0) {
        //        Main.mainErrorStatue &= ~(uint)(1 << (int)en_MainErrorMask.CONNECT_TIMEOUT_CMD_1);
        //    }
        //}
        // 串口2连接-----------
        //if ((Main.mainErrorStatue & (1 << (int)en_MainErrorMask.CONNECT_TIMEOUT_BYTE_2)) == 0) {
        //    if (Main.connectByteTimeoutTime[1] > 0) {
        //        Main.connectByteTimeoutTime[1] -= Time.deltaTime;
        //    } else {
        //        Main.mainErrorStatue |= (1 << (int)en_MainErrorMask.CONNECT_TIMEOUT_BYTE_2);
        //    }
        //} else {
        //    if (Main.connectByteTimeoutTime[1] > 0) {
        //        Main.mainErrorStatue &= ~(uint)(1 << (int)en_MainErrorMask.CONNECT_TIMEOUT_BYTE_2);
        //    }
        //}
        //// 指令检测
        //if ((Main.mainErrorStatue & (1 << (int)en_MainErrorMask.CONNECT_TIMEOUT_CMD_2)) == 0) {
        //    if (Main.connectCmdTimeoutTime[1] > 0) {
        //        Main.connectCmdTimeoutTime[1] -= Time.deltaTime;
        //    } else {
        //        Main.mainErrorStatue |= (1 << (int)en_MainErrorMask.CONNECT_TIMEOUT_CMD_2);
        //    }
        //} else {
        //    if (Main.connectCmdTimeoutTime[1] > 0) {
        //        Main.mainErrorStatue &= ~(uint)(1 << (int)en_MainErrorMask.CONNECT_TIMEOUT_CMD_2);
        //    }
        //}
        //
#if DEBUG_TEST || FPS_TEST  // 测试模式
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Screen.fullScreen)
                Screen.fullScreen = false;
            else
                Screen.fullScreen = true;
        }
#else
        //// 守护程序: 
        //proTime += Time.deltaTime;
        //if(proTime >= 0.5f) {
        //    proTime = 0;
        //    processPro = Process.GetProcessesByName("ProPro");
        ////    print("processPro: " + processPro.Length.ToString());
        //    if (processPro.Length == 0) {
        //        Application.Quit();
        //        return;
        //    }
        //}
        //// 退出全屏时：
        //if(Screen.fullScreen == false) {
        //    Application.Quit();
        //    return;
        //}
        ////// 当程序失去焦点时自动关闭
        ////if(isFocus == false) {
        ////    Application.Quit();
        ////}
#endif
    }

    void OnApplicationFocus(bool focus)
    {
#if !DEBUG_TEST
        //if(focus == false) {
        //    Application.Quit();
        //}
#endif
        //    isFocus = focus;
    }
}
