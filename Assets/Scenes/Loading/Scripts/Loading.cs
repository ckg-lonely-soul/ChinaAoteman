using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum en_LoadingStatue
{
    Logo_Company = 0,   // 公司LOGO
    Logo_Face,          // 笑脸
    Logo_Agent,         // 代理商LOGO
    LoadData,           // 加载数据
    LoadScene,          // 加载场景
}

public class Loading : MonoBehaviour
{
    public Image image_PlaneBackground;
    public Image image_Logo;
    public Image image_progressValue;

    Sprite[] sprite_Logo;

    //异步对象
    AsyncOperation async = null;
    Game_Logo gameLogo;

    Image[] image_numErrorCode;

    en_LoadingStatue statue;
    float time;
    float runTime;
    float errorTime;

    void Awake()
    {
        Set.LoadAll();
        // 隐藏鼠标光标
        if (Main.FREE_SIZE == false)
        {
            Screen.SetResolution((int)Main.TARGET_SCREEN_WIDTH, (int)Main.TARGET_SCREEN_HEIGHT, true);
        }
        if (language != Set.setVal.Language)
        {
            language = Set.setVal.Language;
        }
        
    }

    void Start()
    {
        ChangeStatue(en_LoadingStatue.LoadData);
    }

    IEnumerator LoadScene()
    {
        string sceneName;
        yield return new WaitForEndOfFrame();

        sceneName = "Game_97";
        async = SceneManager.LoadSceneAsync(sceneName);

        //读取完备后返回
        yield return async;
    }

    void Update()
    {
        switch (statue)
        {
            case en_LoadingStatue.Logo_Company:     // 显示 JM logo
                if (Time.time - time >= 1)
                {
                    ChangeStatue(en_LoadingStatue.LoadData);
                }
                break;
            case en_LoadingStatue.Logo_Face:       // 显示 笑脸 logo
                if (Time.time - time >= 1)
                {
                    ChangeStatue(en_LoadingStatue.Logo_Agent);
                }
                break;
            case en_LoadingStatue.Logo_Agent:       // 显示 图瑞 logo
                if (Time.time - time >= 1)
                {
#if UNITY_EDITOR
                    ChangeStatue(en_LoadingStatue.LoadScene);
#else
                    ChangeStatue(en_LoadingStatue.LoadData);
#endif
                }
                break;

            case en_LoadingStatue.LoadData:
                if (Main.Load_Run())
                {
                    ChangeStatue(en_LoadingStatue.LoadScene);
                }
                break;

            case en_LoadingStatue.LoadScene:        // 显示加载进度
                if (async != null)
                {
                    //在这里计算读取的进度，  
                    //progress 的取值范围在0.1 - 1之间， 但是它不会等于1  
                    //也就是说progress可能是0.9的时候就直接进入新场景了  
                    //所以在写进度条的时候需要注意一下。  
                    //为了计算百分比 所以直接乘以100即可  

                    //有了读取进度的数值，大家可以自行制作进度条啦。  
                    image_progressValue.fillAmount = async.progress;
                }
                break;
        }
    }

    int language = -1;
    string[] tab_StrLanguage = { "cn", "en" };

    void ChangeStatue(en_LoadingStatue sta)
    {
        statue = sta;
        time = Time.time;
        runTime = 0;
        switch (statue)
        {
            case en_LoadingStatue.Logo_Company:     // 显示 JM logo

                image_Logo.sprite = Resources.Load<Sprite>("Pic/Logo/logo_0_LaoWanTong");
                image_Logo.SetNativeSize();
                break;
            case en_LoadingStatue.Logo_Face:       // 显示 笑脸 logo
                image_Logo.sprite = Resources.Load<Sprite>("Pic/Logo/logo_1_Face");
                break;
            case en_LoadingStatue.Logo_Agent:       // 显示 图瑞 logo
                image_Logo.sprite = Resources.Load<Sprite>("Pic/Logo/logo_1_JY");
                break;

            case en_LoadingStatue.LoadData:
                Main.Load_Start();
                break;

            case en_LoadingStatue.LoadScene:        // 显示加载进度
                image_progressValue.fillAmount = 0;
                //进入loadScene方法
                StartCoroutine(LoadScene());
                break;
        }
    }
}
