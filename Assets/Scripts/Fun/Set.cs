using UnityEngine;

public enum en_Language
{
    Chinese = 0,		// 中文
    English,			// 英文
}

public enum en_GameMode
{
    Yule = 0,           // 娱乐模式
    Zhongxing,          // 中性模式
}
public enum en_GunMode
{
    Gun = 0,            // 枪
    ShootBead,          // 射珠
    ShootWater,         // 射水
}
public enum en_PlayerMode
{
    One = 0,            // 单人
    Two,                // 双人
}
public enum en_InOutMode
{
    TwoInTwoOut = 0,    // 双投双退
    TwoInOneOut,        // 双投单退
    OneInOneOut,        // 单投单退
}
public enum en_OutMode
{
    OutNone = 0,		// 不退
    OutTicket,			// 退彩票模式
    OutGift,            // 退扭蛋
    //	OutCoin,			// 退币模式
}

public struct GameSet
{
    public int Language;		// 语言: 0:中文; 1:英文
    public int GameMode;		// 游戏模式: 0: 娱乐 1:中性模式
    public int PlayerMode;      // 玩家数量: 0: 单人 1: 双人
    public int GunMode;         // 枪模式：0: 枪  1:射珠   2:射水
    public int InOutMode;       // 投退模式：0: 双投双退； 1：双投单退； 2：单投单退
    public int AttackPower;
    //娱乐模式
    public int OutMode;			// 奖励模式: 不退,退票,退币    
    public int TicketBl;        // 彩票比率
    public int GiftBl;          // 扭蛋比率
    public int StartCoins;		// 开始分数: 几分玩一次
    public int GameTime;		// 游戏时间
    public int OutDcTime;       // 退检测时间
    public int MonsterNum_1;    // 怪物数量(第一关)
    public int MonsterNum_2;    // 怪物数量(第二关)
    public int MonsterNum_3;    // 怪物数量(第三关)
    public int ScoreTtl;        // 是否累积
    // 中性模式
    public int TicketsOneCoin;  // 1币兑票数
    public int EditerOneCoin;   // 1币分数(子弹数)
    public int MinTickets;      // 最小退票(安慰奖)
    // 其它没用到的
    //public int UpCentBl;		// 上下分比率
    //public int MinOut;			// 最小退币
    //public int MaxOut;			// 最大退币
    //public int OverflowCent;	// 爆机分数
    //public int Odds;			// 难度
    //public int Wave;			// 波动
    public int DeskMusic;       // 背景音乐
    public int MainSoundVolume; // 主机音量
    public int ShowQrCode;      // 显示二维码
    public int SysVolume;       // 系统音量(直接左右设置按键调整)
    public int Password;
    public int MP5GunShake;     //mp5枪震动开关
    public int ChildrenSong;    //海燕一体机儿歌开关

    //枪震动
    public int Gun1ShakePower;      // (枪)震动力度
    public int Gun1ShakeTime;      // (枪)震动时间
    public int Gun2ShakePower;      // (枪)震动力度
    public int Gun2ShakeTime;      // (枪)震动时间
    public int Gun3ShakePower;      // (枪)震动力度
    public int Gun3ShakeTime;      // (枪)震动时间
    public int Gun4ShakePower;      // (枪)震动力度
    public int Gun4ShakeTime;      // (枪)震动时间
}

public class Set
{
    //----------------------------
    const string SET_Language = "SETLanguage";
    const string SET_GameMode = "SETGameMode";
    const string SET_InOutMode = "SETInOutMode";
    const string SET_AttackPower = "SETAttackPower";
    const string SET_GunMode = "SETGunMode";
    const string SET_PlayerMode = "SETPlayerMode";
    const string SET_StartCoins = "SETStartCoins";
    const string SET_GameTime = "SETGameTime";
    const string SET_OutDcTime = "SETOutDcTime";
    const string SET_MonsterNum_1 = "SETMonsterNum_1";
    const string SET_MonsterNum_2 = "SETMonsterNum_2";
    const string SET_MonsterNum_3 = "SETMonsterNum_3";
    const string SET_ScoreTtl = "SETScoreTtl";
    const string SET_DeskMusic = "SETDeskMusic";
    const string SET_ShowQrCode = "SETShowQrCode";
    const string SET_MainSoundVolume = "SETMainSoundVolume";
    const string SET_IoSoundVolume = "SETIoSoundVolume";
    const string SET_SysVolume = "SETSysVolume";
    const string SET_Password = "SETPassword";
    const string SET_MP5GunShake = "SETMP5GunShake";
    const string SET_ChildrenSong = "SETChildrenSong";

