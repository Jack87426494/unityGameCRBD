using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoText:MonoBehaviour
{
    private void Start()
    {
        EnumerText enumerText = new EnumerText();
        MonoMgr.Instance.StartCoroutine(enumerText.enumerator());
        MonoMgr.Instance.AddUpdateListener(enumerText.UpdatText);
    }
}
public class EnumerText
{
   public IEnumerator enumerator()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("hahaha");
    }

    public void UpdatText()
    {
        Debug.Log("¸üÐÂÖÐ");
    }
}