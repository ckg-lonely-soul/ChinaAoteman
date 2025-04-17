using UnityEngine;

public delegate void SendData(byte[] buf, int len);

// IO板串口协议
public class CmdIO
{
    //
    static SendData sendData;
    public static bool connectStatue = true;
    public static float connectTimeout = 0;
    public static float connectSendTime = 0;

    // 指令部分
    const int LAN_CMDBUF_SIZE = 128;
    const byte LAN_CMD_HEAD_1 = 0x9a;
    const byte LAN_CMD_HEAD_2 = 0x5b;
    const ushort LAN_CMD_VERIFYCODE = 0x3015;
    const byte LAN_CMD_VERIFYCODE_01 = 0x6a;    // 漫威科技

    //
    static byte[] LAN_CmdBuf = new byte[LAN_CMDBUF_SIZE];       //???????????
    static int LAN_CmdBufIndex = 0;
    //
    static byte[] LAN_OutBuf = new byte[LAN_CMDBUF_SIZE];

    public static int cmdReceiveCnt = 0;
    //
    static byte[] c_Parms = {
        8,		// 0
		1,		// 1
		3,		// 2
		2,		// 3
		2,		// 4
		5,		// 5
		5,		// 6
		16,		// 7
		3,		// 8
		0,		// 9
        5,      // 10
        5,      // 11
        2,      // Version
        3 ,		// GunOnce
		3 ,		//
		1 ,
        0 ,     // 16
        2 ,     // 17
    };
    public static void CheckConnect()
    {
        connectTimeout += Time.deltaTime;
        if (connectTimeout >= 1.5f)
        {
            connectTimeout = 0;
            connectStatue = false;
        }
        connectSendTime += Time.deltaTime;
        if (connectSendTime >= 0.5f)
        {
            connectSendTime = 0;
            CMD0_SendCmd_Line();
        }
    }
    //
    static int LAN_MakeCmd(byte[] outbuf, byte[] inbuf, int len)
    {

        return len + 8;
    }

    static void LAN_SendCmd(byte cmd, byte[] buf)
    {
        int i;

        LAN_OutBuf[0] = LAN_CMD_HEAD_1;
        LAN_OutBuf[1] = LAN_CMD_HEAD_2;
        LAN_OutBuf[2] = cmd;
        // data
        for (i = 0; i < c_Parms[cmd]; i++)
        {
            LAN_OutBuf[i + 3] = buf[i];
        }
        // verify
        if (Main.COMPANY_NUM == 6)
        {
            LAN_OutBuf[c_Parms[cmd] + 3] = LAN_CMD_VERIFYCODE_01;
        }
        else
        {
            LAN_OutBuf[c_Parms[cmd] + 3] = 0;
        }
        for (i = 0; i < c_Parms[cmd] + 3; i++)
        {
            LAN_OutBuf[c_Parms[cmd] + 3] += LAN_OutBuf[i];
        }
        //
        sendData(LAN_OutBuf, c_Parms[cmd] + 4);
    }

    //
    static int LAN_DeCode(byte[] inbuf, int len, byte[] outbuf)
    {

        return len;
    }


