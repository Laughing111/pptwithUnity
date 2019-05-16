/***
* Copyright(C) by #Betech-上海赢赞数字科技有限公司#
* All rights reserved.
* FileName:     #PhotoBooth#
* Author:       #Laughing#
* Version:      #v1.0#
* Date:         #20190430_1415#
* Description:  ##
* History:      ##
***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RDFW;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IconController : MonoBehaviour
{
    private Touch oldTouch1;  //手指触摸点1
    private Touch oldTouch2;  //手指触摸点2   

    private bool mirrored;
    private bool rotated;
    private bool moved;
    public bool isControl;

    private Transform rotateBtn;
    private Transform mirroredBtn;
    private Transform closeBtn;
    private Transform box;
    public Transform tools;
    int toolCounts;

    public void OnDisable()
    {
        isControl = false;
        tools.gameObject.SetActive(false);
    }
    private void Start()
    {
        rotateBtn = transform.SearchforChild("c_btn_RAndS");
        mirroredBtn = transform.SearchforChild("c_btn_mirror");
        closeBtn = transform.SearchforChild("c_btn_close");
        box = transform.parent.parent.parent.SearchforChild("c_box");
        tools = transform.SearchforChild("tools");
        toolCounts = tools.childCount;
        isControl = true;
        EventTriggerManager.GetEventTriggerManager(rotateBtn.gameObject).AddPDEvent(RotateEventDown);
        EventTriggerManager.GetEventTriggerManager(rotateBtn.gameObject).AddPUEvent(RotateEventUp);
        EventTriggerManager.GetEventTriggerManager(mirroredBtn.gameObject).AddPUEvent(mirrorEvent);
        EventTriggerManager.GetEventTriggerManager(closeBtn.gameObject).AddPUEvent(closeEvent);
        EventTriggerManager.GetEventTriggerManager(gameObject).AddPUEvent(ActiveTool);
        EventTriggerManager.GetEventTriggerManager(box.gameObject).AddPUEvent(InActiveTool);
    }

    private void InActiveTool(PointerEventData eventData)
    {
        isControl = false;
    }

    private void ActiveTool(PointerEventData eventData)
    {
        isControl = true;
        transform.SetAsLastSibling();
    }

    private void closeEvent(PointerEventData eventData)
    {
        Destroy(gameObject);
    }

    private void mirrorEvent(PointerEventData eventData)
    {
        mirrored = true;
    }

    private void RotateEventDown(PointerEventData eventData)
    {
        rotated = true;
        moved = false;
    }
    private void RotateEventUp(PointerEventData eventData)
    {
        rotated = false;
        moved = true;
    }

    private void Update()
    {
        if (isControl)
        {
            tools.gameObject.SetActive(true);
            #region move
            if (moved)
            {
                if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Touch touch = Input.GetTouch(0);
                    transform.GetComponent<RectTransform>().anchoredPosition3D += new Vector3(touch.deltaPosition.x, touch.deltaPosition.y, 0);

                }
            }
            #endregion

            #region Scale
            if (Input.touchCount > 1)
            {
                moved = false;
                rotated = false;
                ////多点触摸, 放大缩小  
                Touch newTouch1 = Input.GetTouch(0);
                Touch newTouch2 = Input.GetTouch(1);

                //第2点刚开始接触屏幕, 只记录，不做处理  
                if (newTouch2.phase == TouchPhase.Began)
                {
                    oldTouch2 = newTouch2;
                    oldTouch1 = newTouch1;
                    return;
                }
                //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型  
                float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
                float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

                //两个距离之差，为正表示放大手势， 为负表示缩小手势  
                float offset = newDistance - oldDistance;
                //放大因子， 一个像素按 0.01倍来算  
                float scaleFactor = offset / 200f;
                Vector3 localScale = transform.GetChild(0).localScale;
                Vector3 scale;
                scale = new Vector3(localScale.x + scaleFactor,
                     localScale.y + scaleFactor,
                     1);
                //最小缩放到 0.3 倍  
                if (scale.y > 0.3f)
                {
                    transform.GetChild(0).localScale = scale;
                }
                //记住最新的触摸点，下次使用  
                oldTouch1 = newTouch1;
                oldTouch2 = newTouch2;
            }
            else
            {
                moved = true;
            }
            #endregion

            #region Mirror

            if (mirrored)
            {
                transform.GetChild(0).localScale = new Vector3(-transform.GetChild(0).localScale.x, transform.GetChild(0).localScale.y, transform.GetChild(0).localScale.z);

                mirrored = false;
            }
            #endregion

            #region Rotate 
            if (rotated)
            {
                moved = false;

                SetToolsHide(false);
                Vector3 oldPosRotate = rotateBtn.localPosition;

                Vector3 fromRotate = oldPosRotate - transform.localPosition;
                //控制旋转按钮跟着手指移动
                if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Vector3 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

                    rotateBtn.Translate(touchDeltaPosition.x/* * speed*/, touchDeltaPosition.y /** speed*/, 0);

                    float scaleOff = ((rotateBtn.localPosition - transform.localPosition).magnitude - fromRotate.magnitude) / 200;

                    //设置旋转
                    Vector3 normal = Vector3.Cross(fromRotate, rotateBtn.localPosition - transform.localPosition);

                    float angle = 0;

                    if (normal.z > 0)
                    {
                        angle = Vector3.Angle(fromRotate, rotateBtn.localPosition - transform.localPosition);
                    }
                    else
                    {
                        angle = 360 - Vector3.Angle(fromRotate, rotateBtn.localPosition - transform.localPosition);
                    }

                    rotateBtn.transform.localPosition = oldPosRotate;

                    transform.Rotate(new Vector3(0, 0, angle));

                    Vector3 localScale = transform.GetChild(0).localScale;

                    Vector3 scale;

                    scale = new Vector3(localScale.x + scaleOff,
                     localScale.y + scaleOff,
                     1);

                    //最小缩放到 0.3 倍  
                    if (scale.y > 0.3f)
                    {
                        transform.GetChild(0).localScale = scale;
                    }
                }
            }
            else
            {
                SetToolsHide(true);
                moved = true;
            }

            #endregion


            #region 判断是否在最前端
            if(transform.parent.GetChild(transform.parent.childCount-1)!=transform)
            {
                isControl = false;
            }
            #endregion
        }
        else
        {
            tools.gameObject.SetActive(false);
        }

    }


    void SetToolsHide(bool active)
    { 
        for (int i = 0; i < toolCounts; i++)
        {
            if (active)
            {
                tools.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else
            {
                tools.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0);
            } 
        } 
    }

}
