using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Reflection;

public class BinaryDataMgr
{
    //Excel�ļ���Ŷ�����������ű��ļ���·��
    public static string File_Binary_Path = Application.streamingAssetsPath + "/ExcelBinary/";

    private static BinaryDataMgr instance = new BinaryDataMgr();
    public static BinaryDataMgr Instance => instance;

    //���������ֵ�
    public Dictionary<string, object> containerDic = new Dictionary<string, object>();

    /// <summary>
    /// ��ȡ�����Ʊ��е�����
    /// </summary>
    /// <typeparam name="T">���ݽṹ��</typeparam>
    /// <typeparam name="K">����������</typeparam>
    public void LoadData<T,K>()
    {
        //���������Ŀ���ļ����򷵻�
        if (!Directory.Exists(File_Binary_Path))
            return;

        //�򿪴�Ŷ��������ݵ��ļ�
        using (FileStream fs=File.Open(File_Binary_Path + typeof(T).Name+".Xiao",FileMode.Open,FileAccess.Read))
        {
           
            //���ɶ���������
            byte[] bytes = new byte[fs.Length];
            //��ȡ�ļ��еĶ���������
            fs.Read(bytes, 0, bytes.Length);
            //�ر��ļ�
            fs.Close();
            //��¼�ļ������±�
            int index = 0;
            //��ȡʵ�ʴ洢�˶����е�����
            int rowsNum = BitConverter.ToInt32(bytes, index);
            index += 4;
            //��ȡ���������͵Ķ����Ƴ���
            int primaryKeyLength = BitConverter.ToInt32(bytes, index);
            index += 4;
            //��ȡ���������͵��ַ���
            string primaryKeyName = Encoding.UTF8.GetString(bytes,index,primaryKeyLength);
            index += primaryKeyLength;

            //�õ������������Type
            Type containerType = typeof(K);
            //ʵ��������������
            object containerObj = Activator.CreateInstance(containerType);

            //���������ֶ�����
            for (int i = 0; i<rowsNum;i++)
            {
                //�õ����ݽṹ��
                Type classType = typeof(T);
                object classObj = Activator.CreateInstance(classType);
                //�õ����ݽṹ�������е��ֶ�
                FieldInfo[] fieldInfos = classType.GetFields();
                //�������ݽṹ���������ֶ�
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    //Debug.Log("1");
                    //�������ղ�ͬ����������ֶ�����
                    if (fieldInfo.FieldType == typeof(int))
                    {
                        //Debug.Log(classObj);
                        fieldInfo.SetValue(classObj, BitConverter.ToInt32(bytes, index));
                        index += 4;
                    }
                    else if (fieldInfo.FieldType == typeof(float))
                    {
                        fieldInfo.SetValue(classObj, BitConverter.ToSingle(bytes, index));
                        index += 4;
                    }
                    else if (fieldInfo.FieldType == typeof(bool))
                    {
                        fieldInfo.SetValue(classObj, BitConverter.ToBoolean(bytes, index));
                        index += 1;
                    }
                    else if (fieldInfo.FieldType == typeof(string))
                    {
                        //��ȡ�ַ����Ķ����Ƴ���
                        int stringBinaryLength = BitConverter.ToInt32(bytes, index);
                        index += 4;
                        //��ȡ�ַ�����ʵ�����ݣ���������ʵ���ֶε�ֵ
                        fieldInfo.SetValue(classObj, Encoding.UTF8.GetString(bytes, index, stringBinaryLength));
                        index += stringBinaryLength;
                    }
                }
            
                //�õ�������������ֵ��ֶ�
                FieldInfo dicInfo=containerType.GetField(typeof(T).Name + "dic");
                
                //�õ��ֵ��ʵ��
                object dicObj = dicInfo.GetValue(containerObj);
                //�õ����ݽṹ��������������
                object keyObj = classType.GetField(primaryKeyName).GetValue(classObj);
                //�õ��ֵ��Add����
                MethodInfo addMethodInfo = dicObj.GetType().GetMethod("Add");
                //���ֵ����������
                addMethodInfo.Invoke(dicObj, new object[] { keyObj, classObj });
            }

            if(!containerDic.ContainsKey(containerType.Name))
            //���˶�ȡ��������������ӵ����������ֵ�
            containerDic.Add(containerType.Name, containerObj);
            //Debug.Log(containerType.Name);
        }
    }

    /// <summary>
    /// �õ������������ֵ��е�����������
    /// </summary>
    /// <typeparam name="T">���������������</typeparam>
    /// <returns></returns>
    public T GetTable<T>() where T:class
    {
        //��������������
        string tableName = typeof(T).Name;
        //Debug.Log(tableName);
        //Debug.Log(containerDic.ContainsKey(tableName));
        //��������������ֵ�����������������࣬�򷵻أ����򷵻ؿ�
        if (containerDic.ContainsKey(tableName))
            return containerDic[tableName] as T;
        return null;
    }
   
}
