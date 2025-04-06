using UnityEngine;
using System.Collections;

#if NEW_IO
public enum KeyNum
{
    KEY_K1 = 0,
    KEY_K2,
    KEY_K3,
    KEY_K4,
    KEY_K5,
    KEY_K6,
    KEY_K7,
    KEY_K8,
    KEY_K9,
    KEY_K10,
    KEY_K11,
    KEY_K12,
    KEY_K13,
    KEY_CADD1,
    KEY_CDEC1,
    KEY_SIN3,
    KEY_CADD2,
    KEY_CDEC2,
    KEY_SIN6,
};
# elif IO_GUN1
public enum KeyNum
{
    KEY_K1 = 0,
    KEY_K2,
    KEY_K3,
    KEY_K4,
    KEY_K5,
    KEY_K6,
    KEY_K7,
    KEY_K8,
};
#else 
public enum KeyNum
{
    KEY_K1 = 0,
    KEY_K2,
    KEY_K3,
    KEY_K4,
    KEY_K5,
    KEY_K6,
    KEY_K7,
    KEY_K8,
    KEY_K9,
    KEY_K10,
    KEY_K11,
    KEY_SET1,
    KEY_SET2,
    KEY_SET3,
    KEY_CDEC,
    KEY_CADD1,
    KEY_CADD2,
};
#endif
public enum en_KeySta
{
    Left = 0,
    Right,
}
public class Key
{

    // 玩家按键脚位定义 
#if NEW_IO
    // player1
    public const int KEY_PLAYER1_OK = (int)KeyNum.KEY_K1;       // 开枪
    public const int KEY_PLAYER1_ADD = (int)KeyNum.KEY_K2;       // 换枪(炸弹)
    public const int KEY_PLAYER1_RESET = (int)KeyNum.KEY_K7;       // 修复
    public const int KEY_PLAYER1_CIN = (int)KeyNum.KEY_CADD1;    // 投币
    public const int KEY_PLAYER1_COUT = (int)KeyNum.KEY_CDEC1;    // 退票
                                                                  // player2
    public const int KEY_PLAYER2_OK = (int)KeyNum.KEY_K4;
    public const int KEY_PLAYER2_ADD = (int)KeyNum.KEY_K5;
    public const int KEY_PLAYER2_RESET = (int)KeyNum.KEY_K8;       // 修复
    public const int KEY_PLAYER2_CIN = (int)KeyNum.KEY_CADD2;
    public const int KEY_PLAYER2_COUT = (int)KeyNum.KEY_CDEC2;
    // 公用按键
    public const int KEY_BACK = (int)KeyNum.KEY_K9;


    // 主板按键脚位定义
    //	public const int KEY_MENU_UP 		= (int)KeyNum.KEY_K2;		// 上
    //	public const int KEY_MENU_DOWN 		= (int)KeyNum.KEY_K8;		// 下
    public const int KEY_MENU_LEFT = (int)KeyNum.KEY_K12;       // 左
    public const int KEY_MENU_RIGHT = (int)KeyNum.KEY_K13;      // 右
    public const int KEY_MENU_OK = (int)KeyNum.KEY_K11;     // 确认
    //	public const int KEY_MENU_CANCEL	= (int)KeyNum.KEY_K3;		// 取消
    //	public const int KEY_MENU_SMALLGAME	= (int)KeyNum.KEY_K6;		// 小游戏 
    // 分机生复用菜单按键
    public const int KEYFJ_MENU_LEFT = (int)KeyNum.KEY_K7;       // 左
    public const int KEYFJ_MENU_RIGHT = (int)KeyNum.KEY_K8;       // 右
    public const int KEYFJ_MENU_OK = (int)KeyNum.KEY_K10;       // 确认
#elif IO_GUN1

    public const int KEY_PLAYER1_OK = (int)KeyNum.KEY_K7;       // 开枪
    public const int KEY_PLAYER1_ADD = (int)KeyNum.KEY_K8;       // 换枪(炸弹)
    public const int KEY_PLAYER1_RESET = (int)KeyNum.KEY_K3;       // 修复
    public const int KEY_PLAYER1_CIN = (int)KeyNum.KEY_K1;    // 投币
    public const int KEY_PLAYER1_COUT = (int)KeyNum.KEY_K2;    // 退票
                                                               // player2
    public const int KEY_PLAYER2_OK = (int)KeyNum.KEY_K4;
    public const int KEY_PLAYER2_ADD = (int)KeyNum.KEY_K5;
    public const int KEY_PLAYER2_RESET = (int)KeyNum.KEY_K8;       // 修复
    public const int KEY_PLAYER2_CIN = (int)KeyNum.KEY_K1;
    public const int KEY_PLAYER2_COUT = (int)KeyNum.KEY_K2;
    // 公用按键
    public const int KEY_BACK = (int)KeyNum.KEY_K3;


