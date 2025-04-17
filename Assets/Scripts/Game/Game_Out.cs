using UnityEngine;
using UnityEngine.UI;

public enum en_OutSta
{
    Idle = 0,
    Outing,
    Error,
}

public class Game_Out : MonoBehaviour
{
    public Image image_OutText;
    public Num num_Out;

    Sprite[] sprite_UI;

    public en_OutSta statue;
    float runTime;
    int Id = 0;
    int currOutNum;

    public void Init(int no)
    {
        if (sprite_UI == null)
        {
            sprite_UI = Resources.LoadAll<Sprite>("Company_00/Common/Out_Text");
        }
        Id = no;
        currOutNum = FjData.g_Fj[Id].Wins;
        num_Out.UpdateShow(currOutNum);
        ChangeStatue(en_OutSta.Idle);
    }

    void Update()
    {
        if (Main.statue < en_MainStatue.Game_97)
        {
            return;
        }
        if (currOutNum != FjData.g_Fj[Id].Wins)
        {
            currOutNum = FjData.g_Fj[Id].Wins;
            num_Out.UpdateShow(currOutNum);
        }

        switch (statue)
        {
            case en_OutSta.Idle:
                if (PAction.IsOuting(Id))
                {
                    ChangeStatue(en_OutSta.Outing);
                }
                if (Main.errorStatue[Id] != 0)
                {
                    ChangeStatue(en_OutSta.Error);
                }
                break;
            case en_OutSta.Outing:
                if (PAction.IsOuting(Id) == false)
                {
                    ChangeStatue(en_OutSta.Idle);
                }
                break;
            case en_OutSta.Error:
                runTime += Time.deltaTime;
                if (runTime >= 0.5f)
                {
                    runTime = 0;
                    if (image_OutText.gameObject.activeSelf)
                    {
                        image_OutText.gameObject.SetActive(false);
                        num_Out.gameObject.SetActive(false);
                    }
                    else
                    {
                        image_OutText.gameObject.SetActive(true);
                        num_Out.gameObject.SetActive(true);
                    }
                }
                if (Main.errorStatue[Id] == 0)
                {
                    ChangeStatue(en_OutSta.Idle);
                }
                break;
        }
    }

    void ChangeStatue(en_OutSta sta)
    {
        statue = sta;
        runTime = 0;
        image_OutText.gameObject.SetActive(false);
        num_Out.gameObject.SetActive(false);

        switch (statue)
        {
            case en_OutSta.Idle:
                break;
            case en_OutSta.Outing:
                image_OutText.gameObject.SetActive(true);
                num_Out.gameObject.SetActive(true);
                //
                switch ((en_OutMode)Set.setVal.OutMode)
                {
                    case en_OutMode.OutGift:
                        image_OutText.sprite = sprite_UI[2];
                        break;
                    default:
                        image_OutText.sprite = sprite_UI[0];
                        break;
                }
                Main.FormatImageSizeFollowSprite(image_OutText);
                break;
            case en_OutSta.Error:
                image_OutText.gameObject.SetActive(true);
                num_Out.gameObject.SetActive(true);
                //
                switch ((en_OutMode)Set.setVal.OutMode)
                {
                    case en_OutMode.OutGift:
                        image_OutText.sprite = sprite_UI[6];
                        break;
                    default:
                        image_OutText.sprite = sprite_UI[4];
                        break;
                }
                Main.FormatImageSizeFollowSprite(image_OutText);
                break;
        }
    }
}
