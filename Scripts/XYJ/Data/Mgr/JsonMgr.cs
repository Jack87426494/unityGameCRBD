using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
public enum JsonType
{
    LitJson,
    JsonUtility
}
public class JsonMgr
{
    private static JsonMgr instance = new JsonMgr();
    public static JsonMgr Instance => instance;
    private JsonMgr() { }
    public void SaveData(object data,string fileName,JsonType jsonType=JsonType.LitJson)
    {
        string str = "";
        switch (jsonType)
        {
            case JsonType.LitJson:
                 str= JsonMapper.ToJson(data);
                break;
            case JsonType.JsonUtility:
                str = JsonUtility.ToJson(data);
                break;
        }
        MonoBehaviour.print(Application.persistentDataPath);
        File.WriteAllText(Application.persistentDataPath +"/"+ fileName+".json", str);
    }
    public T LoadData<T>(string fileName, JsonType jsonType = JsonType.LitJson)
    {
        string str = "";
        if (File.Exists(Application.streamingAssetsPath + "/" + fileName + ".json"))
            str = File.ReadAllText(Application.streamingAssetsPath + "/" + fileName + ".json");
        else if (File.Exists(Application.persistentDataPath + "/" + fileName + ".json"))
            str = File.ReadAllText(Application.persistentDataPath + "/" + fileName + ".json");
        else
            return default(T);
        T t=default(T);
        switch (jsonType)
        {
            case JsonType.LitJson:
               t  = JsonMapper.ToObject<T>(str);
                break;
            case JsonType.JsonUtility:
                t = JsonUtility.FromJson<T>(str);
                break;
        }
        return t;
    }
}
