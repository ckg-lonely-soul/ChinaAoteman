using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Game00_Car : MonoBehaviour
{
    public GameObject peopelPos_Obj;
    public GameObject forntWhellCenterPos;
    public GameObject dieFireEff_Prefab;
    public NavMeshObstacle navMeshObstacle;
    public AudioSource audioSource_Run;
    Game00_CarPeopelPos[] peopelPos;
    // Car
    public GameObject[] wheel_F;    // 前
    public GameObject[] wheel_B;    // 后
    public WheelCollider[] wheelCollider_F;     // 前
    public WheelCollider[] wheelCollider_B;     // 后
    Rigidbody rigidbody;
    Vector3 wheelPostion;
    Quaternion wheelRotation;
    float steerAngle;
    float motorTorque;
    float brakeTorque;
    float angle;
    float speed;

    Vector3 currMoveTargetPos;
    Vector3 targetPos;
    int roadPosIndex;
    float runTime;
    int downId;

    //
    Game00_Monster monster;
    [HideInInspector]
    public en_MonsterSta monsterSta = en_MonsterSta.Walk;

    List<Game00_Monster> list_Monster = new List<Game00_Monster>();

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = Vector3.zero;
        navMeshObstacle.enabled = false;

        // 车怪
        monster = GetComponent<Game00_Monster>();
        monsterSta = monster.statue;
        //        
        peopelPos = peopelPos_Obj.GetComponentsInChildren<Game00_CarPeopelPos>();
        for (int i = 0; i < peopelPos.Length; i++)
        {
            list_Monster.Add(peopelPos[i].Init(monster.gameMain, this));
        }
        steerAngle = 0;
        roadPosIndex = 0;
        UpdateRoadPos();

        audioSource_Run.Play();
        audioSource_Run.SetScheduledStartTime(Random.Range(0f, 1f));
        ChangeStatue(en_MonsterSta.Run);
    }
    void FixedUpdate()
    {
        // 
        if (monster == null)
            return;
        switch (monster.statue)
        {
            case en_MonsterSta.Run:
            case en_MonsterSta.RunOut:
                // 自动开车:
                UpdateRun();
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        // 更新显示轮子状态
        for (int i = 0; i < wheel_F.Length; i++)
        {
            if (wheelCollider_F[i].enabled)
            {
                wheelCollider_F[i].GetWorldPose(out wheelPostion, out wheelRotation);
                wheel_F[i].transform.position = wheelPostion;
                wheel_F[i].transform.rotation = wheelRotation;
            }
        }
        for (int i = 0; i < wheel_B.Length; i++)
        {
            if (wheelCollider_B[i].enabled)
            {
                wheelCollider_B[i].GetWorldPose(out wheelPostion, out wheelRotation);
                wheel_B[i].transform.position = wheelPostion;
                wheel_B[i].transform.rotation = wheelRotation;
            }
        }

        if (monster == null)
            return;
        switch (monster.statue)
        {
            case en_MonsterSta.Run:
                // 自动开车:
                //UpdateRun();
                break;

            case en_MonsterSta.Stop:
                if (runTime > 0)
                {
                    runTime -= Time.deltaTime;
                }
                else
                {
                    if (rigidbody != null)
                    {
                        rigidbody.isKinematic = true;
                        for (int i = 0; i < wheel_F.Length; i++)
                        {
                            wheelCollider_F[i].enabled = false;
                        }
                        for (int i = 0; i < wheel_B.Length; i++)
                        {
                            wheelCollider_B[i].enabled = false;
                        }
                    }
                    if (downId < list_Monster.Count)
                    {
                        if (list_Monster[downId] != null)
                        {
                            if (list_Monster[downId].statue == en_MonsterSta.OnCar)
                            {
                                list_Monster[downId].ChangeStatue(en_MonsterSta.DownCar);
                                list_Monster[downId].transform.SetParent(null);
                                runTime = 0.2f;
                            }
                        }
                        downId++;
                    }
                    else if (IsAllPeopelDowned())
                    {
                        if (monster.remainBlood > 200)
                        {
                            monster.remainBlood = 200;
                        }
                        if (rigidbody != null)
                        {
                            rigidbody.isKinematic = false;
                            for (int i = 0; i < wheel_F.Length; i++)
                            {
                                wheelCollider_F[i].enabled = true;
                            }
                            for (int i = 0; i < wheel_B.Length; i++)
                            {
                                wheelCollider_B[i].enabled = true;
                            }
                        }
                        ChangeStatue(en_MonsterSta.RunOut);
                        break;
                    }
                }
                for (int i = 0; i < wheel_F.Length; i++)
                {
                    wheelCollider_F[i].brakeTorque = 400;
                    wheelCollider_F[i].motorTorque = 0;
                }
                for (int i = 0; i < wheel_B.Length; i++)
                {
                    wheelCollider_B[i].brakeTorque = 400;
                }
                break;

            case en_MonsterSta.RunOut:
                // UpdateRun();
                break;

            case en_MonsterSta.Idle:
                for (int i = 0; i < wheel_F.Length; i++)
                {
                    wheelCollider_F[i].brakeTorque = 400;
                    wheelCollider_F[i].motorTorque = 0;
                }
                for (int i = 0; i < wheel_B.Length; i++)
                {
                    wheelCollider_B[i].brakeTorque = 400;
                }
                break;
        }

        if (monster.statue == en_MonsterSta.Die && monsterSta != en_MonsterSta.Die)
        {
            monsterSta = en_MonsterSta.Die;
            //for (int i = 0; i < peplo_Obj.Length; i++) {
            //    Destroy(peplo_Obj[i]);
            //}
            navMeshObstacle.enabled = false;
            Destroy(navMeshObstacle);
            if (rigidbody != null)
            {
                Destroy(rigidbody);
                for (int i = 0; i < wheel_F.Length; i++)
                {
                    wheelCollider_F[i].enabled = false;
                }
                for (int i = 0; i < wheel_B.Length; i++)
                {
                    wheelCollider_B[i].enabled = false;
                }
            }
            if (dieFireEff_Prefab != null)
            {
                GameObject obj = GameObject.Instantiate(dieFireEff_Prefab, transform);
                obj.transform.position = transform.position;
                obj.transform.eulerAngles = transform.eulerAngles;
            }
        }
    }


    void ChangeStatue(en_MonsterSta sta)
    {
        monsterSta = sta;
        runTime = 0;

        switch (monsterSta)
        {
            case en_MonsterSta.Run:
            case en_MonsterSta.RunOut:
                for (int i = 0; i < wheel_F.Length; i++)
                {
                    wheelCollider_F[i].brakeTorque = 0;
                }
                for (int i = 0; i < wheel_B.Length; i++)
                {
                    wheelCollider_B[i].brakeTorque = 0;
                }

                audioSource_Run.Play();
                break;
            case en_MonsterSta.Idle:

                break;
            case en_MonsterSta.Stop:
                navMeshObstacle.enabled = true;
                downId = 0;
                runTime = 0.5f;
                audioSource_Run.Stop();
                break;
        }
        monster.ChangeStatue(sta);
    }

    bool IsAllPeopelDowned()
    {
        for (int i = 0; i < list_Monster.Count; i++)
        {
            if (list_Monster[i] != null)
            {
                if (list_Monster[i].statue == en_MonsterSta.OnCar)
                    return false;
                if (list_Monster[i].statue == en_MonsterSta.DownCar)
                    return false;
            }
        }
        return true;
    }

    void UpdateRun()
    {
        forntWhellCenterPos.transform.LookAt(currMoveTargetPos);
        RoadPosCheck();

        if (forntWhellCenterPos.transform.localEulerAngles.y >= 180)
        {
            angle = forntWhellCenterPos.transform.localEulerAngles.y - 360;
        }
        else
        {
            angle = forntWhellCenterPos.transform.localEulerAngles.y;
        }
        if (steerAngle < angle)
        {
            steerAngle += Time.deltaTime * 90;
        }
        else if (steerAngle > angle)
        {
            steerAngle -= Time.deltaTime * 90;
        }
        if (steerAngle > 45)
        {
            steerAngle = 45;
        }
        else if (steerAngle < -45)
        {
            steerAngle = -45;
        }

        speed = Vector3.Magnitude(rigidbody.velocity);

        motorTorque = 1000 - speed * 125;
        if (speed > 6)
        {
            brakeTorque = speed * 0.00001f;
        }
        else
        {
            brakeTorque = 0;
        }
        // print("CS: " + speed.ToString("F3") + ",  M: " + motorTorque.ToString("F3") + ",  B: " + brakeTorque.ToString("F3"));

        for (int i = 0; i < wheel_F.Length; i++)
        {
            wheelCollider_F[i].steerAngle = steerAngle;
            wheelCollider_F[i].motorTorque = motorTorque;
            wheelCollider_F[i].brakeTorque = brakeTorque;
        }
        for (int i = 0; i < wheel_B.Length; i++)
        {
            wheelCollider_B[i].brakeTorque = brakeTorque;
        }
    }

    void UpdateRoadPos()
    {
        if (monster.roadPos == null || roadPosIndex >= monster.roadPos.Length)
        {
            currMoveTargetPos = transform.position;
        }
        else
        {
            currMoveTargetPos = monster.roadPos[roadPosIndex].position;
        }
    }
    void NextRoadPos()
    {
        if (monster.roadPos != null)
        {
            if (roadPosIndex < monster.roadPos.Length)
            {
                if (monster.statue == en_MonsterSta.Run && monster.attackPos != null)
                {
                    if (monster.roadPos[roadPosIndex].gameObject == monster.attackPos)
                    {
                        ChangeStatue(en_MonsterSta.Stop);
                    }
                }
                roadPosIndex++;
                UpdateRoadPos();
            }
            else
            {
                ChangeStatue(en_MonsterSta.Idle);
            }
        }
    }
    void RoadPosCheck()
    {
        if (roadPosIndex >= monster.roadPos.Length)
            return;
        if (Vector3.Distance(forntWhellCenterPos.transform.position, currMoveTargetPos) < 1.5f ||
            Vector3.Angle(monster.roadPos[roadPosIndex].forward, forntWhellCenterPos.transform.position - currMoveTargetPos) < 90)
        {
            NextRoadPos();
        }
    }
}
