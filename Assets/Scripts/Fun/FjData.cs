using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Acc
{
    public int CoinIn;          //总投币
    public int BillIn;          //总投纸币
    public int CoinOut;         //总退币(礼品所出价值) 
    public int TicketOut;       //总退票
    public int GiftOut;         //退礼品
}

public struct GameData
{
    public int Coins;           // 当前总分
    public int Scores;          // 当前总得分
    public int Wins;            // 当前奖品数(要退的)
    public int JsScores;        // 结算分数
    public int TempScores;      // 临时得分(怪已死，分还没到玩家分数)
    public int TempCoins;       //临时得分(金币)

    // 游戏
    public int GameTime;        // 游戏时间
    public int Blood;           // 血量
    public int BombNum;
    public bool Playing;        // 游戏中    
    public bool Played;         // 是否玩游戏 
}


public class FjData
{
    public static Acc[] acc = new Acc[Main.MAX_PLAYER];             // 当期账目
    public static Acc[] totalAcc = new Acc[Main.MAX_PLAYER];            // 历史总账目
    public static GameData[] g_Fj = new GameData[Main.MAX_PLAYER];      // 当前数据，临时数据


#if SAVE_FILE
    // 本地文件保存 -------------------
    //ACC
    const string ADDR_ACC_CoinIn = "ADDR_ACC_CoinIn";                    //  总投币
    const string ADDR_ACC_BillIn = "ADDR_ACC_BillIn";                    //  总投币
    const string ADDR_ACC_CoinOut = "ADDR_ACC_CoinOut";                    //  总投币
    const string ADDR_ACC_TicketOut = "ADDR_ACC_TicketOut";                    //  总投币
    const string ADDR_ACC_GiftOut = "ADDR_ACC_GiftOut";                    //  总投币
    //TOTALACC
    const string ADDR_TOTALACC_CoinIn = "ADDR_TOTALACC_CoinIn";                    //  总投币
    const string ADDR_TOTALACC_BillIn = "ADDR_TOTALACC_BillIn";                    //  总投币
    const string ADDR_TOTALACC_CoinOut = "ADDR_TOTALACC_CoinOut";                    //  总投币
    const string ADDR_TOTALACC_TicketOut = "ADDR_TOTALACC_TicketOut";                    //  总投币
    const string ADDR_TOTALACC_GiftOut = "ADDR_TOTALACC_GiftOut";                    //  总投币
    //GameData
    const string ADDR_GDATA_Coins = "ADDR_GDATA_Coins";
    const string ADDR_GDATA_Scores = "ADDR_GDATA_Scores";
    const string ADDR_GDATA_Wins = "ADDR_GDATA_Wins";

