using UnityEngine;
using UnityEngine.UI;

public class Game00_PlayerUI : MonoBehaviour
{
    public Animator animator_Combo;
    public Image image_Cursor;
    public Image image_BloodValue;//血量
    public Image image_ComboText;
    public Image image_ScoreBackG;
    public Num num_Combo;
    public Num num_Score;
    public Num num_Bullet;      //子弹数量
    public Num num_Bullet1;      //弹夹子弹数
    public Num num_Bullet2;      //子弹总数
    public Image image_XiShengc;//牺牲中文
    public Image image_XiShenge;
    public Image image_ContinueC;
    public Image image_ContinueE;
    public Image image_PleaseCoinIn;
    public Image image_ContinueText;
    public Num num_ContinueTimeout;     //是否续币  倒计时 
    public Image image_SpecialAttackFlag;    //攻击道具图标
    public GameObject remainBulletNum_Obj;
    public GameObject RemainBulletNum;
    public Image[] image_RemainBulletNum;
    public Game_Out gameOut;
    public Game00_CoinIn coinIn;
    public Transform hitEffTran;//击中特效的父物体
    public Text text_Score;
    Game00_Player player;
    Sprite[] sprite_NumTime;

    int playerId;
    float comboTime;    //连续暴击
    bool checkPleaseCoin;
    Vector3 coinsPos;

    //修改
    Sprite sprite_PleasecoinIn;
    /// <summary>
    /// 导弹图标？
    /// </summary>
    Sprite[] sprite_SpecialAttackFlag;

    public void Awake0(Game00_Player playerFun)
    {
        player = playerFun;
        sprite_NumTime = Resources.LoadAll<Sprite>("Company_00/Pic/Num_Time");     //没找到
        coinsPos = coinIn.transform.localPosition;
        if (RemainBulletNum != null)
            image_RemainBulletNum = RemainBulletNum.GetComponentsInChildren<Image>();
    }
    
    int language = -1;
    string[] tab_StrLanguage = { "CN", "EN" };
    
    public void GameStart(int no)
    {

        playerId = no;
        //修改图标--语言
        string strCompany = "";
        if (language != Set.setVal.Language)
        {
            language = Set.setVal.Language;
        }
        if (Set.setVal.Language == (int)en_Language.Chinese)
        {
            sprite_PleasecoinIn = Resources.Load<Sprite>(strCompany + "Import/UI/Coins_Please/cn/0000");        //中文  请投币
            sprite_SpecialAttackFlag = Resources.LoadAll<Sprite>("Company_00/Common/ShellMode_cn");

            image_ContinueC.gameObject.SetActive(true);//打开中文是否投币
            //image_XiShengc.gameObject.SetActive(true);

            image_ContinueE.gameObject.SetActive(false);
            image_XiShenge.gameObject.SetActive(false);
        }
        else
        {
            sprite_PleasecoinIn = Resources.Load<Sprite>(strCompany + "Import/UI/Coins_Please/en/0000");
            sprite_SpecialAttackFlag = Resources.LoadAll<Sprite>("Company_00/Common/ShellMode_en");
        }

        image_PleaseCoinIn.sprite = Instantiate(sprite_PleasecoinIn) as Sprite;
        image_Cursor.SetNativeSize();

        if (animator_Combo != null)
        {
            animator_Combo.gameObject.SetActive(false);
        }
        if (image_SpecialAttackFlag != null)
        {
            //修改
            image_SpecialAttackFlag.sprite = Instantiate(sprite_SpecialAttackFlag[1]) as Sprite;
            image_SpecialAttackFlag.SetNativeSize();

            image_SpecialAttackFlag.gameObject.SetActive(false);
        }
        if (image_ContinueText != null)
        {
            image_ContinueText.gameObject.SetActive(false);
        }
        image_PleaseCoinIn.gameObject.SetActive(false);
        if (gameOut != null)
        {
            gameOut.Init(playerId);
        }
        

        if (no == 0)
        {
            if (Set.setVal.PlayerMode == (int)en_PlayerMode.One)
            {
                if (num_Bullet != null)
                    num_Bullet.transform.parent.localPosition = new Vector2(-180, 500);
                if (num_Bullet1 != null)
                    num_Bullet1.transform.parent.localPosition = new Vector2(-180, 500);

                //if (num_Score != null)
                //    num_Score.transform.parent.localPosition = new Vector2(-30, -254);//分数的出现位置
                //if (coinIn != null)
                //    coinIn.transform.localPosition = new Vector2(820, -320);

                //if (image_ContinueText != null)
                //    image_ContinueText.transform.localPosition = new Vector2(-transform.localPosition.x, 0);//是否继续
                if (gameOut != null)
                    gameOut.transform.localPosition = new Vector2(-transform.localPosition.x, 0);

                //if (image_PleaseCoinIn != null)
                //    image_PleaseCoinIn.transform.localPosition = new Vector2(-transform.localPosition.x, image_PleaseCoinIn.transform.localPosition.y);
            }
            else
            {
                if (num_Bullet != null)
                    num_Bullet.transform.parent.localPosition = new Vector2(-180, 500);
                if (num_Bullet1 != null)
                    num_Bullet1.transform.parent.localPosition = new Vector2(-180, 500);
                //if (num_Score != null)
                //    num_Score.transform.parent.localPosition = new Vector2(-30, -254);
                //if (image_ContinueText != null)
                //    image_ContinueText.transform.localPosition = new Vector2(0, 20);
                if (gameOut != null)
                    gameOut.transform.localPosition = new Vector2(0, 200);
                //if (image_PleaseCoinIn != null)
                //    image_PleaseCoinIn.transform.localPosition = new Vector2(0, image_PleaseCoinIn.transform.localPosition.y);
            }
        }
        if (Set.setVal.InOutMode == (int)en_InOutMode.OneInOneOut)//单投单退投币数位置
        {
            if (no != 0)
            {
                coinIn.gameObject.SetActive(false);
            }
            else
            {
                coinIn.gameObject.SetActive(true);
                //coinIn.transform.localPosition = new Vector3(320f, -320f, 0);
            }
        }
        else
        {
            coinIn.gameObject.SetActive(true);
        }

        if (remainBulletNum_Obj != null)
        {
            if (Set.setVal.InOutMode == (int)en_InOutMode.OneInOneOut || Set.setVal.PlayerMode == (int)en_PlayerMode.One)
                remainBulletNum_Obj.transform.localPosition = new Vector2(remainBulletNum_Obj.transform.localPosition.x, 215);
            else
                remainBulletNum_Obj.transform.localPosition = new Vector2(remainBulletNum_Obj.transform.localPosition.x, 215);
        }

        coinIn.Init(playerId);

        if (image_BloodValue != null)
            image_BloodValue.transform.parent.SetActive(false);
    }
    
