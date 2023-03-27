using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainView : MonoBehaviour
{
    public Text txtName;
    public Text txtLev;
    public Text txtMoney;
    public Text txtGem;
    public Text txtPower;
    public Button btnSkill;
    public Button btnRole;

    public void UpdateInfo(PlayerModule playerModule)
    {
        txtName.text = playerModule.Name;
        txtLev.text = playerModule.Lev.ToString();
        txtMoney.text = playerModule.Money.ToString();
        txtGem.text = playerModule.Gem.ToString();
        txtPower.text = playerModule.Power.ToString();
    }
}
