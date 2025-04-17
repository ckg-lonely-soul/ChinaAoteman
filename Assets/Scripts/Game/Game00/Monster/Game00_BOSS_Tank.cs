using UnityEngine;
using UnityEngine.AI;

public class Game00_BOSS_Tank : MonoBehaviour
{
    public GameObject paoTai_Obj;
    public GameObject dieBombEff_Prefab;
    public GameObject dieFireEff_Prefab;
    //public GameObject luDai_Obj;
    public NavMeshAgent navMeshAgent;
    public NavMeshObstacle navMeshObstacle;
    public GameObject dieEffPos_Obj;
    Transform[] dieEff_Pos;

    public float rotateSpeed = 180;         // 转身速度

    Game00_Monster monster;
    Game00_Main gameMain;

    GameObject mainCar_Obj;
    GameObject players_Ojb;
    en_MonsterSta monsterSta;

    // 走不动检测
    Vector3 runOldPos;                      // 走时旧坐标
    Vector3 currMoveTargetPos;
    Vector3 targetPos;
    float runOldPosTime = 0;
    int runOldCnt = 0;
    bool moveDraged = false;
    int roadPosIndex;

    //
    float runTime;
    float attackDcTime;
    float dieEffDcTime;
    float navTime;
    int runStatue;
    float emenyDistance;                    // 敌人距离()
    float attackedDistance;					// 被攻击时的距离
    float currAttackDcTime = 0;             // 
    float distance;
    float angle;

