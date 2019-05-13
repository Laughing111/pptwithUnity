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
using UnityEngine.EventSystems;
using RDFW;
using UnityEngine.UI;
using System;
using System.Linq;
using System.IO;
using Texture2D = UnityEngine.Texture2D;
using System.Threading;
using DG.Tweening;

public partial class b_P_Shot : UIBase
{

    private Action shotJPG;
    private Action shotGIF;
    private Texture2D[] gifTex;
    public delegate IEnumerator CoroutineFunc(int index);
    private Thread gifThread;


    public void Awake()
    {
        PanelInit();
    }
    public override void PanelInit()
    {
        Search4Expeted();
        //回到首页
        RegisterInterObjectPointUp(trans_b_btn_close, (x) => UserModel.Ins.CurrentState = State.Index);
        //控制拍照
        RegisterInterObjectPointUp(trans_b_btn_shot, TakeShot);

        shotJPG += ShotInJpg;
        shotGIF += ShotInGIF;
        base.PanelInit();
    }

    private void TakeShot(PointerEventData eventData)
    {
        CheckMode(shotJPG, shotGIF);
    }

    public Text dbug;
    private void ShotInJpg()
    {
        ShotUIDis(false);
        //开始倒计时
        try
        {
            StartCoroutine(CountDown(trans_b_tip_countdown));
        }
        catch(Exception e)
        {
            dbug.text = e.ToString();
            Debug.Log(e.ToString());
        }
        
    }

    private void ShotInGIF()
    {
        ShotUIDis(false);
        //开始倒计时
        StartCoroutine(CountDown(3, trans_b_tip_countdown, PauseCamAndSave2ModelGif, GifShotEnd));
    }

    private void GifShotEnd()
    {
        if (!File.Exists(Application.persistentDataPath + "/gif/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/gif/");
        }
        GameObject.Find("GifMaker")?.GetComponent<GIFFactory>()?.MakeGif(()=>
        {

            ShotUIDis(true);
            UserModel.Ins.StoreLocalPath(Application.persistentDataPath + "/gif/yb.gif");
            UserModel.Ins.CurrentState = State.Share;

        });
    }
    private void CheckMode(Action _shotJPG, Action _shotGIF)
    {
        if (UserModel.Ins.GetShotMode() == ShotMode.Gif)
        {
            shotGIF?.Invoke();
        }
        else
        {
            shotJPG?.Invoke();
        }
    }

    /// <summary>
    /// 拍照UI变
    /// </summary>
    /// <param name="isOnDis"></param>
    private void ShotUIDis(bool isOnDis)
    {
        //控制提示消失
        trans_b_tip.gameObject.SetActive(isOnDis);
        //控制按钮消失
        trans_b_btn_shot.gameObject.SetActive(isOnDis);
        trans_b_mask.gameObject.SetActive(isOnDis);
        trans_b_btn_close.gameObject.SetActive(isOnDis);
    }

    private IEnumerator PauseCamAndSave2ModelJpg()
    {
        CameraManager.Instan().CameraPause();
        //截屏处理 截屏后存储
        yield return Capture((Texture2D te) => 
        {
            
            UserModel.Ins.StoreCamRaw(te);
            CameraManager.Instan().CameraResume();
            ShotUIDis(true);
            UserModel.Ins.CurrentState = State.Edite;
            MessageCenter.GetMessage(PanelAssets_c_P_Edite.c_P_Edite.ToString(), new MessageAddress("ResetIcon", null));
        });
    }

