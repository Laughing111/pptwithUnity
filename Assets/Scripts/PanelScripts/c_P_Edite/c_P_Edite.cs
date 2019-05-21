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

public partial class c_P_Edite : UIBase
{   
    private ShotMode shotMode;
    private Texture2D[] preTex;
    private int gifIndex;
    private string jpgFileName = "yb.png";
    private string gifFileName = "yb.gif";
    public int iconCounts;
    public float countSpace = 194.0f;
    private Texture2D iconBGTex;
    public bool offLine;
    private RawImage[] gifTexRaw;
    private RenderTexture[] gifRT;
    public void Awake()
    {
        //if(offLine)
        //{
        CheckAndCreateIconBtn();
        //} 
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
        AddMessage2LocalBox("UpdateIcon", UpdateIcons);
    }

    /// <summary>
    /// 更新icon的素材，供外部调用
    /// </summary>
    /// <param name="messageAddress"></param>
    private void UpdateIcons(MessageAddress messageAddress)
    {
        CheckAndCreateIconBtn(messageAddress.Parameters as Texture2D[]);
        RegisterIcon();
    }

    /// <summary>
    /// 清除icons
    /// </summary>
    /// <param name="messageAddress"></param>
    private void ResetIocn(MessageAddress messageAddress)
    {
        Transform jt = trans_c_icon_jpg;
        int iconCountsJ = jt.childCount;
        if (iconCountsJ > 0)
        {
            for (int i = 0; i < iconCountsJ; i++)
            {
                Destroy(jt.GetChild(i).gameObject);
            }
        }
        Transform gt = trans_c_icon_gif;
        //gif数量
        for (int i = 0; i < 3; i++)
        {
            int gtCount = gt.GetChild(i).childCount;
            Transform gificon = gt.GetChild(i);
            for (int j = 0; j < gtCount; j++)
            {
                Destroy(gificon.GetChild(j).gameObject);
            }
        }
    }

    private void Register()
    {
        RegisterInterObjectPointUp(trans_c_btn_back, (x) => UserModel.Ins.CurrentState = State.Shoot);
        RegisterInterObjectPointUp(trans_c_btn_next, SaveFXTexAndCheck2Share);
        RegisterInterObjectPointUp(trans_c_btn_gr, ChangeGifTexAdd);
        RegisterInterObjectPointUp(trans_c_btn_gl, ChangeGifTexMinus);
        RegisterIcon();
    }

    private void RegisterIcon()
    {
        int iconCounts = trans_c_icon_group.childCount;
        for (int i = 1; i < iconCounts; i++)
        {
            RegisterInterObjectClick(trans_c_icon_group.GetChild(i), AddIcon);
        }
    }

    private void DisPlayGif()
    {
        trans_c_TexRaw.GetComponent<RawImage>().texture = gifRT[gifIndex];
        for (int i = 0; i < 3; i++)
        {
            trans_c_icon_gif.GetChild(i).gameObject.SetActive(false);
        }
        trans_c_icon_gif.GetChild(gifIndex).gameObject.SetActive(true);
    }

    private void ChangeGifTexMinus(PointerEventData eventData)
    {
        if (gifIndex > 0)
        {
            Transform iconT = trans_c_icon_gif.GetChild(gifIndex);
            int count = iconT.childCount;
            for (int i = 0; i < count; i++)
            {
                //Debug.Log("重置");
                iconT.GetChild(i).GetComponent<IconController>().isControl = false;
            }
            gifIndex--;
            DisPlayGif();
        }
        if (gifIndex == 0)
        {
            trans_c_btn_gl.GetComponent<Image>().gameObject.SetActive(false);
        }
        else if (gifIndex == 1)
        {
            trans_c_btn_gr.GetComponent<Image>().gameObject.SetActive(true);
        }
    }
    private void ChangeGifTexAdd(PointerEventData eventData)
    {
        if (gifIndex < 2)
        {
            Transform iconT = trans_c_icon_gif.GetChild(gifIndex);
            int count = iconT.childCount;
            for (int i = 0; i < count; i++)
            {
                //Debug.Log("重置");
                iconT.GetChild(i).GetComponent<IconController>().isControl = false;
            }

            gifIndex++;
            DisPlayGif();
        }
        if (gifIndex == 2)
        {
            trans_c_btn_gr.GetComponent<Image>().gameObject.SetActive(false);
        }
        else if (gifIndex == 1)
        {
            trans_c_btn_gl.GetComponent<Image>().gameObject.SetActive(true);
        }
    }