    // 主板按键脚位定义
    //	public const int KEY_MENU_UP 		= (int)KeyNum.KEY_K2;		// 上
    //	public const int KEY_MENU_DOWN 		= (int)KeyNum.KEY_K8;		// 下
    public const int KEY_MENU_LEFT = (int)KeyNum.KEY_K4;       // 左
    public const int KEY_MENU_RIGHT = (int)KeyNum.KEY_K5;      // 右
    public const int KEY_MENU_OK = (int)KeyNum.KEY_K6;     // 确认
    //	public const int KEY_MENU_CANCEL	= (int)KeyNum.KEY_K3;		// 取消
    //	public const int KEY_MENU_SMALLGAME	= (int)KeyNum.KEY_K6;		// 小游戏 
    // 分机生复用菜单按键
    //public const int KEYFJ_MENU_LEFT = (int)KeyNum.KEY_K7;       // 左
    //public const int KEYFJ_MENU_RIGHT = (int)KeyNum.KEY_K8;       // 右
    public const int KEYFJ_MENU_OK = (int)KeyNum.KEY_K7;       // 确认
#else
#if PLAYER_ONE
    // player1
    public const int KEY_PLAYER1_OK = (int)KeyNum.KEY_K1;
    public const int KEY_PLAYER1_ADD = (int)KeyNum.KEY_K2;
    public const int KEY_PLAYER1_CIN = (int)KeyNum.KEY_CADD2;
    // player2
    public const int KEY_PLAYER2_OK = (int)KeyNum.KEY_K8;
    public const int KEY_PLAYER2_ADD = (int)KeyNum.KEY_K9;
    public const int KEY_PLAYER2_OUT = (int)KeyNum.KEY_K11;
    public const int KEY_PLAYER2_CIN = (int)KeyNum.KEY_CADD1;

    public const int KEY_PLAYER1_COUT = (int)KeyNum.KEY_CDEC;
    public const int KEY_PLAYER2_COUT = (int)KeyNum.KEY_K10;

    public const int KEY_PLAYER1_RESET = (int)KeyNum.KEY_K8;
    public const int KEY_PLAYER2_RESET = (int)KeyNum.KEY_K10;
    public const int KEY_BACK = (int)KeyNum.KEY_K3;


    // 主板按键脚位定义
    //	public const int KEY_MENU_UP 		= (int)KeyNum.KEY_K2;		// 上
    //	public const int KEY_MENU_DOWN 		= (int)KeyNum.KEY_K8;		// 下
    public const int KEY_MENU_LEFT = (int)KeyNum.KEY_SET2;      // 左
    public const int KEY_MENU_RIGHT = (int)KeyNum.KEY_SET3;     // 右
    public const int KEY_MENU_OK = (int)KeyNum.KEY_SET1;     // 确认
    //	public const int KEY_MENU_CANCEL	= (int)KeyNum.KEY_K3;		// 取消
    //	public const int KEY_MENU_SMALLGAME	= (int)KeyNum.KEY_K6;		// 小游戏 
    // 分机生复用菜单按键
    public const int KEYFJ_MENU_LEFT = (int)KeyNum.KEY_K4;       // 左
    public const int KEYFJ_MENU_RIGHT = (int)KeyNum.KEY_K5;       // 右
    public const int KEYFJ_MENU_OK = (int)KeyNum.KEY_K6;       // 确认
#else
    // player1
    public const int KEY_PLAYER1_OK = (int)KeyNum.KEY_K1;       // 开枪
    public const int KEY_PLAYER1_ADD = (int)KeyNum.KEY_K2;       // 换枪(炸弹)
    public const int KEY_PLAYER1_RESET = (int)KeyNum.KEY_K7;       // 修复
    public const int KEY_PLAYER1_CIN = (int)KeyNum.KEY_CADD2;    // 投币
    public const int KEY_PLAYER1_COUT = (int)KeyNum.KEY_K11;      // 退票
                                                                  // player2
    public const int KEY_PLAYER2_OK = (int)KeyNum.KEY_K8;
    public const int KEY_PLAYER2_ADD = (int)KeyNum.KEY_K9;
    public const int KEY_PLAYER2_RESET = (int)KeyNum.KEY_K10;      // 修复
    public const int KEY_PLAYER2_CIN = (int)KeyNum.KEY_CADD1;
    public const int KEY_PLAYER2_COUT = (int)KeyNum.KEY_CDEC;
    // 公用按键
    public const int KEY_BACK = (int)KeyNum.KEY_K3;

    // 主板按键脚位定义
    //	public const int KEY_MENU_UP 		= (int)KeyNum.KEY_K2;		// 上
    //	public const int KEY_MENU_DOWN 		= (int)KeyNum.KEY_K8;		// 下
    public const int KEY_MENU_LEFT = (int)KeyNum.KEY_SET2;      // 左
    public const int KEY_MENU_RIGHT = (int)KeyNum.KEY_SET3;     // 右
    public const int KEY_MENU_OK = (int)KeyNum.KEY_SET1;     // 确认
    //	public const int KEY_MENU_CANCEL	= (int)KeyNum.KEY_K3;		// 取消
    //	public const int KEY_MENU_SMALLGAME	= (int)KeyNum.KEY_K6;		// 小游戏 
    // 分机生复用菜单按键
    public const int KEYFJ_MENU_LEFT = (int)KeyNum.KEY_K4;       // 左
    public const int KEYFJ_MENU_RIGHT = (int)KeyNum.KEY_K5;       // 右
    public const int KEYFJ_MENU_OK = (int)KeyNum.KEY_K6;       // 确认
#endif  //PLAYER_ONE
#endif

    // 0-5: 玩家按键; 6: 主板按键

