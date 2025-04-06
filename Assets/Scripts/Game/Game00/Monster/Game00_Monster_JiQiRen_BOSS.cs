using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Game00_Monster_JiQiRen_BOSS : MonoBehaviour
{
    const float MOVE_DRAG_TIME = 0.9f;

    public NavMeshAgent navMeshAgent;
    public NavMeshObstacle navMeshObstacle;
    public Game00_AttackSkill[] attackSkill;
    public AudioSource audioSource_Step;
    public AudioSource audioSource_JumpDown;


    Game00_Main gameMain;
    Game00_Monster monster;
    GameObject playersObj;
    en_MonsterSta monsterStatue;

    float runTime;
    float attackDcTime;
    float navTime;
    public int runStatue;
    int attackCnt;
    int attackSkillId;

    Vector3 currMoveTargetPos;
    Vector3 targetPos;
    float moveDragTime = 0;
    float offsetDistance;
    float distance;
    public float angle;

    // Jump:
    public int jumpSta;
    float jumpStartY;
    float jumpSpeedX;
    float jumpSpeedZ;

    //
    public int runIndex;
    //

    // Use this for initialization
    void Start()
    {
        monster = GetComponent<Game00_Monster>();
        gameMain = monster.gameMain;
        playersObj = gameMain.players_Obj;

        navTime = 0.3f;
        navMeshAgent.enabled = false;
        navMeshObstacle.enabled = false;
        monsterStatue = monster.statue;
        runIndex = -1;
        for (int i = 0; i < attackSkill.Length; i++)
        {
            attackSkill[i].Init(monster);
        }
    }

    void Update()
    {
        if (monster == null)
            return;

        // 攻击冷却时间计数
        if (attackDcTime > 0)
        {
            attackDcTime -= Time.deltaTime;
        }

        if (navTime > 0)
        {
            navTime -= Time.deltaTime;
        }

        if (monsterStatue != monster.statue)
        {
            monsterStatue = monster.statue;
            switch (monsterStatue)
            {
                case en_MonsterSta.Run:

                    break;
                case en_MonsterSta.Die:
                    navMeshAgent.enabled = false;
                    navMeshObstacle.enabled = false;
                    break;
                case en_MonsterSta.DownCar:

                    break;
            }
        }

        MoveDragCheck();

        if (monster.animEvent.event_Setp)
        {
            monster.animEvent.event_Setp = false;
            audioSource_Step.Stop();
            audioSource_Step.Play();
            //
            gameMain.ShakeStart(0.3f, 4);
        }

        switch (monster.statue)
        {
            case en_MonsterSta.Idle:

                ChangeStatue(en_MonsterSta.Walk);
                break;

            case en_MonsterSta.OnCar:
                if (monster.car == null)
                {
                    ChangeStatue(en_MonsterSta.Run);
                    break;
                }
                break;

            case en_MonsterSta.DownCar:
                break;

            case en_MonsterSta.Jump:
                runTime += Time.deltaTime;
                if (monster.animEvent.event_JumpStart)
                {
                    monster.animEvent.event_JumpStart = false;
                    jumpSta = 1;
                    jumpStartY = transform.position.y;
                    runTime = 0;
                }
                if (monster.animEvent.event_JumpTop)
                {
                    monster.animEvent.event_JumpTop = false;
                    jumpSta = 2;
                }
                if (monster.animEvent.event_JumpDown)
                {
                    monster.animEvent.event_JumpDown = false;
                }

                switch (jumpSta)
                {
                    case 1:
                        transform.position += new Vector3(jumpSpeedX, 10, jumpSpeedZ) * Time.deltaTime;
                        break;
                    case 2:
                        transform.position += new Vector3(jumpSpeedX, -10, jumpSpeedZ) * Time.deltaTime;
                        if (transform.position.y <= jumpStartY)
                        {
                            jumpSta = 3;
                            transform.position = new Vector3(transform.position.x, jumpStartY, transform.position.z);
                            gameMain.ShakeStart(0.6f, 5);
                            audioSource_JumpDown.Stop();
                            audioSource_JumpDown.Play();
                        }
                        break;
                    case 3:
                        if (monster.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                        {
                            ChangeStatue(en_MonsterSta.Run);
                        }
                        break;
                }
                break;

            case en_MonsterSta.Walk:
                RunCheck(currMoveTargetPos);
                if (monster.isAttack)
                {
                    if (playersObj == null)
                    {
                        // 自动攻击巡逻
                        if (Vector3.Distance(transform.position, monster.mainCar_Obj.transform.position) < monster.maxAttackDistance)
                        {
                            monster.CheckFindEnemy();
                        }
                    }
                    else
                    {
                        ChangeStatue(en_MonsterSta.Run);
                    }
                }
                break;

            case en_MonsterSta.Run:
                // 行动受阻?
                if (moveDragTime >= MOVE_DRAG_TIME)
                {
                    moveDragTime = 0;
                    ChangeStatue(en_MonsterSta.Jump);
                    break;
                }
                // 没有固定点：            
                if (runStatue == 0)
                {
                    targetPos = gameMain.navMeshAgent_MainCar.transform.position;
                    if (Vector3.Distance(transform.position, targetPos) < 8)
                    {
                        UpdateNewTargetPos(false);
                        runStatue = 1;
                        break;
                    }
                    RunCheck(targetPos);
                }
                else if (runStatue == 1)
                {
                    if (runIndex == 0)
                    {
                        RunCheck(currMoveTargetPos);
                        break;
                    }
                    else
                    {
                        if (runIndex != 1)
                            runIndex = Random.Range(0, 2);
                        if (runIndex == 0 || monster.LookedPlayers() == false)
                        {
                            RunCheck(currMoveTargetPos);
                            break;
                        }
                        else if (runIndex == 1)
                        {
                            runStatue = 2;
                            break;
                        }
                    }


                    angle = Vector3.Angle(transform.position - playersObj.transform.position, playersObj.transform.forward);

                    // 是否看得到玩家
                    if (monster.LookedPlayers() == false)
                    {
                        runTime = 0.3f;
                        break;
                    }
                    if (runTime > 0)
                    {
                        runTime -= Time.deltaTime;
                        break;
                    }
                    if (angle > 90)
                    {
                        ChangeStatue(en_MonsterSta.Jump);
                        break;
                    }
                    if (angle < 30)
                    {
                        runStatue = 2;
                    }
                }
                else if (runStatue == 2)
                {
                    targetPos = playersObj.transform.position + playersObj.transform.forward * 1.5f;
                    targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                    RunCheck(targetPos);
                    distance = Vector3.Distance(transform.position, targetPos);

                    if (distance < monster.minAttackDistance)
                    {
                        ChangeStatue(en_MonsterSta.Jump);
                    }
                    else if (distance < monster.maxAttackDistance)
                    {
                        runStatue = 3;
                    }
                }
                else
                {
                    // 停
                    StopCheck();
                    // 转向
                    targetPos = playersObj.transform.position + playersObj.transform.forward * 1.5f;
                    targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position, Vector3.up), 20 * Time.deltaTime);

                    if (Vector3.Angle(targetPos - transform.position, transform.forward) < 3)
                    {
                        ChangeStatue(en_MonsterSta.Attack);
                    }
                }
                break;

            case en_MonsterSta.Attack:
                // 停
                StopCheck();
                //
                if (attackSkill[attackSkillId].IsPlayEnd())
                {
                    // 攻击结束:
                    attackCnt++;
                    if (attackCnt >= Random.Range(1, 4))
                    {
                        ChangeStatue(en_MonsterSta.Jump);
                        break;
                    }
                    UpdateAttackSkillInfo();
                    // 随机点攻击：
                    distance = Vector3.Distance(transform.position, gameMain.transform.position);
                    angle = Vector3.Angle(gameMain.players_Obj.transform.forward, transform.position - gameMain.players_Obj.transform.position);
                    if (distance < monster.minAttackDistance || angle > 60)
                    {
                        ChangeStatue(en_MonsterSta.Jump);
                    }
                    else if (distance > monster.maxAttackDistance || angle > 35 || monster.LookedPlayers() == false)
                    {
                        ChangeStatue(en_MonsterSta.Run);
                    }
                    else
                    {
                        ChangeStatue(en_MonsterSta.Attack);
                    }
                }
                break;

            case en_MonsterSta.Hide:
                if (Vector3.Distance(transform.position, monster.hidePos.transform.position) > 0.2f)
                {
                    RunCheck(monster.hidePos.transform.position);
                }
                else
                {
                    runTime += Time.deltaTime;
                    if (runTime >= 2)
                    {
                        ChangeStatue(en_MonsterSta.Run);
                    }
                }
                break;

            case en_MonsterSta.RunOut:
                RunCheck(currMoveTargetPos);
                break;
            case en_MonsterSta.Dmage:
                // 停
                StopCheck();
                break;
            case en_MonsterSta.Die:
                // 停
                StopCheck();
                break;
        }
    }

    void ChangeStatue(en_MonsterSta sta)
    {
        //   print("MStaue: " + sta);

        monsterStatue = sta;
        runTime = 0;
        if (monster.animator.enabled == false)
        {
            monster.animator.enabled = true;
        }

        switch (sta)
        {
            case en_MonsterSta.Idle:
                break;
            case en_MonsterSta.Jump:
                navMeshAgent.enabled = false;
                navMeshObstacle.enabled = false;
                monster.animator.Play("Jump");
                jumpSta = 0;

                targetPos = gameMain.navMeshAgent_MainCar.transform.position + playersObj.transform.forward * Random.Range(5, 8f) + playersObj.transform.right * Random.Range(-2f, 2f);// -8  8
                transform.LookAt(targetPos);
                jumpSpeedX = (targetPos.x - transform.position.x) / 0.5f;
                jumpSpeedZ = (targetPos.z - transform.position.z) / 0.5f;
                break;
            case en_MonsterSta.Walk:
                monster.animator.Play("Run");
                attackCnt = 0;
                break;
            case en_MonsterSta.Run:
                runIndex = -1;
                runStatue = 0;
                UpdateAttackSkillInfo();
                UpdateNewTargetPos(false);
                monster.animator.Play("Run");
                break;
            case en_MonsterSta.RunOut:
                monster.animator.Play("Run");
                playersObj = null;
                break;
            case en_MonsterSta.Attack:
                attackSkill[attackSkillId].Attack();
                break;
            case en_MonsterSta.Die:
                break;
        }
        monster.ChangeStatueWithoutAnim(sta);
    }

    void UpdateAttackSkillInfo()
    {
        attackSkillId = Random.Range(0, attackSkill.Length);
        monster.minAttackDistance = attackSkill[attackSkillId].minAttackDistance;
        monster.maxAttackDistance = attackSkill[attackSkillId].maxAttackDistance;
        monster.readyAttackDistance = attackSkill[attackSkillId].readyAttackDistance;
    }

    void UpdateNewTargetPos(bool drag)
    {

        if (drag)
        {
            offsetDistance = Random.Range(-5f, 5f);
        }
        else if (Vector3.Distance(transform.position, gameMain.navMeshAgent_MainCar.transform.position) < monster.minAttackDistance)
        {
            offsetDistance = 0;
        }
        else if (Vector3.Cross(playersObj.transform.forward, transform.position - gameMain.navMeshAgent_MainCar.transform.position).y > 0)
        {
            // 右边
            offsetDistance = -4.5f;
        }
        else
        {
            // 左边
            offsetDistance = 4.5f;
        }
        int temp = Random.Range(4, 7);
        for (int i = temp; i > 3; i--)
        {
            currMoveTargetPos = playersObj.transform.position + playersObj.transform.forward * i + playersObj.transform.right * offsetDistance;

            currMoveTargetPos = new Vector3(currMoveTargetPos.x, gameMain.navMeshAgent_MainCar.transform.position.y, currMoveTargetPos.z);
            if (navMeshAgent.enabled == false)
                navMeshAgent.enabled = true;
            if (navMeshAgent.SetDestination(currMoveTargetPos))
            {
                break;
            }
        }
    }

    void RunCheck(Vector3 pos)
    {
        if (navMeshObstacle.enabled)
        {
            navMeshObstacle.enabled = false;
            navTime = 0.2f;
        }
        else if (navTime <= 0)
        {
            if (navMeshAgent.enabled == false)
            {
                navMeshAgent.enabled = true;
            }
            navMeshAgent.SetDestination(pos);
        }
    }
    void StopCheck()
    {
        if (navMeshAgent.enabled)
        {
            navMeshAgent.SetDestination(transform.position);
            navMeshAgent.enabled = false;
            navTime = 0.3f;
        }
        else if (navTime <= 0)
        {
            if (navMeshObstacle.enabled == false)
            {
                navMeshObstacle.enabled = true;
            }
        }
    }

    void MoveDragCheck()
    {
        if (navMeshAgent.enabled == false)
            return;
        // 怪行动受阻检测
        if (Vector3.Magnitude(navMeshAgent.velocity) * 5 < navMeshAgent.speed)
        {
            if (moveDragTime < MOVE_DRAG_TIME)
            {
                moveDragTime += Time.deltaTime;
            }
        }
        else
        {
            moveDragTime = 0;
        }
    }
}