    public static void LoadStart()
    {

    }
    //
    public static bool Load()
    {
        int l1;

        for (int i = 0; i < Main.MAX_PLAYER; i++)
        {
            //ACC ---------------------------
            //
            l1 = SaveData.GetInt(ADDR_ACC_CoinIn + i);
            if ((uint)l1 == SaveData.INVALID_VALUE)
            {
                l1 = 0;
            }
            acc[i].CoinIn = l1;
            //
            l1 = SaveData.GetInt(ADDR_ACC_BillIn + i);
            if ((uint)l1 == SaveData.INVALID_VALUE)
            {
                l1 = 0;
            }
            acc[i].BillIn = l1;
            //
            l1 = SaveData.GetInt(ADDR_ACC_CoinOut + i);
            if ((uint)l1 == SaveData.INVALID_VALUE)
            {
                l1 = 0;
            }
            acc[i].CoinOut = l1;
            //
            l1 = SaveData.GetInt(ADDR_ACC_TicketOut + i);
            if ((uint)l1 == SaveData.INVALID_VALUE)
            {
                l1 = 0;
            }
            acc[i].TicketOut = l1;
            //
            l1 = SaveData.GetInt(ADDR_ACC_GiftOut + i);
            if ((uint)l1 == SaveData.INVALID_VALUE)
            {
                l1 = 0;
            }
            acc[i].GiftOut = l1;
            //





            //TOTALACC ----------------------
            //
            l1 = SaveData.GetInt(ADDR_TOTALACC_CoinIn + i);
            if ((uint)l1 == SaveData.INVALID_VALUE)
            {
                l1 = 0;
            }
            totalAcc[i].CoinIn = l1;
            //
            //
            l1 = SaveData.GetInt(ADDR_TOTALACC_BillIn + i);
            if ((uint)l1 == SaveData.INVALID_VALUE)
            {
                l1 = 0;
            }
            totalAcc[i].BillIn = l1;
            //

            //
            l1 = SaveData.GetInt(ADDR_TOTALACC_CoinOut + i);
            if ((uint)l1 == SaveData.INVALID_VALUE)
            {
                l1 = 0;
            }
            totalAcc[i].CoinOut = l1;
            //
            //
            l1 = SaveData.GetInt(ADDR_TOTALACC_TicketOut + i);
            if ((uint)l1 == SaveData.INVALID_VALUE)
            {
                l1 = 0;
            }
            totalAcc[i].TicketOut = l1;
            //
            //
            l1 = SaveData.GetInt(ADDR_TOTALACC_GiftOut + i);
            if ((uint)l1 == SaveData.INVALID_VALUE)
            {
                l1 = 0;
            }
            totalAcc[i].GiftOut = l1;
            //



            //GAMEDATA ----------------------
            l1 = SaveData.GetInt(ADDR_GDATA_Coins + i);
            if ((uint)l1 == SaveData.INVALID_VALUE)
            {
                l1 = 0;
            }
            g_Fj[i].Coins = l1;           //
            l1 = SaveData.GetInt(ADDR_GDATA_Scores + i);
            if ((uint)l1 == SaveData.INVALID_VALUE)
            {
                l1 = 0;
            }
            g_Fj[i].Scores = l1;
            //
            l1 = SaveData.GetInt(ADDR_GDATA_Wins + i);
            if ((uint)l1 == SaveData.INVALID_VALUE)
            {
                l1 = 0;
            }
            g_Fj[i].Wins = l1;
        }

        return true;
    }

    // 保存
    // 当期账目
    public static void SaveAcc_CoinIn(int playerno)
    {
        SaveData.SetInt(ADDR_ACC_CoinIn + playerno, acc[playerno].CoinIn);
        SaveData.Save();
    }
    public static void SaveAcc_CoinIn(int playerno, bool save)
    {
        SaveData.SetInt(ADDR_ACC_CoinIn + playerno, acc[playerno].CoinIn);
        if (save)
            SaveData.Save();
    }

    public static void SaveAcc_BillIn(int playerno)
    {
        SaveData.SetInt(ADDR_ACC_BillIn + playerno, acc[playerno].BillIn);
        SaveData.Save();
    }
    public static void SaveAcc_BillIn(int playerno, bool save)
    {
        SaveData.SetInt(ADDR_ACC_BillIn + playerno, acc[playerno].BillIn);
        if (save)
            SaveData.Save();
    }

    public static void SaveAcc_CoinOut(int playerno)
    {
        SaveData.SetInt(ADDR_ACC_CoinOut + playerno, acc[playerno].CoinOut);
        SaveData.Save();
    }
    public static void SaveAcc_CoinOut(int playerno, bool save)
    {
        SaveData.SetInt(ADDR_ACC_CoinOut + playerno, acc[playerno].CoinOut);
        if (save)
            SaveData.Save();
    }

    public static void SaveAcc_TicketOut(int playerno)
    {
        SaveData.SetInt(ADDR_ACC_TicketOut + playerno, acc[playerno].TicketOut);
        SaveData.Save();
    }
    public static void SaveAcc_TicketOut(int playerno, bool save)
    {
        SaveData.SetInt(ADDR_ACC_TicketOut + playerno, acc[playerno].TicketOut);
        if (save)
            SaveData.Save();
    }

