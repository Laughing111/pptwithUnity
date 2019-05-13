using RDFW;
using UnityEngine;

public partial class a_P_Index : UIBase
{
    partial void Search4Expeted();
    public PanelAssets_a_P_Index[] panelAssets;
#if UNITY_EDITOR
    public void CreateTargetIns(string[] panelAssets)
    {
        CreatePanelScriptsTools.CreateTargetIns(panelAssets, "a_P_Index");
}
#endif
}
#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(a_P_Index))]
public class a_P_IndexScriptsDevGui : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        a_P_Index panelIns = (a_P_Index)target;
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