using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum CameraPos
{
	ThridPerson,    //第三人称
	FirstPerson,    //第一人称
	BossSpawn,      //BOSS登场
	ZhcrSpawn,   //登场
}

public class CameraChange : MonoBehaviour {

	public Transform thridPerson;
    public Transform firstPerson;
    public Transform bossSpawn;
    public Transform zhcrSpawn;

	public CameraPos currPos;

    public void ChangeCameraPos(CameraPos pos)
	{
		
		switch(pos)
		{
			case CameraPos.ThridPerson:
				transform.parent = thridPerson;
				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;
				currPos = pos;
				break;
			case CameraPos.FirstPerson:
                transform.parent = firstPerson;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                currPos = pos;
                break;
			case CameraPos.BossSpawn:
                transform.parent = bossSpawn;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                currPos = pos;
                break;
			case CameraPos.ZhcrSpawn:
                transform.parent = zhcrSpawn;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                currPos = pos;
                break;
		}
	}
}
