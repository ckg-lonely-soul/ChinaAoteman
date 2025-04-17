using System.Collections;
using UnityEngine;


public enum en_MonsterAttackDistanceType
{
    Near = 0,       // 近程
    Far,            // 远程
    All,            // 都有
}

public enum en_MonsterSta
{
    Idle = 0,
    OnCar,
    DownCar,
    Hide,
    Stop,
    Jump,
    Run,
    RunOut, //离攻击点太近向外跑
    Walk,
    Attack,
    SpecialSkill,
    Dmage,
    Die,
    Roll,
    Quicken, //加速冲刺
    Spawn,   //怪物跳出场

    //Boss状态
    Roar,  
}


public class Game00_Monster : MonoBehaviour
{
    public bool isNoDieAniamtion = false;
    public Animator animator;
    public Animation animation;
    public AnimEvent animEvent;
    public RenderEvent renderEvent;
    public GameObject[] materialModel_Obj;
    //public NavMeshAgent navMeshAgent;
    //public NavMeshObstacle navMeshObstacle;
    public GameObject[] collider_Obj;
    public GameObject centerPos;                    // 中心点
    public GameObject HP_Pos;
    public GameObject firePos_Obj;
    public GameObject dieEffPos_Obj;
    public GameObject effect_Bomb_Prefab;           //
    public GameObject effect_Die_Prefab;            // 死亡特效
    public GameObject effect_Attack_Prefab;         // 攻击特效
    public GameObject effect_AttackScreen_Prefab;   // 攻击在屏幕上的特效
    public GameObject shell_Prefab;                 // 炮弹（攻击球）
    public Material[] material_Damage;

    // 声音
    public AudioSource audioSource_Run;
    public AudioSource audioSource_Attack;
    public AudioSource audioSource_Die;
    public AudioClip[] audioClip_Die;

    GameObject monsterScore_Obj;                    // 怪分数

    Material[] material_Nor;
    Renderer[] material;

    //参数
    public en_MonsterAttackDistanceType attackDistanceType;    // 攻击方式   = en_MonsterAttackDistanceType.Far
    public bool isBOSS = false;             // 是否BOSS怪
    public bool isSmallBOSS = false;        // 是否小BOSS
    public int attackValue = 10;            // 攻击力
    public int blood = 100;
    public int score;
    //
    public bool onCar = false;              // 是否车载
    public bool isAttack = true;            // 是否会攻击
    //public bool autoAttack = false;			// 是否自动攻击
    //public bool byDamage = true;            // 是否进入被攻击动画状态
    public bool isAttackShake = false;      // 攻击是否震动
    public float autoAttackSize = 6f;       // 自动攻击范围
    public float minDistance = 1.0f;        // 最小距离()
    public float minAttackDistance = 0.5f;	// 攻击距离
    public float maxAttackDistance = 1.3f;	// 攻击距离
    public float readyAttackDistance = 1.0f;// 准备攻击距离
    public float attackDcTime = 3.0f;       // 攻击冷却时间
    public float rotateSpeed = 180;         // 转身速度
    public float walkSpeed = 3;             // 移动速度
    public float runSpeed = 8;              // 移动速度
    public GameObject attackPos;            // 攻击时站的位置: 有攻击点时，不会跑走，一直打(有隐藏点除外)
    public GameObject hidePos;              // 隐藏点

    //
    public GameObject[] attackPosArray;
    int attackPosId;
    //
    [HideInInspector]
    public int level;                       // 怪物等级(模型ID)

    //[HideInInspector]
    public en_MonsterSta statue;
    int targetPosId = 0;
    // [HideInInspector]
    public int remainBlood;

    //
    //[HideInInspector]
    public Game00_Main gameMain;
    //[HideInInspector]
    public GameObject mainCar_Obj;
    // [HideInInspector]
    public Game00_Player playerEnemy;       // 敌人
    [HideInInspector]
    public Game00_Car car;                  // 车
    //
    // 变量
    [HideInInspector]
    public float runTime;
    // 渲染检测
    public bool isRender;
    float lastRenderTime;
    float currRenderTime;
    float renderTime;
    //玩家受到伤害后屏幕红闪的次数
    int damageCnt;
    float damageTime;

