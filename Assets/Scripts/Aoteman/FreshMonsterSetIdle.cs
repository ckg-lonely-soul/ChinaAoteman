using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 让怪物一开始Idle状态，可以开和关闭该功能
/// </summary>
public class FreshMonsterSetIdle : MonoBehaviour
{
    public Game00_FreshPos freshPos;
    //多少秒之后不再Idle状态
    public float delayToNotIdle = 1.5f;
    public bool notIdleTrigger;
    private bool idleTriger = false;
    private bool allDone = false;
    //public bool canMove = false;
    //public Vector3 moveTarget;
    //private Vector3 startPos;
    //private float totalTime;

    private void Start()
    {
        freshPos = GetComponent<Game00_FreshPos>();
        //startPos = transform.position;
        //totalTime = delayToNotIdle;
    }

    private void Update()
    {
        if (allDone)
            return;

        //看向玩家
        if (freshPos.currMonster != null)
            freshPos.currMonster.transform.LookAt(freshPos.currMonster.GetPlayersObj().transform);


        if (!idleTriger && freshPos.currMonster != null)
        {
            idleTriger = true;
            //让怪物
            freshPos.currMonster.ChangeStatue(en_MonsterSta.Idle);
            freshPos.currMonster.animator.Play("Idle");
            freshPos.currMonster.GetComponent<Game00_Monster_JiQiRen>().enabled = false;
            freshPos.currMonster.GetComponent<NavMeshAgent>().enabled = false;
            //startPos = freshPos.currMonster.transform.position;
        }
        //已经开始IDLE
        else if (idleTriger && !notIdleTrigger)
        {
            freshPos.currMonster.GetComponent<Game00_Monster_JiQiRen>().enabled = false;
            freshPos.currMonster.GetComponent<NavMeshAgent>().enabled = false;
            delayToNotIdle -= Time.deltaTime;

            if (delayToNotIdle < 0)
            {
                notIdleTrigger = true;
            }

            //if (canMove)
            //{
            //    freshPos.currMonster.transform.position = Vector3.Lerp(startPos, moveTarget,
            //        (Time.deltaTime) / totalTime);
            //}
        }
        else if (notIdleTrigger && freshPos.currMonster != null)
        {
            try
            {
                freshPos.currMonster.GetComponent<Game00_Monster_JiQiRen>().enabled = true;
                freshPos.currMonster.GetComponent<NavMeshAgent>().enabled = true;
            }
            catch (NullReferenceException)
            {
                Debug.Log("有空值引用错误！");
                return;
            }

            allDone = true;
        }
    }

    private void OnDestroy()
    {
        notIdleTrigger = false;
    }
}
