using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePanel : HXW_MapPanel
{
    public static string prefabsPath = "ChoosePanel";

    private Transform characterPanelFather;

    private Button btnSure;
    private Button btnQuit;


    public override void ShowMeFirst()
    {
        characterPanelFather = GetPanel("Characters/Viewport/Content").transform;

        btnSure = GetPanel("btnSure").GetUI<Button>();
        btnSure.onClick.AddListener(()=> 
        {
            Debug.Log("ȷ��");
            HXW_UIManager.GetInstance().HidePanel(ChoosePanel.prefabsPath);
        });
        btnQuit = GetPanel("btnQuit").GetUI<Button>();
        btnQuit.onClick.AddListener(()=> 
        {
            Debug.Log("������ҳ");
            HXW_UIManager.GetInstance().HidePanel(ChoosePanel.prefabsPath);
        });
    }
}
