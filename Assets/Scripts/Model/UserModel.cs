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

public enum ShotMode
{
    Jpg,
    Gif
}
public enum State
{
    None,
    Setting,
    Index,
    Shoot,
    Edite,
    Share
}


public class UserModel
{  
    public bool mode_photo { get; set; }
    public bool mode_gif { get; set; }
    public bool mode_pstick { get; set; }
    public bool mode_gstick { get; set; }
    public bool mode_print { get; set; }
    public bool mode_frame { get; set; }
    public int num_pfp { get; set; }
    public int num_pw { get; set; }
    public int num_ph { get; set; }

    private static UserModel userModel;
   private UserModel(ShotMode _shotMode) { shotMode = _shotMode;}
   #region gifRate
    private float gifRate;
   public float GetGifRate()
    {
        return gifRate;
    }
   public void SetGifRate(float rate)
    {
        gifRate = rate;
    }
    #endregion


    public static UserModel Ins
    {
        get
        {
            if(userModel==null)
            {
                userModel = new UserModel(ShotMode.Jpg);
                userModel.preGifTex = new Texture2D[3];
            }
            return userModel;
        }
    }
    /// <summary>
    /// 设置用户状态并开启互动
    /// </summary>
    /// <param name="state"></param>
    public void SetStateAndStart(State state)
    {
        CurrentState = state;
    }

    private string token;
    private ShotMode shotMode;
    private Texture2D camRaw;
    private string[] localRawCam;
    private Texture2D camFX;
    private Texture2D[] preGifTex;
    private string gifLocalPath;
    private string jpgLocalPath;
    private string onLineFileName;
    private string H5Url;

    private Texture2D[] GifTex;

    #region token
    public string GetToken()
    {
        return token;
    }

    public void StoreToken(string _token)
    {
        token = _token;
    }
    #endregion

    private State currentState;
    public State CurrentState
    {
        set
        {
            currentState = value;
            //Call UIManager
            Debug.Log("切换状态为:"+currentState.ToString());
            UIManager.Ins.CheckAndSwitchPanel(currentState);
        }
        get
        {
            return currentState;
        }
    }

    /// <summary>
    /// 存储制作完的GIF连拍图片
    /// </summary>
    /// <param name="te"></param>
    /// <param name="index"></param>
    public void StoreGIFTex(Texture2D te,int index)
    {
        if (GifTex == null)
        {
            GifTex = new Texture2D[3];
        }
        GifTex[index] = te;
    }
    /// <summary>
    /// 获取制作完的GIF连拍图片
    /// </summary>
    /// <returns></returns>
    public Texture2D[] GetGIFTex()
    {   
        return GifTex;
    }


    /// <summary>
    /// 存储原始拍摄画面
    /// </summary>
    /// <param name="camTex"></param>
    public void StoreCamRaw(Texture2D camTex)
    {
        camRaw = camTex;
    }
    /// <summary>
    /// 获取原始拍摄画面
    /// </summary>
    /// <returns></returns>
    public Texture2D GainCamRaw()
    {
        return camRaw;
    }
    /// <summary>
    /// 存储本地照片地址(多个) GIF用
    /// </summary>
    /// <param name="paths"></param>
    public void StoreLocalRaw(string[] paths)
    {
        localRawCam = paths;
    }


    /// <summary>
    /// 存储本地文件地址
    /// </summary>
    /// <param name="path"></param>
    public void StoreLocalPath(string path)
    {
        if (shotMode == ShotMode.Gif)
        {
            gifLocalPath = path;
        }
        else
        {
            jpgLocalPath = path;
        }
    }

    public string GetLocalPath()
    {
        if (shotMode == ShotMode.Gif)
        {
            return gifLocalPath;
        }
        else
        {
            return jpgLocalPath;
        }
    }

    /// <summary>
    /// 存储制作完的JPG
    /// </summary>
    /// <param name="te"></param>
    public void StoreFXJPGTex(Texture2D te)
    {
        camFX = te;
    }
    /// <summary>
    /// 获取制作完的照片
    /// </summary>
    /// <returns></returns>
    public Texture2D GetFXJPGTex()
    {
        return camFX;
    }



    /// <summary>
    /// 存储连拍完的JPG
    /// </summary>
    /// <param name="te"></param>
    public void StoreFXJPGTex(Texture2D te,int index)
    {
        preGifTex[index] = te;
    }

    /// <summary>
    /// 获取连拍完的JPG
    /// </summary>
    /// <returns></returns>
    public Texture2D[] GetPreGifTex()
    {
        return preGifTex;
    }

    

    /// <summary>
    /// 存储并拼接H5地址
    /// </summary>
    /// <param name="fileName"></param>
    public void StoreH5Path(string fileName)
    {
        H5Url = fileName;
    }

    /// <summary>
    /// 设置拍摄模式
    /// </summary>
    /// <param name="mode"></param>
    public void SetShotMode(ShotMode mode)
    {
        shotMode = mode;
    }

    public ShotMode GetShotMode()
    {
        return shotMode;
    }
}
