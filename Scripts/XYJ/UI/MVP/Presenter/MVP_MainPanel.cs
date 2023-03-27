using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MVP_MainPanel : BaseContraller
{
    private MVP_MainView mainView;
    protected override void Init()
    {
        mainView = GetComponent<MVP_MainView>();
        UpdateInfo(PlayerModule.Instance);
        mainView.btnRole.onClick.AddListener(() =>
        {
            //UIManager.Instance.HideView<MainPanel>();
            UIManager.Instance.ShowView<MVP_RolePanel>();
        });
        PlayerModule.Instance.AddUpGradeAction(UpdateInfo);
    }

    private void UpdateInfo(PlayerModule playerModule)
    {
        mainView.txtName.text = playerModule.Name;
        mainView.txtLev.text = playerModule.Lev.ToString();
        mainView.txtMoney.text = playerModule.Money.ToString();
        mainView.txtGem.text = playerModule.Gem.ToString();
        mainView.txtPower.text = playerModule.Power.ToString();
    }

    private void OnDestroy()
    {
        PlayerModule.Instance.RemoveUpGradeAction(UpdateInfo);
    }
}
