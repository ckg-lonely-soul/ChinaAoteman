using System;
using System.Text;
using UnityEngine;

public class LeYaoYao
{
    const int LYY_ACC_LIMIT = 50000;
    const int LYY_SEND_ONCE = 100;
    const int LYY_UPCOINCODE_LIMIT = 255;
    const int LYY_UPCOINS_LIMIT = 0xffff;

    static byte connectFlag;
    static byte connectCnt;
    static float connectTime;
    // 发送账目------------------------------------------------------------------------------------------
    // Addr:
    const string ADDR_LYY_CADD = "LYY_CADD";
    const string ADDR_LYY_COUT = "LYY_COUT";
    const string ADDR_LYY_CADDSENDING = "LYY_CADDSENDING";
    const string ADDR_LYY_COUTSENDING = "LYY_COUTSENDING";
    const string ADDR_LYY_SENDACCINDEX = "LYY_SENDACCINDEX";
    const string ADDR_LYY_UPCOINCODE = "LYY_UPCOINCODE";
    const string ADDR_LYY_UPCOINS = "LYY_UPCOINS";

    //
    static uint cAdd;
    static uint cOut;
    static byte sendingCAdd;
    static byte sendingCOut;
    static byte sendAccResule;
    static byte sendAccIndex;
    static float sendAccTime;

    // 云上分记录:
    static byte upCoinCode;       // 云上分自增码
    static uint upCoins;

    const int LYY_UID_SIZE = 8;
    static byte[] boxUID = new byte[LYY_UID_SIZE];
    static byte[] upCoinUID = new byte[LYY_UID_SIZE];

    //
    static uint LYY_LoadOne(string addr, uint limit, uint def)
    {
        uint l1;
        l1 = (uint)SaveData.GetInt(addr);
        if (l1 == 0x80000000)
        {
            l1 = def;
        }
        if (l1 > limit)
        {
            l1 = def;
        }
        return l1;
    }

    public static void LYY_Init()
    {
        connectFlag = 0;
        connectTime = 2;
        connectCnt = 0;
        sendAccTime = 0;
        // Load: 	
        cAdd = LYY_LoadOne(ADDR_LYY_CADD, LYY_ACC_LIMIT, 0);
        cOut = LYY_LoadOne(ADDR_LYY_COUT, LYY_ACC_LIMIT, 0);
        sendingCAdd = (byte)LYY_LoadOne(ADDR_LYY_CADDSENDING, LYY_SEND_ONCE, 0);
        sendingCOut = (byte)LYY_LoadOne(ADDR_LYY_COUTSENDING, LYY_SEND_ONCE, 0);
        sendAccIndex = (byte)LYY_LoadOne(ADDR_LYY_SENDACCINDEX, 255, 0);
        sendAccIndex = 0;   // 开机不用保存
        //
        upCoinCode = (byte)LYY_LoadOne(ADDR_LYY_UPCOINCODE, LYY_UPCOINCODE_LIMIT, 0);
        upCoins = LYY_LoadOne(ADDR_LYY_UPCOINS, LYY_UPCOINS_LIMIT, 0);
    }

    static void LYY_SaveUpCoinCode()
    {
        if (upCoinCode > LYY_UPCOINCODE_LIMIT)
            upCoinCode = LYY_UPCOINCODE_LIMIT;
        SaveData.SetInt(ADDR_LYY_UPCOINCODE, upCoinCode);
        SaveData.Save();
    }
    static void LYY_SaveUpCoins()
    {
        if (upCoins > LYY_UPCOINS_LIMIT)
            upCoins = LYY_UPCOINS_LIMIT;
        SaveData.SetInt(ADDR_LYY_UPCOINS, (int)upCoins);
        SaveData.Save();
    }

