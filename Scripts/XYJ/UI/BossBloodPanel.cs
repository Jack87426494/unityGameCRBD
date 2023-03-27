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
    /// ����Ѫ����������ʾ
    /// </summary>
    /// <param name="targetName">Ŀ������</param>
    public void SetName(string targetName)
    {
        text.text = targetName;
    }
    /// <summary>
    /// ˢ��Ѫ��
    /// </summary>
    /// <param name="allBlood">������ֵ</param>
    /// <param name="currentBlood">��ǰ����ֵ</param>
    public void RefreshBlood(float allBlood,float currentBlood)
    {
        print(bloodImage.fillAmount);
        bloodImage.fillAmount = currentBlood / allBlood;
    }
}