    public static void SaveAcc_GiftOut(int playerno)
    {
        SaveData.SetInt(ADDR_ACC_GiftOut + playerno, acc[playerno].GiftOut);
        SaveData.Save();
        // SaveIO.SaveLong((byte)(ADDR_ACC_START + playerno * ADDR_ACC_COUNT + ADDR_GiftOut), acc[playerno].GiftOut);
    }
    public static void SaveAcc_GiftOut(int playerno, bool save)
    {
        SaveData.SetInt(ADDR_ACC_GiftOut + playerno, acc[playerno].GiftOut);
        if (save)
            SaveData.Save();

        // SaveIO.SaveLong((byte)(ADDR_ACC_START + playerno * ADDR_ACC_COUNT + ADDR_GiftOut), acc[playerno].GiftOut);
    }

    // 历史总账目
    public static void SaveTotalAcc_CoinIn(int playerno)
    {
        SaveData.SetInt(ADDR_TOTALACC_CoinIn + playerno, totalAcc[playerno].CoinIn);
        SaveData.Save();
        // SaveIO.SaveLong((byte)(ADDR_TOTALACC_START + playerno * ADDR_ACC_COUNT + ADDR_CoinIn), totalAcc[playerno].CoinIn);
    }
    public static void SaveTotalAcc_CoinIn(int playerno, bool save)
    {
        SaveData.SetInt(ADDR_TOTALACC_CoinIn + playerno, totalAcc[playerno].CoinIn);
        if (save)
            SaveData.Save();
        // SaveIO.SaveLong((byte)(ADDR_TOTALACC_START + playerno * ADDR_ACC_COUNT + ADDR_CoinIn), totalAcc[playerno].CoinIn);
    }


    public static void SaveTotalAcc_BillIn(int playerno)
    {
        SaveData.SetInt(ADDR_TOTALACC_BillIn + playerno, totalAcc[playerno].BillIn);
        SaveData.Save();
        // SaveIO.SaveLong((byte)(ADDR_TOTALACC_START + playerno * ADDR_ACC_COUNT + ADDR_BillIn), totalAcc[playerno].BillIn);
    }
    public static void SaveTotalAcc_BillIn(int playerno, bool save)
    {
        SaveData.SetInt(ADDR_TOTALACC_BillIn + playerno, totalAcc[playerno].BillIn);
        if (save)
            SaveData.Save();
        // SaveIO.SaveLong((byte)(ADDR_TOTALACC_START + playerno * ADDR_ACC_COUNT + ADDR_BillIn), totalAcc[playerno].BillIn);
    }


    public static void SaveTotalAcc_CoinOut(int playerno)
    {
        SaveData.SetInt(ADDR_TOTALACC_CoinOut + playerno, totalAcc[playerno].CoinOut);
        SaveData.Save();
        //  SaveIO.SaveLong((byte)(ADDR_TOTALACC_START + playerno * ADDR_ACC_COUNT + ADDR_CoinOut), totalAcc[playerno].CoinOut);
    }
    public static void SaveTotalAcc_CoinOut(int playerno, bool save)
    {
        SaveData.SetInt(ADDR_TOTALACC_CoinOut + playerno, totalAcc[playerno].CoinOut);
        if (save)
            SaveData.Save();
        //  SaveIO.SaveLong((byte)(ADDR_TOTALACC_START + playerno * ADDR_ACC_COUNT + ADDR_CoinOut), totalAcc[playerno].CoinOut);
    }


    public static void SaveTotalAcc_TicketOut(int playerno)
    {
        SaveData.SetInt(ADDR_TOTALACC_TicketOut + playerno, totalAcc[playerno].TicketOut);
        SaveData.Save();
        //SaveIO.SaveLong((byte)(ADDR_TOTALACC_START + playerno * ADDR_ACC_COUNT + ADDR_TicketOut), totalAcc[playerno].TicketOut);
    }
    public static void SaveTotalAcc_TicketOut(int playerno, bool save)
    {
        SaveData.SetInt(ADDR_TOTALACC_TicketOut + playerno, totalAcc[playerno].TicketOut);
        if (save)
            SaveData.Save();
        //SaveIO.SaveLong((byte)(ADDR_TOTALACC_START + playerno * ADDR_ACC_COUNT + ADDR_TicketOut), totalAcc[playerno].TicketOut);
    }



