﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aliyun.OSS;
using System.Text;
using System.IO;
using Aliyun.OSS.Common;
using System;

public class UploadObject
{

    private OssClient client;
    //字符串上传
    public void PutObjWithStr(UploadMan uploadMan)
    {
        try
        {
            client = new OssClient(AddressConfig.EndPoint, uploadMan.accessKeyId, uploadMan.accessKeySecret,uploadMan.securityToken);
            byte[] data = Encoding.UTF8.GetBytes(uploadMan.text);
            using (Stream stream = new MemoryStream(data))
            {
                if (client != null)
                {
                    //Bucket名称.Endpoint / Object名称
                    client.PutObject(uploadMan.bucket, uploadMan.lineid + "/"+uploadMan.fileName, stream);
                    Debug.Log("字符串上传成功:" + uploadMan.text);
                    string url = string.Format("https://{0}.{1}/{2}", uploadMan.bucket, AddressConfig.EndPoint, uploadMan.lineid + "/" + uploadMan.fileName);
                    Debug.LogFormat("url:" + url);
                    uploadMan.Method?.Invoke(url);
                }
            }
        }
        catch (OssException e)
        {
            Debug.Log("字符串上传错误：" + e);
        }
        catch (System.Exception e)
        {
            Debug.Log("字符串上传错误：" + e);
        }
    }

    //图片上传
    public void PutTexure2DWithStr(UploadMan uploadMan)
    {
        try
        {
            client = new OssClient(AddressConfig.EndPoint, uploadMan.accessKeyId, uploadMan.accessKeySecret, uploadMan.securityToken);
            byte[] data = uploadMan.texData;
            using (Stream stream = new MemoryStream(data))
            {
                if (client != null)
                {
                    //Bucket名称.Endpoint / Object名称
                    client.PutObject(uploadMan.bucket, uploadMan.lineid + "/" + uploadMan.fileName, stream);
                    Debug.Log("图片上传成功:" + uploadMan.fileName);
                    string url = string.Format("https://{0}.{1}/{2}", uploadMan.bucket, AddressConfig.EndPoint, uploadMan.lineid + "/" + uploadMan.fileName);
                    Debug.LogFormat("url:" + url);
                    uploadMan.Method?.Invoke(uploadMan.fileName);
                }
            }
        }
        catch (OssException e)
        {
            Debug.Log("图片上传错误：" + e);
        }
        catch (System.Exception e)
        {
            Debug.Log("图片上传错误：" + e);
        }
    }
    //本地上传
    public void PutObjFromLocal(UploadMan uploadMan)
    {
        try
        {   
           
            Debug.Log(uploadMan.accessKeyId);
            client = new OssClient(AddressConfig.EndPoint, uploadMan.accessKeyId, uploadMan.accessKeySecret, uploadMan.securityToken); 
            client.PutObject(uploadMan.bucket, uploadMan.lineid + "/"+ uploadMan.fileName, uploadMan.localPath);
            Debug.Log("本地文件上传成功:" + uploadMan.localPath);
            string url = string.Format("https://{0}.{1}/{2}", uploadMan.bucket, AddressConfig.EndPoint, uploadMan.lineid + "/" + uploadMan.fileName);
            Debug.LogFormat("url:" + url);
            uploadMan.Method?.Invoke(url);
        }
        catch (OssException e)
        {
            Debug.Log("本地上传报错：" + e.Message);
        }
        catch (System.Exception e)
        {
            Debug.Log("本地上传报错：" + e.Message);
        }
    }

}