    //
    public static ulong Key_Down;
    public static ulong Key_Old;
    // 屏幕微调坐标(UI局部坐标) 
    public static int[] cursorPos_Left = new int[Main.MAX_PLAYER];
    public static int[] cursorPos_Right = new int[Main.MAX_PLAYER];
    public static int[] cursorPos_Up = new int[Main.MAX_PLAYER];
    public static int[] cursorPos_Down = new int[Main.MAX_PLAYER];
    //
    public static int[] screen_Width = new int[Main.MAX_PLAYER];
    public static int[] screen_Height = new int[Main.MAX_PLAYER];
    // 上下[0]; 左右:[1]
    //static int[] adMaxValue = new int[Main.MAX_PLAYER * 2];
    //static int[] adMinValue = new int[Main.MAX_PLAYER * 2];
    public static int[] adcMinXValue = new int[Main.MAX_PLAYER];
    public static int[] adcMidXValue = new int[Main.MAX_PLAYER];
    public static int[] adcMaxXValue = new int[Main.MAX_PLAYER];
    static int[] adcMinYValue = new int[Main.MAX_PLAYER];
    static int[] adcMidYValue = new int[Main.MAX_PLAYER];
    static int[] adcMaxYValue = new int[Main.MAX_PLAYER];
    public static int[] adValue = new int[Main.MAX_PLAYER * 2];

    static int[,] adBuf = new int[Main.MAX_PLAYER * 2, 10];
    static int[] adBufIn = new int[Main.MAX_PLAYER * 2];


    public static void Init()
    {
        LoadAdLimitValue();

        Key_Down = 0;
        Key_Old = 0xffffffff;
    }

    public static void Clear()
    {
        Key_Down = 0;
    }

    // ad save
    static void LoadAdLimitValue()
    {
        //for (int i = 0; i < adMaxValue.Length; i++) {
        //	adMaxValue[i] = PlayerPrefs.GetInt ("AD_MAX_"+i.ToString());
        //	adMinValue[i] = PlayerPrefs.GetInt ("AD_MIN_"+i.ToString());
        //}
        for (int i = 0; i < Main.MAX_PLAYER; i++)
        {
            cursorPos_Left[i] = PlayerPrefs.GetInt("CURSORPOS_LEFT_" + i.ToString());
            if (cursorPos_Left[i] < -Screen.width / 2 || cursorPos_Left[i] > -Screen.width / 2 + 200)
            {
                cursorPos_Left[i] = -Screen.width / 2;
            }
            cursorPos_Right[i] = PlayerPrefs.GetInt("CURSORPOS_RIGHT_" + i.ToString());
            if (cursorPos_Right[i] > Screen.width / 2 || cursorPos_Right[i] < Screen.width / 2 - 200)
            {
                cursorPos_Right[i] = Screen.width / 2;
            }
            cursorPos_Up[i] = PlayerPrefs.GetInt("CURSORPOS_UP_" + i.ToString());
            if (cursorPos_Up[i] > Screen.height / 2 || cursorPos_Up[i] < Screen.height / 2 - 200)
            {
                cursorPos_Up[i] = Screen.height / 2;
            }
            cursorPos_Down[i] = PlayerPrefs.GetInt("CURSORPOS_DOWN_" + i.ToString());
            if (cursorPos_Down[i] < -Screen.height / 2 || cursorPos_Down[i] > -Screen.height / 2 + 200)
            {
                cursorPos_Down[i] = -Screen.height / 2;
            }

            screen_Width[i] = cursorPos_Right[i] - cursorPos_Left[i];
            screen_Height[i] = cursorPos_Up[i] - cursorPos_Down[i];
        }
        for (int i = 0; i < Main.MAX_PLAYER; i++)
        {
            adcMinXValue[i] = PlayerPrefs.GetInt("ADC_MIN_X_" + i.ToString());
            adcMidXValue[i] = PlayerPrefs.GetInt("ADC_MID_X_" + i.ToString());
            adcMaxXValue[i] = PlayerPrefs.GetInt("ADC_MAX_X_" + i.ToString());
            adcMinYValue[i] = PlayerPrefs.GetInt("ADC_MIN_Y_" + i.ToString());
            adcMidYValue[i] = PlayerPrefs.GetInt("ADC_MID_Y_" + i.ToString());
            adcMaxYValue[i] = PlayerPrefs.GetInt("ADC_MAX_Y_" + i.ToString());
        }
    }
    static void SaveAdcMinX_Value(int no)
    {
        PlayerPrefs.SetInt("ADC_MIN_X_" + no.ToString(), adcMinXValue[no]);
        PlayerPrefs.Save();
    }
    static void SaveAdcMidX_Value(int no)
    {
        PlayerPrefs.SetInt("ADC_MID_X_" + no.ToString(), adcMidXValue[no]);
        PlayerPrefs.Save();
    }
    static void SaveAdcMaxX_Value(int no)
    {
        PlayerPrefs.SetInt("ADC_MAX_X_" + no.ToString(), adcMaxXValue[no]);
        PlayerPrefs.Save();
    }
    static void SaveAdcMinY_Value(int no)
    {
        PlayerPrefs.SetInt("ADC_MIN_Y_" + no.ToString(), adcMinYValue[no]);
        PlayerPrefs.Save();
    }
    static void SaveAdcMidY_Value(int no)
    {
        PlayerPrefs.SetInt("ADC_MID_Y_" + no.ToString(), adcMidYValue[no]);
        PlayerPrefs.Save();
    }
    static void SaveAdcMaxY_Value(int no)
    {
        PlayerPrefs.SetInt("ADC_MAX_Y_" + no.ToString(), adcMaxYValue[no]);
        PlayerPrefs.Save();
    }
    public static void SetAdc_MinX(int playerno)
    {
        if (playerno < Main.MAX_PLAYER)
        {
            adcMinXValue[playerno] = adValue[playerno * 2 + 1];
            SaveAdcMinX_Value(playerno);
        }
    }
    public static void SetAdc_MidX(int playerno)
    {
        if (playerno < Main.MAX_PLAYER)
        {
            adcMidXValue[playerno] = adValue[playerno * 2 + 1];
            SaveAdcMidX_Value(playerno);
        }
    }
    public static void SetAdc_MaxX(int playerno)
    {
        if (playerno < Main.MAX_PLAYER)
        {
            adcMaxXValue[playerno] = adValue[playerno * 2 + 1];
            SaveAdcMaxX_Value(playerno);
        }
    }
    public static void SetAdc_MinY(int playerno)
    {
        if (playerno < Main.MAX_PLAYER)
        {
            adcMinYValue[playerno] = adValue[playerno * 2 + 0];
            SaveAdcMinY_Value(playerno);
        }
    }
    public static void SetAdc_MidY(int playerno)
    {
        if (playerno < Main.MAX_PLAYER)
        {
            adcMidYValue[playerno] = adValue[playerno * 2 + 0];
            SaveAdcMidY_Value(playerno);
        }
    }
    public static void SetAdc_MaxY(int playerno)
    {
        if (playerno < Main.MAX_PLAYER)
        {
            adcMaxYValue[playerno] = adValue[playerno * 2 + 0];
            SaveAdcMaxY_Value(playerno);
        }
    }

