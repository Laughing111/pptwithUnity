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
using System.Collections;
using System.Collections.Generic;
using RDFW;
using UnityEngine;


public class UIManager
{

    private static UIManager uiManager;
    private State oldState;
    public static UIManager Ins {
        get
        {
            if (uiManager == null)
            {
                uiManager = new UIManager();
            }
            return uiManager;
        }
    }

    public void CheckAndSwitchPanel(State targetState)
    {
        
        if (oldState != targetState)
        {
            if (oldState != State.None)
            {
                switch (oldState)
                {
                    case State.Index:
                        MessageCenter.ClosePanel(PanelAssets_a_P_Index.a_P_Index.ToString());
                        break;
                    case State.Shoot:
                        MessageCenter.ClosePanel(PanelAssets_b_P_Shot.b_P_Shot.ToString());
                        break;
                    case State.Edite:
                        MessageCenter.ClosePanel(PanelAssets_c_P_Edite.c_P_Edite.ToString());
                        break;
                    case State.Share:
                        MessageCenter.ClosePanel(PanelAssets_d_P_Share.d_P_Share.ToString());
                        break;
                    case State.Setting:
                        MessageCenter.ClosePanel(PanelAssets_e_Setting.e_Setting.ToString());
                        break;
                    default:
                        break;
                }
            }
            

            switch (targetState)
            {
                case State.Index:
                    MessageCenter.OpenPanel(PanelAssets_a_P_Index.a_P_Index.ToString());
                    break;
                case State.Shoot:
                    MessageCenter.OpenPanel(PanelAssets_b_P_Shot.b_P_Shot.ToString());
                    break;
                case State.Edite:
                    MessageCenter.OpenPanel(PanelAssets_c_P_Edite.c_P_Edite.ToString());
                    break;
                case State.Share:
                    MessageCenter.OpenPanel(PanelAssets_d_P_Share.d_P_Share.ToString());
                    break;
                case State.Setting:
                    MessageCenter.OpenPanel(PanelAssets_e_Setting.e_Setting.ToString());
                    break;
                default:
                    break;
            }
            oldState = targetState;
        }
        
    }

   
}
