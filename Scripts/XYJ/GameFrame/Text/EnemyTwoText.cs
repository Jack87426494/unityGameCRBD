using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwoText : MonoBehaviour
{
    private string enemyName = "����";
    private void Start()
    {
        EventMgr.Instance.AddEventListener<EventText>("Wound", Wound);
    }

    public void Wound(EventText obj)
    {
        print(enemyName+ "�ܵ�" + obj.textname + "�Ĺ���");
    }
    private void OnDisable()
    {
        EventMgr.Instance.CanselEventListener<EventText>("Wound", Wound);
    }
}
