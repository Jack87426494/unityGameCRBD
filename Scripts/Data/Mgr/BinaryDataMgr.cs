using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Reflection;

public class BinaryDataMgr
{
    //Excel文件存放二进制数据类脚本文件的路径
    public static string File_Binary_Path = Application.streamingAssetsPath + "/ExcelBinary/";

    private static BinaryDataMgr instance = new BinaryDataMgr();
    public static BinaryDataMgr Instance => instance;

    //数据容器字典
    public Dictionary<string, object> containerDic = new Dictionary<string, object>();

    /// <summary>
    /// 读取二进制表中的数据
    /// </summary>
    /// <typeparam name="T">数据结构类</typeparam>
    /// <typeparam name="K">数据容器类</typeparam>
    public void LoadData<T,K>()
    {
        //如果不存在目标文件夹则返回
        if (!Directory.Exists(File_Binary_Path))
            return;

        //打开存放二进制数据的文件
        using (FileStream fs=File.Open(File_Binary_Path + typeof(T).Name+".Xiao",FileMode.Open,FileAccess.Read))
        {
           
            //生成二进制容器
            byte[] bytes = new byte[fs.Length];
            //读取文件中的二进制数据
            fs.Read(bytes, 0, bytes.Length);
            //关闭文件
            fs.Close();
            //记录文件流的下标
            int index = 0;
            //读取实际存储了多少行的数据
            int rowsNum = BitConverter.ToInt32(bytes, index);
            index += 4;
            //读取主键的类型的二进制长度
            int primaryKeyLength = BitConverter.ToInt32(bytes, index);
            index += 4;
            //读取主键的类型的字符串
            string primaryKeyName = Encoding.UTF8.GetString(bytes,index,primaryKeyLength);
            index += primaryKeyLength;

            //得到数据容器类的Type
            Type containerType = typeof(K);
            //实例化数据容器类
            object containerObj = Activator.CreateInstance(containerType);

            //遍历所有字段数据
            for (int i = 0; i<rowsNum;i++)
            {
                //得到数据结构类
                Type classType = typeof(T);
                object classObj = Activator.CreateInstance(classType);
                //得到数据结构类中所有的字段
                FieldInfo[] fieldInfos = classType.GetFields();
                //遍历数据结构类中所有字段
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    //Debug.Log("1");
                    //按次序按照不同的类型添加字段数据
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
                        //读取字符串的二进制长度
                        int stringBinaryLength = BitConverter.ToInt32(bytes, index);
                        index += 4;
                        //读取字符串的实际内容，并且设置实际字段的值
                        fieldInfo.SetValue(classObj, Encoding.UTF8.GetString(bytes, index, stringBinaryLength));
                        index += stringBinaryLength;
                    }
                }
            
                //得到数据容器类的字典字段
                FieldInfo dicInfo=containerType.GetField(typeof(T).Name + "dic");
                
                //得到字典的实例
                object dicObj = dicInfo.GetValue(containerObj);
                //得到数据结构类中主键的数据
                object keyObj = classType.GetField(primaryKeyName).GetValue(classObj);
                //得到字典的Add方法
                MethodInfo addMethodInfo = dicObj.GetType().GetMethod("Add");
                //在字典中添加数据
                addMethodInfo.Invoke(dicObj, new object[] { keyObj, classObj });
            }

            if(!containerDic.ContainsKey(containerType.Name))
            //将此读取的数据容器类添加到数据容器字典
            containerDic.Add(containerType.Name, containerObj);
            //Debug.Log(containerType.Name);
        }
    }

    /// <summary>
    /// 得到数据容器类字典中的数据容器类
    /// </summary>
    /// <typeparam name="T">数据容器类的类型</typeparam>
    /// <returns></returns>
    public T GetTable<T>() where T:class
    {
        //数据容器类名字
        string tableName = typeof(T).Name;
        //Debug.Log(tableName);
        //Debug.Log(containerDic.ContainsKey(tableName));
        //如果数据容器类字典中有这个数据容器类，则返回，否则返回空
        if (containerDic.ContainsKey(tableName))
            return containerDic[tableName] as T;
        return null;
    }
   
}