    //----------
    //   static void SaveAdMaxValue(int no)
    //{
    //	PlayerPrefs.SetInt ("AD_MAX_"+no.ToString(), adMaxValue[no]);
    //	PlayerPrefs.Save ();
    //}
    //   static void SaveAdMinValue(int no)
    //{
    //	PlayerPrefs.SetInt ("AD_MIN_"+no.ToString(), adMinValue[no]);
    //	PlayerPrefs.Save ();
    //}

    public static void SaveCursorPos_Left(int playerno, int left)
    {
        if (playerno < Main.MAX_PLAYER)
        {
            cursorPos_Left[playerno] = left;           // 左
                                                       //
            PlayerPrefs.SetInt("CURSORPOS_LEFT_" + playerno.ToString(), cursorPos_Left[playerno]);
            PlayerPrefs.Save();
        }
    }
    public static void SaveCursorPos_Right(int playerno, int right)
    {
        if (playerno < Main.MAX_PLAYER)
        {
            cursorPos_Right[playerno] = right;           // 左
                                                         //
            PlayerPrefs.SetInt("CURSORPOS_RIGHT_" + playerno.ToString(), cursorPos_Right[playerno]);
            PlayerPrefs.Save();

            screen_Width[playerno] = cursorPos_Right[playerno] - cursorPos_Left[playerno];
        }
    }
    public static void SaveCursorPos_Top(int playerno, int up)
    {
        if (playerno < Main.MAX_PLAYER)
        {
            cursorPos_Up[playerno] = up;   // 上
                                           //
            PlayerPrefs.SetInt("CURSORPOS_UP_" + playerno.ToString(), cursorPos_Up[playerno]);
            PlayerPrefs.Save();
        }
    }
    public static void SaveCursorPos_Button(int playerno, int down)
    {
        if (playerno < Main.MAX_PLAYER)
        {
            cursorPos_Down[playerno] = down;   // 上
                                               //
            PlayerPrefs.SetInt("CURSORPOS_DOWN_" + playerno.ToString(), cursorPos_Down[playerno]);
            PlayerPrefs.Save();

            screen_Height[playerno] = cursorPos_Up[playerno] - cursorPos_Down[playerno];
        }
    }

    //public static void SaveCursorPos_LeftTop(int playerno, int left, int up) {
    //    if (playerno < Main.MAX_PLAYER) {
    //        cursorPos_Left[playerno] = left;           // 左
    //        cursorPos_Up[playerno] = up;   // 上
    //                                                                 //
    //        PlayerPrefs.SetInt("CURSORPOS_LEFT_" + playerno.ToString(), cursorPos_Left[playerno]);
    //        PlayerPrefs.SetInt("CURSORPOS_UP_" + playerno.ToString(), cursorPos_Up[playerno]);
    //        PlayerPrefs.Save();
    //    }
    //}
    //public static void SaveCursorPos_RightButton(int playerno, int right, int down) {
    //    if (playerno < Main.MAX_PLAYER) {
    //        cursorPos_Right[playerno] = right;           // 左
    //        cursorPos_Down[playerno] = down;   // 上
    //                                       //
    //        PlayerPrefs.SetInt("CURSORPOS_RIGHT_" + playerno.ToString(), cursorPos_Right[playerno]);
    //        PlayerPrefs.SetInt("CURSORPOS_DOWN_" + playerno.ToString(), cursorPos_Down[playerno]);
    //        PlayerPrefs.Save();

