using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private GameObject canvasObj;
    private void Start()
    {
        print(Application.persistentDataPath);
        canvasObj =GameObject.Find("Canvas");
        ABMgr.GetInstance().LoadResAsync<GameObject>("panel", "LoadingPanel", (panel) =>
        {
            panel.GetComponent<LoadingPanel>().UpdateAB();
            panel.transform.SetParent(canvasObj.transform,false);
        });
    }
}
