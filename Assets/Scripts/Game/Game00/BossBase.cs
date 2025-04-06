using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossBase : MonoBehaviour
{
    public enum TypeMove
    {
        one,
        two,
        three,
    }
    public string Tag = "Boss";
    public float dis = 6;
    public TypeMove typeMove = TypeMove.one;
    private void Start()
    {
        GetMoveDis();
    }
    public float GetMoveDis()
    {
        switch (typeMove)
        {
            case TypeMove.one:
                dis = 6;
                break;
            case TypeMove.two:
                dis = 3;
                break;
            case TypeMove.three:
                dis = 1;
                break;
        }
        return dis;
    }

}
