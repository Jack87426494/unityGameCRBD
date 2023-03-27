using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolePanel : BaseContraller
{
    private RoleView roleView;
    protected override void Init()
    {
        roleView = GetComponent<RoleView>();
        roleView.UpdateInfo(PlayerModule.Instance);
        roleView.btnClose.onClick.AddListener(() =>
        {
            UIManager.Instance.HideView<RolePanel>();
            //UIManager.Instance.ShowView<MainPanel>();
        });
        PlayerModule.Instance.AddUpGradeAction(roleView.UpdateInfo);
        roleView.btnLevUp.onClick.AddListener(() =>
        {
            PlayerModule.Instance.UpGrade();
        });
    }
    private void OnDestroy()
    {
        PlayerModule.Instance.RemoveUpGradeAction(roleView.UpdateInfo);
    }
}