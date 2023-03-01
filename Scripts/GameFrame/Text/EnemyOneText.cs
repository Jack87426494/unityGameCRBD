using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOneText : MonoBehaviour
{
    private string enemyName = "½©Κ¬";

    private void Start()
    {
        EventMgr.Instance.AddEventListener<EventText>("Wound", Wound);
    }

    public void Wound(EventText obj)
    {
        print(enemyName + "Κάµ½"+obj.textname + "µΔΉ¥»χ");
    }

    private void OnDisable()
    {
        EventMgr.Instance.CanselEventListener<EventText>("Wound", Wound);
    }
}
