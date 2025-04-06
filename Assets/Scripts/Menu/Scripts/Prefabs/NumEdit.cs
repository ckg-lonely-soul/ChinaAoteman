using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 数字编辑器*****
public class NumEdit : MonoBehaviour {
	public const int NUM_START_X = -71;
	public const int NUM_DISTANCE_X = 36;

	public Text text_name;
//	public Text text_SelectSta;
	public Text[] textNum;

	//
	bool selectSta;		//选中状态
//	int selectBit;		//选中位
	float blenTime;		

	// Use this for initialization
	void Start () {
		for (int i = 0; i < textNum.Length; i++) {
		//	textNum [i] = Text.Instantiate (text_NumPrefab, new Vector3 (0, 0, 0), Quaternion.identity);				
			textNum [i].transform.localPosition = new Vector3 (NUM_START_X + i * NUM_DISTANCE_X, 0, 0);
			textNum [i].text = "0";
		}
	}
	
	// Update is called once per frame
	void Update () {

	}


	public void SetName(string name)
	{
		text_name.text = name;
	}
	public void SetValue(int val)
	{
		for (int i = 0; i < textNum.Length; i++) {
			textNum [textNum.Length - 1 - i].text = (val % 10).ToString ();
			val /= 10;
		}
	}
	public int GetValue()
	{
		int value = 0;

		for (int i = 0; i < textNum.Length; i++) {
			value *= 10;
			value += int.Parse (textNum [i].text);
		}
		return value;
	}


	public void BitValueAdd(int bitn)
	{
		if (bitn >= textNum.Length)
			return;
		int bv = (int.Parse (textNum [bitn].text) + 1) % 10;
		textNum [bitn].text = bv.ToString ();
	}
	public void BitValueDec(int bitn)
	{
		if (bitn >= textNum.Length)
			return;
		int bv = (int.Parse (textNum [bitn].text) + 9) % 10;
		textNum [bitn].text = bv.ToString ();
	}
}