    public static void SaveTotalAcc_GiftOut(int playerno)
    {
        SaveData.SetInt(ADDR_TOTALACC_GiftOut + playerno, totalAcc[playerno].GiftOut);
        SaveData.Save();
        // SaveIO.SaveLong((byte)(ADDR_TOTALACC_START + playerno * ADDR_ACC_COUNT + ADDR_GiftOut), totalAcc[playerno].GiftOut);
    }
    public static void SaveTotalAcc_GiftOut(int playerno, bool save)
    {
        SaveData.SetInt(ADDR_TOTALACC_GiftOut + playerno, totalAcc[playerno].GiftOut);
        if (save)
            SaveData.Save();
        // SaveIO.SaveLong((byte)(ADDR_TOTALACC_START + playerno * ADDR_ACC_COUNT + ADDR_GiftOut), totalAcc[playerno].GiftOut);
    }


    // 保存游戏数据
    public static void SaveData_Coins(int playerno)
    {
        SaveData.SetInt(ADDR_GDATA_Coins + playerno, g_Fj[playerno].Coins);
        SaveData.Save();
        //SaveIO.SaveLong((byte)(ADDR_GAMEDATA_START + playerno * ADDR_GAMEDATA_COUNT + ADDR_Coins), g_Fj[playerno].Coins);
    }
    public static void SaveData_Coins(int playerno, bool save)
    {
        SaveData.SetInt(ADDR_GDATA_Coins + playerno, g_Fj[playerno].Coins);
        if (save)
            SaveData.Save();
        //SaveIO.SaveLong((byte)(ADDR_GAMEDATA_START + playerno * ADDR_GAMEDATA_COUNT + ADDR_Coins), g_Fj[playerno].Coins);
    }


    public static void SaveData_Scores(int playerno)
    {
        SaveData.SetInt(ADDR_GDATA_Scores + playerno, g_Fj[playerno].Scores);
        SaveData.Save();
        // SaveIO.SaveLong((byte)(ADDR_GAMEDATA_START + playerno * ADDR_GAMEDATA_COUNT + ADDR_Scores), g_Fj[playerno].Scores);
    }
    public static void SaveData_Scores(int playerno, bool save)
    {
        SaveData.SetInt(ADDR_GDATA_Scores + playerno, g_Fj[playerno].Scores);
        if (save)
            SaveData.Save();
        // SaveIO.SaveLong((byte)(ADDR_GAMEDATA_START + playerno * ADDR_GAMEDATA_COUNT + ADDR_Scores), g_Fj[playerno].Scores);
    }


    public static void SaveData_Wins(int playerno)
    {
        SaveData.SetInt(ADDR_GDATA_Wins + playerno, g_Fj[playerno].Wins);
        SaveData.Save();
        //SaveIO.SaveLong((byte)(ADDR_GAMEDATA_START + playerno * ADDR_GAMEDATA_COUNT + ADDR_Wins), g_Fj[playerno].Wins);
    }
    public static void SaveData_Wins(int playerno, bool save)
    {
        SaveData.SetInt(ADDR_GDATA_Wins + playerno, g_Fj[playerno].Wins);
        if (save)
            SaveData.Save();
        //SaveIO.SaveLong((byte)(ADDR_GAMEDATA_START + playerno * ADDR_GAMEDATA_COUNT + ADDR_Wins), g_Fj[playerno].Wins);
    }
#else
    // 读IO板方式 -----------------------------------------------------------------------------
    // 账目地址
    const byte ADDR_ACC_START = (byte)en_IOSAVE_ADDR.FJDATA_START;
    const byte ADDR_TOTALACC_START = (byte)en_IOSAVE_ADDR.FJDATA_START + ADDR_ACC_COUNT * Main.MAX_PLAYER;
    const byte ADDR_CoinIn = 0;         //总投币
    const byte ADDR_BillIn = 1;         //总投币
    const byte ADDR_CoinOut = 2;        //总退币
    const byte ADDR_TicketOut = 3;      //总退票
    const byte ADDR_GiftOut = 4;        //总退蛋
    const int ADDR_ACC_COUNT = 9;