    private void AddIcon(PointerEventData eventData)
    {
        if (!eventData.dragging && !eventData.IsPointerMoving())
        {
            string addPrefabName = eventData.pointerPress.name + "i";
            GameObject go = RDResManager.LoadWithCache<GameObject>("icon_addPrefabs/c_icon_selectInsAdd");
            Transform goTrans = Instantiate(go).transform;
            goTrans.name = addPrefabName;
            int oldCount;
            Transform parent;
            if (shotMode == ShotMode.Jpg)
            {
                parent = trans_c_icon_jpg;
                oldCount = parent.childCount;
            }
            else
            {
                parent = trans_c_icon_gif.GetChild(gifIndex);
                oldCount = parent.childCount;
            }
            goTrans.SetParent(parent, false);
            goTrans.GetChild(0).GetComponent<RawImage>().texture = eventData.pointerPress.transform.GetChild(0).GetComponent<RawImage>().texture;
            goTrans.GetChild(0).GetComponent<RawImage>().SetNativeSize();
            if (oldCount > 0)
            {
                goTrans.GetComponent<RectTransform>().anchoredPosition3D = goTrans.parent.GetChild(oldCount - 1).GetComponent<RectTransform>().anchoredPosition3D + new Vector3(30, 0, 0);

            }
        }
    }

