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

public class Change2SettingBtn : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventTriggerManager.GetEventTriggerManager(gameObject).AddClickEvent(CallSetting);    
	}

    private void CallSetting(PointerEventData eventData)
    {
        if (eventData.clickCount >= 10)
        {
            eventData.clickCount = 0;
            UserModel.Ins.CurrentState = State.Setting;
        }
    }
}
