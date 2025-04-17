using UnityEngine;

public class JL //奖励系统
{
    //
    const int SERO_ID_P1 = 1;
    const int SERO_ID_P2 = 2;
    const int MAX_SERO = 3;


    // 小金库
    public static int ttl;
    public static int[] sero = new int[MAX_SERO];


#if SAVE_FILE
    const string ADDR_Ttl = "ADDR_Ttl";
    const string ADDR_Sero = "ADDR_Sero";

    //
    public static void LoadStart()
    {

    }
    // Use this for initialization
    public static bool Load()
    {
        int l1;
        //
        l1 = SaveData.GetInt(ADDR_Ttl);
        if ((uint)l1 == SaveData.INVALID_VALUE || l1 > 100)
        {
            l1 = 0;
        }
        ttl = l1;

        for (int i = 0; i < sero.Length; i++)
        {
            //
            l1 = SaveData.GetInt(ADDR_Sero + i);
            if ((uint)l1 == SaveData.INVALID_VALUE)
            {
                l1 = 0;
            }
            //if (l1 > 10000 || (i > 0 && l1 < 0))
            if (l1 > 10000 || l1 < 0)
            {
                l1 = 0;
            }
            sero[i] = l1;
        }
        return true;
    }

    static void SaveTtl()
    {
        SaveData.SetInt(ADDR_Ttl, ttl);
        SaveData.Save();
    }
    static void SaveTtl(bool save)
    {
        SaveData.SetInt(ADDR_Ttl, ttl);
        if (save)
            SaveData.Save();
    }
    static void SaveSero(int no)
    {
        if (no < sero.Length)
        {
            SaveData.SetInt(ADDR_Sero + no, sero[no]);
            SaveData.Save();
        }
    }
    static void SaveSero(int no, bool save)
    {
        if (no < sero.Length)
        {
            SaveData.SetInt(ADDR_Sero + no, sero[no]);
            if (save)
                SaveData.Save();
        }
    }

#else
    const byte ADDR_START = (byte)en_IOSAVE_ADDR.JL_START;

    const byte ADDR_ttl = ADDR_START + 0;
    const byte ADDR_sero = ADDR_START + 1;


    //
    const int MAX_READ_ID = MAX_SERO + 1;
    static int readId;

    static void LoadOneStart()
    {
        SaveIO.ReadLongStart((byte)(ADDR_START + readId));
    }
    //
    public static void LoadStart()
    {
        readId = 0;
        LoadOneStart();

        // 加载测试值
        //	LoadTestTab ();
    }
    // Use this for initialization
    public static bool Load()
    {
        if (SaveIO.ReadLongFinish())
        {
            ReadOneData(readId);
            readId++;
            if (readId >= MAX_READ_ID)
            {
                return true;
            }
            else
            {
                LoadOneStart();
            }
        }
        return false;
    }
    //
    static bool ReadOneData(int id)
    {
        int dat;
        bool res = true;

        dat = SaveIO.ReadLong();
        if (dat == SaveIO.INVALID_SLONG)
        {
            //	return false;	//解析数据失败
            dat = 0;
            res = false;
        }

        if (id == 0)
        {
            ttl = dat;
            if (res == false)
            {
                ttl = 0;
                SaveTtl();
            }
        }
        else if (id < sero.Length + 1)
        {
            // sero
            sero[id - 1] = dat;
            if (res == false)
            {
                SaveSero(id - 1);
            }
        }
        return true;
    }

    static void SaveTtl()
    {
        SaveIO.SaveLong((byte)(ADDR_ttl), ttl);
    }

    static void SaveSero(int no)
    {
        if (no < sero.Length)
        {
            SaveIO.SaveLong((byte)(ADDR_sero + no), sero[no]);
        }
    }

#endif


