using System.Collections;
using UnityEngine;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime;
using System.IO;
using UnityEngine.Events;
using System.Threading;
using ILRuntime.Mono.Cecil.Pdb;

public class ILRuntimeMgr :SingletonAutoMono<ILRuntimeMgr>
{
    public AppDomain appDomain;
    //dll�ڴ���
    private MemoryStream dllStream;
    //pdb�ڴ���
    private MemoryStream pdbStream;

    //�Ƿ����ILRuntime
    private bool isOpenILRuntime;

    //�ǹ���������ģʽ
    private bool isDebugMode=false;

    public void StartILRuntime(UnityAction<bool> callback)
    {
        if(isOpenILRuntime)
        {
            return;
        }

        try
        {
            //�Զ�ѡ��Ĵ���ģʽ
            appDomain = new AppDomain(ILRuntimeJITFlags.JITOnDemand);

            //����dll�ļ�
            ABMgr.GetInstance().LoadResAsync<TextAsset>("dll", "HotFix_Project.dll.txt", (dll) =>
            {
                //����pdb�ļ�
                ABMgr.GetInstance().LoadResAsync<TextAsset>("dll", "HotFix_Project.pdb.txt", (pdb) =>
                {
                    dllStream = new MemoryStream(dll.bytes);
                    pdbStream = new MemoryStream(pdb.bytes);

                    appDomain.LoadAssembly(dllStream, pdbStream, new PdbReaderProvider());

                    //��ʼ������
                    InitILRuntime();

                    //���Ե�ʱ��ʹ��
                    if (isDebugMode)
                    {
                        StartCoroutine(IcheakDebgger(callback));
                    }
                    else
                    {
                        callback?.Invoke(true);
                    }

                    isOpenILRuntime = true;
                });
            });
        }
        catch(System.Exception e)
        {
            Debug.Log("����ILRuntimeʧ�ܵľ�������:"+e.Message);
            callback?.Invoke(false);
        }
    }

    /// <summary>
    /// ��ʼ������
    /// </summary>
    private void InitILRuntime()
    {
        //�õ��߳�id�����ں������
        //appDomain.UnityMainThreadID = Thread.CurrentThread.ManagedThreadId;
    }

    private IEnumerator IcheakDebgger(UnityAction<bool> callBack)
    {
        //�ȴ����Խ���
        while(!appDomain.DebugService.IsDebuggerAttached)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1);

        callBack?.Invoke(true);
        StopCoroutine(IcheakDebgger(callBack));
    }

    public void CloseILRuntime()
    {
        appDomain.Dispose();
        appDomain = null;
        if(dllStream!=null)
        {
            dllStream.Dispose();
        }
        if(pdbStream!=null)
        {
            pdbStream.Dispose();
        }
        dllStream = null;
        pdbStream = null;
        isOpenILRuntime = false;
    }

}
