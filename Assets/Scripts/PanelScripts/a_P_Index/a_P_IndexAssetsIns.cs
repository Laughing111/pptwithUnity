using RDFW;
using UnityEngine;

public partial class a_P_Index : UIBase{
    private Transform trans_a_btn_next;
    private Transform trans_a_btn_right;
    private Transform trans_a_btn_gif_normal;
    private Transform trans_a_gifInActive;
    private Transform trans_a_textTitle;
    private Transform trans_a_gifActive;
    private Transform trans_a_btn_gif_active;
    private Transform trans_a_btn_photo_active;
    private Transform trans_a_btn_photo_normal;
    private Transform trans_a_btn_left;
    private Transform trans_a_bg_gif;
    private Transform trans_a_bg_photo;
    partial void Search4Expeted()
    {
        trans_a_btn_next=transform.SearchforChild("a_btn_next");
        trans_a_btn_right=transform.SearchforChild("a_btn_right");
        trans_a_btn_gif_normal=transform.SearchforChild("a_btn_gif_normal");
        trans_a_gifInActive=transform.SearchforChild("a_gifInActive");
        trans_a_textTitle=transform.SearchforChild("a_textTitle");
        trans_a_gifActive=transform.SearchforChild("a_gifActive");
        trans_a_btn_gif_active=transform.SearchforChild("a_btn_gif_active");
        trans_a_btn_photo_active=transform.SearchforChild("a_btn_photo_active");
        trans_a_btn_photo_normal=transform.SearchforChild("a_btn_photo_normal");
        trans_a_btn_left=transform.SearchforChild("a_btn_left");
        trans_a_bg_gif=transform.SearchforChild("a_bg_gif");
        trans_a_bg_photo=transform.SearchforChild("a_bg_photo");
    }
}