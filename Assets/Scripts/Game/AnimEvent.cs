using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{

    public bool event_IdlePlayEnd;
    public bool event_Setp;
    public bool event_FallDown;
    public bool event_AttackEffect;
    public bool event_AttackPlayEnd;
    public bool event_DamagePlayEnd;
    public bool event_DiePlayEnd;
    public bool event_JumpStart;
    public bool event_JumpTop;
    public bool event_JumpDown;

    [HideInInspector]
    public int firePosId = 0;
    public void Clear()
    {
        event_IdlePlayEnd = false;
        event_Setp = false;
        event_FallDown = false;
        event_AttackEffect = false;
        event_AttackPlayEnd = false;
        event_DamagePlayEnd = false;
        event_DiePlayEnd = false;
    }

    public void Event_IdlePlayEnd()
    {
        event_IdlePlayEnd = true;
    }
    public void Event_Setp()
    {
        event_Setp = true;
    }
    public void Event_FallDown()
    {
        event_FallDown = true;
    }
    //
    //public void Event_AttackEffect() {
    //    event_AttackEffect = true;
    //}//
    public void Event_AttackEffect(int fireposid)
    {
        event_AttackEffect = true;
        firePosId = fireposid;
    }//
    public void Event_AttackPlayEnd()
    {
        event_AttackPlayEnd = true;
    }
    public void Event_DamagePlayEnd()
    {
        event_DamagePlayEnd = true;
    }
    //
    public void Event_DiePlayEnd()
    {
        event_DiePlayEnd = true;
    }

    public void Event_JumpStart()
    {
        event_JumpStart = true;
    }
    public void Event_JumpTop()
    {
        event_JumpTop = true;
    }
    public void Event_JumpDown()
    {
        event_JumpDown = true;
    }

    private BossBase bossBase;

    private void OnEnable()
    {
        bossBase = GetComponentInParent<BossBase>();
    }

    public void Event_Die()
    {
        if (bossBase != null)
        {
            DieEvent.Trigger(transform.GetComponent<Animator>());
        }
        else
        {
            Debug.Log("Boss为空");
        }
    }
}

#region BOSS死亡处理
public delegate void BossDie(Animator aniamtor);

public class DieEvent
{
    private static event BossDie die;

    public static void Register(BossDie bossDie)
    {
        die += bossDie;
    }

    public static void Trigger(Animator aniamtor)
    {
        if (die != null)
        {
            die(aniamtor);
        }
        else
        {
            Debug.Log("委托为空");
        }
    }

    public static void UnRegister(BossDie bossDie)
    {
        die -= bossDie;
    }

    public static void Clear()
    {
        die = null;
    }
}
#endregion
