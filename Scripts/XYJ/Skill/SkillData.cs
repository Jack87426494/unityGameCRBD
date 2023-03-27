using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
[CreateAssetMenu(menuName ="skill",fileName ="skillData")]
[Serializable]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite sp;
    public string skillKey;
    public string Des;
    public List<SkillData> nextSkill;
}
