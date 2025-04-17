using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Zhcr_Sta
{
	Idle,
	Run,
	Spawn,
	Attack,
	Die
}

public class Zhcr : MonoBehaviour {
	public Animator animator;
	public Zhcr_Sta statue;
	
	public void ChangeStatue(Zhcr_Sta sta)
	{
		if (sta == statue)
		{
			return;
		} 

		switch(sta)
		{
			case Zhcr_Sta.Idle:
				statue = sta;
				animator.Play("Idle");
				break;
			case Zhcr_Sta.Run:
                statue = sta;
                animator.Play("Run");
                break;
			case Zhcr_Sta.Attack:
                statue = sta;
                animator.Play("Attack");
                break;
			case Zhcr_Sta.Die:
                statue = sta;
                animator.Play("Die");
                break;
            case Zhcr_Sta.Spawn:
                statue = sta;
                animator.Play("Spawn");
                break;
        }
	}
	
}
