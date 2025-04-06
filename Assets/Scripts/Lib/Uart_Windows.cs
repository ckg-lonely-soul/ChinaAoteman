using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading; 

public class Uart_Windows {
	[DllImport("UartWindow_Dll")]		//加载库文件 省略lib前缀和.so后缀
	private static extern int UART_Open(int no, int baud);
	[DllImport("UartWindow_Dll")]
	private static extern int UART_Read(byte[] buf, int offset, int len);
	[DllImport("UartWindow_Dll")]
	private static extern int UART_Write(byte[] buf, int offset, int len);
	[DllImport("UartWindow_Dll")]
	private static extern void UART_Close();


	//
	SetCode setCode;

	//
	public int statue;

	const int MAX_RECIVE_BUF_SIZE = 2048;
	byte[] reciveBuf = new byte[MAX_RECIVE_BUF_SIZE] ;
	int readIn;
	int readOut;

	const int MAX_SENDBUF_SIZE = 2048;
	byte[] writeBuf = new byte[MAX_SENDBUF_SIZE];
	int writeIn;
	int writeOut;


	Thread uartSendThread;
	bool threadSta;

	public void Init(int no, int baud, SetCode setCodeFun)
	{
		UART_Open( no,  baud);
		setCode = setCodeFun;

		readIn = 0;
		readOut = 0;
		writeIn = 0;
		writeOut = 0;

		threadSta = true;
		uartSendThread = new Thread (new ThreadStart (SendDataThread));
		uartSendThread.IsBackground = true;
		uartSendThread.Start ();
	}

	public void Close()
	{
		UART_Close();
		threadSta = false;
	}

	// 写线程
	void SendDataThread()
	{
		const int ONCE_SENDD_LENGTH = 128;		// 单次最多发送 128 字节
		int len;

		while (threadSta) {
			if (writeIn != writeOut) {
				//发送: 单次最多发送 128 字节
				if (writeOut < writeIn)
					len = writeIn - writeOut;
				else
					len = MAX_SENDBUF_SIZE - writeOut;				
				if (len > ONCE_SENDD_LENGTH)
					len = ONCE_SENDD_LENGTH;
				//
				len = UART_Write(writeBuf, writeOut, len);

				writeOut = (writeOut + len) % MAX_SENDBUF_SIZE;
			}
			//
			Thread.Sleep(1);
		}
	}

	//
	public void CheckRead(){
		/*
		while (readIn != readOut) {
			setCode (reciveBuf [readOut]);
			readOut = (readOut + 1) % MAX_RECIVE_BUF_SIZE;
		}
		*/
		int len = UART_Read(reciveBuf, 0, 128);
		if (len > 0) {
			for(int i = 0; i < len; i++){
				setCode (reciveBuf [i]);
			}
		}
	}

	//
	public void SendData(byte[] buf, int len)
	{		
		for (int i = 0; i < len; i++) {
			writeBuf [writeIn] = buf [i];
			writeIn++;
			if (writeIn >= MAX_SENDBUF_SIZE)
				writeIn = 0;
		}
/*
		len = UART_Write(buf, 0, len);
		GameMain.uartTxCnt += len;
*/
	}
}
