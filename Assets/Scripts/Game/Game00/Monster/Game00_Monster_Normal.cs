using UnityEngine;
using UnityEngine.AI;

public enum en_AttackSta
{
    Attack = 0,
    End,
}
public enum en_HideSta
{
    Move,
    Hide,
    HideEnd,
}

public class Game00_Monster_Normal : MonoBehaviour
{
    const float MOVE_DRAG_TIME = 0.9f;
    const float MIN_DISTANCE = 1.5f;

    public NavMeshAgent navMeshAgent;
    public NavMeshObstacle navMeshObstacle;
    public bool isAttackReady = false;
    public bool isAttackEnd = false;
    public int maxAttackCnt = 1;
    public float attackDcTime = 1f;
    public float rotateSpeed = 180;         // 转身速度

    public Game00_Main gameMain;
    public Game00_Monster monster;
    Game00_MonsterTempPos monsterTempPos;
    GameObject mainCarObj;
    public GameObject playersObj;
    GameObject currHidePos;
    en_MonsterSta monsterStatue;

    float runTime;
    float dcTime;
    float navTime;
    public int runStatue;
    public en_AttackSta attackStatue;
    public en_HideSta hideStatue;
    int attackCnt;

    // 走不动检测
    Vector3 currMoveTargetPos;
    Vector3 targetPos;
    float moveDragTime = 0;
    int roadPosIndex;

    //
    float emenyDistance;                    // 敌人距离()
    float attackedDistance;					// 被攻击时的距离
    float currAttackDcTime = 0;             // 
    public float distance;
    public float angle;
    public float angle2;
    public float offsetDistance;
    float readyAttackDistance;
    float changeMaxDistanceTime;

    // Use this for initialization
    void Start()
    {
        monster = GetComponent<Game00_Monster>();
        gameMain = monster.gameMain;
        mainCarObj = gameMain.navMeshAgent_MainCar.gameObject;
        playersObj = gameMain.players_Obj;
        roadPosIndex = 0;
        attackCnt = 0;
        changeMaxDistanceTime = Random.Range(2.5f, 4.5f);
        navTime = 0.3f;
        navMeshAgent.enabled = false;
        navMeshObstacle.enabled = false;
        UpdateRoadPos();
        monsterStatue = monster.statue;
    }

