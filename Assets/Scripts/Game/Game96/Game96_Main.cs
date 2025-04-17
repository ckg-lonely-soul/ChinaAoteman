using UnityEngine;
using UnityEngine.UI;

public class Game96_Main : MonoBehaviour
{

    private Main main;
    public Game_MediaPlayer gameMediaPlayer;
    public ChangeSprite changSprite;
    public Text minTime_Text;
    public Text secTime_Text;

    private float gameTime;
    public Game00_CoinIn[] coinIn;
    public float fireCD;

    //public Sprite[] coinsSprites;
    private float nextDcTime;

    private int language = -1;
    private string[] tab_Language = { "CN", "EN" };


    public void Awake0(Main main)
    {
        this.main = main;
    }

    public void GameStart()
    {
#if IO_LOCAL || IO_ZF
        IO.Motor_Out(true);
#endif
#if IO_LOCAL
        IO.MotorOld_Out(true);
#endif
        if (Main.DecStartCoin(0))
        {
            FjData.g_Fj[0].Playing = true;
            FjData.g_Fj[0].Played = true;
        }
        gameTime = Set.setVal.GameTime;
        fireCD = 0;
        if (language != Set.setVal.Language)
        {
            language = Set.setVal.Language;
            for (int i = 0; i < coinIn.Length; i++)
            {
                //coinIn[i].sprite = coinsSprites[language];
                //coinIn[i].SetNativeSize();
                coinIn[i].Init(i);
            }
        }
        SetCoinInPos();
    }

    void Update()
    {

        if (nextDcTime > 0)
        {
            nextDcTime -= Time.deltaTime;
        }
        if (fireCD > 0)
        {
            fireCD -= Time.deltaTime;
        }
        Fire();
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
#else
        if (Key.KEYFJ_AddPressed(0)) 
        {
#endif
            if (nextDcTime <= 0)
            {
                //nextDcTime = 1.5f;
                nextDcTime = 5.5f;
                gameMediaPlayer.PlayNextOne();
            }
        }


        gameTime -= Time.deltaTime;
        UpdateTimeText();
        if (gameTime <= 0 && main != null)
        {
            FjData.g_Fj[0].Playing = false;
            FjData.g_Fj[0].Played = false;
            main.ChangeScene(en_MainStatue.Game_97);
        }
    }

    private void UpdateTimeText()
    {
        //changSprite.enabled = gameTime > 10 ? false : true;
        int min = (int)gameTime / 60;
        int second = (int)gameTime % 60;

        minTime_Text.text = min.ToString("D2");
        secTime_Text.text = second.ToString("D2");
    }
    public void SetCoinInPos()//根据玩家数设置coinin位置
    {
        if (Set.setVal.GameMode == (int)en_PlayerMode.One)
        {
            coinIn[1].gameObject.SetActive(false);
            coinIn[0].transform.localPosition = new Vector3(0, -330f, 0);
        }
        else
        {
            for (int i = 0; i < coinIn.Length; i++)
            {
                coinIn[i].gameObject.SetActive(true);
            }
            coinIn[0].transform.localPosition = new Vector3(-320f, -330f, 0);
            coinIn[1].transform.localPosition = new Vector3(320f, -330f, 0);
        }
    }

    public void Fire()
    {
        if (Key.KEYFJ_Statue_Ok(0))
        {
            if (fireCD <= 0)
            {
                fireCD = 0.2f;
                if (Set.setVal.MP5GunShake == 1)
                {
#if SHAKE_POWER
                    IO.GunRunStart(0, (byte)Set.setVal.Gun1ShakePower, (byte)(Set.setVal.Gun1ShakeTime - Set.setVal.Gun1ShakePower));
#else
                    IO.GunRunStart(0, 20, 30);
#endif
                }
            }
        }
    }
}
