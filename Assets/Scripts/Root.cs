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
    public bool mode_photo;
    public bool mode_gif;
    public bool mode_pstick;
    public bool mode_gstick;
    public bool mode_print;
    public bool mode_frame;
    public int num_pfp;
    public int num_ph;
    public int num_pw;
    // Start is called before the first frame update
    void Start()
    {
        UserModel.Ins.mode_photo= mode_photo;
        UserModel.Ins.mode_gif = mode_gif;
        UserModel.Ins.mode_gstick = mode_gstick;
        UserModel.Ins.mode_pstick = mode_pstick;
        UserModel.Ins.mode_print = mode_print;
        UserModel.Ins.mode_frame = mode_frame;
        UserModel.Ins.num_pfp = num_pfp;
        UserModel.Ins.num_ph = num_ph;
        UserModel.Ins.num_pw = num_pw;

        UserModel.Ins.CurrentState = panelState;
        UserModel.Ins.SetGifRate(0.2f);
        Debug.Log(Application.streamingAssetsPath.Remove(0, 3)+ "/png/yb.png");
    }
}