    // 游戏数据、临时数据
    const byte ADDR_GAMEDATA_START = (byte)en_IOSAVE_ADDR.FJDATA_START + ADDR_ACC_COUNT * Main.MAX_PLAYER * 2;
    const byte ADDR_Scores = 0;       //当前得分
    const byte ADDR_Coins = 1;        //当前分数
    const byte ADDR_Wins = 2;		    //当前分数
    const int ADDR_GAMEDATA_COUNT = 3;

    //
    static int readPlayerId;
    static int readId;
    static int readAddrStart;
    static int readAddrLengh;

    enum en_LoadSta
    {
        Acc = 0,        // 账目
        TotalAcc,       // 历史总账目
        GameData,       // 游戏数据
        Others,         // 其它
        End,
    }
    static en_LoadSta loadStatue;
    //
    static void LoadChangeStatue(en_LoadSta sta)
    {
        loadStatue = sta;

        readPlayerId = 0;
        readId = 0;
        switch (loadStatue)
        {
            case en_LoadSta.Acc:
                readAddrStart = ADDR_ACC_START;
                readAddrLengh = ADDR_ACC_COUNT;
                break;
            case en_LoadSta.TotalAcc:
                readAddrStart = ADDR_TOTALACC_START;
                readAddrLengh = ADDR_ACC_COUNT;
                break;
            case en_LoadSta.GameData:
                readAddrStart = ADDR_GAMEDATA_START;
                readAddrLengh = ADDR_GAMEDATA_COUNT;
                break;
            case en_LoadSta.Others:
                readAddrStart = ADDR_GAMEDATA_START;
                readAddrLengh = 0;
                break;
            case en_LoadSta.End:
                readAddrLengh = 0;
                return;
        }
        LoadOneStart();
    }

    //
    static void LoadOneStart()
    {
        SaveIO.ReadLongStart((byte)(readAddrStart + readPlayerId * readAddrLengh + readId));
    }
    // 
    public static void LoadStart()
    {
        LoadChangeStatue(en_LoadSta.Acc);
    }
    //
    public static bool Load()
    {
        if (SaveIO.ReadLongFinish())
        {
            // 接收完成, 下一个,  或者结束
            int dat;
            bool res = true;

            dat = (int)SaveIO.ReadLong();
            Main.Log("Load: " + (readAddrStart + readPlayerId * readAddrLengh + readId).ToString() + ", :" + dat);
            if ((uint)dat == SaveIO.INVALID_LONG)
            {
                dat = 0;
                res = false;
                SaveIO.SaveLong((byte)(readAddrStart + readPlayerId * readAddrLengh + readId), dat);
            }
            switch (loadStatue)
            {
                case en_LoadSta.Acc:
                    ReadAcc(ref acc[readPlayerId], readId, dat);
                    readId++;
                    if (readId >= readAddrLengh)
                    {
                        readId = 0;
                        readPlayerId++;
                        if (readPlayerId >= Main.MAX_PLAYER)
                        {
                            LoadChangeStatue(en_LoadSta.TotalAcc);
                            break;
                        }
                    }
                    LoadOneStart();
                    break;

                case en_LoadSta.TotalAcc:
                    ReadAcc(ref totalAcc[readPlayerId], readId, dat);
                    readId++;
                    if (readId >= readAddrLengh)
                    {
                        readId = 0;
                        readPlayerId++;
                        if (readPlayerId >= Main.MAX_PLAYER)
                        {
                            LoadChangeStatue(en_LoadSta.GameData);
                            break;
                        }
                    }
                    LoadOneStart();
                    break;

                case en_LoadSta.GameData:
                    switch ((byte)readId)
                    {
                        case ADDR_Scores:
                            g_Fj[readPlayerId].Scores = dat;
                            break;
                        case ADDR_Coins:
                            g_Fj[readPlayerId].Coins = dat;
                            break;
                        case ADDR_Wins:
                            g_Fj[readPlayerId].Wins = dat;
                            break;
                    }
                    readId++;
                    if (readId >= readAddrLengh)
                    {
                        readId = 0;
                        readPlayerId++;
                        if (readPlayerId >= Main.MAX_PLAYER)
                        {
                            LoadChangeStatue(en_LoadSta.End);
                            return true;    // 全部接收完成
                        }
                    }
                    LoadOneStart();
                    break;

                case en_LoadSta.Others:
                    readAddrLengh = 0;
                    break;

                case en_LoadSta.End:
                    return true;    // 全部接收完成
            }

        }
        return false;
    }

