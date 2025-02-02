﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RDFW;

namespace RDFW
{
    /// <summary>
    /// 公共消息处理中心
    /// </summary>
    /// <remarks>公共消息处理中心</remarks>
    public class MessageCenter
    {

        /// <summary>
        /// 委托：消息类型
        /// </summary>
        /// <param name="messageAddress">需要传递的消息键值对</param>
        public delegate void Message(MessageAddress messageAddress);
        /// <summary>
        /// 公共信箱<UI的名字 消息实体>
        /// </summary>
        public static Dictionary<string, Message> messageBox = new Dictionary<string, Message>();

        public static void AddMessage2Center(string panelName, Message message)
        {
            if (!messageBox.ContainsKey(panelName))
            {
                //Debug.Log("消息中心存入" + panelName + "的消息");
                messageBox.Add(panelName, null);
            }
            messageBox[panelName] = message;
            if (messageBox[panelName] == null)
            {
                Debug.Log("消息中心为空");
            }
            // Debug.Log(message.ToString());
        }


        public static void GetMessage(string panelName, MessageAddress messageAddress)
        {   
            
            Message message;
            if(messageAddress.Address==null)
            {
                messageAddress.Address = panelName;
            }
            if (messageBox.TryGetValue(panelName, out message))
            {
                if (message != null)
                {
                    //Debug.Log("现在去筛选阅读" + panelName + "的本地消息");
                    message(messageAddress);
                }
            }
        }

        public static void OpenPanel(string panelName)
        {
            MessageAddress ma = new MessageAddress( "PanelActive",null, panelName);
            GetMessage(panelName, ma);
        }
        public static void ClosePanel(string panelName)
        {
            MessageAddress ma = new MessageAddress("PanelInActive", null, panelName);
            GetMessage(panelName, ma);
        }


    }

    /// <summary>
    /// 键值对，配合委托数据传递
    /// </summary>
    public class MessageAddress
    {
        public string Address{ get; set; }
        public object Parameters { get; }
        public string FuncName { get; }
        /// <summary>
        /// 消息键值对
        /// </summary>
        /// <param name="_address">地址，具体是哪个子部件</param>
        /// <param name="_funcName">方法名称</param>
        /// <param name="_parameters">需要携带的参数</param>
        public MessageAddress(string _funcName, object _parameters, string _address=null)
        {
            Address = _address; 
            Parameters = _parameters;
            FuncName = _funcName;
        }

    }
}

