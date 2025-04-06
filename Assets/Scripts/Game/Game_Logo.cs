using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
                String_Company = "Company_00";
                break;
            case 1: //海燕电子
                String_Company = "Company_01";
                if (Main.statue != en_MainStatue.Game_97)
                {
                    Image_logo.transform.localPosition = new Vector3(transform.localPosition.x, 150, transform.localPosition.z);
                    NumHeOne.transform.localPosition = new Vector3(transform.localPosition.x, -80, transform.localPosition.z);
                }
                else
                {
                    Image_logo.transform.localPosition = new Vector3(-90f, 295f, 0);
                    NumHeOne.transform.localPosition = new Vector3(140f, 390f, 0);
                }
                break;
            case 2:
                String_Company = "Company_02";
                Image_logo.transform.localPosition = new Vector3(transform.localPosition.x, 84, transform.localPosition.z);
                NumHeOne.transform.localPosition = new Vector3(transform.localPosition.x, -20, transform.localPosition.z);
                break;
            case 3:
            case 9:
                String_Company = "Company_03";
                Image_logo.transform.localPosition = new Vector3(transform.localPosition.x, 220, transform.localPosition.z);
                NumHeOne.transform.localPosition = new Vector3(transform.localPosition.x, -90, transform.localPosition.z);
                break;
            case 4:
                String_Company = "Company_04";
                //Image_logo.transform.localPosition = new Vector3(0, 84, 0);
                NumHeOne.transform.localPosition = new Vector3(0, -50, 0);
                break;
            case 5:
                String_Company = "Company_05";
                NumHeOne.transform.localPosition = new Vector3(0, -5, 0);
                break;
            case 6:
                String_Company = "Company_06";
                NumHeOne.transform.localPosition = new Vector3(0, -5, 0);
                break;
            case 7:
                String_Company = "Company_07";
                NumHeOne.transform.localPosition = new Vector3(-57, 72, 0);
                if (Main.statue == en_MainStatue.LoadScene)
                {
                    transform.localPosition = new Vector3(94, 60, 0);
                }
                //else {
                //    transform.localPosition = new Vector3(94, -138, 0);
                //}
                break;
            case 8:
                String_Company = "Company_08";
                NumHeOne.transform.localPosition = new Vector3(42, -21, 0);
                break;
            case 10:
                String_Company = "Company_10";
                break;
            case 11:
                String_Company = "Company_11";
                Image_logo.transform.localPosition = new Vector3(transform.localPosition.x, 100, transform.localPosition.z);
                NumHeOne.transform.localPosition = new Vector3(35f, 0, transform.localPosition.z);
                break;
            case 12:
                String_Company = "Company_12";
                Image_logo.transform.localPosition = new Vector3(transform.localPosition.x, 100, transform.localPosition.z);
                NumHeOne.transform.localPosition = new Vector3(50f, -20f, transform.localPosition.z);
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

        Image_logo.SetNativeSize();


    }
}
