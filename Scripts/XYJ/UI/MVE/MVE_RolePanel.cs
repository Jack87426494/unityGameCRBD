using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MVE_RolePanel : TheBasePanel
{
    private void Start()
    {
        UpdateInfo(PlayerModule.Instance);
        EventMgr.Instance.AddEventListener<PlayerModule>("UpGrade", UpdateInfo);
    }

    protected override void OnClick(string name)
    {
        base.OnClick(name);
        switch (name)
        {
            case "btnClose":
                UIManager.Instance.HideView<MVP_RolePanel>();
                break;
            case "btnLevUp":
                PlayerModule.Instance.UpGrade();
                break;
        }
    }

    private void UpdateInfo(PlayerModule playerModule)
    {
        GetControl<Text>("txtLev").text = playerModule.Lev.ToString();
        GetControl<Text>("txtHp").text = playerModule.Hp.ToString();
        GetControl<Text>("txtAtk").text = playerModule.Atk.ToString();
        GetControl<Text>("txtDef").text = playerModule.Def.ToString();
        GetControl<Text>("txtCrit").text = playerModule.Crit.ToString();
        GetControl<Text>("txtMiss").text = playerModule.Miss.ToString();
        GetControl<Text>("txtLuck").text = playerModule.Luck.ToString();
    }
    private void OnDestroy()
    {
        EventMgr.Instance.CanselEventListener<PlayerModule>("UpGrade", UpdateInfo);
    }
}