    // 识别账目
    static void ReadAcc(ref Acc refAcc, int no, int dat)
    {
        switch ((byte)no)
        {
            case ADDR_CoinIn:
                refAcc.CoinIn = dat;
                break;
            case ADDR_BillIn:
                refAcc.BillIn = dat;
                break;
            case ADDR_CoinOut:
                refAcc.CoinOut = dat;
                break;
            case ADDR_TicketOut:
                refAcc.TicketOut = dat;
                break;
            case ADDR_GiftOut:
                refAcc.GiftOut = dat;
                break;
        }
    }


    // 保存
    // 当期账目
    public static void SaveAcc_CoinIn(int playerno)
    {
        SaveIO.SaveLong((byte)(ADDR_ACC_START + playerno * ADDR_ACC_COUNT + ADDR_CoinIn), acc[playerno].CoinIn);
    }
    public static void SaveAcc_BillIn(int playerno)
    {
        SaveIO.SaveLong((byte)(ADDR_ACC_START + playerno * ADDR_ACC_COUNT + ADDR_BillIn), acc[playerno].BillIn);
    }
    public static void SaveAcc_CoinOut(int playerno)
    {
        SaveIO.SaveLong((byte)(ADDR_ACC_START + playerno * ADDR_ACC_COUNT + ADDR_CoinOut), acc[playerno].CoinOut);
    }
    public static void SaveAcc_TicketOut(int playerno)
    {
        SaveIO.SaveLong((byte)(ADDR_ACC_START + playerno * ADDR_ACC_COUNT + ADDR_TicketOut), acc[playerno].TicketOut);
    }
    public static void SaveAcc_GiftOut(int playerno)
    {
        SaveIO.SaveLong((byte)(ADDR_ACC_START + playerno * ADDR_ACC_COUNT + ADDR_GiftOut), acc[playerno].GiftOut);
    }

    // 历史总账目
    public static void SaveTotalAcc_CoinIn(int playerno)
    {
        SaveIO.SaveLong((byte)(ADDR_TOTALACC_START + playerno * ADDR_ACC_COUNT + ADDR_CoinIn), totalAcc[playerno].CoinIn);
    }
    public static void SaveTotalAcc_BillIn(int playerno)
    {
        SaveIO.SaveLong((byte)(ADDR_TOTALACC_START + playerno * ADDR_ACC_COUNT + ADDR_BillIn), totalAcc[playerno].BillIn);
    }
    public static void SaveTotalAcc_CoinOut(int playerno)
    {
        SaveIO.SaveLong((byte)(ADDR_TOTALACC_START + playerno * ADDR_ACC_COUNT + ADDR_CoinOut), totalAcc[playerno].CoinOut);
    }
    public static void SaveTotalAcc_TicketOut(int playerno)
    {
        SaveIO.SaveLong((byte)(ADDR_TOTALACC_START + playerno * ADDR_ACC_COUNT + ADDR_TicketOut), totalAcc[playerno].TicketOut);
    }
    public static void SaveTotalAcc_GiftOut(int playerno)
    {
        SaveIO.SaveLong((byte)(ADDR_TOTALACC_START + playerno * ADDR_ACC_COUNT + ADDR_GiftOut), totalAcc[playerno].GiftOut);
    }

    // 保存游戏数据
    public static void SaveData_Coins(int playerno)
    {
        SaveIO.SaveLong((byte)(ADDR_GAMEDATA_START + playerno * ADDR_GAMEDATA_COUNT + ADDR_Coins), g_Fj[playerno].Coins);
    }
    public static void SaveData_Scores(int playerno)
    {
        SaveIO.SaveLong((byte)(ADDR_GAMEDATA_START + playerno * ADDR_GAMEDATA_COUNT + ADDR_Scores), g_Fj[playerno].Scores);
    }
    public static void SaveData_Wins(int playerno)
    {
        SaveIO.SaveLong((byte)(ADDR_GAMEDATA_START + playerno * ADDR_GAMEDATA_COUNT + ADDR_Wins), g_Fj[playerno].Wins);
    }
#endif

