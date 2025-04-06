using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum en_AttackSkillSta
{
    Start = 0,
    Run,
    Over,
    End,
}

public class Game00_AttackSkill : MonoBehaviour
{
    public Animator animator;
    public AnimEvent animEvent;
    public GameObject[] firePos;
    public GameObject shell_Prefab;
    public GameObject attackEff_Prefab;
    public GameObject attackEffUI_Prefab;
    public AudioSource audioSource_AttackStart;     // 攻击(开始)声音
    public AudioSource audioSource_AttackEffect;    // 攻击(生效)声音
    public string animStartName = "Attack";
    public string animEndName = "AttackEnd";
    public bool isFarAttack;
    public bool effectByAnimation = true;       // 攻击生效以动画为标准，否则以时间
    public bool isContinueAttack = false;       // 持续攻击
    public int attackValue = 10;                // 攻击力
    public int attackCnt = 1;                   // 攻击次数
    public float attackTime = 1f;               // 攻击(持续)时间
    public float onceDcTime = 0.5f;             // 单次攻击生效时间间隔

    public float minAttackDistance = 0.5f;	    // 攻击距离
    public float maxAttackDistance = 1.3f;	    // 攻击距离
    public float readyAttackDistance = 1.0f;    // 准备攻击距离

    public float stopMainCarTime = 0f;          // 停止主相机时间
    public float shakePower = 0f;
    public int shakeCnt = 0;

    //public bool attackEffected;

    //
    Game00_Monster monster;
    Game00_Main gameMain;

    public en_AttackSkillSta statue;
    float runTime;
    float dcTime;
    int attackedCnt;
    int posId;


    public bool shell_down;
    // Use this for initialization
    public void Init(Game00_Monster mons)
    {
        monster = mons;
        gameMain = monster.gameMain;
        gameObject.SetActive(false);
    }
    public void Attack()
    {
        animEvent.Clear();
        runTime = 0;
        dcTime = 0;
        attackedCnt = 0;
        posId = 0;
        //attackEffected = false;
        //
        gameObject.SetActive(true);
        ChangeStatue(en_AttackSkillSta.Start);
    }

    // Update is called once per frame
    void Update()
    {
        if (dcTime > 0)
        {
            dcTime -= Time.deltaTime;
        }

        switch (statue)
        {
            case en_AttackSkillSta.Start:
                // gameMain.AttackPlayers(monster, attackValue);

                if (animEvent.event_AttackEffect)
                {
                    animEvent.event_AttackEffect = false;
                    // 动画控制攻击生效:
                    if (effectByAnimation)
                    {
                        //attackEffected = true;
                        AttackEffct();
                        attackedCnt++;
                        if (attackedCnt >= attackCnt)
                        {
                            ChangeStatue(en_AttackSkillSta.Over);
                        }
                    }
                    else
                        if (isContinueAttack)
                        {
                            // 持续攻击
                            ChangeStatue(en_AttackSkillSta.Run);
                        }
                        else
                        {
                            // 单次攻击
                            //attackEffected = true;
                            AttackEffct();
                            ChangeStatue(en_AttackSkillSta.Over);
                        }
                }
                break;
            case en_AttackSkillSta.Run:
                // 时间控制：持续攻击
                if (dcTime <= 0)
                {
                    dcTime = onceDcTime;
                    //attackEffected = true;
                    AttackEffct();
                }
                if (attackTime > 0)
                {
                    runTime += Time.deltaTime;
                    if (runTime >= attackTime)
                    {
                        ChangeStatue(en_AttackSkillSta.Over);
                    }
                }
                break;
            case en_AttackSkillSta.Over:
                if (runTime < 0.1f)
                {
                    runTime += Time.deltaTime;
                    break;
                }
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    ChangeStatue(en_AttackSkillSta.End);
                }
                break;
            case en_AttackSkillSta.End:
                break;
        }
    }

    void ChangeStatue(en_AttackSkillSta sta)
    {
        statue = sta;
        runTime = 0;
        switch (statue)
        {
            case en_AttackSkillSta.Start:
                if (animator.enabled == false)
                    animator.enabled = true;
                animator.Play(animStartName, 0, 0);
                if (audioSource_AttackStart != null)
                {
                    audioSource_AttackStart.Play();
                }
                break;
            case en_AttackSkillSta.Run:
                break;
            case en_AttackSkillSta.Over:
                animator.Play(animEndName);
                break;
            case en_AttackSkillSta.End:
                gameObject.SetActive(false);
                break;
        }
    }
    public bool IsPlayEnd()
    {
        if (statue == en_AttackSkillSta.End)
            return true;
        return false;
    }

    void AttackEffct()
    {
        //
        if (effectByAnimation)
        {
            posId = animEvent.firePosId;
        }
        else
        {
            posId++;
            if (posId >= firePos.Length)
            {
                posId = 0;
            }
        }

        if (shell_Prefab != null)
        {
            if (shell_down)
            {
                for (int i = 0; i < firePos.Length; i++)
                {
                    GameObject obj = GameObject.Instantiate(shell_Prefab);
                    obj.transform.position = firePos[i].transform.position;
                    obj.transform.eulerAngles = firePos[i].transform.eulerAngles;
                    Game00_Shell_Monster shell = obj.GetComponent<Game00_Shell_Monster>();
                    shell.speed = shell.speed - ((firePos.Length - i) * 6);
                    shell.Init(gameMain, monster, null);
                }
            }
            else
            {
                GameObject obj = GameObject.Instantiate(shell_Prefab);
                obj.transform.position = firePos[posId].transform.position;
                obj.transform.eulerAngles = firePos[posId].transform.eulerAngles;
                Game00_Shell_Monster shell = obj.GetComponent<Game00_Shell_Monster>();
                shell.Init(gameMain, monster, null);
            }

        }
        if (attackEff_Prefab != null)
        {
            GameObject obj = GameObject.Instantiate(attackEff_Prefab);
            obj.transform.position = firePos[posId].transform.position;
            obj.transform.eulerAngles = firePos[posId].transform.eulerAngles;
        }
        if (attackEffUI_Prefab != null)
        {
            if (Main.statue == en_MainStatue.Game_00)
            {
                for (int i = 0; i < firePos.Length; i++)
                {
                    GameObject obj = GameObject.Instantiate(attackEffUI_Prefab, gameMain.gameUI.transform);
                    obj.transform.position = Camera.main.WorldToScreenPoint(firePos[i].transform.position);
                }
            }
            else
            {
                GameObject obj = GameObject.Instantiate(attackEffUI_Prefab, gameMain.gameUI.transform);
                obj.transform.position = Camera.main.WorldToScreenPoint(firePos[posId].transform.position);
            }

        }
        if (shakePower > 0)
        {
            gameMain.ShakeStart(shakePower, shakeCnt);
        }
        if (stopMainCarTime > 0)
        {
            gameMain.RunStop(stopMainCarTime);
        }
        //
        gameMain.AttackPlayers(monster, attackValue);
        //
        if (audioSource_AttackEffect != null)
        {
            audioSource_AttackEffect.Play();
        }
    }
}
