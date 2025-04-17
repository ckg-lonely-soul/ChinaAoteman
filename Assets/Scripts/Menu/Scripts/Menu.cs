using UnityEngine;
using UnityEngine.UI;

public enum en_MenuStatue
{
    MenuSta_SysSet = 0,
    MenuSta_GameSet,
    MenuSta_WinSet,
    MenuSta_GameSelect,
    MenuSta_Acc,
    MenuSta_AdcVerify,
    MenuSta_UpdateVideo,
    MenuSta_IoCheck,
    MenuSta_Dna,
    MenuSta_Enc,
    MenuSta_Language
};

public class Menu : MonoBehaviour
{
    public const bool SCREEN_H = true;      // 横屏

    // 坐标,大小定义 X: 从左到右, Y: 从下到上
    public const float DEFAULT_SCREEN_WIDTH = 1280;
    public const float DEFAULT_SCREEN_HEIGHT = 720;

    public const float SCREEN_WIDTH = 1280; // Main.DEFAULT_SCREEN_WIDTH;
    public const float SCREEN_HEIGHT = 720; // Main.DEFAULT_SCREEN_HEIGHT;

    public static float MENU_CODE_START_Y = 630;                //C1开始坐标 Y
    public static float MENU_CODE_DISTANCE_Y = 90;          //C1开始坐标 Y
    public static float MENU_CODE_BUTTON_Y = 60;                //C1按键坐标 Y
    public static float MENU_CODE_BUTTON_DISTANCE_X = 330;  //C1按键 X 间距
    public static float MENU_CURSOR_CODE_START_Y = 630;     //C1开始坐标 Y
    public static float MENU_CURSOR_CODE_DISTANCE_Y = 90;   //C1开始坐标 Y

    // 页面:
    Main gmain;
    public Text text_VersionText;
    public Menu_Tips menuTips;
    public Menu_PasswordManager passwordManager;
    public Menu_SysSet menu_SysSet;
    public Menu_GameSet menu_GameSet;
    public Menu_GameSelect menu_GameSelect;
    public Menu_AdcVerify menu_AdcVerify;
    public Menu_Acc menu_Acc;
    public Menu_IoCheck menu_IoCheck;
    public Menu_Dna menu_Dna;
    public Menu_Enc menu_Enc;
    public Menu_Language menu_Language;


    static GameObject planeOld;
    static GameObject planeCurr;

    public static en_MenuStatue statue;

    float planePosX;
    //    
    public void Awake0(Main main)
    {
        gmain = main;
        //初始化接口
        planeOld = null;
        planeCurr = null;

        passwordManager.Init(this);
        menu_SysSet.Awake0(this);
        menu_GameSet.Awake0(this);
        menu_GameSelect.Awake0(this);
        menu_AdcVerify.Awake0(this);
        menu_Acc.Awake0(this);
        menu_IoCheck.Awake0(this);
        menu_Language.Awake0(this);

        // 大小调整
        //menu_SysSet.transform.parent.localScale = new Vector3 ((float)Screen.width / DEFAULT_SCREEN_WIDTH, (float)Screen.height / DEFAULT_SCREEN_HEIGHT, 1);
        // 版本显示
        text_VersionText.text = "Version: " + Main.VERSION;
    }

    public void Enter()
    {
        transform.localPosition = new Vector3(0, 0, 0);
        ChangeStatue(en_MenuStatue.MenuSta_SysSet);
    }

    void Update()
    {
        //界面移动
        if (planePosX > 0)
        {
            planePosX -= Time.deltaTime * 6605;
            if (planePosX < 0)
                planePosX = 0;
            ShowPlane();
        }
        else if (planePosX < 0)
        {
            planePosX += Time.deltaTime * 6605;
            if (planePosX > 0)
                planePosX = 0;
            ShowPlane();
        }
    }

    public void ChangeStatue(en_MenuStatue sta)
    {
        statue = sta;
        Key.Clear();

        passwordManager.gameObject.SetActive(false);
        menuTips.gameObject.SetActive(false);
        menu_SysSet.gameObject.SetActive(false);
        menu_GameSet.gameObject.SetActive(false);
        menu_GameSelect.gameObject.SetActive(false);
        menu_AdcVerify.gameObject.SetActive(false);
        menu_Acc.gameObject.SetActive(false);
        menu_IoCheck.gameObject.SetActive(false);
        menu_Dna.gameObject.SetActive(false);
        menu_Enc.gameObject.SetActive(false);
        menu_Language.gameObject.SetActive(false);

        menuTips.waitTips = false;
        passwordManager.waitCheck = false;

        planeOld = planeCurr;
#if SHOOT_BEAD || SHOOT_WATER
        GameMain.GunMotorStatue_Clear();
#endif
        switch (statue)
        {
            case en_MenuStatue.MenuSta_SysSet:
                menu_SysSet.gameObject.SetActive(true);
                menu_SysSet.GameStart();
                planeCurr = menu_SysSet.gameObject;
                planePosX = -SCREEN_WIDTH;
                break;
            case en_MenuStatue.MenuSta_GameSet:
                menu_GameSet.gameObject.SetActive(true);
                menu_GameSet.GameStart();
                planeCurr = menu_GameSet.gameObject;
                planePosX = SCREEN_WIDTH;
                break;
            case en_MenuStatue.MenuSta_GameSelect:
                menu_GameSelect.gameObject.SetActive(true);
                menu_GameSelect.GameStart();
                planeCurr = menu_GameSelect.gameObject;
                planePosX = SCREEN_WIDTH;
                break;
            case en_MenuStatue.MenuSta_AdcVerify:
                menu_AdcVerify.gameObject.SetActive(true);
                menu_AdcVerify.GameStart();
                planeCurr = menu_AdcVerify.gameObject;
                planePosX = SCREEN_WIDTH;
                break;
            case en_MenuStatue.MenuSta_Acc:
                menu_Acc.gameObject.SetActive(true);
                menu_Acc.GameStart();
                planeCurr = menu_Acc.gameObject;
                planePosX = SCREEN_WIDTH;
                break;
            case en_MenuStatue.MenuSta_IoCheck:
                menu_IoCheck.gameObject.SetActive(true);
                menu_IoCheck.GameStart();
                planeCurr = menu_IoCheck.gameObject;
                planePosX = SCREEN_WIDTH;
                break;
            case en_MenuStatue.MenuSta_Enc:
                menu_Enc.gameObject.SetActive(true);
                menu_Enc.GameStart();
                planeCurr = menu_Enc.gameObject;
                planePosX = SCREEN_WIDTH;
                break;
            case en_MenuStatue.MenuSta_Dna:
                menu_Dna.gameObject.SetActive(true);
                menu_Dna.GameStart();
                planeCurr = menu_Dna.gameObject;
                planePosX = SCREEN_WIDTH;
                break;
            case en_MenuStatue.MenuSta_Language:
                menu_Language.gameObject.SetActive(true);
                menu_Language.GameStart();
                planeCurr = menu_Language.gameObject;
                planePosX = SCREEN_WIDTH;
                break;
        }
        ShowPlane();
    }

    void ShowPlane()
    {
        if (planeOld != null)
        {
            planeOld.transform.localPosition = new Vector3(planePosX - SCREEN_WIDTH, 0, 0);
        }
        if (planeCurr != null)
        {
            planeCurr.transform.localPosition = new Vector3(planePosX, 0, 0);
        }
    }
    public void Quit()
    {
        gmain.ChangeStatue_To_GameIdle();
    }
}