    // [HideInInspector]
    public Transform[] roadPos;

    Vector3 screenPos;
    Vector2 uguiPos;

    public void Init(Game00_Main gamemain, int maxblood, int scorevalue, int attackvlaue)
    {
        gameMain = gamemain;
        mainCar_Obj = gameMain.navMeshAgent_MainCar.gameObject;
        // 材质
        material = new Renderer[materialModel_Obj.Length];
        material_Nor = new Material[materialModel_Obj.Length];
        for (int i = 0; i < materialModel_Obj.Length; i++)
        {
            if (materialModel_Obj[i] != null)
            {
                material[i] = materialModel_Obj[i].GetComponent<Renderer>();
                material_Nor[i] = material[i].sharedMaterial;     //material.material 会产生内存泄漏
            }
        }


        blood = maxblood;
        score = scorevalue;
        attackValue = attackvlaue;
        remainBlood = blood;
        renderTime = 10;

        damageCnt = 0;
        damageTime = 0;
        //if (navMeshObstacle != null)
        //    navMeshObstacle.enabled = false;
        if (attackDistanceType == en_MonsterAttackDistanceType.Far)
        {
            gameMain.farMonsterNum++;
        }
        remainBlood = blood;
        //
        if (animEvent != null)
        {
            animEvent.Clear();
        }
        if (isBOSS)
        {
            gameMain.FreshedBoss(this);
        }
        else if (isSmallBOSS)
        {
            gameMain.monsterSmallBoss = this;
        }
        ChangeStatue(en_MonsterSta.Walk);
        // 初始化怪物会变成行走状态。
    }

