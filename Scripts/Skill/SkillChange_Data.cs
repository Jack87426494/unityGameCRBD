using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class SkillChange_Data 
{
    public float updateBulletNumTime_Change = 1;//�����ӳ�
    public float bulletMaxNum_Change = 0;//�ӵ���������ӳ�
    public float fireCD_Change = 1;//�������ӳ�
    public float bulletDamage_Change = 1;//�ӵ��˺��ӳ�
    public float bulletMoveSpeed_Change = 1;//�ӵ����ټӳ�
    public float bulletForce_Change = 1;//�ӵ����˼ӳ�
    public float bulletSize_Change = 1;

    public float ghostFireCD_Change = 1;//���鹥�ټӳ�
    public float ghostAttackRange_Change = 1;//���鹥����Χ�ӳ�
    public float ghostBulletDamage_Change = 1;//�����ӵ��˺��ӳ�
    public bool ghostSkill3;//���鼼�ܵ����׶�

    public float protectionCD_Change = 1;//������CD�ӳ�

    public bool _05_03;//05_03���ܽ׶�
    public bool _07_02;//07_02���ܽ׶�
    public bool _07_03;//07_03���ܽ׶�
    public bool _09_01;//09_01�������ܽ׶�
    public bool _09_02;//09_02���ܽ׶�

    public bool isSplit;//�ӵ��Ƿ����
    public bool isBackShoot;//�ӵ��Ƿ񱳺����
    public int penetrateNum = 1;//�ӵ���͸��
}
