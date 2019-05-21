
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RDFW;
using System;

public partial class a_P_Index : UIBase {
    private bool isJPGMode;
    public void Awake()
    {
        PanelInit();
    }
    public override void PanelInit()
    {
        Search4Expeted();
        RegisterInterObjectPointUp(trans_a_btn_left, ChangeMode);
        RegisterInterObjectPointUp(trans_a_btn_right, ChangeMode);
        RegisterInterObjectPointUp(trans_a_btn_gif_normal, ChangeMode);
        RegisterInterObjectPointUp(trans_a_btn_photo_normal, ChangeMode);
        RegisterInterObjectPointUp(trans_a_btn_next, Go2Shot);
        base.PanelInit();
    }

    private void Go2Shot(PointerEventData eventData)
    {
        UserModel.Ins.CurrentState = State.Shoot;
    }

    private void ChangeMode(PointerEventData eventData)
    {
        isJPGMode = !isJPGMode;
        DisGif_JPG(isJPGMode);
    }

    public override void OnActive()
    {   
        if (UserModel.Ins.GetShotMode() == ShotMode.Jpg)
        {
            isJPGMode = true;
        }
        else
        {
            isJPGMode = false;
        }
        DisGif_JPG(isJPGMode);
        CheckFuncMode();
        base.OnActive();
    }

    private void CheckFuncMode()
    {
        //说明含有拍照模式
        if (UserModel.Ins.mode_photo&&!UserModel.Ins.mode_gif)
        {
            UserModel.Ins.SetShotMode(ShotMode.Jpg);
            trans_a_gifActive.gameObject.SetActive(false);
            trans_a_btn_gif_normal.gameObject.SetActive(false);
            trans_a_btn_right.gameObject.SetActive(false);
            trans_a_btn_left.gameObject.SetActive(false);
        }
        else if(!UserModel.Ins.mode_photo &&UserModel.Ins.mode_gif)
        {
            UserModel.Ins.SetShotMode(ShotMode.Gif);
            trans_a_gifInActive.gameObject.SetActive(false);
            trans_a_btn_photo_normal.gameObject.SetActive(false);
            trans_a_btn_right.gameObject.SetActive(false);
            trans_a_btn_left.gameObject.SetActive(false);
        }
        else if(!UserModel.Ins.mode_photo&&!UserModel.Ins.mode_gif)
        {
            trans_a_gifInActive.gameObject.SetActive(false);
            trans_a_gifActive.gameObject.SetActive(false);
            trans_a_btn_next.gameObject.SetActive(false);
        }
    }

    private void DisGif_JPG(bool isJpg)
    {   
        if(isJpg)
        {
            trans_a_gifActive.gameObject.SetActive(false);
            trans_a_gifInActive.gameObject.SetActive(true);
            trans_a_bg_gif.gameObject.SetActive(false);
            trans_a_bg_photo.gameObject.SetActive(true);
            UserModel.Ins.SetShotMode(ShotMode.Jpg);
        }
        else
        {
            trans_a_gifActive.gameObject.SetActive(true);
            trans_a_gifInActive.gameObject.SetActive(false);
            trans_a_bg_gif.gameObject.SetActive(true);
            trans_a_bg_photo.gameObject.SetActive(false);
            UserModel.Ins.SetShotMode(ShotMode.Gif);
        }
    }

    
    public override void OnInActive()
    {
        base.OnInActive();
    }
}