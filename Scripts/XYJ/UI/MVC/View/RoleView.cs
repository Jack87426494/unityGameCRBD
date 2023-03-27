using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoleView : MonoBehaviour
{
    public Text txtLev;
    public Text txtHp;
    public Text txtAtk;
    public Text txtDef;
    public Text txtCrit;
    public Text txtMiss;
    public Text txtLuck;
    public Button btnClose;
    public Button btnLevUp;

    public void UpdateInfo(PlayerModule playerModule)
    {
        txtLev.text = playerModule.Lev.ToString();
        txtHp.text = playerModule.Hp.ToString();
        txtAtk.text = playerModule.Atk.ToString();
        txtDef.text = playerModule.Def.ToString();
        txtCrit.text = playerModule.Crit.ToString();
        txtMiss.text = playerModule.Miss.ToString();
        txtLuck.text = playerModule.Luck.ToString();
    }
}
