using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MVE_MainPanel : TheBasePanel
{

    private void Start()
    {
        EventMgr.Instance.AddEventListener<PlayerModule>("UpGrade", UpdateInfo);
        UpdateInfo(PlayerModule.Instance);
    }

    protected override void OnClick(string name)
    {
        base.OnClick(name);
        switch (name)
        {
            case "btnRole":
                UIManager.Instance.ShowView<MVP_RolePanel>();
                break;
        }
    }


    private void UpdateInfo(PlayerModule playerModule)
    {
        GetControl<Text>("txtName").text = playerModule.Name;
        GetControl<Text>("txtLev").text = playerModule.Lev.ToString();
        GetControl<Text>("txtMoney").text = playerModule.Money.ToString();
        GetControl<Text>("txtGem").text = playerModule.Gem.ToString();
        GetControl<Text>("txtPower").text = playerModule.Power.ToString();
    }

    private void OnDestroy()
    {
        EventMgr.Instance.CanselEventListener<PlayerModule>("UpGrade", UpdateInfo);
    }
}