    //        screen_Width[playerno] = cursorPos_Right[playerno] - cursorPos_Left[playerno];
    //        screen_Height[playerno] = cursorPos_Up[playerno] - cursorPos_Down[playerno];
    //    }
    //}
    //


    //   public static void SetAd_RightButton(int playerno)
    //{
    //	if (playerno < Main.MAX_PLAYER) {
    //		adMinValue[playerno * 2] = adValue[playerno * 2];			// 左
    //		adMaxValue[playerno * 2 + 1] = adValue[playerno * 2 + 1];	// 上
    //		//
    //		SaveAdMinValue(playerno * 2);
    //		SaveAdMaxValue (playerno * 2 + 1);
    //	}
    //}
    //public static void SetAd_LeftTop(int playerno)
    //{
    //	if (playerno < Main.MAX_PLAYER) {
    //		adMaxValue[playerno * 2] = adValue[playerno * 2];			// 右
    //		adMinValue[playerno * 2 + 1] = adValue[playerno * 2 + 1];	// 下
    //		//
    //		SaveAdMaxValue(playerno * 2);
    //		SaveAdMinValue (playerno * 2 + 1);
    //	}
    //}
    public static void UpdateAdVlaue(int no, int value)
    {
        if (no < adValue.Length)
        {
            //adBuf[no, adBufIn[no]] = value;
            //adBufIn[no]++;
            //if (adBufIn[no] >= 10) {
            //    adBufIn[no] = 0;
            //}
            //int zval = 0;
            //for (int i = 0; i < 10; i++) {
            //    zval += adBuf[no, i];
            //}
            //adValue[no] = zval / 10;
            adValue[no] = value;
        }
    }
    public static void KEY_Update(ulong l1)
    {
        Key_Down |= (Key_Old ^ l1) & Key_Old;
        Key_Old = l1;
    }

    public static bool KEY_State(int no)
    {
        if ((Key_Old & (ulong)(1 << no)) == 0)
        {
            return true;
        }
        return false;
    }

    public static bool KEY_Pressed(int no)
    {
        if ((Key_Down & (ulong)(1 << no)) != 0)
        {
            Key_Down &= ~(ulong)(1 << no);
            return true;
        }
        return false;
    }


    // key_status: ----------------------------------------------------------------------------------------
    public static bool KEYFJ_Statue_Ok(int player)
    {
        if (player == 0)
            return KEY_State(KEY_PLAYER1_OK);
        return KEY_State(KEY_PLAYER2_OK);
    }
    public static bool KEYFJ_Statue_Add(int player)
    {
        if (player == 0)
            return KEY_State(KEY_PLAYER1_ADD);
        return KEY_State(KEY_PLAYER2_ADD);
    }
    public static bool KEYFJ_Statue_Reset(int player)
    {
        if (player == 0)
            return KEY_State(KEY_PLAYER1_RESET);
        return false;// KEY_State (KEY_PLAYER2_RESET);
    }

    public static bool KEYFJ_Statue_Back()
    {
        //return KEY_State (KEY_BACK);
        return false;
    }

    //public static bool KEYFJ_Statue_Out(int player)
    //{
    //	if(player == 0)
    //		return KEY_State (KEY_PLAYER1_OUT);
    //	return KEY_State (KEY_PLAYER2_OUT);
    //}
    public static bool KEYFJ_Statue_Cin(int player)
    {
        if (player == 0)
            return KEY_State(KEY_PLAYER1_CIN);
#if IO_GUN1
        return false;
#else
        return KEY_State(KEY_PLAYER2_CIN);
#endif
    }
    //public static bool KEYFJ_Statue_Cout()
    //{
    //	return KEY_State (KEY_PLAYER_COUT);
    //}

    // key_pressed: ----------------------------------------------------------------------------------------
    public static bool KEYFJ_OkPressed(int player)
    {
        if (player == 0)
            return KEY_Pressed(KEY_PLAYER1_OK);
        return KEY_Pressed(KEY_PLAYER2_OK);
    }
    public static bool KEYFJ_AddPressed(int player)
    {
        if (player == 0)
            return KEY_Pressed(KEY_PLAYER1_ADD);
        return KEY_Pressed(KEY_PLAYER2_ADD);
    }
    public static bool KEYFJ_ResetPressed(int player)
    {
        if (player == 0)
            return KEY_Pressed(KEY_PLAYER1_RESET);
#if IO_GUN1
        return false;
#else
        return KEY_Pressed (KEY_PLAYER2_RESET);
#endif
    }
    public static bool KEYFJ_BackPressed()
    {
#if IO_GUN1
        return false;
#else
        return KEY_Pressed (KEY_BACK);
#endif
    }

    //public static bool KEYFJ_OutPressed(int player)
    //{
    //	if(player == 0)
    //		return KEY_Pressed (KEY_PLAYER1_OUT);
    //	return KEY_Pressed (KEY_PLAYER2_OUT);
    //}
    public static bool KEYFJ_CinPressed(int player)
    {
        if (player == 0)
            return KEY_Pressed(KEY_PLAYER1_CIN);
#if IO_GUN1
        return false;
#else
        return KEY_Pressed(KEY_PLAYER2_CIN);
#endif
    }
    public static bool KEYFJ_CoutPressed(int player)
    {
        if (player == 0)
            return KEY_Pressed(KEY_PLAYER1_COUT);
#if IO_GUN1
        return false;
#else
        return KEY_Pressed(KEY_PLAYER2_COUT);
#endif
    }


