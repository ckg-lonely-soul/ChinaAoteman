using UnityEngine;
using UnityEngine.UI;

public class AccValue : MonoBehaviour
{
    public Text text_Name;
    public Text text_Data;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetName(string name)
    {
        text_Name.text = name;
    }

    public void SetData(float data)
    {
        text_Data.text = data.ToString();
    }
}
