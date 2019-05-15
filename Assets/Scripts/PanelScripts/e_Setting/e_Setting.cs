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
using UnityEngine.EventSystems;
using RDFW;
using System;
using UnityEngine.UI;
using System.IO;

public partial class e_Setting : UIBase {
    private UIDownLoadManager uiDownLoadManager;
    
    public void Awake()
    {
        PanelInit();
        uiDownLoadManager = UIDownLoadManager.Ins;
        
    }
    public override void PanelInit()
    {
        Search4Expeted();
        RegisterInterObjectPointUp(trans_e_btn_Add, DownLoadUI);
        AddMessage2LocalBox("PopInput", (x) => trans_e_TipGroup.gameObject.SetActive(true));
        base.PanelInit();
    }

    private void DownLoadUI(PointerEventData eventData)
    {
        UIDownLoadManager.Ins.SetUUID(trans_e_InputUUID.GetComponent<InputField>().text);
        trans_e_TipGroup.gameObject.SetActive(false);
        UIDownLoadManager.Ins.CheckAndUpdate();
    }

    public override void OnActive()
    {
        trans_e_TipGroup.gameObject.SetActive(false);
        trans_e_tip.GetComponent<Text>().text = "正在为您初始化资源...";
        UIDownLoadManager.Ins.debugText = trans_e_tip.GetComponent<Text>();
        uiDownLoadManager.CheckAndUpdate();
        base.OnActive();
    }
    public override void OnInActive()
    {
        base.OnInActive();
    }
}