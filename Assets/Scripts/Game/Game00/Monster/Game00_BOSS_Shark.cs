using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game00_BOSS_Shark : MonoBehaviour
{
    public GameObject dieBombEff_Prefab;
    public GameObject dieEffPos_Obj;
    public float walkSpeed = 3.5f;
    public float runSpeed = 8.5f;
    public float shakePower = 0.2f;
    public int shakeCnt = 4;

    Game00_Main gameMain;
    Game00_Monster monster;
    GameObject playersObj;

    Transform[] dieEff_Pos;

    Vector3 targetPos;
    Vector3 vecter_OutTarget;
    float distance;
    float angle;
    float rotateSpeed;
    float targetDistance;
    int walkStatue;
    int runStatue;
    int outRunDir = 1;
    float runTime;
    float attackDcTime = 0;
    float dieEffDcTime = 0;

    // Use this for initialization
    void Start()
    {
        dieEff_Pos = dieEffPos_Obj.GetComponentsInChildren<Transform>();

        monster = GetComponent<Game00_Monster>();
        gameMain = monster.gameMain;
        playersObj = gameMain.players_Obj;
        walkStatue = 0;
        targetDistance = monster.maxAttackDistance * Random.Range(1.2f, 3.3f);
        ChangeStatue(en_MonsterSta.Walk);
    }

    // Update is called once per frame
    void Update()
    {
        if (monster == null)
            return;

        if (attackDcTime > 0)
        {
            attackDcTime -= Time.deltaTime;
        }

        switch (monster.statue)
        {
            case en_MonsterSta.Idle:            // 停下等待
                break;

            case en_MonsterSta.Walk:            // 慢走
                // 移动
                transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
                // 转向
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position), rotateSpeed * Time.deltaTime);
                //
                monster.CheckFindEnemy();
                if (attackDcTime <= 0)
                { //&& playersObj != null
                    runStatue = 0;
                    ChangeStatue(en_MonsterSta.Run);
                    break;
                }
                distance = Vector3.Distance(transform.position, targetPos);
                if (distance < 1)
                {
                    ChangeStatue(en_MonsterSta.RunOut);
                }
                break;

            case en_MonsterSta.Run:             // 攻击中快走
                distance = Vector3.Distance(transform.position, playersObj.transform.position);
                angle = Vector3.Angle(transform.position - playersObj.transform.position, playersObj.transform.forward);
                //
                if (runStatue == 0)
                {
                    // 进入攻击范围
                    targetPos = playersObj.transform.position + playersObj.transform.forward * monster.maxAttackDistance * 2;
                    rotateSpeed = 5;
                    if (angle <= 20 && distance >= monster.maxAttackDistance)
                    {
                        runStatue = 1;
                    }
                }
                else if (runStatue == 1)
                {
                    // 冲向目标
                    targetPos = playersObj.transform.position;
                    rotateSpeed = 10;
                    if (angle > 45)
                    {
                        runStatue = 2;      // 放弃攻击，游向一边
                    }
                    else if (distance <= monster.minAttackDistance + 1.8f)
                    {
                        ChangeStatue(en_MonsterSta.Attack);
                        break;
                    }
                }
                else
                {
                    // 放弃
                    ChangeStatue(en_MonsterSta.RunOut);
                }
                // 移动
                transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
                // 转向
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position), rotateSpeed * Time.deltaTime);
                break;

            case en_MonsterSta.RunOut:
                // 移动
                transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
                // 转向
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position), rotateSpeed * Time.deltaTime);
                //
                distance = Vector3.Distance(transform.position, targetPos);
                runTime += Time.deltaTime;
                if (runTime >= 10f || distance < 0.5f)
                {
                    ChangeStatue(en_MonsterSta.Walk);
                }
                break;

            case en_MonsterSta.Attack:          // 攻击
                if (runTime == 0)
                {
                    if (monster.effect_AttackScreen_Prefab != null)
                    {
                        GameObject obj = GameObject.Instantiate(monster.effect_AttackScreen_Prefab, gameMain.gameUI.transform);
                        Vector3 screenPos = Camera.main.WorldToScreenPoint(monster.firePos_Obj.transform.position);
                        obj.transform.position = new Vector3(screenPos.x, screenPos.y, 0);
                    }
                    if (shakePower > 0)
                    {
                        gameMain.ShakeStart(shakePower, shakeCnt);
                    }
                    gameMain.AttackPlayers(monster, monster.attackValue);
                    gameMain.RunStop(0.7f);
                }
                runTime += Time.deltaTime;
                if (runTime >= 0.5f)
                {
                    runTime = 0;
                    ChangeStatue(en_MonsterSta.RunOut);
                }
                break;

            case en_MonsterSta.Die:
                if (runTime >= 8)
                    break;
                runTime += Time.deltaTime;

                if (dieEffDcTime > 0)
                {
                    dieEffDcTime -= Time.deltaTime;
                }
                else
                {
                    dieEffDcTime = Random.Range(0.1f, 0.9f);
                    if (dieBombEff_Prefab != null)
                    {
                        int posId = Random.Range(0, dieEff_Pos.Length);
                        GameObject obj = GameObject.Instantiate(dieBombEff_Prefab, transform);
                        obj.transform.position = dieEff_Pos[posId].position;
                        obj.transform.eulerAngles = transform.eulerAngles;
                        //
                        monster.gameMain.ShakeStart(0.2f, 4);
                    }
                }
                break;
        }
    }

    void ChangeStatue(en_MonsterSta sta)
    {
        runTime = 0;
        switch (sta)
        {
            case en_MonsterSta.Attack:
                break;
            case en_MonsterSta.RunOut:
                if (Random.Range(0, 1000) < 500)
                {
                    outRunDir = 1;
                }
                else
                {
                    outRunDir = -1;
                }
                Vector3 forward = new Vector3(playersObj.transform.forward.x, 0, playersObj.transform.forward.z);
                float distanceForward = Random.Range(5f, 10f);
                float distanceRight = Random.Range(40f, 60f);
                float distanceUp = Random.Range(0.5f, 10.0f);
                targetPos = playersObj.transform.position +
                    forward * distanceForward +
                    playersObj.transform.right * distanceRight * outRunDir +
                    playersObj.transform.up * distanceUp;
                break;

            case en_MonsterSta.Walk:
                attackDcTime = Random.Range(2f, 7f);
                forward = new Vector3(playersObj.transform.forward.x, 0, playersObj.transform.forward.z);
                distanceForward = Random.Range(15f, 25f);
                distanceRight = Random.Range(-5f, 5f);
                distanceUp = Random.Range(0.5f, 5.0f);
                targetPos = playersObj.transform.position +
                    forward * distanceForward +
                    playersObj.transform.right * distanceRight * outRunDir +
                    playersObj.transform.up * distanceUp;
                break;
        }
        monster.ChangeStatue(sta);
    }





    //// Use this for initialization
    //void Start() {
    //    monster = GetComponent<Game00_Monster>();
    //    playersObj = monster.GetPlayersObj();
    //    walkStatue = 0;
    //    targetDistance = monster.maxAttackDistance * Random.Range(1.2f, 3.3f);
    //    distance = Vector3.Distance(transform.position, playersObj.transform.position);
    //    vecter_OutTarget = playersObj.transform.forward * distance - (transform.position - playersObj.transform.position);
    //}

    //// Update is called once per frame
    //void Update () {
    //    if (monster == null)
    //        return;

    //    if (monster.statue == en_MonsterSta.Walk) {
    //        angle = Vector3.Angle(transform.position - playersObj.transform.position, playersObj.transform.forward);
    //        distance = Vector3.Distance(transform.position, playersObj.transform.position);

    //        if (walkStatue == 0) {
    //            // 远离视野
    //            if (angle >= 70) {
    //                walkStatue = 1;
    //            }
    //            rotateSpeed = 5;
    //            targetPos = playersObj.transform.position + playersObj.transform.forward * targetDistance + vecter_OutTarget * 50;
    //        } else {
    //            // 冲向中心
    //            if (angle < 30) {
    //                walkStatue = 0;
    //                targetDistance = monster.maxAttackDistance * Random.Range(1.2f, 2.3f);
    //                vecter_OutTarget = playersObj.transform.forward * distance - (transform.position - playersObj.transform.position);
    //            }
    //            rotateSpeed = 5;
    //            targetPos = playersObj.transform.position + playersObj.transform.forward * monster.maxAttackDistance * 3;                
    //        }
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position), rotateSpeed * Time.deltaTime);
    //    } 
    //}
}
