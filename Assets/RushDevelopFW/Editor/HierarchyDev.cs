using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using RDFW;

public class HierarchyDev : MonoBehaviour {
    [InitializeOnLoadMethod]
    static void StartInitializeOnLoadMethod()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        if (Event.current.shift)
        {
            if (Event.current != null && selectionRect.Contains(Event.current.mousePosition)
             && Event.current.button == 1 && Event.current.type == EventType.MouseUp)
            {
                GameObject selectedGameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
                //这里可以判断selectedGameObject的条件
                if (selectedGameObject != null)
                {
                    //Debug.Log(selectedGameObject.name);
                    Vector2 mousePosition = Event.current.mousePosition;

                    EditorUtility.DisplayPopupMenu(new Rect(mousePosition.x, mousePosition.y, 0, 0), "R.D.Tools/RushScripts", null);
                    Event.current.Use();
                }
            }
        }
    }

    private static string panelName;
    private static string code;
    [MenuItem("R.D.Tools/RushScripts/Serialized2Sys")]
    static void GetAndSerUIPath()
    {    
        Transform PanelTrans = Selection.activeTransform;
        panelName = PanelTrans.name;
        code = "public enum PanelAssets_" + panelName+" {\n"+panelName+",";
        Debug.Log(panelName);
        Search(PanelTrans);
        code = code.Remove(code.Length - 1, 1);
        code += "\n}";
        string url = Application.dataPath + "/RushDevelopFW/Scripts/SysTemDefine/PanelAssets/PanelAssets_"+panelName+".cs";
        Debug.Log(code);
        if(!File.Exists(Application.dataPath + "/RushDevelopFW/Scripts/SysTemDefine/PanelAssets"))
        {
            Directory.CreateDirectory(Application.dataPath + "/RushDevelopFW/Scripts/SysTemDefine/PanelAssets");
        }
        File.WriteAllText(url,code);
        AssetDatabase.Refresh();
    }
    [MenuItem("R.D.Tools/RushScripts/CreatePanelScripts")]
    static void CreatePanelScripts()
    {
        string headName="";
        if (PopHeadLineWindow.isInfoSet)
        {
            if(File.Exists(Application.dataPath + "/RushDevelopFW/Scripts/SystemDefine/GlobalDefine/HeadInfo.json"))
            {
                headName = File.ReadAllText(Application.dataPath + "/RushDevelopFW/Scripts/SystemDefine/GlobalDefine/HeadInfo.json");
            }
            
        }
        else
        {
            headName = "";
        }
        string scripts = "\nusing System.Collections;\nusing System.Collections.Generic;\nusing UnityEngine;\nusing UnityEngine.EventSystems;\nusing RDFW;\n\npublic partial class " +
            Selection.activeGameObject.name + " : UIBase {\n    public void Awake()\n    {\n        PanelInit();\n    }\n    public override void PanelInit()\n    {\n        Search4Expeted();\n        base.PanelInit();\n    }\n    public override void OnActive()\n    {\n        base.OnActive();\n    }\n    public override void OnInActive()\n    {\n        base.OnInActive();\n    }\n}";

        if (!File.Exists(Application.dataPath + "/Scripts/PanelScripts/" + Selection.activeTransform.name))
        {
            //Debug.Log("新建");
            Directory.CreateDirectory(Application.dataPath + "/Scripts/PanelScripts/" + Selection.activeTransform.name);
        }
        //Debug.Log(headName);
        File.WriteAllText(Application.dataPath + "/Scripts/PanelScripts/"+ Selection.activeTransform.name + "/" + Selection.activeTransform.name + ".cs",scripts);
        CreatePanelScriptsTools.CreatePanelInit(Selection.activeTransform.name);
        AssetDatabase.Refresh();
    }

    private static void Search(Transform father)
    {
        int length = father.childCount;
        if (length > 0)
        {
            for (int i = 0; i < length; i++)
            {
                Transform child = father.GetChild(i);
                //Debug.Log(child.name);
                code +="\n"+child.name + ",";
                Search(father.GetChild(i));
            }
        }
    }
}