    //menu -------------------------------------------------------------------------------
    public static bool MENU_Statue_Left()
    {
        return KEY_State(KEY_MENU_LEFT);
    }
    public static bool MENU_Statue_Right()
    {
        return KEY_State(KEY_MENU_RIGHT);
    }
    public static bool MENU_Statue_Ok()
    {
        return KEY_State(KEY_MENU_OK);
    }

    public static bool KEYFJ_Menu_Statue_Left()
    {
#if IO_GUN1
        return KEY_State(KEY_MENU_LEFT);
#else
        return KEY_State(KEYFJ_MENU_LEFT);
#endif
    }
    public static bool KEYFJ_Menu_Statue_Right()
    {
#if IO_GUN1
        return KEY_State(KEY_MENU_RIGHT);
#else
        return KEY_State(KEYFJ_MENU_RIGHT);
#endif
    }
    public static bool KEYFJ_Menu_Statue_Ok()
    {
#if IO_GUN1
        return KEY_State(KEY_MENU_OK);
#else
        return KEY_State(KEYFJ_MENU_OK);
#endif
    }

    //menu
    public static bool MENU_LeftPressed()
    {
        return KEY_Pressed(KEY_MENU_LEFT);
    }
    public static bool MENU_RightPressed()
    {
        return KEY_Pressed(KEY_MENU_RIGHT);
    }
    public static bool MENU_OkPressed()
    {
        return KEY_Pressed(KEY_MENU_OK);
    }
    public static bool KEYFJ_Menu_LeftPressed()
    {
#if IO_GUN1
        return false;
#else
        return KEY_Pressed(KEYFJ_MENU_LEFT);
#endif
    }
    public static bool KEYFJ_Menu_RightPressed()
    {
#if IO_GUN1
        return false;
#else
        return KEY_Pressed(KEYFJ_MENU_RIGHT);
#endif
    }
    public static bool KEYFJ_Menu_OkPressed()
    {
#if IO_GUN1
        return false;
#else
        return KEY_Pressed(KEYFJ_MENU_OK);
#endif
    }


    // ADC retrun[0~1]
    public static float GetAd_UpDown(int playerno)
    {
#if GUN_HW
        return (float)adValue[playerno * 2] / Screen.height;
#else
        if (playerno >= Main.MAX_PLAYER)
            return 0;
        if (adcMinYValue[playerno] == adcMaxYValue[playerno])
            return 0;
        return ((float)adValue[playerno * 2] - adcMinYValue[playerno]) / ((float)adcMaxYValue[playerno] - adcMinYValue[playerno]);
#endif
    }
    //
    public static float GetAd_LeftRight(int playerno)
    {
#if GUN_HW
        return (float)adValue[playerno * 2 + 1] / Screen.width;
#else
        if (playerno >= Main.MAX_PLAYER)
            return 0;
        if (adcMinXValue[playerno] == adcMaxXValue[playerno])
            return 0;
        if (adcMinXValue[playerno] == adcMidXValue[playerno])
            return 0;
        if (adcMidXValue[playerno] == adcMaxXValue[playerno])
            return 0;
        if ((adcMaxXValue[playerno] - adcMinXValue[playerno]) * (adValue[playerno * 2 + 1] - adcMidXValue[playerno]) < 0)
        {
            return ((float)adValue[playerno * 2 + 1] - adcMinXValue[playerno]) / ((float)adcMidXValue[playerno] - adcMinXValue[playerno]) / 2f;
        }
        return ((float)adValue[playerno * 2 + 1] - adcMidXValue[playerno]) / ((float)adcMaxXValue[playerno] - adcMidXValue[playerno]) / 2f + 0.5f;
#endif
    }
    //   public static float GetAd_UpDown(int playerno)
    //{
    //	if(playerno < 2 && adMaxValue[playerno * 2] != adMinValue[playerno * 2]){
    //		return ((float)adValue [playerno * 2] - adMinValue[playerno * 2])/((float)adMaxValue[playerno * 2] - adMinValue[playerno * 2]) ;
    //	}
    //	return 0;
    //}
    ////
    //public static float GetAd_LeftRight(int playerno)
    //{
    //	if(playerno < 2 && adMaxValue[playerno * 2 + 1] != adMinValue[playerno * 2 + 1]){
    //		return ((float)adValue [playerno * 2 + 1] - adMinValue[playerno * 2 + 1])/((float)adMaxValue[playerno * 2 + 1] - adMinValue[playerno * 2 + 1]) ;
    //	}
    //	return 0;
    //}


