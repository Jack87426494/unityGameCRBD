using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : BaseContraller
{
    private MainView mainView;
    protected override void Init()
    {
        mainView = GetComponent<MainView>();
        mainView.UpdateInfo(PlayerModule.Instance);
        mainView.btnRole.onClick.AddListener(() =>
        {
            //UIManager.Instance.HideView<MainPanel>();
            UIManager.Instance.ShowView<RolePanel>();
        });
        PlayerModule.Instance.AddUpGradeAction(mainView.UpdateInfo);
    }
    private void OnDestroy()
    {
        PlayerModule.Instance.RemoveUpGradeAction(mainView.UpdateInfo);
    }
}
