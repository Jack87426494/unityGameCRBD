using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RedDevil : BaseWeapen
{
    
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        bulletUIText.GetComponent<Text>().text = bulletCurrentNum.ToString();
        base.Update();
        timer += Time.deltaTime;
        if(bulletCurrentNum==0&&timer>updateBulletNumTime)
        {
            bulletCurrentNum = bulletMaxNum;
            //bulletUIText.GetComponent<Text>().text = bulletCurrentNum.ToString();
        }

        if (Input.GetMouseButton(0) && timer > fireSpeed&&bulletCurrentNum>0)
        {
            
            timer = 0;
            Shoot();
            bulletCurrentNum--;
            //bulletUIText.GetComponent<Text>().text = bulletCurrentNum.ToString();
        }
    }
    protected override void Shoot()
    {
        base.Shoot();
        Instantiate(Resources.Load<GameObject>("Weapon/Bullet"), firePos.transform.position,firePos.transform.rotation);
        //Instantiate(Resources.Load<GameObject>(PlayerBullet.prefabsPath), firePos.transform.position, firePos.transform.rotation);
    }
}
