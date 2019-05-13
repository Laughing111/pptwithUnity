using System.Collections;
using System.Collections.Generic;
using RDFW;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace RDFW
{   
    public class ChangeScriptHeadLine : UnityEditor.AssetModificationProcessor
    {  
        // 创建资源调用
        public static void OnWillCreateAsset(string path)
        {   
            if(HeadInfoTool.isNeedHeadInfo)
            {
                // 只修改C#脚本
                path = path.Replace(".meta", "");
                if (path.EndsWith(".cs"))
                {
                    string allText="";

                    if (File.Exists(Application.dataPath + "/RushDevelopFW/Scripts/SystemDefine/GlobalDefine/HeadInfo.json"))
                    {
                        allText = File.ReadAllText(Application.dataPath + "/RushDevelopFW/Scripts/SystemDefine/GlobalDefine/HeadInfo.json");
                    }
                    allText += File.ReadAllText(path);
                    File.WriteAllText(path, allText);
                    AssetDatabase.Refresh();
                }
            }
        }
    }


    public class ScriptsHeadLineDev : EditorWindow
    {

        [MenuItem("R.D.Tools/SetHeadLineInfo")]
        private static void Go2UI()
        {
            PopHeadLineWindow pw = GetWindow(typeof(PopHeadLineWindow), true, "设置脚本头信息") as PopHeadLineWindow;
            pw.minSize = PopHeadLineWindow.minResolution;
            pw.maxSize = PopHeadLineWindow.minResolution;
            pw.Init();
            pw.Show();
        }
    }

    public class PopHeadLineWindow : EditorWindow
    {
        public static PopHeadLineWindow window;
        public static Vector2 minResolution = new Vector2(450, 450);
        public static Rect middleCenterRect = new Rect(50, 10, 350, 450);
        public GUIStyle labelStyle;
        public static Transform uiroot;
        private JsonRead uiData;
        public static bool isInfoSet = HeadInfoTool.isNeedHeadInfo;
        public static string headlineContent;

        /// <summary>
        /// 头信息
        /// </summary>
        private const string beginInfo = "/***";
        private const string copyRightHead = "\n* Copyright(C) by ";
        private static string copyRight = "";
        private const string rightInfo = "\n* All rights reserved.";
        private const string fileNameInfo = "\n* FileName:     ";
        private static string fileName = "";
        private const string authorInfo = "\n* Author:       ";
        private static string author = "";
        private const string versionInfo = "\n* Version:      ";
        private static string version = "";
        private const string dateInfo = "\n* Date:         ";
        private static string date;
        private const string descInfo = "\n* Description:  ";
        private static string desc = "";
        private const string histInfo = "\n* History:      ";
        private static string hist = "";
        private const string endInfo = "\n***/\n";

        public void Init()
        {
            labelStyle = new GUIStyle();
            labelStyle.normal.textColor = Color.white;
            labelStyle.alignment = TextAnchor.UpperLeft;
            labelStyle.fontSize = 14;
            labelStyle.border = new RectOffset(10, 10, 20, 20);
            if(File.Exists(Application.dataPath + "/RushDevelopFW/Scripts/SystemDefine/GlobalDefine/HeadInfo.json"))
            {
                headlineContent = File.ReadAllText(Application.dataPath + "/RushDevelopFW/Scripts/SystemDefine/GlobalDefine/HeadInfo.json");
            }  
        }
        private void OnGUI()
        {
            GUILayout.BeginArea(middleCenterRect);
            GUILayout.BeginVertical();
            EditorGUILayout.LabelField("R.D.设置脚本头信息", labelStyle, GUILayout.Width(220));
            GUILayout.Space(20);
            //uiAssetsUrl = EditorGUILayout.TextField(new GUIContent("UIAssetsUrl", "UI资源路径"), uiAssetsUrl);
            //输入条目
            copyRight = EditorGUILayout.TextField(new GUIContent("Team"), copyRight);
            fileName = EditorGUILayout.TextField(new GUIContent("FileName"), fileName);
            author = EditorGUILayout.TextField(new GUIContent("Author"), author);
            version = EditorGUILayout.TextField(new GUIContent("Version"), version);
            desc = EditorGUILayout.TextField(new GUIContent("Description"), desc);
            hist = EditorGUILayout.TextField(new GUIContent("History"), hist);
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            if(copyRight!=""||fileName!=""|| author!=""|| version!=""||desc!=""||hist!="")
            {
                headlineContent = beginInfo + copyRightHead + "#" + copyRight + "#" + rightInfo + fileNameInfo + "#" + fileName + "#"
                 + authorInfo + "#" + author + "#" + versionInfo + "#" + version + "#" + dateInfo + "#" + RDTimer.GetTime() + "#" + descInfo + "#" + desc + "#"
                 + histInfo + "#" + hist + "#" + endInfo;
            }
            else
            {
                if (File.Exists(Application.dataPath + "/RushDevelopFW/Scripts/SystemDefine/GlobalDefine/HeadInfo.json"))
                {
                    headlineContent = File.ReadAllText(Application.dataPath + "/RushDevelopFW/Scripts/SystemDefine/GlobalDefine/HeadInfo.json");
                }
            }
            isInfoSet = GUILayout.Toggle(isInfoSet, new GUIContent("是否需要设置头信息"), GUILayout.Width(200));
            if (GUILayout.Button("保存", GUILayout.Width(80)))
            {
                //File.WriteAllText(Application.dataPath + "/RushDevelopFW/Scripts/SystemDefine/GlobalDefine/HeadInfoTool.cs",
                //    "public class HeadInfoTool\n{\npublic static bool isNeedHeadInfo=" + isInfoSet.ToString().ToLower() + ";\npublic const string headInfo = @\"" + headlineContent + "\";\n}");
                File.WriteAllText(Application.dataPath + "/RushDevelopFW/Scripts/SystemDefine/GlobalDefine/HeadInfoTool.cs",
                    "public class HeadInfoTool\n{\npublic static bool isNeedHeadInfo=" + isInfoSet.ToString().ToLower()+ ";\n}");
                File.WriteAllText(Application.dataPath + "/RushDevelopFW/Scripts/SystemDefine/GlobalDefine/HeadInfo.json", headlineContent);
                EditorUtility.DisplayDialog("R.D.UI设置脚本头信息", "保存成功！", "好的");
                AssetDatabase.Refresh();
            }
            GUILayout.Space(20);
            //HeadInfoTool.isNeedHeadInfo = GUILayout.Toggle(isInfoSet, new GUIContent("是否需要设置头信息"), GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.Space(30);
            EditorGUILayout.LabelField(new GUIContent(headlineContent), labelStyle, GUILayout.Width(500));
            GUILayout.Space(10);
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

    }
}

