using UnityEngine;
using UnityEngine.UI;

enum en_IdleSta
{
    ShowVideo = 0,
    ShowLogo,
}

public class Game97_Idle : MonoBehaviour
{
    public Image image_Logo;
    public float image_Logo_Y = 220;
    public Image image_CompanyLogo;
    public Game_VideoPlayer gameVideoPlayer;

    Game97_Main game97_Main;
    en_IdleSta statue;
    float runTime;
    int language = -1;
    string[] tab_StrLanguage = { "cn", "en" };
    public void Awake0(Game97_Main game)
    {
        game97_Main = game; 
    }

    public void GameStart()
    {
        IO.Init();
        if (language != Set.setVal.Language)
        {
            language = Set.setVal.Language;
        }
        image_CompanyLogo.gameObject.SetActive(false);
        
        ChangeStatue(en_IdleSta.ShowVideo);
    }

    void Update()
    {
        switch (statue)
        {
            case en_IdleSta.ShowVideo:
                runTime += Time.deltaTime;
                if (runTime >= 180)
                {
                    ChangeStatue(en_IdleSta.ShowLogo);
                }
                break;
            case en_IdleSta.ShowLogo:
                runTime += Time.deltaTime;
                if (runTime >= 30)
                {
                    ChangeStatue(en_IdleSta.ShowVideo);
                }
                break;
        }
        if (Main.COMPANY_NUM == 3)
        {
            image_Logo.transform.localPosition = new Vector3(0, image_Logo_Y - 30f * Mathf.Sin(runTime * 3f));
        }
    }

    void ChangeStatue(en_IdleSta sta)
    {
        statue = sta;
        runTime = 0;

        gameVideoPlayer.gameObject.SetActive(false);
        switch (statue)
        {
            //展示待机页面视频
            case en_IdleSta.ShowVideo:
                break;
            case en_IdleSta.ShowLogo:
                break;
        }
    }
}
