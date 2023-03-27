using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputText : MonoBehaviour
{
    private void Start()
    {
        InputMgr.Instance.IsOpen(true);
        EventMgr.Instance.AddEventListener<int>("MouseDown", (o) =>
        {
            switch(o)
            {
                case 0:
                    InputMgr.Instance.ChangeKeyCode("MoveUp", KeyCode.UpArrow);
                    Debug.Log("MouseDown");
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
           
        });
        EventMgr.Instance.AddEventListener("MoveUpKeyDown", () =>
        {
            Debug.Log("MoveUpKeyDown");
        });

    }
    private void OnDisable()
    {
        EventMgr.Instance.CanselEventListener<int>("MouseDown", (o) =>
        {
            InputMgr.Instance.ChangeKeyCode("MoveUp", KeyCode.UpArrow);
            Debug.Log("MouseDown");
        });
        EventMgr.Instance.CanselEventListener("MoveUpKeyDown", () =>
        {
            Debug.Log("MoveUpKeyDown");
        });
    }
}
