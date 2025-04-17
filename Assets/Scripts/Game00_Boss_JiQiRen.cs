using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Game00_Boss_JiQiRen : BossBase
{

    const float MOVE_DRAG_TIME = 0.9f;
    const float MIN_DISTANCE = 1.5f;

    public float runSpeed = 60;
    public float walkSpeed = 30;

    public Animator animator_LvDai;
    public NavMeshAgent navMeshAgent;
    public NavMeshObstacle navMeshObstacle;
    public Game00_AttackSkill[] attackSkill;
    public Game00_AttackSkill[] specialSkill;

    Game00_Main gameMain;
    Game00_Monster monster;
    GameObject playersObj;
    en_MonsterSta monsterStatue;

    [Header("Boss参数")]
    public Vector3 bossNearAttackPos;
    public float bossNearAttackPosZOffset = 3f;
    /// <summary>
    /// 执行特殊技能的CD时间
    /// </summary>
    public float specialSkillCD = 20f;
    public float specialSkillCDTimer;

    float runTime;
    float attackDcTime;
    float navTime;
    public int runStatue;
    int attackCnt;
    int attackSkillId;
    int specialSkillId;

    float emenyDistance;                    // 敌人距离()
    bool isAttackingPlayer;                 //检测是否正在攻击玩家
    float attackedDistance;					// 被攻击时的距离
    float currAttackDcTime = 0;             // 
    public float distance;
    public float angle;
    float offsetDistance;
    float readyAttackDistance;

    // 走不动检测
    public Vector3 currMoveTargetPos;
    public Vector3 tempMoveTargetPos;
    public Vector3 targetPos;
    float moveDragTime = 0;
    int roadPosIndex;

    public float _distance;

    #region 新增字段、方法
    [Header("跳跃分离路面参数处理")]
    public float jumpTime = 2f / 3;
    public float animeYOffset = 2.8f;
    public Vector3 startOffNavPos;
    public Vector3 endOffNavPos;
    private float progress = 0f;

    #endregion
    void Start()
    {
        monster = GetComponent<Game00_Monster>();
        gameMain = monster.gameMain;
        playersObj = gameMain.players_Obj;

        roadPosIndex = 0;
        navTime = 0.3f;
        specialSkillCDTimer = specialSkillCD;
        navMeshAgent.enabled = false;
        navMeshObstacle.enabled = false;
        UpdateRoadPos();
        monsterStatue = monster.statue;

        for (int i = 0; i < attackSkill.Length; i++)
        {
            attackSkill[i].Init(monster);
        }

        if (specialSkill != null)
        {
            for (int i = 0; i < specialSkill.Length; i++)
            {
                specialSkill[i].Init(monster);
            }
        }

        attackDcTime = monster.attackDcTime;
        isAttackingPlayer = false;
    }
    //void OnEnable() {
    //    ChangeStatue(en_MonsterSta.Attack);
    //}



    void Update()
    {
        if (monster == null)
            return;

        //bossNearAttackPosZOffset是boss离玩家距离
        bossNearAttackPos = gameMain.nearAttackGroup.transform.position
            + gameMain.nearAttackGroup.transform.forward * bossNearAttackPosZOffset;

        if (!monster.isAttack)
            isAttackingPlayer = false;

        //更新攻击CD计时器
        if (attackDcTime < monster.attackDcTime && monster.statue != en_MonsterSta.Attack)
            attackDcTime += Time.deltaTime;


        if (navTime > 0)
        {
            navTime -= Time.deltaTime;
        }

        if (specialSkillCDTimer > 0)
        {
            specialSkillCDTimer -= Time.deltaTime;
        }

        if (monsterStatue != monster.statue)
        {
            monsterStatue = monster.statue;
            switch (monsterStatue)
            {
                case en_MonsterSta.Run:
                    if (animator_LvDai != null)
                    {
                        animator_LvDai.enabled = true;
                    }
                    UpdateAttackSkillInfo();
                    break;
                case en_MonsterSta.Die:
                    navMeshAgent.enabled = false;
                    navMeshObstacle.enabled = false;
                    break;
                case en_MonsterSta.DownCar:
                    UpdateRoadPos();
                    break;
            }
        }
        MoveDragCheck();

        switch (monster.statue)     
        {
            case en_MonsterSta.Idle:
                //加这一大段都是为了怪物在攻击之后不用切换到Run状态，否则Run动画会生效，有一帧的时间
                //让怪物动画变得很突兀。
                if (attackDcTime >= monster.attackDcTime && isAttackingPlayer)
                {
                    #region 攻击之前先检测能否使用特殊攻击，特殊攻击优先使用

                    if (specialSkillCDTimer <= 0 && specialSkill != null)
                    {


                        if (runStatue == 0)
                        {
                            //执行特殊技能
                            UpdateSpecialSkillInfo();
                            //是近战特殊技能
                            if (!specialSkill[specialSkillId].isFarAttack)
                            {
                                targetPos = bossNearAttackPos; //到远处施展技能
                                targetPos = new Vector3(targetPos.x, transform.position.y,
                                    targetPos.z);
                                if (Vector3.Distance(transform.position, targetPos) < 0.15f)
                                {
                                    runStatue = 2;

                                }
                                else
                                {
                                    ChangeStatue(en_MonsterSta.Run);
                                }
                                break;
                            }//远程特殊技能
                            else
                            {
                                targetPos = monster.attackPos.transform.position;
                                //RunCheck(targetPos);
                                if (Vector3.Distance(transform.position, targetPos) < 0.2f)
                                {
                                    runStatue = 2;
                                    break;
                                }
                                else
                                {
                                    ChangeStatue(en_MonsterSta.Run);
                                }
                                break;
                            }

                        }
                    }

                    #endregion


                    // 有预定的固定点:
                    // 没有固定点：            
                    if (runStatue == 0)
                    {
                        // 近程怪
                        if (monster.attackDistanceType == en_MonsterAttackDistanceType.Near)
                        {
                            if (monster.attackPos == null)
                            {
                                ChangeStatue(en_MonsterSta.RunOut); isAttackingPlayer = false;
                                break;
                            }
                            targetPos = bossNearAttackPos;
                            targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                            //RunCheck(targetPos);
                            _distance = Vector3.Distance(transform.position, targetPos);
                            if (Vector3.Distance(transform.position, targetPos) < 0.15f)
                            {
                                runStatue = 2;
                                break;
                            }
                            else
                            {
                                ChangeStatue(en_MonsterSta.Run);
                            }
                            break;
                        }
                        // 有固定攻击点的(非近程怪，可能远近程都有)
                        if (monster.attackPos != null)
                        {
                            targetPos = monster.attackPos.transform.position;
                            //RunCheck(targetPos);
                            //Debug.Log(gameObject.name + Vector3.Distance(transform.position, targetPos));
                            if (Vector3.Distance(transform.position, targetPos) < 0.2f)
                            {
                                runStatue = 2;
                                break;
                            }
                            else
                            {
                                ChangeStatue(en_MonsterSta.Run);
                            }
                            break;
                        }
                        // 远程怪:
                        // 进入视野：于小30度
                        //RunCheck(tempMoveTargetPos);
                        //Debug.Log(tempMoveTargetPos);
                        //
                        angle = Vector3.Angle(playersObj.transform.forward, transform.position - gameMain.navMeshAgent_MainCar.transform.position);
                        if (Vector3.Distance(transform.position, tempMoveTargetPos) < 1.5f || angle > 45)
                        {
                            UpdateNewTargetPos(false);
                        }
                        //// 行动受阻?
                        if (moveDragTime >= MOVE_DRAG_TIME)
                        {
                            moveDragTime = 0;
                            UpdateNewTargetPos(true);
                        }

                        // 是否看得到玩家
                        if (monster.LookedPlayers() == false)
                        {
                            runTime = 0.45f;
                            break;
                        }
                        if (runTime > 0)
                        {
                            runTime -= Time.deltaTime;
                            break;
                        }
                        distance = Vector3.Distance(transform.position, playersObj.transform.position);
                        if (angle < 30 && distance >= monster.minAttackDistance + MIN_DISTANCE)
                        {
                            runStatue = 1;
                        }
                    }
                    //调整位置
                    else if (runStatue == 1)
                    {
                        targetPos = playersObj.transform.position;
                        targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                        //RunCheck(targetPos);
                        distance = Vector3.Distance(transform.position, targetPos);
                        if (distance >= monster.minAttackDistance + MIN_DISTANCE &&
                            distance < monster.maxAttackDistance + MIN_DISTANCE)
                        {

                            runStatue = 2;
                        }
                        else
                            ChangeStatue(en_MonsterSta.Run);
                        break;
                    }
                    //调整位置
                    else
                    { // runStatue = 2
                      // 停
                        StopCheck();
                        // 转向
                        targetPos = playersObj.transform.position;
                        targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position, Vector3.up), 20 * Time.deltaTime);

                        if (Vector3.Angle(targetPos - transform.position, transform.forward) < 3)
                        {
                            //在执行攻击状态之前，看一下是否满足执行特殊技能的条件
                            if (specialSkillCDTimer <= 0 && specialSkill != null)
                            {

                                //这里执行动画包括逻辑

                                ChangeStatue(en_MonsterSta.SpecialSkill);
                                break;
                            }

                            if (attackDcTime >= monster.attackDcTime)
                                ChangeStatue(en_MonsterSta.Attack);
                        }
                    }
                    break;
                }



                if (!isAttackingPlayer)
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
                if (roadPosIndex >= monster.roadPos.Length)
                {
                    ChangeStatue(en_MonsterSta.Run);
                    break;
                }
                transform.position = Vector3.MoveTowards(transform.position, currMoveTargetPos, 2f * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(currMoveTargetPos.x, transform.position.y, currMoveTargetPos.z) - transform.position, Vector3.up), 3 * Time.deltaTime);

                RoadPosCheck(0.05f);
                break;

            case en_MonsterSta.Walk:
                RoadPosCheck(0.3f); //更新下一个目标点
                RunCheck(currMoveTargetPos); //到下一个目标点

                if (Vector3.Distance(transform.position, currMoveTargetPos) < 0.3f)
                {

                }

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
                } //禁止攻击后不会走到玩家身旁的近战攻击点
                break;

            case en_MonsterSta.Run:

                #region 分离路面跳跃处理

                if (navMeshAgent.isOnOffMeshLink)
                {
                    Debug.Log("怪物正在处于分离导航路面！");

                    ChangeStatue(en_MonsterSta.Spawn);
                    break;
                }

                #endregion

                #region 攻击之前先检测能否使用特殊攻击，特殊攻击优先使用

                if (specialSkillCDTimer <= 0)
                {


                    if (runStatue == 0)
                    {
                        //执行特殊技能
                        UpdateSpecialSkillInfo();
                        //是近战特殊技能
                        if (!specialSkill[specialSkillId].isFarAttack && specialSkill != null)
                        {
                            targetPos = bossNearAttackPos; //到远处施展技能
                            Vector3 _targetPos = new Vector3(targetPos.x, transform.position.y,
                                targetPos.z);
                            RunCheck(targetPos);
                            if (Vector3.Distance(transform.position, _targetPos) < 0.15f)
                            {
                                runStatue = 2;

                            }
                            break;
                        }//远程特殊技能
                        else
                        {
                            targetPos = monster.attackPos.transform.position;
                            RunCheck(targetPos);
                            if (Vector3.Distance(transform.position, targetPos) < 0.2f)
                            {
                                runStatue = 2;
                                break;
                            }
                            break;
                        }

                    }
                }

                #endregion

                // 有预定的固定点:
                // 没有固定点：            
                if (runStatue == 0)
                {
                    // 近程怪
                    if (monster.attackDistanceType == en_MonsterAttackDistanceType.Near)
                    {
                        if (monster.attackPos == null)
                        {
                            ChangeStatue(en_MonsterSta.RunOut); isAttackingPlayer = false;
                            break;
                        }
                        targetPos = bossNearAttackPos;
                        Vector3 _targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                        RunCheck(targetPos);
                        _distance = Vector3.Distance(transform.position, targetPos);
                        if (Vector3.Distance(transform.position, _targetPos) < 0.15f)
                        {
                            runStatue = 2;
                            break;
                        }
                        break;
                    }
                    // 有固定攻击点的(非近程怪，可能远近程都有)
                    if (monster.attackPos != null)
                    {
                        targetPos = monster.attackPos.transform.position;
                        RunCheck(targetPos);
                        //Debug.Log(gameObject.name + Vector3.Distance(transform.position, targetPos));
                        Vector3 _targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                        if (Vector3.Distance(transform.position, _targetPos) < 0.2f)
                        {
                            runStatue = 2;
                            break;
                        }
                        break;
                    }
                    // 远程怪:
                    // 进入视野：于小30度
                    RunCheck(tempMoveTargetPos);
                    //Debug.Log(tempMoveTargetPos);
                    //
                    angle = Vector3.Angle(playersObj.transform.forward, transform.position - gameMain.navMeshAgent_MainCar.transform.position);
                    if (Vector3.Distance(transform.position, tempMoveTargetPos) < 1.5f || angle > 45)
                    {
                        UpdateNewTargetPos(false);
                    }
                    //// 行动受阻?
                    if (moveDragTime >= MOVE_DRAG_TIME)
                    {
                        moveDragTime = 0;
                        UpdateNewTargetPos(true);
                    }

                    // 是否看得到玩家
                    if (monster.LookedPlayers() == false)
                    {
                        runTime = 0.45f;
                        break;
                    }
                    if (runTime > 0)
                    {
                        runTime -= Time.deltaTime;
                        break;
                    }
                    distance = Vector3.Distance(transform.position, playersObj.transform.position);
                    if (angle < 30 && distance >= monster.minAttackDistance + MIN_DISTANCE)
                    {
                        runStatue = 1;
                    }
                }
                //调整位置
                else if (runStatue == 1)
                {
                    targetPos = playersObj.transform.position;
                    targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                    RunCheck(targetPos);
                    distance = Vector3.Distance(transform.position, targetPos);
                    if (distance < monster.minAttackDistance + MIN_DISTANCE)
                    {
                        runStatue = 0;
                    }
                    else if (distance < monster.maxAttackDistance + MIN_DISTANCE)
                    {
                        runStatue = 2;
                    }
                }
                //调整位置
                else
                { // runStatue = 2
                    // 停
                    StopCheck();
                    // 转向
                    targetPos = playersObj.transform.position;
                    targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position, Vector3.up), 20 * Time.deltaTime);

                    if (Vector3.Angle(targetPos - transform.position, transform.forward) < 3)
                    {
                        //在执行攻击状态之前，看一下是否满足执行特殊技能的条件
                        if (specialSkillCDTimer <= 0 && specialSkill != null)
                        {

                            //这里执行动画包括逻辑

                            ChangeStatue(en_MonsterSta.SpecialSkill);
                            break;
                        }

                        if (attackDcTime >= monster.attackDcTime)
                            ChangeStatue(en_MonsterSta.Attack);
                    }
                }

                break;

            case en_MonsterSta.Attack:
                // 停
                StopCheck();
                isAttackingPlayer = true;

                if (attackSkill[attackSkillId].IsPlayEnd())
                {

                    UpdateAttackSkillInfo();
                    // 随机点攻击：

                    // 攻击结束:
                    // 固定点攻击：
                    if (monster.attackPos != null)
                    {
                        //怪处于近战模式

                        Vector3 _bossNearAttackPos = new Vector3(bossNearAttackPos.x, transform.position.y,
                            bossNearAttackPos.z);

                        if (Vector3.Distance(transform.position, _bossNearAttackPos) < 3f)
                        {
                            ChangeStatue(en_MonsterSta.Idle);

                            break;

                        }
                    }
                    //远程
                    distance = Vector3.Distance(transform.position, gameMain.navMeshAgent_MainCar.transform.position);
                    angle = Vector3.Angle(gameMain.players_Obj.transform.forward, transform.position - gameMain.players_Obj.transform.position);
                    if (distance < monster.minAttackDistance || angle > 60)
                    {
                        ChangeStatue(en_MonsterSta.RunOut); isAttackingPlayer = false;
                    }
                    else if (distance > monster.maxAttackDistance || angle > 35 || monster.LookedPlayers() == false)
                    {
                        ChangeStatue(en_MonsterSta.Run); isAttackingPlayer = false;
                    }
                    else
                    {

                        ChangeStatue(en_MonsterSta.Idle);
                    }


                }
                break;

            case en_MonsterSta.Roar:
                //停
                StopCheck();
                // 转向
                targetPos = playersObj.transform.position;
                targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position, Vector3.up), 20 * Time.deltaTime);

                if (animator_LvDai.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 &&
                    animator_LvDai.GetCurrentAnimatorStateInfo(0).IsName("Roar"))
                {
                    monster.isAttack = true;
                    ChangeStatue(en_MonsterSta.Run);
                }

                break;

            case en_MonsterSta.SpecialSkill:
                // 停
                StopCheck();
                if (specialSkill[specialSkillId].IsPlayEnd())
                {
                    // 攻击结束:
                    // 固定点攻击：
                    if (monster.attackPos != null)
                    {
                        //怪处于近战模式
                        if (Vector3.Distance(transform.position, bossNearAttackPos) < 0.5f)
                        {
                            ChangeStatue(en_MonsterSta.Idle);

                            break;

                        }

                    }
                    //这时候attackPos = null;

                    //远程，没有固定点
                    distance = Vector3.Distance(transform.position, gameMain.navMeshAgent_MainCar.transform.position);
                    angle = Vector3.Angle(gameMain.players_Obj.transform.forward, transform.position - gameMain.players_Obj.transform.position);
                    if (distance < monster.minAttackDistance || angle > 60)
                    {
                        ChangeStatue(en_MonsterSta.RunOut); isAttackingPlayer = false;
                    }
                    else if (distance > monster.maxAttackDistance || angle > 35 || monster.LookedPlayers() == false)
                    {
                        ChangeStatue(en_MonsterSta.Run); isAttackingPlayer = false;
                    }
                    else
                    {

                        ChangeStatue(en_MonsterSta.Idle);
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
                RoadPosCheck(0.3f);
                RunCheck(currMoveTargetPos);
                // 重新找回攻击点
                if (monster.attackDistanceType == en_MonsterAttackDistanceType.Near)
                {
                    if (monster.attackPos == null)
                    {
                        runTime += Time.deltaTime;
                        if (runTime >= 0.3f)
                        {
                            runTime = 0;
                            monster.attackPos = gameMain.GetMonsterNearAttackPos(monster);
                        }
                    }
                    else
                    {
                        playersObj = gameMain.players_Obj;
                        ChangeStatue(en_MonsterSta.Run);
                    }
                }
                break;
            case en_MonsterSta.Dmage:
                // 停
                AnimatorStateInfo animatorInfo;
                animatorInfo = monster.animator.GetCurrentAnimatorStateInfo(0);
                //normalizedTime: 范围0 -- 1,  0是动作开始，1是动作结束
                if ((animatorInfo.normalizedTime > 1.0f))
                {
                    ChangeStatue(en_MonsterSta.Run);
                }
                StopCheck();
                break;
            case en_MonsterSta.Die:
                break;

            case en_MonsterSta.Spawn:  //怪物跳出场
                //走进度
                progress += Time.deltaTime / jumpTime;
                animatorInfo = monster.animator.GetCurrentAnimatorStateInfo(0);
                if (progress >= 0.99f && animatorInfo.normalizedTime >= 1.0f)
                {
                    navMeshAgent.enabled = false;
                    navMeshAgent.enabled = true; //启用导航
                    navMeshAgent.updatePosition = true;
                    navMeshAgent.CompleteOffMeshLink();
                    progress = 0f;
                    ChangeStatue(en_MonsterSta.Run);
                }
                else
                {
                    navMeshAgent.updatePosition = false;
                    //抛物线跳跃
                    transform.position =
                        Vector3.Lerp(startOffNavPos + Vector3.up * animeYOffset, //2.8是动画初始会自动下降2.8f左右
                            endOffNavPos,
                            progress);
                }
                break;
        }



        //怪物突然不能攻击
        if (!monster.isAttack && monster.statue == en_MonsterSta.Run)
        {
            isAttackingPlayer = false;
            ChangeStatue(en_MonsterSta.Walk);
        }
    }

    void ChangeStatue(en_MonsterSta sta)
    {
        ///print("MStaue: " + sta);

        runTime = 0;
        if (animator_LvDai != null)
        {
            animator_LvDai.enabled = false;
        }
        switch (sta)
        {
            case en_MonsterSta.Idle:
                if (animator_LvDai != null)
                {
                    animator_LvDai.enabled = true;
                }
                if (monster.attackDcTime <= 0.5f && isAttackingPlayer)
                {
                    UpdateAttackSkillInfo();
                    monster.UpdateAttackPos();
                    runStatue = 0;
                    offsetDistance = 0;
                    UpdateNewTargetPos(false);
                    readyAttackDistance = monster.readyAttackDistance;

                    //monster.animator.Play("Attack", 0, 0);
                    break;
                } //如果CD时间太短，那么不播放Idle动画。

                monster.animator.Play("Idle");
                break;
            case en_MonsterSta.Walk:
                if (animator_LvDai != null)
                {
                    animator_LvDai.enabled = true;
                }
                navMeshAgent.speed = walkSpeed;
                monster.animator.Play("Walk");
                break;
            case en_MonsterSta.Run:
                if (animator_LvDai != null)
                {
                    animator_LvDai.enabled = true;
                }
                navMeshAgent.speed = runSpeed;
                UpdateAttackSkillInfo();
                monster.UpdateAttackPos();
                runStatue = 0;
                offsetDistance = 0;
                UpdateNewTargetPos(false);
                readyAttackDistance = monster.readyAttackDistance;
                //
                monster.animator.Play("Run");
                break;
            case en_MonsterSta.RunOut:
                if (animator_LvDai != null)
                {
                    animator_LvDai.enabled = true;
                }
                playersObj = null;
                monster.animator.Play("Run");
                break;
            case en_MonsterSta.Roar:
                if (animator_LvDai != null)
                {
                    animator_LvDai.enabled = true;
                }
                monster.animator.Play("Roar", 0, 0);
                break;
            case en_MonsterSta.Attack:
                attackSkill[attackSkillId].Attack();
                //重置攻击时间
                attackDcTime = 0f;
                break;
            case en_MonsterSta.SpecialSkill:
                specialSkill[specialSkillId].Attack();
                specialSkillCDTimer = specialSkillCD;
                break;
            case en_MonsterSta.Die:
                monster.animator.Play("Die");
                break;
            case en_MonsterSta.Spawn:
                if (animator_LvDai != null)
                {
                    animator_LvDai.enabled = true;
                }

                startOffNavPos = navMeshAgent.currentOffMeshLinkData.startPos;
                endOffNavPos = navMeshAgent.currentOffMeshLinkData.endPos;
                navMeshAgent.enabled = false; //禁用导航
                transform.position = startOffNavPos + Vector3.up * animeYOffset;
                monster.animator.Play("Spawn", 0, 0);
                break;
        }
        monster.ChangeStatueWithoutAnim(sta);
    }

    public void ChangeStatueToRoar()
    {
        ChangeStatue(en_MonsterSta.Roar);
    }

    void UpdateAttackSkillInfo()
    {
        attackSkillId = Random.Range(0, attackSkill.Length);
        monster.minAttackDistance = attackSkill[attackSkillId].minAttackDistance;
        monster.maxAttackDistance = attackSkill[attackSkillId].maxAttackDistance;
        monster.readyAttackDistance = attackSkill[attackSkillId].readyAttackDistance;
    }

    void UpdateSpecialSkillInfo()
    {
        if (specialSkill != null)
        {
            specialSkillId = Random.Range(0, specialSkill.Length);
            monster.minAttackDistance = specialSkill[specialSkillId].minAttackDistance;
            monster.maxAttackDistance = specialSkill[specialSkillId].maxAttackDistance;
            monster.readyAttackDistance = specialSkill[specialSkillId].readyAttackDistance;
        }
        
    }

    //void UpdateAttackPos()
    //{

    //}
    void UpdateRoadPos()
    {
        if (monster.roadPos == null || roadPosIndex >= monster.roadPos.Length)
        {
            currMoveTargetPos = transform.position;
        }
        else
        {
            currMoveTargetPos = monster.roadPos[roadPosIndex].position;
        }
    }
    /// <summary>
    /// currMoveTargetPos = monster.roadPos的下一个索引（roadPosIndex）
    /// </summary>
    void NextRoadPos()
    {
        if (monster.roadPos != null)
        {
            if (roadPosIndex < monster.roadPos.Length)
            {
                roadPosIndex++;
                if (roadPosIndex >= monster.roadPos.Length)
                    roadPosIndex = 0;
                UpdateRoadPos();
            }
        }
    }

    /// <summary>
    /// 如果距离目标位置currMoveTargetPos少于dis距离，那么NextRoadPos
    /// </summary>
    /// <param name="dis"></param>
    void RoadPosCheck(float dis)
    {
        if (Vector3.Distance(transform.position, currMoveTargetPos) < dis)
        {
            NextRoadPos();
        }
    }
    /// <summary>
    /// 导航导到该位置Pos
    /// </summary>
    /// <param name="pos"></param>
    public void RunCheck(Vector3 pos)
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
    /// <summary>
    /// 停止导航，0.3秒后开启导航
    /// </summary>
    public void StopCheck()
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

    public void MoveDragCheck()
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

    /// <summary>
    /// 调整tempMoveTargetPos
    /// </summary>
    /// <param name="drag"></param>
    void UpdateNewTargetPos(bool drag)
    {
        if (drag)
        {
            offsetDistance = Random.Range(-10f, 10f);
        }
        else if (Vector3.Distance(transform.position, gameMain.navMeshAgent_MainCar.transform.position) < monster.minAttackDistance + MIN_DISTANCE)
        {
            offsetDistance = 0;
        }
        else if (Vector3.Cross(playersObj.transform.forward, transform.position - gameMain.navMeshAgent_MainCar.transform.position).y > 0)
        {
            // 右边
            offsetDistance = -10;
        }
        else
        {
            // 左边
            offsetDistance = 10;
        }

        tempMoveTargetPos = playersObj.transform.position + playersObj.transform.forward * monster.readyAttackDistance
             + playersObj.transform.right * offsetDistance;

        tempMoveTargetPos = new Vector3(tempMoveTargetPos.x, gameMain.navMeshAgent_MainCar.transform.position.y, tempMoveTargetPos.z);
    }
}
