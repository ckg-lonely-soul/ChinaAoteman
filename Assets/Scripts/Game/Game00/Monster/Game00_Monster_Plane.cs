using UnityEngine;

public class Game00_Monster_Plane : MonoBehaviour
{

    Game00_Monster monster;
    Game00_Main gameMain;
    GameObject mainCar_Obj;
    GameObject players_Obj;
    en_MonsterSta monsterSta;

    Vector3 targetPos;
    int runStatue;

    // Use this for initialization
    void Start()
    {
        monster = GetComponent<Game00_Monster>();
        gameMain = monster.gameMain;
        mainCar_Obj = monster.mainCar_Obj;
        players_Obj = gameMain.players_Obj;

        monsterSta = monster.statue;

        ChangeStatue(en_MonsterSta.Run);
    }

    // Update is called once per frame
    void Update()
    {
        if (monster == null)
            return;

        if (monsterSta != monster.statue)
        {
            monsterSta = monster.statue;
            switch (monsterSta)
            {
                case en_MonsterSta.Die:
                    if (monster.effect_Die_Prefab != null)
                    {
                        GameObject.Instantiate(monster.effect_Die_Prefab, transform.position, Quaternion.identity);
                    }
                    Destroy(gameObject);
                    break;
            }
        }

        switch (monster.statue)
        {
            case en_MonsterSta.Die:
                break;

            case en_MonsterSta.Run:
                if (runStatue == 0)
                {
                    targetPos = mainCar_Obj.transform.position + players_Obj.transform.forward * 80 + new Vector3(0, 10, 0);
                    if (Vector3.Distance(transform.position, targetPos) < 40 ||
                        Vector3.Angle(players_Obj.transform.forward, transform.position - players_Obj.transform.position) < 20)
                    {
                        runStatue = 1;
                    }
                }
                else
                {
                    targetPos = mainCar_Obj.transform.position + players_Obj.transform.forward * 1 + new Vector3(0, 2, 0);
                    if (Vector3.Distance(transform.position, targetPos) < 20)
                    {
                        targetPos = mainCar_Obj.transform.position - players_Obj.transform.forward * 100 + new Vector3(0, 100, 0);
                        gameMain.ShakeStart(0.4f, 6);
                        if (monster.shell_Prefab != null)
                        {
                            GameObject obj = GameObject.Instantiate(monster.shell_Prefab);
                            obj.transform.position = monster.firePos_Obj.transform.position;
                            obj.transform.eulerAngles = monster.firePos_Obj.transform.eulerAngles;
                            Game00_Shell_Monster shell = obj.GetComponent<Game00_Shell_Monster>();
                            Rigidbody rigidbody = obj.AddComponent<Rigidbody>();
                            rigidbody.mass = 2;
                            rigidbody.velocity = transform.forward * monster.runSpeed / 1.5f;
                            shell.Init(gameMain, monster, null);
                            shell.Fire();
                        }
                        ChangeStatue(en_MonsterSta.RunOut);
                    }
                }
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position), 3.5f * Time.deltaTime);
                transform.Translate(Vector3.forward * monster.runSpeed * Time.deltaTime);
                break;

            case en_MonsterSta.RunOut:
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position), 4.5f * Time.deltaTime);
                transform.Translate(Vector3.forward * monster.runSpeed * Time.deltaTime);
                break;
        }

    }

    void ChangeStatue(en_MonsterSta sta)
    {
        print("Plane: " + sta);
        switch (sta)
        {
            case en_MonsterSta.Idle:
                break;
            case en_MonsterSta.Walk:
                break;
            case en_MonsterSta.Run:
                runStatue = 0;
                break;
            case en_MonsterSta.RunOut:
                monster.playerEnemy = null;
                break;
            case en_MonsterSta.Attack:
                break;
            case en_MonsterSta.Die:
                break;
        }
        monster.ChangeStatue(sta);
    }
}
