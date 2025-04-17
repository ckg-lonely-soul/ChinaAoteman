using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public enum en_Game00_Sta
{
    Ready = 0,
    Play,           // 固定点
    Move,               // 移动(到下一个点)    
    Boss,
    End,
    Pass,
    Out,
    OutEnd,
}

public class Game00_Main : MonoBehaviour
{
    const float DEFAULT_MOVE_SPEED = 20f;
    const int MAX_MONSTER = 20;

    public Zhcr zhcr;
    public Game00_GameUI gameUI;
    public GameObject shakeMain_Obj;
    public NavMeshAgent navMeshAgent_MainCar;
    public NavMeshObstacle navMeshObstacle_MainCar;
    public GameObject players_Obj;
    public Game00_Player[] player;
    public GameObject road_Obj;
    public GameObject bossRoad_Obj;
    public GameObject nearAttackGroup;
    // Prefab
    public GameObject[] monster_Prefab;
    public GameObject shell_SanDan_Prefab;
    public GameObject shell_DaoDan_Prefab;
    public GameObject decBlood_Crit_Prefab;     // 扣血(爆击)
    public GameObject decBlood_Nor_Prefab;      // 扣血(普通)
    public GameObject num_GetScore_Prefab;      // 得分
    public GameObject openBox_Prefab;
    public GameObject[] daoJu_Prefab;           // 道具:
    public GameObject testObj_Prefab;
    public int index;

    // 声音
    public AudioSource audioSource_BackG;       // 背景音乐
    public AudioSource audioSource_BossWarning; //
    public AudioSource audioSource_GamePass;    //
    public AudioClip audioClip_Fire;            // (枪王)枪开火声音
    public int maxMonsterNum = 7;               // 当前怪最大个数(自由出怪)
    public int maxFarMonsterNum = 7;            // 远程怪最大个数
    public bool runOnBackground = false;        // 是否在地面走: 是：目标时，加上身高

    Main main;
    public Game00_RoadPos[] roadPos;
    Game00_RoadPos[] bossRoadPos;
    public Game00_MonsterTempPos[] monsterNearAttackPos;
    public Game00_MonsterTempPos[] monsterHidePos;
    int maxRoadPos;
    public int roadPosIndex;
    public int bossRoadPosIndex;
    Vector3 moveTargetPos;
    public Vector3 lookAtOffsetPos;
    public GameObject lookAtPosObj;
    //
    public Game97_GameSel game97_GameSel;
    //
    public static float ScreenWidth3D = 0.616f;
    public static float ScreenHeight3D = 0.348f;

    public en_Game00_Sta statue;
    // 当前出移动点信息
    public int monsterTotalNum;         // 当前点出怪总数
    public int monsterRemainNum;        // 当前剩余怪个数
    public int monsterAliveNum;         // 当前还活着的怪的个数
    public int monsterNum;              // 当前怪个数
    public int farMonsterNum;           // 远程怪个数

    public float runTime = 0;
    public float attackedDcTime = 0;            // 被攻击冷却时间
    public float delayTime;                 //（在刷怪点）延尺停留时间
    public float stopTime;                  //（在刷怪点）停留时间
    public float stopTimeOut;
    float rotateSpeed = 180;                // 当前点相机旋转速度 
    public float runStopTime = 0;
    float navTime = 0;
    float bossWarningTime;
    float bossWarningCnt;
    float colora;
    int playerNum = Main.MAX_PLAYER;


    readonly int[] tab_MonsterBlood_00 = { 3000, 130, 130, 130, 130 };//每个多加了个0

    public int[] tab_MonsterBlood;
    // 分数
    readonly int[] tab_MonsterScore_00 = { 500, 20, 20, 20, 20};

    public int[] tab_MonsterScore;
    // 攻击值
    readonly int[] tab_MonsterAttack_00 = { 30, 10, 10, 10, 10};
    
    public int[] tab_MonsterAttack;


    public en_MonsterAttackDistanceType[] tab_MonsterAttackDistanceType;

    public List<Game00_Monster> list_Monster = new List<Game00_Monster>();
    Game00_Monster monsterBoss;
    [HideInInspector]
    public Game00_Monster monsterSmallBoss;


#if UNITY_EDITOR

#endif

