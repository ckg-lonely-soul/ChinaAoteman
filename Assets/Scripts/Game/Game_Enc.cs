using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Enc {
	const bool VER_ENC_P15 = true;		//
	//
	enc enc = new enc();

	public static uint MACHINE_NO;			// 机台号
	public static uint mactime;			// 剩余时间(秒)
	public static uint mactype;			// 是否为买卖台

	// 上次打码的账目
	public static uint GENC_LastZyf;		// 总押分
	public static uint GENC_LastZjf;		// 总奖分
	// 当前账目
	public static uint GENC_Zyf;	
	public static uint GENC_Zjf;
	//
	public static uint GENC_NowAcc;		// 当前账目
	public static uint GENC_TotalAcc;		//
	// 校验码
	public static uint GENC_JiaoYan;        //

    // 码
    static ushort[] GENC_Code = new ushort[2];

	//
	public static void LoadStart () {

	}
	//
	public static bool Load () {
		mactime = (uint)PlayerPrefs.GetInt ("GENC_mactime");

		return true;
	}

	// Use this for initialization
	public static void Init () {
		ushort[] iCode = new ushort[2];
		ushort 	i1;
        uint    l1 , l2 ;
        uint    c1;

		// 获取当前总压分，总奖分
		l1 = 0;
		l2 = 0;

		for (c1 = 0; c1 < Main.MAX_PLAYER; c1++) {
			l1 += (uint)FjData.totalAcc [c1].CoinIn;
			l2 += (uint)FjData.totalAcc [c1].TicketOut;
		}
		GENC_TotalAcc = l1 - l2 ;
		GENC_Zyf = l1 ;
		GENC_Zjf = l2 ;	
		GENC_NowAcc = ( l1 - GENC_LastZyf ) - ( l2 - GENC_LastZjf ) ;

		if(GENC_TotalAcc >= 0x80000000)
			GENC_TotalAcc = 0;
		if(GENC_Zyf >= 0x80000000)
			GENC_Zyf = 0;
		if(GENC_Zjf >= 0x80000000)
			GENC_Zjf = 0;
		if(GENC_NowAcc >= 0x80000000)
			GENC_NowAcc = 0;

		GENC_Code[0] = 0 ;
		GENC_Code[1] = 0 ;

		// 校验码
		iCode[0] = (ushort)MACHINE_NO ;
		iCode[1] = (ushort)( ( GENC_Zyf >> 16 ) & 0xFFFF ) ;
		enc.cl_enc_block( iCode ) ;
		iCode[1] = (ushort)( GENC_Zyf & 0xFFFF ) ;
		enc.cl_enc_block( iCode ) ;
		iCode[1] = (ushort)( ( GENC_NowAcc >> 16 ) & 0xFFFF ) ;
		enc.cl_enc_block( iCode ) ;
		iCode[1] = (ushort)( GENC_NowAcc & 0xFFFF ) ;
		enc.cl_enc_block( iCode ) ;
		iCode[1] = (ushort)( ( GENC_Zjf >> 16 ) & 0xFFFF ) ;
		enc.cl_enc_block( iCode ) ;
		iCode[1] = (ushort)( GENC_Zjf & 0xFFFF ) ;
		enc.cl_enc_block( iCode ) ;
		GENC_JiaoYan = iCode[0];
	}
	
	// 打码::
	public static bool SetCode (ushort[] code) 
	{
		ushort[] iCode = new ushort[2];
		ushort i1;
		int c1;

		//确认输入
		enc.cl_dec_block( code ) ;
	
		iCode[0] = (ushort)MACHINE_NO ;
		iCode[1] = (ushort)( ( GENC_Zyf >> 16 ) & 0xFFFF ) ;
		enc.cl_enc_block( iCode ) ;
		iCode[1] = (ushort)( GENC_Zyf & 0xFFFF ) ;
		enc.cl_enc_block( iCode ) ;
		iCode[1] = (ushort)( ( GENC_NowAcc >> 16 ) & 0xFFFF ) ;
		enc.cl_enc_block( iCode ) ;
		iCode[1] = (ushort)( GENC_NowAcc & 0xFFFF ) ;
		enc.cl_enc_block( iCode ) ;
		if (VER_ENC_P15) {
			iCode [1] = (ushort)((GENC_Zjf >> 16) & 0xFFFF);
			enc.cl_enc_block (iCode);
			iCode [1] = (ushort)(GENC_Zjf & 0xFFFF);
			enc.cl_enc_block (iCode);
		}
	
		if ( code[0] == iCode[0] )
		{
			//报码正确
			i1 = code[1] ;
			//允许场数
			//SAVE_SavEEP( GENC_MacTimeSav , (ulong)( i1 & 0x1F ) * 24 * 60 * 60 ) ;//秒数
			//	20天
			if( 20 == ( i1 & 0x1F ) )
			{
				mactype = 1 ;
			}
			mactime = (uint)( i1 & 0x1F ) * 24 * 60 * 60 ;
		//	SAVE_SavEEP( GENC_MacTimeSav , mactime ) ;//秒数
			PlayerPrefs.SetInt ("GENC_mactime", (int)mactime);
			
			
			// 难度 (uchar)( ( i1 >> 5 ) & 0x7 ) ;
			// 波动 (uchar)( ( i1 >> 8 ) & 0x7 ) ;
			// 分控 (uchar)( ( i1 >> 11 ) & 0x7 ) ;
			//Set.setVal.Odds = (byte)( ( i1 >> 5 ) & 0x7 );
			//Set.setVal.Wave = (byte)( ( i1 >> 8 ) & 0x7 );
			//Set.SaveOddsAndWave ();
			
			if ( (i1 & 0x8000) != 0 )
			{
				//清零
			//	FJDATA_Clear();
           //     JL_Clear();
			}
									
			// 记录当前帐
			GENC_Zyf = 0;
			GENC_Zjf = 0;
			for(c1=0; c1<Main.MAX_PLAYER; c1++)
			{
				if (VER_ENC_P15) {
					GENC_Zyf += (uint)FjData.totalAcc [c1].CoinIn;
					GENC_Zjf += (uint)FjData.totalAcc[c1].TicketOut;
				} else {
					GENC_Zyf += (uint)FjData.totalAcc[c1].CoinIn;
					GENC_Zjf += (uint)FjData.totalAcc[c1].TicketOut;
				}
			}						
			GENC_LastZyf = GENC_Zyf ;
			GENC_LastZjf = GENC_Zjf ;
		//	SAVE_SavEEP( GENC_LastZyfSav , GENC_LastZyf ) ;
		//	SAVE_SavEEP( GENC_LastZjfSav , GENC_LastZjf ) ;
			
			return true;
		}
		
		GENC_Code[0] = 0;
		GENC_Code[1] = 0;
		return false;	
	}

}
