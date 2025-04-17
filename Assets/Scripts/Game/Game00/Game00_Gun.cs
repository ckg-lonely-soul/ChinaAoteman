using UnityEngine;

public enum en_GunType
{
    Normal = 0,     // 普通枪
    Jiguang1,        // 激光枪
    Jiguang2,        // 激光枪
    Jiguang3,        // 激光枪
}

public class Game00_Gun : MonoBehaviour
{
    public GameObject firePos;
    public GameObject fireEff;              // 开火特效
    public GameObject hitTargetEff;         // 击中目标特效
    public GameObject bullet_Prefab;
    public GameObject gun_Obj;
    public Animator animator_Fire;
    // 声音
    public AudioSource audioSource_Fire;
    public en_GunType gunType = en_GunType.Normal;

    public float dcTime = 0.2f;
    public int attackValue = 10;            // 攻击力
    public int attackDistance = 100;        // 最大攻击距离
    public int maxBulletNum = 0;            // 最大子弹数
    //
    public int shakeCdTime = 30;            // 震动间隔时间(ms)
    public float backForce = 0.05f;          // 后座力

    public Game00_Player player;
    int bulletNum;
    bool firePlaying;
    float fireTime = 0;
    float fireDcTime = 0;
    float fireSoundTime = 0;
    float distance;
    int[] tab_attackValue = { 10, 15, 20, 25, 30 };
    RaycastHit hit;

    public void UpdateBulletNum()
    {
        if (maxBulletNum > 0)
        {
            player.playerUI.Update_RemainBulletNum(bulletNum, maxBulletNum);
        }
    }
    public void Init(Game00_Player playerFun)
    {
        attackValue = tab_attackValue[Set.setVal.AttackPower];
        player = playerFun;
        if (Main.COMPANY_NUM == 8 && gunType == en_GunType.Normal)
        {
            audioSource_Fire.clip = player.gameMain.audioClip_Fire;
        }
#if IO_LOCAL
        audioSource_Fire.volume = 0.6f;
#endif
        firePlaying = false;
        if (animator_Fire != null)
        {
            animator_Fire.Update(1);
        }
        bulletNum = maxBulletNum;
        UpdateBulletNum();
        switch (gunType)
        {
            case en_GunType.Normal:
#if GUN_HW//红外手枪隐藏枪、开火特效，只保留准心、击中怪特效
                gun_Obj.gameObject.SetActive(false);
                firePos.gameObject.SetActive(false);
#endif
                break;
            case en_GunType.Jiguang1:
            case en_GunType.Jiguang2:
            case en_GunType.Jiguang3:
                firePos.gameObject.SetActive(false);
                dcTime = 0.05f;
                break;
        }
    }
    void Update()
    {
        if (animator_Fire != null)
        {
            if (animator_Fire.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                firePlaying = false;
            }
        }

        if (transform.localPosition.z < 0)
        {
            transform.Translate(Vector3.forward * 1 * Time.deltaTime);
            if (transform.localPosition.z > 0)
            {
                transform.localPosition = Vector3.zero;
            }
        }
        if (Main.COMPANY_NUM == 8 && gunType == en_GunType.Normal)
        {
            if (fireSoundTime > 0)
            {
                fireSoundTime -= Time.deltaTime;
                if (fireSoundTime <= 0 && audioSource_Fire.isPlaying)
                {
                    audioSource_Fire.Stop();
                }
            }
        }

        if (gunType != en_GunType.Normal)// && Set.setVal.GunMode == (int)en_GunMode.Gun)
        {
            if (fireDcTime > 0)
            {
                fireDcTime -= Time.deltaTime;
            }
            if (fireTime > 0)
            {
                fireTime -= Time.deltaTime;
                if (firePos.gameObject.activeSelf == false)
                {
                    firePos.gameObject.SetActive(true);
                }
                if (Physics.Raycast(transform.position, transform.forward, out hit, attackDistance))
                { //, layerMask    (1 << LayerMask.NameToLayer("Shell"))
                    hitTargetEff.transform.position = hit.point;
                    distance = Vector3.Distance(firePos.transform.position, hit.point);
                    //
                    if (fireDcTime <= 0)
                    {
                        fireDcTime = 0.05f;
                        //
                        bool crit = false;
                        if (hit.collider.transform.root.tag == "Monster")
                        {
                            Game00_Monster monster = hit.collider.transform.root.GetComponent<Game00_Monster>();
                            if (monster != null)
                            {
                                if (Random.Range(0, 1000) < 50)
                                {
                                    crit = true;
                                }
                                monster.Attacked(player, attackValue, crit);
                            }
                        }
                        else if (hit.collider.transform.root.tag == "DaoJu")
                        {
                            Game00_DaoJu daoju = hit.collider.transform.root.GetComponent<Game00_DaoJu>();
                            if (daoju != null)
                            {
                                daoju.Attacked(player);
                            }
                        }
                    }
                }
                else
                {
                    // 没有打到目标
                    hitTargetEff.transform.localPosition = new Vector3(0, 0, attackDistance);
                    distance = attackDistance;
                }
                bullet_Prefab.transform.localScale = new Vector3(1, 1, distance);

            }
            else if (firePos.gameObject.activeSelf)
            {
                firePos.gameObject.SetActive(false);
            }
        }
    }
    public bool Fire()
    {
        if (firePlaying)
        {
            return false;
        }
        if (transform.localPosition.z != 0)
        {
            return false;
        }
        switch (gunType)
        {
            case en_GunType.Normal:

                if (firePos != null)
                {
                    if (Set.setVal.GunMode == (int)en_GunMode.Gun)
                    {
#if SHAKE_POWER
                        IO.GunRunStart(player.Id, (byte)Set.setVal.Gun1ShakePower, (byte)(Set.setVal.Gun1ShakeTime - Set.setVal.Gun1ShakePower));
#else
                        IO.GunRunStart(player.Id, 20, 30);
#endif
                        // 子弹: 
#if !GUN_HW
                        if (bullet_Prefab != null)
                        {
                            //GameObject obj = Instantiate(bullet_Prefab, firePos.transform);
                            //对象池得到子弹，用完后不用销毁子弹，否则报错！
                            GameObject obj = PoolManager.instance.GetObj(bullet_Prefab);
                            obj.transform.parent = firePos.transform;
                            obj.transform.position = firePos.transform.position;
                            //obj.transform.eulerAngles = firePos.transform.eulerAngles;
                            obj.transform.localEulerAngles = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);
                            obj.GetComponent<Game00_Shell>().Init(player, Vector3.zero, attackValue);
                        }
#endif
                    }
                    //IO.GunMotorStart(player.Id, 0.06f);
                    //IO.GunRunStart(player.Id, 20, 30);
                    // 火花
                    if (fireEff != null)
                    {
                        //GameObject obj = Instantiate(fireEff, firePos.transform);
                        GameObject obj = PoolManager.instance.GetObj(fireEff);
                        obj.transform.parent = firePos.transform;
                        obj.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360f));
                        obj.transform.localPosition = new Vector3(0, 0, 0);
                        //    obj.transform.parent = null;
                    }
                    if (animator_Fire != null)
                    {
                        animator_Fire.Play("Fire", 0, 0);
                        firePlaying = true;
                    }
                    if (audioSource_Fire != null)
                    {
                        if (Main.COMPANY_NUM == 8 && gunType == en_GunType.Normal)
                        {
                            if (audioSource_Fire.isPlaying == false)
                            {
                                audioSource_Fire.Play();
                            }
                            fireSoundTime = 0.2f;
                        }
                        else
                        {
                            audioSource_Fire.Stop();
                            audioSource_Fire.Play();
                        }
                    }
                    //
                    transform.localPosition = new Vector3(0, 0, -backForce);

