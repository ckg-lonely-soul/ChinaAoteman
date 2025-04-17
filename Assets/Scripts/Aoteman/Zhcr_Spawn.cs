using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zhcr_Spawn : MonoBehaviour {

	public Zhcr zhcr;
	public Transform mainCar;

	private void Update()
	{
		if (gameObject.activeSelf)
		{
            mainCar.position = transform.position;
            zhcr.transform.localScale = Vector3.one;
            zhcr.ChangeStatue(Zhcr_Sta.Spawn);
			this.enabled = false;
        }
		
	}
}
