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
using System.IO;

public class UIDownLoadManager : MonoBehaviour {

    private Transform uiRoot;
    private string jsonText;
    private static UIDownLoadManager loadManager;

    private Action<string> AfterTokenAndUpdate;
    [HideInInspector]
    public Text debugText;
    
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
                loadManager.AfterTokenAndUpdate += loadManager.UpdateJsonAndTexture2D;
            }
            return loadManager;
        }
    }
    
    /// <summary>
    /// 面板名字+url地址
    /// </summary>
    private Dictionary<string, string> panelAssetsPairs;

    /// <summary>
    /// 面板名字+实际材质
    /// </summary>
    private Dictionary<string, Texture2D> panelTexturePairs;

    private void ReadJson(string json)
    {
        JsonData data =JsonMapper.ToObject(json);
        if (panelAssetsPairs == null)
        {
            panelAssetsPairs = new Dictionary<string,string>();
        }
        for (int i = 0; i < data.Count; i++)
        {   
            string panelName = data[i]["name"].ToString();
            Debug.Log(panelName);
            int assetsCount = data[i][panelName].Count;
            Debug.Log(assetsCount);
            for(int j = 0; j < assetsCount; j++)
            {
                string asstsName = data[i][panelName][j]["name"].ToString();
                panelAssetsPairs.Add(data[i][panelName][j]["name"].ToString(), data[i][panelName][j]["url"].ToString());
            }
        }

        CheckUrlAndDownLoad();
    }

    private void CheckUrlAndDownLoad()
    {
        //添加材质字典，
        if (panelTexturePairs == null)
        {
            panelTexturePairs = new Dictionary<string, Texture2D>();
        }
        
        foreach(var e in panelAssetsPairs)
        {
            panelTexturePairs.Add(e.Key, null);
        }

        StartCoroutine(GetTexture(panelTexturePairs, panelAssetsPairs, UpdateTextureWithName));
    }

    private void UpdateTextureWithName(Dictionary<string,Texture2D> nameTexPairs,Action endMethod)
    {
        if (uiRoot == null)
        {
            uiRoot = GameObject.Find("Canvas").transform;
        }
        if (debugText != null)
        {
            debugText.text = "正在配置UI素材...";
        }
        foreach (var kv in nameTexPairs)
        {
            string astName = kv.Key;
            //按钮正常
            if (astName.Contains("_normal"))
            {
                astName = astName.StringModify("_normal", "");
                Transform ast = uiRoot.SearchforChild(astName);
                if (ast != null)
                {
                    Image img = ast.GetComponent<Image>();
                    if (img != null)
                    {
                        img.sprite = Sprite.Create(kv.Value, new Rect(0, 0, kv.Value.width, kv.Value.height),Vector2.zero);
                        img.SetNativeSize();
                    }
                }
            }
            //按钮激活
            else if (astName.Contains("_active"))
            {
                astName = astName.StringModify("_active", "");
                Transform ast = uiRoot.SearchforChild(astName);
                if (ast != null)
                {
                    Button btn = ast.GetComponent<Button>();
                    if (btn != null)
                    {

                        btn.transition = Selectable.Transition.SpriteSwap;
                        SpriteState spriteState = new SpriteState();
                        spriteState.pressedSprite= Sprite.Create(kv.Value, new Rect(0, 0, kv.Value.width, kv.Value.height), Vector2.zero);
                        btn.spriteState = spriteState;
                    }
                }
            }
            //rawImg
            else
            {
                Transform ast = uiRoot.SearchforChild(astName);
                if (ast != null)
                {
                    RawImage rimg = ast.GetComponent<RawImage>();
                    if (rimg != null)
                    {
                        rimg.texture = kv.Value;
                        rimg.SetNativeSize();
                    }
                }
            }
        }

        if (debugText != null)
        {
            debugText.text = "加载成功...";
        }
        endMethod?.Invoke();
    }

    private IEnumerator GetTexture(Dictionary<string,Texture2D> nameTexPair,Dictionary<string,string> nameUrlPairs,Action<Dictionary<string,Texture2D>,Action> updateTexture)
    {
        
        if(debugText!=null)
        {
            debugText.text = "正在同步UI素材...";
        }
        
        List<string> nameList = new List<string>();
        foreach(var k in nameUrlPairs.Keys)
        {
            nameList.Add(k);
        }

        int DownLoadTimes = nameList.Count;
        WWW www;
        while (DownLoadTimes>=1)
        {
            DownLoadTimes -= 1;
            string assetsName = nameList[DownLoadTimes];
            string url = nameUrlPairs[assetsName];
            www = new WWW(url);
            yield return www;
            if (www.error != null)
            {
                Debug.Log(www.error + ":" + assetsName);
            }
            else
            {
                if(nameTexPair.ContainsKey(nameList[DownLoadTimes]))
                {
                    try
                    {
                        nameTexPair[assetsName]=www.texture;
                    }
                    catch
                    {
                        Debug.LogError(assetsName);
                    }
                    
                } 
            }
        }
        if(nameTexPair.Count>0)
        {
            updateTexture(nameTexPair, EndEvent);
        }
    }

    private void EndEvent()
    {
        UserModel.Ins.CurrentState = State.Index;
    }



    private IEnumerator GetJsonText(Action<string> sucMethod)
    {
        if (debugText != null)
        {
            debugText.text = "正在获取配置文件...";
        }
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

        CheckUUID();
        //StartCoroutine(GetJsonText(ReadJson)); 
        
    }

    public void UpdateJsonAndTexture2D(string _uuid)
    {
        
        StartCoroutine(GetJsonText(ReadJson));
    }

    

    public void SetUUID(string _uuid)
    {
        uuid = _uuid;
    }

    private string uuid;
    /// <summary>
    /// 获取本地uuid,并http-get验证token
    /// </summary>
    private void CheckUUID()
    {   
        if(uuid==null)
        {
            //读取本地的uuid
            if (File.Exists(Application.streamingAssetsPath + "/uuid.json"))
            {
                uuid = File.ReadAllText(Application.streamingAssetsPath + "/uuid.json");
            }
        }
        //开始获取token
        StartCoroutine(GetToken());

    }

    private IEnumerator GetToken()
    {
        WWW www = new WWW("http://tweb.btech.cc/api/v1/items/uuid/" + uuid);
        yield return www;
        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            string jsonText = www.text;
            JsonToken tokenData = JsonMapper.ToObject<JsonToken>(jsonText);
            Debug.Log(tokenData.status+":"+tokenData.data.data_json);
            if (tokenData.status == 200)
            {
                UserModel.Ins.StoreToken(tokenData.data.token);
                AfterTokenAndUpdate?.Invoke(uuid);
            }
            yield return null;
        }

        
            debugText.text = "uuid失效，请重新输入。";
            MessageCenter.GetMessage(PanelAssets_e_Setting.e_Setting.ToString(), new MessageAddress("PopInput", null));
          
    }

}

[Serializable]
public class JsonToken
{
    public TokenData data;
    public string errmsg;
    public int status;
}

[Serializable]
public class TokenData
{
    public string data_json;
    public string online;
    public string token;
}
