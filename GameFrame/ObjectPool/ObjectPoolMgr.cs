using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 对象池
/// </summary>
public class ObjectPool
{
    //对象池中对象的父对象
    private GameObject objectsFather;
    //对象池中的对象列表
    public List<GameObject> objectPoolList;

    /// <summary>
    /// 初始化对象池
    /// </summary>
    public ObjectPool(GameObject obj, Transform objectPoolsFatherT)
    {
        objectsFather = new GameObject(obj.name);
        objectsFather.transform.SetParent(objectPoolsFatherT);
        objectPoolList = new List<GameObject>();
        PutObject(obj);
    }

    /// <summary>
    /// 池子里面拿东西
    /// </summary>
    /// <returns>池子里面的东西</returns>
    public GameObject GetObject()
    {

        GameObject obj;
        //直接取出来
        obj = objectPoolList[0];
        obj.SetActive(true);
        obj.transform.SetParent(null);
        objectPoolList.RemoveAt(0);

        return obj;
    }

    /// <summary>
    /// 池子里面放东西
    /// </summary>
    /// <param name="obj">要放到池子里面的东西</param>

    public void PutObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(objectsFather.transform);
        objectPoolList.Add(obj);

    }
}

/// <summary>
/// 对象池管理器
/// </summary>
public class ObjectPoolMgr : BaseMgr<ObjectPoolMgr>
{
    //对象池字典，一个list一个对象池
    private Dictionary<string,ObjectPool> objectPoolDic = new Dictionary<string, ObjectPool>();

    //缓存池的父对象
    private GameObject objectPoolsFather;
    /// <summary>
    /// 从池子里面拿东西，并保持对象池中对象大于0
    /// </summary>
    /// <param name="objPath">要拿的对象的路径</param>
    /// <param name="callBack">对象创建或激活后需要执行的函数，参数为被创建的物体的GameObject</param>
    /// <returns>返回从对象池中拿出来的数据</returns>
    public void GetObject(string objPath, UnityAction<GameObject> callBack)
    {
       

        //首先判断有无这种数据类型的对象池,并且如果有这个对象池，如果对象池中的数据足够，就将其拿出来
        if (objectPoolDic.ContainsKey(objPath)&& objectPoolDic[objPath].objectPoolList.Count > 0)
        {
            callBack(objectPoolDic[objPath].GetObject());
        }
        else
        {
            //如果先前没有这个对象池
            //实例化数据创建一个游戏对象在游戏场景中

            //如果对象池中的对象不够了
            //就实例化数据创建一个游戏对象在游戏场景中
            ResLoadMgr.Instance.LoadAsyn<GameObject>(objPath, (obj) =>
            {
                //设置名字
                obj.name = objPath;
                callBack(obj);
            });

        }
       
       
       
    }

    /// <summary>
    /// 放置对象在对象池中
    /// </summary>
    /// <param name="obj"></param>
    public void PutObject(GameObject obj)
    {

        if (objectPoolsFather == null)
            objectPoolsFather = new GameObject("ObjectPoolsFather");

        string objPath = obj.name;

        //如果没池子就建一个
        if(!objectPoolDic.ContainsKey(objPath))
        {
            objectPoolDic.Add(objPath, new ObjectPool(obj, objectPoolsFather.transform));
        }

        objectPoolDic[objPath].PutObject(obj);
    }

    /// <summary>
    /// 过场景时清空对象池
    /// </summary>
    public void ClearObjectPool()
    {
        objectPoolDic.Clear();
        objectPoolsFather = null;
    }

}
