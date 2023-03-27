using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAround : MonoBehaviour
{
    private GameObject weaponObj;
    private HXW_BaseWeapon HXW_BaseWeapon;
    private bool isFire;
    private void Awake()
    {
        weaponObj = FindObjectOfType<BasePlayerController>().weapenObj;
        HXW_BaseWeapon = weaponObj.GetComponent<HXW_BaseWeapon>();
    }
    private void Start()
    {
        HXW_BaseWeapon.huanDanEvent += fireAround;
    }
    void fireAround()
    {
        Vector3 angel = new Vector3(0, 0, 0);
        for (int i = 0; i < 6; i++)
        {
            ObjectPoolMgr.Instance.GetObject("Weapon/PlayerBullet", (obj) =>
            {
                MusicMgr.Instance.PlaySound("Music/Audio/Fire1");
                PlayerBullet bullet = obj.GetComponent<PlayerBullet>();
                bullet.Fire(weaponObj.transform, weaponObj.transform.right, angel);
                angel += new Vector3(0, 0, 60);
            });
        }
    }
}
