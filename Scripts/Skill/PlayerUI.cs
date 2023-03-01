using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    public Slider updateBulletSlider;
    public GameObject bulletImage;
    public GameObject bulletText;
    private float timer;
    public bool isUpdateBullet;
    private GameObject weaponObj;
    private WeaponData weaponData;
    private SkillChange_Data skillChange_Data;
    
    private void Start()
    {
        skillChange_Data = SkillManager.Instance.skillChange_Data;

        bulletImage.SetActive(false);
        bulletText.SetActive(false);
        EventMgr.Instance.AddEventListener("UpdateWeapon", UpdateWeapon);


    }
    private void Update()
    {
        if(isUpdateBullet)
        {
            timer += Time.deltaTime;
            updateBulletSlider.value = timer / (weaponData.updateBulletNumTime * skillChange_Data.updateBulletNumTime_Change);
            if (updateBulletSlider.value >= 1)
            {
                timer = 0;
                updateBulletSlider.gameObject.SetActive(false);
                isUpdateBullet = false;
                bulletImage.SetActive(true);
                bulletText.SetActive(true);
            }
        }
        
    }

    public void UpdateBullet()
    {
        weaponObj = transform.parent.GetComponent<BasePlayerController>().weapenObj;
        weaponData = weaponObj.GetComponent<HXW_BaseWeapon>().weaponData;
        isUpdateBullet = true;
        updateBulletSlider.gameObject.SetActive(true);
        bulletImage.SetActive(false);
        bulletText.SetActive(false);
    }

    private void OnDestroy()
    {
        EventMgr.Instance.CanselEventListener("UpdateWeapon", UpdateWeapon);
    }

    private void UpdateWeapon()
    {
        weaponObj = transform.parent.GetComponent<BasePlayerController>().weapenObj;
        weaponObj.GetComponent<HXW_BaseWeapon>().huanDanEvent += UpdateBullet;
        bulletImage.SetActive(true);
        bulletText.SetActive(true);
    }
}