    void OnDestroy()
    {
        if (gameMain.monsterNum > 0)
        {
            gameMain.monsterNum--;
        }
        if (attackDistanceType == en_MonsterAttackDistanceType.Far)
        {
            if (gameMain.farMonsterNum > 0)
            {
                gameMain.farMonsterNum--;
            }
        }
        if (statue != en_MonsterSta.Die)
        {
            if (gameMain.monsterAliveNum > 0)
            {
                gameMain.monsterAliveNum--;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        // 显示渲染
        if (renderEvent != null && !isBOSS)
        {
            if (renderEvent.IsRender())
            {
                renderTime = 45.0f;
            }
            if (renderTime > 0)
            {
                if (statue != en_MonsterSta.Hide)
                {
                    renderTime -= Time.deltaTime;
                    if (renderTime <= 0)
                    {
                        //这里意思应该是如果玩家见到了怪物，之后怪物在玩家镜头之后消失
                        //如果时间超过renderTime,就会摧毁该怪物。
                        Destroy(gameObject);

                        return;
                    }
                }
            }
        }

        if (damageCnt > 0)
        {
            damageTime += Time.deltaTime;
            if (damageTime >= 0.2f)
            {
                damageTime = 0;
                damageCnt--;

                for (int i = 0; i < materialModel_Obj.Length; i++)
                {
                    if (materialModel_Obj[i] != null)
                    {
                        if ((damageCnt % 2) == 0)
                        {
                            material[i].sharedMaterial = material_Nor[i];
                        }
                        else
                        {
                            material[i].sharedMaterial = material_Damage[i];
                        }
                    }
                }
            }
        }


        if (statue == en_MonsterSta.Die)
        {
            if (isBOSS == false)
            {
                if (runTime < 3d)
                {
                    runTime += Time.deltaTime;
                }
                else
                {
                    Destroy(gameObject);
                }

            }
            return;
        }



        switch (statue)
        {
            case en_MonsterSta.Idle:

                break;
            case en_MonsterSta.Run:

                break;
            case en_MonsterSta.Walk:

                break;
            case en_MonsterSta.Attack:

                break;
            case en_MonsterSta.Dmage:

                break;
            case en_MonsterSta.Die:

                break;
        }
    }

    private IEnumerator SetHide()
    {
        int count = 0;
        while (count < 20)
        {
            renderEvent.gameObject.SetActive(!renderEvent.gameObject.activeSelf);
            yield return new WaitForSeconds(0.1f);
            count++;
        }
        yield return new WaitForFixedUpdate();
        if (gameObject != null)
        {
            Destroy(gameObject);
        }

    }
    void LateUpdate()
    {
        if (isBOSS)
        {
            //gameMain.Update_BossHP_Pos(HP_Pos.transform.position); //设置boss血条位置
        }
    }
    public void ChangeStatueWithoutAnim(en_MonsterSta sta)
    {
        statue = sta;
        runTime = 0;
    }
    public void ChangeStatue(en_MonsterSta sta)
    {
        //     print("Monst Change Statue: " + sta.ToString());
        statue = sta;
        runTime = 0;
        if (animator != null)
        {
            if (animator.enabled == false)
            {
                animator.enabled = true;
            }
        }

        if (audioSource_Run != null)
        {
            audioSource_Run.Stop();
        }

        switch (statue)
        {
            case en_MonsterSta.Idle:
                if (animator != null)
                {
                    animator.Play("Idle");
                }
                else if (animation != null)
                {
                    if (animation.name == "Idle")
                    {
                        animation.Play("Idle");
                    }
                }
                break;
            case en_MonsterSta.OnCar:
                animator.enabled = false;
                animator.Update(0);
                break;
            case en_MonsterSta.DownCar:
                UpdateRoadPos();
                if (animator != null)
                {
                    animator.Play("Run");
                }
                else if (animation != null)
                {
                    animation.Play("Run");
                }
                break;
            case en_MonsterSta.Run:
                if (animator != null)
                {
                    animator.Play("Run");
                }
                else if (animation != null)
                {
                    if (animation.name == "Run")
                    {
                        animation.Play("Run");
                    }
                }
                if (audioSource_Run != null)
                {
                    audioSource_Run.Play();
                }
                break;

            case en_MonsterSta.Walk:
                if (animator != null)
                {
                    animator.Play("Walk");
                }
                else if (animation != null)
                {
                    animation.Play("Walk");
                }
                if (audioSource_Run != null)
                {
                    audioSource_Run.Play();
                }
                break;
            case en_MonsterSta.Hide:
            case en_MonsterSta.RunOut:
                //if (animator != null)
                //{
                //    animator.Play("Run");
                //}
                //else if (animation != null)
                //{
                //    if (animation.name == "Run")
                //    {
                //        animation.Play("Run");
                //    }
                //}
                break;
            case en_MonsterSta.Attack:
                if (animator != null)
                {
                    animator.Play("Attack", 0, 0);
                    //强制更新动画
                    //animator.Update(0);
                }
                else if (animation != null)
                {
                    if (animation.name == "Attack")
                    {
                        animation.Play("Attack");
                    }
                }
                break;
            case en_MonsterSta.Dmage:
                if (animator != null)
                {
                    animator.Play("Damage");
                }
                else if (animation != null)
                {
                    if (animation.name == "Damage")
                    {
                        animation.Play("Damage");
                    }
                }
                break;
            case en_MonsterSta.Die:
                if (animator != null)
                {
                    animator.Play("Die", 0, 0);
                }
                else if (animation != null)
                {
                    animation.Play("Die");
                }
                for (int i = 0; i < collider_Obj.Length; i++)
                {
                    Destroy(collider_Obj[i]);
                }
                if (audioSource_Run != null)
                {
                    audioSource_Run.Stop();
                }
                if (renderEvent != null && isNoDieAniamtion)
                    StartCoroutine("SetHide");
                break;
        }
    }

    public void SetRoadPos(Transform[] roadpos)
    {
        roadPos = roadpos;
    }
    GameObject roadPosGroupObj;
    public void SetRoadPos(GameObject roadObj)
    {
        roadPosGroupObj = roadObj;
    }
    public void UpdateRoadPos()
    {
        if (roadPosGroupObj != null)
        {
            roadPos = roadPosGroupObj.GetComponentsInChildren<Transform>();
        }
    }
    void Move()
    {

    }
    #region 被攻击旧方法
    // 被攻击
    //public void Attacked(Game00_Player playerFun, int attackValue, bool crit)
    //{
    //    if (statue == en_MonsterSta.Die)
    //        return;
    //    //ChangeStatue(en_MonsterSta.Dmage);
    //    //  UnityEditor.EditorApplication.isPaused = true;
    //    if (transform.root != transform)
    //    {
    //        attackValue /= 50;
    //    }
    //    //出数字
    //    //if (Set.setVal.GameMode == (int)en_GameMode.Yule) {
    //    //    GameObject bloodNum;
    //    //    if (crit) {
    //    //        bloodNum = GameObject.Instantiate(gameMain.decBlood_Crit_Prefab);
    //    //        bloodNum.GetComponent<DecBlood_Crit>().NumValue(attackValue);
    //    //    } else {
    //    //        bloodNum = GameObject.Instantiate(gameMain.decBlood_Nor_Prefab);
    //    //    }
    //    //    bloodNum.transform.position = HP_Pos.transform.position;
    //    //    bloodNum.transform.eulerAngles = new Vector3(0, gameMain.players_Obj.transform.eulerAngles.y, 0);
    //    //    Num num_blood = bloodNum.GetComponentInChildren<Num>();
    //    //    num_blood.SetBoundsChar(11);    // '-'
    //    //    if (remainBlood >= attackValue) {
    //    //        num_blood.UpdateShow(attackValue);
    //    //    } else {
    //    //        num_blood.UpdateShow(remainBlood);
    //    //    }
    //    //}
    //    //
    //    remainBlood -= attackValue;
    //    if (remainBlood <= 0)
    //    {
    //        // 死亡
    //        damageCnt = 0;
    //        for (int i = 0; i < materialModel_Obj.Length; i++)
    //        {
    //            if (materialModel_Obj[i] != null)
    //            {
    //                if (material[i] != null && material_Nor[i] != null)
    //                {
    //                    material[i].sharedMaterial = material_Nor[i];
    //                }
    //            }
    //        }

    //        ChangeStatue(en_MonsterSta.Die);
    //        //
    //        gameMain.OneMonsterDie();

    //        //
    //        if (playerFun != null)
    //        {
    //            playerFun.KillMonster(score);
    //            if (Game_Prefab.instance.image_Score != null)
    //            {

    //                GameObject scoreObj = GameObject.Instantiate(Game_Prefab.instance.image_Score, gameMain.gameUI.transform);//生成得分
    //                scoreObj.transform.position = Camera.main.WorldToScreenPoint(HP_Pos.transform.position);
    //                scoreObj.GetComponent<Game00_GetScore>().Init(playerFun, score);
    //                //爆出keyCard
    //                if (hasKeyCard)
    //                {
    //                    GameObject keyCard_obj = GameObject.Instantiate(gameMain.daoJu_Prefab[gameMain.daoJu_Prefab.Length - 1], transform.position, Quaternion.identity);
    //                    keyCard_obj.GetComponent<Game00_DaoJu>().Init(gameMain);
    //                }
    //                else
    //                {
    //                    // 暴出道具
    //                    en_Game00_DaoJuType daojuType = gameMain.GetDaoJu(playerFun);
    //                    if (daojuType != en_Game00_DaoJuType.None)
    //                    {
    //                        GameObject daoju_obj = GameObject.Instantiate(gameMain.daoJu_Prefab[(int)daojuType], transform.position, Quaternion.identity);
    //                        daoju_obj.GetComponent<Game00_DaoJu>().Init(gameMain);
    //                    }
    //                }


    //            }

    //        }
    //        // 震屏效果
    //        if (effect_Bomb_Prefab != null)
    //        {
    //            gameMain.ShakeStart(1.0f, 8);
    //            for (int i = 0; i < 3; i++)
    //            {
    //                GameObject obj = GameObject.Instantiate(effect_Bomb_Prefab);
    //                obj.transform.position = HP_Pos.transform.position;
    //                obj.transform.LookAt(Camera.main.transform);
    //            }
    //        }
    //        if (effect_Die_Prefab != null)
    //        {
    //            GameObject obj = GameObject.Instantiate(effect_Die_Prefab);
    //            obj.transform.position = dieEffPos_Obj.transform.position; // centerPos.transform.position;
    //            obj.transform.LookAt(Camera.main.transform);
    //        }
    //        // 声音
    //        if (audioSource_Die != null)
    //        {
    //            if (audioClip_Die.Length > 0)
    //            {
    //                int no = Random.Range(0, audioClip_Die.Length);
    //                print("no: " + no);
    //                audioSource_Die.clip = audioClip_Die[no];
    //            }
    //            audioSource_Die.Play();
    //        }
    //        //
    //        if (isBOSS && gameMain.gameUI.boss_HP_Obj != null)
    //        {
    //            gameMain.gameUI.boss_HP_Obj.SetActive(false);
    //        }
    //        return;
    //    }
    //    else
    //    {
    //        // 没死
    //        if (material_Damage != null)
    //        {
    //            if (damageCnt < 4)
    //            {
    //                damageCnt = 6;
    //            }
    //        }
    //        if (isBOSS)
    //        {
    //            gameMain.gameUI.Update_BossHpValue((float)remainBlood / blood);
    //        }
    //    }

    //    //
    //    if (playerEnemy == null && playerFun != null)
    //    {
    //        if (playerFun.CanAttacked())
    //        {
    //            playerEnemy = playerFun;
    //        }
    //    }
    //}
    #endregion
    public void Attacked(Game00_Player playerFun, int attackValue, bool crit)
    {
        if (statue == en_MonsterSta.Die)
            return;
        //ChangeStatue(en_MonsterSta.Dmage);
        //  UnityEditor.EditorApplication.isPaused = true;
        if (transform.root != transform)
        {
            attackValue /= 50;
        }
        //出数字
        //if (Set.setVal.GameMode == (int)en_GameMode.Yule) {
        //    GameObject bloodNum;
        //    if (crit) {
        //        bloodNum = GameObject.Instantiate(gameMain.decBlood_Crit_Prefab);
        //        bloodNum.GetComponent<DecBlood_Crit>().NumValue(attackValue);
        //    } else {
        //        bloodNum = GameObject.Instantiate(gameMain.decBlood_Nor_Prefab);
        //    }
        //    bloodNum.transform.position = HP_Pos.transform.position;
        //    bloodNum.transform.eulerAngles = new Vector3(0, gameMain.players_Obj.transform.eulerAngles.y, 0);
        //    Num num_blood = bloodNum.GetComponentInChildren<Num>();
        //    num_blood.SetBoundsChar(11);    // '-'
        //    if (remainBlood >= attackValue) {
        //        num_blood.UpdateShow(attackValue);
        //    } else {
        //        num_blood.UpdateShow(remainBlood);
        //    }
        //}
        //
        remainBlood -= attackValue;
        if (remainBlood <= 0)
        {
            // 死亡
            damageCnt = 0;
            for (int i = 0; i < materialModel_Obj.Length; i++)
            {
                if (materialModel_Obj[i] != null)
                {
                    if (material[i] != null && material_Nor[i] != null)
                    {
                        material[i].sharedMaterial = material_Nor[i];
                    }
                }
            }

            ChangeStatue(en_MonsterSta.Die);
            //
            gameMain.OneMonsterDie();

            //
            if (playerFun != null)
            {
                playerFun.KillMonster(score);
                if (gameMain.num_GetScore_Prefab != null)
                {
                    // 得分
                    GameObject scoreObj = GameObject.Instantiate(gameMain.num_GetScore_Prefab);
                    scoreObj.transform.position = HP_Pos.transform.position;
                    scoreObj.transform.LookAt(Camera.main.transform);
                    scoreObj.transform.forward *= -1;
                    Num nums = scoreObj.GetComponent<Num>();
                    nums.SetBoundsChar(10); // '+'
                    nums.UpdateShow(score);

                    // 暴出道具
                    //en_Game00_DaoJuType daojuType = gameMain.GetDaoJu(playerFun);
                    //if (daojuType != en_Game00_DaoJuType.None)
                    //{
                    //    GameObject daoju_obj = GameObject.Instantiate(gameMain.daoJu_Prefab[(int)daojuType], transform.position, Quaternion.identity);
                    //    daoju_obj.GetComponent<Game00_DaoJu>().Init(gameMain);
                    //}
                }

            }
            // 震屏效果
            if (effect_Bomb_Prefab != null)
            {
                gameMain.ShakeStart(1.0f, 8);
                for (int i = 0; i < 3; i++)
                {
                    GameObject obj = GameObject.Instantiate(effect_Bomb_Prefab);
                    obj.transform.position = HP_Pos.transform.position;
                    obj.transform.LookAt(Camera.main.transform);
                }
            }
            if (effect_Die_Prefab != null)
            {
                GameObject obj = GameObject.Instantiate(effect_Die_Prefab);
                obj.transform.position = dieEffPos_Obj.transform.position; // centerPos.transform.position;
                obj.transform.LookAt(Camera.main.transform);
            }
            // 声音
            if (audioSource_Die != null)
            {
                if (audioClip_Die.Length > 0)
                {
                    int no = Random.Range(0, audioClip_Die.Length);
                    print("no: " + no);
                    audioSource_Die.clip = audioClip_Die[no];
                }
                audioSource_Die.Play();
            }
            //
            if (isBOSS && gameMain.gameUI.boss_HP_Obj != null)
            {
                gameMain.gameUI.boss_HP_Obj.SetActive(false);
            }
            return;
        }
        else
        {
            // 没死
            if (material_Damage != null)
            {
                if (damageCnt < 4)
                {
                    damageCnt = 6;
                }
            }
            if (isBOSS)
            {
                gameMain.gameUI.Update_BossHpValue((float)remainBlood / blood);
            }
            //if (byDamage == true) {
            //    ChangeStatue(en_MonsterSta.Dmage);
            //}
        }

        // attackedDistance = Vector3.Distance(transform.position, playerFun.transform.position);
        //
        if (playerEnemy == null && playerFun != null)
        {
            if (playerFun.CanAttacked())
            {
                playerEnemy = playerFun;
            }
        }
    }


    //找寻攻击目标
    public void CheckFindEnemy()
    {
        if (playerEnemy != null)
            return;
        if (Vector3.Angle((transform.position - gameMain.players_Obj.transform.position), gameMain.players_Obj.transform.forward) > 30)
            return;
        for (int i = 0; i < gameMain.player.Length; i++)
        {
            if (gameMain.player[i].CanAttacked())
            {
                playerEnemy = gameMain.player[i];
                gameMain.player[i].monsterEnemy = this;
                break;
            }
        }
    }
    RaycastHit hit;
    public bool LookedPlayers()
    {
        float dis = Vector3.Distance(centerPos.transform.position, gameMain.players_Obj.transform.position);
        if (Physics.Raycast(centerPos.transform.position, gameMain.players_Obj.transform.position - centerPos.transform.position, out hit, dis))
        {
            return false;
        }
        return true;
    }
    public GameObject GetPlayersObj()
    {
        return gameMain.players_Obj;
    }

    //
    public bool IsAlive()
    {
        if (statue == en_MonsterSta.Die)
            return false;
        return true;
    }

    /// <summary>
    /// 远程怪物的更新攻击位置
    /// </summary>
    public void UpdateAttackPos()
    {
        if (attackDistanceType == en_MonsterAttackDistanceType.Far && attackPosArray != null && attackPosArray.Length != 0)
        {
            attackPosId = Random.Range(0, attackPosArray.Length);
            attackPos = attackPosArray[attackPosId];
        }
    }


    #region 新增代码-->针对怪物的移动、动画更加多样化

    /// <summary>
    /// 怪物也能获取道具
    /// </summary>
    /// <param name="type"></param>
    public void GetDaoJu(en_Game00_DaoJuType type)
    {
        Main.Log("MonsterGetDaoJu:" + type);
        switch (type)
        {
            case en_Game00_DaoJuType.BloodPag:
                int addblood = 20000;
                remainBlood += addblood;
                if (remainBlood > blood)
                    remainBlood = blood;
                if (isBOSS)
                {
                    gameMain.gameUI.Update_BossHpValue((float)remainBlood / blood);
                }

                break;

        }
    }
    #endregion
}


