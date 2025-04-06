using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum en_PlayerSta
{
    Idle = 0,           // 空闲
    Play,               // 游戏中
    Die,                // 死亡
    ContinueTimeout,    // 续玩倒计时
    JoinGame,           // 加入游戏
}

public enum en_StartType
{
    Reset = 0,          // 第一次开始
    Continue,           // 续玩
    NextGame,           // 正常下一关
}

public class Game00_Player : MonoBehaviour
{
    //#if UNITY_EDITOR
    //    const int MAX_PLAYER_BLOOD = 2000;
    //#else
    //    const int MAX_PLAYER_BLOOD = 1000;
    //#endif
    const float MAX_SPECIALATTCK_TIME = 3;

    public Canvas canvas;
    public GameObject arm;              // 手
    public GameObject gunPos;           // 枪位置
    public Game00_PlayerUI playerUI;
    public GameObject[] gun_Prefab;
    public GameObject daoDanFirePos;
    //public GameObject targetPos;

    Sprite[] sprite_ContinueGameStart;

    //[HideInInspector]
    public Game00_Monster monsterEnemy;
    public Game00_Monster monsterTarget;
    Game00_OpenBox openBox;

    public Game00_Main gameMain;
    Game00_Gun currGun;
    public int Id;
    //public int score;
    public int comboNum;
    int openBoxNum;
    int daoDanNum;
    int gunId;

    public en_PlayerSta statue;
    public en_PlayerSta oldStatue;
    en_ShellType shellType;
    float runTime;
    public float fireDcTime;
    float specialAttackTime;
    public float attackedDcTime;       // 被攻击间隔时间
    public float changeBulltTime;       // 换子弹时间
    bool pressedOkContinue = false;
    int timeOut;
    bool[] isEnd = new bool[2];//倒计时结束不续币

    #region 新增字段、方法
    bool canChangeGun = true;
    public void SetCanChangeGun(bool canChangeGun)
    {
        this.canChangeGun = canChangeGun;
    }
    #endregion

