using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwoText : MonoBehaviour
{
    private string enemyName = "÷¼÷Ã";
    private void Start()
    {
        EventMgr.Instance.AddEventListener<EventText>("Wound", Wound);
    }

    public void Wound(EventText obj)
    {
        print(enemyName+ "ÊÜµ½" + obj.textname + "µÄ¹¥»÷");
    }
    private void OnDisable()
    {
        EventMgr.Instance.CanselEventListener<EventText>("Wound", Wound);
    }
}
