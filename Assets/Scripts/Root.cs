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

public class Root : MonoBehaviour
{
    public State panelState;
    // Start is called before the first frame update
    void Start()
    {
        UserModel.Ins.CurrentState = panelState;
        UserModel.Ins.SetGifRate(0.2f);
        Debug.Log(Application.streamingAssetsPath.Remove(0, 3)+ "/png/yb.png");
    }
}
