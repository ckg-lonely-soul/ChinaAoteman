using UnityEngine;
using UnityEngine.UI;

public class Game_LoadScene : MonoBehaviour
{
    public Image image_ProgressBarValue;
    public Text text_ProgressValue;
    public Num num_ProgressValue;
    public Slider slider_ProgressBarValue;
    public Image image_TipsTile;
    public Image image_TipsText;

    public void GameStart()
    {
        Sprite[] sprite_Tips;
        string strCompany = "";
        if (Set.setVal.Language == (int)en_Language.Chinese)
        {
            sprite_Tips = Resources.LoadAll<Sprite>(strCompany + "Pic/GameLoad/CN");
        }
        else
        {
            sprite_Tips = Resources.LoadAll<Sprite>(strCompany + "Pic/GameLoad/EN");
        }
        if (image_TipsTile != null)
        {
            image_TipsTile.sprite = sprite_Tips[0];
            image_TipsTile.SetNativeSize();
        }
        if (image_TipsText != null)
        {
            image_TipsText.sprite = sprite_Tips[1];
            image_TipsText.SetNativeSize();
        }
    }

    public void Update_ProgressValue(float value)
    {
        if (image_ProgressBarValue != null)
        {
            image_ProgressBarValue.fillAmount = value;
        }
        if (text_ProgressValue != null)
        {
            text_ProgressValue.text = ((int)(value * 100) / 100).ToString("D2") + "%";
        }
        if (num_ProgressValue != null)
        {
            num_ProgressValue.UpdateShow((int)(value * 100));
        }
        if (slider_ProgressBarValue != null)
        {
            slider_ProgressBarValue.value = value;
        }
    }
}
