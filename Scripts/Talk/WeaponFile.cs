using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFile : BaseFile
{
    private bool isLoadWeapon;

    public GameObject gun;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (FindObjectOfType<BasePlayerController>().weapenObj != null) return;

        if (col2D != null && col2D.CompareTag("Player")&&!isLoadWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gun.SetActive(false);
                EventMgr.Instance.EventTrigger("LoadWeapon", "Weapon/HXW_RedDevil");
                EventMgr.Instance.EventTrigger("UpdateWeapon");
                isLoadWeapon = true;
            }
            
        }

            
    }
}
