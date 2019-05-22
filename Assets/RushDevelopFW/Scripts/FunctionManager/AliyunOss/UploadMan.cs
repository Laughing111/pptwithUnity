using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadMan  {
    public string localPath;
    public string fileName;
    public Action<string> Method;
    public string text;
    public byte[] texData;
    public string securityToken;
    public string bucket;
    public string lineid;
    public string accessKeyId;
    public string accessKeySecret;
    
    public UploadMan(string securityToken,string bucket,string lineid,string accessKeyId,string accessKeySecret,string fileName,Action<string> Method, string localPath=null, string text=null,Texture2D tex=null)
    {
        this.fileName = fileName;
        this.localPath = localPath;
        this.Method = Method;
        this.text = text;
        this.securityToken = securityToken;
        this.bucket = bucket;
        this.lineid = lineid;
        this.accessKeyId = accessKeyId;
        this.accessKeySecret = accessKeySecret;
        if(tex!=null)
        {
            texData = tex.EncodeToPNG();
        }
        
    }
   
}
