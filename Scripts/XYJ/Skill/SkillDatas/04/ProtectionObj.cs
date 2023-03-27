using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionObj : MonoBehaviour
{
    private float originalCD = 10;//³õÊ¼CD
    private float protectCD;//Ä¿Ç°CD
    private SkillChange_Data skillChange_Data;
    private void Start()
    {
        SkillManager.Instance.updateDataEvent += UpdateCD;
        skillChange_Data = SkillManager.Instance.skillChange_Data;
        protectCD = originalCD;
    }
    public void CloseProtect()
    {
        //StartCoroutine("StartCD");
        gameObject.SetActive(false);
        Invoke("StartProtect", protectCD);
    }

    void StartProtect()
    {
        gameObject.SetActive(true);
    }

    private void UpdateCD()
    {
        protectCD = originalCD * skillChange_Data.protectionCD_Change;
    }

    IEnumerator StartCD()
    {

        yield return new WaitForSecondsRealtime(protectCD);

        gameObject.SetActive(true);

    }

    private void OnDestroy()
    {
        SkillManager.Instance.updateDataEvent -= UpdateCD;
    }
}
