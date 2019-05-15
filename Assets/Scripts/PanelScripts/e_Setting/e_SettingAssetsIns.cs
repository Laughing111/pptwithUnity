using RDFW;
using UnityEngine;

public partial class e_Setting : UIBase{
    private Transform trans_e_tip;
    private Transform trans_e_btn_Add;
    private Transform trans_e_TipGroup;
    private Transform trans_e_InputUUID;
    partial void Search4Expeted()
    {
        trans_e_tip=transform.SearchforChild("e_tip");
        trans_e_btn_Add=transform.SearchforChild("e_btn_Add");
        trans_e_TipGroup=transform.SearchforChild("e_TipGroup");
        trans_e_InputUUID=transform.SearchforChild("e_InputUUID");
    }
}