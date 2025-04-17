using UnityEngine;
using UnityEngine.UI;

public enum en_GameOverSta
{
    Run = 0,
}

public class Game97_GameOver : MonoBehaviour
{
    public Image image_TitleBackG;
    public Image image_TitleText;
    public Game97_PlayerScoreResult[] playerScoreResult;
    // 声音
    public AudioSource audioSource_BackG;       // 背景音乐

    Game97_Main game97_Main;

    Sprite[] sprite_PlayerScoreResult;
    Sprite[] sprite_Title;
    public Sprite allpass, gameOver;
    en_GameOverSta statue;
    public float runTime;

    int[] nowScore = new int[Main.MAX_PLAYER];
    int[] nowWins = new int[Main.MAX_PLAYER];

    public int playerNum;

    public void Awake0(Game97_Main game)
    {
        game97_Main = game;
        playerNum = 0;
        sprite_Title = Resources.LoadAll<Sprite>("Company_00/Game97/GameOver/Title");
        sprite_PlayerScoreResult = Resources.LoadAll<Sprite>("Company_00/Game97/GameOver/Plsyer");
    }

    public void GameStart()
    {
        if (Set.setVal.Language == (int)en_Language.Chinese)
        {
            allpass = Resources.Load<Sprite>("Company_00/Game97/GameOver/cn/win_cn");
            gameOver = Resources.Load<Sprite>("Company_00/Game97/GameOver/cn/lose_cn");
        }
        else
        {
            allpass = Resources.Load<Sprite>("Company_00/Game97/GameOver/en/win_en");
            gameOver = Resources.Load<Sprite>("Company_00/Game97/GameOver/en/lose_en");
        }

        if (game97_Main.IsAllGamePass())
        {
            image_TitleText.sprite = allpass; 
        }
        else
        {
            image_TitleText.sprite = gameOver;
        }

        playerNum = 0;
        for (int i = 0; i < Main.MAX_PLAYER; i++)
        {
            if (FjData.g_Fj[i].Played)
            {
                playerNum++;
            }
        }

        if (playerNum == 1 || Set.setVal.PlayerMode == (int)en_PlayerMode.One)
        {
            playerScoreResult[0].gameObject.SetActive(true);
            playerScoreResult[1].gameObject.SetActive(false);
            playerScoreResult[2].gameObject.SetActive(false);
        }
        else if (playerNum == 2)
        {
            playerScoreResult[0].gameObject.SetActive(false);
            playerScoreResult[1].gameObject.SetActive(true);
            playerScoreResult[2].gameObject.SetActive(true);
        }

        for (int i = 0; i < Main.MAX_PLAYER; i++)
        {
            nowScore[i] = FjData.g_Fj[i].JsScores;

            if (playerNum == 1 || Set.setVal.PlayerMode == (int)en_PlayerMode.One)
            {
                playerScoreResult[i].image_ScoreText.sprite = sprite_PlayerScoreResult[2 * i + Set.setVal.Language];
                //结算分数
                if (Set.setVal.OutMode == (int)en_OutMode.OutGift)
                {
                    playerScoreResult[i].image_WinsText.sprite = sprite_PlayerScoreResult[4 + 2 * i + Set.setVal.Language];
                }
                else
                {
                    playerScoreResult[i].image_WinsText.sprite = sprite_PlayerScoreResult[8 + 2 * i + Set.setVal.Language];
                }
                Main.FormatImageSizeFollowSprite(playerScoreResult[i].image_Title);
                Main.FormatImageSizeFollowSprite(playerScoreResult[i].image_ScoreText);
                Main.FormatImageSizeFollowSprite(playerScoreResult[i].image_WinsText);             
            }
            else if (playerNum == 2)
            {
                playerScoreResult[i + 1].image_ScoreText.sprite = sprite_PlayerScoreResult[2 * i + Set.setVal.Language];
                if (Set.setVal.OutMode == (int)en_OutMode.OutGift)
                {
                    playerScoreResult[i + 1].image_WinsText.sprite = sprite_PlayerScoreResult[4 + 2 * i + Set.setVal.Language];
                }
                else
                {
                    playerScoreResult[i + 1].image_WinsText.sprite = sprite_PlayerScoreResult[8 + 2 * i + Set.setVal.Language];
                }
                Main.FormatImageSizeFollowSprite(playerScoreResult[i + 1].image_Title);
                Main.FormatImageSizeFollowSprite(playerScoreResult[i + 1].image_ScoreText);
                Main.FormatImageSizeFollowSprite(playerScoreResult[i + 1].image_WinsText);              
            }
            Main.JieSuanScore(i);
        }

        for (int i = 0; i < playerScoreResult.Length; i++)
        {
            if (playerScoreResult[i].gameObject.activeSelf)
            {
                if (playerNum == 1 || Set.setVal.PlayerMode == (int)en_PlayerMode.One)
                {
                    //playerScoreResult[i].num_Score.UpdateShow(nowScore[i]);
                    for (int j = 0; j < nowScore.Length; j++)
                    {
                        if (nowScore[j] > 0)
                        {
                            playerScoreResult[i].num_Score.UpdateShow(nowScore[j]);
                        }
                    }
                }
                else if (playerNum == 2)
                {
                    playerScoreResult[i].num_Score.UpdateShow(nowScore[i - 1]);
                }
            }
        }

        ChangeStatue(en_GameOverSta.Run);
    }

    void Update()
    {
        runTime += Time.deltaTime;
        if (runTime >= 8)
        {
            game97_Main.GameStart();
        }
    }

    void ChangeStatue(en_GameOverSta sta)
    {
        statue = sta;
        runTime = 0;

        Key.Clear();
    }
}
