using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using RDFW;

namespace RDFW
{
    [System.Serializable]
    public class JsonRead
    {
        public List<ImageJson> data;
    }
    [System.Serializable]
    public class ImageJson
    {
        public string name;
        public float x;
        public float y;
        public float width;
        public float height;
        public string father;
    }


    public class AutoUgui : EditorWindow
    {

        [MenuItem("R.D.Tools/UIPageLayOut")]
        private static void Go2UI()
        {
            //Transform canvas=GameObject.Find("Canvas").transform;
            //GameObject UI = new GameObject("UI");
            //UI.AddComponent<RawImage>();
            //UI.transform.SetParent(canvas, false);
            PopWindow pw = GetWindow(typeof(PopWindow), true, "UI自动布局") as PopWindow;
            pw.minSize = PopWindow.minResolution;
            pw.maxSize = PopWindow.minResolution;
            pw.Init();
            pw.Show();
        }
    }

    public class PopWindow : EditorWindow
    {
        public static PopWindow window;
        public static Vector2 minResolution = new Vector2(500, 200);
        public static Rect middleCenterRect = new Rect(50, 10, 300, 200);
        public GUIStyle labelStyle;
        public static Transform uiroot;
        public static string uiAssetsUrl = "/UIAssets";
        private JsonRead uiData;

