using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class Eeprom
{
    [DllImport("ztl_i2cwr")]
    public static extern int ztl_i2c_read(byte dev, byte[] buff, int addr, int count);
    [DllImport("ztl_i2cwr")]
    public static extern int ztl_i2c_write(byte dev, byte[] buff, int addr, int count);


    public static int ReadBytes(int addr, byte[] buf, int len)
    {
        //return 0;
        return ztl_i2c_read(1, buf, addr, len);
    }

    public static int WriteBytes(int addr, byte[] buf, int len)
    {
        // return 0;
        return ztl_i2c_write(1, buf, addr, len);
    }
}
