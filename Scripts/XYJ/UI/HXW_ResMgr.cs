using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;


//资源加载模块
public class HXW_ResMgr : HXW_BaseManager<HXW_ResMgr>
{
    //注意Resources.Load不管是/还是\\都可以使用
    //目前测试unity中/和\\效果几乎都是一样的，比如FileStream，Resource中都一样

    //同步加载资源
    public T Load<T>(string name, bool isInstantiate = true) where T : Object
    {
        T res = Resources.Load<T>(name);
        //如果对象是一个GameObject类型的，我把它实例化后，再返回出去直接使用。
        if (res is GameObject && isInstantiate == true) //有时候我们需要对一个GameObject重复Instantiate
            return GameObject.Instantiate(res);
        else //else情况示例：TextAsset、AudioClip
            return res;
    }

    //同步加载资源
    //LoadAll目标为文件夹，则会查找文件夹下的所有子文件夹的所有T类型文件
    //LoadAll目标为Mutiply的图片，则会返回该Mutiply的所有单张图片
    public T[] LoadAll<T>(string name, bool isInstantiate = true) where T : Object
    {
        T[] res = Resources.LoadAll<T>(name);
        //如果对象是一个GameObject类型的，我把它实例化后，再返回出去直接使用。
        if (res is GameObject[] && isInstantiate == true)
        {
            for (int i = 0; i < res.Length; i++)
                res[i] = GameObject.Instantiate(res[i]);

            return res;
        }
        else //else情况示例：TextAsset、AudioClip
            return res;
    }

    //异步加载资源 
    public void LoadAsync<T>(string name, UnityAction<T> callback, bool isInstantiate = true) where T : Object
    {
        //开启异步加载的协程
        HXW_MonoMgr.GetInstance().StartCoroutine(ReallyLoadAsync<T>(name, callback, isInstantiate));
    }
    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callback, bool isInstantiate = true) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);
        yield return r;

        if (r.asset is GameObject && isInstantiate)
        {
            //实例化一下再传给方法
            callback(GameObject.Instantiate(r.asset) as T);
        }
        else
        {
            //直接传给方法
            callback(r.asset as T);
        }
    }

}
