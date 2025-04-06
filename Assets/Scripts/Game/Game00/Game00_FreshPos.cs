using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum en_FreshPosSta
{

}

public class Game00_FreshPos : MonoBehaviour
{
    public GameObject monsterRoadPosGroup;
    public bool isJumpDown = false;             // 是否沿路径跳下去
    public int[] monsterId;
    public int totalMonsterNum = 0;             // 当前点出怪总数
    public bool OnlyOneMonster = false;         // 当前点只刷一个怪：当前怪消失后，才刷第二个
    public bool freshImmediatelyFirst = false;       // 立刻刷新(不考虑其它条件)
    public float curPosFreshDcTime = 4;     //怪冷却时间
    public float delayTime = 0;
    public GameObject[] attackPos;                // 攻击点
    public GameObject hidePos;                  // 隐藏点



    Game00_Main gameMain;
    public Game00_Monster currMonster;
    Transform[] monsterRoadPos;
    //控制怪物能否攻击的开关
    public bool newMonsterCanAttack = true;

    // 公用数据
    static float[] monsterFreshDctime = new float[20];

    // 本地变量
    int Id = -1;
    public int freshNum = 0;
    float freshDcTime;

    public void Init(Game00_Main game, int no)
    {
        gameMain = game;
        Id = no;
        freshNum = 0;

        if (monsterRoadPos == null && monsterRoadPosGroup != null)
        {
            //monsterRoadPos = monsterRoadPosGroup.GetComponentsInChildren<Transform>();
            monsterRoadPos = monsterRoadPosGroup.GetComponentsInChildren<Transform>(false)
                .Where(t => t != monsterRoadPosGroup.transform)
                .ToArray(); //只收集子对象的激活对象
        }
        for (int i = 0; i < monsterFreshDctime.Length; i++)
        {
            monsterFreshDctime[i] = Random.Range(0.2f, 0.8f);
        }
        freshDcTime = Random.Range(0.2f, 2.8f);
    }

    // Update is called once per frame
    void Update()
    {
        //return;
        if (delayTime > 0)
        {
            delayTime -= Time.deltaTime;
            return;
        }
        if (freshImmediatelyFirst == false || freshNum > 0)
        {
            if (gameMain.monsterTotalNum > 0)
            {
                if (gameMain.monsterNum >= gameMain.monsterRemainNum)
                    return;
            }
            if (freshDcTime > 0)
            {
                freshDcTime -= Time.deltaTime;
                return;
            }
            if (totalMonsterNum > 0 && freshNum >= totalMonsterNum)
                return;
            if (OnlyOneMonster && currMonster != null)
                return;
            if (gameMain.monsterNum >= gameMain.maxMonsterNum)
                return;
            //if (gameMain.freshDcTime > 0)       //??
            //    return;
        }

        int mid = GetMonsterId();//
        if (mid >= 0)
        {
            // 刷怪
            GameObject obj = GameObject.Instantiate(gameMain.monster_Prefab[mid]);
            obj.name = gameObject.name;
            obj.transform.position = transform.position;
            obj.transform.eulerAngles = transform.eulerAngles;
            Game00_Monster monster = obj.GetComponent<Game00_Monster>();
            // 近程兵
            //gameMain.tab_MonsterAttackDistanceType[mid] == en_MonsterAttackDistanceType.Near
            if (monster.attackDistanceType == en_MonsterAttackDistanceType.Near)
            {
                monster.attackPos = gameMain.GetMonsterNearAttackPos(monster);
                if (monster.attackPos == null)
                {
                    Debug.Log("Dot Get attackPos");
                }
            }
            else
            {
                monster.attackPosArray = attackPos;
                monster.hidePos = hidePos;
            }
            //Debug.Log (100001);
            monster.Init(gameMain, gameMain.tab_MonsterBlood[mid], gameMain.tab_MonsterScore[mid], gameMain.tab_MonsterAttack[mid]);
            monster.SetRoadPos(monsterRoadPos);
            if (isJumpDown && monsterRoadPosGroup != null)
            {
                monster.ChangeStatue(en_MonsterSta.DownCar);
            }
            //更改怪物能否攻击
            monster.isAttack = newMonsterCanAttack;
            gameMain.list_Monster.Add(monster);
            gameMain.monsterNum++;
            gameMain.monsterAliveNum++;
            currMonster = monster;
            //
            freshDcTime = curPosFreshDcTime;
            gameMain.freshDcTime = 0.5f;//??
            Game00_Main.monsterFreshDcTime[mid] = gameMain.tab_FreshDcTime[mid];

            freshNum++;
        }
    }

    int[] idBuf = new int[Main.MAX_MONSTER];
    int GetMonsterId()
    {
        int mid;
        int len = 0;
        for (int i = 0; i < monsterId.Length; i++)
        {
            //Debug.Log (Main.MAX_MONSTER);
            mid = monsterId[i];
            if (mid >= gameMain.monster_Prefab.Length)
                continue;
            if (freshImmediatelyFirst == false || freshNum > 0)
            {
                if (Game00_Main.monsterFreshDcTime[mid] > 0)
                    continue;
                //??
                //if (gameMain.tab_MonsterAttackDistanceType[mid] == en_MonsterAttackDistanceType.Near) {
                //    if (gameMain.GetMonsterNearAttackPos(null) == null)
                //        continue;
                //}
                //if (gameMain.tab_MonsterAttackDistanceType[mid] == en_MonsterAttackDistanceType.Far)
                //{
                //    if (gameMain.farMonsterNum >= gameMain.maxFarMonsterNum)
                //        continue;
                //}
                //
            }
            idBuf[len] = i;
            len++;
        }
        if (len > 0)
        {
            mid = idBuf[Random.Range(0, len)];
            return monsterId[mid];
        }
        return -1;
    }
}
