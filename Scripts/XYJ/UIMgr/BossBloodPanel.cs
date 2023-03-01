using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBloodPanel : HXW_MapPanel
{
    public static string prefabsPath = "BossBloodPanel";
    private Image bloodImage;
    private Text text;
    public override void ShowMeFirst()
    {
        text = GetPanel("Text").GetUI<Text>();
        bloodImage = GetPanel("Back/Image").GetUI<Image>();
    }

    /// <summary>
    /// 设置血条的名字显示
    /// </summary>
    /// <param name="targetName">目标名字</param>
    public void SetName(string targetName)
    {
        text.text = targetName;
    }
    /// <summary>
    /// 刷新血条
    /// </summary>
    /// <param name="allBlood">总生命值</param>
    /// <param name="currentBlood">当前生命值</param>
    public void RefreshBlood(float allBlood,float currentBlood)
    {
        print(bloodImage.fillAmount);
        bloodImage.fillAmount = currentBlood / allBlood;
    }
}