    void Update()
    {
        // 请投币
        if (checkPleaseCoin)
        {
            if (Main.IsCanGamePlay(playerId))//币数够玩
            {
                if (image_PleaseCoinIn.gameObject.activeSelf)
                {
                    image_PleaseCoinIn.gameObject.SetActive(false);
                }
            }
            else if (image_PleaseCoinIn.gameObject.activeSelf == false && !FjData.g_Fj[playerId].Played)
            {
                image_PleaseCoinIn.gameObject.SetActive(true);
            }
        }
        // 连击:
        if (animator_Combo != null)
        {
            if (comboTime > 0)
            {
                comboTime -= Time.deltaTime;
                if (comboTime <= 0)
                {
                    animator_Combo.Play("Destroy");
                }
            }
            else if (animator_Combo.gameObject.activeSelf)
            {
                if (animator_Combo.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    animator_Combo.gameObject.SetActive(false);
                }
            }
        }
    }

    public void SetCheckPleaseCoinIn(bool open)
    {
        checkPleaseCoin = open;
        image_PleaseCoinIn.gameObject.SetActive(false);
    }

    public void Update_RemainBulletNum(int remain, int max)
    {
        int value1;
        int value2;
        value2 = remain;

        //400发子弹
        value1 = max;

        if (num_Bullet != null)
        {
            num_Bullet.UpdateShow(value2);
        }
        if (num_Bullet1 != null && value1 >= 0)
        {
            num_Bullet1.UpdateShow(400);
        }

        if (image_RemainBulletNum == null || max <= 0)
            return;
        if (image_RemainBulletNum.Length == 0)
            return;
        int adv = max / image_RemainBulletNum.Length;
        if (adv < 1)
            adv = 1;
        int rnum = remain / adv;
        if ((remain % adv) > 0)
        {
            rnum++;
        }
        float bl = (float)rnum / 20;
        for (int i = 0; i < image_RemainBulletNum.Length; i++)
        {
            if (i < rnum)
                image_RemainBulletNum[i].gameObject.SetActive(true);
            else
                image_RemainBulletNum[i].gameObject.SetActive(false);
        }
    }

    public void Update_BloodValue(float value)
    {

        if (value < 0)
            value = 0;
        else if (value > 1)
            value = 1;
        image_BloodValue.fillAmount = value;
    }

    public void Update_ScoreNum(int value)
    {
        num_Score.UpdateShow(value);
    }

    public void Update_Score(int value)
    {
        if (text_Score != null)
        {
            text_Score.text = value.ToString();
        }
    }

    public void Update_ComboNum(int value)
    {
        animator_Combo.gameObject.SetActive(true);
        num_Combo.UpdateShow(value);
        comboTime = 1.5f;
    }

    public void Update_SpecialAttackTime(float value)
    {
        if (value < 0)
            value = 0;
        else if (value > 1)
            value = 1;
    }
    
    public void Update_ContinueTime(int value)
    {
        num_ContinueTimeout.UpdateShow(value);
        num_ContinueTimeout.transform.localPosition = new Vector2
            (num_ContinueTimeout.transform.localPosition.x, -54);
    }
}
