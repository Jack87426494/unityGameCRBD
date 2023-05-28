using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Up : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
    private void Start()
    {
        EventMgr.Instance.AddEventListener("LevelUp", LevelUp);
        
    }

    protected void OpenSkill()
    {
        Time.timeScale = 0;
        HXW_UIManager.GetInstance().ShowPanelAsync<SkillPanel>(SkillPanel.prefabsPath, PanelLayer.Max, FirstLoad, (SkillPanel) =>
        {
            SkillPanel.gameObject.SetActive(true);
            SkillPanel.UpdateSkillPanel();
        });
    }

    void FirstLoad(SkillPanel skillPanel)
    {
        skillPanel.activatedSkills.Clear();
        foreach (SkillData skillData in SkillManager.Instance.initialskills)
        {
            skillPanel.activatedSkills.Add(Instantiate(skillData));
        }
    }

    private void LevelUp()
    {
        MusicMgr.Instance.PlaySound("Music/Audio/Up");
        animator.Play("Up");
    }

    private void OnDestroy()
    {
        EventMgr.Instance.CanselEventListener("LevelUp", LevelUp);
    }
}
