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

public partial class e_Setting : UIBase {
    public void Awake()
    {
        PanelInit();
    }
    public override void PanelInit()
    {
        Search4Expeted();
        RegisterInterObjectPointUp(trans_e_btn_Add, DownLoadUI);
        base.PanelInit();
    }

    private void DownLoadUI(PointerEventData eventData)
    {
        trans_e_tip.GetComponent<Text>().text = "正在加载中，请稍后...";
        UIDownLoadManager.Ins.CheckAndUpdate();
    }

    public override void OnActive()
    {
        trans_e_tip.GetComponent<Text>().text = "点击按钮进行UI加载...";
        base.OnActive();
    }
    public override void OnInActive()
    {
        base.OnInActive();
    }
}