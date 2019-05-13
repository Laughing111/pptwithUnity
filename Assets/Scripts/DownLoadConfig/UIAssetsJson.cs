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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UIAssetsJson {
    public global global;
    public home_page home_page;
    public photo_page photo_page;
    public stick_page stick_page;
    public result_page result_page;
}

[Serializable]
public class global
{
    public int jpg_mode;
    public int gif_mode;
    public int jpg_stick_mode;
    public int gif_stick_mode;
    public int print_mode;
    public int photo_frame_mode;
    public int photo_width;
    public int photo_height;
    public photo_frame photo_frame;
}
[Serializable]
public class photo_frame
{
    public int padding;
    public string url;
}
#region home_page
[Serializable]
public class home_page
{
    public string jpg_background;
    public string gif_background;
    public button next_button;
    public button jpg_button;
    public button gif_button;
}
#endregion

#region photo_page
[Serializable]
public class photo_page
{
    public string background;
    public padding_url jpg_capture_hint;
    public padding_url gif_capture_hint;
    public button capture_button;
    public button back_button;
    public padding_url look_hint;
    public countdown countdown;
    public texturePadding liveview;
}

[Serializable]
public class padding_url
{
    public string name;
    public int x;
    public int y;
    public string url;
}
[Serializable]
public class countdown
{
    public int x;
    public int y;
    public List<string> url_list;
}

#endregion

#region stick_page
[Serializable]
public class stick_page
{
    public string background;
    public button gif_left_button;
    public button gif_right_button;
    public button next_button;
    public button retake_button;
    public string stick_background;
    public string reset_button;
    public texturePadding result_photo;
    public List<string> sticks;
}
#endregion

#region result_page
[Serializable]
public class result_page
{
    public string jpg_background;
    public string gif_background;
    public button back_button;
    public button print_button;
    public button home_button;
    public texturePadding result_photo;
    public qrcode qrcode;
}
[Serializable]
public class qrcode
{
    public int x;
    public int y;
    public int size;
}
#endregion

[Serializable]
public class url
{
    public string normal;
    public string active;
}
[Serializable]
public class button
{
    public string name;
    public int x;
    public int y;
    public url url;
}
[Serializable]
public class texturePadding
{
    public int x;
    public int y;
    public int width;
    public int height;
}
