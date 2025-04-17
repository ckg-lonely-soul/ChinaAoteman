using System;
using System.IO;
using UnityEngine;

public class SaveData
{
    public const uint INVALID_VALUE = 0x80000000;
    static string _fileName = Application.persistentDataPath + "/UnityUserData";

    public static void Init()
    {
        FileInfo file = new FileInfo(_fileName);
        if (!file.Exists)
        {
            FileStream fStream = File.Open(_fileName, FileMode.Create);
            fStream.Flush();
            fStream.Close();
        }
    }

    static int ReadFile(string filename, int addr, byte[] buf, int len)
    {
        int rlen = 0;
        FileStream fStream = File.Open(filename, FileMode.OpenOrCreate);

        if (addr + len <= fStream.Length)
        {
            fStream.Seek(addr, SeekOrigin.Begin);
            rlen = fStream.Read(buf, 0, len);
        }
        fStream.Close();
        return rlen;
    }
    static void WriteFile(string filename, int addr, byte[] buf, int len)
    {
        FileStream fStream = File.Open(filename, FileMode.OpenOrCreate);

        fStream.Seek(addr, SeekOrigin.Begin);
        fStream.Write(buf, 0, len);
        fStream.Flush();
        fStream.Close();
        fStream.Dispose();
        //fStream.fsync(fileno(stream));
    }




    static int ReadPlayerPrefs(string key, byte[] buf, int len)
    {
        //if (key == "ADD_FJ_Zf0_" + 0 || key == "ADD_FJ_Zf0_" + 1) {
        //    Main.Log("R_Str: " + key);
        //}

        string str = PlayerPrefs.GetString(key);
        //if (key == "ADD_FJ_Zf0_" + 0 || key == "ADD_FJ_Zf0_" + 1) {
        //    Main.Log("R_Str: " + str);
        //}
        int id = 0;
        int dat;
        for (int i = 0; i < str.Length && id < len; i++)
        {
            dat = Convert.ToInt32(str[i]);
            if (dat >= '0' && dat <= '9')
            {
                dat -= '0';
            }
            if (dat >= 'A' && dat <= 'F')
            {
                dat -= 'A';
                dat += 10;
            }
            if ((i % 2) == 0)
            {
                buf[id] = (byte)(dat << 4);
            }
            else
            {
                buf[id] |= (byte)(dat & 0x0f);
                id++;
            }
        }

        return id;
    }
    static void WritePlayerPrefs(string key, byte[] buf, int len)
    {
        string str = "";
        for (int i = 0; i < len; i++)
        {
            str += buf[i].ToString("X2");
        }

        //if (key == "ADD_FJ_Zf0_" + 0 || key == "ADD_FJ_Zf0_" + 1) {
        //    Main.Log("W_Str: " + key);
        //    Main.Log("W_Str: " + str);
        //}

        PlayerPrefs.SetString(key, str);
    }



    static byte[] saveBuf = new byte[16];
    public static uint ReadLong(string fileName, int addr)
    {
        uint dat = 0;
        byte verfiy;

        for (int j = 0; j < 2; j++)
        {
            //int rlen = ReadFile(fileName + j, addr * 5, saveBuf, 5);
            int rlen = ReadPlayerPrefs(fileName + "_" + j, saveBuf, 5);
            if (rlen < 5)
            {
                // 数据不存在
                continue;
            }
            verfiy = (byte)0x54;
            for (int i = 0; i < 4; i++)
            {
                verfiy += saveBuf[i];
            }
            if (verfiy == saveBuf[4])
            {
                dat = (uint)saveBuf[0] << 24;
                dat |= (uint)saveBuf[1] << 16;
                dat |= (uint)saveBuf[2] << 8;
                dat |= (uint)saveBuf[3] << 0;
                return dat;
            }
        }

        return INVALID_VALUE;
    }

    public static void SaveLong(string fileName, int addr, int value)
    {

        saveBuf[0] = (byte)((value >> 24) & 0xff);
        saveBuf[1] = (byte)((value >> 16) & 0xff);
        saveBuf[2] = (byte)((value >> 8) & 0xff);
        saveBuf[3] = (byte)((value >> 0) & 0xff);
        //
        saveBuf[4] = (byte)0x54;
        for (int i = 0; i < 4; i++)
        {
            saveBuf[4] += saveBuf[i];
        }
        //WriteFile(fileName + 0, addr * 5, saveBuf, 5);
        //WriteFile(fileName + 1, addr * 5, saveBuf, 5);

        WritePlayerPrefs(fileName + "_" + 0, saveBuf, 5);
        WritePlayerPrefs(fileName + "_" + 1, saveBuf, 5);
    }

    public static void SetInt(string key, int value)
    {
        //SaveLong(_fileName + key, 0, value);
        SaveLong(key, 0, value);
    }
    public static int GetInt(string key, int def)
    {
        //return (int)ReadLong(_fileName + key, 0);
        return (int)ReadLong(key, 0);
    }
    public static int GetInt(string key)
    {
        return (int)ReadLong(key, 0);
    }





    public static void Save()
    {
        PlayerPrefs.Save();
    }


}