    // 清当期账目
    public static void ClearStart()
    {
        //    SaveIO.ClearLongsStart(ADDR_START, Main.MAX_PLAYER * ADDR_ONE_PLAYER_COUNT);
    }
    public static bool Clear()
    {
        //if (SaveIO.ClearFinish()) {
        //    //
        //    for (int playerno = 0; playerno < acc.Length; playerno++) {
        //        acc[playerno].CoinIn = 0;
        //        acc[playerno].CoinOut = 0;
        //        acc[playerno].TicketOut = 0;
        //        acc[playerno].CoinUp = 0;
        //        acc[playerno].CoinDown = 0;
        //        acc[playerno].Zjf = 0;
        //        acc[playerno].Zyf = 0;
        //        acc[playerno].Scores = 0;
        //        acc[playerno].Coins = 0;
        //        acc[playerno].Wins = 0;
        //    }
        //    return true;
        //}
        //return false;
        for (int playerno = 0; playerno < acc.Length; playerno++)
        {
            // 账目
            acc[playerno].CoinIn = 0;
            acc[playerno].BillIn = 0;
            acc[playerno].CoinOut = 0;
            acc[playerno].TicketOut = 0;
            acc[playerno].GiftOut = 0;
            // 临时数据
            g_Fj[playerno].Scores = 0;
            g_Fj[playerno].Coins = 0;
            g_Fj[playerno].Wins = 0;
            // 保存
#if SAVE_FILE
            SaveAcc_CoinIn(playerno, false);
            SaveAcc_BillIn(playerno, false);
            SaveAcc_CoinOut(playerno, false);
            SaveAcc_TicketOut(playerno, false);
            SaveAcc_GiftOut(playerno, false);
            //
            SaveData_Scores(playerno, false);
            SaveData_Coins(playerno, false);
            SaveData_Wins(playerno, false);
#else 
            SaveAcc_CoinIn(playerno);
            SaveAcc_BillIn(playerno);
            SaveAcc_CoinOut(playerno);
            SaveAcc_TicketOut(playerno);
            SaveAcc_GiftOut(playerno);
            //
            SaveData_Scores(playerno);
            SaveData_Coins(playerno);
            SaveData_Wins(playerno);
#endif
        }

#if SAVE_FILE
        SaveData.Save();
#endif
        return true;
    }


    // 清历史全部账目


    public static void ClearTotalStart()
    {
        //        SaveIO.ClearLongsStart(ADDR_START, Main.MAX_PLAYER * 2 * ADDR_ONE_PLAYER_COUNT);
    }
    public static bool ClearTotal()
    {
        // 当期账目
        Clear();
        // 历史账目
        for (int playerno = 0; playerno < acc.Length; playerno++)
        {
            totalAcc[playerno].CoinIn = 0;
            totalAcc[playerno].BillIn = 0;
            totalAcc[playerno].CoinOut = 0;
            totalAcc[playerno].TicketOut = 0;
            totalAcc[playerno].GiftOut = 0;
            // 保存
#if SAVE_FILE
            SaveTotalAcc_CoinIn(playerno, false);
            SaveTotalAcc_BillIn(playerno, false);
            SaveTotalAcc_CoinOut(playerno, false);
            SaveTotalAcc_TicketOut(playerno, false);
            SaveTotalAcc_GiftOut(playerno, false);
#else 
            SaveTotalAcc_CoinIn(playerno);
            SaveTotalAcc_BillIn(playerno);
            SaveTotalAcc_CoinOut(playerno);
            SaveTotalAcc_TicketOut(playerno);
            SaveTotalAcc_GiftOut(playerno);
#endif
        }
        LeYaoYao.LYY_Clear();
#if SAVE_FILE
        SaveData.Save();
#endif
        return true;
    }
}
