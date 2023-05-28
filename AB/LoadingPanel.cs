using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    [Header("进度条")]
    public Image loadingImg;
    [Header("提示文字")]
    public Text txtLoading;
    [Header("进度文字")]
    public Text txtProcess;

    private void Awake()
    {
       
        //UpdateAB();
    }

    //更新ab包资源
    public void UpdateAB()
    {
        txtLoading.text = "资源准备中";
        //首先更新ab包对比文件信息
        ABUpdateMgr.Instance.UpdateABCompareFile((isDownload) =>
        {
            //如果下载ab包对比文件成功
            if(isDownload)
            {
                //更新ab
                ABUpdateMgr.Instance.UpdateABFile((isDownload) =>
                {
                    if(isDownload)
                    {
                        txtLoading.text = "全部的ab包更新完毕";
                        print(txtLoading.text);
                        LoadILRuntime();
                    }
                    else
                    {
                        txtLoading.text = "ab包下载失败";
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
                txtLoading.text = "更新远端的ab包对比文件失败";
                print(txtLoading.text);
            }
        });
    }
    
    //加载ilruntime更新相关
    private void LoadILRuntime()
    {
        ILRuntimeMgr.GetInstance().StartILRuntime((isWin) =>
        {
            if(isWin)
            {
                txtLoading.text = "加载ILRuntime完毕";
                print(txtLoading.text);
                ////隐藏该面板
                //gameObject.SetActive(false);

                //执行ILRuntime的代码
                AppDomain appDomain = ILRuntimeMgr.GetInstance().appDomain;
                //IType itype=appDomain.LoadedTypes["HotFix_Project.ILRuntimeMain"];
                //object obj=((ILType)itype).Instantiate();
                appDomain.Invoke("HotFix_Project.ILRuntimeMain", "StartIlRuntime", null,null);
            }
            else
            {
                txtLoading.text = "加载ILRuntime时出现问题";
                print(txtLoading.text);
            }
            
        });
    }
}
