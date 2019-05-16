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
using System;
using UnityEngine.UI;
using System.Threading;
using LCPrinter;
using System.IO;

public partial class d_P_Share : UIBase
{
    private Texture2D[] Gif;
    
    private Thread thread;
    UploadMan um;
    UploadObject uo;
    string fileName;
    private bool sucEncode;
    private string printerName;
    public Text debugTxt;

    public void Awake()
    {
        if(File.Exists(Application.streamingAssetsPath + "/printer.txt"))
        {
            printerName = File.ReadAllText(Application.streamingAssetsPath + "/printer.txt");
        }  
        um = new UploadMan(fileName, (x) => {
            Debug.Log("去生成二维码:" + x + "https://h5.btech.cc/ab06-723e-11e9-baa1/index.html?i=" + fileName);
            sucEncode = true;
        }, 
        UserModel.Ins.GetLocalPath());
        uo = new UploadObject();
        PanelInit();
    }
    public override void PanelInit()
    {
        Search4Expeted();
        Register();
        base.PanelInit();
    }

    private void Register()
    {
        RegisterInterObjectPointUp(trans_d_btn_back, (x) => UserModel.Ins.CurrentState = State.Edite);
        RegisterInterObjectPointUp(trans_d_btn_home, Reset2Home);
        RegisterInterObjectPointUp(trans_d_btn_print, PrintImg);
    }

    private void PrintImg(PointerEventData eventData)
    {
        try
        {
            //Print.PrintTextureByPath(UserModel.Ins.GetLocalPath(), 1, "DP-DS620");
            Texture2D tex = UserModel.Ins.GetFXJPGTex();
            
            Print.PrintTexture(tex.EncodeToPNG(),1, printerName);
        }
        catch(Exception e) {
            Debug.Log(e.ToString());
            debugTxt.text = e.ToString();
        }
        
    }

    private void Reset2Home(PointerEventData eventData)
    {
        UserModel.Ins.CurrentState = State.Index;
        //是否要注销？
    }
    private void Update()
    {
        if (sucEncode)
        {
            sucEncode = false;
            trans_qrcode.gameObject.SetActive(true);
            trans_qrcode.GetComponent<RawImage>().texture = QRManager.Encode("https://h5.btech.cc/ab06-723e-11e9-baa1/index.html?i=" + fileName,226,226);
        }
    }

    public override void OnActive()
    {
       
        base.OnActive();

        CheckMode();
        trans_qrcode.gameObject.SetActive(false);
        um.localPath = UserModel.Ins.GetLocalPath();
        thread = new Thread(new
             ParameterizedThreadStart(push));
        thread.IsBackground = true;
        thread.Start(um);
    }

    public void push(object o)
    {
       
        uo.PutObjFromLocal((UploadMan)o);
    }

    private void CheckMode()
    {
        if (UserModel.Ins.GetShotMode() == ShotMode.Gif)
        {
            fileName = RDTimer.GetTime() + ".gif";
            um.fileName = fileName;
            trans_d_bg_gif.gameObject.SetActive(true);
            trans_d_bg_photo.gameObject.SetActive(false);
            trans_d_btn_print.gameObject.SetActive(false);
            Display(true);
        }
        else
        {
            fileName = RDTimer.GetTime() + ".png";
            um.fileName = fileName;
            trans_d_bg_gif.gameObject.SetActive(false);
            trans_d_bg_photo.gameObject.SetActive(true);
            Display(false);
        }
    }

    private void Display(bool isGif)
    {
        if (isGif)
        {
            Gif = UserModel.Ins.GetGIFTex();
            GameObject cameraRect = RDResManager.LoadWithCache<GameObject>("CameraRect/d_camFX");
            Transform camRectTrans = Instantiate(cameraRect).transform;
            camRectTrans.SetParent(trans_d_picture, false);
            StartCoroutine(ShowAnim(camRectTrans));
        }
        else
        {
            GameObject cameraRect = RDResManager.LoadWithCache<GameObject>("CameraRect/d_camFX");
            Transform camRectTrans = Instantiate(cameraRect).transform;
            camRectTrans.SetParent(trans_d_picture, false);
            camRectTrans.GetComponent<RawImage>().texture = UserModel.Ins.GetFXJPGTex();
        }
    }

    private IEnumerator ShowAnim(Transform camRectTrans)
    {
        float rate = UserModel.Ins.GetGifRate();
        while(true)
        {
            for (int i = 0; i < Gif.Length; i++)
            {
                camRectTrans.GetComponent<RawImage>().texture = Gif[i];
                yield return new WaitForSeconds(rate);
            }
        }
    }

    public override void OnInActive()
    {
        base.OnInActive();
    }
}