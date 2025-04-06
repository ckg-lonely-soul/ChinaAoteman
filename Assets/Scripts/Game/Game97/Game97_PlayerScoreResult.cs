using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game97_PlayerScoreResult : MonoBehaviour
{
    public Image image_Title;
    public Image image_ScoreText;       // 得分
    public Image image_WinsText;       // 得分
    public Image image_KillNum;         // 击败敌人数
    public Image image_PassTime;        // 
    public Image image_PlayLevel;
    public Num num_Score;
    public Num num_Wins;
    public Num num_KillNum;
    public Num num_PassTime_Min;
    public Num num_PassTime_Sec;
    public Image[] image_LevelStar;
}
