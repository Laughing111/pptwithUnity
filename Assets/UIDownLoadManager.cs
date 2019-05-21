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

public class UIDownLoadManager : MonoBehaviour
{

    private Transform uiRoot;
    private string jsonText;
    private static UIDownLoadManager loadManager;

    public List<string> iconUrlList;
    public Texture2D[] iconTex;

    private Action<string> AfterTokenAndUpdate;
    [HideInInspector]
    public Text debugText;

    public static UIDownLoadManager Ins
    {
        get
        {
            if (loadManager == null)
            {
                GameObject go = GameObject.Find("UIDownLoadManager");
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
                else
                {
                    go = new GameObject("UIDownLoadManager");
                    loadManager = go.AddComponent<UIDownLoadManager>();
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
    /// 面板名字+位置地址
    /// </summary>
    private Dictionary<string, Vector2> panelPosPairs;

    /// <summary>
    /// 面板名字+实际材质
    /// </summary>
    private Dictionary<string, Texture2D> panelTexturePairs;

    /// <summary>
    /// 面板名字+位置长宽
    /// </summary>
    private Dictionary<string, Vector4> motionArea;

    /// <summary>
    /// 停止下载
    /// </summary>
    public void StopDownLoadManager()
    {
        StopAllCoroutines();
    }

    private void ReadJson(string json)
    {
        if (uiRoot == null)
        {
            uiRoot = GameObject.Find("Canvas").transform;
        }
        JsonData data = JsonMapper.ToObject(json);
        if (panelAssetsPairs == null)
        {
            panelAssetsPairs = new Dictionary<string, string>();
        }
        if (iconUrlList == null)
        {
            iconUrlList = new List<string>();
        }
        if (panelPosPairs == null)
        {
            panelPosPairs = new Dictionary<string, Vector2>();
        }
        for (int i = 0; i < data.Count; i++)
        {
            string panelName = data[i]["name"].ToString();
            if (panelName != "g_p")
            {
                Debug.Log(panelName);
                int assetsCount = data[i][panelName].Count;
                Debug.Log(assetsCount);
                for (int j = 0; j < assetsCount; j++)
                {
                    string asstsName = data[i][panelName][j]["name"].ToString();
                    //检查是否icon
                    if (asstsName.Split('_')[1] == "url")
                    {
                        //说明是icon
                        //为iconUrl添加
                        iconUrlList.Add(data[i][panelName][j]["url"].ToString());
                    }
                    else
                    {
                        if (data[i][panelName][j]["url"] != null)
                        {
                            panelAssetsPairs.Add(data[i][panelName][j]["name"].ToString(), data[i][panelName][j]["url"].ToString());
                        }
                        else   //没有url 即为画面调整参数
                        {
                            if (motionArea == null)
                            {
                                motionArea = new Dictionary<string, Vector4>();
                            }
                            if (!motionArea.ContainsKey(asstsName))
                            {
                                float Ux,Uy,Uw,Uh;
                                float.TryParse(data[i][panelName][j]["x"].ToString(),out Ux);
                                float.TryParse(data[i][panelName][j]["y"].ToString(), out Uy);
                                float.TryParse(data[i][panelName][j]["width"].ToString(), out Uw);
                                float.TryParse(data[i][panelName][j]["height"].ToString(), out Uh);
                                motionArea.Add(asstsName, new Vector4(Ux, Uy, Uw, Uh));
                            }
                        }
                    }
                    //todo位置参数
                    //配置位置参数
                    JsonData posX = data[i][panelName][j]["x"];
                    JsonData posY = data[i][panelName][j]["y"];

                    if (asstsName.Contains("_active") || asstsName.Contains("_normal"))
                    {
                        Transform temp = uiRoot.SearchforChild(asstsName);
                        if (temp == null)
                        {
                            asstsName = asstsName.StringModify("_active", "");
                            asstsName = asstsName.StringModify("_normal", "");
                        }
                    }
                    if (posX != null && posY != null)
                    {
                        if (!panelPosPairs.ContainsKey(asstsName))
                        {
                            float aX;
                            float.TryParse(posX.ToString(), out aX);
                            float aY;
                            float.TryParse(posY.ToString(), out aY);
                            panelPosPairs.Add(asstsName, new Vector2(aX, aY));
                        }
                    }
                }
            }
            else  //全局配置参数
            {
                Debug.Log(panelName);
                int assetsCount = data[i][panelName].Count;
                Debug.Log(assetsCount);
                for (int j = 0; j < assetsCount; j++)
                {
                    string asstsName = data[i][panelName][j]["name"].ToString();
                    JsonData modeData = data[i][panelName][j]["mode"];
                    bool mode = false;
                    if (modeData != null)
                    {
                        if (modeData.ToString() == "1")
                        {
                            mode = true;
                        }
                    }
                    JsonData numData;
                    switch (asstsName)
                    {
                        case "g_mode_photo":
                            UserModel.Ins.mode_photo = mode;
                            break;
                        case "g_mode_gif":
                            UserModel.Ins.mode_gif = mode;
                            break;
                        case "g_mode_pstick":
                            UserModel.Ins.mode_pstick = mode;
                            break;
                        case "g_mode_gstick":
                            UserModel.Ins.mode_gstick = mode;
                            break;
                        case "g_mode_print":
                            UserModel.Ins.mode_print = mode;
                            break;
                        case "g_mode_frame":
                            UserModel.Ins.mode_frame = mode;
                            break;
                        case "g_num_pfp":
                            numData = data[i][panelName][j]["num"];
                            if (numData != null)
                            {
                                int num;
                                int.TryParse(numData.ToString(), out num);
                                UserModel.Ins.num_pfp = num;
                            }
                            break;
                        case "g_num_ph":
                             numData = data[i][panelName][j]["num"];
                            if (numData != null)
                            {
                                int num;
                                int.TryParse(numData.ToString(), out num);
                                UserModel.Ins.num_ph = num;
                            }
                            break;
                        case "g_num_pw":
                            numData = data[i][panelName][j]["num"];
                            if (numData != null)
                            {
                                int num;
                                int.TryParse(numData.ToString(), out num);
                                UserModel.Ins.num_pw = num;
                            }
                            break;
                    }
                }

            }
        }
        Debug.Log(iconUrlList.Count);
        CheckUrlAndDownLoad();
    }

    private void CheckUrlAndDownLoad()
    {
        //添加材质字典，
        if (panelTexturePairs == null)
        {
            panelTexturePairs = new Dictionary<string, Texture2D>();
        }
        foreach (var e in panelAssetsPairs)
        {
            panelTexturePairs.Add(e.Key, null);
        }
        StartCoroutine(GetTexture(panelTexturePairs, panelAssetsPairs, UpdateTextureWithName));
    }

    private void UpdateTextureWithName(Dictionary<string, Texture2D> nameTexPairs, Action endMethod)
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
            if (astName.Contains("_normal") || astName.Contains("_active"))
            {
                if (uiRoot.SearchforChild(astName) == null)
                {
                    if (astName.Contains("_normal"))
                    {

                        astName = astName.StringModify("_normal", "");
                        Transform ast = uiRoot.SearchforChild(astName);
                        if (ast != null)
                        {
                            Image img = ast.GetComponent<Image>();
                            if (img != null)
                            {
                                img.sprite = Sprite.Create(kv.Value, new Rect(0, 0, kv.Value.width, kv.Value.height), Vector2.zero);
                                img.SetNativeSize();

                                //设置素材的位置
                                if (panelPosPairs.ContainsKey(astName))
                                {
                                    img.rectTransform.anchoredPosition3D = new Vector3(panelPosPairs[astName].x - Screen.width * 0.5f + img.rectTransform.sizeDelta.x * 0.5f, Screen.height * 0.5f - panelPosPairs[astName].y - img.rectTransform.sizeDelta.y * 0.5f, 0);
                                }
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
                                spriteState.pressedSprite = Sprite.Create(kv.Value, new Rect(0, 0, kv.Value.width, kv.Value.height), Vector2.zero);
                                btn.spriteState = spriteState;
                            }
                        }
                    }
                }
                else
                {
                    //rawImg
                    Transform ast = uiRoot.SearchforChild(astName);
                    if (ast != null)
                    {
                        RawImage rimg = ast.GetComponent<RawImage>();
                        if (rimg != null)
                        {
                            rimg.texture = kv.Value;
                            rimg.SetNativeSize();

                            //设置素材的位置
                            if (panelPosPairs.ContainsKey(astName))
                            {
                                rimg.rectTransform.anchoredPosition3D = new Vector3(panelPosPairs[astName].x - Screen.width * 0.5f + rimg.rectTransform.sizeDelta.x * 0.5f, Screen.height * 0.5f - panelPosPairs[astName].y - rimg.rectTransform.sizeDelta.y * 0.5f, 0);
                            }
                        }
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

                        //设置素材的位置
                        if (panelPosPairs.ContainsKey(astName))
                        {
                            rimg.rectTransform.anchoredPosition3D = new Vector3(panelPosPairs[astName].x - Screen.width * 0.5f + rimg.rectTransform.sizeDelta.x * 0.5f, Screen.height * 0.5f - panelPosPairs[astName].y - rimg.rectTransform.sizeDelta.y * 0.5f, 0);
                        }
                    }
                }
            }

            //调整画面位置和大小
            int areaCount = motionArea.Count;
            foreach(var kev in motionArea)
            {
                Transform t=null;
                switch (kev.Key)
                {
                    case "b_area_lv":
                        t = uiRoot.SearchforChild(PanelAssets_b_P_Shot.b_Mask.ToString());
                        break;
                    case "b_area_result":
                        t = uiRoot.SearchforChild(PanelAssets_c_P_Edite.c_mask_picture.ToString());
                        break;
                    case "d_area_result":
                        t = uiRoot.SearchforChild(PanelAssets_d_P_Share.d_picture.ToString());
                        break;
                    case "d_area_qrcode":
                        t = uiRoot.SearchforChild(PanelAssets_d_P_Share.d_img_qrcode.ToString());
                        break;
                }
                if (t != null)
                {
                    t.GetComponent<RectTransform>().sizeDelta = new Vector2(kev.Value.z, kev.Value.w);
                    t.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(kev.Value.x - Screen.width * 0.5f + t.GetComponent<RectTransform>().sizeDelta.x * 0.5f, Screen.height * 0.5f - kev.Value.y - t.GetComponent<RectTransform>().sizeDelta.y * 0.5f, 0);
                }
            }
        }

        //加载icon
        MessageCenter.GetMessage(PanelAssets_c_P_Edite.c_P_Edite.ToString(), new MessageAddress("UpdateIcon", iconTex));

        if (debugText != null)
        {
            debugText.text = "加载成功...";
        }
        endMethod?.Invoke();
    }

    private IEnumerator GetTexture(Dictionary<string, Texture2D> nameTexPair, Dictionary<string, string> nameUrlPairs, Action<Dictionary<string, Texture2D>, Action> updateTexture)
    {

        if (debugText != null)
        {
            debugText.text = "正在同步UI素材...";
        }

        List<string> nameList = new List<string>();
        foreach (var k in nameUrlPairs.Keys)
        {
            nameList.Add(k);
        }

        int DownLoadTimes = nameList.Count;
        WWW www;
        while (DownLoadTimes >= 1)
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
                if (nameTexPair.ContainsKey(nameList[DownLoadTimes]))
                {
                    try
                    {
                        nameTexPair[assetsName] = www.texture;
                    }
                    catch
                    {
                        Debug.LogError(assetsName);
                    }
                }
            }
        }

        //下载icon
        int iconCount = iconUrlList.Count;
        if (iconCount > 0)
        {
            iconTex = new Texture2D[iconCount];
        }
        int iconIndex = 0;
        while (iconIndex < iconCount)
        {
            string iconUrl = iconUrlList[iconIndex];
            www = new WWW(iconUrl);
            yield return www;
            if (www.error != null)
            {
                Debug.Log(www.error + ":" + iconIndex);
            }
            else
            {
                iconTex[iconIndex] = www.texture;
            }
            iconIndex++;
        }

        if (nameTexPair.Count > 0)
        {
            updateTexture(nameTexPair, EndEvent);
        }
    }

    private void EndEvent()
    {
        MessageCenter.GetMessage(PanelAssets_e_Setting.e_Setting.ToString(), new MessageAddress("ChangeState", null));
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
            jsonText = System.Text.Encoding.UTF8.GetString(www.bytes, 3, www.bytes.Length - 3); ;
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
        if (uuid == null)
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
            Debug.Log(tokenData.status + ":" + tokenData.data.data_json);
            if (tokenData.status == 200)
            {
                UserModel.Ins.StoreToken(tokenData.data.token);
                AfterTokenAndUpdate?.Invoke(uuid);
            }
            yield return null;
        }
        //debugText.text = "uuid失效，请重新输入。";
        //MessageCenter.GetMessage(PanelAssets_e_Setting.e_Setting.ToString(), new MessageAddress("PopInput", null));

        //test
        AfterTokenAndUpdate?.Invoke(uuid);

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
