using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RDFW;

namespace RDFW
{
    public class EventTriggerManager : UnityEngine.EventSystems.EventTrigger
    {


        /// <summary>
        /// 事件管理器的点击事件委托
        /// </summary>
        /// <param name="eventData">固定参数</param>
        public delegate void ClickListened(PointerEventData eventData);

        public delegate void PointDownListened(PointerEventData eventData);

        public delegate void PointUpListened(PointerEventData eventData);

        public ClickListened clickListened;
        public PointDownListened pointDownListened;
        public PointUpListened pointUpListened;
        /// <summary>
        /// 获取监听组件
        /// </summary>
        /// <param name="Listened">被监听的对象</param>
        /// <returns></returns>
        public static EventTriggerManager GetEventTriggerManager(GameObject Listened)
        {
            EventTriggerManager eventTriggerManager = Listened.GetComponent<EventTriggerManager>();
            if (eventTriggerManager == null)
            {
                eventTriggerManager = Listened.AddComponent<EventTriggerManager>();
            }
            return eventTriggerManager;

        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            //如果对象被点击，则出发这个方法
            clickListened?.Invoke(eventData);
            if (GameObject.Find("TimeManager") != null)
            {
                TimeManager tm = TimeManager.GetTimeManagerIns();
                tm.ResetTimeAndStartCount();
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            pointDownListened?.Invoke(eventData);
            if (GameObject.Find("TimeManager") != null)
            {
                TimeManager tm = TimeManager.GetTimeManagerIns();
                tm.ResetTimeAndStartCount();
            }
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            pointUpListened?.Invoke(eventData);
            if (GameObject.Find("TimeManager") != null)
            {
                TimeManager tm = TimeManager.GetTimeManagerIns();
                tm.ResetTimeAndStartCount();
            }
        }

        public void AddClickEvent(ClickListened ClickEvent)
        {

            clickListened += ClickEvent;
        }
        public void AddPUEvent(PointUpListened ClickEvent)
        {

            pointUpListened += ClickEvent;
        }
        public void AddPDEvent(PointDownListened ClickEvent)
        {

            pointDownListened += ClickEvent;
        }

        public void RemoveAllRegister()
        {
            clickListened = null;
            pointDownListened = null;
            pointUpListened = null;
        }
    }
}


