using UnityEngine;

public class BossBase : MonoBehaviour
{
    public enum TypeMove
    {
        one,
        two,
        three,
    }
    public string Tag = "Boss";
    public float dis = 60;
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
                dis = 60;
                break;
            case TypeMove.two:
                dis = 30;
                break;
            case TypeMove.three:
                dis = 10;
                break;
        }
        return dis;
    }

}
