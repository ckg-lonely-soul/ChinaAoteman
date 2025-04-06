using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum en_Game00_DaoJuType
{
    None = -1,          // 没有
    BloodPag = 0,       // 血量包
    DaoDan,             // 导弹
    SanDan,             // 散弹
    Box,                // 宝箱
}

public class Game00_DaoJu : MonoBehaviour
{
    public GameObject model_Obj;
    public en_Game00_DaoJuType type = en_Game00_DaoJuType.BloodPag;
    public bool isOnScene;
    Game00_Player player;
    Game00_Main gameMain;
    Game00_Monster monsterCanUse;
    float runTime;

    public void Init(Game00_Main game00_Main)
    {
        gameMain = game00_Main;
    }

    void Start()
    {
        runTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (model_Obj != null)
        {
            model_Obj.transform.Rotate(0, 90 * Time.deltaTime, 0);
        }

        runTime += Time.deltaTime;
        if (isOnScene)
            return;
        if (runTime >= 10)
        {
            gameMain.DaoJu_Destroy(type);
            Destroy(gameObject);
        }
    }

    public void Attacked(Game00_Player playerFun)
    {
        if (player != null)
            return;
        if (runTime < 1.1f)
            return;
        player = playerFun;
        gameMain = player.gameMain;
        player.GetDaoJu(type);
        Destroy(gameObject, 5);
        gameObject.SetActive(false);
    }

    public void SetMonsterCanUse(Game00_Monster monster)
    {
        monsterCanUse = monster;
    }

    public void MonsterUse()
    {
        if (monsterCanUse == null)
        {
            Debug.Log("没有怪物能够使用该道具！");
            return;
        }
        if (runTime < 1.1f)
            return;
        gameMain = monsterCanUse.gameMain;
        monsterCanUse.GetDaoJu(type);
        Destroy(gameObject);
    }

    public Game00_Monster GetMonsterCanUse()
    {
        return monsterCanUse;
    }
}
