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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using RDFW;
using UnityEngine.UI;

public class UIDownLoadManager : MonoBehaviour {

    private Transform uiRoot;
    private string jsonText;
    private static UIDownLoadManager loadManager;
    public static UIDownLoadManager Ins
    {
        get
        {
            if (loadManager == null)
            {
               GameObject go=GameObject.Find("UIDownLoadManager");
               if (go != null)
               {
                    if (go.GetComponent<UIDownLoadManager>() != null)
                    {
                        loadManager = go.GetComponent<UIDownLoadManager>();
                    }
                    else
                    {
                        loadManager = go.AddComponent<UIDownLoadManager>();
                    }
                }
                else{
                    go = new GameObject("UIDownLoadManager");
                    loadManager=go.AddComponent<UIDownLoadManager>();
                }
            }
            return loadManager;
        }
    }
    private Dictionary<string, Dictionary<string, string>> panelAssetsPairs;
    private List<string> panelNames;

    public void ReadJson(string json)
    {
        JsonData data =JsonMapper.ToObject(json);
        if (panelAssetsPairs == null)
        {
            panelAssetsPairs = new Dictionary<string, Dictionary<string, string>>();
        }
        if (panelNames == null)
        {
            panelNames = new List<string>();
        }
        for (int i = 0; i < data.Count; i++)
        {
            Dictionary<string, string> nameUrlPairs = new Dictionary<string, string>();
            string panelName = data[i]["name"].ToString();
            Debug.Log(panelName);
            int assetsCount = data[i][panelName].Count;
            Debug.Log(assetsCount);
            for(int j = 0; j < assetsCount; j++)
            {
                string asstsName = data[i][panelName][j]["name"].ToString();
                nameUrlPairs.Add(asstsName, data[i][panelName][j]["url"].ToString());
            }
            panelAssetsPairs.Add(panelName, nameUrlPairs);
            panelNames.Add(panelName);
        }
    }

    public void CheckUrlAndDownLoad()
    {
        List<string> keys = new List<string>();
        if (uiRoot == null)
        {
            uiRoot = GameObject.Find("Canvas").transform;
        }
        int dicCount = panelAssetsPairs.Count;
        if (dicCount> 0)
        {   
            foreach(var values in panelAssetsPairs)
            {
                foreach(KeyValuePair<string,string> url in values.Value)
                {
                    
                }
            }
        }
    }

    private IEnumerator GetTexture(string url,RawImage rawImg,Image img)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.error != null)
        {
            Debug.Log(www.error + ":" + url);
        }
        else
        {

        }
    }



    private IEnumerator GetJsonText(Action<string> sucMethod)
    {
        WWW www = new WWW("file://" + Application.streamingAssetsPath + "/UIAssets.json");
        yield return www;
        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            jsonText= System.Text.Encoding.UTF8.GetString(www.bytes, 3, www.bytes.Length - 3); ;
            Debug.Log(jsonText);
            sucMethod(jsonText);
        }
    }

    public void CheckAndUpdate()
    {
        StartCoroutine(GetJsonText(ReadJson)); 
    }
}
