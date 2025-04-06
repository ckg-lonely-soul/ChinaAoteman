using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;
using System;


// Sprite 精灵的变换 ; 移动,缩放,透明度; 
public class ChangeImage : MonoBehaviour {
    public enum en_Change_MoveType {
        Up = 0,         //	向上
        Down,           //	向下
        Left,           //	向左
        Right,          //	向右
        Fornt,          //
        Back,           //
    }

    //	public event void FunSetTransparent(float transparent);
    public float showTime;                      // 显示时间: 大于0: 时间到销毁
                                                // 透明变化
    public float transparentSpeed;              // 透明速度 : 为0时不透明, 大于0时全透明的时候销毁
    public float transparentDelay;              // 延时
                                                //	public float transparentLimit;			// 限值: 达到限值时停止, 限值为0时无效
                                                // 移动变化
    public en_Change_MoveType moveType;
    public float moveDelay;                     // 延时
    public float moveSpeed;                     // 移动速度 : 为0时不移动
    public float moveLimit;                     // 移动距离 : 
                                                // 缩放变化
    public float scaleDelay;                    // 延时
    public float scaleSpeed;                    // 大于0: 放大; 小于0:缩小
    public float scaleLimit;                    // 限值: 达到限值时停止, 限值为0时无效

    // 变化
    float transparent;                          // 透明度
    public float scale;                                // 缩放
    float moveDistance;                         //

    // 组件
    Image spriteRenderer;
    Image[] spriteRendererChildren;    // 子对象的 

    //
    void Awake() {
        spriteRenderer = GetComponent<Image>();
        spriteRendererChildren = GetComponentsInChildren<Image>();

        //
        transparent = 1;
        scale = 1;
        moveDistance = 0;
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //
        if (showTime > 0) {
            showTime -= Time.deltaTime;
            if (showTime <= 0) {
                Destroy(gameObject);
                return;
            }
        }

        // 透明变化
        if (transparentDelay > 0) {
            transparentDelay -= Time.deltaTime;
        } else if (transparentSpeed > 0) {
            transparent -= Time.deltaTime * transparentSpeed;
            if (transparent <= 0) {
                Destroy(gameObject);
                return;
            }
            //
            if (spriteRenderer) {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, transparent);
            }
            //
            foreach (Image sp in spriteRendererChildren) {
                sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, transparent);
            }
        }
        // 移动变化
        if (moveDelay > 0) {
            moveDelay -= Time.deltaTime;
        } else if (moveSpeed > 0) {
            if (moveLimit == 0 || (moveLimit > 0 && moveDistance < moveLimit)) {
                switch (moveType) {
                case en_Change_MoveType.Up:
                    transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
                    break;
                case en_Change_MoveType.Down:
                    transform.Translate(Vector3.down * Time.deltaTime * moveSpeed);
                    break;
                case en_Change_MoveType.Left:
                    transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
                    break;
                case en_Change_MoveType.Right:
                    transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
                    break;
                case en_Change_MoveType.Fornt:
                    transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
                    break;
                case en_Change_MoveType.Back:
                    transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
                    break;
                }
                moveDistance += Time.deltaTime * moveSpeed;
            }
        }
        // 缩放变化
        if (scaleDelay > 0) {
            scaleDelay -= Time.deltaTime;
        } else if (scaleSpeed != 0) {
            if (scaleLimit > 0) {
                if ((scaleSpeed > 0 && scale >= scaleLimit) || (scaleSpeed < 0 && scale <= scaleLimit))
                    return;
            }
            scale += Time.deltaTime * scaleSpeed;
            if (scale <= 0) {
                Destroy(gameObject);
                return;
            }
            if (scaleLimit > 0) {
                if (scaleSpeed > 0) {
                    if (scale >= scaleLimit) {
                        scale = scaleLimit;
                    }
                } else {
                    if (scale < scaleLimit) {
                        scale = scaleLimit;
                    }
                }
            }

            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
