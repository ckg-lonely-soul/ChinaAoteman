﻿using UnityEngine;
using UnityEngine.UI;

public class Game_Logo : MonoBehaviour
{
    public Image image_He;
    public Num num_GameNum;
    public GameObject NumHeOne;

    //logo
    public Image Image_logo;
    [HideInInspector]
    public string String_Company;//根据公司类别区分路径

    void OnEnable()
    {
        //切换数字
        int gameNum = Main.tab_GameId.Length;

        if (Set.setVal.Language == (int)en_Language.Chinese)
        {
            image_He.sprite = Instantiate(Resources.Load<Sprite>("Pic/Game97/Idle/SeveralCombineOne_cn/91")) as Sprite;
            num_GameNum.spritePath = "Pic/Game97/Idle/SeveralCombineOne_cn";
        }
        else
        {
            image_He.sprite = Instantiate(Resources.Load<Sprite>("Pic/Game97/Idle/SeveralCombineOne_en/91")) as Sprite;
            num_GameNum.spritePath = "Pic/Game97/Idle/SeveralCombineOne_en";
        }

        num_GameNum.UpdateShow(gameNum);
        if (gameNum >= 10)
        {
            NumHeOne.transform.localPosition = new Vector3(20f, 0, 0);
        }
        else
        {
            NumHeOne.transform.localPosition = new Vector3(0, 0, 0);
        }

        //切换logo
        switch (Main.COMPANY_NUM)
        {
            case 0: //公版
                break;
            case 1: 
                break;
            case 2:
                break;
            case 3:
                String_Company = "Company_03";
                //Image_logo.transform.localPosition = new Vector3(transform.localPosition.x, 220, transform.localPosition.z);
                NumHeOne.transform.localPosition = new Vector3(transform.localPosition.x, -90, transform.localPosition.z);
                break;
            case 4: 
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            case 11:
                break;
            case 12:
                break;
            default:
                String_Company = "Company_00";
                break;
        }
        if (Set.setVal.Language == (int)en_Language.Chinese)
        {
            //logo
            Image_logo.sprite = Resources.Load<Sprite>(String_Company + "/Pic/Game97/Idle/BackG_cn/0000");
        }
        else
        {
            //logo
            Image_logo.sprite = Resources.Load<Sprite>(String_Company + "/Pic/Game97/Idle/BackG_en/0000");
        }

        //Image_logo.SetNativeSize();


    }
}