    public void Awake0(Game00_Main gmain, int no)
    {
        gameMain = gmain;
        Id = no;

        playerUI.Awake0(this);

        sprite_ContinueGameStart = Resources.LoadAll<Sprite>("Company_00/Common/ContinueGameStart");
        for (int i = 0; i < isEnd.Length; i++)
        {
            isEnd[i] = false;
        }
    }
    public void GameStart(en_StartType type)
    {
        //Debug.Log("Start: " + type);
        switch (type)
        {
            case en_StartType.Reset:
                if(Set.setVal.InOutMode != (int)en_InOutMode.OneInOneOut)
                {
                    if (FjData.g_Fj[Id].Playing == false)
                    {
                        if (Main.DecStartCoin(Id))
                        {
                            FjData.g_Fj[Id].Playing = true;
                            FjData.g_Fj[Id].Played = true;
                        }
                    }
                }
                FjData.g_Fj[Id].Scores = 0;
                FjData.g_Fj[Id].JsScores = 0;
                FjData.g_Fj[Id].Blood = Set.setVal.GameTime;    //MAX_PLAYER_BLOOD;
                FjData.g_Fj[Id].GameTime = Set.setVal.GameTime;
                shellType = en_ShellType.Normal;
                specialAttackTime = 0;
                break;
            case en_StartType.Continue:
                if (FjData.g_Fj[Id].Playing == false)
                {
                    if (Main.DecStartCoin(Id))
                    {
                        FjData.g_Fj[Id].Playing = true;
                        FjData.g_Fj[Id].Played = true;
                    }
                }
                FjData.g_Fj[Id].Blood = Set.setVal.GameTime;    //MAX_PLAYER_BLOOD;
                FjData.g_Fj[Id].GameTime = Set.setVal.GameTime;
                break;
            case en_StartType.NextGame:
                shellType = en_ShellType.Normal;
                specialAttackTime = 0;
                break;
        }
#if UNITY_EDITOR
        //shellType = en_ShellType.DaoDan;
        //specialAttackTime = 5;
#endif
        openBoxNum = 0;
        if (openBox != null)
        {
            Destroy(openBox.gameObject);
        }
        attackedDcTime = 0;
        monsterEnemy = null;
        Update_HP();
        Update_Coins();
        Update_Score();
        //playerUI.Update_Time((int)FjData.acc[Id].gameTime);

        playerUI.GameStart(Id);
        playerUI.remainBulletNum_Obj.gameObject.SetActive(false);
        gunId = 0;
        // Update_Gun(gunId);
        if (FjData.g_Fj[Id].Playing)
        {
            ChangeStatue(en_PlayerSta.Play);
        }
        else
        {
            ChangeStatue(en_PlayerSta.Idle);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // gameMain.DamageStart();
            if (Id == 0)
                GetDaoJu(en_Game00_DaoJuType.DaoDan);
        }
        // 投币检测
        if (Main.coinInChanged[Id])
        {
            Main.coinInChanged[Id] = false;
            Update_Coins();
            // 声音
        }
#if !GUN_HW
        if (attackedDcTime > 0)
        {
            attackedDcTime -= Time.deltaTime;
        }
        if (fireDcTime > 0)
        {
            fireDcTime -= Time.deltaTime;
        }
        else if (Set.setVal.GunMode == (int)en_GunMode.Gun && IO.gunMotorSta[Id])
        {
            IO.GunRunStop(Id);
        }
#endif
        switch (shellType)
        {
            case en_ShellType.DaoDan:
                if (daoDanNum <= 0)
                {
                    shellType = en_ShellType.Normal;
                    playerUI.image_SpecialAttackFlag.gameObject.SetActive(false);
                }
                break;
            case en_ShellType.SanDan:
                if (specialAttackTime > 0)
                {
                    specialAttackTime -= Time.deltaTime;
                    playerUI.Update_SpecialAttackTime(specialAttackTime / MAX_SPECIALATTCK_TIME);
                }
                else
                {
                    shellType = en_ShellType.Normal;
                    playerUI.image_SpecialAttackFlag.gameObject.SetActive(false);
                }
                break;
        }

        switch (statue)
        {
            case en_PlayerSta.Idle:
                if (Main.IsCanGamePlay(Id))
                {
                    if (Set.setVal.GunMode == (int)en_GunMode.ShootWater)
                    {
                        GameStart(en_StartType.Reset);
                    }
                    else
                    {
                        if (isEnd[Id] == false && Key.KEYFJ_OkPressed(Id))
                        {
                            if (Main.DecStartCoin(Id))
                            {
                                FjData.g_Fj[Id].Playing = true;
                                FjData.g_Fj[Id].Played = true;
                                GameStart(en_StartType.Reset);
                            }
                        }
                    }
                }
                break;
            case en_PlayerSta.Play:
                runTime += Time.deltaTime;
                if (runTime >= 1)
                {
                    runTime = 0;
                    if (FjData.g_Fj[Id].GameTime > 0)
                    {
                        FjData.g_Fj[Id].GameTime--;
                    }
                }
                if (openBox != null)
                {
                    break;
                }
                if (openBoxNum > 0)
                {
                    openBoxNum--;
                    GameObject obj = GameObject.Instantiate(gameMain.openBox_Prefab, gameMain.gameUI.transform);
                    obj.transform.localPosition = new Vector3(0, 180, 0);
                    openBox = obj.GetComponent<Game00_OpenBox>();
                    openBox.Init(this);
                    if (Set.setVal.GunMode == (int)en_GunMode.ShootBead && IO.gunMotorSta[Id])
                    {
                        IO.GunMotorStop(Id);
                    }
                    break;
                }
                if (changeBulltTime > 0)
                {
                    changeBulltTime -= Time.deltaTime;
                    if (changeBulltTime <= 0)
                    {
                        if (currGun != null)
                        {
                            currGun.Init(this);
                        }
                    }
                    break;
                }
#if FPS_TEST
            if (fireDcTime <= 0) {
                Fire();
            }
#elif GUN_HW
                if (Key.KEYFJ_OkPressed(Id))
                {
                    Fire();
                }
#else
                //Key.KEYFJ_Statue_Ok(Id) || Set.setVal.GunMode == (int)en_GunMode.ShootWater
                if (Key.KEYFJ_Statue_Ok(Id) || Set.setVal.GunMode == (int)en_GunMode.ShootWater)
                {
                    if (Set.setVal.GunMode == (int)en_GunMode.ShootBead && IO.gunMotorSta[Id] == false)
                    {
                        IO.GunMotorStart(Id);
                    }
                    if (fireDcTime <= 0)
                    {
                        Fire();
                    }
                    pressedOkContinue = true;
                }
                else
                {
                    if (Set.setVal.GunMode == (int)en_GunMode.ShootBead && IO.gunMotorSta[Id])
                    {
                        IO.GunMotorStop(Id);
                    }

                    pressedOkContinue = false;
                }
#endif
#if !GUN_HW
                if (canChangeGun)
                {
                    if (Key.KEYFJ_AddPressed(Id))
                    {
                        ChangeGun();
                    }
                }
                
#endif
                break;

            case en_PlayerSta.Die:
                runTime += Time.deltaTime;
                if (runTime >= 1)
                {
                    ChangeStatue(en_PlayerSta.ContinueTimeout);
                }
                break;
            case en_PlayerSta.ContinueTimeout:
                // 倒计时
                runTime += Time.deltaTime;
                if (runTime >= 1)
                {
                    runTime -= 1;
                    if (timeOut > 0)
                    {
                        timeOut--;
                        playerUI.Update_ContinueTime(timeOut);
                    }
                    else
                    {
                        Main.JieSuanScore(Id);
                        Update_Score();
                        isEnd[Id] = true;
                        ChangeStatue(en_PlayerSta.Idle);
                        break;
                    }
                }
                if (Main.IsCanGamePlay(Id))
                {
                    if (Set.setVal.GunMode == (int)en_GunMode.ShootWater)
                    {
                        GameStart(en_StartType.Continue);
                    }
                    else
                    {
                        ChangeStatue(en_PlayerSta.JoinGame);
                    }
                }
                break;

            case en_PlayerSta.JoinGame:
                if (Main.IsCanGamePlay(Id) == false)
                {
                    if (oldStatue == en_PlayerSta.Idle)
                    {
                        ChangeStatue(en_PlayerSta.Idle);
                    }
                    else
                    {
                        ChangeStatue(en_PlayerSta.ContinueTimeout);
                    }
                    break;
                }
                // 倒计时
                runTime += Time.deltaTime;
                if (runTime >= 1)
                {
                    runTime -= 1;
                    if (timeOut > 0)
                    {
                        timeOut--;
                        playerUI.Update_ContinueTime(timeOut);
                    }
                    else
                    {
                        if (Main.IsCanGamePlay(Id))
                        {
                            //if (oldStatue == en_PlayerSta.Idle)
                            //{
                            //    GameStart(en_StartType.Reset);
                            //}
                            //else
                            //{
                            //    GameStart(en_StartType.Continue);
                            //}
                            Main.JieSuanScore(Id);
                            Update_Score();
                            isEnd[Id] = true;
                            ChangeStatue(en_PlayerSta.Idle);
                            break;
                        }
                        break;
                    }
                }
                if (Key.KEYFJ_OkPressed(Id))//按确定是否续币
                {
                    if (Main.IsCanGamePlay(Id))
                    {
                        if (oldStatue == en_PlayerSta.Idle)
                        {
                            GameStart(en_StartType.Reset);
                        }
                        else
                        {
                            GameStart(en_StartType.Continue);
                        }
                    }
                }
                break;
        }
#if FPS_TEST
        FindMonster();
#else
        UpdateCursor();
#endif
    }
    void ChangeStatue(en_PlayerSta sta)
    {
        //print("P_"+ Id + "_Statue: " + sta);
        Key.Clear();

        oldStatue = statue;
        statue = sta;
        runTime = 0;
#if GUN_HW
        if (statue == en_PlayerSta.Play && FjData.g_Fj[Id].Playing)
        {
            IO.GunMotorStart(Id);
        }
        else
        {
            IO.GunMotorStop(Id);
        }
#endif
        switch (statue)
        {
            case en_PlayerSta.Idle:
                // playerUI.gameObject.SetActive(false);

                playerUI.image_ContinueText.gameObject.SetActive(false);
                playerUI.SetCheckPleaseCoinIn(true);
                playerUI.image_Cursor.gameObject.SetActive(false);
                if (currGun != null)
                {
                    Destroy(currGun.gameObject);
                }
                if (Set.setVal.GunMode == (int)en_GunMode.ShootWater || Set.setVal.GunMode == (int)en_GunMode.ShootBead)
                {
                    IO.GunMotorStop(Id);
                }
                break;
            case en_PlayerSta.Play:
                //  playerUI.gameObject.SetActive(true);
                playerUI.image_ContinueText.gameObject.SetActive(false);
                playerUI.SetCheckPleaseCoinIn(false);
                if (Set.setVal.GunMode == (int)en_GunMode.ShootBead || Set.setVal.GunMode == (int)en_GunMode.ShootWater)
                {
                    playerUI.image_Cursor.gameObject.SetActive(false);
                }
                else
                {
                    playerUI.image_Cursor.gameObject.SetActive(true);
                    if (Set.setVal.PlayerMode == (int)en_PlayerMode.One && Id == 1)
                    {
                        playerUI.image_Cursor.gameObject.SetActive(false);
                    }
                }
                Update_Gun(gunId);
                if (Set.setVal.GunMode == (int)en_GunMode.ShootWater)
                {
                    IO.GunMotorStart(Id);
                }
#if !GUN_HW
                if (Set.setVal.GunMode == (int)en_GunMode.ShootWater)
                {
                    IO.GunMotorStart(Id);
                }
#endif
                IO.ButtonLED(Id, 1);
                break;
            case en_PlayerSta.Die:
                FjData.g_Fj[Id].Playing = false;
                if (Set.setVal.GunMode == (int)en_GunMode.ShootWater || Set.setVal.GunMode == (int)en_GunMode.ShootBead)
                {
                    IO.GunMotorStop(Id);
                }
                IO.ButtonLED(Id, 0);
                // 枪神: 玩家死亡时开始退彩票/扭蛋
                if (FjData.g_Fj[Id].Wins > 0)
                {
                    PAction.outEnable[Id] = true;
                }
                break;
            case en_PlayerSta.ContinueTimeout:
                playerUI.SetCheckPleaseCoinIn(true);
                playerUI.image_ContinueText.gameObject.SetActive(true);
                timeOut = 20;
                if (Main.COMPANY_NUM == 9)
                {
                    timeOut = 30;
                }
                //#if UNITY_EDITOR
                //                timeOut = 2;
                //#endif
                playerUI.Update_ContinueTime(timeOut);
                playerUI.image_ContinueText.sprite = sprite_ContinueGameStart[0 + Set.setVal.Language];
                Main.FormatImageSizeFollowSprite(playerUI.image_ContinueText);
                break;
            case en_PlayerSta.JoinGame:
                playerUI.image_ContinueText.gameObject.SetActive(true);
                playerUI.image_ContinueText.sprite = sprite_ContinueGameStart[2 + Set.setVal.Language];
                Main.FormatImageSizeFollowSprite(playerUI.image_ContinueText);
                timeOut = 20;
#if FPS_TEST
            timeOut = 1;
#endif
                playerUI.Update_ContinueTime(timeOut);
                break;
        }
    }


