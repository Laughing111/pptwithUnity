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
using UnityEngine.UI;

public class GetTex : MonoBehaviour
{
    public int index;
    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<RawImage>().texture = UserModel.Ins.GetPreGifTex()[index];
    }
}