    /*
	public const int KEY_PLAYER_UP 		= (int)KeyNum.KEY_K1;
	public const int KEY_PLAYER_DOWN 	= (int)KeyNum.KEY_K2;
	public const int KEY_PLAYER_LEFT 	= (int)KeyNum.KEY_K3;
	public const int KEY_PLAYER_RIGHT 	= (int)KeyNum.KEY_K4;
	public const int KEY_PLAYER_ADD 	= (int)KeyNum.KEY_K5;
	public const int KEY_PLAYER_OK 		= (int)KeyNum.KEY_K6;
	public const int KEY_PLAYER_OUT 	= (int)KeyNum.KEY_K7;
	public const int KEY_PLAYER_RESET 	= (int)KeyNum.KEY_K8;
	public const int KEY_PLAYER_UPCENT 	= (int)KeyNum.KEY_K9;
	public const int KEY_PLAYER_DOWNCENT= (int)KeyNum.KEY_K10;
	public const int KEY_PLAYER_CIN 	= (int)KeyNum.KEY_CADD1;
	public const int KEY_PLAYER_COUT 	= (int)KeyNum.KEY_CDEC;
	*/
    static ulong keyTest;
#if IO_LOCAL
    public static void CheckByInput()
    {
        keyTest = Key_Old;
#if KEY_TEST || UNITY_EDITOR
        // 方向盘-确定
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    keyTest &= ~(ulong)(1 << (int)KeyNum.KEY_K7);
        //}
        //else
        //{
        //    keyTest |= (ulong)(1 << (int)KeyNum.KEY_K7);
        //}
        if (Input.GetMouseButton(0))
        {
            //	if (Input.GetKey (KeyCode.Space)) {
            keyTest &= ~(ulong)(1 << KEY_PLAYER1_OK);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_PLAYER1_OK);
        }
        // 投币

        if (Input.GetKey(KeyCode.F1))
        {
            keyTest &= ~(ulong)(1 << KEY_PLAYER1_CIN);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_PLAYER1_CIN);
        }
#else
        // 方向盘-确定
        if (Input.GetKey(KeyCode.JoystickButton0)) {
            keyTest &= ~(ulong)(1 << (int)KeyNum.KEY_K1);
        } else {
            keyTest |= (ulong)(1 << (int)KeyNum.KEY_K1);
        }
        // 投币
        if (Input.GetKey(KeyCode.Menu)) {
            keyTest &= ~(ulong)(1 << KEY_PLAYER1_CIN);
        } else {
            keyTest |= (ulong)(1 << KEY_PLAYER1_CIN);
        }
#endif
        // 方向盘-左
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            keyTest &= ~(ulong)(1 << (int)KeyNum.KEY_K2);
        }
        else
        {
            keyTest |= (ulong)(1 << (int)KeyNum.KEY_K2);
        }
        // 方向盘-右
        if (Input.GetKey(KeyCode.RightArrow))
        {
            keyTest &= ~(ulong)(1 << (int)KeyNum.KEY_K3);
        }
        else
        {
            keyTest |= (ulong)(1 << (int)KeyNum.KEY_K3);
        }

        // 设置
        if (Input.GetKey(KeyCode.T))
        {
            keyTest &= ~(ulong)(1 << KEY_MENU_OK);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_MENU_OK);
        }
        // 设置-上
        if (Input.GetKey(KeyCode.UpArrow))
        {
            keyTest &= ~(ulong)(1 << KEY_MENU_LEFT);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_MENU_LEFT);
        }
        // 设置-下
        if (Input.GetKey(KeyCode.DownArrow))
        {
            keyTest &= ~(ulong)(1 << KEY_MENU_RIGHT);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_MENU_RIGHT);
        }
        KEY_Update(keyTest);

#if UNITY_EDITOR
        UpdateAdVlaue(0, (int)Input.mousePosition.y);
        UpdateAdVlaue(1, (int)Input.mousePosition.x);
#endif
    }
#endif

    static float key4Time = 0;
    static bool key4Pressed = false;
    public static void Key_ForKeybord()
    {
        keyTest = Key_Old;
        //        
        if (Input.GetKey(KeyCode.Alpha5) || Input.GetKey(KeyCode.Alpha4))
        {
            keyTest &= ~(ulong)(1 << KEY_PLAYER1_OK);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_PLAYER1_OK);
        }
        //        
        if (Input.GetKey(KeyCode.Alpha6))
        {
            keyTest &= ~(ulong)(1 << KEY_PLAYER1_ADD);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_PLAYER1_ADD);
        }
        //        
        //if (Input.GetKey (KeyCode.Alpha6)) {
        //    keyTest &= ~(ulong)(1 << (int)KeyNum.KEY_K3);
        //} else {
        //    keyTest |= (ulong)(1 << (int)KeyNum.KEY_K3);
        //}
        KEY_Update(keyTest);

        if (Input.GetKey(KeyCode.Alpha4))
        {
            key4Time += Time.deltaTime;
            if (key4Time >= 0.5f)
            {
                key4Time = 0.4f;
                key4Pressed = true;
            }
            if (key4Pressed)
            {
                key4Pressed = false;
                Key_Down |= (1 << KEY_PLAYER1_OK);
            }
        }
        else
        {
            key4Pressed = true;
            key4Time = 0;
        }

        // ADC        
        UpdateAdVlaue(0, (int)Input.mousePosition.y);
        UpdateAdVlaue(1, (int)Input.mousePosition.x);
    }
    // 测试(用键盘)
    public static void KeyTest_ForKeybord()
    {
        // key
        keyTest = Key_Old;
        //
#if !GUN_HW || UNITY_EDITOR
        //
        if (Input.GetKey(KeyCode.J))
        {
            keyTest &= ~(ulong)(1 << KEY_PLAYER1_ADD);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_PLAYER1_ADD);
        }
        // 
        if (Input.GetMouseButton(0))
        {
            //	if (Input.GetKey (KeyCode.Space)) {
            keyTest &= ~(ulong)(1 << KEY_PLAYER1_OK);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_PLAYER1_OK);
        }
