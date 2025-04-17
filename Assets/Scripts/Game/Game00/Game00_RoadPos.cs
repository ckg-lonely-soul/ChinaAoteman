using UnityEngine;

public class Game00_RoadPos : MonoBehaviour
{
    public GameObject targetPos;
    public GameObject lookAtPosObj;
    public Game00_FreshPos[] freshPos;
    public Game00_MonsterTempPos[] monsterHidePos;
    public int monsterTotalNum = 0;             // 怪总数：当前点最多出怪个数
    public int minMonsterNum = 0;               // 怪最小数量：当前点怪个数小于该点时，切换到下一个点
    public float delayTime = 0;                 // 延时到下一点的时间
    public float stopTime = 0;                  // 当前点停留时间      移到下一个点
    public float stopTimeOut = 0;
    public float moveSpeed = 3.5f;              // 上一点到当前点的移动速度
    public float rotateSpeed = 180f;            // 上一点到当前点的相机旋转速度
    public bool killAllMonster = false;         // 需要击杀全部怪，才跳下一个点
    public bool nextPosImmediately = false;     // 到达当前点后立刻下一点

    public void Init(Game00_Main gameMain)
    {
        freshPos = GetComponentsInChildren<Game00_FreshPos>();
        for (int i = 0; i < freshPos.Length; i++)
        {
            freshPos[i].Init(gameMain, i);
        }

        monsterHidePos = GetComponentsInChildren<Game00_MonsterTempPos>();
    }
}