    private void SaSaveFXTexAndCheck2Share(bool isGif)
    {   
        if (!isGif)
        {
            trans_c_GifWaite.gameObject.SetActive(false);
            StartCoroutine(CaptureFXTexture(EndEventJpg));
        }
        else
        {
            trans_c_GifWaite.gameObject.SetActive(true);
            StartCoroutine(CaptureFXTextureGif(EndEventGif));
        }
    }
    private void SaveFXTexAndCheck2Share(PointerEventData eventData)
    {
        int count = trans_c_Edite_Rect.childCount;

        for (int i = 1; i < count; i++)
        {
            int iconCount = trans_c_Edite_Rect.GetChild(i).childCount;
            Transform iconT = trans_c_Edite_Rect.GetChild(i);
            for (int j = 0; j < iconCount; j++)
            {
                IconController ict = iconT.GetChild(j).GetComponent<IconController>();
                if (ict == null)
                {
                    Transform ictT = iconT.GetChild(j);
                    int jcount = ictT.childCount;
                    for (int k = 0; k < jcount; k++)
                    {
                        ictT.GetChild(k).GetComponent<IconController>().isControl = false;
                    }
                }
                else
                {
                    ict.isControl = false;
                }
            }
        }

        if (shotMode == ShotMode.Jpg)
        {
            StartCoroutine(CaptureFXTexture(EndEventJpg));
        }
        else
        {
            trans_c_gificon_0.SetParent(gifTexRaw[0].transform, false);
            trans_c_gificon_0.gameObject.SetActive(true);
            trans_c_gificon_1.SetParent(gifTexRaw[1].transform, false);
            trans_c_gificon_1.gameObject.SetActive(true);
            trans_c_gificon_2.SetParent(gifTexRaw[2].transform, false);
            trans_c_gificon_2.gameObject.SetActive(true);
            StartCoroutine(CaptureFXTextureGif(EndEventGif));
        }
    }
    /// <summary>
    /// 最后切换-JPG
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void EndEventJpg(string arg1, Texture2D arg2)
    {
        UserModel.Ins.StoreFXJPGTex(arg2);
        if (!File.Exists(arg1))
        {
            Directory.CreateDirectory(arg1);
        }
        if(UserModel.Ins.mode_frame)
        {
            Debug.Log("width:" + UserModel.Ins.num_pfp);
            arg2 = CameraManager.Instan().AddOutLine(arg2, UserModel.Ins.num_pfp, Color.white);
        }
        File.WriteAllBytes(arg1 + jpgFileName, arg2.EncodeToPNG());
        UserModel.Ins.StoreLocalPath(arg1 + jpgFileName);
        UserModel.Ins.CurrentState = State.Share;
    }
    /// <summary>
    /// 最后切换-Gif
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void EndEventGif(string arg1, Texture2D[] arg2)
    {
        if (!File.Exists(arg1))
        {
            Directory.CreateDirectory(arg1);
        } 
        for (int i = 0; i < 3; i++)
        {
            if (UserModel.Ins.mode_frame)
            {
                arg2[i] = CameraManager.Instan().AddOutLine(arg2[i], UserModel.Ins.num_pfp, Color.white);
            }
            File.WriteAllBytes(arg1 + i.ToString() + ".png", arg2[i].EncodeToPNG());
        }

        //GifMaker
        if (!File.Exists(Application.streamingAssetsPath + "/gif/"))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath + "/gif/");
        }
        GameObject.Find("GifMaker")?.GetComponent<GIFFactory>()?.MakeGif(() =>
        {
            isComp = true;
        });
    }

    private void Update()
    {
        if (isComp)
        {
            isComp = false;
            StopAllCoroutines();
            UserModel.Ins.StoreLocalPath(Application.streamingAssetsPath + "/gif/yb.gif");
            UserModel.Ins.CurrentState = State.Share;
        }
    }

    private bool isComp;


    private void AddWhiteOutLine()
    {
        Texture2D[] finalTex = UserModel.Ins.GetGIFTex();
        CameraManager cm = CameraManager.Instan();
        UserModel user = UserModel.Ins;
        Texture2D temp;
        for (int i = 0; i < 3; i++)
        {
            if (finalTex[i].width != 880)
            {
                temp = CameraManager.Instan().ScaleTexture(finalTex[i], 880, 660);
                user.StoreGIFTex(temp, i);
            }
            temp = cm.AddOutLine(finalTex[i], 10, Color.white);
            File.WriteAllBytes(Application.streamingAssetsPath + "/png/" + i + ".png", temp.EncodeToPNG());
            user.StoreGIFTex(temp, i);
        }
    }

    private IEnumerator CaptureFXTexture(Action<string, Texture2D> endEvent)
    {
        RectTransform rt = trans_c_bg_Rect.GetComponent<RectTransform>();
        yield return new WaitForEndOfFrame();
        Texture2D te = new Texture2D((int)rt.sizeDelta.x, (int)rt.sizeDelta.y);
        int rectX = (int)(Screen.width * 0.5 - (te.width * 0.5 - rt.anchoredPosition3D.x));
        int rectY = (int)(Screen.height * 0.5 - (te.height * 0.5 - rt.anchoredPosition3D.y));
        te.ReadPixels(new Rect(rectX, rectY, te.width, te.height), 0, 0);
        te.Apply();
        endEvent(Application.streamingAssetsPath + "/png/", te);
    }

    private IEnumerator CaptureFXTextureGif(Action<string, Texture2D[]> endEvent)
    {
        //yield return new WaitForSeconds(0.5f);
        Texture2D[] gifCom = new Texture2D[3];
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForEndOfFrame();
            RenderTexture.active = gifRT[i];
            Texture2D te = new Texture2D((int)gifTexRaw[i].rectTransform.sizeDelta.x, (int)gifTexRaw[i].rectTransform.sizeDelta.y);
            Debug.Log(te.width + ":" + te.height);
            float TEx = Screen.width * 0.5f - gifTexRaw[i].rectTransform.sizeDelta.x * 0.5f + gifTexRaw[i].rectTransform.anchoredPosition3D.x;
            float TEy = Screen.height * 0.5f - gifTexRaw[i].rectTransform.anchoredPosition3D.y - gifTexRaw[i].rectTransform.sizeDelta.y * 0.5f;
            te.ReadPixels(new Rect(TEx, TEy, te.width, te.height), 0, 0);
            Debug.Log(TEx + "," + TEy + "," + te.width + "," + te.height);
            te.Apply();
            UserModel.Ins.StoreGIFTex(te, i);
            gifCom[i] = te;
            te = null;
        }
        trans_c_gificon_0.SetParent(trans_c_icon_gif, false);
        trans_c_gificon_1.SetParent(trans_c_icon_gif, false);
        trans_c_gificon_2.SetParent(trans_c_icon_gif, false);

        endEvent(Application.streamingAssetsPath + "/png/", gifCom);
    }

    // public Text dbug;
    public override void OnActive()
    {
        CheckModeAndUpdateTex();
        base.OnActive();
        if (shotMode == ShotMode.Jpg)
        {
            if (!UserModel.Ins.mode_pstick)
            {
                SaSaveFXTexAndCheck2Share(false);
            }
        }
        else
        {
           if(!UserModel.Ins.mode_gstick)
            {
                SaSaveFXTexAndCheck2Share(true);
            }
        }
        
    }


    private void CheckAndCreateIconBtn()
    {
        if (iconCounts > 0)
        {
            for (int i = 0; i < iconCounts; i++)
            {
                GameObject go = RDResManager.LoadWithCache<GameObject>("icon_prefabs/c_icon_selectIns");
                GameObject insGo = Instantiate(go);
                insGo.name = "c_icon_selectIns" + (i + 1).ToString();
                if (trans_c_icon_group == null)
                {
                    trans_c_icon_group = transform.SearchforChild(PanelAssets_c_P_Edite.c_icon_group.ToString());
                }
                insGo.transform.SetParent(trans_c_icon_group, false);
                insGo.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(133 + countSpace * (i + 1), 0, 0);
            }
        }
    }
    public void CheckAndCreateIconBtn(Texture2D[] te)
    {
        int iCount = te.Length;
        if (iCount >= 2)
        {
            iconBGTex = te[0];
            trans_c_url_sdefault.GetComponent<RawImage>().texture = te[1];
            for (int i = 2; i < iCount; i++)
            {
                GameObject go = RDResManager.LoadWithCache<GameObject>("icon_prefabs/c_icon_selectIns");
                GameObject insGo = Instantiate(go);
                insGo.name = "c_icon_selectIns" + (i - 1).ToString();
                if (trans_c_icon_group == null)
                {
                    trans_c_icon_group = transform.SearchforChild(PanelAssets_c_P_Edite.c_icon_group.ToString());
                }
                insGo.transform.SetParent(trans_c_icon_group, false);
                insGo.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(133 + countSpace * (i - 1), 0, 0);
                insGo.GetComponent<RawImage>().texture = iconBGTex;
                insGo.GetComponent<RawImage>().SetNativeSize();
                insGo.transform.GetChild(0).GetComponent<RawImage>().texture = te[i];
                insGo.transform.GetChild(0).GetComponent<RawImage>().SetNativeSize();
            }
        }
    }

    private void CheckModeAndUpdateTex()
    {
        shotMode = UserModel.Ins.GetShotMode();
        if (shotMode == ShotMode.Gif)
        {
            trans_c_GifSwitch.gameObject.SetActive(true);
            preTex = UserModel.Ins.GetPreGifTex();
            //查找gif相框
            if (gifTexRaw == null)
            {
                gifTexRaw = new RawImage[3];
            }
            if (gifRT == null)
            {
                gifRT = new RenderTexture[3];
            }
            for (int i = 1; i <= 3; i++)
            {
                gifTexRaw[i - 1] = GameObject.Find("c_TexRaw" + i.ToString()).GetComponent<RawImage>();
                gifTexRaw[i - 1].texture = preTex[i - 1];
                gifRT[i - 1] = GameObject.Find("c_TexCamera" + i.ToString()).GetComponent<Camera>().targetTexture;
            }
            //查找rendertexture
            trans_c_TexRaw.GetComponent<RawImage>().texture = gifRT[0];
            trans_c_btn_gl.gameObject.SetActive(false);
            gifIndex = 0;
        }
        else
        {
            if (gifTexRaw == null)
            {
                gifTexRaw = new RawImage[3];
            }
            if (gifRT == null)
            {
                gifRT = new RenderTexture[3];
            }
            gifTexRaw[0]= GameObject.Find("c_TexRaw1").GetComponent<RawImage>();
            gifRT[0]= GameObject.Find("c_TexCamera1").GetComponent<Camera>().targetTexture;
            gifTexRaw[0].texture = UserModel.Ins.GainCamRaw();
            trans_c_TexRaw.GetComponent<RawImage>().texture = gifRT[0];
            trans_c_GifSwitch.gameObject.SetActive(false);
        }
    }

    public override void OnInActive()
    {
        trans_c_GifWaite.gameObject.SetActive(false);
        base.OnInActive();
    }
}