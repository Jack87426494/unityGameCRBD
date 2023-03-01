using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITipPanel : HXW_MapPanel
{
    public static string prefabsPath = "UITipPanel";

    private void OnEnable()
    {
        Invoke("Close", 60f);
    }
  
    private void Close()
    {
        HXW_UIManager.GetInstance().HidePanel(UITipPanel.prefabsPath);
    }
}
