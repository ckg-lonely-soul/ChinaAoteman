using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enc {
	const short		ROUNDS = 32;
	const ushort	DELTA = 0x9e37; ///* sqr(5)-1 * 2^31 */

	static ushort[] key = {0x39a7, 0x56b1, 0x14e5, 0x38f2} ;//Ð¾ÊÆÁ¦
                                                            //	ushort[] key = {49555, 1405, 2160, 28183};

    static void tean(ushort[] v, short N)
	{
		ushort y, z;
		ushort limit, sum = 0;
		
		y = v [0];
		z = v [1];
		
		if (N >= 1) { /* ENCRYPT */
			limit = (ushort)(DELTA * N);
			while (sum != limit) {
				y += (ushort)(((z << 4) ^ (z >> 5)) + (z ^ sum) + key [sum & 3]);
				sum += DELTA;
				z += (ushort)(((y << 4) ^ (y >> 5)) + (y ^ sum) + key [(sum >> 7) & 3]);
			}
		} else { /* DECRYPT */
			sum = (ushort)(DELTA * (-N));
			while (sum != 0) {
				z -= (ushort)(((y << 4) ^ (y >> 5)) + (y ^ sum) + key [(sum >> 7) & 3]);
				sum -= DELTA;
				y -= (ushort)(((z << 4) ^ (z >> 5)) + (z ^ sum) + key [sum & 3]);
			}
		}
		v [0] = y;
		v [1] = z;
	}

	public static void cl_enc_block(ushort[] v)
	{
		tean (v, ROUNDS);
	}

	public static void cl_dec_block(ushort[] v)
	{
		tean (v, -ROUNDS);
	}
}
