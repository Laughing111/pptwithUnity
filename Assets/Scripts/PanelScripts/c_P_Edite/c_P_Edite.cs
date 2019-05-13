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
using System.IO;
using UnityEngine.UI;

public partial class c_P_Edite : UIBase {
    public void Awake()
    {
        PanelInit();
    }
    public override void PanelInit()
    {  
        Search4Expeted();
        Register();
        AddLocalMsg();
        base.PanelInit();
    }

    private void AddLocalMsg()
    {
        AddMessage2LocalBox("ResetIcon", ResetIocn);
    }

    private void ResetIocn(MessageAddress messageAddress)
    {
        int iconCounts = trans_c_Edite_Rect.childCount;
        if(iconCounts>1)
        {
            for (int i = 1; i < iconCounts; i++)
            {
                Destroy(trans_c_Edite_Rect.GetChild(i).gameObject);
            }
        }
        
    }

    private void Register()
    {
        RegisterInterObjectPointUp(trans_c_btn_back,(x)=>UserModel.Ins.CurrentState=State.Shoot);
        RegisterInterObjectPointUp(trans_c_btn_next, SaveFXTexAndCheck2Share);

        int iconCounts = trans_c_icon_group.childCount;
        for(int i = 1; i < iconCounts; i++)
        {
            RegisterInterObjectPointUp(trans_c_icon_group.GetChild(i), AddIcon);
        }

        
    }
    
    private void AddIcon(PointerEventData eventData)
    {
        string addPrefabName = eventData.pointerPress.name+"i";
        GameObject go=RDResManager.LoadWithCache<GameObject>("icon_addPrefabs/" + addPrefabName);
        Transform goTrans = Instantiate(go).transform;
        goTrans.SetParent(trans_c_Edite_Rect, false);
    }

    private void SaveFXTexAndCheck2Share(PointerEventData eventData)
    {
        int count = trans_c_Edite_Rect.childCount;
        if(count>1)
        {
            for (int i = 1; i < count; i++)
            {
                Debug.Log("重置");
                trans_c_Edite_Rect.GetChild(i).GetComponent<IconController>().isControl=false;
            }
        }
        StartCoroutine(CaptureFXTexture(EndEvent));
    }

    

    /// <summary>
    /// 最后切换
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void EndEvent(string arg1, Texture2D arg2)
    {
        UserModel.Ins.StoreFXJPGTex(arg2);
        UserModel.Ins.StoreLocalPath(arg1);
        UserModel.Ins.CurrentState = State.Share;
    }

    private IEnumerator CaptureFXTexture(Action<string,Texture2D> endEvent)
    {
        RectTransform rt =trans_c_bg_Rect.GetComponent<RectTransform>();
        yield return new WaitForEndOfFrame();
        Texture2D te = new Texture2D((int)rt.sizeDelta.x,(int)rt.sizeDelta.y);
        int rectX = (int)(Screen.width * 0.5 - (te.width * 0.5 - rt.anchoredPosition3D.x));
        int rectY = (int)(Screen.height * 0.5 - (te.height * 0.5 - rt.anchoredPosition3D.y));
        te.ReadPixels(new Rect(rectX, rectY, te.width, te.height),0,0);
        te.Apply();
        if (!File.Exists(Application.persistentDataPath + "/png/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/png/");
        }
        File.WriteAllBytes(Application.persistentDataPath + "/png/yb.png", te.EncodeToPNG());
        
        endEvent(Application.persistentDataPath + "/png/yb.png", te);
    }

    public Text dbug;
    public override void OnActive()
    {
        trans_c_TexRaw.GetComponent<RawImage>().texture = UserModel.Ins.GainCamRaw();
        if (UserModel.Ins.GainCamRaw() == null)
        {
            dbug.text = "没有图像";
        }
        else
        {
            dbug.text = "有图像";
        }
        base.OnActive();
    }
    public override void OnInActive()
    {
        base.OnInActive();
    }
}