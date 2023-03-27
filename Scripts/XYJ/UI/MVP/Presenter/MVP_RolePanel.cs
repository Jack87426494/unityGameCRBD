using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MVP_RolePanel : BaseContraller
{
    private MVP_RoleView roleView;
    protected override void Init()
    {
        roleView = GetComponent<MVP_RoleView>();
        UpdateInfo(PlayerModule.Instance);
        roleView.btnClose.onClick.AddListener(() =>
        {
            UIManager.Instance.HideView<MVP_RolePanel>();
            //UIManager.Instance.ShowView<MainPanel>();
        });
        PlayerModule.Instance.AddUpGradeAction(UpdateInfo);
        roleView.btnLevUp.onClick.AddListener(() =>
        {
            PlayerModule.Instance.UpGrade();
        });
    }
    private void OnDestroy()
    {
        PlayerModule.Instance.RemoveUpGradeAction(UpdateInfo);
    }
    public void UpdateInfo(PlayerModule playerModule)
    {
        roleView.txtLev.text = playerModule.Lev.ToString();
        roleView.txtHp.text = playerModule.Hp.ToString();
        roleView.txtAtk.text = playerModule.Atk.ToString();
        roleView.txtDef.text = playerModule.Def.ToString();
        roleView.txtCrit.text = playerModule.Crit.ToString();
        roleView.txtMiss.text = playerModule.Miss.ToString();
        roleView.txtLuck.text = playerModule.Luck.ToString();
    }
}