    public static void LYY_SaveSendAccIndex()
    {
        if (sendAccIndex > 255)
            sendAccIndex = 0;
        SaveData.SetInt(ADDR_LYY_SENDACCINDEX, sendAccIndex);
        SaveData.Save();
    }
    public static void LYY_SaveCAdd()
    {
        if (cAdd > LYY_ACC_LIMIT)
            cAdd = LYY_ACC_LIMIT;
        SaveData.SetInt(ADDR_LYY_CADD, (int)cAdd);
        SaveData.Save();
    }
    public static void LYY_SaveCOut()
    {
        if (cOut > LYY_ACC_LIMIT)
            cOut = LYY_ACC_LIMIT;
        SaveData.SetInt(ADDR_LYY_COUT, (int)cOut);
        SaveData.Save();
    }
    public static void LYY_SaveSendingCAdd()
    {
        if (sendingCAdd > LYY_SEND_ONCE)
            sendingCAdd = LYY_SEND_ONCE;
        SaveData.SetInt(ADDR_LYY_CADDSENDING, sendingCAdd);
        SaveData.Save();
    }
    public static void LYY_SaveSendingCOut()
    {
        if (sendingCOut > LYY_SEND_ONCE)
            sendingCOut = LYY_SEND_ONCE;
        SaveData.SetInt(ADDR_LYY_COUTSENDING, sendingCOut);
        SaveData.Save();
    }
    public static void LYY_Clear()
    {
        cAdd = 0;
        cOut = 0;
        sendingCAdd = 0;
        sendingCOut = 0;
        //
        LYY_SaveCAdd();
        LYY_SaveCOut();
        LYY_SaveSendingCAdd();
        LYY_SaveSendingCOut();
    }

    // Cmd -------------------------------------------------------------------------------
    const byte CMD_HEAD = 0xAA;
    const byte CMD_END = 0xDD;
    const byte CMD_VERIFY = 0;
    const byte CMDINDEX_BOX = 0x01;
    const byte CMDINDEX_MAC = 0x02;
    const int CMDBUF_SIZE = 256;
    // 登陆标志(4668)
    const byte COMPAY_IDH = 0x12;
    const byte COMPAY_IDL = 0x3C;

    static byte[] cmdBuf = new byte[CMDBUF_SIZE];
    static int cmdIndex;
    static int cmdLen;
    static float cmdOutTime;

    static byte[] outBuf = new byte[CMDBUF_SIZE];
    static byte[] tempBuf = new byte[CMDBUF_SIZE];

    static void SendCmd(byte cmd, byte[] buf, int len)
    {
        byte verifyCode = CMD_VERIFY;

        // head:
        outBuf[0] = CMD_HEAD;
        // lenght:
        outBuf[1] = (byte)(len + 3);
        // Index:
        outBuf[2] = CMDINDEX_MAC;
        // Cmd:	
        outBuf[3] = cmd;
        // Data:
        for (int i = 0; i < len; i++)
        {
            outBuf[4 + i] = buf[i];
        }
        // SumCheck:
        verifyCode = outBuf[1];
        for (int i = 2; i < len + 4; i++)
        {
            verifyCode ^= outBuf[i];
        }
        outBuf[4 + len] = verifyCode;
        // End:
        outBuf[5 + len] = CMD_END;

        LYY_SendBuff(outBuf, len + 6);

        //string str = "";
        //for (int i = 0; i < len + 5; i++) {
        //    str += outBuf[i].ToString("X2") + ",";
        //}
#if UNITY_EDITOR
        Debug.Log("LyySendCmd: " + (en_CmdLYY)cmd);
#endif
    }

