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
using RDFW;
using UnityEngine;

public partial class c_P_Edite : UIBase
{
    partial void Search4Expeted();
    public PanelAssets_c_P_Edite[] panelAssets;
#if UNITY_EDITOR
    public void CreateTargetIns(string[] panelAssets)
    {
        CreatePanelScriptsTools.CreateTargetIns(panelAssets, "c_P_Edite");
}
#endif
}
#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(c_P_Edite))]
public class c_P_EditeScriptsDevGui : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        c_P_Edite panelIns = (c_P_Edite)target;
        if (GUILayout.Button("Gain target"))
        {
            int length = panelIns.panelAssets.Length;
            string[] assetsName = new string[length];
            for (int i = 0; i < length; i++)
            {
                assetsName[i] = panelIns.panelAssets[i].ToString();
            }
            panelIns.CreateTargetIns(assetsName);
        }
    }
}
#endif