    //
    static byte sc_cmdno;
    static byte sc_len;
    static byte sc_c4;
    static int sc_c3;
    public static void LAN_SetCode(byte value)
    {
        if (LAN_CmdBufIndex == 0)
        {
            if (value == LAN_CMD_HEAD_1)
            {
                LAN_CmdBuf[LAN_CmdBufIndex] = value;
                LAN_CmdBufIndex++;
            }
        }
        else if (LAN_CmdBufIndex == 1)
        {
            if (value == LAN_CMD_HEAD_2)
            {
                LAN_CmdBuf[LAN_CmdBufIndex] = value;
                LAN_CmdBufIndex++;
            }
            else
            {
                LAN_CmdBufIndex = 0;
            }
        }
        else
        {
            LAN_CmdBuf[LAN_CmdBufIndex] = value;

            sc_cmdno = LAN_CmdBuf[2];	// 指令号
            if (sc_cmdno >= c_Parms.Length)
            {
                LAN_CmdBufIndex = 0;
                return;
            }
            sc_len = (byte)(c_Parms[sc_cmdno] + 4); // 长度
            if (++LAN_CmdBufIndex >= sc_len)
            {
                LAN_CmdBufIndex = 0;
                sc_len--;
                if (Main.COMPANY_NUM == 6)
                {
                    sc_c4 = LAN_CMD_VERIFYCODE_01;
                }
                else
                {
                    sc_c4 = 0;
                }
                for (sc_c3 = 0; sc_c3 < sc_len; sc_c3++)
                {
                    sc_c4 += LAN_CmdBuf[sc_c3];
                }
                // 判断检验结果
                if ((sc_c4 & 0xff) == LAN_CmdBuf[sc_len])
                {
                    // 收到正确指令
                    cmdReceiveCnt++;
                    switch ((en_CMD0)sc_cmdno)
                    {
                        case en_CMD0.CMD0_ENC:
                            break;
                        case en_CMD0.CMD0_LED_ALL:
                            break;
                        case en_CMD0.CMD0_KEY_STA:
                            ulong l1 = 0;
                            for (sc_c3 = 0; sc_c3 < c_Parms[sc_cmdno]; sc_c3++)
                            {
                                l1 |= (ulong)(LAN_CmdBuf[3 + sc_c3] << (sc_c3 * 8));
                            }
                            Key.KEY_Update(l1);
                            if (Main.statue <= en_MainStatue.Game_97 || Main.statue == en_MainStatue.LoadScene)
                            {
                                PAction.Check();
                            }
                            break;
                        case en_CMD0.CMD0_LED_ONE:
                            break;
                        case en_CMD0.CMD0_SSR_ONE:
                            break;
                        case en_CMD0.CMD0_READ_EEP:
                            int dat;
                            dat = (LAN_CmdBuf[3] << 8);
                            dat |= (LAN_CmdBuf[4] << 16);
                            dat |= (LAN_CmdBuf[6] << 0);
                            dat |= (LAN_CmdBuf[7] << 24);
                            SaveIO.ReceiveLong(LAN_CmdBuf[5], dat);
                            break;
                        case en_CMD0.CMD0_SAVE_EEP:
                            break;
                        case en_CMD0.CMD0_GET_ADC:
                            int[] adValue = new int[4];
                            for (sc_c3 = 0; sc_c3 < 4; sc_c3++)
                            {
                                adValue[sc_c3] = LAN_CmdBuf[3 + sc_c3 * 4];
                                adValue[sc_c3] |= LAN_CmdBuf[4 + sc_c3 * 4] << 8;
                                adValue[sc_c3] |= LAN_CmdBuf[5 + sc_c3 * 4] << 16;
                                adValue[sc_c3] |= LAN_CmdBuf[6 + sc_c3 * 4] << 24;
#if !UNITY_EDITOR
                            Key.UpdateAdVlaue (sc_c3, adValue [sc_c3]);
#endif
                            }
                            break;
                        case en_CMD0.CMD0_SEND_UID:
                            break;
                        case en_CMD0.CMD0_BLUETOOTH:
                            break;

                        case en_CMD0.CMD0_GUNONCE:
                            break;
                        case en_CMD0.CMD0_LINE: // 连线
                            connectTimeout = 0;
                            connectStatue = true;
                            break;
                    }
                }
            }
        }
    }


    // 用户协议 --------------------------------------------------------------------------------------
    enum en_CMD0
    {
        CMD0_ENC = 0,
        CMD0_LED_ALL = 1,
        CMD0_KEY_STA = 2,
        CMD0_LED_ONE = 3,
        CMD0_SSR_ONE = 4,
        CMD0_READ_EEP = 5,
        CMD0_SAVE_EEP = 6,
        CMD0_GET_ADC = 7,
        CMD0_SEND_UID = 8,
        CMD0_BLUETOOTH = 9,
        CMD0_LINEDATA = 9,      //��������
        CMD0_LOADSLONG = 10,        //
        CMD0_SAVESLONG = 11,
        CMD0_VERSION = 12,  //
        CMD0_GUNONCE = 13,
        CMD0_GUNSTART = 14,
        CMD0_GUNSTOP = 15,
        CMD0_LINE = 16,       // 连线
        CMD0_BUTTONLED = 17,
    }