    const string SET_OutMode = "SETOutMode";         // 退模式: 不退,退票,退币
    const string SET_TicketBl = "SETTicketBl";        // 彩票比率
    const string SET_GiftBl = "SETGiftBl";		    // 彩票比率
    const string SET_UpCentBl = "SETUpCentBl";		// 上下分比率
    const string SET_MinOut = "SETMinOut";			// 最小退币
    const string SET_MaxOut = "SETMaxOut";			// 最大退币
    const string SET_OverflowCent = "SETOverflowCent";	// 爆机分数
    const string SET_Odds = "SETOdds";			// 难度
    const string SET_Wave = "SETWave";			// 波动
    // 中性
    const string SET_TicketsOneCoin = "SETTicketsOneCoin";  // 1币兑票数
    const string SET_EditerOneCoin = "SETEditerOneCoin";   // 1币分数(子弹数)
    const string SET_MinTickets = "SETMinTickets";      // 最小退票(安慰奖)
    const string SET_GameSelect = "SETGameSelect";  //

    //枪震动
    const string SET_Gun1ShakePower = "SETGun1ShakePower";
    const string SET_Gun1ShakeTime = "SETGun1ShakeTime";
    const string SET_Gun2ShakePower = "SETGun2ShakePower";
    const string SET_Gun2ShakeTime = "SETGun2ShakeTime";
    const string SET_Gun3ShakePower = "SETGun3ShakePower";
    const string SET_Gun3ShakeTime = "SETGun3ShakeTime";
    const string SET_Gun4ShakePower = "SETGun4ShakePower";
    const string SET_Gun4ShakeTime = "SETGun4ShakeTime";


    //---------------------------
    public static int[] SET_c_Language = { 0, 1 };
    public static int[] SET_c_GameMode = { 0, 1 };
    public static int[] SET_c_InOutMode = { 0, 1, 2 };
    public static int[] SET_c_AttackPower = { 0, 1, 2, 3, 4 };
    public static int[] SET_c_ShakePower = { 5, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 35, 40 };
    public static int[] SET_c_ShakeTime = { 50, 60, 70, 80, 90, 100 };
    public static int[] SET_c_CoinBl = { 0, 1, 2, 3, 4, 5, 10, 15, 20, 30, 40, 50, 60, 70, 80, 90, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };
    public static int[] SET_c_StartCoins = { 0, 1, 2, 3, 4, 5, 10, 15, 20, 30, 40, 50 };
    public static int[] SET_c_GameTime = { 20, 30, 60, 90, 120, 180, 240, 300, 360, 420, 480, 540, 600 };
    public static int[] SET_c_OutDcTime = { 0, 20, 30, 40, 50, 60, 70, 80, 90, 100, 120, 150, 180, 210, 250, 300, 350, 400, 450, 500, 600 };
    public static int[] SET_c_MonsterNum_1 = { 30, 40, 50, 60, 70, 80, 90, 100, 120, 150, 200, 300 };
    public static int[] SET_c_MonsterNum_2 = { 30, 40, 50, 60, 70, 80, 90, 100, 120, 150, 200, 300 };
    public static int[] SET_c_MonsterNum_3 = { 30, 40, 50, 60, 70, 80, 90, 100, 120, 150, 200, 300 };

