using RDFW;
using UnityEngine;

public partial class a_P_Index : UIBase{
    private Transform trans_a_icon_gif;
    private Transform trans_a_icon_photo;
    private Transform trans_a_gifInActive;
    private Transform trans_a_gifActive;
    private Transform trans_a_btn_next;
    private Transform trans_a_btn_right;
    private Transform trans_a_btn_left;
    private Transform trans_a_jpgInAc;
    private Transform trans_a_gifInAc;
    partial void Search4Expeted()
    {
        trans_a_icon_gif=transform.SearchforChild("a_icon_gif");
        trans_a_icon_photo=transform.SearchforChild("a_icon_photo");
        trans_a_gifInActive=transform.SearchforChild("a_gifInActive");
        trans_a_gifActive=transform.SearchforChild("a_gifActive");
        trans_a_btn_next=transform.SearchforChild("a_btn_next");
        trans_a_btn_right=transform.SearchforChild("a_btn_right");
        trans_a_btn_left=transform.SearchforChild("a_btn_left");
        trans_a_jpgInAc=transform.SearchforChild("a_jpgInAc");
        trans_a_gifInAc=transform.SearchforChild("a_gifInAc");
    }
}