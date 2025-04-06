using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Button : MonoBehaviour {
    public Image image_BackG;
    public Image image_SelectFlag;
    public Text text;
    public Sprite sprite_Normal;
    public Sprite sprite_Selected;

    bool isSelect;
    Vector3 movePos;
    float runTime;

	// Use this for initialization	
    public void SetSelect(bool select) {
        isSelect = select;
        if(image_SelectFlag != null) {
            image_SelectFlag.gameObject.SetActive(select);
        }
        if (isSelect) {
            image_BackG.sprite = sprite_Selected;
            movePos = new Vector3(20, 0);
        } else {
            image_BackG.sprite = sprite_Normal;
            movePos = Vector3.zero;
        }
        runTime = 0;
    }

    // Update is called once per frame
    void Update() {
        if (image_SelectFlag == null)
            return;
        if (isSelect) {
            runTime += Time.deltaTime;
            if (runTime >= 0.5f) {
                runTime = 0;
                if (image_SelectFlag.gameObject.activeSelf) {
                    image_SelectFlag.gameObject.SetActive(false);
                } else {
                    image_SelectFlag.gameObject.SetActive(true);
                }
            }
        }
        if (text.transform.localPosition == movePos)
            return;
        text.transform.localPosition = Vector3.Lerp(text.transform.localPosition, movePos, 0.2f);
    }
}
