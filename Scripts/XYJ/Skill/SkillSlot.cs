using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SkillSlot : MonoBehaviour,IPointerDownHandler
{
    public SkillData skillData;
    public Image image;
    public SkillPanel skillPanel;

    private void Start()
    {
        skillPanel = FindObjectOfType<SkillPanel>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        MusicMgr.Instance.PlaySound("Music/Audio/Click3");
        if (skillData!=null)
        {
            skillPanel.selectedSkill = skillData;
            skillPanel.textDes.text = skillData.Des;
        }
        
    }

    public void UpdateSlot(SkillData data)
    {
        skillData = data;

        if (data != null)
        {
            image.sprite = data.sp;
        }
        else
        {
            image.sprite = null;
        }
    }
}
