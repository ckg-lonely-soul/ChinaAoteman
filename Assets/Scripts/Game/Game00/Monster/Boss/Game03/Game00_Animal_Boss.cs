using UnityEngine;
using UnityEngine.AI;
public class Game00_Animal_Boss : MonoBehaviour
{

    const float MOVE_DRAG_TIME = 0.1f;

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
    public int nextIndex;
    public float initSpeed;
    public float quickSpeed;
    public float startQuickDis;
    public bool isStop;
    public int attackState;
    //
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
        nextIndex = -1;
        attackState = -1;
        initSpeed = navMeshAgent.speed;
        quickSpeed = initSpeed * 2f;
        startQuickDis = 8.5f;
        isStop = false;
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
        //  AnimatorStateInfo animatorInfo;
        switch (monster.statue)
        {
            case en_MonsterSta.Idle:

                ChangeStatue(en_MonsterSta.Walk);
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
                    UpdateNewTargetPos(false);
                    break;
                }
                // 没有固定点：            
                if (runStatue == 0)
                {
                    targetPos = gameMain.navMeshAgent_MainCar.transform.position;
                    if (Vector3.Distance(transform.position, targetPos) < 12)
                    {
                        UpdateNewTargetPos(false);
                        runStatue = 1;
                        break;
                    }
                }
                else if (runStatue == 1)
                {
                    //移向当前的目标点
                    if (runIndex == 0)
                    {
                        RunCheck(currMoveTargetPos);
                        if (Vector3.Distance(transform.position, currMoveTargetPos) < startQuickDis)
                        {
                            //远攻
                            if (attackState == 1)
                            {
                                if (Vector3.Distance(transform.position, currMoveTargetPos) < monster.minAttackDistance)
                                {

                                    StopCheck();
                                    //  transform.LookAt(playersObj.transform.position);
                                    transform.rotation = Quaternion.Slerp(transform.rotation,
               Quaternion.LookRotation(playersObj.transform.position - transform.position, Vector3.up), 2 * Time.deltaTime);
                                    if (Vector3.Angle(playersObj.transform.position - transform.position, transform.forward) < 30)
                                    {
                                        attackSkillId = 0;
                                        attackState = -1;
                                        ChangeStatue(en_MonsterSta.Attack);
                                    }

                                    break;
                                }
                                break;
                            }
                            else
                            {
                                if (attackState != 0)
                                    attackState = Random.Range(0, 2);
                                if (attackState == 1)
                                {
                                    if (Vector3.Distance(transform.position, currMoveTargetPos) < monster.minAttackDistance)
                                    {
                                        // attackSkillId = 0;
                                        //  attackState = -1;
                                        StopCheck();
                                        transform.rotation = Quaternion.Slerp(transform.rotation,
          Quaternion.LookRotation(playersObj.transform.position - transform.position, Vector3.up), 2 * Time.deltaTime);
                                        if (Vector3.Angle(playersObj.transform.position - transform.position, transform.forward) < 30)
                                        {
                                            attackSkillId = 0;
                                            attackState = -1;
                                            ChangeStatue(en_MonsterSta.Attack);
                                        }
                                        break;
                                    }
                                    break;
                                }
                                else
                                {
                                    nextIndex = 1;
                                    ChangeStatue(en_MonsterSta.Quicken);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                    else
                    {
                        if (runIndex != 1)
                            runIndex = Random.Range(0, 2);
                        if (runIndex == 0)
                        {
                            RunCheck(currMoveTargetPos);
                            if (Vector3.Distance(transform.position, currMoveTargetPos) < startQuickDis)
                            {
                                if (attackState == 1)
                                {
                                    if (Vector3.Distance(transform.position, currMoveTargetPos) < monster.minAttackDistance)
                                    {
                                        //attackSkillId = 0;
                                        //attackState = -1;
                                        StopCheck();
                                        transform.rotation = Quaternion.Slerp(transform.rotation,
      Quaternion.LookRotation(playersObj.transform.position - transform.position, Vector3.up), 2 * Time.deltaTime);
                                        if (Vector3.Angle(playersObj.transform.position - transform.position, transform.forward) < 30)
                                        {
                                            attackSkillId = 0;
                                            attackState = -1;
                                            ChangeStatue(en_MonsterSta.Attack);
                                        }
                                        break;
                                    }
                                    break;
                                }
                                else
                                {
                                    if (attackState != 0)
                                        attackState = Random.Range(0, 2);
                                    if (attackState == 1)
                                    {
                                        if (Vector3.Distance(transform.position, currMoveTargetPos) < monster.minAttackDistance)
                                        {
                                            //  attackSkillId = 0;
                                            // attackState = -1;
                                            StopCheck();
                                            //transform.LookAt(playersObj.transform.position);
                                            transform.rotation = Quaternion.Slerp(transform.rotation,
               Quaternion.LookRotation(playersObj.transform.position - transform.position, Vector3.up), 2 * Time.deltaTime);
                                            if (Vector3.Angle(playersObj.transform.position - transform.position, transform.forward) < 30)
                                            {
                                                attackSkillId = 0;
                                                attackState = -1;
                                                ChangeStatue(en_MonsterSta.Attack);
                                            }
                                            break;
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        nextIndex = 1;
                                        ChangeStatue(en_MonsterSta.Quicken);
                                    }
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    runStatue = 2;
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

                }
                else if (runStatue == 2)
                {
                    targetPos = playersObj.transform.position;
                    targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                    RunCheck(targetPos);
                    distance = Vector3.Distance(transform.position, targetPos);
                    if (distance < startQuickDis)
                    {
                        runIndex = -1;
                        nextIndex = 0;
                        ChangeStatue(en_MonsterSta.Quicken);
                        break;
                    }
                }
                //else
                //{
                //    // 停
                //    RunCheck(playersObj.transform.position);
                //    // 转向
                //    attackSkillId = 1;
                //    ChangeStatue(en_MonsterSta.Attack);
                //}
                break;
            case en_MonsterSta.Quicken:

                if (nextIndex == 0)
                {
                    RunCheck(playersObj.transform.position + (playersObj.transform.forward * 2f));
                    if (Vector3.Distance(transform.position, playersObj.transform.position + (playersObj.transform.forward * 2f)) < 2.5f)
                    {

                        attackSkillId = 1;
                        nextIndex = -1;
                        ChangeStatue(en_MonsterSta.Attack);
                        break;
                    }
                }
                else if (nextIndex == 1)
                {
                    RunCheck(currMoveTargetPos);
                    if (Vector3.Distance(transform.position, currMoveTargetPos) < monster.minAttackDistance)
                    {
                        int temp = Random.Range(0, 2);
                        if (temp == 0)
                        {
                            StopCheck();
                            attackSkillId = 0;
                            transform.rotation = Quaternion.Slerp(transform.rotation,
                                Quaternion.LookRotation(playersObj.transform.position - transform.position, Vector3.up), 2 * Time.deltaTime);
                            if (Vector3.Angle(playersObj.transform.position - transform.position, transform.forward) < 30)
                            {
                                nextIndex = -1;
                                ChangeStatue(en_MonsterSta.Attack);
                                break;
                            }
                        }
                        else
                        {
                            ChangeStatue(en_MonsterSta.Run);
                            runIndex = -1;
                            nextIndex = -1;
                        }

                        //else
                        //{
                        //    attackSkillId = 0;
                        //    transform.rotation = Quaternion.Slerp(transform.rotation, 
                        //        Quaternion.LookRotation(playersObj.transform.position - transform.position, Vector3.up), 20 * Time.deltaTime);
                        //    if (Vector3.Angle(playersObj.transform.position - transform.position, transform.forward) < 3)
                        //    {
                        //        nextIndex = -1;
                        //        ChangeStatue(en_MonsterSta.Attack);
                        //    }
                        //}
                        break;
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
                    nextIndex = 1;
                    // transform.LookAt(currMoveTargetPos);
                    attackCnt++;
                    UpdateNewTargetPos(false);
                    ChangeStatue(en_MonsterSta.Quicken);
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

                jumpSpeedX = (targetPos.x - transform.position.x) / 0.5f;
                jumpSpeedZ = (targetPos.z - transform.position.z) / 0.5f;
                break;
            case en_MonsterSta.Walk:
                monster.animator.Play("Run");
                attackCnt = 0;
                break;
            case en_MonsterSta.Run:
                navMeshAgent.speed = initSpeed;
                runStatue = 0;
                //UpdateAttackSkillInfo();
                UpdateNewTargetPos(false);
                monster.animator.Play("Run");
                break;
            case en_MonsterSta.RunOut:
                monster.animator.Play("Run");
                playersObj = null;
                break;
            case en_MonsterSta.Attack:
                attackState = -1;
                transform.LookAt(playersObj.transform);
                attackSkill[attackSkillId].Attack();
                break;
            case en_MonsterSta.Die:
                break;
            case en_MonsterSta.Quicken:
                //isStop = false;
                navMeshAgent.speed = quickSpeed;
                monster.animator.Play("Quicken");
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
            offsetDistance = Random.Range(-6f, 6f);
        }
        else if (Vector3.Distance(transform.position, gameMain.navMeshAgent_MainCar.transform.position) < monster.minAttackDistance)
        {
            offsetDistance = 0;
        }
        else if (Vector3.Cross(playersObj.transform.forward, transform.position - gameMain.navMeshAgent_MainCar.transform.position).y > 0)
        {
            // 右边
            offsetDistance = -6f;
        }
        else
        {
            // 左边
            offsetDistance = 6f;
        }

        int temp = Random.Range(8, 13);
        for (int i = temp; i > 3; i--)
        {
            currMoveTargetPos = playersObj.transform.position + playersObj.transform.forward * i + playersObj.transform.right * offsetDistance;

            currMoveTargetPos = new Vector3(currMoveTargetPos.x, gameMain.navMeshAgent_MainCar.transform.position.y, currMoveTargetPos.z);
            if (navMeshAgent.enabled == false)
                navMeshAgent.enabled = true;
            if (navMeshAgent.SetDestination(currMoveTargetPos))
            {
                //navMeshAgent.SetDestination(currMoveTargetPos);
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
