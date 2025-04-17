using UnityEngine;
using UnityEngine.UI;


enum en_OpenBosSta
{
    Start = 0,
    Move,
    Idle,
    Open,
    Result,
    End,
}

public class Game00_OpenBox : MonoBehaviour
{
    public Image[] iamge_Box;
    public Animator[] animator_Box;
    public GameObject[] daoJuIcon_Prefab;

    Game00_Player player;
    Game00_Main gameMain;
    en_OpenBosSta statue;
    float runTime;
    int boxSel;
    int moveId;
    int moveIndex;
    Vector3 moveTargetPos;
    Vector3 moveLastPos;

    readonly int[] tab_BoxMoveToPos = { 4, 1, 5, 3, 2, 0 };
    //#if UNITY_EDITOR
    //    void Start() {
    //        Init(null);
    //    }
    //#endif
    public void Init(Game00_Player playerFun)
    {
        player = playerFun;
        if (player != null)
        {
            gameMain = player.gameMain;
        }
        //
        Key.Clear();
        for (int i = 0; i < animator_Box.Length; i++)
        {
            animator_Box[i].enabled = false;
            animator_Box[i].Update(0);
        }
        moveIndex = 0;
        moveLastPos = animator_Box[tab_BoxMoveToPos[5]].transform.localPosition;
        UpdateMovePos();
        ChangeStatue(en_OpenBosSta.Start);
    }
    void UpdateMovePos()
    {
        moveTargetPos = moveLastPos;
        moveId = tab_BoxMoveToPos[moveIndex];
        moveLastPos = animator_Box[moveId].transform.localPosition;
        //    print("moveIndex: " + moveIndex + ", moveLastPos: " + moveLastPos + ", moveTargetPos: " + moveTargetPos);
    }
    // Update is called once per frame
    void Update()
    {
        switch (statue)
        {
            case en_OpenBosSta.Start:
                runTime += Time.deltaTime;
                if (runTime >= 0.9f)
                {
                    ChangeStatue(en_OpenBosSta.Move);
                }
                break;
            case en_OpenBosSta.Move:
                animator_Box[moveId].transform.localPosition = Vector3.MoveTowards(animator_Box[moveId].transform.localPosition, moveTargetPos, 4000 * Time.deltaTime);
                if (animator_Box[moveId].transform.localPosition == moveTargetPos)
                {
                    moveIndex++;
                    if (moveIndex >= tab_BoxMoveToPos.Length)
                    {
                        ChangeStatue(en_OpenBosSta.Idle);
                        break;
                    }
                    else
                    {
                        UpdateMovePos();
                    }
                }
                break;

            case en_OpenBosSta.Idle:
                runTime += Time.deltaTime;
                if (runTime >= 15)
                {
                    Destroy(gameObject);
                    return;
                }
                if (Key.KEYFJ_OkPressed(player.Id) || Set.setVal.GunMode == (int)en_GunMode.ShootWater)
                {
                    for (int i = 0; i < iamge_Box.Length; i++)
                    {
                        if (Main.IsOnButton(player.playerUI.image_Cursor, iamge_Box[i]))
                        {
                            boxSel = i;
                            animator_Box[i].enabled = true;
                            //animator_Box[i].Play(0);
                            ChangeStatue(en_OpenBosSta.Open);
                        }
                    }
                }
                break;
            case en_OpenBosSta.Open:
                if (animator_Box[boxSel].GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f)
                {
                    //
                    int wins = JL.GetWinNum(player.Id, FjData.g_Fj[player.Id].GameTime);
                    if (wins > 0)
                    {
                        FjData.g_Fj[player.Id].Wins += wins;
                        //
                        int uiPic = 5;
                        if (Set.setVal.OutMode == (int)en_OutMode.OutTicket)
                        {
                            uiPic = 5;
                        }
                        else if (Set.setVal.OutMode == (int)en_OutMode.OutGift)
                        {
                            uiPic = 6;
                        }
                        GameObject uiObj = GameObject.Instantiate(daoJuIcon_Prefab[uiPic], player.playerUI.transform);
                        uiObj.transform.position = animator_Box[boxSel].transform.position;
                        Num num = uiObj.GetComponentInChildren<Num>();
                        if (num != null)
                        {
                            num.UpdateShow(wins);
                        }
                        ChangeStatue(en_OpenBosSta.Result);
                        break;
                    }
                    int daojuType;
                    int jl = Random.Range(0, 1000);
                    if (jl < 300)
                    {
                        daojuType = (int)en_Game00_DaoJuType.BloodPag;
                    }
                    else if (jl < 600)
                    {
                        daojuType = (int)en_Game00_DaoJuType.DaoDan;
                    }
                    else
                    {
                        daojuType = (int)en_Game00_DaoJuType.DaoDan;
                    }
                    GameObject obj = GameObject.Instantiate(daoJuIcon_Prefab[daojuType], player.playerUI.transform);
                    obj.transform.position = animator_Box[boxSel].transform.position;
                    player.GetDaoJu((en_Game00_DaoJuType)daojuType);
                    ChangeStatue(en_OpenBosSta.Result);
                }
                break;
            case en_OpenBosSta.Result:
                runTime += Time.deltaTime;
                if (runTime >= 1.5f)
                {
                    Key.Clear();
                    Destroy(gameObject);
                    ChangeStatue(en_OpenBosSta.End);
                    return;
                }
                break;
            case en_OpenBosSta.End:
                break;
        }
    }
    void ChangeStatue(en_OpenBosSta sta)
    {
        statue = sta;
        runTime = 0;
        // 清掉按键
        Key.KEYFJ_OkPressed(player.Id);
    }
}
