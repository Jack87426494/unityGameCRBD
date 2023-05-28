using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class SkillChange_Data 
{
    public float updateBulletNumTime_Change = 1;//换弹加成
    public float bulletMaxNum_Change = 0;//子弹最大数量加成
    public float fireCD_Change = 1;//开火间隔加成
    public float bulletDamage_Change = 1;//子弹伤害加成
    public float bulletMoveSpeed_Change = 1;//子弹移速加成
    public float bulletForce_Change = 1;//子弹击退加成
    public float bulletSize_Change = 1;

    public float ghostFireCD_Change = 1;//幽灵攻速加成
    public float ghostAttackRange_Change = 1;//幽灵攻击范围加成
    public float ghostBulletDamage_Change = 1;//幽灵子弹伤害加成
    public bool ghostSkill3;//幽灵技能第三阶段

    public float protectionCD_Change = 1;//保护罩CD加成

    public bool _05_03;//05_03技能阶段
    public bool _07_02;//07_02技能阶段
    public bool _07_03;//07_03技能阶段
    public bool _09_01;//09_01冰冻技能阶段
    public bool _09_02;//09_02技能阶段

    public bool isSplit;//子弹是否分裂
    public bool isBackShoot;//子弹是否背后射击
    public int penetrateNum = 1;//子弹穿透数
}