    Vector3 screenPos;
    RaycastHit hit;
    Ray ray;
    void UpdateCursor()
    {
        // 准心坐标
        //#if UNITY_EDITOR
        ////        gunCursor.transform.localPosition = new Vector3((Input.mousePosition.x / Screen.width - 0.5f) * Game00_Main.ScreenWidth3D, (Input.mousePosition.y / Screen.height - 0.5f) * Game00_Main.ScreenHeight3D, 0);
        //        playerUI.image_Cursor.transform.position = Input.mousePosition;
        //#else
        //        gunCursor.transform.localPosition = new Vector3((Key.GetAd_LeftRight(Id) - 0.5f) * Game00_Main.ScreenWidth3D, (Key.GetAd_UpDown(Id) - 0.5f) * Game00_Main.ScreenHeight3D, 0);
        //playerUI.image_Cursor.transform.localPosition = new Vector3((Key.GetAd_LeftRight(Id) - 0.5f) * Screen.width, (Key.GetAd_UpDown(Id) - 0.5f) * Screen.height, 0);
#if GUN_HW
        playerUI.image_Cursor.transform.position = new Vector3(Key.GetAd_LeftRight(Id) * Screen.width, Key.GetAd_UpDown(Id) * Screen.height, 0);
#else
        playerUI.image_Cursor.transform.localPosition = new Vector3(Key.cursorPos_Left[Id] + Key.GetAd_LeftRight(Id) * Key.screen_Width[Id], (Key.GetAd_UpDown(Id) - 0.5f) * Screen.height, 0);
#endif
        //playerUI.image_Cursor.transform.localPosition = new Vector3(0, 0, 0);
        //#endif


        //// 手枪角度
        screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, playerUI.image_Cursor.transform.position);
        //screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, gunCursor.transform.position);
        ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            //targetPos.transform.position = hit.point;
            // if (hit.collider.gameObject.tag != "Monster")
            gunPos.transform.LookAt(hit.point, transform.up);   //  + new Vector3(0, -0.1f, 0)
        }
    }
    void FindMonster()
    {
        if (monsterTarget == null)
        {
            monsterTarget = gameMain.FindMonster(this);
        }
        else
        {
            if (monsterTarget.statue == en_MonsterSta.Die || Vector3.Angle(transform.forward, monsterTarget.transform.position - transform.position) > 60)
            {
                monsterTarget = null;
                return;
            }
            // 枪头瞄准目标怪
            gunPos.transform.rotation = Quaternion.Slerp(gunPos.transform.rotation, Quaternion.LookRotation(monsterTarget.centerPos.transform.position - gunPos.transform.position), 10 * Time.deltaTime);
            //
            if (Physics.Raycast(gunPos.transform.position, gunPos.transform.forward, out hit, 1000))
            {
                screenPos = Camera.main.WorldToScreenPoint(hit.point);
                playerUI.image_Cursor.transform.position = new Vector3(screenPos.x, screenPos.y, 0);
            }
        }
    }

    void Update_Gun(int no)
    {
        if (no >= gun_Prefab.Length)
            return;
        gunId = no;
        if (currGun != null)
        {
            Destroy(currGun.gameObject);
        }
        GameObject obj = GameObject.Instantiate(gun_Prefab[no], gunPos.transform);
        obj.transform.localPosition = new Vector3(0, 0, 0);
        obj.transform.localEulerAngles = new Vector3(0, 0, 0);
        currGun = obj.GetComponent<Game00_Gun>();
        currGun.Init(this);

        if (playerUI.remainBulletNum_Obj != null)
        {
            if (currGun.maxBulletNum > 0 && Set.setVal.GunMode == (int)en_GunMode.Gun)
            {
                playerUI.remainBulletNum_Obj.SetActive(true);
            }
            else
            {
                playerUI.remainBulletNum_Obj.SetActive(false);
            }
        }
    }
    void ChangeGun()
    {
        int no = (gunId + 1) % gun_Prefab.Length;
        Update_Gun(no);
    }
    void Fire()
    {
        switch (shellType)
        {
            case en_ShellType.DaoDan:
                if (daoDanNum > 0 && (pressedOkContinue == false || Set.setVal.GunMode == (int)en_GunMode.ShootWater))
                {
                    GameObject obj = GameObject.Instantiate(gameMain.shell_DaoDan_Prefab);
                    obj.transform.position = daoDanFirePos.transform.position;
                    obj.transform.eulerAngles = daoDanFirePos.transform.eulerAngles;
                    if (Vector3.Distance(obj.transform.position, hit.point) < 8f)
                    {
                        obj.transform.LookAt(hit.point);
                    }
                    // Debug.Log(hit.transform.root.gameObject.name);
                    // UnityEditor.EditorApplication.isPaused = true;
                    obj.GetComponent<Game00_Shell>().Init(this, hit.point, 100);
                    fireDcTime = 0.3f;
                    daoDanNum--;
                }
                break;
            case en_ShellType.Normal:
            case en_ShellType.SanDan:
                // 火花
                if (currGun != null)
                {
                    if (currGun.Fire())
                    {
                        fireDcTime = currGun.dcTime;
#if GUN_HW
                        screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, playerUI.image_Cursor.transform.position + new Vector3(Random.Range(-35, 35), Random.Range(-35, 35), 0));
                        ray = Camera.main.ScreenPointToRay(screenPos);
                        if (Physics.Raycast(ray, out hit, 1000))
                        {
                            // 火花
                            if (currGun.gunType == en_GunType.Normal && currGun.hitTargetEff != null)
                            {
                                //GameObject obj = Instantiate(currGun.hitTargetEff);
                                //这里用对象池得到击中怪的特效，记得在特效上绑RemoveObj脚本，不用再销毁物体了！
                                GameObject obj = PoolManager.instance.GetObj(currGun.hitTargetEff);
                                obj.transform.parent = playerUI.hitEffTran;
                                //obj.transform.position = hit.point;
                                obj.transform.position = playerUI.image_Cursor.transform.position;
                                obj.transform.eulerAngles = Camera.main.transform.eulerAngles;
                                float random;//随机大小
                                if (obj.GetComponent<Animator>() != null)//序列帧特效
                                {
                                    random = Random.Range(0.6f, 0.7f);
                                }
                                else//粒子特效
                                {
                                    random = Random.Range(1.2f, 1.5f);
                                }
                                obj.transform.localScale = new Vector3(random, random, random);
                                random = Random.Range(0, 360f);
                                //obj.transform.localEulerAngles = new Vector3(obj.transform.localEulerAngles.x, obj.transform.localEulerAngles.y, random);
                                obj.transform.localEulerAngles = new Vector3(0, 0, random);
                            }
                            // 怪
                            if (hit.collider.transform.root.tag == "Monster")
                            {
                                Game00_Monster monster = hit.collider.transform.root.GetComponent<Game00_Monster>();
                                if (monster != null)
                                {
                                    bool crit = false;
                                    if (Random.Range(0, 1000) < 50)
                                    {
                                        crit = true;
                                    }
                                    monster.Attacked(this, currGun.attackValue, crit);
                                }
                                break;
                            }
                            // 道具
                            if (hit.collider.transform.root.tag == "DaoJu")
                            {
                                Game00_DaoJu daoju = hit.collider.transform.root.GetComponent<Game00_DaoJu>();
                                if (daoju != null)
                                {
                                    daoju.Attacked(this);
                                }
                            }
                        }
#else
                        if (Set.setVal.GunMode == (int)en_GunMode.ShootWater || Set.setVal.GunMode == (int)en_GunMode.ShootBead)
                        {
                            screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, playerUI.image_Cursor.transform.position + new Vector3(Random.Range(-35, 35), Random.Range(-35, 35), 0));
                            ray = Camera.main.ScreenPointToRay(screenPos);
                            if (Physics.Raycast(ray, out hit, 1000))
                            {
                                // 火花
                                if (currGun.gunType == en_GunType.Normal && currGun.hitTargetEff != null)
                                {
                                    //GameObject obj = Instantiate(currGun.hitTargetEff);
                                    GameObject obj = PoolManager.instance.GetObj(currGun.hitTargetEff);
                                    obj.transform.position = hit.point;
                                    obj.transform.eulerAngles = Camera.main.transform.eulerAngles;
                                    float random;//随机大小
                                    if (obj.GetComponent<Animator>() != null)//序列帧特效
                                    {
                                        random = Random.Range(0.8f, 1.2f);
                                    }
                                    else//粒子特效
                                    {
                                        random = Random.Range(0.2f, 0.4f);
                                    }
                                    obj.transform.localScale = new Vector3(random, random, random);
                                }
                                // 怪
                                if (hit.collider.transform.root.tag == "Monster")
                                {
                                    Game00_Monster monster = hit.collider.transform.root.GetComponent<Game00_Monster>();
                                    if (monster != null)
                                    {
                                        bool crit = false;
                                        if (Random.Range(0, 1000) < 50)
                                        {
                                            crit = true;
                                        }
                                        monster.Attacked(this, currGun.attackValue, crit);
                                    }
                                    break;
                                }
                                // 道具
                                if (hit.collider.transform.root.tag == "DaoJu")
                                {
                                    Game00_DaoJu daoju = hit.collider.transform.root.GetComponent<Game00_DaoJu>();
                                    if (daoju != null)
                                    {
                                        daoju.Attacked(this);
                                    }
                                }
                            }
                        }
#endif
                    }
                }
                break;
        }
        //// 火花
        //if (currGun != null) {
        //    currGun.Fire();
        //    fireDcTime = currGun.dcTime;

        //    screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, playerUI.image_Cursor.transform.position);
        //    ray = Camera.main.ScreenPointToRay(screenPos);
        //    if (Physics.Raycast(ray, out hit, 1000)) {
        //        // 怪?
        //        GameObject obj;
        //        switch (shellType) {
        //        case en_ShellType.Normal:     // 普通子弹
        //            IO.GunMotorStart(Id, 0.06f);
        //            //
        //            if (hit.collider.tag == "FarCollidor") {
        //                break;
        //            }
        //            // 其它
        //            if (currGun.hitTargetEff != null) {
        //                obj = GameObject.Instantiate(currGun.hitTargetEff);
        //                obj.transform.position = hit.point;
        //                obj.transform.eulerAngles = gameMain.shakeMain_Obj.transform.eulerAngles;
        //            }

        //            if (hit.collider.tag == "Monster") {
        //                Game00_Monster monster = hit.collider.transform.root.GetComponent<Game00_Monster>();
        //                if (monster != null) {
        //                    bool crit = false;
        //                    if (Random.Range(0, 1000) < 100) {
        //                        crit = true;
        //                    }
        //                    monster.Attacked(this, currGun.attackValue, crit);
        //                }
        //                return;
        //            }
        //            // 道具
        //            if (hit.collider.tag == "DaoJu") {
        //                Game00_DaoJu daoju = hit.collider.transform.root.GetComponent<Game00_DaoJu>();
        //                if (daoju != null) {
        //                    daoju.Attacked(this);
        //                }
        //                return;
        //            }
        //            break;

        //        case en_ShellType.SanDan:
        //            IO.GunMotorStart(Id, 0.06f);
        //            for (int i = 0; i < 9; i++) {
        //                obj = GameObject.Instantiate(gameMain.shell_SanDan_Prefab);
        //                obj.transform.position = currGun.firePos.transform.position + currGun.firePos.transform.right * (i / 3) * 0.5f + currGun.firePos.transform.up * (i % 3) * 0.5f;
        //                obj.transform.eulerAngles = currGun.firePos.transform.eulerAngles;
        //                obj.GetComponent<Game00_Shell>().Init(this, hit.point, 10);
        //            }
        //            fireDcTime = 0.2f;
        //            break;
        //        case en_ShellType.DaoDan:
        //            if (daoDanNum > 0) {
        //                obj = GameObject.Instantiate(gameMain.shell_DaoDan_Prefab);
        //                obj.transform.position = daoDanFirePos.transform.position;
        //                obj.transform.eulerAngles = daoDanFirePos.transform.eulerAngles;
        //                obj.GetComponent<Game00_Shell>().Init(this, hit.point, 100);
        //                fireDcTime = 0.3f;
        //                daoDanNum--;
        //            }
        //            break;
        //        }

        //    }
        //}        
    }

    public void Update_HP()
    {
        playerUI.Update_BloodValue((float)FjData.g_Fj[Id].Blood / Set.setVal.GameTime);  //MAX_PLAYER_BLOOD);
    }
    public void Update_Coins()
    {

    }
    public void Update_Score()
    {
        playerUI.Update_ScoreNum(FjData.g_Fj[Id].Scores);
        playerUI.Update_Score(FjData.g_Fj[Id].Scores);
    }
    public void KillMonster(int scorevalue)
    {
        FjData.g_Fj[Id].Scores += scorevalue;
        FjData.g_Fj[Id].JsScores = FjData.g_Fj[Id].Scores;
        Update_Score();
    }
    public void GetDaoJu(en_Game00_DaoJuType type)
    {
        if (statue != en_PlayerSta.Play)
            return;
        Main.Log("GetDaoJu:" + type);
        switch (type)
        {
            case en_Game00_DaoJuType.BloodPag:
                //int addblood = Set.setVal.GameTime / 5;
                //if (addblood == 0)
                //{
                //    addblood = 1;
                //}
                int addblood = 10;
                FjData.g_Fj[Id].Blood += addblood;
                if (FjData.g_Fj[Id].Blood > Set.setVal.GameTime)
                {
                    FjData.g_Fj[Id].Blood = Set.setVal.GameTime; //MAX_PLAYER_BLOOD;
                }
                FjData.g_Fj[Id].GameTime += addblood;
                if (FjData.g_Fj[Id].GameTime > Set.setVal.GameTime)
                {
                    FjData.g_Fj[Id].GameTime = Set.setVal.GameTime; //MAX_PLAYER_BLOOD;
                }
                Update_HP();
                break;
            case en_Game00_DaoJuType.SanDan:
                shellType = en_ShellType.DaoDan;
                specialAttackTime = MAX_SPECIALATTCK_TIME;
                playerUI.image_SpecialAttackFlag.gameObject.SetActive(true);
                break;
            case en_Game00_DaoJuType.DaoDan:
                shellType = en_ShellType.DaoDan;
                daoDanNum = 3;
                specialAttackTime = MAX_SPECIALATTCK_TIME;
                playerUI.image_SpecialAttackFlag.gameObject.SetActive(true);
                break;
            case en_Game00_DaoJuType.Box:
                openBoxNum++;
                break;
        }
    }
    public bool CanAttacked()
    {
        if (statue != en_PlayerSta.Play)
            return false;
        if (openBox != null)        // 正在开宝箱
            return false;
        if (monsterEnemy != null)   // 有怪正在攻击
            return false;
        if (attackedDcTime > 0)     // 被攻击后间隔时间
            return false;
        return true;
    }
    public void Attacked(Game00_Monster monster, int attackvalue)
    {
        if (statue != en_PlayerSta.Play)
            return;



        //print("PlayerAttacked: " + attackvalue);
        FjData.g_Fj[Id].Blood -= attackvalue;//attackvalue
        if (FjData.g_Fj[Id].Blood < FjData.g_Fj[Id].GameTime)
        {
            FjData.g_Fj[Id].Blood = FjData.g_Fj[Id].GameTime;//FjData.g_Fj[Id].GameTime
        }
        if (FjData.g_Fj[Id].Blood <= 0)
        {
            FjData.g_Fj[Id].Blood = 0;
            ChangeStatue(en_PlayerSta.Die);
        }
        Update_HP();
        attackedDcTime = 2f;
    }
    //void OnGUI() {
    //    GUI.color = Color.yellow;
    //    GUI.Label(new Rect(50, 520, 300, 20), "ADC_LR: " + Key.GetAd_LeftRight(Id));
    //    GUI.Label(new Rect(50, 550, 300, 20), "ADC_UD: " + Key.GetAd_UpDown(Id));
    //}
}