        public void Init()
        {
            labelStyle = new GUIStyle();
            labelStyle.normal.textColor = Color.white;
            labelStyle.alignment = TextAnchor.UpperLeft;
            labelStyle.fontSize = 14;
            labelStyle.border = new RectOffset(10, 10, 20, 20);
        }
        private void OnGUI()
        {
            GUILayout.BeginArea(middleCenterRect);
            GUILayout.BeginVertical();
            EditorGUILayout.LabelField("根据psd文件自动布局UI", labelStyle, GUILayout.Width(220));
            GUILayout.Space(20);
            uiroot = EditorGUILayout.ObjectField(new GUIContent("UIRoot", "选择UI根节点"), uiroot, typeof(Transform), true) as Transform;
            uiAssetsUrl = EditorGUILayout.TextField(new GUIContent("UIAssetsUrl", "UI资源路径"), uiAssetsUrl);
            GUILayout.Space(20);
            EditorGUILayout.LabelField("点击下面的按钮进行自动布局", labelStyle, GUILayout.Width(220));
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("加载UI信息", GUILayout.Width(80)))
            {
                ReadJson();
            }
            GUILayout.Space(20);
            if (GUILayout.Button("自动布局", GUILayout.Width(80)))
            {
                SetLayout();
                SetLayout();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        private void ReadJson()
        {
            string jsonTxt = System.IO.File.ReadAllText(Application.dataPath + uiAssetsUrl + "/JsonData.txt");
            uiData = JsonUtility.FromJson<JsonRead>(jsonTxt);
            EditorUtility.DisplayDialog("R.D.UI自动布局", "读取UI配置信息完毕！", "好的");
        }

        private void SetLayout()
        {
            List<ImageJson> uiList = uiData.data;
            if (uiroot != null)
            {
                Transform[] father = new Transform[3];
                for (int i = uiList.Count - 1; i >= 0; i--)
                {
                    father[0] = null;
                    father[1] = null;
                    father[2] = null;
                    if (uiList[i].father.Contains(":"))
                    {
                        //有祖父级别的物体
                        string[] fatherGroup = uiList[i].father.Split(':');
                        int fatherLength = fatherGroup.Length;
                        for (int index = fatherLength - 1; index >= 0; index--)
                        {
                            string fatherUIName = fatherGroup[index].StringModify("#", "");
                            if (fatherUIName.Contains(".psd") || fatherUIName.Contains("Adobe"))
                            {
                                continue;
                            }
                            father[index] = uiroot.SearchforChild(fatherUIName);
                            if (father[index] == null)
                            {
                                if (index == fatherLength - 1)
                                {
                                    father[index] = new GameObject(fatherUIName).AddComponent<RectTransform>().transform;
                                    father[index].transform.SetParent(uiroot, false);
                                    father[index].transform.localPosition = Vector3.zero;
                                }
                                else
                                {
                                    father[index] = new GameObject(fatherUIName).AddComponent<RectTransform>().transform;

                                    if (father[index + 1] == null)
                                    {
                                        father[index + 1] = uiroot;
                                    }
                                    father[index].transform.SetParent(father[index + 1], false);
                                    father[index].transform.localPosition = Vector3.zero;
                                }
                            }
                        }
                    }
                    else
                    {
                        string fatherUIName = uiList[i].father.StringModify("#", "");
                        //只有父物体
                        father[0] = uiroot.SearchforChild(fatherUIName);
                        if (father[0] == null)
                        {
                            father[0] = new GameObject(fatherUIName).AddComponent<RectTransform>().transform;
                            father[0].transform.SetParent(uiroot, false);
                            father[0].transform.localPosition = Vector3.zero;
                        }

                    }
                    string name = uiList[i].name;
                    string uiName = name.StringModify("#", "");
                    Transform UIT = null;
                    GameObject UI = null;
                    if ((uiName.Contains("btn") || uiName.Contains("Btn")) && (uiName.Contains("_pressed") || uiName.Contains("_Pressed")))
                    {
                        string uiBtnName = uiName.StringModify("_pressed", "");
                        uiBtnName = uiBtnName.StringModify("_Pressed", "");
                        Debug.Log(uiBtnName);
                        UIT = uiroot.SearchforChild(uiBtnName);
                        if (UIT != null)
                        {
                            Sprite tex = AssetDatabase.LoadAssetAtPath<Sprite>("Assets" + uiAssetsUrl + "/" + name + ".png");
                            SpriteState _spriteState = new SpriteState();
                            _spriteState.pressedSprite = tex;
                            UIT.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
                            UIT.GetComponent<Button>().spriteState = _spriteState;
                            Debug.Log(name);

                        }
                        continue;
                    }
                    else
                    {
                        UIT = uiroot.SearchforChild(uiName);
                    }
                    if (UIT == null)
                    {
                        UI = new GameObject(uiName);
                        if (father[0] == null)
                        {
                            father[0] = uiroot;
                        }
                        UI.transform.SetParent(father[0], false);
                        if (UI.GetComponent<RectTransform>() == null)
                        {
                            UI.AddComponent<RectTransform>();
                        }
                        UI.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(uiList[i].x, -uiList[i].y, 0);
                        UI.GetComponent<RectTransform>().sizeDelta = new Vector2(uiList[i].width, uiList[i].height);
                    }
                    else
                    {
                        UI = UIT.gameObject;
                    }
                    if ((uiName.Contains("btn") || uiName.Contains("Btn")) && !uiName.Contains("pressed") && !uiName.Contains("Pressed"))
                    {
                        Image uiImg = UI.GetComponent<Image>() ? UI.GetComponent<Image>() : UI.AddComponent<Image>();
                        Button button = UI.GetComponent<Button>() ? UI.GetComponent<Button>() : UI.AddComponent<Button>();
                        Sprite tex = AssetDatabase.LoadAssetAtPath<Sprite>("Assets" + uiAssetsUrl + "/" + name + ".png");
                        uiImg.sprite = tex;
                    }
                    else if (!uiName.Contains("btn") && !uiName.Contains("Btn"))
                    {
                        RawImage uiRawImg = UI.GetComponent<RawImage>() ? UI.GetComponent<RawImage>() : UI.AddComponent<RawImage>();
                        Texture tex = AssetDatabase.LoadAssetAtPath<Texture>("Assets" + uiAssetsUrl + "/" + name + ".png");
                        uiRawImg.texture = tex;
                    }
                }
                EditorUtility.DisplayDialog("R.D.UI自动布局", "布局完毕！", "知道了！");
            }
            else
            {
                EditorUtility.DisplayDialog("R.D.UI自动布局", "Error:请指定UI根节点", "知道了！");
            }
        }
    }
}


