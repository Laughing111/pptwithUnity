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
    }
}