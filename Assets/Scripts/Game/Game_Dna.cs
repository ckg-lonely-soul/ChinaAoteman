using UnityEngine;

public class Game_Dna
{
    enc enc = new enc();

    static ushort[] GDNA_Dna = new ushort[4];
    static ushort[] GDNA_DnaCode = new ushort[2];
    public static ushort GDNA_MyDna;

    int GDNA_ReqDna;        // 强制请求进入DNA

    //
    public static void LoadStart()
    {
        CmdIO.CMD0_SendCmd_GetDna();
    }
    //
    public static bool Load()
    {
        // 1: Dna[4]

        // 2: dnaCode[2]
        GDNA_DnaCode[0] = (ushort)PlayerPrefs.GetInt("GDNA_C1");
        GDNA_DnaCode[1] = (ushort)PlayerPrefs.GetInt("GDNA_C2");
        return true;
    }

    // Update is called once per frame
    public static void Init()
    {
        // 1: Dna[4]

        // 2: dnaCode[2]

        // 生成 GDNA_MyDna :
        ushort[] code = new ushort[2];

        //	GDNA_Dna[0] = ( (uint)p[0] << 7 ) | (uint)p[1] ;
        //	GDNA_Dna[1] = ( (uint)p[2] << 7 ) | (uint)p[3] ;
        //	GDNA_Dna[2] = ( (uint)p[4] << 7 ) | (uint)p[5] ;
        //	GDNA_Dna[3] = ( (uint)p[6] << 7 ) | (uint)p[7] ;	

        code[0] = GDNA_Dna[0];
        code[1] = GDNA_Dna[1];
        enc.cl_enc_block(code);
        code[1] = GDNA_Dna[2];
        enc.cl_enc_block(code);
        code[1] = GDNA_Dna[3];
        enc.cl_enc_block(code);
        GDNA_MyDna = 25634;     //	code [1];
    }

    public static bool CheckDnaCode()
    {
        SetDna(GDNA_DnaCode);
        if (Game_Enc.MACHINE_NO == 0)
            return false;
        return true;
    }

    // 打码
    public static bool SetDna(ushort[] dnacode)
    {
        Game_Enc.MACHINE_NO = 0;
        //
        enc.cl_dec_block(dnacode);
        if (dnacode[0] == GDNA_MyDna)
        {
            Game_Enc.MACHINE_NO = dnacode[1];
        }
        enc.cl_enc_block(dnacode);  // 还原
                                    //
        if (Game_Enc.MACHINE_NO != 0)
        {
            if (dnacode[0] != GDNA_DnaCode[0] || dnacode[1] != GDNA_DnaCode[1])
            {
                GDNA_DnaCode[0] = dnacode[0];
                GDNA_DnaCode[1] = dnacode[1];
                //保存
                PlayerPrefs.SetInt("GDNA_C1", GDNA_DnaCode[0]);
                PlayerPrefs.SetInt("GDNA_C2", GDNA_DnaCode[1]);
            }
            return true;
        }
        return false;
    }
}
