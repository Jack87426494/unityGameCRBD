using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowPanelText : MonoBehaviour
{
    private void Start()
    {
        TheUiMgr.Instance.ShowPanel<PanelText>(PosType.Bot, (panel) =>
        {
            TheUiMgr.Instance.AddCustomEvent(panel.GetControl<Button>("beginBtn"), EventTriggerType.PointerEnter, (o) =>
            {
                print("½øÈë");
            });
        });
        
    }
}
