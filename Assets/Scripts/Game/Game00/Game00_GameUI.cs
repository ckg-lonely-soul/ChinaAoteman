using UnityEngine;
using UnityEngine.UI;

public class Game00_GameUI : MonoBehaviour
{
    public Game00_PlayerUI[] playerUI;
    public GameObject boss_HP_Obj;
    public Image image_BossHpValue;
    public Image image_Damage;
    public Image image_Demo;
    public Image image_BossWarning;
    public Image image_BossWarning2;
    public Image image_BossWarning3;
    public Image image_Pass;
    public Image image_Pass2;
    public Num num_TotalMonsterNum;
    public Num num_RemainMonsterNum;
    Sprite[] sprite_BossWarning;

    // Use this for initialization
    public void GameStart()
    {
        string strCompany;

        strCompany = "Company_00/Common/";

        if (Set.setVal.Language == (int)en_Language.Chinese)
        {
            sprite_BossWarning = Resources.LoadAll<Sprite>("Company_00/Common/BossWarning_cn");
            image_Pass2.sprite = Resources.Load<Sprite>(strCompany + "GamePass/0000");
            image_BossWarning3.sprite = Resources.Load<Sprite>("Company_00/Common/BossWarning_cn/0000");
        }
        else
        {
            sprite_BossWarning = Resources.LoadAll<Sprite>("Company_00/Common/BossWarning_en");
            image_Pass2.sprite = Resources.Load<Sprite>(strCompany + "GamePass/0001");
            image_BossWarning3.sprite = Resources.Load<Sprite>("Company_00/Common/BossWarning_en/0000");
        }

        image_BossWarning.sprite = Instantiate(sprite_BossWarning[0]) as Sprite;
        image_BossWarning.SetNativeSize();
        image_BossWarning.sprite = Instantiate(sprite_BossWarning[1]) as Sprite;
        image_BossWarning.SetNativeSize();
        image_Damage.gameObject.SetActive(false);
        image_Demo.gameObject.SetActive(false);
        image_BossWarning.gameObject.SetActive(false);
        image_Pass.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Update_BossHpValue(float value)
    {
        if (value < 0)
            value = 0;
        else if (value > 1)
            value = 1;
        image_BossHpValue.fillAmount = value;
    }
    public void Update_TotalMonsterNum(int value)
    {
        num_TotalMonsterNum.UpdateShow(value);
    }
    public void Update_RemainMonsterNum(int value)
    {
        num_RemainMonsterNum.UpdateShow(value);
    }
}
