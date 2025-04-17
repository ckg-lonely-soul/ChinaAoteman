using UnityEngine;
using UnityEngine.AI;

public class Game00_Monster_Motor : MonoBehaviour
{
    public GameObject peopel_Obj;
    public NavMeshAgent navMeshAgent;
    public NavMeshObstacle navMeshObstacle;
    public GameObject dieFireEff_Prefab;

    Game00_Main gameMain;
    Game00_Monster monster;
    Game00_Shell_Monster shell;

    en_MonsterSta monsterStatue;


    Vector3 currMoveTargetPos;
    Vector3 targetPos;
    float moveDragTime = 0;
    int roadPosIndex;

    float distance;
    float angle;
    float navTime = 0;
    float runTime = 0;
    int runStatue;

    // Use this for initialization
    void Start()
    {
        monster = GetComponent<Game00_Monster>();
        gameMain = monster.gameMain;

        roadPosIndex = 0;
        UpdateRoadPos();

        ChangeStatue(en_MonsterSta.Run);
    }

    // Update is called once per frame
    void Update()
    {
        if (monster == null)
            return;

        if (navTime > 0)
        {
            navTime -= Time.deltaTime;
        }

        if (monster.statue == en_MonsterSta.Die)
        {
            if (monsterStatue != en_MonsterSta.Die)
            {
                monsterStatue = en_MonsterSta.Die;
                //
                navMeshAgent.enabled = false;
                navMeshObstacle.enabled = false;
                monster.animator.enabled = false;
                //
                if (peopel_Obj != null)
                {
                    Destroy(peopel_Obj);
                }
                // 
                if (monster.effect_Die_Prefab != null)
                {
                    GameObject.Instantiate(monster.effect_Die_Prefab, monster.dieEffPos_Obj.transform.position, Quaternion.identity);
                }
                if (dieFireEff_Prefab != null)
                {
                    GameObject.Instantiate(dieFireEff_Prefab, monster.dieEffPos_Obj.transform.position, Quaternion.identity, transform);
                }
            }
        }
        else
        {

        }
        RoadPosCheck();

        switch (monster.statue)
        {
            case en_MonsterSta.Run:
                switch (runStatue)
                {
                    case 0:
                        targetPos = gameMain.players_Obj.transform.position + gameMain.players_Obj.transform.forward * monster.readyAttackDistance;
                        if (Vector3.Angle(gameMain.players_Obj.transform.forward, transform.position - gameMain.players_Obj.transform.position) < 10 ||
                            Vector3.Distance(transform.position, targetPos) < 4)
                        {
                            runStatue = 1;
                        }
                        break;
                    case 1:
                        targetPos = gameMain.players_Obj.transform.position;
                        targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                        distance = Vector3.Distance(transform.position, targetPos);
                        if (distance < monster.minAttackDistance)
                        {
                            ChangeStatue(en_MonsterSta.RunOut);
                            break;
                        }
                        if (distance < monster.maxAttackDistance && Vector3.Angle(transform.forward, targetPos - transform.position) < 5)
                        {
                            ChangeStatue(en_MonsterSta.Attack);
                        }
                        break;
                }
                targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                RunCheck(targetPos);
                break;
            case en_MonsterSta.Attack:
                runTime += Time.deltaTime;

                if (monster.animEvent.event_AttackEffect)
                {
                    monster.animEvent.event_AttackEffect = false;
                    if (shell != null)
                    {
                        shell.transform.SetParent(null);
                        Rigidbody rigidbody = shell.gameObject.AddComponent<Rigidbody>();
                        rigidbody.mass = 2;
                        rigidbody.velocity = monster.firePos_Obj.transform.forward * 5;
                        shell.Fire();
                    }
                }
                if (monster.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 || runTime >= 5)
                {
                    ChangeStatue(en_MonsterSta.RunOut);
                }
                //
                targetPos = gameMain.players_Obj.transform.position;
                targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                RunCheck(targetPos);
                break;
            case en_MonsterSta.RunOut:
                RunCheck(currMoveTargetPos);
                break;
        }
    }

    void ChangeStatue(en_MonsterSta sta)
    {
        monsterStatue = sta;
        runTime = 0;

        switch (monsterStatue)
        {
            case en_MonsterSta.Run:
                runStatue = 0;
                break;
            case en_MonsterSta.Attack:
                GameObject obj = GameObject.Instantiate(monster.shell_Prefab, monster.firePos_Obj.transform, transform);
                obj.transform.localPosition = new Vector3(0, 0, 0);
                obj.transform.localEulerAngles = new Vector3(0, 0, 0);
                shell = obj.GetComponent<Game00_Shell_Monster>();
                shell.Init(gameMain, monster, null);
                break;
            case en_MonsterSta.RunOut:
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
            if (roadPosIndex < monster.roadPos.Length)
            {
                UpdateRoadPos();
            }
        }
    }
    void RoadPosCheck()
    {
        if (roadPosIndex >= monster.roadPos.Length)
            return;
        if (Vector3.Distance(transform.position, currMoveTargetPos) < 0.3f || Vector3.Angle(monster.roadPos[roadPosIndex].forward, transform.position - monster.roadPos[roadPosIndex].position) < 80)
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