    public void Awake0(Main mainn)
    {
        main = mainn;
        for (int i = 0; i < player.Length; i++)
        {
            player[i].Awake0(this, i);
        }
        tab_MonsterAttackDistanceType = new en_MonsterAttackDistanceType[monster_Prefab.Length];
        for (int i = 0; i < tab_MonsterAttackDistanceType.Length; i++)
        {
            tab_MonsterAttackDistanceType[i] = monster_Prefab[i].GetComponent<Game00_Monster>().attackDistanceType;
        }
        // 加载怪
        roadPos = road_Obj.GetComponentsInChildren<Game00_RoadPos>();
        bossRoadPos = bossRoad_Obj.GetComponentsInChildren<Game00_RoadPos>();
        maxRoadPos = roadPos.Length;
        for (int i = 0; i < roadPos.Length; i++)
        {
            roadPos[i].Init(this);
        }
        for (int i = 0; i < bossRoadPos.Length; i++)
        {
            bossRoadPos[i].Init(this);
        }
        monsterNearAttackPos = nearAttackGroup.GetComponentsInChildren<Game00_MonsterTempPos>();

#if UNITY_EDITOR
        Main.Log("maxRoadPos: " + maxRoadPos);
#endif

    }

    public void GameStart()
    {
#if IO_LOCAL
        IO.MotorOld_Out(true);
        audioSource_BackG.volume = 0.6f;
        audioSource_BossWarning.volume = 0.6f;
#endif
#if UNITY_EDITOR
        Debug.Log("statue:" + Main.statue);
#endif
        playerNum = Main.MAX_PLAYER;
        if (Set.setVal.PlayerMode == (int)en_PlayerMode.One)
        {
            playerNum = 1;
        }
        audioSource_BackG.Stop();
        switch (Main.statue)
        {
            case en_MainStatue.Game_00:
                tab_MonsterBlood = tab_MonsterBlood_00;
                tab_MonsterScore = tab_MonsterScore_00;
                tab_MonsterAttack = tab_MonsterAttack_00;
                tab_FreshDcTime = tab_FreshDcTime_00;
                break;
        }

        gameUI.GameStart();

        if (Set.setVal.PlayerMode == (int)en_PlayerMode.One)
        {
            player[0].transform.localPosition = new Vector3(0, player[0].transform.localPosition.y, 0);
            for (int i = 1; i < player.Length; i++)
            {
                player[i].gameObject.SetActive(false);
                player[i].playerUI.gameObject.SetActive(false);
                player[i].playerUI.image_Cursor.gameObject.SetActive(false);
            }
        }
        else
        {
            player[0].transform.localPosition = new Vector3(-0.25f, player[0].transform.localPosition.y, 0);
            for (int i = 1; i < player.Length; i++)
            {
                player[i].gameObject.SetActive(true);
                player[i].playerUI.gameObject.SetActive(true);

            }
        }

        roadPosIndex = 0;
#if UNITY_EDITOR
        roadPosIndex = index;
#endif
        navMeshAgent_MainCar.enabled = false;
        navMeshObstacle_MainCar.enabled = false;
        navMeshAgent_MainCar.transform.position = roadPos[roadPosIndex].transform.position;
        navMeshAgent_MainCar.transform.eulerAngles = roadPos[roadPosIndex].transform.eulerAngles;
        navMeshAgent_MainCar.enabled = true;

        for (int i = 0; i < player.Length; i++)
        {
            if (FjData.g_Fj[i].Blood > 0)
            {
                player[i].GameStart(en_StartType.NextGame);
            }
            else
            {

                player[i].GameStart(en_StartType.Reset);        //第一次开始
            }
        }
        for (int i = 0; i < roadPos.Length; i++)
        {
            roadPos[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < daoJuDcTime.Length; i++)
        {
            daoJuDcTime[i] = 40;
        }

        if (runOnBackground)
        {
            lookAtOffsetPos = players_Obj.transform.localPosition;
        }
        else
        {
            lookAtOffsetPos = Vector3.zero;
        }
        // 清怪：
        list_Monster.Clear();
        monsterBoss = null;
        monsterSmallBoss = null;
        gameUI.boss_HP_Obj.SetActive(false);

        monsterNum = 0;
        monsterRemainNum = 0;
        stopTime = 0;
        delayTime = 0;
        runStopTime = 0;
        attackedDcTime = 0;
        bossWarningTime = 0;
        bossWarningCnt = 0;
        UpdateRoadPosInfo();

        ShakeStop();
        DamageStop();
        bossRoad_Obj.SetActive(false);
        ChangeStatue(en_Game00_Sta.Ready);
    }

    void Update()
    {
        ShakeRun(); //抖动
        //
        DamageRun();
        //
        FreshMonsterControl();
        DaoJu_Run();
        if (attackedDcTime > 0)
        {
            attackedDcTime -= Time.deltaTime;
        }
        if (runStopTime > 0)
        {
            runStopTime -= Time.deltaTime;
        }
        if (navTime > 0)
        {
            navTime -= Time.deltaTime;
        }

        if (monsterBoss != null && bossWarningCnt < 5)
        {
            if (bossWarningTime == 0)
            {
                gameUI.image_BossWarning.gameObject.SetActive(true);
            }
            bossWarningTime += Time.deltaTime * 3;
            if (bossWarningTime >= 3.14f * 2)
            {
                bossWarningTime = 0;
                bossWarningCnt++;
                audioSource_BossWarning.Play();
            }
            colora = 0.5f - Mathf.Cos(bossWarningTime) * 0.5f;
            gameUI.image_BossWarning.color = new Color(1, 1, 1, colora);
            if (gameUI.image_BossWarning2 != null)
                gameUI.image_BossWarning2.color = new Color(1, 1, 1, colora);
            if (bossWarningCnt >= 5)
            {
                gameUI.image_BossWarning.gameObject.SetActive(false);
            }
        }

        // 所有玩家结束
        if (statue < en_Game00_Sta.End)
        {
            if (IsAllPlayerEnd())
            {
                Main.gameOver = true;
                ChangeStatue(en_Game00_Sta.Out);
            }
        }

        switch (statue)
        {
            case en_Game00_Sta.Ready:
                runTime += Time.deltaTime;
                if (runTime >= 0.5f)
                {
                    ChangeStatue(en_Game00_Sta.Play);
                }
                break;


            case en_Game00_Sta.Play:
                if (!audioSource_BackG.isPlaying)//播放背景音乐
                {
#if IO_LOCAL//海燕枪神一体机播放儿歌
                    if (Set.setVal.ChildrenSong == 0)
                    {
                        audioSource_BackG.clip = SoundManager.Instance.levelBgm[Random.Range(0, SoundManager.Instance.levelBgm.Length)];
                    }
                    else
                    {
                        if (Set.setVal.Language == (int)en_Language.Chinese)
                        {
                            audioSource_BackG.clip = SoundManager.Instance.gunGodBgmCN[Random.Range(0, SoundManager.Instance.gunGodBgmCN.Length)];
                        }
                        else
                        {
                            audioSource_BackG.clip = SoundManager.Instance.gunGodBgmEN[Random.Range(0, SoundManager.Instance.gunGodBgmEN.Length)];
                        }
                    }
#else//其他不播放儿歌
                    audioSource_BackG.clip = SoundManager.Instance.levelBgm[Random.Range(0, SoundManager.Instance.levelBgm.Length)];
#endif
                    audioSource_BackG.Stop();
                    audioSource_BackG.Play();
                }
                StopCheck();
                // 看目标
                LookAtTarget();

                if (monsterBoss != null)
                {
                    ChangeStatue(en_Game00_Sta.Boss);
                    break;
                }
                if (delayTime > 0)
                {
                    delayTime -= Time.deltaTime;
                    break;
                }
                if (stopTimeOut > 0)
                {
                    stopTimeOut -= Time.deltaTime;
                }
                else
                {
                    ChangeStatue(en_Game00_Sta.Move);
                    break;
                }
                if (monsterSmallBoss != null)
                {
                    break;
                }
                if (roadPos[roadPosIndex].minMonsterNum > 0)
                {
                    if (monsterAliveNum <= roadPos[roadPosIndex].minMonsterNum)
                    {
                        ChangeStatue(en_Game00_Sta.Move);
                    }
                    break;
                }
                if (roadPos[roadPosIndex].stopTime > 0)
                {
                    runTime += Time.deltaTime;
                    if (runTime >= roadPos[roadPosIndex].stopTime)
                    {
                        ChangeStatue(en_Game00_Sta.Move);
                        break;
                    }
                }
                if (roadPos[roadPosIndex].killAllMonster)
                {
                    if (monsterAliveNum > 0)
                    {
                        runTime = 0;
                    }
                    else
                    {
                        bool canMove = true;

                        //给每一个freshPos是否还有残留的怪物没刷出来做检测
                        for (int i = 0; i < roadPos[roadPosIndex].freshPos.Length; i++)
                        {
                            if (roadPos[roadPosIndex].freshPos[i].freshNum <
                                roadPos[roadPosIndex].freshPos[i].totalMonsterNum)
                            {
                                canMove = false;
                                runTime = 0;
                                break;
                            }
                        }
                        if (canMove)
                            runTime += Time.deltaTime;
                    }
                    if (runTime < 0.5f)
                    {
                        break;
                    }
                }
                else if (roadPos[roadPosIndex].monsterTotalNum > 0)
                {
                    if (monsterRemainNum > 0)
                    {
                        break;
                    }
                }
                // 下一站
                ChangeStatue(en_Game00_Sta.Move);
                break;


            case en_Game00_Sta.Move:
                if (navMeshAgent_MainCar.velocity.magnitude > 3)
                {
                    zhcr.ChangeStatue(Zhcr_Sta.Run);
                }

                if (monsterBoss != null && monsterBoss.statue == en_MonsterSta.Die)
                {
                    ChangeStatue(en_Game00_Sta.Boss);
                    break;
                }
                if (roadPos[roadPosIndex].monsterTotalNum > 0 && navMeshAgent_MainCar.speed != DEFAULT_MOVE_SPEED)
                {
                    if (monsterAliveNum == 0)
                    {
                        runTime += Time.deltaTime;
                        if (runTime >= 0.3f)
                        {
                            navMeshAgent_MainCar.speed = DEFAULT_MOVE_SPEED;
                        }
                    }
                    else
                    {
                        runTime = 0;
                    }
                }
                if (runStopTime > 0)
                {
                    StopCheck();
                    break;
                }

                if (Vector3.Distance(navMeshAgent_MainCar.transform.position, moveTargetPos) < 0.5f)
                {
                    if (roadPosIndex + 1 >= roadPos.Length || monsterSmallBoss != null)
                    {
                        ChangeStatue(en_Game00_Sta.Play);
                        break;
                    }

                    //直接移向下一个点
                    if (roadPos[roadPosIndex].nextPosImmediately)
                    {
                        NextRoadPos();
                        break;
                    }
                    if (roadPos[roadPosIndex].killAllMonster && monsterNum == 0 &&
                        roadPos[roadPosIndex].delayTime <= 0)
                    {

                        NextRoadPos();
                        break;
                    }
                    if (roadPos[roadPosIndex].killAllMonster || roadPos[roadPosIndex].stopTime > 0 || roadPos[roadPosIndex].delayTime > 0 || roadPos[roadPosIndex].monsterTotalNum > 0)
                    {
                        ChangeStatue(en_Game00_Sta.Play);
                    }
                    else
                    {
                        NextRoadPos();
                    }
                }
                else
                {
                    MonsterNearCheck(3.5f);
                    RunCheck(moveTargetPos);
                }
                // 看目标
                LookAtTarget();
                break;

            case en_Game00_Sta.Boss:
                LookAtTarget();
                StopCheck();

                if (monsterBoss == null || monsterBoss.statue == en_MonsterSta.Die)
                {
                    // BOSS死亡，小怪全死
                    for (int i = 0; i < list_Monster.Count; i++)
                    {
                        if (list_Monster[i] != null)
                        {
                            if (list_Monster[i].statue != en_MonsterSta.Die)
                            {
                                list_Monster[i].Attacked(null, 1000000, false);
                            }
                        }
                    }
                    ChangeStatue(en_Game00_Sta.End);
                }

                #region bossRoad不会让玩家沿着路径点移动，这里加
                //bossRoad不会让玩家沿着路径点移动，这里加

                if (delayTime > 0)
                {
                    delayTime -= Time.deltaTime;
                    break;
                }

                if (stopTimeOut > 0)
                {
                    stopTimeOut -= Time.deltaTime;
                }
                else
                {
                    ChangeStatue(en_Game00_Sta.Move); //stopTimeOut不会作用让玩家移动到下一个路径点
                    break;
                }
                if (monsterSmallBoss != null)
                {
                    break;
                }

                //minMonsterNum为最多留下的怪才能走
                if (roadPos[roadPosIndex].minMonsterNum > 0)
                {
                    if (monsterAliveNum <= roadPos[roadPosIndex].minMonsterNum)
                    {
                        ChangeStatue(en_Game00_Sta.Move);
                    }
                    break;
                }
                if (roadPos[roadPosIndex].stopTime > 0)
                {
                    runTime += Time.deltaTime;
                    if (runTime >= roadPos[roadPosIndex].stopTime)
                    {
                        ChangeStatue(en_Game00_Sta.Move);
                        break;
                    }
                }
                if (roadPos[roadPosIndex].killAllMonster)
                {
                    if (monsterAliveNum > 0)
                    {
                        runTime = 0;
                    }
                    else
                    {
                        runTime += Time.deltaTime;
                    }
                    if (runTime < 0.5f)
                    {
                        break;
                    }
                }
                else if (roadPos[roadPosIndex].monsterTotalNum > 0)
                {
                    if (monsterRemainNum > 0)
                    {
                        break;
                    }
                }
                // 下一站
                ChangeStatue(en_Game00_Sta.Move);
                #endregion
                break;
            case en_Game00_Sta.End:
                runTime += Time.deltaTime;
                if (runTime >= 3)
                {
                    int no = Main.IndexOfArray(Main.tab_GameId, (int)Main.statue);
                    if (no >= 0)
                    {
                        Main.gamePassed[no] = true;    // 游戏过关
                    }
                    ChangeStatue(en_Game00_Sta.Pass);
                }
                break;

            case en_Game00_Sta.Pass:
                runTime += Time.deltaTime;
                if (runTime >= 8)
                {
                    int no = Main.IndexOfArray(Main.tab_GameId, (int)Main.statue);
                    if (no >= 0)
                    {
                        Main.gamePassed[no] = true;    // 游戏过关
                    }
                    ChangeStatue(en_Game00_Sta.Out);
                }
                break;

            case en_Game00_Sta.Out:

                runTime += Time.deltaTime;
                if (runTime >= 0.3f)
                {
                    ChangeStatue(en_Game00_Sta.OutEnd);
                    main.ChangeScene(en_MainStatue.Game_97);
                }
                break;
        }
        if (nearAttackGroup != null)
        {
            nearAttackGroup.transform.localEulerAngles = new Vector3(0, players_Obj.transform.localEulerAngles.y, 0);
        }
    }

    public void ChangeStatue(en_Game00_Sta sta)
    {

        statue = sta;
        runTime = 0;
        switch (statue)
        {
            case en_Game00_Sta.Ready:
                break;
            case en_Game00_Sta.Play:
                delayTime = roadPos[roadPosIndex].delayTime;
                stopTime = roadPos[roadPosIndex].stopTime;
                stopTimeOut = roadPos[roadPosIndex].stopTimeOut;
                roadPos[roadPosIndex].gameObject.SetActive(true);
                if (stopTimeOut == 0)
                {
                    stopTimeOut = 180;
                }
                if (zhcr != null && zhcr.statue == Zhcr_Sta.Run)
                    zhcr.ChangeStatue(Zhcr_Sta.Idle);
                break;

            case en_Game00_Sta.Move:
                // 下一个点
                NextRoadPos();
                break;

            case en_Game00_Sta.Boss:
                bossRoad_Obj.SetActive(true);
                bossRoadPosIndex = 0;
                break;

            case en_Game00_Sta.End:
                // 关闭刷怪
                road_Obj.SetActive(false);
                bossRoad_Obj.SetActive(false);
                break;

            case en_Game00_Sta.Pass:
                gameUI.image_Pass.gameObject.SetActive(true);
                break;

            case en_Game00_Sta.Out:
                road_Obj.SetActive(false);
                navMeshAgent_MainCar.enabled = false;
                Destroy(navMeshAgent_MainCar);
                ClearMonster();
                break;
        }
    }
    void LookAtTarget()
    {
        // 看目标
        if (lookAtPosObj != null)
        {
            players_Obj.transform.rotation = Quaternion.RotateTowards(players_Obj.transform.rotation, Quaternion.LookRotation(lookAtPosObj.transform.position + lookAtOffsetPos - players_Obj.transform.position), rotateSpeed * Time.deltaTime);
        }
    }
    public void RunStop(float time)
    {
        runStopTime = time;
    }
    public bool IsAttackTime()
    {
        if (attackedDcTime > 0)
            return false;
        attackedDcTime = Random.Range(1.6f, 2.5f);
        return true;
    }
    void RunCheck(Vector3 pos)
    {
        if (navMeshObstacle_MainCar.enabled)
        {
            navMeshObstacle_MainCar.enabled = false;
            navTime = 0.2f;
        }
        else if (navTime <= 0)
        {
            if (navMeshAgent_MainCar.enabled == false)
            {
                navMeshAgent_MainCar.enabled = true;
            }
            if (navMeshAgent_MainCar.SetDestination(pos) == false)
            {
                navMeshAgent_MainCar.SetDestination(navMeshAgent_MainCar.transform.position);
            }
        }
    }
    void StopCheck()
    {
        if (navMeshAgent_MainCar.enabled)
        {
            navMeshAgent_MainCar.SetDestination(navMeshAgent_MainCar.transform.position);
            navMeshAgent_MainCar.enabled = false;
            //if (zhcr != null && zhcr.statue == Zhcr_Sta.Run)
            //    zhcr.ChangeStatue(Zhcr_Sta.Idle);
            navTime = 0.3f;
        }
        else if (navTime <= 0)
        {
            if (navMeshObstacle_MainCar.enabled == false)
            {
                navMeshObstacle_MainCar.enabled = true;
            }
        }
    }

    void NextRoadPos()
    {
        if (roadPosIndex + 1 >= roadPos.Length)
            return;
        runTime = 0;
        // 关闭刷怪,维持5关
        if (roadPosIndex - 4 >= 0)
            roadPos[roadPosIndex - 4].gameObject.SetActive(false);
        // 下一个点
        roadPosIndex++;

        #region 新增代码，nearAttackPos会随路径点的变化偏移y值偏移量

        NearAttackPosController nearAttackPosController = nearAttackGroup?.GetComponent<NearAttackPosController>();

        if (nearAttackPosController != null)
        {
            if (nearAttackPosController.yOffsets.Length != roadPos.Length)
            {
                Debug.LogError("近战攻击位置y的偏移量值与路径点数量不匹配！");
            }
            else  //调整该路径点的近战攻击位置y值
            {
                nearAttackPosController.InitOriginY();
                nearAttackGroup.transform.localPosition +=
                    Vector3.up * nearAttackPosController.yOffsets[roadPosIndex];

            }
        }
        #endregion

        if (roadPosIndex >= maxRoadPos)
        {
            roadPosIndex = 0;
        }
        UpdateRoadPosInfo();
        // 打开刷怪
        roadPos[roadPosIndex].gameObject.SetActive(true);
    }
    void UpdateRoadPosInfo()
    {
        monsterHidePos = roadPos[roadPosIndex].monsterHidePos;
        monsterTotalNum = roadPos[roadPosIndex].monsterTotalNum;
        monsterRemainNum = roadPos[roadPosIndex].monsterTotalNum;
        delayTime = roadPos[roadPosIndex].delayTime;
        lookAtPosObj = roadPos[roadPosIndex].lookAtPosObj;
        rotateSpeed = roadPos[roadPosIndex].rotateSpeed;
        navMeshAgent_MainCar.speed = roadPos[roadPosIndex].moveSpeed;
        moveTargetPos = roadPos[roadPosIndex].transform.position;
    }
    public void AttackPlayers(Game00_Monster mon, int attackvalue)
    {
        for (int i = 0; i < player.Length; i++)
        {
            player[i].Attacked(mon, attackvalue);//attackvalue
        }
        Attacked(attackvalue);
    }
    bool IsAllPlayerEnd()
    {
        for (int i = 0; i < playerNum; i++)
        {
            if (player[i].statue != en_PlayerSta.Idle)
            {
                return false;
            }
        }
        return true;
    }

    public void OneMonsterDie()
    {
        if (monsterRemainNum > 0)
        {
            monsterRemainNum--;
        }
        if (monsterAliveNum > 0)
        {
            monsterAliveNum--;
        }
    }
    void ClearMonster()
    {
        for (int i = 0; i < list_Monster.Count; i++)
        {
            if (list_Monster[i] != null)
            {
                Destroy(list_Monster[i].gameObject);
            }
        }
        list_Monster.Clear();
    }

    // 刷怪控制 ------------------------------------------------
    public void FreshedBoss(Game00_Monster boss)
    {
        //BOSS
        monsterBoss = boss;
        gameUI.boss_HP_Obj.SetActive(true);
    }
    public void Update_BossHP_Pos(Vector3 pos)
    {
        Vector3 screenpos = Camera.main.WorldToScreenPoint(pos);
        gameUI.boss_HP_Obj.transform.position = new Vector3(screenpos.x, screenpos.y, 0);
    }

    public float freshDcTime;
    public static float[] monsterFreshDcTime = new float[MAX_MONSTER];
    public readonly float[] tab_FreshDcTime_00 = { 2, 2, 2, 2, 2 }; 

    public float[] tab_FreshDcTime;
    //清零
    void FreshMonsterControl()
    {
        if (freshDcTime > 0)
        {
            freshDcTime -= Time.deltaTime;
        }
        for (int i = 0; i < monsterFreshDcTime.Length; i++)
        {
            if (monsterFreshDcTime[i] > 0)
            {
                monsterFreshDcTime[i] -= Time.deltaTime;
            }
        }
    }

    // 是否有怪靠得很近
    float monsterNearCheckTime = 0;
    void MonsterNearCheck(float minDis)
    {
        if (monsterNearCheckTime > 0)
        {
            monsterNearCheckTime -= Time.deltaTime;
            return;
        }
        if (navMeshAgent_MainCar == null)
            return;
        float distance;
        for (int i = 0; i < list_Monster.Count; i++)
        {
            if (list_Monster[i] != null)
            {
                if (list_Monster[i].statue == en_MonsterSta.Die)
                    continue;
                if (Vector3.Angle(players_Obj.transform.forward, list_Monster[i].transform.position - navMeshAgent_MainCar.transform.position) > 35f)
                    continue;
                distance = Vector3.Distance(list_Monster[i].transform.position, navMeshAgent_MainCar.transform.position);
                if (distance < minDis)
                {
                    navMeshAgent_MainCar.speed = 0.0f;
                    monsterNearCheckTime = 1f;
                    return;
                }
                else if (distance < minDis + 1f)
                {
                    navMeshAgent_MainCar.speed = 0.3f;
                    monsterNearCheckTime = 0.5f;
                    return;
                }
            }
        }
        navMeshAgent_MainCar.speed = DEFAULT_MOVE_SPEED;
    }

    // 道具管理---------------------------------------------------------------------
    const int DAOJU_ID_BLOOD = 0;
    const int DAOJU_ID_DAODAN = 1;
    const int DAOJU_ID_SANDAN = 2;
    const int DAOJU_ID_BOX = 3;
    const int MAX_DAOJU_ID = 4;
    float[] daoJuDcTime = new float[MAX_DAOJU_ID];
    float daoJuTotalDcTime = 0;
    public void DaoJu_Destroy(en_Game00_DaoJuType daojutype)
    {
        switch (daojutype)
        {
            case en_Game00_DaoJuType.BloodPag:
                daoJuDcTime[DAOJU_ID_BLOOD] = 0;
                break;
            case en_Game00_DaoJuType.DaoDan:
                daoJuDcTime[DAOJU_ID_DAODAN] = 0;
                break;
            case en_Game00_DaoJuType.SanDan:
                daoJuDcTime[DAOJU_ID_SANDAN] = 0;
                break;
            case en_Game00_DaoJuType.Box:
                daoJuDcTime[DAOJU_ID_BOX] = 0;
                break;
        }
    }
    void DaoJu_Run()
    {
        for (int i = 0; i < daoJuDcTime.Length; i++)
        {
            if (daoJuDcTime[i] > 0)
            {
                daoJuDcTime[i] -= Time.deltaTime;
            }
        }
        if (daoJuTotalDcTime > 0)
        {
            daoJuTotalDcTime -= Time.deltaTime;
        }
    }

    public en_Game00_DaoJuType GetDaoJu(Game00_Player tempPlayer)
    {
        if (daoJuTotalDcTime > 0)
        {
            return en_Game00_DaoJuType.None;
        }

        int jl;
        // 掉血包:
        if (daoJuDcTime[DAOJU_ID_BLOOD] <= 0)
        {
            if (FjData.g_Fj[tempPlayer.Id].Blood * 5 < Set.setVal.GameTime)
            {
                jl = 350;
            }
            else
            {
                jl = 50;
            }
            if (Random.Range(0, 1000) < jl)
            {
                daoJuDcTime[DAOJU_ID_BLOOD] = 30;
                daoJuTotalDcTime = 5;
                return en_Game00_DaoJuType.BloodPag;
            }
        }
        // 掉导弹:
        if (daoJuDcTime[DAOJU_ID_DAODAN] <= 0)
        {
            if (monsterAliveNum >= 7)
            {
                jl = 250;
            }
            else
            {
                jl = 150;
            }
            if (Random.Range(0, 1000) < jl)
            {
                daoJuDcTime[DAOJU_ID_DAODAN] = 15;
                daoJuTotalDcTime = 5;
                return en_Game00_DaoJuType.DaoDan;
            }
        }

        // 掉宝箱:
        if (daoJuDcTime[DAOJU_ID_BOX] <= 0)
        {
            jl = JL.GetBoxJlBase();
            if (Random.Range(0, 1000) < jl)
            {
                daoJuDcTime[DAOJU_ID_BOX] = 15;
                daoJuTotalDcTime = 5;
                return en_Game00_DaoJuType.Box;
            }
        }

        return en_Game00_DaoJuType.None;
    }

    // 被攻击效果 ----------------------------------------------
    int damageCnt;
    float damageTime;
    int damageValue = 0;
    public void Attacked(int attackvalue)
    {
        damageValue += attackvalue;
        if (damageValue >= 50)
        {
            damageValue = 0;
            DamageStart();
        }
    }
    public void DamageStart()
    {
        damageCnt = 5;
        gameUI.image_Damage.gameObject.SetActive(true);
    }
    public void DamageStop()
    {
        gameUI.image_Damage.gameObject.SetActive(false);
        damageCnt = 0;
    }
    public void DamageRun()
    {
        if (damageCnt > 0)
        {
            damageTime += Time.deltaTime;
            if (damageTime >= 0.15f)
            {
                damageTime = 0;
                //
                damageCnt--;
                if ((damageCnt % 2) == 0)
                {
                    gameUI.image_Damage.gameObject.SetActive(false);
                }
                else
                {
                    gameUI.image_Damage.gameObject.SetActive(true);
                }
            }
        }
    }
    // 震屏效果 ----------------------------------------------------
    int shakeCnt;
    float shakeTime;
    float shakePower;
    public void ShakeStart(float power, int cnt)
    {
        shakePower = power;
        shakeCnt = cnt;
    }
    public void ShakeStop()
    {
        shakeMain_Obj.transform.localPosition = new Vector3(0, 0, 0);
        shakeCnt = 0;
    }
    public void ShakeRun()
    {
        if (shakeCnt > 0)
        {
            shakeTime += Time.deltaTime;
            if (shakeTime >= 0.05f)
            {
                shakeTime = 0;
                //
                shakeCnt--;
                if (shakeCnt == 0)
                {
                    shakeMain_Obj.transform.localPosition = new Vector3(0, 0, 0);
                }
                else
                {
                    shakeMain_Obj.transform.localPosition = new Vector3(Random.Range(-0.8f, 0.8f), Random.Range(-0.8f, 0.8f), 0) * shakePower;
                }
            }
        }
    }

    bool IsPlayerTarget(Game00_Monster mons)
    {
        for (int i = 0; i < player.Length; i++)
        {
            if (mons == player[i].monsterTarget)
            {
                return true;
            }
        }
        return false;
    }
    public Game00_Monster FindMonster(Game00_Player playerTemp)
    {
        float angle;
        for (int i = 0; i < list_Monster.Count; i++)
        {
            if (list_Monster[i] != null && list_Monster[i].statue != en_MonsterSta.Die)
            {
                if (IsPlayerTarget(list_Monster[i]))
                    continue;
                angle = Vector3.Angle(playerTemp.transform.forward, list_Monster[i].transform.position - playerTemp.transform.position);
                if (angle < 50)
                {
                    return list_Monster[i];
                }
            }
        }
        return null;
    }
    public GameObject GetMonsterNearAttackPos(Game00_Monster tempmonster)
    {
        for (int i = 0; i < monsterNearAttackPos.Length; i++)
        {
            if (monsterNearAttackPos[i].monster == null)
            {
                monsterNearAttackPos[i].monster = tempmonster;
                return monsterNearAttackPos[i].gameObject;
            }
        }
        return null;
    }
    public Game00_MonsterTempPos GetMonsterHidePos(Game00_Monster tempmonster)
    {
        for (int i = 0; i < monsterHidePos.Length; i++)
        {
            if (monsterHidePos[i].monster == null)
            {
                monsterHidePos[i].monster = tempmonster;
                return monsterHidePos[i];
            }
        }
        return null;
    }
#if DEBUG_TEST
    void OnGUI()
    {

    }
#endif

    #region 新增字段、方法
    public SerializableDictionary<Game00_RoadPos, Transform[]> bossFarAttackPosByRoads;
    [Header("处理boss的攻击位置随路径变化")]
    public Game00_RoadPos[] bossRunRoads;
    public Transform[] attackPosEachRoad;

    void InitBossFarAttackPosByRoads()
    {
        bossFarAttackPosByRoads = new SerializableDictionary<Game00_RoadPos, Transform[]>();

        if (bossRunRoads.Length == attackPosEachRoad.Length)
        {
            for (int i = 0; i < bossRunRoads.Length; i++)
            {
                bossFarAttackPosByRoads.Dictionary.Add(bossRunRoads[i],
                    attackPosEachRoad[i].GetComponentsInChildren<Transform>(false)
                    .Where(t => t != attackPosEachRoad[i].transform)
                    .ToArray()); //只收集子对象的激活对象)
            }
        }
        else
        {
            Debug.LogError("bossRunRoads与attackPosEachRoad不匹配");
        }
    }

    private void Start()
    {
        InitBossFarAttackPosByRoads();
    }
    #endregion

}
