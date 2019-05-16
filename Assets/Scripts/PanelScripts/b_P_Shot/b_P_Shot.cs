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
    //private Texture2D[] gifTex;
    public delegate IEnumerator CoroutineFunc(int index);
    //private Thread gifThread;


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

        //为拍jpg和拍gif添加事件
        shotJPG += ShotInJpg;
        shotGIF += ShotInGIF;
        base.PanelInit();
    }

    private void TakeShot(PointerEventData eventData)
    {
        //按下拍照按钮后 检查并选择拍照的模式
        CheckMode(shotJPG, shotGIF);
    }

    public Text dbug;

    /// <summary>
    /// 在jpg模式下拍照
    /// </summary>
    private void ShotInJpg()
    {
        //控制UI变化
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
        //if (!File.Exists(Application.streamingAssetsPath + "/gif/"))
        //{
        //    Directory.CreateDirectory(Application.streamingAssetsPath + "/gif/");
        //}
        //GameObject.Find("GifMaker")?.GetComponent<GIFFactory>()?.MakeGif(()=>
        //{

        //    
        //    UserModel.Ins.StoreLocalPath(Application.streamingAssetsPath + "/gif/yb.gif");
        //    UserModel.Ins.CurrentState = State.Edite;

        //});
        ShotUIDis(true);
        UserModel.Ins.CurrentState = State.Edite;
        MessageCenter.GetMessage(PanelAssets_c_P_Edite.c_P_Edite.ToString(), new MessageAddress("ResetIcon", null));
    }

    /// <summary>
    /// 选择拍照的模式进行拍照
    /// </summary>
    /// <param name="_shotJPG"></param>
    /// <param name="_shotGIF"></param>
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
    /// 拍照UI变化
    /// </summary>
    /// <param name="isOnDis"></param>
    private void ShotUIDis(bool isOnDis)
    {
        //控制提示消失
        trans_b_tip.gameObject.SetActive(isOnDis);
        //控制按钮消失
        trans_b_btn_shot.gameObject.SetActive(isOnDis);
        trans_b_hint_look.gameObject.SetActive(isOnDis);
        trans_b_btn_close.gameObject.SetActive(isOnDis);
        trans_b_grayMask.gameObject.SetActive(isOnDis);
    }

    /// <summary>
    /// 存储方法，JPG模式，到这里结束
    /// </summary>
    /// <returns></returns>
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
    
    /// <summary>
    /// 存储方法，GIF模式
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private IEnumerator PauseCamAndSave2ModelGif(int index)
    {

        CameraManager.Instan().CameraPause();
        //截屏处理 截屏后存储
        yield return (Capture(index,(Texture2D te, int i) => {
            UserModel.Ins.StoreFXJPGTex(te, i);
            //if (!File.Exists(Application.streamingAssetsPath + "/png/"))
            //{
            //    Directory.CreateDirectory(Application.streamingAssetsPath + "/png/");
            //}
            //File.WriteAllBytes(Application.streamingAssetsPath + "/png/" + i + ".png", te.EncodeToPNG()); 
            CameraManager.Instan().CameraResume();
        }));
    }

    /// <summary>
    /// 实际存储方法，存gif
    /// </summary>
    /// <param name="index">gif编号</param>
    /// <param name="captureSucceed">存储完执行</param>
    /// <returns></returns>
    private IEnumerator Capture(int index, Action<Texture2D, int> captureSucceed)
    {
        RectTransform rt = trans_b_hint_look.GetComponent<RectTransform>();
        yield return new WaitForEndOfFrame();
        Texture2D te = new Texture2D((int)rt.sizeDelta.x, (int)rt.sizeDelta.y,TextureFormat.ARGB32,false);
        te.ReadPixels(new Rect(240, 0, te.width, te.height), 0, 0);
        te.Apply();
        //yield return new WaitForSeconds(0.1f);
        captureSucceed?.Invoke(te, index);
    }
    /// <summary>
    /// 实际存储方法，存储jpg
    /// </summary>
    /// <param name="captureSucceed">存储完执行</param>
    /// <returns></returns>
    private IEnumerator Capture(Action<Texture2D> captureSucceed)
    {
        RectTransform rt = trans_b_hint_look.GetComponent<RectTransform>();
        yield return new WaitForEndOfFrame();
        Texture2D te = new Texture2D((int)rt.sizeDelta.x, (int)rt.sizeDelta.y);
        te.ReadPixels(new Rect(240, 0, te.width, te.height), 0, 0);
        te.Apply();
        //yield return new WaitForSeconds(0.1f);
        captureSucceed?.Invoke(te);
    }

    /// <summary>
    /// 到计时处理Jpg,拍照完成后，进入存储方法
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
        //进入存储方法
        yield return PauseCamAndSave2ModelJpg();
    }

    /// <summary>
    ///  到计时处理GIF
    /// </summary>
    /// <param name="shotCount"></param>
    /// <param name="btnCount"></param>
    /// <param name="ShotendEvent"></param>
    /// <param name="AllEnd">保证存储完后，执行最终的方法</param>
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
            trans_b_hint_gif.gameObject.SetActive(true);
            trans_b_hint_photo.gameObject.SetActive(false);
        }
        else
        {
            trans_b_hint_gif.gameObject.SetActive(false);
            trans_b_hint_photo.gameObject.SetActive(true);
        }
    }

    public override void OnInActive()
    {
        CameraManager.Instan().StopCamera();
        base.OnInActive();
    }
}