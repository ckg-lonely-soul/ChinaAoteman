using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmdIO_GUN1
{
    //
    static SendData sendData;
    public static bool connectStatue;
    public static float connectTimeout;
    public static float connectSendTime;

    // 指令部分
    const int CMDBUF_SIZE = 32;
    const byte CMD_HEAD = 0xdf;
    const byte CMD_VERIFYCODE = 0x49;

    //
    public static byte[] CmdBuf0 = new byte[CMDBUF_SIZE];       //???????????
    static int CmdIn0 = 0;
    //
    static byte[] OutBuf0 = new byte[CMDBUF_SIZE];

    public static int cmdReceiveCnt = 0;
    //
    static byte[] c_Parms = {
        3, //CMDIO_Line = 0,
        1, //CMDIO_KeySta,
        4, //CMDIO_Adc,
        2, //CMDIO_GunOnce,
        2, //CMDIO_GunStart,
        0, //CMDIO_GunStop,
        //CMDIO_Count,
    };


    static void LAN_SendCmd(byte cmd, byte[] buf, int len)
    {
        int i;

        OutBuf0[0] = CMD_HEAD;
        OutBuf0[1] = cmd;
        // data
        for (i = 0; i < len; i++)
        {
            OutBuf0[i + 2] = buf[i];
        }
        // verify
        OutBuf0[len + 2] = CMD_VERIFYCODE;
        for (i = 0; i < len + 2; i++)
        {
            OutBuf0[len + 2] += OutBuf0[i];
        }
        OutBuf0[len + 2] &= 0x7f;
        //
        sendData(OutBuf0, len + 3);
    }


    //
    static int sc_cmdno;
    static byte sc_len;
    static byte sc_c4;
    static int sc_c3;
    public static void LAN_SetCode(byte value)
    {

        if (value == CMD_HEAD)
        {
            CmdBuf0[0] = value;
            CmdIn0 = 1;
        }
        else if (value >= 0x80)
        {
            CmdIn0 = 0;
        }
        else if (CmdIn0 > 0)
        {
            CmdBuf0[CmdIn0] = value;
            CmdIn0++;
            sc_cmdno = CmdBuf0[1];	// 指令号            
            if (sc_cmdno >= CmdBuf0.Length)
            {
                CmdIn0 = 0;
                return;
            }
            sc_len = (byte)(c_Parms[sc_cmdno] + 3); // 长度
            if (CmdIn0 >= sc_len && CmdIn0 >= 3)
            {
                CmdIn0 = 0;
                sc_len--;
                sc_c4 = CMD_VERIFYCODE;
                for (sc_c3 = 0; sc_c3 < sc_len; sc_c3++)
                {
                    sc_c4 += CmdBuf0[sc_c3];
                }
                // 判断检验结果
                if ((sc_c4 & 0x7f) == CmdBuf0[sc_len])
                {
                    // 收到正确指令
                    //Main.Log("Cmd: " + (en_CMDIO)sc_cmdno);
                    switch ((en_CMDIO)sc_cmdno)
                    {
                        case en_CMDIO.CMDIO_Line: // 连线
                            connectTimeout = 0;
                            connectStatue = true;
                            break;

                        case en_CMDIO.CMDIO_KeySta:
                            //
                            ulong old_Key = Key.Key_Old;
                            if ((CmdBuf0[2] & 0x01) == 0)
                            {
                                old_Key &= ~(ulong)(1 << Key.KEY_PLAYER1_OK);
                            }
                            else
                            {
                                old_Key |= (ulong)(1 << Key.KEY_PLAYER1_OK);
                            }
                            if ((CmdBuf0[2] & 0x02) == 0)
                            {
                                old_Key &= ~(ulong)(1 << Key.KEY_PLAYER1_ADD);
                            }
                            else
                            {
                                old_Key |= (ulong)(1 << Key.KEY_PLAYER1_ADD);
                            }
                            Key.KEY_Update(old_Key);
                            //Debug.Log(CmdBuf0[2].ToString("X2"));
                            //
                            if (Main.statue <= en_MainStatue.Game_97)
                            {
                                //PAction.Check();
                            }
                            break;

                        case en_CMDIO.CMDIO_Adc:
                            int adc;
                            for (int i = 0; i < 2; i++)
                            {
                                adc = (CmdBuf0[2 + i * 2] << 7);
                                adc |= (CmdBuf0[3 + i * 2] << 0);
                                Key.UpdateAdVlaue(i, adc);

                            }
                            break;
                    }
                }
            }
        }
    }


    // 用户协议 --------------------------------------------------------------------------------------
    enum en_CMDIO
    {
        CMDIO_Line = 0,
        CMDIO_KeySta,
        CMDIO_Adc,
        CMDIO_GunOnce,
        CMDIO_GunStart,
        CMDIO_GunStop,
        CMDIO_Count,
    };

    public static void Init(SendData funSendData)
    {
        sendData = funSendData;
        //
        for (int i = 0; i < Main.MAX_PLAYER; i++)
        {
            connectTimeout = 0;
            connectStatue = true;
            connectSendTime = i * 0.3f;
        }
    }


    // 发送区
    const int OUT_BUF_SIZE = 64;
    static byte[] CMD0_OutBuf = new byte[OUT_BUF_SIZE];
    static byte[] CMD0_OutBufTemp = new byte[OUT_BUF_SIZE];

    public static void CMD0_SendCmd_GunOnce(byte actTime, byte cdTime)
    {
        CMD0_OutBuf[0] = (byte)(actTime & 0x7f);
        CMD0_OutBuf[1] = (byte)(cdTime & 0x7f);
        //
        LAN_SendCmd((byte)en_CMDIO.CMDIO_GunOnce, CMD0_OutBuf, 2);
    }
    public static void CMD0_SendCmd_GunStart(byte actTime, byte cdTime)
    {
        CMD0_OutBuf[0] = (byte)(actTime & 0x7f);
        CMD0_OutBuf[1] = (byte)(cdTime & 0x7f);
        //
        LAN_SendCmd((byte)en_CMDIO.CMDIO_GunStart, CMD0_OutBuf, 2);
    }
    public static void CMD0_SendCmd_GunStop()
    {
        LAN_SendCmd((byte)en_CMDIO.CMDIO_GunStop, CMD0_OutBuf, 0);
    }

}
