using UnityEngine;

public class PAction
{
    const float OUT_TICKET_TIMEOUT = 3.0f;	//退币超时时间
    const float OUT_GIFT_TIMEOUT = 10.0f;	//退币超时时间
    const float OUT_COIN_TIMEOUT = 6.0f;	//退币超时时间

    public static bool[] outing = new bool[Main.MAX_PLAYER];        // 退币/彩票/礼品 正在退状态
    public static bool[] outEnable = new bool[Main.MAX_PLAYER];     // 退币/彩票/礼品 开关
    static float[] outTimeout = new float[Main.MAX_PLAYER];         // 退币/彩票/礼品超时时间    
    static float[] outDcTime = new float[Main.MAX_PLAYER];         // 退币/彩票/礼品间隔时间    
    static int[] outCount = new int[Main.MAX_PLAYER];                   // 单次已退币个数


    //
    //float[] codeTablePowerTime_CoinIn = new float[Main.MAX_PLAYER];
    //float[] codeTablePowerTime_CoinOut = new float[Main.MAX_PLAYER];
    //bool[] codeTableStatue_CoinIn = new bool[Main.MAX_PLAYER];
    //bool[] codeTableStatue_CoinOut = new bool[Main.MAX_PLAYER];

    // Use this for initialization
    public static void Init()
    {
        for (int i = 0; i < Main.MAX_PLAYER; i++)
        {
            outCount[i] = 0;
            outTimeout[i] = Time.time;
            outDcTime[i] = Time.time;
            outing[i] = false;
            IO.Out_SSR(i, 0);

            //CodeTable_CoinIn_Stop (i);
            //CodeTable_CoinOut_Stop (i);

            if (FjData.g_Fj[i].Wins <= 0)
            {    // 枪神
                outEnable[i] = false;
            }
            else
            {
                outEnable[i] = true;
            }
        }
    }
    // Update is called once per frame
    public static void Check()
    {
        if (Main.statue >= en_MainStatue.Game_98 && Main.statue != en_MainStatue.LoadScene)
            return;

        int coins;
        int playerNum = Main.MAX_PLAYER;
        if (Set.setVal.PlayerMode == (int)en_PlayerMode.One)
        {
            playerNum = 1;
        }
        // 娱乐 --------------------------------------------------------------------------
        for (int i = 0; i < playerNum; i++)
        {
            coins = FjData.g_Fj[i].Coins;
            //投币信号
            if (Key.KEYFJ_CinPressed(i))
            {
                if (i == 0 || Set.setVal.InOutMode != (int)en_InOutMode.OneInOneOut)
                {
                    FjData.acc[i].CoinIn++;
                    FjData.totalAcc[i].CoinIn++;
                    coins++;    //+= (uint)Set.setVal.CoinBl;
                    FjData.SaveAcc_CoinIn(i);
                    FjData.SaveTotalAcc_CoinIn(i);
                    Main.PlaySound_CoinIn();
                    LeYaoYao.LYY_AccCAdd(1);
                }
            }

            //退币信号
            if (Key.KEYFJ_CoutPressed(i))
            {
                if (Time.time - outDcTime[i] >= (float)Set.setVal.OutDcTime / 1000)
                {
                    outDcTime[i] = Time.time;
                    if (Set.setVal.InOutMode == (int)en_InOutMode.TwoInTwoOut ||
                        Set.setVal.PlayerMode == (int)en_PlayerMode.One)
                    {
                        // 双退
                        if (outing[i])
                        {
                            outCount[i]++;
                            outTimeout[i] = Time.time;
                            OutOne_ForYule(i);
                        }
                    }
                    else if (i == 0)
                    {
                        // 单退
                        for (int j = 0; j < Main.MAX_PLAYER; j++)
                        {
                            if (outing[j])
                            {
                                outCount[j]++;
                                outTimeout[j] = Time.time;
                                OutOne_ForYule(j);
                                break;
                            }
                        }
                    }
                }
            }

            // 上分
            //		if(Key.KEYFJ_UpCentPressed(i)){ }

            // 下分
            //		if(Key.KEYFJ_DownCentPressed(i)){ }

            //退币按键 
            //		if(Key.KEYFJ_OutPressed(i)){ }	
            if (outing[i] == true)
            {
                // 正在退
                //	outTimeout[i] += Time.deltaTime;
                switch ((en_OutMode)Set.setVal.OutMode)
                {
                    case en_OutMode.OutTicket:
                        if (Time.time - outTimeout[i] >= OUT_TICKET_TIMEOUT)
                        {
                            SSR_Stop(i);
                            Main.errorStatue[i] |= (ulong)en_ErrorMask.OUT_TIMEOUT;
                            Main.errorOutTimeoutTime[i] = 0;
                            Main.errorChanged[i] = true;
                        }
                        // 剩余个数不足
                        if (FjData.g_Fj[i].Wins <= 0)
                        {
                            SSR_Stop(i);
                            //Main.enableAutoOut[i] = false;
                        }
                        break;
                    case en_OutMode.OutGift:
                        if (Time.time - outTimeout[i] >= OUT_GIFT_TIMEOUT)
                        {
                            SSR_Stop(i);
                            Main.errorStatue[i] |= (ulong)en_ErrorMask.OUT_TIMEOUT;
                            Main.errorOutTimeoutTime[i] = 0;
                            Main.errorChanged[i] = true;
                        }
                        // 剩余个数不足
                        if (FjData.g_Fj[i].Wins <= 0)
                        {
                            SSR_Stop(i);
                            //Main.enableAutoOut[i] = false;
                        }
                        break;
                }
            }
            else if (FjData.g_Fj[i].Wins > 0 && outEnable[i])
            {
                if (Set.setVal.InOutMode == (int)en_InOutMode.TwoInTwoOut ||
                    Set.setVal.PlayerMode == (int)en_PlayerMode.One)
                {
                    // 自动退奖励: 双退
                    if (Main.errorStatue[i] == 0)
                    { // && Main.enableAutoOut[i]) {
                        switch ((en_OutMode)Set.setVal.OutMode)
                        {
                            case en_OutMode.OutTicket:
                            case en_OutMode.OutGift:
                                SSR_Start(i);
                                Main.errorChanged[i] = true;
                                break;
                        }
                    }
                }
                else
                {
                    // (单退)自动退币检测
                    int i1;
                    for (i1 = 0; i1 < Main.MAX_PLAYER; i1++)
                    {
                        if (Main.errorStatue[i1] != 0 || outing[i1])
                        {
                            break;  // 不能退
                        }
                    }
                    if (i1 >= Main.MAX_PLAYER)
                    {
                        //if (Main.enableAutoOut[i]) {
                        // 自动退奖励: 
                        switch ((en_OutMode)Set.setVal.OutMode)
                        {
                            case en_OutMode.OutTicket:
                            case en_OutMode.OutGift:
                                SSR_Start(i);       // 1退口
                                Main.errorChanged[i] = true;
                                return;
                        }
                        //}
                    }
                }
            }

            // 保存
            if (coins != FjData.g_Fj[i].Coins)
            {
                FjData.g_Fj[i].Coins = coins;
                FjData.SaveData_Coins(i);
                Main.coinInChanged[i] = true;
            }
        }

        // 修复
        if (Key.KEYFJ_ResetPressed(0))
        {
            for (int i = 0; i < Main.MAX_PLAYER; i++)
            {
                Main.errorStatue[i] = 0;
            }
        }
    }

