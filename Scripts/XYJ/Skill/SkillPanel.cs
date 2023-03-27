using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillPanel : HXW_MapPanel
{
    public static string prefabsPath = "SkillPanel";
    public GameObject[] slots;
    public List<SkillData> activatedSkills;//����ļ���
    public Text textDes;
    public Button sureButton;
    public SkillData selectedSkill;//��ǰ����б�ѡ��ļ���
    private List<SkillData> usedSkills=new List<SkillData>();//�Ѿ�ѡ����ļ���
    private void Start()
    {
        //foreach (SkillData skillData in SkillManager.Instance.initialskills)
        //{
        //    activatedSkills.Add(Instantiate(skillData));
        //}
        //�����������ĵ�sure��ť
        sureButton.onClick.AddListener(() =>
        {
            MusicMgr.Instance.PlaySound("Music/Audio/Click4");
            if (selectedSkill == null) return;
            if (selectedSkill!=null)
            {
                SkillManager.Instance.dic[selectedSkill.skillKey]();
                if (selectedSkill.nextSkill != null)
                {
                    foreach(SkillData skill in selectedSkill.nextSkill)
                    {
                        if(!activatedSkills.Contains(skill)&&!usedSkills.Contains(skill))
                        {
                            activatedSkills.Add(skill);
                        }
                        
                    }
                    
                }
                activatedSkills.Remove(selectedSkill);
                usedSkills.Add(selectedSkill);
                selectedSkill = null;
            }


            HXW_UIManager.GetInstance().HidePanel(prefabsPath);
            
        });
        UpdateSkillPanel();
    }

    //��skillpanel�����Ҫ�������ݵĺ���
    public void UpdateSkillPanel()
    {
        
        textDes.text = "";
        for(int i=0;i<slots.Length;i++)
        {
            slots[i].GetComponent<SkillSlot>().UpdateSlot(null);
        }
        for(int i=0;i<slots.Length;i++)
        {
            if(activatedSkills.Count-1-i<0)
            {
                break;
            }
            SkillData temp;
            int index = Random.Range(0, activatedSkills.Count-i);
            slots[i].GetComponent<SkillSlot>().UpdateSlot(activatedSkills[index]);
            temp = activatedSkills[index];
            activatedSkills[index] = activatedSkills[activatedSkills.Count - 1-i];
            activatedSkills[activatedSkills.Count - 1-i] = temp;

            
            if (activatedSkills[index]== activatedSkills[activatedSkills.Count - 1 - i])
            {
                //print("666");
            }
        }
        
    }
    private void OnDisable()
    {
        Time.timeScale = 1;
    }


}
