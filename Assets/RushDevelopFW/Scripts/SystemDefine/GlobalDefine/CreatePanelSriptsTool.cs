using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using RDFW;

public class CreatePanelScriptsTools
{

    public static void CreatePanelInit(string panelName)
    {   
        
        string panelInitCode =  "using RDFW;\nusing UnityEngine;\n"
                                +"\npublic partial class "+ panelName + " : UIBase\n{"
                                + "\n    partial void Search4Expeted();"
                                + "\n    public PanelAssets_" + panelName + "[] panelAssets;" +
                                "\n#if UNITY_EDITOR" +
                                "\n    public void CreateTargetIns(string[] panelAssets)"
                                + "\n    {"
                                + "\n        CreatePanelScriptsTools.CreateTargetIns(panelAssets, \""+panelName+"\");\n}" +
                                "\n#endif\n}" +
                                "\n#if UNITY_EDITOR" +
                                "\n[UnityEditor.CustomEditor(typeof(" + panelName + "))]" +
                                "\npublic class "+panelName+"ScriptsDevGui : UnityEditor.Editor\n{" +
                                "\n    public override void OnInspectorGUI()\n    {" +
                                "\n        DrawDefaultInspector();" +
                                "\n        " + panelName + " panelIns = (" + panelName + ")target;" +
                                "\n        if (GUILayout.Button(\"Gain target\"))\n        {" +
                                "\n            int length = panelIns.panelAssets.Length;" +
                                "\n            string[] assetsName = new string[length];" +
                                "\n            for (int i = 0; i < length; i++)\n            {" +
                                "\n                assetsName[i] = panelIns.panelAssets[i].ToString();\n            }" +
                                "\n            panelIns.CreateTargetIns(assetsName);\n        }\n    }\n}\n#endif";
                                                                                                         
        File.WriteAllText(Application.dataPath + "/Scripts/PanelScripts/"+ panelName+"/" + panelName + "_AssetsInit.cs",panelInitCode);
}
public static void CreateTargetIns(string[] panelAssets,string panelName)
    {
        int assetsLength = panelAssets.Length;
        if (assetsLength > 0)
        {   
            string panelInsCodes = "";
            string methodCodes = "    partial void Search4Expeted()"
                + "\n    {\n";

            for (int i = 0; i < assetsLength; i++)
            {
                if (panelAssets[i].ToString() != panelName)
                {
                    panelInsCodes += "\n    private Transform trans_" + panelAssets[i].ToString() + ";";
                    methodCodes += "        trans_" + panelAssets[i].ToString() + "=transform.SearchforChild(\"" + panelAssets[i].ToString() + "\");\n";
                }
            }
            methodCodes += "    }";
            File.WriteAllText(Application.dataPath + "/Scripts/PanelScripts/"+ panelName +"/"+ panelName + "AssetsIns.cs", "using RDFW;\nusing UnityEngine;" +
                "\n\npublic partial class " + panelName + " : UIBase{"
                + panelInsCodes + "\n" + methodCodes
                + "\n}");
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }

   
}
