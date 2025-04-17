using UnityEngine;

public enum en_PlayerIO
{
    LED1 = 0,
    LED2,
    LED3,
    LED4,
    SSR1,
    SSR2,
}

public class IO
{
    public const int MAX_IO_ONEPLAYER = 5;

    // 玩家按键脚位定义 
    //	public const int IO_PLAYER_LED1 	= (int)en_PlayerIO.LED1;
    //	public const int IO_PLAYER_LED2 	= (int)en_PlayerIO.LED2;
    //	public const int IO_PLAYER_MK1		= (int)en_PlayerIO.MK1;
    //	public const int IO_PLAYER_MK2		= (int)en_PlayerIO.MK2;
    //	public const int IO_PLAYER_SSR		= (int)en_PlayerIO.SSR;

    //
    static byte IO_Statue;
    static bool sendChange;
    static float sendTime;
    public static bool[] gunMotorSta = new bool[Main.MAX_PLAYER];

    static int gunCnt;
    public static bool gunSta;
    //
    public static void Init()
    {
#if IO_LOCAL
        gunCnt = 0;
        gunSta = false;
        Motor_Out(gunSta);
        MotorOld_Out(false);
#endif
        Update((int)en_PlayerIO.LED1, false);
        Update((int)en_PlayerIO.LED2, false);
        Update((int)en_PlayerIO.LED3, false);
        Update((int)en_PlayerIO.LED4, false);
        Update((int)en_PlayerIO.SSR1, false);
        Update((int)en_PlayerIO.SSR2, false);

        CmdIO.CMD0_SendCmd_IO(IO_Statue);
#if !IO_LOCAL
        for (int i = 0; i < Main.MAX_PLAYER; i++)
        {
            GunMotorStop(i);
            ButtonLED(i, 0);
        }
#endif
    }

    public static void CheckSend()
    {
        // 200ms 发一次
        if (Time.time - sendTime >= 0.2f)
        {
            sendChange = true;
        }

        //if (sendChange) {
        //    sendChange = false;
        //    CmdIO.CMD0_SendCmd_IO(IO_Statue);
        //    sendTime = Time.time;
        //}
#if GUN_HW
        if (sendChange)
        {
            sendChange = false;
            sendTime = Time.time;
            for (int i = 0; i < gunMotorSta.Length; i++)
            {
                if (gunMotorSta[i] && FjData.g_Fj[i].Playing)
                {
                    GunMotorStart(i);
                }
                else
                {
                    GunMotorStop(i);
                }
            }
        }
#endif
    }

    public static void Update(int key, bool enable)
    {
        if (key < MAX_IO_ONEPLAYER)
        {
            if (enable == false)
            {
                IO_Statue &= (byte)~(1 << key);
            }
            else
            {
                IO_Statue |= (byte)(1 << key);
            }
            sendChange = true;
        }
    }
    //
    public static void Out_LED(int no, byte enable)
    {
        //Main.Log("LDE_"+no.ToString() + " :" + enable.ToString());
        CmdIO.CMD0_SendCmd_LED((byte)no, enable);
        CmdIO.CMD0_SendCmd_LED((byte)no, enable);
    }
    //
    public static void Out_SSR(int no, byte enable)
    {
        //Main.Log("SSR_" + no.ToString() + " :" + enable.ToString());
        CmdIO.CMD0_SendCmd_SSR((byte)no, enable);
        CmdIO.CMD0_SendCmd_SSR((byte)no, enable);
    }

    //
    public static void Test()
    {
        for (int i = 0; i < 8; i++)
        {
            if (Key.KEY_Pressed(i))
            {
                IO_Statue = (byte)(1 << i);
                sendChange = true;
            }
        }
    }