    // Update is called once per frame
    void Update()
    {
        if (monster == null)
            return;

        // 攻击冷却时间计数
        if (dcTime > 0)
        {
            dcTime -= Time.deltaTime;
        }
        if (navTime > 0)
        {
            navTime -= Time.deltaTime;
        }
        if (changeMaxDistanceTime > 0)
        {
            changeMaxDistanceTime -= Time.deltaTime;
        }
        // 移动受阻检测
        MoveDragCheck();

        if (monsterStatue != monster.statue)
        {
            ChangeStatue(monster.statue);
        }

        switch (monster.statue)
        {
            case en_MonsterSta.Idle:
                //if (playersObj == null) 
                //{
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
                    RunCheck(transform.position);
                    runTime += Time.deltaTime;
                    if (runTime >= 0.1f)
                    {
                        ChangeStatue(en_MonsterSta.Run);
                    }
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
                        //  Debug.Log(Vector3.Distance(transform.position, targetPos));
                        if (Vector3.Distance(transform.position, targetPos) < 0.2f)
                        {

                            runStatue = 2;
                            break;
                        }
                        break;
                    }
                    // 远程怪:
                    // 进入视野：于小30度
                    targetPos = playersObj.transform.position + playersObj.transform.forward * readyAttackDistance
                        + playersObj.transform.right * offsetDistance;
                    targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                    RunCheck(targetPos);
                    // 行动受阻?
                    if (moveDragTime >= MOVE_DRAG_TIME)
                    {

                    }
                    distance = Vector3.Distance(transform.position, targetPos);
                    angle = Vector3.Angle(transform.position - mainCarObj.transform.position, playersObj.transform.forward);
                    if (offsetDistance == 0)            //??
                    {
                        if (angle < 5 || distance < 1)
                        {
                            if (Vector3.Cross(playersObj.transform.forward, transform.position - playersObj.transform.position).y > 0)
                            {
                                offsetDistance = -10;       //??
                            }
                            else
                            {
                                offsetDistance = 10;        //??
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
                    if (angle < 30 && distance > monster.minAttackDistance + MIN_DISTANCE)
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

                if (monster.animEvent != null)
                {
                    if (monster.animEvent.event_AttackEffect)
                    {
                        monster.animEvent.event_AttackEffect = false;
                        if (monster.effect_Attack_Prefab != null)
                        {
                            GameObject obj = GameObject.Instantiate(monster.effect_Attack_Prefab);
                            obj.transform.position = monster.firePos_Obj.transform.position;
                            obj.transform.eulerAngles = monster.firePos_Obj.transform.eulerAngles;
                        }
                        if (monster.effect_AttackScreen_Prefab != null)
                        {
                            GameObject obj = GameObject.Instantiate(monster.effect_AttackScreen_Prefab, gameMain.gameUI.transform);
                            obj.transform.position = Camera.main.WorldToScreenPoint(monster.firePos_Obj.transform.position);
                        }
                        if (playersObj != null)
                        {
                            //playersObj.Attacked(monster, monster.attackValue);
                            gameMain.AttackPlayers(monster, monster.attackValue);
                        }
                    }
                }
                runTime += Time.deltaTime;
                if (runTime < 0.2f)
                {
                    gameMain.AttackPlayers(monster, monster.attackValue);
                    break;

                }

                if (monster.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 || runTime >= 3.5f)
                {
                    //动画结束
                    if (monster.attackPos == null)
                    {
                        distance = Vector3.Distance(transform.position, gameMain.navMeshAgent_MainCar.transform.position);
                    }
                    else
                    {
                        distance = monster.maxAttackDistance;
                    }
                    targetPos = gameMain.players_Obj.transform.position - transform.position;
                    angle = Vector3.Angle(transform.forward, new Vector3(targetPos.x, 0, targetPos.z));

                    if (attackStatue == en_AttackSta.Attack)
                    {
                        if (attackCnt >= maxAttackCnt ||
                            distance < monster.minAttackDistance ||// distance > monster.maxAttackDistance || 
                            angle > 15)
                        {
                            if (isAttackEnd)
                            {
                                runTime = 0;
                                //monster.animator.Play("AttackEnd");
                                attackStatue = en_AttackSta.End;
                                break;
                            }
                        }
                        else
                        {
                            if (runTime >= attackDcTime)
                            {
                                runTime = 0;
                                monster.animator.Play("Attack", 0, 0);
                                attackCnt++;
                            }
                            break;
                        }
                    }
                    else if (attackStatue == en_AttackSta.End)
                    {

                    }
                    // 一轮攻击结束
                    // 隐藏：
                    if (monster.hidePos != null)
                    {
                        // 固定隐藏点
                        currHidePos = monster.hidePos;
                        ChangeStatue(en_MonsterSta.Hide);
                        break;
                    }
                    else
                    {
                        // 公用隐藏点
                        monsterTempPos = gameMain.GetMonsterHidePos(monster);
                        if (monsterTempPos != null)
                        {
                            currHidePos = monsterTempPos.gameObject;
                            ChangeStatue(en_MonsterSta.Hide);
                            break;
                        }
                    }
                    // 固定点攻击：
                    if (monster.attackPos != null)
                    {
                        if (Vector3.Distance(transform.position, monster.attackPos.transform.position) > 0.5f)
                        {
                            ChangeStatue(en_MonsterSta.Run);
                            break;
                        }
                    }
                    // 随机点攻击：
                    if (changeMaxDistanceTime <= 0)
                    {
                        changeMaxDistanceTime = Random.Range(2.5f, 4.5f);
                        if (monster.minAttackDistance + 3 <= monster.maxAttackDistance)
                        {
                            monster.maxAttackDistance -= 1.5f;
                        }
                    }
                    angle2 = Vector3.Angle(gameMain.players_Obj.transform.forward, transform.position - gameMain.players_Obj.transform.position);
                    if (distance < monster.minAttackDistance || angle2 > 60)
                    {
                        ChangeStatue(en_MonsterSta.RunOut);
                    }
                    else if (distance > monster.maxAttackDistance || angle > 15 || angle2 > 35 || monster.LookedPlayers() == false)
                    {
                        ChangeStatue(en_MonsterSta.Run);
                    }
                    else
                    {
                        ChangeStatue(en_MonsterSta.Attack);
                        //ChangeStatue(en_MonsterSta.Run);
                    }
                }
                break;

            case en_MonsterSta.Hide:
                switch (hideStatue)
                {
                    case en_HideSta.Move:
                        if (Vector3.Distance(transform.position, currHidePos.transform.position) > 0.2f)
                        {
                            RunCheck(currHidePos.transform.position);
                        }
                        else
                        {
                            transform.rotation = Quaternion.RotateTowards(transform.rotation, currHidePos.transform.rotation, 180 * Time.deltaTime);
                            if (Vector3.Angle(transform.forward, currHidePos.transform.forward) < 3)
                            {
                                monster.animator.Play("Hide");
                                hideStatue = en_HideSta.Hide;
                            }
                        }
                        break;
                    case en_HideSta.Hide:
                        // 停
                        StopCheck();
                        runTime += Time.deltaTime;
                        if (runTime >= 2)
                        {
                            runTime = 0;
                            monster.animator.Play("HideEnd");
                            hideStatue = en_HideSta.HideEnd;
                        }
                        break;
                    case en_HideSta.HideEnd:
                        if (monster.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                        {
                            // 解除公用隐藏点
                            if (monsterTempPos != null)
                            {
                                monsterTempPos.monster = null;
                                monsterTempPos = null;
                                currHidePos = null;
                            }
                            ChangeStatue(en_MonsterSta.Run);
                        }
                        break;
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
        print("MonsStaue: " + sta);
        monsterStatue = sta;
        runTime = 0;
        switch (sta)
        {
            case en_MonsterSta.Idle:
                break;
            case en_MonsterSta.DownCar:
                UpdateRoadPos();
                if (monster.animator != null)
                    monster.animator.Play("Run");
                else if (monster.animation != null)
                    monster.animation.Play("Run");
                break;
            case en_MonsterSta.Walk:
                if (monster.animator != null)
                    monster.animator.Play("Run");
                else if (monster.animation != null)
                    monster.animation.Play("Run");
                break;
            case en_MonsterSta.Run:
                runStatue = 0;
                offsetDistance = 0;
                readyAttackDistance = monster.readyAttackDistance;
                if (monster.animator != null)
                    monster.animator.Play("Run");
                else if (monster.animation != null)
                    monster.animation.Play("Run");
                break;
            case en_MonsterSta.RunOut:
                playersObj = null;
                if (monster.animator != null)
                    monster.animator.Play("Run");
                else if (monster.animation != null)
                    monster.animation.Play("Run");
                break;
            case en_MonsterSta.Attack:
                attackCnt = 1;
                if (isAttackReady)
                {
                    // monster.animator.Play("AttackReady");
                    if (monster.animator != null)
                        monster.animator.Play("AttackReady");
                    else if (monster.animation != null)
                        monster.animation.Play("AttackReady");
                }
                else
                {
                    // monster.animator.Play("Attack", 0, 0);
                    if (monster.animator != null)
                        monster.animator.Play("Attack");
                    else if (monster.animation != null)
                        monster.animation.Play("Attack");
                }
                attackStatue = en_AttackSta.Attack;
                break;
            case en_MonsterSta.Hide:
                if (Vector3.Distance(transform.position, currHidePos.transform.position) > 0.2f ||
                    Vector3.Angle(transform.forward, currHidePos.transform.forward) > 3)
                {
                    if (monster.animator != null)
                        monster.animator.Play("Run");
                    else if (monster.animation != null)
                        monster.animation.Play("Run");
                    hideStatue = en_HideSta.Move;
                }
                else
                {
                    //monster.animator.Play("Hide");
                    if (monster.animator != null)
                        monster.animator.Play("Hide");
                    else if (monster.animation != null)
                        monster.animation.Play("Hide");
                    hideStatue = en_HideSta.Hide;
                }
                break;
            case en_MonsterSta.Die:
                navMeshAgent.enabled = false;
                navMeshObstacle.enabled = false;
                // monster.animator.Play("Die");
                if (monster.animator != null)
                    monster.animator.Play("Die");
                else if (monster.animation != null)
                    monster.animation.Play("Die");

                break;
        }

        monster.ChangeStatueWithoutAnim(sta);
    }
    // 是否在准备攻击范围内
    bool IsInReadyAttackArea()
    {
        targetPos = playersObj.transform.position + playersObj.transform.forward * monster.readyAttackDistance;
        targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        distance = Vector3.Distance(transform.position, targetPos);
        angle = Vector3.Angle(transform.position - playersObj.transform.position, playersObj.transform.forward);
        return true;
    }
    // 是否在攻击范围内
    bool IsInAttackArea()
    {
        return true;
    }

    void FindNewOffsetDistance()
    {
        //print("F_N:");
        //readyAttackDistance = 0;
        //offsetDistance = 0;

        //Vector3 newPos;
        //for (float i = monster.readyAttackDistance; i > monster.minAttackDistance; i--) {
        //    for (float j = -10; j < 10; j++) {
        //        newPos = playersObj.transform.position + playersObj.transform.forward * i
        //            + playersObj.transform.right * j;
        //        if (navMeshAgent.SetDestination(newPos)) {
        //            readyAttackDistance = i;
        //            offsetDistance = j;
        //            print("FN_ed: " + offsetDistance);
        //            return;
        //        }
        //    }
        //}
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

    void Attack()
    {

        monster.ChangeStatue(en_MonsterSta.Attack);
    }

    //
    //void Check_AttackEffect() {
    //    if (attackEffect) {
    //        attackEffect = false;

    //        // 发射炮弹
    //        if (firePos_Obj != null) {
    //            if (shell_Prefab != null) {
    //                GameObject shell = GameObject.Instantiate(shell_Prefab, firePos_Obj.transform.position, Quaternion.identity);
    //                //shell.transform.LookAt(new Vector3(playersObj.transform.position.x, transform.position.y, playersObj.transform.position.z));
    //                shell.transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, playersObj.transform.position.z));
    //                Shell shell_Scripts = shell.GetComponent<Shell>();
    //                shell_Scripts.attackValue = attackValue;
    //                if (playersObj != null) {
    //                    shell_Scripts.targetObj = playersObj.gameObject;
    //                }
    //            }
    //            // 特效
    //            if (effect_Attack_Prefab != null) {
    //                GameObject effect = GameObject.Instantiate(effect_Attack_Prefab, firePos_Obj.transform);
    //                effect.transform.localPosition = new Vector3(0, 0, 0);
    //                effect.transform.localEulerAngles = new Vector3(0, 0, 0);
    //                effect.transform.LookAt(Camera.main.transform);
    //                effect.transform.parent = null;
    //            }
    //        }
    //        // 
    //        if (attackSoundPlayTime == en_AttackSoundPlayTime.Effect) {
    //            if (audioSource_Attack != null) {
    //                audioSource_Attack.Play();
    //            }
    //        }

    //        if (playersObj != null) {    // && shell_Prefab != null
    //            if (Vector3.Distance(transform.position, playersObj.transform.position) < attackDistance * 1.5f) {
    //                playersObj.Attacked(this, attackValue);  //, effect_AttackScreen_Prefab, transform.position + new Vector3(0, 0, 0), attackType);
    //            }
    //        }

    //        //
    //        if (isBOSS) {
    //            // 震屏
    //            gameMain.ShakeStart();
    //        }
    //    }
    //}


    void AnimationEnventCheck()
    {

        ////
        //public void Event_IdlePlayEnd() {

        //}
        //// 脚步
        //public void Event_Setp() {
        //    if (audioSource_Run != null) {
        //        audioSource_Run.Play();
        //    }
        //    if (isBOSS) {
        //        gameMain.ShakeStart();
        //    }
        //}
        //// 倒地
        //public void Event_FallDown() {
        //    if (audioSource_Run != null) {
        //        audioSource_Run.Play();
        //    }
        //    if (isBOSS) {
        //        gameMain.ShakeStart();
        //    }
        //}
        ////
        //public void Event_AttackEffect() {
        //    attackEffect = true;
        //}//
        //public void Event_AttackPlayEnd() {
        //    if (statue == en_MonsterSta.Attack) {
        //        if (moveTargetPos.Length > 1) {
        //            moveTargetPosId++;
        //            if (moveTargetPosId >= moveTargetPos.Length) {
        //                moveTargetPosId = 0;
        //            }
        //            currMoveTargetPos = moveTargetPos[moveTargetPosId].transform.position;
        //        }
        //        ChangeStatue(en_MonsterSta.Idle);
        //    }
        //}
        //public void Event_DamagePlayEnd() {
        //    if (statue == en_MonsterSta.Dmage) {
        //        ChangeStatue(en_MonsterSta.Idle);
        //    }
        //}
        ////
        //public void Event_DiePlayEnd() {
        //    if (isFly == false) {
        //        Destroy(gameObject, 2.0f);
        //    }
        //}
    }
}
