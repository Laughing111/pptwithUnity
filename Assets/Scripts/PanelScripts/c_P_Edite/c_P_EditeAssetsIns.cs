using RDFW;
using UnityEngine;

public partial class c_P_Edite : UIBase{
    private Transform trans_c_btn_back;
    private Transform trans_c_btn_next;
    private Transform trans_c_TexRaw;
    private Transform trans_c_mask_picture;
    private Transform trans_c_bg_picture;
    private Transform trans_c_icon_group;
    private Transform trans_c_bg_Rect;
    private Transform trans_c_Edite_Rect;
    private Transform trans_c_icon_gif;
    private Transform trans_c_gificon_0;
    private Transform trans_c_gificon_1;
    private Transform trans_c_gificon_2;
    private Transform trans_c_icon_jpg;
    private Transform trans_c_btn_right;
    private Transform trans_c_btn_left;
    partial void Search4Expeted()
    {
        trans_c_btn_back=transform.SearchforChild("c_btn_back");
        trans_c_btn_next=transform.SearchforChild("c_btn_next");
        trans_c_TexRaw=transform.SearchforChild("c_TexRaw");
        trans_c_mask_picture=transform.SearchforChild("c_mask_picture");
        trans_c_bg_picture=transform.SearchforChild("c_bg_picture");
        trans_c_icon_group=transform.SearchforChild("c_icon_group");
        trans_c_bg_Rect=transform.SearchforChild("c_bg_Rect");
        trans_c_Edite_Rect=transform.SearchforChild("c_Edite_Rect");
        trans_c_icon_gif=transform.SearchforChild("c_icon_gif");
        trans_c_gificon_0=transform.SearchforChild("c_gificon_0");
        trans_c_gificon_1=transform.SearchforChild("c_gificon_1");
        trans_c_gificon_2=transform.SearchforChild("c_gificon_2");
        trans_c_icon_jpg=transform.SearchforChild("c_icon_jpg");
        trans_c_btn_right=transform.SearchforChild("c_btn_right");
        trans_c_btn_left=transform.SearchforChild("c_btn_left");
    }
}