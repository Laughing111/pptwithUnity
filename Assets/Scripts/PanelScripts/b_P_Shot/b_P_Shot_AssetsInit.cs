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

public partial class b_P_Shot : UIBase
{
    partial void Search4Expeted();
    public PanelAssets_b_P_Shot[] panelAssets;
#if UNITY_EDITOR
    public void CreateTargetIns(string[] panelAssets)
    {
        CreatePanelScriptsTools.CreateTargetIns(panelAssets, "b_P_Shot");
}
#endif
}
#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(b_P_Shot))]
public class b_P_ShotScriptsDevGui : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        b_P_Shot panelIns = (b_P_Shot)target;
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