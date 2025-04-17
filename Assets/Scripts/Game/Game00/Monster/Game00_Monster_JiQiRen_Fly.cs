using UnityEngine;
using UnityEngine.AI;

public class Game00_Monster_JiQiRen_Fly : MonoBehaviour
{

    const float MOVE_DRAG_TIME = 0.9f;
    const float MIN_DISTANCE = 1.5f;

    public Animator animator_LvDai;
    public NavMeshAgent navMeshAgent;
    public NavMeshObstacle navMeshObstacle;
    public Game00_AttackSkill[] attackSkill;

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

    float emenyDistance;                    // 敌人距离()
    float attackedDistance;					// 被攻击时的距离
    float currAttackDcTime = 0;             // 
    public float distance;
    public float angle;
    float offsetDistance;
    float readyAttackDistance;

    // 走不动检测
    Vector3 currMoveTargetPos;
    Vector3 targetPos;
    float moveDragTime = 0;
    int roadPosIndex;

    // Use this for initialization
    void Start()
    {
        monster = GetComponent<Game00_Monster>();
        gameMain = monster.gameMain;
        playersObj = gameMain.players_Obj;

        roadPosIndex = 0;
        navTime = 0.3f;
        navMeshAgent.enabled = false;
        navMeshObstacle.enabled = false;
        UpdateRoadPos();
        monsterStatue = monster.statue;

        for (int i = 0; i < attackSkill.Length; i++)
        {
            attackSkill[i].Init(monster);
        }
    }
    //void OnEnable() {
    //    ChangeStatue(en_MonsterSta.Attack);
    //}

    // Update is called once per frame
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
                    if (animator_LvDai != null)
                    {
                        animator_LvDai.enabled = true;
                    }
                    attackSkillId = Random.Range(0, attackSkill.Length);
                    monster.minAttackDistance = attackSkill[attackSkillId].minAttackDistance;
                    monster.maxAttackDistance = attackSkill[attackSkillId].maxAttackDistance;
                    monster.readyAttackDistance = attackSkill[attackSkillId].readyAttackDistance;
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

        switch (monster.statue)
        {
            case en_MonsterSta.Idle:
                //if (playersObj == null) {
                //    // 自动攻击巡逻
                //    monster.CheckFindEnemy();
                //} else {
                //    // 有敌人, 靠近攻击
                //    if (currAttackDcTime <= 0 || Vector3.Distance(transform.position, currMoveTargetPos) >= 0.3f) {
                //        ChangeStatue(en_MonsterSta.Run);
                //    } else {
                //        //		transform.LookAt (playersObj.transform);
                //    }
                //}
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
                RoadPosCheck(0.3f);
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

                // 有预定的固定点:
                // 没有固定点：            
                if (runStatue == 0)
                {
                    // 近程怪
                    if (monster.attackDistanceType == en_MonsterAttackDistanceType.Near)
                    {
                        if (monster.attackPos == null)
                        {
                            ChangeStatue(en_MonsterSta.RunOut);
                            break;
                        }
                        targetPos = monster.attackPos.transform.position;
                        //targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                        RunCheck(targetPos);
                        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
                        {
                            runStatue = 2;
                            break;
                        }
                        break;
                    }
                    // 有固定攻击点的
                    if (monster.attackPos != null)
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
                    // 远程怪:
                    // 进入视野：于小30度
                    targetPos = playersObj.transform.position + playersObj.transform.forward * monster.readyAttackDistance
                        + playersObj.transform.right * offsetDistance;
                    targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                    RunCheck(targetPos);
                    //// 行动受阻?
                    //if (moveDragTime >= MOVE_DRAG_TIME) {

                    //}
                    distance = Vector3.Distance(transform.position, targetPos);
                    angle = Vector3.Angle(transform.position - playersObj.transform.position, playersObj.transform.forward);
                    if (offsetDistance == 0)
                    {
                        if (angle < 5 || distance < 1)
                        {
                            if (Vector3.Cross(playersObj.transform.forward, transform.position - playersObj.transform.position).y > 0)
                            {
                                offsetDistance = -10;
                            }
                            else
                            {
                                offsetDistance = 10;
                            }
                        }
                    }
                    else
                    {
                        if (angle >= 30 || distance < 1)
                        {
                            offsetDistance = 0;
                        }
                    }

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
                    targetPos = playersObj.transform.position;
                    distance = Vector3.Distance(transform.position, targetPos);
                    if (angle < 30 && distance >= monster.minAttackDistance + MIN_DISTANCE)
                    {
                        runStatue = 1;
                    }
                }
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
                else
                {
                    // 停
                    StopCheck();
                    // 转向
                    targetPos = playersObj.transform.position;
                    targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position, Vector3.up), 20 * Time.deltaTime);

