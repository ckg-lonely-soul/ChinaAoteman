using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;


public delegate void SetCode(byte dat);

public class Uart_Android {
	[DllImport("serial_port")]		//加载库文件 省略lib前缀和.so后缀
	private static extern int SERIALPORT_Init(int no, int baud);
	[DllImport("serial_port")]
	private static extern int SERIALPORT_Read(int fd, byte[] buf, int len);
	[DllImport("serial_port")]
	private static extern int SERIALPORT_Write(int fd, byte[] buf, int len);
	[DllImport("serial_port")]
	private static extern void SERIALPORT_Close(int fd);
	[DllImport("serial_port")]
	private static extern void SERIALPORT_Clse(int fd);
	[DllImport("serial_port")]
	private static extern int addFun(int a, int b);

	//
	SetCode setCode;
	//
	public int portFd;		// 外接串口
//	static int portFd3;		// 板上接口

	//
	const int MAX_RBUF_SIZE = 2048;
	const int MAX_SENDBUF_SIZE = 2048;
	const int MAX_CMDBUF_SIZE = 64;

	static byte[] CmdBuf1 = new byte[MAX_CMDBUF_SIZE];
	static byte[] readBuf = new byte[MAX_RBUF_SIZE] ;
	static int CmdIn1;
	//--------

	static byte[] uartBuf = new byte[MAX_RBUF_SIZE] ;
	static int uartBytes = 0;
	static ulong keyStatue;

    public int receiveCnt = 0;
	//---------------------------------------------------
	//
	public void Init(int no, int baud, SetCode funSetCode)
	{
		portFd = SERIALPORT_Init (no, baud);
		setCode = funSetCode;
	}
	//
	public void CheckRead()
	{
		if (portFd > 0) {
			int rlen = SERIALPORT_Read ( portFd, readBuf, MAX_RBUF_SIZE );
			if(rlen > 0)
			{
                receiveCnt += rlen;
                for (int i = 0; i < rlen; i++) {
					if (setCode != null) {
						setCode (readBuf [i]);
					}
				}
			}
		}
	}

	//
	public void SendData(byte[] buf, int len)
	{
		if(portFd > 0) {
			SERIALPORT_Write(portFd, buf, len);
		}
	}

	public void Close()
	{
		if (portFd > 0) {
			SERIALPORT_Close (portFd);
		}
	}
}
