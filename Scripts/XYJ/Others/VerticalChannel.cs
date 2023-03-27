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
    [Header("ͨ��ͨ������")]
    public ChannelGoTo channelGoTo;
    [Header("ͨ�������ҵķ�Χ")]
    public float rad=3;
    [Header("������ʾ")]
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
                        //�����л�

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

                        //�����л�
                        
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
                        
                        //�����л�
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
                        
                        //�����л�
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