                    if (Vector3.Angle(targetPos - transform.position, transform.forward) < 3)
                    {
                        ChangeStatue(en_MonsterSta.Attack);
                    }
                }
                //if(runStatue != 0) {
                //    if(Vector3.Distance(transform.position, monster.mainCar_Obj.transform.position) < monster.minDistance) {
                //        runStatue = 0;
                //    }
                //}
                break;

            case en_MonsterSta.Attack:
                // 停
                StopCheck();

                //
                if (attackSkill[attackSkillId].IsPlayEnd())
                {
                    // 攻击结束:
                    // 固定点攻击：
                    if (monster.attackPos != null)
                    {
                        if (Vector3.Distance(transform.position, monster.attackPos.transform.position) < 0.5f)
                        {
                            ChangeStatue(en_MonsterSta.Run);
                            break;
                        }
                    }
                    // 随机点攻击：
                    distance = Vector3.Distance(transform.position, gameMain.navMeshAgent_MainCar.transform.position);
                    angle = Vector3.Angle(gameMain.players_Obj.transform.forward, transform.position - gameMain.players_Obj.transform.position);
                    if (distance < monster.minAttackDistance || angle > 60)
                    {
                        ChangeStatue(en_MonsterSta.RunOut);
                    }
                    else if (distance > monster.maxAttackDistance || angle > 35 || monster.LookedPlayers() == false)
                    {
                        ChangeStatue(en_MonsterSta.Run);
                    }
                    else
                    {
                        ChangeStatue(en_MonsterSta.Attack);
                        //ChangeStatue(en_MonsterSta.Run);
                    }
                }


                //if (monster.animEvent != null) {
                //    if (monster.animEvent.event_AttackEffect) {
                //        monster.animEvent.event_AttackEffect = false;
                //        if (monster.effect_Attack_Prefab != null) {
                //            GameObject obj = GameObject.Instantiate(monster.effect_Attack_Prefab);
                //            obj.transform.position = monster.firePos_Obj.transform.position;
                //            obj.transform.eulerAngles = monster.firePos_Obj.transform.eulerAngles;
                //        }
                //        if (playersObj != null) {
                //            //playersObj.Attacked(monster, monster.attackValue);
                //            gameMain.AttackPlayers(monster, monster.attackValue);
                //        }
                //    }
                //}
                //runTime += Time.deltaTime;
                //if (runTime < 0.5f)
                //    break;
                //if (monster.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 || runTime >= 3.5f) {
                //    //ChangeStatue(en_MonsterSta.RunOut);

                //    attackCnt++;
                //    // 隐藏：
                //    if (monster.hidePos != null && attackCnt >= 2) {
                //        attackCnt = 0;
                //        ChangeStatue(en_MonsterSta.Hide);
                //        break;
                //    }
                //    // 固定点攻击：
                //    if (monster.attackPos != null) {
                //        if (Vector3.Distance(transform.position, monster.attackPos.transform.position) < 0.5f) {
                //            ChangeStatue(en_MonsterSta.Run);
                //            break;
                //        }
                //    }
                //    // 随机点攻击：
                //    distance = Vector3.Distance(transform.position, gameMain.transform.position);
                //    angle = Vector3.Angle(gameMain.players_Obj.transform.forward, transform.position - gameMain.players_Obj.transform.position);
                //    if (distance < monster.minAttackDistance || angle > 60) {
                //        ChangeStatue(en_MonsterSta.RunOut);
                //    } else if (distance > monster.maxAttackDistance || angle > 35 || monster.LookedPlayers() == false) {
                //        ChangeStatue(en_MonsterSta.Run);
                //    } else {
                //        ChangeStatue(en_MonsterSta.Attack);
                //        //ChangeStatue(en_MonsterSta.Run);
                //    }
                //}
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
        ///print("MStaue: " + sta);

        runTime = 0;
        if (animator_LvDai != null)
        {
            animator_LvDai.enabled = false;
        }
        switch (sta)
        {
            case en_MonsterSta.Idle:
                break;
            case en_MonsterSta.Walk:
                if (animator_LvDai != null)
                {
                    animator_LvDai.enabled = true;
                }
                monster.animator.Play("Run");
                break;
            case en_MonsterSta.Run:
                if (animator_LvDai != null)
                {
                    animator_LvDai.enabled = true;
                }
                attackSkillId = Random.Range(0, attackSkill.Length);
                monster.minAttackDistance = attackSkill[attackSkillId].minAttackDistance;
                monster.maxAttackDistance = attackSkill[attackSkillId].maxAttackDistance;
                monster.readyAttackDistance = attackSkill[attackSkillId].readyAttackDistance;
                runStatue = 0;
                offsetDistance = 0;
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
            case en_MonsterSta.Attack:
                attackSkill[attackSkillId].Attack();
                break;
            case en_MonsterSta.Die:
                monster.animator.Play("Die");
                break;
        }
        monster.ChangeStatueWithoutAnim(sta);
    }

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
    void NextRoadPos()
    {
        if (monster.roadPos != null)
        {
            if (roadPosIndex < monster.roadPos.Length)
            {
                roadPosIndex++;
                UpdateRoadPos();
            }
        }
    }
    void RoadPosCheck(float dis)
    {
        if (Vector3.Distance(transform.position, currMoveTargetPos) < dis)
        {
            NextRoadPos();
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
}
