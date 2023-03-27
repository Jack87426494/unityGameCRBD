using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.ShowView<MVP_MainPanel>();
    }
}