    private IEnumerator PauseCamAndSave2ModelGif(int index)
    {

        CameraManager.Instan().CameraPause();
        //截屏处理 截屏后存储
        yield return (Capture(index,(Texture2D te, int i) => {
            UserModel.Ins.StoreFXJPGTex(te, i);
            if (!File.Exists(Application.persistentDataPath + "/png/"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/png/");
            }
            File.WriteAllBytes(Application.persistentDataPath + "/png/" + i + ".png", te.EncodeToPNG()); 
            CameraManager.Instan().CameraResume();
        }));
    }

    private IEnumerator Capture(int index, Action<Texture2D, int> captureSucceed)
    {
        RectTransform rt = trans_b_mask.GetComponent<RectTransform>();
        yield return new WaitForEndOfFrame();
        Texture2D te = new Texture2D((int)rt.sizeDelta.x, (int)rt.sizeDelta.y,TextureFormat.ARGB32,false);
        te.ReadPixels(new Rect(240, 0, te.width, te.height), 0, 0);
        te.Apply();
        //yield return new WaitForSeconds(0.1f);
        captureSucceed?.Invoke(te, index);
    }
    private IEnumerator Capture(Action<Texture2D> captureSucceed)
    {
        RectTransform rt = trans_b_mask.GetComponent<RectTransform>();
        yield return new WaitForEndOfFrame();
        Texture2D te = new Texture2D((int)rt.sizeDelta.x, (int)rt.sizeDelta.y);
        te.ReadPixels(new Rect(240, 0, te.width, te.height), 0, 0);
        te.Apply();
        //yield return new WaitForSeconds(0.1f);
        captureSucceed?.Invoke(te);
    }

    /// <summary>
    /// 到计时处理Jpg
    /// </summary>
    /// <param name="btnCount"></param>
    /// <param name="endEvent"></param>
    /// <returns></returns>
    private IEnumerator CountDown(Transform btnCount)
    {

        for (int i = 0; i < 3; i++)
        {
            btnCount.GetChild(i).gameObject.SetActive(false);
        }
        btnCount.gameObject.SetActive(true);
        int time = 2;
        while (time >= 0)
        {
            btnCount.GetChild(time).gameObject.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            btnCount.GetChild(time).gameObject.SetActive(false);
            time--;
        }
        btnCount.gameObject.SetActive(false);
        trans_b_flash.gameObject.SetActive(true);
        trans_b_flash.GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
        trans_b_flash.GetComponent<RawImage>().DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.7f);
        trans_b_flash.gameObject.SetActive(false);
        yield return PauseCamAndSave2ModelJpg();
    }

    /// <summary>
    ///  到计时处理GIF
    /// </summary>
    /// <param name="shotCount"></param>
    /// <param name="btnCount"></param>
    /// <param name="ShotendEvent"></param>
    /// <param name="AllEnd"></param>
    /// <returns></returns>
    private IEnumerator CountDown(int shotCount, Transform btnCount, CoroutineFunc ShotendEvent, Action AllEnd)
    {
        for (int j = 0; j < shotCount; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                btnCount.GetChild(i).gameObject.SetActive(false);
            }
            btnCount.gameObject.SetActive(true);
            int time = 2;
            while (time >= 0)
            {
                btnCount.GetChild(time).gameObject.SetActive(true);
                yield return new WaitForSeconds(1.0f);
                btnCount.GetChild(time).gameObject.SetActive(false);
                time--;
            }
            btnCount.gameObject.SetActive(false);
            trans_b_flash.gameObject.SetActive(true);
            trans_b_flash.GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
            trans_b_flash.GetComponent<RawImage>().DOFade(0, 0.5f);
            yield return new WaitForSeconds(0.7f);
            trans_b_flash.gameObject.SetActive(false);
            yield return ShotendEvent(j);
        }
        yield return new WaitForSeconds(0.5f);
        AllEnd?.Invoke();
    }

    public override void OnActive()
    {
        trans_b_camTex.GetComponent<RawImage>().texture = CameraManager.Instan().CameraOpen(1920, 1080, 60);
        //获取UserModel的数据
        ShotMode mode = UserModel.Ins.GetShotMode();
        SetTipWithMode(mode);
        base.OnActive();
    }

    private void SetTipWithMode(ShotMode mode)
    {
        trans_b_tip.gameObject.SetActive(true);
        if (mode == ShotMode.Gif)
        {
            trans_b_tip_gif.gameObject.SetActive(true);
            trans_b_tip_photo.gameObject.SetActive(false);
        }
        else
        {
            trans_b_tip_gif.gameObject.SetActive(false);
            trans_b_tip_photo.gameObject.SetActive(true);
        }
    }

    public override void OnInActive()
    {
        CameraManager.Instan().StopCamera();
        base.OnInActive();
    }
}