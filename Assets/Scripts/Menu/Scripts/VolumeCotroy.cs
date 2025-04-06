using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeCotroy : MonoBehaviour
{
    const string SET_SysVolume = "SETSysVolume";
    public const int SET_c_MinSysVolume = 0;
    public const int SET_c_MaxSysVolume = 100;

    public Image image_Value;
    public Text text_Value;

    float runTime;
    float leftTime;
    float rightTime;

    public static int sysVolume;       // 系统音量(直接左右设置按键调整)

    public static void Init()
    {
        // 读
        int l1 = PlayerPrefs.GetInt(SET_SysVolume, 50);
        if (l1 < SET_c_MinSysVolume || l1 > SET_c_MaxSysVolume)
        {
            l1 = 50;
            PlayerPrefs.SetInt(SET_SysVolume, l1);
        }
        sysVolume = l1;
        //
#if VER_YBJ
        AudioListener.volume = (float)sysVolume / SET_c_MaxSysVolume * 0.15f;
#else
        AudioListener.volume = (float)sysVolume / SET_c_MaxSysVolume;
#endif
    }

    void SaveSysVolume()
    {
        PlayerPrefs.SetInt(SET_SysVolume, sysVolume);
        PlayerPrefs.Save();
    }
    void UpdateVolume()
    {
        float volume = (float)sysVolume / SET_c_MaxSysVolume;
        Debug.Log(volume + "volume");
        image_Value.fillAmount = volume;
        text_Value.text = sysVolume.ToString();
#if VER_YBJ
        AudioListener.volume = volume * 0.15f;
#else
        AudioListener.volume = volume;
#endif
    }

    void Start()
    {
        //Debug.Log("VolumeCotroy_Start");
        runTime = 0;
        leftTime = 0;
        rightTime = 0;
        UpdateVolume();
    }

    void Update()
    {
        runTime += Time.deltaTime;
        if (runTime >= 3 || Main.statue == en_MainStatue.Game_98)
        {
            Destroy(gameObject);
            return;
        }
        //左右调整音量
        if (Key.MENU_Statue_Left())
        {
            if (leftTime == 0 || leftTime >= 0.5f)
            {
                if (sysVolume > SET_c_MinSysVolume)
                {
                    sysVolume--;
                    SaveSysVolume();
                    UpdateVolume();
                }
                if (leftTime >= 0.5f)
                {
                    leftTime = 0.49f;
                }
            }
            leftTime += Time.deltaTime;
            runTime = 0;
        }
        else
        {
            leftTime = 0;
        }
        if (Key.MENU_Statue_Right())
        {
            if (rightTime == 0 || rightTime >= 0.5f)
            {
                if (sysVolume < SET_c_MaxSysVolume)
                {
                    sysVolume++;
                    SaveSysVolume();
                    UpdateVolume();
                }
                if (rightTime >= 0.5f)
                {
                    rightTime = 0.49f;
                }
            }
            rightTime += Time.deltaTime;
            runTime = 0;
        }
        else
        {
            rightTime = 0;
        }
    }


}
