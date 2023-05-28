using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BaseWeapen : MonoBehaviour
{
    [Header("开火的位置")]
    public Transform firePos;

    public float fireSpeed;//射速，开火间隔

    public float updateBulletNumTime;//换弹时间

    protected float timer;//计时器，用于计算开火间隔时间

    public float bulletMaxNum;

    protected float bulletCurrentNum;

    public Transform bulletUIText;
    //public void Attack()
    //{
    //    Shoot();
    //}

    protected virtual void Start()
    {
        bulletCurrentNum = bulletMaxNum;
    }
    protected virtual void Update()
    {
        
    }
    protected virtual void Shoot()
    {
        
    }
}
