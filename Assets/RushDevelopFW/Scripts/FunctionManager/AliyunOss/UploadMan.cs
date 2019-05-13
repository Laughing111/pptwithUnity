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
    public UploadMan(string fileName,Action<string> Method, string localPath=null, string text=null,Texture2D tex=null)
    {
        this.fileName = fileName;
        this.localPath = localPath;
        this.Method = Method;
        this.text = text;
        if(tex!=null)
        {
            texData = tex.EncodeToPNG();
        }
        
    }
   
}