                    // 显示剩余子弹数
                    if (Set.setVal.GunMode == (int)en_GunMode.Gun)
                    {
                        if (bulletNum > 0)
                        {
                            bulletNum--;
                        }
                        UpdateBulletNum();
                        if (maxBulletNum > 0 && bulletNum == 0)
                        {
                            player.changeBulltTime = 2;
                        }
                    }

                    return true;
                }
                break;

            case en_GunType.Jiguang1:
                fireTime = 0.1f;
                // 添加震动
                if (Set.setVal.GunMode == (int)en_GunMode.Gun)
                {
                    IO.GunRunStart(player.Id, (byte)Set.setVal.Gun2ShakePower, (byte)(Set.setVal.Gun2ShakeTime - Set.setVal.Gun2ShakePower));
                    player.fireDcTime = (float)((byte)Set.setVal.Gun2ShakePower + (byte)(Set.setVal.Gun2ShakeTime - Set.setVal.Gun2ShakePower)) / 1000f;
                }
                break;
            case en_GunType.Jiguang2:
                fireTime = 0.1f;
                // 添加震动
                if (Set.setVal.GunMode == (int)en_GunMode.Gun)
                {
                    //IO.GunRunStart(player.Id, 40, (byte)shakeCdTime);
                    //player.fireDcTime = (float)(40 + shakeCdTime) / 1000f;
                    IO.GunRunStart(player.Id, (byte)Set.setVal.Gun3ShakePower, (byte)(Set.setVal.Gun3ShakeTime - Set.setVal.Gun3ShakePower));
                    player.fireDcTime = (float)((byte)Set.setVal.Gun3ShakePower + (byte)(Set.setVal.Gun3ShakeTime - Set.setVal.Gun3ShakePower)) / 1000f;
                }
                break;
            case en_GunType.Jiguang3:
                fireTime = 0.1f;
                // 添加震动
                if (Set.setVal.GunMode == (int)en_GunMode.Gun)
                {
                    //IO.GunRunStart(player.Id, 40, (byte)shakeCdTime);
                    //player.fireDcTime = (float)(40 + shakeCdTime) / 1000f;
                    IO.GunRunStart(player.Id, (byte)Set.setVal.Gun4ShakePower, (byte)(Set.setVal.Gun4ShakeTime - Set.setVal.Gun4ShakePower));
                    player.fireDcTime = (float)((byte)Set.setVal.Gun4ShakePower + (byte)(Set.setVal.Gun4ShakeTime - Set.setVal.Gun4ShakePower)) / 1000f;
                }
                break;
        }
        return false;
    }
}