    public static int[] SET_c_ScoreTtl = { 0, 1 };
    public static int[] SET_c_DeskMusic = { 0, 1 };
    public static int[] SET_c_ShowQrCode = { 0, 1 };
    public static int[] SET_c_MainSoundVolume = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    public static int[] SET_c_OutMode = { 0, 1, 2 };            // 退模式: 不退,退票,退扭蛋
    public static int[] SET_c_GunMode = { 0, 1, 2 };            //枪  射珠 射水
    public static int[] SET_c_PlayerMode = { 0, 1 };
    public static int[] SET_c_TicketBl = { 0, 1, 2, 3, 4, 5, 10, 15, 20, 30, 40, 50, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };		// 彩票比率
    public static int[] SET_c_GiftBl = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
    public static int[] SET_c_UpCentBl = { 0, 10, 20, 30, 40, 50, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };     // 上下分比率
    public static int[] SET_c_MinOut = { 0, 10, 20, 30, 40, 50, 100, 200, 300, 400, 500 };         // 最小退币
    public static int[] SET_c_MaxOut = { 0, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };			// 最大退币
    public static int[] SET_c_OverflowCent = { 0, 5000, 7000, 10000, 20000, 30000, 40000, 50000, 100000, 200000, 300000, 400000, 500000, 1000000 }; // 爆机分数
    public static int[] SET_c_Odds = { 0, 1, 2, 3, 4, 5, 6, 7 };           // 难度
    public static int[] SET_c_Wave = { 0, 1, 2, 3, 4, 5, 6, 7 };            // 波动
    //
    public static int[] SET_c_TicketsOneCoin = { 10, 20, 30, 40, 50, 100, 200, 300, 400, 500 };
    public static int[] SET_c_EditerOneCoin = { 10, 20, 30, 40, 50, 100, 200, 300, 400, 500 };
    public static int[] SET_c_MinTickets = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    public static int[] SET_c_GameSelect = { 0, 1 };
    public static int[] SET_c_MP5GunShake = { 0, 1 };
    public static int[] SET_c_ChildrenSong = { 0, 1 };
    public static float MAX_SOUND_VOLUME = 10;		// 最大声音量

    public const int SET_c_MinSysVolume = 0;
    public const int SET_c_MaxSysVolume = 100;
    public const int MIN_PASSWORD = 0;
    public const int MAX_PASSWORD = 999999;
    public const int DEF_PASSWORD = 0;
    public const int SUP_PASSWORD = 461835;


    public static GameSet setVal;
    public static int[] GameSelect = new int[Main.tab_GameId.Length];

    static bool CheckLoad(int val, int[] tab)
    {
        for (int i = 0; i < tab.Length; i++)
        {
            if (val == tab[i])
            {
                return true;
            }
        }
        return false;
    }

    // fun
    public static void LoadAll()
    {
        int l1;

        // 第一次??
        if (PlayerPrefs.HasKey(SET_Language) == false)
        {
            DefaultLanguage();
            DefaultPassword();
            Default();
            DefaultGameSelect();
        }

        // 固定语言：
        l1 = PlayerPrefs.GetInt(SET_Language);
        if (!CheckLoad(l1, SET_c_Language))
        {
            l1 = SET_c_Language[0];
            PlayerPrefs.SetInt(SET_Language, l1);
        }
#if LANGUAGE_ALL
        setVal.Language = l1;
#else
        setVal.Language = (int)en_Language.English;
#endif
        //
        l1 = PlayerPrefs.GetInt(SET_GameMode);
        if (!CheckLoad(l1, SET_c_GameMode))
        {
            l1 = SET_c_GameMode[0];
            PlayerPrefs.SetInt(SET_GameMode, l1);
        }
        setVal.GameMode = (int)en_GameMode.Yule;// l1;
        //
        l1 = PlayerPrefs.GetInt(SET_InOutMode);
        if (!CheckLoad(l1, SET_c_InOutMode))
        {
            l1 = SET_c_InOutMode[0];
            PlayerPrefs.SetInt(SET_InOutMode, l1);
        }
        setVal.InOutMode = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_AttackPower);
        if (!CheckLoad(l1, SET_c_AttackPower))
        {
            l1 = SET_c_AttackPower[0];
            PlayerPrefs.SetInt(SET_AttackPower, l1);
        }
        setVal.AttackPower = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_Gun1ShakePower);
        if (!CheckLoad(l1, SET_c_ShakePower))
        {
            l1 = 20;
            PlayerPrefs.SetInt(SET_Gun1ShakePower, l1);
        }
        setVal.Gun1ShakePower = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_Gun1ShakeTime);
        if (!CheckLoad(l1, SET_c_ShakeTime))
        {
            l1 = 50;
            PlayerPrefs.SetInt(SET_Gun1ShakeTime, l1);
        }
        setVal.Gun1ShakeTime = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_Gun2ShakePower);
        if (!CheckLoad(l1, SET_c_ShakePower))
        {
            l1 = 20;
            PlayerPrefs.SetInt(SET_Gun2ShakePower, l1);
        }
        setVal.Gun2ShakePower = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_Gun2ShakeTime);
        if (!CheckLoad(l1, SET_c_ShakeTime))
        {
            l1 = 50;
            PlayerPrefs.SetInt(SET_Gun2ShakeTime, l1);
        }
        setVal.Gun2ShakeTime = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_Gun3ShakePower);
        if (!CheckLoad(l1, SET_c_ShakePower))
        {
            l1 = 20;
            PlayerPrefs.SetInt(SET_Gun3ShakePower, l1);
        }
        setVal.Gun3ShakePower = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_Gun3ShakeTime);
        if (!CheckLoad(l1, SET_c_ShakeTime))
        {
            l1 = 50;
            PlayerPrefs.SetInt(SET_Gun3ShakeTime, l1);
        }
        setVal.Gun3ShakeTime = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_Gun4ShakePower);
        if (!CheckLoad(l1, SET_c_ShakePower))
        {
            l1 = 20;
            PlayerPrefs.SetInt(SET_Gun4ShakePower, l1);
        }
        setVal.Gun4ShakePower = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_Gun4ShakeTime);
        if (!CheckLoad(l1, SET_c_ShakeTime))
        {
            l1 = 50;
            PlayerPrefs.SetInt(SET_Gun4ShakeTime, l1);
        }
        setVal.Gun4ShakeTime = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_OutMode);
        if (!CheckLoad(l1, SET_c_OutMode))
        {
            l1 = SET_c_OutMode[0];
            PlayerPrefs.SetInt(SET_OutMode, l1);
        }
        setVal.OutMode = l1;
        //

        //
        l1 = PlayerPrefs.GetInt(SET_GunMode);
        if (!CheckLoad(l1, SET_c_GunMode))
        {
            l1 = SET_c_GunMode[0];
            PlayerPrefs.SetInt(SET_GunMode, l1);
        }