    public static void Init(SendData funSendData)
    {
        sendData = funSendData;
    }

    // 
    static void CMD0_ReceiveCmd(byte[] buf, int len)
    {

    }



    // 发送区
    const int OUT_BUF_SIZE = 64;
    static byte[] CMD0_OutBuf = new byte[OUT_BUF_SIZE];
    static byte[] CMD0_OutBufTemp = new byte[OUT_BUF_SIZE];


    //
    public static void CMD0_SendCmd_GetDna()
    {
        //
        LAN_SendCmd((byte)en_CMD0.CMD0_SEND_UID, CMD0_OutBuf);
    }

    //
    public static void CMD0_SendCmd_ReadLong(byte addr)
    {
        CMD0_OutBuf[2] = addr;
        //
        LAN_SendCmd((byte)en_CMD0.CMD0_READ_EEP, CMD0_OutBuf);
    }
    //
    public static void CMD0_SendCmd_WriteLong(byte addr, int dat)
    {
        CMD0_OutBuf[1] = addr;
        CMD0_OutBuf[4] = (byte)((dat >> 0) & 0xff);
        CMD0_OutBuf[2] = (byte)((dat >> 8) & 0xff);
        CMD0_OutBuf[3] = (byte)((dat >> 16) & 0xff);
        CMD0_OutBuf[0] = (byte)((dat >> 24) & 0xff);
        //
        LAN_SendCmd((byte)en_CMD0.CMD0_SAVE_EEP, CMD0_OutBuf);
    }

    //
    public static void CMD0_SendCmd_IO(byte value)
    {
        CMD0_OutBuf[0] = value;
        //
        LAN_SendCmd((byte)en_CMD0.CMD0_LED_ALL, CMD0_OutBuf);
    }
    public static void CMD0_SendCmd_LED(byte no, byte enable)
    {
        CMD0_OutBuf[0] = no;
        CMD0_OutBuf[1] = enable;
        //
        LAN_SendCmd((byte)en_CMD0.CMD0_LED_ONE, CMD0_OutBuf);
    }
    public static void CMD0_SendCmd_SSR(byte no, byte enable)
    {
        CMD0_OutBuf[0] = no;
        CMD0_OutBuf[1] = enable;
        //
        LAN_SendCmd((byte)en_CMD0.CMD0_SSR_ONE, CMD0_OutBuf);
    }

    public static void CMD0_SendCmd_GunOnce(byte no, byte enableTime, byte dcTime)
    {
        CMD0_OutBuf[0] = no;
        CMD0_OutBuf[1] = enableTime;
        CMD0_OutBuf[2] = dcTime;
        //
        LAN_SendCmd((byte)en_CMD0.CMD0_GUNONCE, CMD0_OutBuf);
    }
    public static void CMD0_SendCmd_GunStart(byte no, byte enableTime, byte dcTime)
    {
        CMD0_OutBuf[0] = no;
        CMD0_OutBuf[1] = enableTime;
        CMD0_OutBuf[2] = dcTime;
        //
        LAN_SendCmd((byte)en_CMD0.CMD0_GUNSTART, CMD0_OutBuf);
    }
    public static void CMD0_SendCmd_GunStop(byte no)
    {
        CMD0_OutBuf[0] = no;
        //
        LAN_SendCmd((byte)en_CMD0.CMD0_GUNSTOP, CMD0_OutBuf);
    }
    public static void CMD0_SendCmd_Line()
    {
        //
        LAN_SendCmd((byte)en_CMD0.CMD0_LINE, CMD0_OutBuf);
    }
    public static void CMD0_SendCmd_ButtonLED(byte no, byte enable)
    {
        CMD0_OutBuf[0] = no;
        CMD0_OutBuf[1] = enable;
        //
        LAN_SendCmd((byte)en_CMD0.CMD0_BUTTONLED, CMD0_OutBuf);
    }
}