    //Material material;
    // Use this for initialization
    void Start()
    {
        dieEff_Pos = dieEffPos_Obj.GetComponentsInChildren<Transform>();

        monster = GetComponent<Game00_Monster>();
        gameMain = monster.gameMain;
        mainCar_Obj = monster.mainCar_Obj;
        players_Ojb = monster.GetPlayersObj();
        //material = luDai_Obj.GetComponent<Renderer>().sharedMaterial;
        monsterSta = monster.statue;

        roadPosIndex = 0;
        navTime = 0.3f;
        navMeshAgent.enabled = false;
        navMeshObstacle.enabled = false;
        UpdateRoadPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (monster == null)
            return;

        if (monster.statue == en_MonsterSta.Die && monsterSta != en_MonsterSta.Die)
        {
            monsterSta = en_MonsterSta.Die;
            dieEffDcTime = 0;
            if (dieFireEff_Prefab != null)
            {
                GameObject obj = GameObject.Instantiate(dieFireEff_Prefab, transform);
                obj.transform.position = dieEffPos_Obj.transform.position;
                obj.transform.eulerAngles = transform.eulerAngles;
            }
        }

        // 攻击冷却时间计数
        if (attackDcTime > 0)
        {
            attackDcTime -= Time.deltaTime;
        }

        if (navTime > 0)
        {
            navTime -= Time.deltaTime;
            if (navTime <= 0)
            {
                if (navMeshAgent != null)
                {
                    navMeshAgent.enabled = true;
                }
            }
        }
        switch (monster.statue)
        {
            case en_MonsterSta.Idle:
                ChangeStatue(en_MonsterSta.Walk);
                break;

            case en_MonsterSta.Walk:
                ChangeStatue(en_MonsterSta.Run);
                break;

            case en_MonsterSta.Run:
                RoadPosCheck();
                RunCheck(currMoveTargetPos);

                if (attackDcTime > 0)
                    break;

                angle = Vector3.Angle(transform.position - mainCar_Obj.transform.position, players_Ojb.transform.forward);
                if (angle < 35)
                {
                    ChangeStatue(en_MonsterSta.Attack);
                }
                break;

            case en_MonsterSta.Attack:
                // 停
                StopCheck();

                // 转向: 炮台转向目标
                targetPos = mainCar_Obj.transform.position;
                targetPos = new Vector3(targetPos.x, paoTai_Obj.transform.position.y, targetPos.z);
                paoTai_Obj.transform.rotation = Quaternion.Slerp(paoTai_Obj.transform.rotation, Quaternion.LookRotation(targetPos - paoTai_Obj.transform.position, Vector3.up), 3 * Time.deltaTime);

                if (Vector3.Angle(targetPos - paoTai_Obj.transform.position, paoTai_Obj.transform.forward) < 1)
                {
                    // 开炮:
                    if (monster.shell_Prefab != null)
                    {
                        //
                        GameObject obj = GameObject.Instantiate(monster.shell_Prefab);
                        obj.transform.position = monster.firePos_Obj.transform.position;
                        obj.transform.eulerAngles = monster.firePos_Obj.transform.eulerAngles;
                        Game00_Shell_Monster monsterShell = obj.GetComponent<Game00_Shell_Monster>();
                        // 计算炮弹速度：
                        //float dis = Vector3.Distance(mainCar_Obj.transform.position, new Vector3(obj.transform.position.x, 0, obj.transform.position.y));
                        //float time = obj.transform.position.y / 0.3f;
                        //monsterShell.speed = dis / time;
                        monsterShell.attackValue = monster.attackValue;
                        monsterShell.speed = GetPaoDanSpeed(monster.firePos_Obj.transform.forward, monster.firePos_Obj.transform.position.y - mainCar_Obj.transform.position.y, Vector3.Distance(transform.position, mainCar_Obj.transform.position));
                        monsterShell.Init(monster.gameMain, monster, null);
                    }
                    // 特效
                    if (monster.effect_Attack_Prefab != null)
                    {
                        GameObject obj = GameObject.Instantiate(monster.effect_Attack_Prefab);
                        obj.transform.position = monster.firePos_Obj.transform.position;
                        obj.transform.eulerAngles = monster.firePos_Obj.transform.eulerAngles;
                    }
                    attackDcTime = 5.5f;
                    ChangeStatue(en_MonsterSta.Stop);
                }
                break;

            case en_MonsterSta.Stop:
                if (runTime < 1.2f)
                {
                    runTime += Time.deltaTime;
                }
                else
                {
                    paoTai_Obj.transform.rotation = Quaternion.Slerp(paoTai_Obj.transform.rotation, transform.rotation, 3 * Time.deltaTime);
                    if (Vector3.Angle(paoTai_Obj.transform.forward, transform.forward) < 3)
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
                //
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
                        monster.gameMain.ShakeStart(0.3f, 4);
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
            case en_MonsterSta.Idle:
                break;
            case en_MonsterSta.Walk:
                monster.animator.Play("Run");
                break;
            case en_MonsterSta.Run:
                monster.animator.Play("Run");
                runStatue = 0;
                MoveStart();
                break;
            case en_MonsterSta.RunOut:
                runStatue = 0;
                // monster.playerEnemy = null;
                monster.statue = sta;
                return;
            case en_MonsterSta.Attack:
                monster.animator.Play("Stop");
                break;
            case en_MonsterSta.Die:
                break;
        }
        monster.ChangeStatue(sta);
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
            roadPosIndex++;
            if (roadPosIndex >= monster.roadPos.Length)
            {
                roadPosIndex = 0;
            }
            UpdateRoadPos();
        }
    }
    void RoadPosCheck()
    {
        if (Vector3.Distance(transform.position, currMoveTargetPos) < 0.3f)
        {
            NextRoadPos();
        }
    }
    void RunCheck(Vector3 pos)
    {
        if (navMeshObstacle.enabled)
        {
            navMeshObstacle.enabled = false;
            navTime = 0.3f;
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

    void MoveStart()
    {
        moveDraged = false;
        runOldCnt = 0;
    }
    void MoveDragCheck()
    {
        // 怪行动受阻检测
        runOldPosTime += Time.deltaTime;
        if (runOldPosTime >= 0.2f)
        {
            runOldPosTime = 0;
            if (Vector3.Distance(runOldPos, transform.position) < navMeshAgent.speed * 0.1f)
            {
                if (runOldCnt++ >= 5)
                {
                    runOldCnt = 0;
                    moveDraged = true;
                }
            }
            else
            {
                runOldCnt = 0;
                moveDraged = false;
            }
            runOldPos = transform.position;
        }
    }

    float GetPaoDanSpeed(Vector3 dir, float height, float distance)
    {
        float t = 1f;
        t = Mathf.Sqrt(2 * height / 10);

        return distance / t;
    }
}
