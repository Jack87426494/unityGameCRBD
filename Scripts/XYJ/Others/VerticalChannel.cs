using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public enum ChannelGoTo
{
    GameScene,  
    GameSceneBack,
    StartRoom,
    LaboratoryBuilding
}

public class VerticalChannel : MonoBehaviour
{
    [Header("通道通向哪里")]
    public ChannelGoTo channelGoTo;
    [Header("通道检测玩家的范围")]
    public float rad=3;
    [Header("交互提示")]
    public GameObject tipObj;

    private Collider2D col2D;

    protected virtual void Update()
    {
        col2D = Physics2D.OverlapCircle(transform.position, rad, 1 << LayerMask.NameToLayer("Player"));
        if (col2D != null && col2D.CompareTag("Player"))
        {
            tipObj.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {

                switch (channelGoTo)
                {
                    case ChannelGoTo.GameScene:
                        ObjectPoolMgr.Instance.ClearObjectPool();
                        tipObj.SetActive(false);
                        HXW_UIManager.GetInstance().HidePanel(UITipPanel.prefabsPath);
                        //场景切换

                        HXW_UIManager.GetInstance().ShowPanelAsync<LoadIngPanel>(LoadIngPanel.prefabsPath, PanelLayer.Max, (LoadIngPanel) =>
                        {
                            LoadIngPanel.LoadFirst();
                        });

                        ScenesMgr.Instance.LoadSceneAsyn("GameScene", () =>
                        {
                            BasePlayerController.Instance.transform.position = new Vector3(0, 5, 0);
                        });
                        
                        MusicMgr.Instance.PlayBkAudioSource("Music/battle");
                        break;
                    case ChannelGoTo.GameSceneBack:
                        ObjectPoolMgr.Instance.ClearObjectPool();
                        tipObj.SetActive(false);

                        //场景切换
                        
                        HXW_UIManager.GetInstance().ShowPanelAsync<LoadIngPanel>(LoadIngPanel.prefabsPath, PanelLayer.Max, (LoadIngPanel) =>
                        {
                            LoadIngPanel.LoadFirst();
                        });
                        
                        ScenesMgr.Instance.LoadSceneAsyn("GameScene", () =>
                        {
                            
                        });

                        BasePlayerController.Instance.transform.position = new Vector3(78, 10, 0);
                        
                        break;

                    case ChannelGoTo.StartRoom:
                        ObjectPoolMgr.Instance.ClearObjectPool();
                        tipObj.SetActive(false);
                        
                        //场景切换
                        HXW_UIManager.GetInstance().ShowPanelAsync<LoadIngPanel>(LoadIngPanel.prefabsPath, PanelLayer.Max, (LoadIngPanel) =>
                        {
                            LoadIngPanel.LoadFirst();
                        });
                        ScenesMgr.Instance.LoadSceneAsyn("StartRoom", () =>
                        {
                            BasePlayerController.Instance.transform.position = new Vector3(-0.5f, -26, 0);
                        });
                        
                        break;
                    case ChannelGoTo.LaboratoryBuilding:
                       

                        tipObj.SetActive(false);
                        
                        //场景切换
                        HXW_UIManager.GetInstance().ShowPanelAsync<LoadIngPanel>(LoadIngPanel.prefabsPath, PanelLayer.Max, (LoadIngPanel) =>
                        {
                            LoadIngPanel.LoadFirst();
                        });
                        ScenesMgr.Instance.LoadSceneAsyn("LaboratoryBuilding", () =>
                        {
                            
                        });
                        BasePlayerController.Instance.transform.position = new Vector3(-15, 11, 0);
                        break;
                    
                }
              
            }
               
        }
        else
        {
            tipObj.SetActive(false);
        }
    }

}