#if GUN_HW
        setVal.GunMode = (int)en_GunMode.Gun;
#else
        setVal.GunMode = l1;
#endif
        //
        l1 = PlayerPrefs.GetInt(SET_PlayerMode);
        if (!CheckLoad(l1, SET_c_PlayerMode))
        {
            l1 = SET_c_PlayerMode[0];
            PlayerPrefs.SetInt(SET_PlayerMode, l1);
        }
        setVal.PlayerMode = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_TicketBl);
        if (!CheckLoad(l1, SET_c_TicketBl))
        {
            l1 = SET_c_TicketBl[0];
            PlayerPrefs.SetInt(SET_TicketBl, l1);
        }
        setVal.TicketBl = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_GiftBl);
        if (!CheckLoad(l1, SET_c_GiftBl))
        {
            l1 = SET_c_GiftBl[1];
            PlayerPrefs.SetInt(SET_GiftBl, l1);
        }
        setVal.GiftBl = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_StartCoins);
        if (!CheckLoad(l1, SET_c_StartCoins))
        {
            l1 = SET_c_StartCoins[0];
            PlayerPrefs.SetInt(SET_StartCoins, l1);
        }
        setVal.StartCoins = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_GameTime);
        if (!CheckLoad(l1, SET_c_GameTime))
        {
            l1 = SET_c_GameTime[4];
            PlayerPrefs.SetInt(SET_GameTime, l1);
        }
        setVal.GameTime = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_OutDcTime);
        if (!CheckLoad(l1, SET_c_OutDcTime))
        {
            l1 = SET_c_OutDcTime[1];
            PlayerPrefs.SetInt(SET_OutDcTime, l1);
        }
        setVal.OutDcTime = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_MonsterNum_1);
        if (!CheckLoad(l1, SET_c_MonsterNum_1))
        {
            l1 = SET_c_MonsterNum_1[0];
            PlayerPrefs.SetInt(SET_MonsterNum_1, l1);
        }
        setVal.MonsterNum_1 = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_MonsterNum_2);
        if (!CheckLoad(l1, SET_c_MonsterNum_2))
        {
            l1 = SET_c_MonsterNum_2[0];
            PlayerPrefs.SetInt(SET_MonsterNum_2, l1);
        }
        setVal.MonsterNum_2 = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_MonsterNum_3);
        if (!CheckLoad(l1, SET_c_MonsterNum_3))
        {
            l1 = SET_c_MonsterNum_3[0];
            PlayerPrefs.SetInt(SET_MonsterNum_3, l1);
        }
        setVal.MonsterNum_3 = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_ScoreTtl);
        if (!CheckLoad(l1, SET_c_ScoreTtl))
        {
            l1 = SET_c_ScoreTtl[0];
            PlayerPrefs.SetInt(SET_ScoreTtl, l1);
        }
        setVal.ScoreTtl = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_DeskMusic);
        if (!CheckLoad(l1, SET_c_DeskMusic))
        {
            l1 = SET_c_DeskMusic[0];
            PlayerPrefs.SetInt(SET_DeskMusic, l1);
        }
        setVal.DeskMusic = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_ShowQrCode);
        if (!CheckLoad(l1, SET_c_ShowQrCode))
        {
            l1 = SET_c_ShowQrCode[0];
            PlayerPrefs.SetInt(SET_ShowQrCode, l1);
        }
        setVal.ShowQrCode = l1;
        // 中性模式
        //
        l1 = PlayerPrefs.GetInt(SET_TicketsOneCoin);
        if (!CheckLoad(l1, SET_c_TicketsOneCoin))
        {
            l1 = SET_c_TicketsOneCoin[0];
            PlayerPrefs.SetInt(SET_TicketsOneCoin, l1);
        }
        setVal.TicketsOneCoin = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_EditerOneCoin);
        if (!CheckLoad(l1, SET_c_EditerOneCoin))
        {
            l1 = SET_c_EditerOneCoin[0];
            PlayerPrefs.SetInt(SET_EditerOneCoin, l1);
        }
        setVal.EditerOneCoin = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_MinTickets);
        if (!CheckLoad(l1, SET_c_MinTickets))
        {
            l1 = SET_c_MinTickets[0];
            PlayerPrefs.SetInt(SET_MinTickets, l1);
        }
        setVal.MinTickets = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_MainSoundVolume);
        if (!CheckLoad(l1, SET_c_MainSoundVolume))
        {
            l1 = SET_c_MainSoundVolume[50];
            PlayerPrefs.SetInt(SET_MainSoundVolume, l1);
        }
        setVal.MainSoundVolume = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_SysVolume, 50);
        if (l1 < SET_c_MinSysVolume || l1 > SET_c_MaxSysVolume)
        {
            l1 = 50;
            PlayerPrefs.SetInt(SET_SysVolume, l1);
        }
        setVal.SysVolume = l1;//l1   默认值  音量为50
        //
        l1 = PlayerPrefs.GetInt(SET_Password);
        if (l1 < MIN_PASSWORD || l1 > MAX_PASSWORD)
        {
            l1 = DEF_PASSWORD;
            PlayerPrefs.SetInt(SET_Password, l1);
        }
        setVal.Password = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_MP5GunShake);
        if (!CheckLoad(l1, SET_c_MP5GunShake))
        {
            l1 = SET_c_MP5GunShake[1];
            PlayerPrefs.SetInt(SET_MP5GunShake, l1);
        }
        setVal.MP5GunShake = l1;
        //
        l1 = PlayerPrefs.GetInt(SET_ChildrenSong);
        if (!CheckLoad(l1, SET_c_ChildrenSong))
        {
            l1 = SET_c_ChildrenSong[1];
            PlayerPrefs.SetInt(SET_ChildrenSong, l1);
        }
        setVal.ChildrenSong = l1;
        //
        for (int i = 0; i < GameSelect.Length; i++)
        {
            l1 = PlayerPrefs.GetInt(SET_GameSelect + i);
            if (!CheckLoad(l1, SET_c_GameSelect))
            {
                l1 = 1;
                PlayerPrefs.SetInt(SET_GameSelect + i, l1);
            }
            GameSelect[i] = l1;
        }
        
    }
    public static void SaveOddsAndWave()
    {
        //PlayerPrefs.SetInt (SET_Odds, setVal.Odds);
        //PlayerPrefs.SetInt (SET_Wave, setVal.Wave);
        //PlayerPrefs.Save ();
    }

    public static void SaveAll()
    {
        PlayerPrefs.SetInt(SET_GameMode, setVal.GameMode);
        PlayerPrefs.SetInt(SET_InOutMode, setVal.InOutMode);
        PlayerPrefs.SetInt(SET_AttackPower, setVal.AttackPower);
        PlayerPrefs.SetInt(SET_Gun1ShakePower, setVal.Gun1ShakePower);
        PlayerPrefs.SetInt(SET_Gun1ShakeTime, setVal.Gun1ShakeTime);
        PlayerPrefs.SetInt(SET_Gun2ShakePower, setVal.Gun2ShakePower);
        PlayerPrefs.SetInt(SET_Gun2ShakeTime, setVal.Gun2ShakeTime);
        PlayerPrefs.SetInt(SET_Gun3ShakePower, setVal.Gun3ShakePower);
        PlayerPrefs.SetInt(SET_Gun3ShakeTime, setVal.Gun3ShakeTime);
        PlayerPrefs.SetInt(SET_Gun4ShakePower, setVal.Gun4ShakePower);
        PlayerPrefs.SetInt(SET_Gun4ShakeTime, setVal.Gun4ShakeTime);
        PlayerPrefs.SetInt(SET_GunMode, setVal.GunMode);
        PlayerPrefs.SetInt(SET_PlayerMode, setVal.PlayerMode);
        PlayerPrefs.SetInt(SET_OutMode, setVal.OutMode);
        PlayerPrefs.SetInt(SET_StartCoins, setVal.StartCoins);
        PlayerPrefs.SetInt(SET_TicketBl, setVal.TicketBl);
        PlayerPrefs.SetInt(SET_GiftBl, setVal.GiftBl);
        PlayerPrefs.SetInt(SET_GameTime, setVal.GameTime);
        PlayerPrefs.SetInt(SET_OutDcTime, setVal.OutDcTime);
        PlayerPrefs.SetInt(SET_MonsterNum_1, setVal.MonsterNum_1);
        PlayerPrefs.SetInt(SET_MonsterNum_2, setVal.MonsterNum_2);
        PlayerPrefs.SetInt(SET_MonsterNum_3, setVal.MonsterNum_3);
        PlayerPrefs.SetInt(SET_ScoreTtl, setVal.ScoreTtl);
        PlayerPrefs.SetInt(SET_DeskMusic, setVal.DeskMusic);
        PlayerPrefs.SetInt(SET_ShowQrCode, setVal.ShowQrCode);
        PlayerPrefs.SetInt(SET_MP5GunShake, setVal.MP5GunShake);
        PlayerPrefs.SetInt(SET_ChildrenSong, setVal.ChildrenSong);
        PlayerPrefs.SetInt(SET_TicketsOneCoin, setVal.TicketsOneCoin);
        PlayerPrefs.SetInt(SET_EditerOneCoin, setVal.EditerOneCoin);
        PlayerPrefs.SetInt(SET_MinTickets, setVal.MinTickets);
        PlayerPrefs.SetInt(SET_MainSoundVolume, setVal.MainSoundVolume);

        //PlayerPrefs.SetInt(SET_UpCentBl, setVal.UpCentBl);
        //PlayerPrefs.SetInt(SET_MinOut, setVal.MinOut);
        //PlayerPrefs.SetInt(SET_MaxOut, setVal.MaxOut);
        //PlayerPrefs.SetInt(SET_OverflowCent, setVal.OverflowCent);
        //PlayerPrefs.SetInt(SET_Odds, setVal.Odds);
        //PlayerPrefs.SetInt(SET_Wave, setVal.Wave);

        PlayerPrefs.Save();
    }

    public static void SaveLanguage()
    {
        PlayerPrefs.SetInt(SET_Language, setVal.Language);
        PlayerPrefs.Save();
    }
    public static void SavePassword()
    {
        PlayerPrefs.SetInt(SET_Password, setVal.Password);
        PlayerPrefs.Save();
    }
    public static void SaveSysVolume()
    {

        PlayerPrefs.SetInt(SET_SysVolume, setVal.SysVolume);
        PlayerPrefs.Save();
    }
    public static void SaveGameSelect()
    {
        for (int i = 0; i < GameSelect.Length; i++)
        {
            PlayerPrefs.SetInt(SET_GameSelect + i, GameSelect[i]);
        }
        PlayerPrefs.Save();
    }

    public static void Default()
    {
        setVal.GameMode = (int)en_GameMode.Yule;		    // 游戏模式: 0: 娱乐 1:博彩模式
        setVal.InOutMode = (int)en_InOutMode.OneInOneOut;
        setVal.AttackPower = 2;
        setVal.Gun1ShakePower = 20;
        setVal.Gun1ShakeTime = 50;
        setVal.Gun2ShakePower = 20;
        setVal.Gun2ShakeTime = 50;
        setVal.Gun3ShakePower = 20;
        setVal.Gun3ShakeTime = 50;
        setVal.Gun4ShakePower = 20;
        setVal.Gun4ShakeTime = 50;
        setVal.GunMode = (int)en_GunMode.Gun;//默认枪
        setVal.PlayerMode = (int)en_PlayerMode.Two;
        setVal.OutMode = (int)en_OutMode.OutNone;			// 奖励模式: 退彩票
        setVal.StartCoins = 1;			// 游戏比例: 几分玩一次
        setVal.TicketBl = 10;			// 彩票比率
        setVal.GiftBl = 1;            //
        setVal.GameTime = 180;          // 游戏时间(自动出兵时间)
        setVal.OutDcTime = 20;
        setVal.MonsterNum_1 = 40;
        setVal.MonsterNum_2 = 50;
        setVal.MonsterNum_3 = 40;
        setVal.ScoreTtl = 0;
        setVal.DeskMusic = 0;   
        setVal.ShowQrCode = 0;   
        setVal.TicketsOneCoin = 20;				
        setVal.EditerOneCoin = 40;				
        setVal.MinTickets = 0;
        setVal.MP5GunShake = 1;
        setVal.ChildrenSong = 1;
        setVal.MainSoundVolume = 8;		// 主机音量

        //setVal.MinOut = 1;			// 最小退币
        //setVal.MaxOut = 500;			// 最大退币
        //setVal.OverflowCent = 50000	// 爆机分数
        //setVal.Odds = 1;				// 难度
        //setVal.Wave = 0;				// 波动

        SaveAll();
    }

    public static void DefaultLanguage()
    {
        setVal.Language = (int)en_Language.Chinese;	//中文
        SaveLanguage();
    }
    public static void DefaultPassword()
    {
        setVal.Password = DEF_PASSWORD;
        SavePassword();
    }
    public static void DefaultGameSelect()
    {
        for (int i = 0; i < GameSelect.Length; i++)//i= 5
        {
            GameSelect[i] = 1;
        }
        SaveGameSelect();
    }
    public static void ReciveBuf(byte[] buf, int offset)
    {
        setVal.StartCoins = buf[offset + 0];
        setVal.GameTime = buf[offset + 1];
        setVal.DeskMusic = buf[offset + 2];
        setVal.SysVolume = buf[offset + 3];
        SaveAll();
    }
    public static int WriteBuf(byte[] buf, int offset)
    {
        int index = offset;
        buf[index++] = (byte)setVal.StartCoins;
        buf[index++] = (byte)setVal.GameTime;
        buf[index++] = (byte)setVal.DeskMusic;
        buf[index++] = (byte)setVal.SysVolume;
        return index - offset;
    }
    
    public static void Test()
    {
        setVal.GameMode = (int)en_GameMode.Yule;        // 游戏模式: 0: 娱乐1-倒计时, 1:娱乐2-得分退礼品; 2:博彩模式
        setVal.InOutMode = (int)en_InOutMode.TwoInOneOut;

        setVal.OutMode = (int)en_OutMode.OutTicket;			// 奖励模式: 退
        setVal.StartCoins = 1;			// 游戏比例: 几分玩一次
        setVal.TicketBl = 10;			// 彩票比率
        setVal.GiftBl = 100;
        setVal.GameTime = 30;			// 游戏时间(自动出兵时间)
        setVal.ScoreTtl = 0;            

        setVal.TicketsOneCoin = 0;
        setVal.EditerOneCoin = 0;
        setVal.MinTickets = 0;

        setVal.MainSoundVolume = 8;		// 主机音量
    }
}
