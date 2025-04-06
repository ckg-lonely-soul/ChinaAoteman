using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum en_ShellType
{
    Normal = 0,
    SanDan,
    DaoDan,
    PaoDan,         // 炮弹
    Buttle,         // 子弹
    Bomb,           // 炸弹/手雷
    Sword,          //道具 Game04BOSS用
    FarmTool ,      //农具 Game01怪用
    Arrow,          //Game11使用
    Bullet,         //Game07使用
    FireBall,       //Game05使用
    FireBallDown,
    tordano,
    Sword_16,
    longjuanfeng
    //Catapult
}

public enum en_ShellSta
{
    Idle = 0,
    Run,
    Die,
}


public class Game00_Shell : MonoBehaviour
{
    public GameObject dieEff_Prefab;        // 爆炸
    public en_ShellType shellType;
    public float speed = 40;
    Game00_Main gameMain;
    Game00_Player player;

    Vector3 targetPos;
    float moveDistance;
    int attackValue = 10;
    RaycastHit hit;
    public LayerMask layerMask;// = LayerMask.GetMask("Shell");

    public void Init(Game00_Player playerFun, Vector3 targetpos, int attackvalue)
    {
        player = playerFun;
        gameMain = player.gameMain;
        targetPos = targetpos;
        attackValue = attackvalue;
    }

    void Update()
    {
        moveDistance = speed * Time.deltaTime;

        switch (shellType)
        {
            case en_ShellType.Normal:
                if (Physics.Raycast(transform.position, transform.forward, out hit, moveDistance))
                { //, layerMask    (1 << LayerMask.NameToLayer("Shell"))
                    transform.position = hit.point;
                    moveDistance = 0;
                    OnTriggerEnter(hit.collider);
                    return;
                }
                break;

            case en_ShellType.DaoDan:
                if (Vector3.Angle(transform.forward, targetPos - transform.position) < 90)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position), 5 * Time.deltaTime);
                }
                if (Physics.Raycast(transform.position, transform.forward, out hit, moveDistance))
                { //, layerMask    (1 << LayerMask.NameToLayer("Shell"))
                    transform.position = hit.point;
                    moveDistance = 0;
                    OnTriggerEnter(hit.collider);
                    return;
                }
                break;
        }
        transform.Translate(Vector3.forward * moveDistance);
    }

    void OnTriggerEnter(Collider collider)
    {
        bool crit = false;
        switch (shellType)
        {
            case en_ShellType.Normal:
            case en_ShellType.SanDan:
                if (collider.transform.root.tag == "Monster")
                {
                    Game00_Monster monster = collider.transform.root.GetComponent<Game00_Monster>();
                    if (monster != null)
                    {
                        if (Random.Range(0, 1000) < 50)
                        {
                            crit = true;
                        }
                        monster.Attacked(player, attackValue, crit);
                    }
                    break;
                }
                if (collider.transform.root.tag == "DaoJu")
                {
                    Game00_DaoJu daoju = collider.transform.root.GetComponent<Game00_DaoJu>();
                    if (daoju != null)
                    {
                        daoju.Attacked(player);
                    }
                }
                break;

            case en_ShellType.DaoDan:
                gameMain.ShakeStart(1f, 8);
                for (int i = 0; i < gameMain.list_Monster.Count; i++)
                {
                    if (gameMain.list_Monster[i] != null)
                    {
                        if (Vector3.Distance(transform.position, gameMain.list_Monster[i].transform.position) < 8)
                        {
                            if (Random.Range(0, 1000) < 50)
                            {
                                crit = true;
                            }
                            else
                            {
                                crit = false;
                            }
                            gameMain.list_Monster[i].Attacked(player, attackValue, crit);
                        }
                    }
                }
                Destroy(gameObject);
                break;
        }
        if (dieEff_Prefab != null)
        {
            GameObject obj = GameObject.Instantiate(dieEff_Prefab);
            obj.transform.position = transform.position;
            obj.transform.eulerAngles = Camera.main.transform.eulerAngles;
#if IO_LOCAL
            obj.GetComponent<AudioSource>().volume = 0.6f;
#endif
        }
    }
}
