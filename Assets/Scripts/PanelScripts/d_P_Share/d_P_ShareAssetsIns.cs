using RDFW;
using UnityEngine;

public partial class d_P_Share : UIBase{
    private Transform trans_d_picture;
    private Transform trans_d_btn_print;
    private Transform trans_d_btn_home;
    private Transform trans_d_text;
    private Transform trans_d_text_photo;
    private Transform trans_qrcode;
    private Transform trans_d_btn_back;
    private Transform trans_d_text_gif;
    partial void Search4Expeted()
    {
        trans_d_picture=transform.SearchforChild("d_picture");
        trans_d_btn_print=transform.SearchforChild("d_btn_print");
        trans_d_btn_home=transform.SearchforChild("d_btn_home");
        trans_d_text=transform.SearchforChild("d_text");
        trans_d_text_photo=transform.SearchforChild("d_text_photo");
        trans_qrcode=transform.SearchforChild("qrcode");
        trans_d_btn_back=transform.SearchforChild("d_btn_back");
        trans_d_text_gif=transform.SearchforChild("d_text_gif");
    }
}