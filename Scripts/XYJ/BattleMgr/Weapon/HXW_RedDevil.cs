using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class HXW_RedDevil : HXW_BaseWeapon
{
    //子弹Text
    private Text textBullet;
    protected override void Start()
    {
        base.Start();
        
        //得到序号0的武器数据
        weaponData = GameDataMgr.Instance.GetWeaponData(0);
        bulletCurrentNum = bulletMaxNum;
        UpdateWeaponData();
        textBullet = bulletUIText.GetComponent<Text>();
        textBullet.text = bulletCurrentNum.ToString();
        
    }
    protected override void Update()
    {
        base.Update();
        //bulletUIText.GetComponent<Text>().text = bulletCurrentNum.ToString();
        time += Time.deltaTime;
        if (bulletCurrentNum == 0 && time > updateBulletNumTime)
        {
            bulletCurrentNum = bulletMaxNum;
            textBullet.text = bulletCurrentNum.ToString();
        }

        if (Input.GetMouseButton(0) && time > fireCD && bulletCurrentNum > 0)
        {
            time = 0;
            Shoot(shooter);
            bulletCurrentNum--;
            textBullet.text = bulletCurrentNum.ToString();
            if (bulletCurrentNum == 0)
            {
                MusicMgr.Instance.PlaySound("Music/Audio/Reload");
                DoHuanDanEvent();
            }

        }
    }
    public override void Shoot(IShootAble shooter)
    {
        base.Shoot(shooter);

        Vector3 euler = firePos.rotation.eulerAngles;
        //Instantiate(Resources.Load<GameObject>("Weapon/Bullet"), firePos.transform.position, firePos.transform.rotation);
        //Instantiate(Resources.Load<GameObject>(PlayerBullet.prefabsPath), firePos.transform.position, Quaternion.Euler(euler + new Vector3(0, 0, 90)));
        //Instantiate(Resources.Load<GameObject>(PlayerBullet.prefabsPath), firePos.transform.position, Quaternion.Euler(euler + new Vector3(0, 0, 30)));
        
    }
}
