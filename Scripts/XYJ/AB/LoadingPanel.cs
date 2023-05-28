using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    [Header("������")]
    public Image loadingImg;
    [Header("��ʾ����")]
    public Text txtLoading;
    [Header("��������")]
    public Text txtProcess;

    private void Awake()
    {
       
        //UpdateAB();
    }

    //����ab����Դ
    public void UpdateAB()
    {
        txtLoading.text = "��Դ׼����";
        //���ȸ���ab���Ա��ļ���Ϣ
        ABUpdateMgr.Instance.UpdateABCompareFile((isDownload) =>
        {
            //�������ab���Ա��ļ��ɹ�
            if(isDownload)
            {
                //����ab
                ABUpdateMgr.Instance.UpdateABFile((isDownload) =>
                {
                    if(isDownload)
                    {
                        txtLoading.text = "ȫ����ab���������";
                        print(txtLoading.text);
                        LoadILRuntime();
                    }
                    else
                    {
                        txtLoading.text = "ab������ʧ��";
                        print(txtLoading.text);
                    }
                },
                (nowNum,allNum) =>
                {
                    txtProcess.text = nowNum + "/" + allNum;
                    if (allNum == 0)
                    {
                        loadingImg.rectTransform.sizeDelta = new Vector2(800, 30);
                    }
                    else
                    {
                        loadingImg.rectTransform.sizeDelta = new Vector2((nowNum / allNum) * 800, 30);
                    }
                }
                );
            }
            else
            {
                txtLoading.text = "����Զ�˵�ab���Ա��ļ�ʧ��";
                print(txtLoading.text);
            }
        });
    }
    
    //����ilruntime�������
    private void LoadILRuntime()
    {
        ILRuntimeMgr.GetInstance().StartILRuntime((isWin) =>
        {
            if(isWin)
            {
                txtLoading.text = "����ILRuntime���";
                print(txtLoading.text);
                ////���ظ����
                //gameObject.SetActive(false);

                //ִ��ILRuntime�Ĵ���
                AppDomain appDomain = ILRuntimeMgr.GetInstance().appDomain;
                //IType itype=appDomain.LoadedTypes["HotFix_Project.ILRuntimeMain"];
                //object obj=((ILType)itype).Instantiate();
                appDomain.Invoke("HotFix_Project.ILRuntimeMain", "StartIlRuntime", null,null);
            }
            else
            {
                txtLoading.text = "����ILRuntimeʱ��������";
                print(txtLoading.text);
            }
            
        });
    }
}
