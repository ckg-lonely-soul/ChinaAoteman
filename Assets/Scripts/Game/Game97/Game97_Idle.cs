using System.Collections;
using System.Collections.Generic;
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
    public Image image_CompanyLogo;
    public Game_VideoPlayer gameVideoPlayer;

    Game97_Main game97_Main;
    en_IdleSta statue;
    float runTime;
    int language = -1;
    string[] tab_StrLanguage = { "cn", "en" };
    public void Awake0(Game97_Main game)
    {
        //print("111");
        game97_Main = game;
        //
        if (Main.COMPANY_NUM == 8)
        {
            GameObject prefab = Resources.Load<GameObject>("Company_" + Main.COMPANY_NUM.ToString("D2") + "/Loading/Image_Tips");
            GameObject obj = Instantiate(prefab, transform);
            obj.transform.localPosition = new Vector3(0, -270);
            obj.transform.localScale = Vector3.one * 0.5f;
        }
    }

    public void GameStart()
    {
        IO.Init();
        if(language != Set.setVal.Language)
        {
            language = Set.setVal.Language;
        }
        image_CompanyLogo.gameObject.SetActive(false);
        if (Main.COMPANY_NUM == 7)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Company_" + Main.COMPANY_NUM.ToString("D2") + "/Pic/Game97/Idle/BackG");
            image_Logo.transform.localPosition = new Vector3(94, -138, 0);
        }
        else if (Main.COMPANY_NUM == 6)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Company_" + Main.COMPANY_NUM.ToString("D2") + "/Pic/Game97/Idle/BackG");
            image_Logo.transform.localPosition = new Vector3(156, 66, 0);
        }
        else if (Main.COMPANY_NUM == 4)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Company_" + Main.COMPANY_NUM.ToString("D2") + "/Pic/Game97/Idle/BackG");
            image_Logo.transform.localPosition = new Vector3(0, 100, 0);
        }
        else if (Main.COMPANY_NUM == 8)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Company_" + Main.COMPANY_NUM.ToString("D2") + "/Pic/Game97/Idle/BackG");
            image_Logo.transform.localPosition = new Vector3(0, 100, 0);
        }
        else if (Main.COMPANY_NUM == 11)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Company_" + Main.COMPANY_NUM.ToString("D2") + "/Pic/Game97/Idle/BackG");
            image_Logo.transform.localPosition = new Vector3(0, 100, 0);
        }
        else if (Main.COMPANY_NUM == 12)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Company_" + Main.COMPANY_NUM.ToString("D2") + "/Pic/Game97/Idle/BackG");
            image_Logo.transform.localPosition = new Vector3(0, 100, 0);
            image_CompanyLogo.sprite = Resources.Load<Sprite>("Company_" + Main.COMPANY_NUM.ToString("D2") + "/Pic/Loading/Logo/Logo_" + tab_StrLanguage[language]);
            image_CompanyLogo.gameObject.SetActive(true);
        }
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
            image_Logo.transform.localPosition = new Vector3(0, 220 - 30f * Mathf.Sin(runTime * 3f));
        }
        else if (Main.COMPANY_NUM == 11)
        {
            image_Logo.transform.localPosition = new Vector3(0, 150 - 30f * Mathf.Sin(runTime * 3f));
        }
        else if (Main.COMPANY_NUM == 12)
        {
            image_Logo.transform.localPosition = new Vector3(0, 150 - 30f * Mathf.Sin(runTime * 3f));
        }
    }

    void ChangeStatue(en_IdleSta sta)
    {
        statue = sta;
        runTime = 0;

        gameVideoPlayer.gameObject.SetActive(false);
        switch (statue)
        {
            case en_IdleSta.ShowVideo:
                //gameVideoPlayer.gameObject.SetActive(true);
                // image_Logo.transform.localPosition = new Vector3(-353, 125, 0);
                // image_Logo.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case en_IdleSta.ShowLogo:
                //image_Logo.transform.localPosition = new Vector3(0, 52, 0);
                // image_Logo.transform.localScale = new Vector3(1, 1, 1);
                break;
        }
    }
}
