using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadIngPanel : HXW_MapPanel
{
    public static string prefabsPath = "LoadIngPanel";//避免后期文件路径改变，那时，只需要改变这里的路径即可

    private Slider slider;

    private bool isLoad;
    //public override void ShowMeFirst()
    //{
    //    slider = GetPanel("Control/Slider").GetUI<Slider>();

    //    //EventMgr.Instance.AddEventListener<float>("Load/Slider", (progress) =>
    //    //{
    //    //    slider.value = progress;
    //    //    if (progress>0.99f)
    //    //    {
    //    //        slider.value = 1f;
    //    //        gameObject.SetActive(false);
    //    //    }
    //    //});
    //}
    //private void Update()
    //{
    //    slider.value+=Time.deltaTime;
    //    if(slider.value>=1)
    //    {
    //        HXW_UIManager.Instance.HidePanel(prefabsPath);
    //    }
    //}
    
    public void LoadFirst()
    {
        
        slider = GetPanel("Slider").GetUI<Slider>();
        EventMgr.Instance.AddEventListener<float>("Load", (progress) =>
        {
            if (progress >=0.9f&&!isLoad)
            {
                gameObject.SetActive(true);
                StartCoroutine("StartLoad");
            }
        });
    }

    
    IEnumerator StartLoad()
    {
        isLoad = true;
        
        slider.value = 0.3f;
        yield return new WaitForSeconds(0.3f);
        slider.value = 0.6f;
        yield return new WaitForSeconds(0.3f);
        slider.value = 1;
        yield return new WaitForSeconds(0.3f);

        HXW_UIManager.Instance.HidePanel(prefabsPath);
        slider.value = 0;
        isLoad = false;
    }
    //void LoadScene(float process)
    //{
    //    slider.value = process;
    //    if(slider.value==1)
    //    {
    //        HXW_UIManager.Instance.HidePanel(prefabsPath);
    //    }
    //}

}