    public static void SetCode(byte value)
    {
        if (cmdIndex > 0 && cmdOutTime <= 0)
        {
            cmdIndex = 0;
        }
        cmdOutTime = 1.0f;

        if (cmdIndex <= 0)
        {
            if (value == CMD_HEAD)
            {
                cmdBuf[0] = value;
                cmdIndex = 1;
            }
        }
        else
        {
            cmdBuf[cmdIndex] = value;
            cmdIndex++;
            if (cmdIndex < 6)
                return;
            // Length:
            if (cmdBuf[1] < 3)
            {
                cmdIndex = 0;
                return;
            }
            // Index:
            if (cmdBuf[2] != CMDINDEX_BOX)
            {
                cmdIndex = 0;
                return;
            }
            if (cmdIndex < cmdBuf[1] + 3)
                return;
            cmdIndex = 0;
            cmdLen = cmdBuf[1] + 2;  // 长度：不包含尾            
            if (cmdBuf[cmdLen] != CMD_END)
                return;
            // CheckSum:--------------------
            cmdLen--;
            //byte verifyCode = CMD_VERIFY;
            byte verifyCode = cmdBuf[1];
            for (int i = 2; i < cmdLen; i++)
            {
                verifyCode ^= cmdBuf[i];
            }
            if (verifyCode != cmdBuf[cmdLen])
                return;
            // ReciveCmd:-------------------
#if UNITY_EDITOR
            Debug.Log("LYYCmd: " + (en_CmdLYY)cmdBuf[3]);
#endif
            connectFlag = 1;
            connectTime = 5;

            switch ((en_CmdLYY)cmdBuf[3])
            {
                case en_CmdLYY.Online:
                    for (int i = 0; i < LYY_UID_SIZE; i++)
                    {
                        boxUID[i] = cmdBuf[4 + i];
                    }
                    connectFlag = 1;
                    SendCmd_Online();
                    break;

                case en_CmdLYY.UpCoin:
                    // 自增码: cmdBuf[4]
                    // 投币数:
                    uint coins = (uint)(cmdBuf[5] << 0);
                    coins |= (uint)(cmdBuf[6] << 8);
                    //
                    if (upCoinCode != cmdBuf[4] || upCoins != coins)
                    {
                        upCoinCode = cmdBuf[4];
                        upCoins = coins;
                        PAction.OnlineCoinIn((int)coins);
                        LYY_SaveUpCoinCode();
                        LYY_SaveUpCoins();
                        Debug.Log("OnLineUpCoin: " + coins + ", " + upCoinCode);
                    }
                    // 金额:()
                    SendCmd_UpCoinRet(1);
                    break;

                case en_CmdLYY.GetQrCode:
                    byte[] byteArray = new byte[cmdLen - 4];
                    for (int i = 0; i < cmdLen - 4; i++)
                    {
                        byteArray[i] = cmdBuf[i + 4];
                    }
                    ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                    try
                    {
                        string str = asciiEncoding.GetString(byteArray);
                        //Debug.Log("QRCode: " + str);                    
                        if (QRcodeDraw.instance != null)
                        {
                            QRcodeDraw.instance.Update_QrCode(str);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("QRCode Error: " + e.Message);
                    }
                    break;

                case en_CmdLYY.UploadAcc:
                    if (sendingCAdd > 0)
                    {
                        sendingCAdd = 0;
                        sendAccTime = 30;     // 30s
                        LYY_SaveSendingCAdd();
                    }
                    if (sendingCOut > 0)
                    {
                        sendingCOut = 0;
                        sendAccTime = 30;     // 30s
                        LYY_SaveSendingCOut();
                    }
                    break;

                case en_CmdLYY.GetSet:
                    SendCmd_GetSetRet();
                    break;

                case en_CmdLYY.SaveSet:
                    Set.ReciveBuf(cmdBuf, 4);
                    SendCmd_SaveSetRet();
                    break;
            }
        }
    }

    enum en_CmdLYY
    {
        Online = 0x01,
        UpCoin = 0x03,       // 云上分
        GetSet = 0x05,       // 查询终端参数
        SaveSet = 0x06,      // 设置终端参数
        GetQrCode = 0x10,        // 获取二维码
        UploadAcc = 0x13,        // 上传账目增量
        GetNetSta = 0x19,		// 查询盒子网络状态
    }

    //SendCmd ------------------------------------
    static void SendCmd_Online()
    {
        tempBuf[0] = COMPAY_IDL;
        tempBuf[1] = COMPAY_IDH;
        SendCmd((byte)en_CmdLYY.Online, tempBuf, 2);
    }
    public static void SendCmd_GetQrCode()
    {
        SendCmd((byte)en_CmdLYY.GetQrCode, tempBuf, 0);
    }
    //static void SendCmd_GetAccIndex() {
    //    SendCmd((byte)en_CmdLYY.GetAccIndex, tempBuf, 0);
    //}
    static void SendCmd_UploadAcc(byte coinIn, byte giftOut)
    {
        // 自增码
        tempBuf[0] = sendAccIndex;
        // 故障码
        tempBuf[1] = 0;
        // 投币增量
        tempBuf[2] = coinIn;
        // 退礼品增量
        tempBuf[3] = giftOut;
        // 营收增量
        tempBuf[4] = 0;
        // 预留[2]
        tempBuf[5] = 0;
        tempBuf[6] = 0;
        //
        SendCmd((byte)en_CmdLYY.UploadAcc, tempBuf, 7);
    }
    static void SendCmd_GetSetRet()
    {
        int len = Set.WriteBuf(tempBuf, 0);
        SendCmd((byte)en_CmdLYY.GetSet, tempBuf, len);
    }
    static void SendCmd_SaveSetRet()
    {
        tempBuf[0] = 1;
        SendCmd((byte)en_CmdLYY.SaveSet, tempBuf, 1);
    }

    static void SendCmd_UpCoinRet(byte res)
    {
        tempBuf[0] = res;
        SendCmd((byte)en_CmdLYY.UpCoin, tempBuf, 1);
    }


    //---------------------------------------
    public static void LYY_Check()
    {
        if (cmdOutTime > 0)
            cmdOutTime -= Time.deltaTime;
        // connect check --------------------
        if (connectFlag == 0)
            return;

        // accSend check --------------------
        if (sendAccTime > 0)
        {
            sendAccTime -= Time.deltaTime;
            return;
        }
        sendAccTime = 1f;   // 200ms
                            //	if(sendAccResule == 0)
                            //		return;
        if (sendingCAdd > 0)
        {
            SendCmd_UploadAcc(sendingCAdd, 0);
            sendAccTime = 10;     // 10 秒
            return;
        }
        if (sendingCOut > 0)
        {
            SendCmd_UploadAcc(0, sendingCOut);
            sendAccTime = 10;     // 10 秒
            return;
        }
        //
        if (cAdd > 0)
        {
            if (cAdd > LYY_SEND_ONCE)
            {
                sendingCAdd = LYY_SEND_ONCE;
            }
            else
            {
                sendingCAdd = (byte)cAdd;
            }
            cAdd -= sendingCAdd;
            sendAccIndex++;
            LYY_SaveSendingCAdd();
            LYY_SaveCAdd();
            LYY_SaveSendAccIndex();
            SendCmd_UploadAcc(sendingCAdd, 0);
            sendAccTime = 10;     // 60 秒
            return;
        }
        if (cOut > 0)
        {
            if (cOut > LYY_SEND_ONCE)
            {
                sendingCOut = LYY_SEND_ONCE;
            }
            else
            {
                sendingCOut = (byte)cOut;
            }
            cOut -= sendingCOut;
            sendAccIndex++;
            LYY_SaveSendingCOut();
            LYY_SaveCOut();
            LYY_SaveSendAccIndex();
            SendCmd_UploadAcc(0, sendingCOut);
            sendAccTime = 10;     // 10 秒
        }
    }
    public static void LYY_AccCAdd(uint value)
    {
        cAdd += value;
        LYY_SaveCAdd();
    }
    public static void LYY_AccCOut(uint value)
    {
        cOut += value;
        LYY_SaveCOut();
    }


    static void LYY_SendBuff(byte[] buf, int len)
    {
        //CmdIO_GZF.CMD0_SendCmd_Transmit(buf, len);
        MainRun.uartLYY.SendData(buf, len);

    }
}
