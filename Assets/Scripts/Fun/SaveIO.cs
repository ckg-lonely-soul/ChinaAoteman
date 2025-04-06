using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum en_IOSAVE_ADDR{
	MACHINENO = 1,
	//
	DNA_Code1 = 2,
	DNA_Code2 = 3,
	ENC_Code1 = 4,
	ENC_Code2 = 5,
	ENC_TIME = 6,
	ENC_TYPE = 7,
	//
	JL_START = 10,
	//
	FJDATA_START = 50,
}




public class SaveIO {
	public const uint INVALID_LONG = 0x80000000;
	public const int INVALID_SLONG = 0x7FFFFFFF;

	// 地址: DNA,Enc
	public const ushort ADDR_DNA_C1 		= 2;
	public const ushort ADDR_DNA_C2 		= 3;
	public const ushort ADDR_ENC_C1 		= 4;
	public const ushort ADDR_ENC_C2 		= 5;
	public const ushort ADDR_ENC_TIME 	= 6;
	// 地址: 游戏数据
	public const ushort ADDR_FJ_START 	= 20;



	static byte currAddr;           // 当前操作的地址
    static int currReadLength;          // 当前读数据长度
    static bool readFinish;         // 读完成标志
    static bool clearFinish;            // 读完成标志

    static byte[] receiveBuf = new byte[128];
    static int receiveLength;
    static int receiveLong;


	//
	const int DATA_LONG_SIZE	= 10;
	const int OUT_BUF_SIZE		= 32;
    static byte[] outBuf = new byte[OUT_BUF_SIZE];

    // 重发
    static float readTimeout;
    static float clearTimeout;
    

	//  read write long ----------------------------------
	public static void SaveLong(byte addr, int dat)
	{		
		CmdIO.CMD0_SendCmd_WriteLong (addr, dat);
	}
	//
	public static void ReceiveLong(byte addr, int udat)
	{
        Main.receiveDataNum++;
        Main.addr = addr;
        if (addr == currAddr) {
			receiveLong = udat;
			readFinish = true;
		}
	}
	//
	public static void ReadLongStart(byte addr)
	{
        Main.readAddr = addr;
        readFinish = false;
		readTimeout = Time.time; 
		currAddr = addr;
		currReadLength = 1;
		CmdIO.CMD0_SendCmd_ReadLong (currAddr);
	}
	//
	public static bool ReadLongFinish()
	{
		if (readFinish) {
			return true;
		}
		if (Time.time - readTimeout >= 0.5f) {
			readTimeout = Time.time;
			CmdIO.CMD0_SendCmd_ReadLong (currAddr);
		} 
		return false;	
	}
	//
	public static int ReadLong()
	{
		return receiveLong;
	}

    
}
