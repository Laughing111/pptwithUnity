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
using RDFW;
using UnityEngine;

public partial class e_Setting : UIBase{
    private Transform trans_e_btn_Add;
    private Transform trans_e_tip;
    partial void Search4Expeted()
    {
        trans_e_btn_Add=transform.SearchforChild("e_btn_Add");
        trans_e_tip=transform.SearchforChild("e_tip");
    }
}