#endif
        //
        //if (Input.GetKey (KeyCode.O)) {
        //	keyTest &= ~(ulong)(1 << KEY_PLAYER1_OUT);
        //} else {
        //	keyTest |= (ulong)(1 << KEY_PLAYER1_OUT);
        //}
        //
        if (Input.GetKey(KeyCode.F1))
        {
            keyTest &= ~(ulong)(1 << KEY_PLAYER1_CIN);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_PLAYER1_CIN);
        }
        //
        if (Input.GetKey(KeyCode.F2))
        {
            keyTest &= ~(ulong)(1 << KEY_PLAYER1_COUT);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_PLAYER1_COUT);
        }
        //
        if (Input.GetKey(KeyCode.F7))
        {
            keyTest &= ~(ulong)(1 << KEY_PLAYER1_RESET);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_PLAYER1_RESET);
        }
        //
#if !IO_GUN1
        if (Input.GetKey(KeyCode.F9))
        {
            keyTest &= ~(ulong)(1 << KEY_BACK);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_BACK);
        }

        if (Input.GetMouseButton(1))
        {
            //	if (Input.GetKey (KeyCode.Space)) {
            keyTest &= ~(ulong)(1 << KEY_PLAYER2_OK);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_PLAYER2_OK);
        }
        if (Input.GetKey(KeyCode.F5))
        {
            keyTest &= ~(ulong)(1 << KEY_PLAYER2_CIN);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_PLAYER2_CIN);
        }
        //
        if (Input.GetKey(KeyCode.F6))
        {
            keyTest &= ~(ulong)(1 << KEY_PLAYER2_COUT);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_PLAYER2_COUT);
        }
        if (Input.GetKey(KeyCode.F5))
        {
            keyTest &= ~(ulong)(1 << KEY_PLAYER2_CIN);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_PLAYER2_CIN);
        }
        //
        if (Input.GetKey(KeyCode.F6))
        {
            keyTest &= ~(ulong)(1 << KEY_PLAYER2_COUT);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_PLAYER2_COUT);
        }
#endif
        /*
            public const int KEY_MENU_UP 		= (int)KeyNum.KEY_K2;		// 上
            public const int KEY_MENU_DOWN 		= (int)KeyNum.KEY_K8;		// 下
            public const int KEY_MENU_LEFT 		= (int)KeyNum.KEY_K4;		// 左
            public const int KEY_MENU_RIGHT 	= (int)KeyNum.KEY_K5;		// 右
            public const int KEY_MENU_OK 		= (int)KeyNum.KEY_K7;		// 确认
            public const int KEY_MENU_CANCEL	= (int)KeyNum.KEY_K3;		// 取消
            public const int KEY_MENU_SMALLGAME	= (int)KeyNum.KEY_K6;		// 小游戏 

                    */
        //
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            keyTest &= ~(ulong)(1 << KEY_MENU_LEFT);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_MENU_LEFT);
        }
        //
        if (Input.GetKey(KeyCode.RightArrow))
        {
            keyTest &= ~(ulong)(1 << KEY_MENU_RIGHT);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_MENU_RIGHT);
        }
        //
        if (Input.GetKey(KeyCode.KeypadEnter))
        {
            keyTest &= ~(ulong)(1 << KEY_MENU_OK);
        }
        else
        {
            keyTest |= (ulong)(1 << KEY_MENU_OK);
        }
        //
        KEY_Update(keyTest);


#if !GUN_HW
        // ADC        
        UpdateAdVlaue(0, (int)Input.mousePosition.y);
        UpdateAdVlaue(1, (int)Input.mousePosition.x);
        UpdateAdVlaue(2, (int)Input.mousePosition.y);
        UpdateAdVlaue(3, (int)Input.mousePosition.x);
#endif
    }
    private static bool[] isLeft = new bool[Main.MAX_PLAYER];
    private static bool[] isRight = new bool[Main.MAX_PLAYER];

    /// <summary>
    /// 获取方向盘Adc返回值.
    /// </summary>
    /// <returns></returns>
    public static float GetDirADCReturn(int Id)
    {
        float maxMin = (adcMaxXValue[Id] - adcMinXValue[Id]) * 1.0f;
        float cur = (adValue[Id * 2 + 1] - adcMinXValue[Id]) / maxMin;
        return cur;
    }
    /// <summary>
    /// 获取左右状态.
    /// </summary>
    /// <param name="Id">玩家id</param>
    public static int GetKeySta(int Id)
    {
        if (GetDirADCReturn(Id) <= 0.3f && isLeft[Id] == false)
        {
            isLeft[Id] = true;
            isRight[Id] = false;
            return (int)en_KeySta.Left;
        }
        if (GetDirADCReturn(Id) >= 0.8f && isRight[Id] == false)
        {
            isRight[Id] = true;
            isLeft[Id] = false;
            return (int)en_KeySta.Right;
        }

        if (GetDirADCReturn(Id) > 0.3f && GetDirADCReturn(Id) < 0.8f)
        {
            isLeft[Id] = false;
            isRight[Id] = false;
        }
        return -1;
    }
}



// 测试


