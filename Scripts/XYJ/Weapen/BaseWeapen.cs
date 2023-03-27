using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BaseWeapen : MonoBehaviour
{
    [Header("�����λ��")]
    public Transform firePos;

    public float fireSpeed;//���٣�������

    public float updateBulletNumTime;//����ʱ��

    protected float timer;//��ʱ�������ڼ��㿪����ʱ��

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
