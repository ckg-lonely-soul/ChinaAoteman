using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save
{
#if NEW_IO
    const int SAVCOUNT = 2;
    const byte RAM_VERIFY_CODE = 0x5A;

    public static void SavLong(int addr, uint value)
    {
        byte[] buf = new byte[5];

        addr *= 10;

        buf[0] = (byte)(value & 0xff);
        buf[1] = (byte)((value >> 8) & 0xff);
        buf[2] = (byte)((value >> 16) & 0xff);
        buf[3] = (byte)((value >> 24) & 0xff);
        buf[4] = (byte)(buf[0] ^ buf[1] ^ buf[2] ^ buf[3] ^ RAM_VERIFY_CODE);
#if !UNITY_EDITOR
        for (int c1 = 0; c1 < SAVCOUNT; c1++) {
            Eeprom.WriteBytes(addr, buf, 5);
            addr += 5;
        }
#endif
    }

    public static uint LoadLong(int addr)
    {
        byte[] buf = new byte[5];
        int c1, c2, c3;
        int addr_sav;
        int l1;

        addr_sav = addr;
        addr *= 10;
#if !UNITY_EDITOR
        for (c1 = 0; c1 < SAVCOUNT; c1++) {
            Eeprom.ReadBytes(addr, buf, 5);
            addr += 5;
            c3 = RAM_VERIFY_CODE;
            for (c2 = 0; c2 < 4; c2++) {
                c3 ^= buf[c2];
            }
            if (c3 == buf[4]) {
                l1 = buf[0] | (buf[1] << 8) | (buf[2] << 16) | (buf[3] << 24);
                if (c1 != 0) {
                    SavLong(addr_sav, (uint)l1);
                }
                return (uint)l1;
            }
        }
#endif
        //
        return 0x80000000;
    }

#endif
}
