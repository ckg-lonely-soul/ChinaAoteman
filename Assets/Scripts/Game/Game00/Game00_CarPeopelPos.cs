using UnityEngine;

public class Game00_CarPeopelPos : MonoBehaviour
{
    public GameObject downRoadPos;          // 下车路线
    public int monsterId = -1;

    Transform[] transRoadPos;

    // Use this for initialization
    public Game00_Monster Init(Game00_Main gameMain, Game00_Car car)
    {
        if (monsterId < 0 || monsterId >= gameMain.monster_Prefab.Length)
            return null;
        transRoadPos = downRoadPos.GetComponentsInChildren<Transform>();
        // 刷怪
        GameObject obj = GameObject.Instantiate(gameMain.monster_Prefab[monsterId], transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = Vector3.zero;
        Game00_Monster monster = obj.GetComponent<Game00_Monster>();
        monster.car = car;
        monster.Init(gameMain, gameMain.tab_MonsterBlood[monsterId], gameMain.tab_MonsterScore[monsterId], gameMain.tab_MonsterAttack[monsterId]);
        monster.ChangeStatue(en_MonsterSta.OnCar);
        monster.SetRoadPos(transRoadPos);
        //monster.SetRoadPos(downRoadPos);
        monster.onCar = true;
        gameMain.list_Monster.Add(monster);
        gameMain.monsterNum++;
        gameMain.monsterAliveNum++;

        return monster;
    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
