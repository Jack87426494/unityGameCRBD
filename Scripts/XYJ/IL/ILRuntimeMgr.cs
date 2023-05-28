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
    //dll内存流
    private MemoryStream dllStream;
    //pdb内存流
    private MemoryStream pdbStream;

    //是否打开了ILRuntime
    private bool isOpenILRuntime;

    //是够开启调试模式
    private bool isDebugMode=false;

    public void StartILRuntime(UnityAction<bool> callback)
    {
        if(isOpenILRuntime)
        {
            return;
        }

        try
        {
            //自动选择寄存器模式
            appDomain = new AppDomain(ILRuntimeJITFlags.JITOnDemand);

            //加载dll文件
            ABMgr.GetInstance().LoadResAsync<TextAsset>("dll", "HotFix_Project.dll.txt", (dll) =>
            {
                //加载pdb文件
                ABMgr.GetInstance().LoadResAsync<TextAsset>("dll", "HotFix_Project.pdb.txt", (pdb) =>
                {
                    dllStream = new MemoryStream(dll.bytes);
                    pdbStream = new MemoryStream(pdb.bytes);

                    appDomain.LoadAssembly(dllStream, pdbStream, new PdbReaderProvider());

                    //初始化操作
                    InitILRuntime();

                    //调试的时候使用
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
            Debug.Log("加载ILRuntime失败的具体问题:"+e.Message);
            callback?.Invoke(false);
        }
    }

    /// <summary>
    /// 初始化操作
    /// </summary>
    private void InitILRuntime()
    {
        //得到线程id，便于后面调试
        //appDomain.UnityMainThreadID = Thread.CurrentThread.ManagedThreadId;
    }

    private IEnumerator IcheakDebgger(UnityAction<bool> callBack)
    {
        //等待调试接入
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
