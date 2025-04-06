using System.Collections;
using System.Collections.Generic;
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
    //public Image image_Yes;//是
    //public Image image_No;
    public Image image_ContinueC;
    public Image image_ContinueE;
    public Image image_PleaseCoinIn;
    public Image image_ContinueText;
    public Num num_ContinueTimeout;     //是否续币  倒计时 
    public Image image_SpecialAttackFlag;    //攻击道具图标
    public GameObject remainBulletNum_Obj;
    // public Image image_RemainBulletNum;
    public GameObject RemainBulletNum;
    public Image[] image_RemainBulletNum;
    //public Image image_SpecialAttackTime;
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
        string strCompay = "";
        if (Main.COMPANY_NUM == 4 || Main.COMPANY_NUM == 9 || Main.COMPANY_NUM == 11 || Main.COMPANY_NUM == 12)
        {
            strCompay = "Company_" + Main.COMPANY_NUM.ToString("D2") + "/";
        }
        if(language != Set.setVal.Language)
        {
            language = Set.setVal.Language;
        }
        if (Set.setVal.Language == (int)en_Language.Chinese)
        {
            sprite_PleasecoinIn = Resources.Load<Sprite>(strCompay + "Pic/Game97/CoinPlease_cn/0000");        //中文  请投币
            sprite_SpecialAttackFlag = Resources.LoadAll<Sprite>("Company_00/Common/ShellMode_cn");
            
            //image_No.gameObject.SetActive (true);

            //image_Yes.gameObject.SetActive (true);//是否

            image_ContinueC.gameObject.SetActive(true);//打开中文是否投币
            image_XiShengc.gameObject.SetActive(true);//牺牲

            image_ContinueE.gameObject.SetActive(false);
            image_XiShenge.gameObject.SetActive(false);//攻击图标
        }
        else
        {
            //image_continueC.gameObject.SetActive (false);
            //image_continueE.gameObject.SetActive (true);
            //	image_no.gameObject.SetActive (false);
            //	image_yes.gameObject.SetActive (false);
            //	image_XiShengc.gameObject.SetActive (false);
            sprite_PleasecoinIn = Resources.Load<Sprite>(strCompay + "Pic/Game97/CoinPlease_en/0000");
            sprite_SpecialAttackFlag = Resources.LoadAll<Sprite>("Company_00/Common/ShellMode_en");
        }
        if (Main.COMPANY_NUM == 12)
        {
            coinIn.image_Coins.sprite = Resources.Load<Sprite>(strCompay + "Pic/Game97/Coins/Coins/Coins_" + tab_StrLanguage[language]);
            coinIn.image_Coins.transform.localScale = Vector3.one;
            coinIn.coin_Text.font = Resources.Load<Font>(strCompay + "Pic/Game97/Coins/Font/Num_Coins");
            coinIn.coin_Text.transform.localPosition = new Vector3(35f, 5f);
            if (playerId == 0)
            {
                coinIn.transform.localPosition = new Vector3(-160f, -320f);
            }
            else
            {
                coinIn.transform.localPosition = new Vector3(190f, -320f);
            }
        }
        else
        {
            coinIn.image_Coins.sprite = Resources.Load<Sprite>("Company_00/Common/CoinInGame/" + playerId.ToString("D3"));
            coinIn.image_Coins.transform.localScale = Vector3.one * 1.2f;
            coinIn.coin_Text.font = Resources.Load<Font>("Company_00/Common/Font/Num_Coins");
            if (playerId == 0)
            {
                coinIn.transform.localPosition = new Vector3(-200f, -330f);
                coinIn.coin_Text.transform.localPosition = new Vector3(22f, 21f);
            }
            else
            {
                coinIn.transform.localPosition = new Vector3(200f, -330f);
                coinIn.coin_Text.transform.localPosition = new Vector3(-16f, 21f);
            }
        }
        coinIn.image_Coins.SetNativeSize();
        image_PleaseCoinIn.sprite = Instantiate(sprite_PleasecoinIn) as Sprite;
        if (Main.COMPANY_NUM == 7)
        {
            image_PleaseCoinIn.transform.localPosition = new Vector3(0, -280);
        }
        if (Main.COMPANY_NUM == 4 || Main.COMPANY_NUM == 12)
        {
            image_PleaseCoinIn.SetNativeSize();       //初始化请投币大小
        }
        if (Main.COMPANY_NUM != 5)
        {
            image_Cursor.SetNativeSize();
        }


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
                if (Main.COMPANY_NUM == 5)
                {
                    if (remainBulletNum_Obj != null)
                    {
                        remainBulletNum_Obj.transform.localPosition = new Vector2(782, -327);
                        remainBulletNum_Obj.transform.localEulerAngles = new Vector3(0, 180, 0);
                    }
                    if (num_Score != null)
                    {
                        num_Score.transform.parent.localPosition = new Vector2(816, 320);//825, 308    分数的出现位置
                        image_ScoreBackG.transform.localPosition = new Vector2(18, 0);
                        image_ScoreBackG.transform.localEulerAngles = new Vector3(0, 180, 0);
                    }
                }
                else
                {
                    if (num_Bullet != null)
                        num_Bullet.transform.parent.localPosition = new Vector2(-180, 500);
                    if (num_Bullet1 != null)
                        num_Bullet1.transform.parent.localPosition = new Vector2(-180, 500);
                    /*if (num_Bullet2 != null)
                        num_Bullet2.transform.parent.localPosition = new Vector2(-180, 500);*/
                    if (num_Score != null)
                        num_Score.transform.parent.localPosition = new Vector2(-210, -330);//825, 308    分数的出现位置
                    if (coinIn != null)
                        coinIn.transform.localPosition = new Vector2(820, -320);
                }
                if (image_ContinueText != null)
                    image_ContinueText.transform.localPosition = new Vector2(-transform.localPosition.x, 0);//是否继续
                if (gameOut != null)
                    gameOut.transform.localPosition = new Vector2(-transform.localPosition.x, 0);    //
                //if (remainBulletNum_Obj != null)
                //    remainBulletNum_Obj.transform.localPosition = new Vector2(-182, -325);                    
                if (image_PleaseCoinIn != null)
                    image_PleaseCoinIn.transform.localPosition = new Vector2(-transform.localPosition.x, image_PleaseCoinIn.transform.localPosition.y);
            }
            else
            {
                if (Main.COMPANY_NUM == 5)
                {
                    if (remainBulletNum_Obj != null)
                    {
                        remainBulletNum_Obj.transform.localPosition = new Vector2(-160, -276);
                        remainBulletNum_Obj.transform.localEulerAngles = new Vector3(0, 0, 0);
                    }
                    if (num_Score != null)
                    {
                        num_Score.transform.parent.localPosition = new Vector2(-192, 251);//825, 308    分数的出现位置
                        image_ScoreBackG.transform.localPosition = new Vector2(-18, 0);
                        image_ScoreBackG.transform.localEulerAngles = new Vector3(0, 0, 0);
                    }
                }
                else
                {
                    if (num_Bullet != null)
                        num_Bullet.transform.parent.localPosition = new Vector2(-180, 500);
                    if (num_Bullet1 != null)
                        num_Bullet1.transform.parent.localPosition = new Vector2(-180, 500);
                    /*if (num_Bullet2 != null)
                        num_Bullet2.transform.parent.localPosition = new Vector2(-180, 500);*/
                    if (num_Score != null)
                        num_Score.transform.parent.localPosition = new Vector2(-190, -255);//-218, 247.5f
                    //if (coinIn != null)
                    //    coinIn.transform.localPosition = new Vector2(-145, -336);
                }
                if (image_ContinueText != null)
                    image_ContinueText.transform.localPosition = new Vector2(0, 20);//62
                if (gameOut != null)
                    gameOut.transform.localPosition = new Vector2(0, 200);
                //if (remainBulletNum_Obj != null)
                //    remainBulletNum_Obj.transform.localPosition = new Vector2(-182, -274);                    
                if (image_PleaseCoinIn != null)
                    image_PleaseCoinIn.transform.localPosition = new Vector2(0, image_PleaseCoinIn.transform.localPosition.y);
            }
        }
        if (Set.setVal.InOutMode == (int)en_InOutMode.OneInOneOut)//单投单退投币数位置
        {
            if(no != 0)
            {
                coinIn.gameObject.SetActive(false);
            }
            else
            {
                coinIn.gameObject.SetActive(true);
                coinIn.transform.localPosition = new Vector3(320f, -320f, 0);
            }
        }
        else
        {
            coinIn.gameObject.SetActive(true);
        }
        //if (Set.setVal.PlayerMode == (int)en_PlayerMode.Two)
        //{

        //}
        //// 投币
        //else if (Set.setVal.InOutMode == (int)en_InOutMode.OneInOneOut)
        //{
        //    coinIn.gameObject.SetActive(false);

        //    //			Debug.Log (10);
        //}
        //else
        //{
        //    //Game00_CoinIn.MethA();
        //    //image_Cover.gameObject.SetActive (true);
        //    coinIn.gameObject.SetActive(true);
        //    if (Set.setVal.InOutMode == (int)en_InOutMode.OneInOneOut)
        //    {
        //        //if (image_PleaseCoinIn != null)
        //        //    image_PleaseCoinIn.transform.localPosition = new Vector2(-transform.localPosition.x, image_PleaseCoinIn.transform.localPosition.y);
        //        //coinIn.transform.localPosition = new Vector3(360, coinsPos.y, 0);

        //    }
        //    else
        //    {

        //        //coinIn.transform.localPosition = coinsPos;
        //    }
        //}

        if (remainBulletNum_Obj != null)
        {
            if (Main.COMPANY_NUM == 5)
            {
                if (Set.setVal.InOutMode == (int)en_InOutMode.OneInOneOut || Set.setVal.PlayerMode == (int)en_PlayerMode.One)
                    remainBulletNum_Obj.transform.localPosition = new Vector2(remainBulletNum_Obj.transform.localPosition.x, -327);//-325
                else
                    remainBulletNum_Obj.transform.localPosition = new Vector2(remainBulletNum_Obj.transform.localPosition.x, -276);//-274
            }
            else
            {
                if (Set.setVal.InOutMode == (int)en_InOutMode.OneInOneOut || Set.setVal.PlayerMode == (int)en_PlayerMode.One)
                    remainBulletNum_Obj.transform.localPosition = new Vector2(remainBulletNum_Obj.transform.localPosition.x, 215);//-325
                else
                    remainBulletNum_Obj.transform.localPosition = new Vector2(remainBulletNum_Obj.transform.localPosition.x, 215);//-274
            }
        }

        coinIn.Init(playerId);
    }
    void Update()
    {     // 退
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
                //	image_Cover.gameObject.SetActive(true);
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

        //弹夹400容
        // int value = 999;

        if (num_Bullet != null)
        {
            num_Bullet.UpdateShow(value2);
        }
        if (num_Bullet1 != null && value1 >= 0)
        {
            num_Bullet1.UpdateShow(400);
        }

        //num_Bullet2.UpdateShow (value);

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
        //image_RemainBulletNum.fillAmount = bl;
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
        //     image_SpecialAttackTime.fillAmount = value;
    }
    public void Update_ContinueTime(int value)
    {

        // 秒
        num_ContinueTimeout.UpdateShow(value);
    }
    //public void Update_Time(int value) {
    //    // 秒
    //    image_Time[0].sprite = sprite_NumTime[(value % 60) % 10];
    //    image_Time[1].sprite = sprite_NumTime[((value % 60) / 10) % 10];
    //    // 分
    //    image_Time[2].sprite = sprite_NumTime[(value / 60) % 10];
    //    image_Time[3].sprite = sprite_NumTime[((value / 60) / 10) % 10];
    //}
}
