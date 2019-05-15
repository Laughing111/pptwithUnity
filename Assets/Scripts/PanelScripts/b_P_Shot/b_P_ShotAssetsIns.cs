using RDFW;
using UnityEngine;

public partial class b_P_Shot : UIBase{
    private Transform trans_b_camTex;
    private Transform trans_b_tip;
    private Transform trans_b_mask;
    private Transform trans_b_btn;
    private Transform trans_b_tip_gif;
    private Transform trans_b_tip_countdown;
    private Transform trans_b_btn_shot;
    private Transform trans_b_tip_photo;
    private Transform trans_b_flash;
    private Transform trans_b_btn_close;
    private Transform trans_b_grayMask;
    partial void Search4Expeted()
    {
        trans_b_camTex=transform.SearchforChild("b_camTex");
        trans_b_tip=transform.SearchforChild("b_tip");
        trans_b_mask=transform.SearchforChild("b_mask");
        trans_b_btn=transform.SearchforChild("b_btn");
        trans_b_tip_gif=transform.SearchforChild("b_tip_gif");
        trans_b_tip_countdown=transform.SearchforChild("b_tip_countdown");
        trans_b_btn_shot=transform.SearchforChild("b_btn_shot");
        trans_b_tip_photo=transform.SearchforChild("b_tip_photo");
        trans_b_flash=transform.SearchforChild("b_flash");
        trans_b_btn_close=transform.SearchforChild("b_btn_close");
        trans_b_grayMask=transform.SearchforChild("b_grayMask");
    }
}