    public static void Init()
    {
        //      if (Main.TestPC) {
#if UNITY_EDITOR
        //int seros = SERO_FPBL;
        //int fpfs;
        //for (int i = 1; i < MAX_SERO; i++) {
        //    if (seros > 0) {
        //        if (seros >= tab_fpFs[i])
        //            fpfs = tab_fpFs[i];
        //        else
        //            fpfs = seros;
        //        sero[i] += fpfs * 3;
        //        seros -= fpfs;
        ////        SaveSero(i);
        //    } else {
        //        break;
        //    }
        //}
        //if (seros > 0) {
        //    sero[0] += seros * 3;
        ////    SaveSero(0);
        //}
#endif
    }
    // 
    public static void Clear()
    {

#if SAVE_FILE
        ttl = 0;
        SaveTtl(false);
        for (int i = 0; i < sero.Length; i++)
        {
            sero[i] = 0;
            SaveSero(i, false);
        }
        SaveData.Save();
#else
        ttl = 0;
        SaveTtl();
        for (int i = 0; i < sero.Length; i++)
        {
            sero[i] = 0;
            SaveSero(i);
        }
#endif
        //SeroFP(3);
    }




    // -------------------------------------------------------------------------------
    // 投币时调用
    public static void PushCoin(int playerno, int coins)
    {
        sero[0] += coins;       // 公用一个金库
        SaveSero(0);
    }

    static void DecSero(int seroid, int score)
    {
        if (seroid == 0 || sero[seroid] >= score)
        {
            sero[seroid] = score;
        }
        else
        {
            sero[0] -= (score - sero[seroid]);
            sero[seroid] = 0;
            SaveSero(0);
        }
        SaveSero(seroid);
    }
    public static int GetBoxJlBase()
    {
        int seros = sero[0];

        if (Set.setVal.OutMode == (int)en_OutMode.OutTicket)
        {
            if (seros < 10)
                return 70;
            if (seros < 50)
                return 100;
            if (seros < 100)
                return 150;
            return 250;
        }
        else if (Set.setVal.OutMode == (int)en_OutMode.OutGift)
        {
            if (seros < (int)(Set.setVal.GiftBl * 1f))
                return 70;
            else if (seros < (int)(Set.setVal.GiftBl * 2f))
                return 150;
            else if (seros < (int)(Set.setVal.GiftBl * 2.5f))
                return 200;
            return 250;
        }
        return 70;
    }
    public static int GetWinNum(int playerno, int reaminTime)
    {
#if ROTATE_OUT
        return 0;
#endif
        if (playerno >= Main.MAX_PLAYER)
            return 0;
        int seroId = 1 + playerno;
        int seros = sero[0] + sero[seroId];
        int jl;
        if (Set.setVal.OutMode == (int)en_OutMode.OutTicket)
        {
            // 退彩票
            int min = 5;
            int max = 50;
            if (seros < 50)
            {
                min = 5;
                max = 50;
            }
            else if (seros < 100)
            {
                min = 20;
                max = 80;
            }
            else
            {
                min = 50;
                max = 100;
            }
            if (max > seros)
                max = seros;
            if (seros >= min && Set.setVal.TicketBl > 0)
            {
                if (seros < 10)
                {
                    jl = 250;
                }
                else if (seros < 30)
                {
                    jl = 500;
                }
                else if (seros < 50)
                {
                    jl = 750;
                }
                else
                {
                    jl = 950;
                }
                if (Random.Range(0, 1000) < jl)
                {
                    int win = Random.Range(min, max);
                    DecSero(seroId, win);
                    return win;
                }
            }
        }
        else if (Set.setVal.OutMode == (int)en_OutMode.OutGift)
        {
            // 退扭蛋
            if (seros >= Set.setVal.GiftBl && Set.setVal.GiftBl > 0)
            {
                if (seros < (int)(Set.setVal.GiftBl * 1.5f))
                    jl = 300;
                else if (seros < (int)(Set.setVal.GiftBl * 2f))
                    jl = 500;
                else if (seros < (int)(Set.setVal.GiftBl * 2.5f))
                    jl = 750;
                else
                    jl = 950;
                if (Random.Range(0, 1000) < jl)
                {
                    DecSero(seroId, Set.setVal.GiftBl);
                    return 1;
                }
            }
        }
        return 0;
    }
}
