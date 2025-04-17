using UnityEngine;
using UnityEngine.AI;

public class Game00_Monster_Tank : MonoBehaviour
{
    public GameObject paoTai_Obj;
    public GameObject dieBombEff_Prefab;
    public GameObject dieFireEff_Prefab;
    public NavMeshAgent navMeshAgent;
    public NavMeshObstacle navMeshObstacle;
    public GameObject dieEffPos_Obj;
    public AudioSource audioSource_Run;

    public float rotateSpeed = 180;         // 转身速度

    Game00_Monster monster;

    GameObject mainCar_Obj;
    GameObject players_Obj;
    en_MonsterSta monsterSta;

    // 走不动检测
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
        monster = GetComponent<Game00_Monster>();
        mainCar_Obj = monster.mainCar_Obj;
        players_Obj = monster.gameMain.players_Obj;
        //material = luDai_Obj.GetComponent<Renderer>().sharedMaterial;
        monsterSta = monster.statue;

        roadPosIndex = 0;
        attackDcTime = 0;
        navTime = 0.3f;
        navMeshAgent.enabled = false;
        navMeshObstacle.enabled = false;
        audioSource_Run.Play();
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
            navMeshAgent.enabled = false;
            navMeshObstacle.enabled = false;
            dieEffDcTime = 0;
            // 爆炸
            if (dieBombEff_Prefab != null)
            {
                GameObject obj = GameObject.Instantiate(dieBombEff_Prefab, transform);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localEulerAngles = Vector3.zero;
            }
            // 燃烧
            if (dieFireEff_Prefab != null)
            {
                GameObject obj = GameObject.Instantiate(dieFireEff_Prefab, transform);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localEulerAngles = Vector3.zero;
            }
            monster.gameMain.ShakeStart(0.2f, 4);
        }

        // 攻击冷却时间计数
        if (attackDcTime > 0)
        {
            attackDcTime -= Time.deltaTime;
        }

        if (navTime > 0)
        {
            navTime -= Time.deltaTime;
        }
        switch (monster.statue)
        {
            case en_MonsterSta.Idle:
                ChangeStatue(en_MonsterSta.Walk);
                break;

            case en_MonsterSta.Walk:
                RoadPosCheck();
                RunCheck(currMoveTargetPos);

                ChangeStatue(en_MonsterSta.Run);
                break;

            case en_MonsterSta.Run:
                // 沿路走
                RoadPosCheck();
                RunCheck(currMoveTargetPos);
                break;

            case en_MonsterSta.Attack:
                break;

            case en_MonsterSta.Stop:
                StopCheck();
                break;
            case en_MonsterSta.Dmage:
                // 停
                StopCheck();
                break;
            case en_MonsterSta.Die:
                // 停
                break;
        }

        if (monster.statue != en_MonsterSta.Die)
        {
            // 攻击
            if (attackDcTime <= 0)
            {
                distance = Vector3.Distance(transform.position, mainCar_Obj.transform.position);
                angle = Vector3.Angle(players_Obj.transform.forward, transform.position - mainCar_Obj.transform.position);
                if (distance >= monster.minAttackDistance && distance <= monster.maxAttackDistance && angle < 30)
                {
                    targetPos = mainCar_Obj.transform.position;
                    targetPos = new Vector3(targetPos.x, paoTai_Obj.transform.position.y, targetPos.z);
                    angle = Vector3.Angle(paoTai_Obj.transform.forward, targetPos - paoTai_Obj.transform.position);
                    if (angle > 1)
                    {
                        paoTai_Obj.transform.rotation = Quaternion.Slerp(paoTai_Obj.transform.rotation, Quaternion.LookRotation(targetPos - paoTai_Obj.transform.position, Vector3.up), 3 * Time.deltaTime);
                    }
                    else
                    {
                        attackDcTime = 3.5f;
                        // 开炮
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
                    }
                }
            }
        }
    }
    void ChangeStatue(en_MonsterSta sta)
    {
        runTime = 0;
        monsterSta = sta;
        switch (sta)
        {
            case en_MonsterSta.Idle:
                break;
            case en_MonsterSta.Walk:
                break;
            case en_MonsterSta.Run:
                runStatue = 0;
                MoveStart();
                break;
            case en_MonsterSta.RunOut:
                runStatue = 0;
                monster.playerEnemy = null;
                monster.statue = sta;
                return;
            case en_MonsterSta.Attack:
                break;
            case en_MonsterSta.Stop:
                if (monster.animator)
                {
                    monster.animator.enabled = false;
                }
                audioSource_Run.Stop();
                break;
            case en_MonsterSta.Die:
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
            if (roadPosIndex + 1 < monster.roadPos.Length)
            {
                roadPosIndex++;
                UpdateRoadPos();
            }
            else
            {
                ChangeStatue(en_MonsterSta.Stop);
            }
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

    float GetPaoDanSpeed(Vector3 dir, float height, float distance)
    {
        float t = 1f;
        t = Mathf.Sqrt(2 * height / 10);

        return distance / t;
    }
}
