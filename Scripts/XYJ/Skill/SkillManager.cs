using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillManager : MonoBehaviour
{
    private static SkillManager instance;

    public static SkillManager Instance => instance;

    //��ʼ����
    public List<SkillData> initialskills;

    public Dictionary<string, Action> dic = new Dictionary<string, Action>();

    
    public SkillChange_Data skillChange_Data = new SkillChange_Data();//���ܸ�������

    private GameObject player;
    public GameObject weaponObj;
    
    public SkillPanel skillPanel;

    public event Action updateDataEvent;//���������¼�

    //���ܵ��߼ӳ�
    private GameObject flashLightObj;//�ֵ�Ͳ
    private GameObject protectionObj;//������
    private GameObject fireAround;//���ܿ�������
    private GameObject GhostObj;//����


    
    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            if(instance!=this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        EventMgr.Instance.AddEventListener("LevelUp", OpenSkill);
        player = GameObject.FindGameObjectWithTag("Player");
        weaponObj = player.GetComponent<BasePlayerController>().weapenObj;
        //skillPanel = FindObjectOfType<SkillPanel>();
        dic.Add("01_01", Skill01_01);
        dic.Add("01_02", Skill01_02);

        dic.Add("02_01", Skill02_01);
        dic.Add("02_02", Skill02_02);
        dic.Add("02_03", Skill02_03);

        dic.Add("03_01", Skill03_01);
        dic.Add("03_02", Skill03_02);
        dic.Add("03_03", Skill03_03);

        dic.Add("04_01", Skill04_01);
        dic.Add("04_02", Skill04_02);

        dic.Add("05_01", Skill05_01);
        dic.Add("05_02_01", Skill05_02_01);
        dic.Add("05_02_02", Skill05_02_02);
        dic.Add("05_03", Skill05_03);

        dic.Add("06_01", Skill06_01);
        dic.Add("06_02", Skill06_02);
        dic.Add("06_03", Skill06_03);

        dic.Add("07_01", Skill07_01);
        dic.Add("07_02_01", Skill07_02_01);
        dic.Add("07_02_02", Skill07_02_02);
        dic.Add("07_03", Skill07_03);

        dic.Add("08_01", Skill08_01);
        dic.Add("08_02", Skill08_02);

        dic.Add("09_01", Skill09_01);
        dic.Add("09_02", Skill09_02);

        dic.Add("10_01", Skill10_01);
        dic.Add("10_02", Skill10_02);
        dic.Add("10_03", Skill10_03);
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    OpenSkill();
        //}
    }
    private void OpenSkill()
    {
        
        weaponObj = FindObjectOfType<BasePlayerController>().weapenObj;

    }
    void Skill01_01()
    {
        flashLightObj=Instantiate(Resources.Load<GameObject>("Items/FlashLight"), player.transform.position,Quaternion.identity,player.transform);
    }

    void Skill01_02()
    {
        UnityEngine.Rendering.Universal.Light2D light2d = flashLightObj.transform.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        light2d.pointLightInnerAngle = 90;
        light2d.pointLightOuterAngle = 130;
        light2d.pointLightInnerRadius = 10;
        light2d.pointLightOuterRadius = 15;
    }

    void Skill02_01()
    {
        //PlayerStatus playerStatus= player.GetComponent<PlayerStatus>();
        skillChange_Data.bulletDamage_Change *= 1.3f;
        skillChange_Data.fireCD_Change *= 1.2f;
        updateDataEvent?.Invoke();
        //weaponObj.GetComponent<HXW_BaseWeapon>().UpdateWeaponData();
    }

    void Skill02_02()
    {
        skillChange_Data.isSplit = true;
    }

    void Skill02_03()
    {
        skillChange_Data.isBackShoot = true;
    }

    void Skill03_01()
    {
        skillChange_Data.updateBulletNumTime_Change *= 0.7f;
        skillChange_Data.fireCD_Change *= 0.8f;
        updateDataEvent?.Invoke();
        //weaponObj.GetComponent<HXW_BaseWeapon>().UpdateWeaponData();
    }
    void Skill03_02()
    {
        skillChange_Data.bulletMaxNum_Change += 2;
        updateDataEvent?.Invoke();
        //weaponObj.GetComponent<HXW_BaseWeapon>().UpdateWeaponData();
    }
    void Skill03_03()
    {
        ResLoadMgr.Instance.LoadAsyn<GameObject>("Items/FireAround", (obj) =>
        {
            
        });
    }

    void Skill04_01()
    {
        Instantiate(Resources.Load<GameObject>("Items/Protective Cover"),
                                  player.transform.position,
                                  Quaternion.identity,
                                  player.transform);
    }

    void Skill04_02()
    {
        skillChange_Data.protectionCD_Change *= 0.5f;
        updateDataEvent?.Invoke();
    }
    void Skill05_01()
    {
        skillChange_Data.bulletMoveSpeed_Change *= 1.3f;
        updateDataEvent?.Invoke();
    }
    void Skill05_02_01()
    {
        skillChange_Data.bulletMoveSpeed_Change *= 1.2f;
        skillChange_Data.penetrateNum++;
        updateDataEvent?.Invoke();
    }
    
    void Skill05_02_02()
    {
        skillChange_Data.bulletMoveSpeed_Change *= 1.1f;
        skillChange_Data.bulletDamage_Change *= 1.15f;
        updateDataEvent?.Invoke();
    }
    void Skill05_03()
    {
        skillChange_Data.bulletMoveSpeed_Change *= 1.15f;
        skillChange_Data._05_03 = true;
        updateDataEvent?.Invoke();
    }
    void Skill06_01()
    {
        GhostObj=Instantiate(Resources.Load<GameObject>("Items/Pet Ghost"), 
                    player.transform.position + new Vector3(1, 0, 0),
                    Quaternion.identity,
                    player.transform);
    }

    void Skill06_02()
    {
        skillChange_Data.ghostFireCD_Change *= 0.8f;
        skillChange_Data.ghostAttackRange_Change *= 1.3f;
        skillChange_Data.ghostBulletDamage_Change *= 1.2f;
        updateDataEvent?.Invoke();
    }

    void Skill06_03()
    {
        skillChange_Data.ghostSkill3 = true;
        updateDataEvent?.Invoke();
    }

    void Skill07_01()
    {
        skillChange_Data.bulletDamage_Change *= 1.3f;
        skillChange_Data.bulletForce_Change *= 1.15f;
        updateDataEvent?.Invoke();
    }

    void Skill07_02_01()
    {
        skillChange_Data._07_02 = true;
    }

    void Skill07_02_02()
    {
        skillChange_Data.bulletDamage_Change *= 1.4f;
        skillChange_Data.fireCD_Change *= 1.15f;
        updateDataEvent?.Invoke();
    }
    void Skill07_03()
    {
        skillChange_Data.bulletDamage_Change *= 1.15f;
        skillChange_Data.penetrateNum++;
        skillChange_Data._07_03 = true;
        updateDataEvent?.Invoke();
    }

    void Skill08_01()
    {
        skillChange_Data.fireCD_Change *= 0.8f;
        updateDataEvent?.Invoke();
    }

    void Skill08_02()
    {
        skillChange_Data.fireCD_Change *= 0.85f;
        skillChange_Data.bulletMaxNum_Change++;
        skillChange_Data.bulletMoveSpeed_Change *= 1.1f;
        updateDataEvent?.Invoke();
    }

    void Skill09_01()
    {
        skillChange_Data._09_01 = true;
    }
    
    void Skill09_02()
    {
        skillChange_Data._09_02 = true;
    }

    void Skill10_01()
    {
        skillChange_Data.bulletSize_Change *= 2;
        skillChange_Data.bulletDamage_Change *= 1.2f;
        updateDataEvent?.Invoke();
    }

    void Skill10_02()
    {
        skillChange_Data.penetrateNum += 2;
        skillChange_Data.fireCD_Change *= 0.9f;
        skillChange_Data.bulletDamage_Change *= 0.8f;
        updateDataEvent?.Invoke();
    }

    void Skill10_03()
    {
        skillChange_Data.bulletMaxNum_Change += 4;
        updateDataEvent?.Invoke();
    }
}
