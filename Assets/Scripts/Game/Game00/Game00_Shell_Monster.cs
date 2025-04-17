using UnityEngine;

public class Game00_Shell_Monster : MonoBehaviour
{
    public GameObject dieEff_Prefab;
    public GameObject effUI_Prefab;
    public en_ShellType shellType = en_ShellType.Normal;
    public int attackValue = 10;
    public float attackSize = 5;
    public float speed;
    public float shakePower = 1f;
    public int shakeCnt = 8;
    public float minBombTime = 0.5f;
    public bool followDir = false;

    Game00_Main gameMain;
    Game00_Monster monster;
    Game00_Player player_Target;
    Vector3 newPos;

    //
    //  GameObject playerObj;
    GameObject effUI_Obj;
    public float effDis;
    //
    en_ShellSta statue = en_ShellSta.Idle;
    float runTime;
    float checkTime;

    float speedX;
    float speedY;
    public float speedZ;
    float _distance;
    float distance;
    RaycastHit hit;
    // public GameObject firePos;
    float fireDcTime;
    //点积运算
    float val;
    //public float rotateSpeed;
    public void Init(Game00_Main gamemain, Game00_Monster mon, Game00_Player target)
    {
        gameMain = gamemain;
        monster = mon;
        //player_Target = target;
        //  playerObj = monster.mainCar_Obj;
        speedX = transform.forward.x * speed;
        speedY = transform.forward.y * speed;
        speedZ = transform.forward.z * speed;

        if (shellType == en_ShellType.Bomb || shellType == en_ShellType.Sword)
        {
            statue = en_ShellSta.Idle;
        }
        else
        {
            statue = en_ShellSta.Run;
        }
        if (shellType == en_ShellType.Sword)
        {

            transform.LookAt(gameMain.players_Obj.transform.position);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, -90);
        }
        if (shellType == en_ShellType.FarmTool || shellType == en_ShellType.FireBall || shellType == en_ShellType.Bullet || shellType == en_ShellType.longjuanfeng)
        {
            transform.LookAt(gameMain.players_Obj.transform.position);
        }
        if (shellType == en_ShellType.FireBallDown)
            transform.LookAt(gameMain.players_Obj.transform.position - new Vector3(0, 1.5f, 0));
        //if (shellType == en_ShellType.Catapult)
        //{
        //    transform.LookAt(gameMain.players_Obj.transform.position + new Vector3(0,80,0));
        //}
        if (shellType == en_ShellType.tordano)
            Destroy(gameObject, 6f);
        if (shellType == en_ShellType.Sword_16)
        {

            transform.LookAt(gameMain.players_Obj.transform.position);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, -90);
        }
        runTime = 0;
        checkTime = 0;
        newPos = transform.position;
        effUI_Obj = null;
    }
    public void Fire()
    {
        statue = en_ShellSta.Run;
    }

    // Update is called once per frame
    void Update()
    {
        switch (shellType)
        {

            case en_ShellType.FarmTool:

                transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
                if (effUI_Prefab != null)
                {

                    float _distance = Vector3.Distance(transform.position, gameMain.players_Obj.transform.position);
                    if (_distance < effDis && effUI_Obj == null)
                    {
                        effUI_Obj = Instantiate(effUI_Prefab, gameMain.gameUI.transform);
                        Vector3 worldPos = transform.position;
                        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
                        effUI_Obj.transform.position = screenPos;
                        Destroy(gameObject, 0.2f);
                    }
                }
                break;
            case en_ShellType.Sword:
                transform.Translate(transform.forward * -speedZ * Time.deltaTime, Space.World);
                if (effUI_Prefab != null)
                {

                    float _distance = Vector3.Distance(transform.position, gameMain.players_Obj.transform.position);
                    if (_distance < 1.8f && effUI_Obj == null)
                    {
                        effUI_Obj = Instantiate(effUI_Prefab, gameMain.gameUI.transform);
                        Vector3 worldPos = transform.position;
                        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
                        effUI_Obj.transform.position = screenPos;
                    }
                }
                break;
            case en_ShellType.PaoDan:
                newPos = transform.position + new Vector3(speedX, speedY, speedZ) * Time.deltaTime;
                speedY -= 10 * Time.deltaTime;
                transform.LookAt(newPos);
                transform.position = newPos;
                break;
            case en_ShellType.DaoDan:
                break;
            case en_ShellType.Buttle:
                break;

            case en_ShellType.Bomb:
                if (followDir)
                {
                    checkTime += Time.deltaTime;
                    if (checkTime >= 0.1f)
                    {
                        checkTime = 0;
                        transform.rotation = Quaternion.LookRotation(transform.position - newPos);
                        newPos = transform.position;
                    }
                }
                runTime += Time.deltaTime;
                if (runTime >= 10)
                {
                    OnCollisionEnter(null);
                }
                break;

            case en_ShellType.Arrow:

                transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
                if (effUI_Prefab != null)
                {

                    _distance = Vector3.Distance(transform.position, gameMain.players_Obj.transform.position);
                    if (_distance < 3.5f && effUI_Obj == null)  //修改_distance<2.5f
                    {
                        effUI_Obj = Instantiate(effUI_Prefab, gameMain.gameUI.transform);
                        Vector3 worldPos = transform.position;
                        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
                        effUI_Obj.transform.position = screenPos;
                    }
                }
                break;
            case en_ShellType.FireBall:

                transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
                _distance = Vector3.Distance(transform.position, gameMain.players_Obj.transform.position);
                if (_distance < effDis)
                {
                    //Debug.Log(44);
                    if (effUI_Prefab != null && effUI_Obj == null)
                    {
                        effUI_Obj = Instantiate(effUI_Prefab, gameMain.gameUI.transform);
                        Vector3 worldPos = transform.position;
                        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
                        effUI_Obj.transform.position = screenPos;
                    }
                    if (dieEff_Prefab != null)
                    {
                        GameObject temp = Instantiate(dieEff_Prefab);
                        temp.transform.position = transform.position;
                        //Destroy(temp, 0.5f);
                    }
                    if (gameObject.activeInHierarchy)
                    {
                        Destroy(gameObject);
                    }

                }
                break;

            case en_ShellType.FireBallDown:

                transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);


                _distance = Vector3.Distance(transform.position, gameMain.players_Obj.transform.position);
                if (_distance < effDis)
                {
                    if (effUI_Prefab != null && effUI_Obj == null)
                    {
                        effUI_Obj = Instantiate(effUI_Prefab, gameMain.gameUI.transform);
                        Vector3 worldPos = transform.position;
                        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
                        effUI_Obj.transform.position = screenPos;
                    }
                    if (dieEff_Prefab != null)
                    {
                        GameObject temp = Instantiate(dieEff_Prefab);
                        temp.transform.position = transform.position;
                    }
                    Destroy(gameObject);


                }


                break;

            case en_ShellType.Bullet:

                transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
                if (effUI_Prefab != null)
                {

                    _distance = Vector3.Distance(transform.position, gameMain.players_Obj.transform.position);
                    if (_distance < effDis && effUI_Obj == null)
                    {
                        effUI_Obj = Instantiate(effUI_Prefab, gameMain.gameUI.transform);
                        Vector3 worldPos = transform.position;
                        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
                        effUI_Obj.transform.position = screenPos;

                        if (dieEff_Prefab != null)
                        {
                            GameObject temp = Instantiate(dieEff_Prefab);
                            temp.transform.position = transform.position;
                            Destroy(temp, 0.5f);
                        }
                        Destroy(gameObject);
                    }
                }
                break;

            #region
            //case en_ShellType.Catapult:
            //    transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
            //    if (Vector3.Angle(transform.forward, gameMain.players_Obj.transform.position - transform.position) < 90)
            //    {
            //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(gameMain.players_Obj.transform.position - transform.position), rotateSpeed * Time.deltaTime);
            //    }

            //    _distance = Vector3.Distance(transform.position, gameMain.players_Obj.transform.position);
            //    if (_distance < effDis)
            //    {
            //        if (effUI_Prefab != null && effUI_Obj == null)
            //        {
            //            effUI_Obj = Instantiate(effUI_Prefab, gameMain.gameUI.transform);
            //            Vector3 worldPos = transform.position;
            //            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            //            effUI_Obj.transform.position = screenPos;
            //        }
            //        if (dieEff_Prefab != null)
            //        {
            //            GameObject temp = Instantiate(dieEff_Prefab);
            //            temp.transform.position = transform.position;
            //        }
            //        Destroy(gameObject);
            //    }
            //    break;
            #endregion
            case en_ShellType.Sword_16:
                transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
                if (effUI_Prefab != null)
                {

                    _distance = Vector3.Distance(transform.position, gameMain.players_Obj.transform.position);
                    if (_distance < effDis && effUI_Obj == null)
                    {
                        //Debug.Log(44);
                        effUI_Obj = Instantiate(effUI_Prefab, gameMain.gameUI.transform);
                        Vector3 worldPos = transform.position;
                        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
                        effUI_Obj.transform.position = screenPos;

                        if (dieEff_Prefab != null)
                        {
                            GameObject temp = Instantiate(dieEff_Prefab);
                            temp.transform.position = transform.position;
                            Destroy(temp, 0.5f);
                        }
                        Destroy(gameObject);
                    }
                }
                break;
            case en_ShellType.longjuanfeng:

                transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
                _distance = Vector3.Distance(transform.position, gameMain.players_Obj.transform.position);
                if (_distance < effDis)
                {
                    //Debug.Log(44);
                    if (effUI_Prefab != null && effUI_Obj == null)
                    {
                        effUI_Obj = Instantiate(effUI_Prefab, gameMain.gameUI.transform);
                        Vector3 worldPos = transform.position;
                        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
                        effUI_Obj.transform.position = screenPos;
                    }
                    if (dieEff_Prefab != null)
                    {
                        GameObject temp = Instantiate(dieEff_Prefab);
                        temp.transform.position = transform.position;
                        //Destroy(temp, 0.5f);
                    }
                    //val = Vector3.Dot(transform.up, gameMain.players_Obj.transform.position - transform.position);
                    //if (val < 0)
                    //{
                    //    Destroy(gameObject);
                    //}
                }
                Destroy(gameObject, 5f);
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        switch (shellType)
        {
            case en_ShellType.PaoDan:
                // 特效
                if (dieEff_Prefab != null)
                {
                    GameObject obj = GameObject.Instantiate(dieEff_Prefab);
                    obj.transform.position = transform.position;
                    //obj.transform.eulerAngles = monster.firePos_Obj.transform.eulerAngles;
                }
                // 伤害
                if (Vector3.Distance(gameMain.players_Obj.transform.position, transform.position) < 5)
                {
                    gameMain.AttackPlayers(monster, attackValue);
                    //for (int i = 0; i < gameMain.player.Length; i++) {
                    //    gameMain.player[i].Attacked(monster, attackValue);
                    //}
                }

                if (shakePower > 0)
                {
                    gameMain.ShakeStart(shakePower, shakeCnt);
                }
                Destroy(gameObject);
                break;
            case en_ShellType.DaoDan:
                break;
            case en_ShellType.Buttle:
                break;

            case en_ShellType.Bomb:

                break;
            case en_ShellType.Sword:
                if (collider.transform.root.tag != "Monster")
                {
                    // 特效
                    if (dieEff_Prefab != null)
                    {
                        GameObject obj = GameObject.Instantiate(dieEff_Prefab);
                        obj.transform.position = transform.position;
                        //obj.transform.eulerAngles = monster.firePos_Obj.transform.eulerAngles;
                    }
                    // 伤害
                    if (Vector3.Distance(gameMain.players_Obj.transform.position, transform.position) < 3.5f)
                    {
                        gameMain.AttackPlayers(monster, attackValue);
                        //for (int i = 0; i < gameMain.player.Length; i++) {
                        //    gameMain.player[i].Attacked(monster, attackValue);
                        //}
                    }

                    if (shakePower > 0)
                    {
                        gameMain.ShakeStart(shakePower, shakeCnt);
                    }
                    Destroy(gameObject);
                }

                break;
            case en_ShellType.FarmTool:
                if (collider.transform.root.tag != "Monster")
                {
                    // 特效
                    if (dieEff_Prefab != null)
                    {
                        GameObject obj = GameObject.Instantiate(dieEff_Prefab);
                        obj.transform.position = transform.position;
                        //obj.transform.eulerAngles = monster.firePos_Obj.transform.eulerAngles;
                    }
                    // 伤害
                    if (Vector3.Distance(gameMain.players_Obj.transform.position, transform.position) < 5)
                    {
                        gameMain.AttackPlayers(monster, attackValue);
                        //for (int i = 0; i < gameMain.player.Length; i++) {
                        //    gameMain.player[i].Attacked(monster, attackValue);
                        //}
                    }

                    if (shakePower > 0 && monster.isBOSS)
                    {
                        gameMain.ShakeStart(shakePower, shakeCnt);
                    }
                    Destroy(gameObject);
                }

                break;

            case en_ShellType.Arrow:
                if (collider.transform.root.tag != "Monster")
                {
                    // 特效
                    if (dieEff_Prefab != null)
                    {
                        GameObject obj = GameObject.Instantiate(dieEff_Prefab);
                        obj.transform.position = transform.position;
                        Destroy(obj, 0.7F);
                    }
                    // 伤害
                    if (Vector3.Distance(gameMain.players_Obj.transform.position, transform.position) < 5)
                    {
                        //gameMain.AttackPlayers(monster, attackValue);
                        //for (int i = 0; i < gameMain.player.Length; i++) {
                        //    gameMain.player[i].Attacked(monster, attackValue);
                        //}
                    }

                    if (shakePower > 0 && monster.isBOSS)
                    {

                        gameMain.ShakeStart(shakePower, shakeCnt);

                    }
                    Destroy(gameObject);


                }
                break;
            default:
                break;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        switch (shellType)
        {
            case en_ShellType.Bomb:
                if (runTime >= minBombTime && statue == en_ShellSta.Run)
                {
                    if (Vector3.Distance(transform.position, gameMain.players_Obj.transform.position) < attackSize)
                    {
                        gameMain.AttackPlayers(monster, attackValue);
                    }
                    //
                    if (dieEff_Prefab != null)
                    {
                        GameObject obj = GameObject.Instantiate(dieEff_Prefab);
                        obj.transform.position = transform.position;
                    }
                    statue = en_ShellSta.Die;
                    gameMain.ShakeStart(shakePower, shakeCnt);
                    Destroy(gameObject);
                }
                break;
            case en_ShellType.Sword:
                if (other.transform.root.tag != "Monster")
                {
                    if (runTime >= minBombTime && statue == en_ShellSta.Run)
                    {
                        if (Vector3.Distance(transform.position, gameMain.players_Obj.transform.position) < attackSize)
                        {
                            gameMain.AttackPlayers(monster, attackValue);
                        }
                        //
                        if (dieEff_Prefab != null)
                        {
                            GameObject obj = GameObject.Instantiate(dieEff_Prefab);
                            obj.transform.position = transform.position;
                        }
                        statue = en_ShellSta.Die;
                        gameMain.ShakeStart(shakePower, shakeCnt);
                        Destroy(gameObject);
                    }
                }


                break;
            case en_ShellType.FarmTool:
                if (other.transform.root.tag != "Monster")
                {
                    if (runTime >= minBombTime && statue == en_ShellSta.Run)
                    {
                        if (Vector3.Distance(transform.position, gameMain.players_Obj.transform.position) < attackSize)
                        {
                            gameMain.AttackPlayers(monster, attackValue);
                        }
                        //
                        if (dieEff_Prefab != null)
                        {
                            GameObject obj = GameObject.Instantiate(dieEff_Prefab);
                            obj.transform.position = transform.position;
                        }
                        statue = en_ShellSta.Die;
                        gameMain.ShakeStart(shakePower, shakeCnt);
                        Destroy(gameObject);
                    }
                }

                break;

            case en_ShellType.Arrow:
                if (other.transform.root.tag != "Monster")
                {
                    // 特效
                    if (dieEff_Prefab != null)
                    {
                        GameObject obj = GameObject.Instantiate(dieEff_Prefab);
                        obj.transform.position = transform.position;
                        Destroy(obj, 0.7F);
                    }
                    // 伤害
                    if (Vector3.Distance(gameMain.players_Obj.transform.position, transform.position) < 5)
                    {
                        //gameMain.AttackPlayers(monster, attackValue);
                        //for (int i = 0; i < gameMain.player.Length; i++) {
                        //    gameMain.player[i].Attacked(monster, attackValue);
                        //}
                    }

                    if (shakePower > 0 && monster.isBOSS)
                    {

                        gameMain.ShakeStart(shakePower, shakeCnt);

                    }
                    Destroy(gameObject);


                }
                break;
        }
    }
}
