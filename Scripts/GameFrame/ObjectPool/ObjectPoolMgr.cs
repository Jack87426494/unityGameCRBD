using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �����
/// </summary>
public class ObjectPool
{
    //������ж���ĸ�����
    private GameObject objectsFather;
    //������еĶ����б�
    public List<GameObject> objectPoolList;

    /// <summary>
    /// ��ʼ�������
    /// </summary>
    public ObjectPool(GameObject obj, Transform objectPoolsFatherT)
    {
        objectsFather = new GameObject(obj.name);
        objectsFather.transform.SetParent(objectPoolsFatherT);
        objectPoolList = new List<GameObject>();
        PutObject(obj);
    }

    /// <summary>
    /// ���������ö���
    /// </summary>
    /// <returns>��������Ķ���</returns>
    public GameObject GetObject()
    {

        GameObject obj;
        //ֱ��ȡ����
        obj = objectPoolList[0];
        obj.SetActive(true);
        obj.transform.SetParent(null);
        objectPoolList.RemoveAt(0);

        return obj;
    }

    /// <summary>
    /// ��������Ŷ���
    /// </summary>
    /// <param name="obj">Ҫ�ŵ���������Ķ���</param>

    public void PutObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(objectsFather.transform);
        objectPoolList.Add(obj);

    }
}

/// <summary>
/// ����ع�����
/// </summary>
public class ObjectPoolMgr : BaseMgr<ObjectPoolMgr>
{
    //������ֵ䣬һ��listһ�������
    private Dictionary<string,ObjectPool> objectPoolDic = new Dictionary<string, ObjectPool>();

    //����صĸ�����
    private GameObject objectPoolsFather;
    /// <summary>
    /// �ӳ��������ö����������ֶ�����ж������0
    /// </summary>
    /// <param name="objPath">Ҫ�õĶ����·��</param>
    /// <param name="callBack">���󴴽��򼤻����Ҫִ�еĺ���������Ϊ�������������GameObject</param>
    /// <returns>���شӶ�������ó���������</returns>
    public void GetObject(string objPath, UnityAction<GameObject> callBack)
    {
       

        //�����ж����������������͵Ķ����,����������������أ����������е������㹻���ͽ����ó���
        if (objectPoolDic.ContainsKey(objPath)&& objectPoolDic[objPath].objectPoolList.Count > 0)
        {
            callBack(objectPoolDic[objPath].GetObject());
        }
        else
        {
            //�����ǰû����������
            //ʵ�������ݴ���һ����Ϸ��������Ϸ������

            //���������еĶ��󲻹���
            //��ʵ�������ݴ���һ����Ϸ��������Ϸ������
            ResLoadMgr.Instance.LoadAsyn<GameObject>(objPath, (obj) =>
            {
                //��������
                obj.name = objPath;
                callBack(obj);
            });

        }
       
       
       
    }

    /// <summary>
    /// ���ö����ڶ������
    /// </summary>
    /// <param name="obj"></param>
    public void PutObject(GameObject obj)
    {

        if (objectPoolsFather == null)
            objectPoolsFather = new GameObject("ObjectPoolsFather");

        string objPath = obj.name;

        //���û���Ӿͽ�һ��
        if(!objectPoolDic.ContainsKey(objPath))
        {
            objectPoolDic.Add(objPath, new ObjectPool(obj, objectPoolsFather.transform));
        }

        objectPoolDic[objPath].PutObject(obj);
    }

    /// <summary>
    /// ������ʱ��ն����
    /// </summary>
    public void ClearObjectPool()
    {
        objectPoolDic.Clear();
        objectPoolsFather = null;
    }

}
