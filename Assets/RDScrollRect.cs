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
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using DG.Tweening;

namespace RDFW
{
    public class RDScrollRect : MonoBehaviour
    {
        public bool isMouseDown;
        public bool isPointDown;
        public float endX;
        public float startX;
        public Camera renderCamera;
        private GameObject mask;
        private Vector3 dragDelta=Vector3.zero;
        private RectTransform rt;
        private Vector3 oldPos;
        private Touch selectTouch;
        public bool ControlIconSelectable;
        private Tweener tweener;
        [Range(0,1)]
        public float sensitive;
        private void Start()
        {
            
            rt = transform.GetComponent<RectTransform>();
            rt.anchoredPosition3D = new Vector3(startX, rt.anchoredPosition3D.y, rt.anchoredPosition3D.z);
            oldPos = rt.anchoredPosition3D;
            if (mask == null)
            {
                mask = new GameObject("DragMask");
                mask.transform.SetParent(transform.parent, false);
                mask.AddComponent<RectTransform>().sizeDelta = transform.GetComponent<RectTransform>().sizeDelta;
                mask.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                mask.transform.SetAsLastSibling();
                mask.AddComponent<RawImage>().color=new Color(1,1,1,0.3f);
                mask.GetComponent<RawImage>().raycastTarget = false;
            }
        }
        private void Update()
        {
            //触摸
            if (Input.touchCount > 0)
            {
                Touch[] touches = Input.touches;
                int tCount = touches.Length;
                for(int i = 0; i < tCount; i++)
                {
                    if (DetectedPosition(touches[i].position))
                    {
                        selectTouch = touches[i];
                        dragDelta = new Vector3(selectTouch.position.x, selectTouch.position.y, rt.anchoredPosition3D.z) - rt.anchoredPosition3D;
                        isPointDown = true;
                        break;
                    }
                }
            }

            if(Input.GetMouseButtonDown(0))
            {
                if (DetectedPosition(Input.mousePosition))
                {   
                    dragDelta = Input.mousePosition- rt.anchoredPosition3D;
                    Debug.Log("MY");
                    //增加一层与Icon之间的触摸阻隔
                    isMouseDown = true;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                isMouseDown = false;
                isPointDown = false;
                if (rt.anchoredPosition3D.x < endX)
                {
                    rt.DOLocalMoveX(endX,1- sensitive);
                }
                else if(rt.anchoredPosition3D.x > startX)
                {
                    rt.DOLocalMoveX(startX,1- sensitive);
                }
                
            }

            if (isMouseDown&&!isPointDown)
            {
                if (tweener!=null&&tweener.IsPlaying())
                {
                    tweener.Kill();
                    
                }
                tweener=rt.DOLocalMoveX(Input.mousePosition.x - dragDelta.x,0.1f)
                    .OnStart(()=>mask.GetComponent<RawImage>().raycastTarget = true)
                    .OnComplete(() => { StopAllCoroutines();StartCoroutine(WaiteForSeconds(() => mask.GetComponent<RawImage>().raycastTarget = false)); });
            }
            else if (isPointDown&&!isMouseDown)
            {
                if (tweener != null&&tweener.IsPlaying())
                {
                    tweener.Kill();
                }
                tweener = rt.DOLocalMoveX(selectTouch.position.x - dragDelta.x, 0.1f)
                    .OnStart(() => mask.GetComponent<RawImage>().raycastTarget = true)
                    .OnComplete(() => { StopAllCoroutines(); StartCoroutine(WaiteForSeconds(()=>mask.GetComponent<RawImage>().raycastTarget = false)); } );
            }

        } 

        void LockAllIcon(Transform t,bool value)
        {
            int count = t.childCount;
            for (int i = 0; i < count; i++)
            {
                if (t.GetComponent<RawImage>() != null)
                {
                    t.GetComponent<RawImage>().raycastTarget = value;
                }
                if (t.GetChild(i).GetComponent<RawImage>() != null)
                {
                    t.GetChild(i).GetComponent<RawImage>().raycastTarget = value;
                }
            }
        }

        private IEnumerator WaiteForSeconds(Action method)
        {
            ControlIconSelectable = true;
            yield return new WaitForSeconds(0.2f);
            method();
        }
        private bool DetectedPosition(Vector2 ScreenPosition)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(transform.GetComponent<RectTransform>(), ScreenPosition, renderCamera))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }




}