    //
    /*
    public static void GunMotorStart(int playerno, float time)
    {
        if (gunMotorDcTime[playerno] > 0)
            return;
        if (playerno < Main.MAX_PLAYER && time > 0)
        {
            Out_LED(playerno * 2, 1);
            Out_LED(playerno * 2 + 1, 1);
            gunMotorTime[playerno] = time;
            gunMotorDcTime[playerno] = 0.1f;
        }
    }
     * */

#if IO_LOCAL
    public static void ButtonLED(int playerno, byte power)
    {

    }
    public static void GunMotorStart(int playerno)
    {
#if IO_GUN1
#else
        Motor_Out(true);
#endif

    }
    public static void GunMotorStop(int playerno)
    {
#if IO_GUN1
#else
        Motor_Out(false);
#endif

    }
    public static void GunRunStart(int playId, int time, int time2)
    {
#if IO_GUN1
        CmdIO_GUN1.CMD0_SendCmd_GunStart((byte)time, (byte)time2);
        gunMotorSta[playId] = true;
#else
        gunCnt = 3;
#endif
    }
    public static void GunRunStop(int playId)
    {
#if IO_GUN1
        CmdIO_GUN1.CMD0_SendCmd_GunStop();
        gunMotorSta[playId] = false;
#else
        gunCnt = 0;
#endif
        //gunSta = false;
        //Motor_Out(gunSta);
    }
    // 50 ms 一次
    //------------------------------------------------------------------
    //输出路径
    const string IoPath_Motor = "/sys/class/gpio_sw/PG13/data";
    const string IoPath_Motor_Old = "/sys/class/gpio_sw/PG4/data";
    const string IoPath_MB = "/sys/class/gpio_sw/PG3/data";
    //
    //输入脚路径：
    readonly static string[] tab_GPIO_InPath = {
        "/sys/class/gpio_sw/PD15/data",
        "/sys/class/gpio_sw/PD16/data",
        "/sys/class/gpio_sw/PM0/data",
        "/sys/class/gpio_sw/PM1/data",
        "/sys/class/gpio_sw/PM2/data",
        "/sys/class/gpio_sw/PM3/data",
        "/sys/class/gpio_sw/PM4/data",
    };
    public static void CheckKey()
    {

        ulong key = 0;
        for (int i = 0; i < 7; i++)
        {
            if (Key_Read(i) != 0)
            {
                key |= (ulong)(1 << i);
            }
        }
        Key.KEY_Update(key);
    }
    static int Key_Read(int no)
    {
        if (no >= tab_GPIO_InPath.Length)
            return 1;
        return GPIO_Read(tab_GPIO_InPath[no]);
    }

    public static void Motor_Out(bool enable)
    {
#if UNITY_EDITOR
        //Debug.Log("Gun: " + enable);
#endif
#if IO_GUN1
#else
        IO_Out(IoPath_Motor, enable);
#endif

    }

    public static void MotorOld_Out(bool enable)
    {
#if UNITY_EDITOR
        Debug.Log("GunOld: " + enable);
#else
        IO_Out(IoPath_Motor_Old, enable);
#endif
    }

    public static void MB_Out(bool enable)
    {
        //IO_Out(IoPath_MB, enable);
    }

    static void IO_Out(string ioPath, bool enable)
    {
        if (File.Exists(ioPath) == false)
        {
            Debug.Log("IoPath is not exist!");
            return;
        }
        try
        {
            StreamWriter streamWriter = new StreamWriter(ioPath);
            try
            {
                if (enable)
                {
                    streamWriter.Write("1");
                }
                else
                {
                    streamWriter.Write("0");
                }
            }
            finally
            {
                streamWriter.Close();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("IO Fails: " + e);
        }
    }


    static int GPIO_Read(string path)
    {
        if (File.Exists(path) == false)
        {
            //Debug.Log("Not Read path: " + path);
            return 1;
        }
        try
        {
            StreamReader streamReader = new StreamReader(path);
            try
            {
                string str = streamReader.ReadLine();
                streamReader.Close();
                //Debug.Log("Read: " + str);
                if (str == "0")
                    return 0;
                return 1;
            }
            catch
            {
                return 1;
            }
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("ReadError: " + e);
            return 1;
        }
    }
#else
    public static void GunMotorStart(int playerno)
    {
        if (playerno >= Main.MAX_PLAYER)
            return;
        Out_LED(playerno * 2, 1);
        Out_LED(playerno * 2 + 1, 1);
        gunMotorSta[playerno] = true;
        //Debug.Log("玩家" + playerno + "开");
    }
    public static void GunMotorStop(int playerno)
    {
        if (playerno >= Main.MAX_PLAYER)
            return;
        Out_LED(playerno * 2, 0);
        Out_LED(playerno * 2 + 1, 0);
        gunMotorSta[playerno] = false;
        //Debug.Log("玩家" + playerno + "关");
    }

    public static void GunRunOnce(int playerno, byte runtime, byte dctime)
    {
        CmdIO.CMD0_SendCmd_GunOnce((byte)playerno, runtime, dctime);
    }
    public static void GunRunStart(int playerno, byte runtime, byte dctime)
    {
        CmdIO.CMD0_SendCmd_GunStart((byte)playerno, runtime, dctime);
        gunMotorSta[playerno] = true;
    }
    public static void GunRunStop(int playerno)
    {
        CmdIO.CMD0_SendCmd_GunStop((byte)playerno);
        gunMotorSta[playerno] = false;
    }
    public static void ButtonLED(int playerno, byte power)
    {
        CmdIO.CMD0_SendCmd_ButtonLED((byte)playerno, power);
        CmdIO.CMD0_SendCmd_ButtonLED((byte)playerno, power);
    }
#endif
}
