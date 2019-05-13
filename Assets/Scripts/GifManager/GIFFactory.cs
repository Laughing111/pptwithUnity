using System;
using FFmpeg;
using UnityEngine;
using UnityEngine.UI;


public class GIFFactory : MonoBehaviour, IFFmpegHandler
{

   
    FFmpegHandler defaultHandler = new FFmpegHandler();

    public Action succeed;


    //------------------------------

    void Awake()
    {
        FFmpegParser.Handler = this;
        //debug.text = Application.persistentDataPath;

    }
    public void MakeGif(Action method)
    {
        FFmpegCommands.MakeGif(Application.persistentDataPath + "/png/%d.png", Application.persistentDataPath + "/gif/yb.gif");
        succeed += method;
    }

    public void OnStart()
    {
        defaultHandler.OnStart();
    }

    public void OnProgress(string msg)
    {
        defaultHandler.OnProgress(msg);
    }

    public void OnFailure(string msg)
    {
        defaultHandler.OnFailure(msg);
    }

    public void OnSuccess(string msg)
    {
        defaultHandler.OnSuccess(msg);
    }

    public void OnFinish()
    {
        defaultHandler.OnFinish();
        Debug.Log("");
        succeed?.Invoke();
    }
}