    public static bool IsOuting(int playerno)
    {
        return outing[playerno];
    }

    static void OutOne_ForYule(int playerno)
    {
        switch ((en_OutMode)Set.setVal.OutMode)
        {
            case en_OutMode.OutTicket:
                if (FjData.g_Fj[playerno].Wins > 0)
                {
                    FjData.g_Fj[playerno].Wins--;
                    FjData.SaveData_Wins(playerno);
                    Main.outNumChanged[playerno] = true;
                }
                FjData.acc[playerno].TicketOut++;
                FjData.totalAcc[playerno].TicketOut++;
                FjData.SaveAcc_TicketOut(playerno);
                FjData.SaveTotalAcc_TicketOut(playerno);
                break;
            case en_OutMode.OutGift:
                if (FjData.g_Fj[playerno].Wins > 0)
                {
                    FjData.g_Fj[playerno].Wins--;
                    FjData.SaveData_Wins(playerno);
                    Main.outNumChanged[playerno] = true;
                }
                FjData.acc[playerno].GiftOut++;
                FjData.totalAcc[playerno].GiftOut++;
                FjData.SaveAcc_GiftOut(playerno);
                FjData.SaveTotalAcc_GiftOut(playerno);
                break;
        }
        LeYaoYao.LYY_AccCOut(1);
    }

    //
    public static void SSR_Start(int playerno)
    {
        outTimeout[playerno] = Time.time;
        outing[playerno] = true;
        if (Set.setVal.InOutMode == (int)en_InOutMode.TwoInTwoOut)
        {
            IO.Out_SSR(playerno, 1);    // 双退
        }
        else
        {
            IO.Out_SSR(0, 1);           // 单退
        }
    }

    public static void SSR_Stop(int playerno)
    {
        outCount[playerno] = 0;
        outTimeout[playerno] = Time.time;
        outing[playerno] = false;
        if (Set.setVal.InOutMode == (int)en_InOutMode.TwoInTwoOut)
        {
            IO.Out_SSR(playerno, 0);    // 双退
        }
        else
        {
            IO.Out_SSR(0, 0);           // 单退
        }
        outEnable[playerno] = false;
    }
    public static void OnlineCoinIn(int coins)
    {
        FjData.g_Fj[0].Coins += coins;
        FjData.acc[0].CoinIn += coins;
        FjData.totalAcc[0].CoinIn += coins;
        FjData.SaveData_Coins(0);
        FjData.SaveAcc_CoinIn(0);
        FjData.SaveTotalAcc_CoinIn(0);
        Main.PlaySound_CoinIn();
    }
}
