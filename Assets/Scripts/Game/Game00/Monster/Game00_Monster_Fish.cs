using UnityEngine;

public class Game00_Monster_Fish : MonoBehaviour
{

    public float walkSpeed = 3.5f;
    public float runSpeed = 8.5f;
    public float shakePower = 0.2f;
    public int shakeCnt = 4;

    Game00_Main gameMain;
    Game00_Monster monster;
    GameObject players_Obj;

    float rotateSpeed;
    float distance;
    float angle;
    Vector3 targetPos;
    int runStatue;
    float runTime;

    // Use this for initialization
    void Start()
    {
        monster = GetComponent<Game00_Monster>();
        gameMain = monster.gameMain;
        players_Obj = gameMain.players_Obj;
    }

    // Update is called once per frame
    void Update()
    {
        if (monster == null)
            return;

        switch (monster.statue)
        {
            case en_MonsterSta.Idle:            // 停下等待
                break;

            case en_MonsterSta.Walk:            // 慢走
                transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
                //
                //monster.CheckFindEnemy();
                //if (monster.playerEnemy != null) {
                //    runStatue = 0;
                //    monster.ChangeStatue(en_MonsterSta.Run);
                //}

                distance = Vector3.Distance(transform.position, players_Obj.transform.position);
                angle = Vector3.Angle(transform.position - players_Obj.transform.position, players_Obj.transform.forward);
                if (distance >= monster.minAttackDistance && distance <= monster.autoAttackSize && angle < 35)
                {
                    if (gameMain.IsAttackTime())
                    {
                        monster.ChangeStatue(en_MonsterSta.Run);
                    }
                }
                break;

            case en_MonsterSta.Run:             // 攻击中快走
                distance = Vector3.Distance(transform.position, players_Obj.transform.position);
                angle = Vector3.Angle(transform.position - players_Obj.transform.position, players_Obj.transform.forward);
                //
                if (runStatue == 0)
                {
                    // 进入攻击范围
                    targetPos = players_Obj.transform.position + players_Obj.transform.forward * monster.maxAttackDistance * 2;
                    rotateSpeed = 5;
                    if (angle <= 20 && distance >= monster.maxAttackDistance)
                    {
                        runStatue = 1;
                    }
                }
                else if (runStatue == 1)
                {
                    // 冲向目标
                    targetPos = players_Obj.transform.position;
                    rotateSpeed = 10;
                    if (angle > 45)
                    {
                        runStatue = 2;      // 放弃攻击，游向一边
                        //    monster.playerEnemy.monsterEnemy = null;
                    }
                    else if (distance <= monster.minAttackDistance + 1.8f)
                    {
                        runTime = 0;
                        monster.ChangeStatue(en_MonsterSta.Attack);
                        break;
                    }
                }
                else
                {
                    // 放弃
                    monster.ChangeStatue(en_MonsterSta.RunOut);
                }
                // 移动
                transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
                // 转向
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position), rotateSpeed * Time.deltaTime);
                break;

            case en_MonsterSta.RunOut:
                distance = Vector3.Distance(transform.position, players_Obj.transform.position);
                targetPos = transform.position + (transform.position - players_Obj.transform.position) - players_Obj.transform.forward * distance;
                targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                // 移动
                transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
                // 转向
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position), rotateSpeed * Time.deltaTime);

                angle = Vector3.Angle(transform.position - players_Obj.transform.position, players_Obj.transform.forward);
                if (angle > 70)
                {
                    //    monster.playerEnemy = null;
                    monster.ChangeStatue(en_MonsterSta.Walk);
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
                    //monster.playerEnemy.Attacked(monster, monster.attackValue);
                    //    monster.playerEnemy.monsterEnemy = null;
                    monster.gameMain.RunStop(0.7f);
                }
                runTime += Time.deltaTime;
                if (runTime >= 0.5f)
                {
                    runTime = 0;
                    monster.ChangeStatue(en_MonsterSta.RunOut);
                }
                break;
        }
    }
}
