﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager {

    private CameraManager() { }
    private WebCamDevice[] device;
    private static WebCamTexture te;
    private static CameraManager cameraManager;
    public static CameraManager Instan()
    {
        if (cameraManager == null)
        {
            cameraManager = new CameraManager();
        }
        return cameraManager;
    }
    /// <summary>
    /// 摄像头开启
    /// </summary>
    public WebCamTexture CameraOpen(int width, int height, int fps)
    {
        Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            device = WebCamTexture.devices;
            
            if (device.Length > 0)
            {
                string camName = device[0].name;
                Debug.Log("开启摄像头"+ camName);
                te = new WebCamTexture(camName, width, height, fps);
                te.wrapMode = TextureWrapMode.Repeat;
                te.Play();
                
               
            }
            else
            {
                Debug.Log("没有识别到摄像头");
            }
        } 
        return te;
    }

    /// <summary>
    /// 摄像头开启
    /// </summary>
    public WebCamTexture CameraOpen()
    {
        Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            device = WebCamTexture.devices;
            Debug.Log("开启摄像头");
            if (device.Length > 0)
            {
                string camName = device[0].name;
                te = new WebCamTexture(camName);
                te.wrapMode = TextureWrapMode.Repeat;
                te.Play();
            }
            else
            {
                Debug.Log("没有识别到摄像头");
            }
        }
        return te;
    }
    /// <summary>
    /// 摄像头暂停
    /// </summary>
    public void CameraPause()
    {
        if(te!=null)
        {
            te.Pause();
        } 
    }
    /// <summary>
    /// 摄像头恢复
    /// </summary>
    public void CameraResume()
    {
        if(te!=null)
        {
            te.Play();
        }
    }
    //调整尺寸
    public Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }
        result.Apply();
        return result;
    }

    /// <summary>
    /// 加白边
    /// </summary>
    /// <param name="source">源图片</param>
    /// <param name="outLineWidth">边框边距值</param>
    /// <param name="color">边框颜色</param>
    /// <returns></returns>
    public Texture2D AddOutLine(Texture2D source,int outLineWidth,Color color)
    {
        if (outLineWidth > 0)
        {
            Texture2D result = new Texture2D(source.width + outLineWidth * 2, source.height + outLineWidth * 2, source.format, false);
            for (int i = 0; i < result.height; i++)
            {
                for (int j = 0; j < result.width; j++)
                {
                    if (i >= outLineWidth && j >= outLineWidth && i < result.height - outLineWidth && j < result.width - outLineWidth)
                    {
                        Color c = source.GetPixel(j - outLineWidth, i - outLineWidth);
                        result.SetPixel(j, i, c);
                    }
                    else
                    {
                        result.SetPixel(j, i, color);
                    }
                }
            }
            result.Apply();
            return result;
        }
        else
        {
            return source;
        }
        
    }

    /// <summary>
    /// 摄像机关闭
    /// </summary>
    public void StopCamera()
    {
        te?.Stop();
    }
}
