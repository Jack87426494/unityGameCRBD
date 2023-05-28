using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using UnityEngine.Rendering;
using System.IO;
using UnityEngine.Events;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using System.Xml.Linq;
using System.Text;
using UnityEngine.Networking;

public class ABUpdateMgr : MonoBehaviour
{
    private static ABUpdateMgr instance;
    public static ABUpdateMgr Instance
    {
        get
        {
            if(instance==null)
            {
                GameObject obj = new GameObject("ABUpdateMgr");
                instance=obj.AddComponent<ABUpdateMgr>();
            }
            return instance;
        }
    }

    //������ab���Ա��ļ��ֵ�,��һ��������ab��������,�ڶ���������ab���Ա��ļ���Ϣ
    private Dictionary<string, ABCompareClass> abCompareDic = new Dictionary<string, ABCompareClass>();

    //�������б� ��һ������ΪҪ���ص�ab������
    private List<string> downloadList=new List<string>();

    //Զ�����ص���ʱ�Ա��ļ�����Ϣ
    private string cacheCompareStr="";

    //Զ�˵ķ�������ַ
    private string serverPath = "ftp://192.168.43.200";

    //�ܵ������ֽ���
    private int byteNums;
    //Ŀǰ���ص��ֽ���
    private int nowNums;

    /// <summary>
    /// ���·���˵�����ab���ĶԱ��ļ���Ϣ������
    /// </summary>
    /// <param name="isDownloadCallback">�Ƿ����سɹ��ص�����</param>
    public async void UpdateABCompareFile(UnityAction<bool> isDownloadCallback=null)
    {
        //��������ϴ�ʧ�ܲ�����Ϣ
        abCompareDic.Clear();
        int reDownloadCount = 3;

        string path =
#if UNITY_IOS
            Application.persistentDataPath + "/AB/IOS/ABCompareFile.txt";
#elif UNITY_ANDRIOD
            Application.persistentDataPath + "/AB/Andriod/ABCompareFile.txt";
#else
            Application.persistentDataPath + "/AB/PC/ABCompareFile.txt";
#endif
        //�첽������⿨��
        await Task.Run(() =>
        {
            bool isDownload = false;
            //����ab���Ա��ļ�
            while (!isDownload && reDownloadCount > 0)
            {
                try
                {
                    FtpWebRequest ftpWebRequest = FtpWebRequest.Create(new Uri(serverPath+"/AB/PC/ABCompareFile.txt")) as FtpWebRequest;
                    NetworkCredential networkCredential = new NetworkCredential("xyj", "qwe123123");
                    ftpWebRequest.Credentials = networkCredential;
                    ftpWebRequest.KeepAlive = false;
                    ftpWebRequest.Proxy = null;
                    ftpWebRequest.UseBinary = true;
                    ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                    FtpWebResponse ftpWebResponse = ftpWebRequest.GetResponse() as FtpWebResponse;
                    using (Stream downloadStream = ftpWebResponse.GetResponseStream())
                    {
                        byte[] bytes = new byte[1024 * 20];
                        int nums = downloadStream.Read(bytes, 0, bytes.Length);
                        while (nums != 0)
                        {
                            nums = downloadStream.Read(bytes, 0, bytes.Length);
                        }
                        downloadStream.Dispose();
                        cacheCompareStr = Encoding.UTF8.GetString(bytes);
                        string[] abStrs = cacheCompareStr.Split('|');
                        foreach (string abStr in abStrs)
                        {
                            string[] abInfo = abStr.Split(' ');
                            abCompareDic.Add(abInfo[0], new ABCompareClass(abInfo[0], abInfo[1], abInfo[2]));
                        }
                    }
                    isDownload = true;
                }
                catch
                {
                    isDownload = false;
                }
                --reDownloadCount;
            }
        });

        isDownloadCallback(reDownloadCount != 0);
    }

    /// <summary>
    /// ��ȡԶ��ab���Ա��ļ���Ϣ
    /// </summary>
    public void GetABCompareInfo()
    {
        //��ȡ���غõ��ļ������Ҳ�����е���Ϣ�洢
        string abCompareContent =
#if UNITY_IOS
            File.ReadAllText(Application.persistentDataPath + "/AB/IOS/ABCompareFile.txt");
#elif UNITY_ANDRIOD
            File.ReadAllText(Application.persistentDataPath + "/AB/Andriod/ABCompareFile.txt");
#else
            File.ReadAllText(Application.persistentDataPath + "/AB/PC/ABCompareFile.txt");
#endif

        string[] abStrs = abCompareContent.Split('|');
        foreach (string abStr in abStrs)
        {
            string[] abInfos = abStr.Split(' ');
            abCompareDic.Add(abInfos[0], new ABCompareClass(abInfos[0], abInfos[1], abInfos[2]));
        }
        print("Զ��ab���Ա��ļ���Ϣλ�ã�" + Application.persistentDataPath);
    }

    /// <summary>
    /// ����ab����Դ�Ա��ļ�����Ϣ������ab����Դ
    /// </summary>
    /// <param name="resultCallBack">���ؽ��ί�У�����Ϊ�����Ƿ�ɹ�</param>
    /// <param name="prossesCallBack">���ؽ���ί�У���������Ϊ����һ������ΪĿǰ�Ѿ����ص���Դ�����ڶ�������Ϊ��Ҫ���ص���Դ����</param>
    public void UpdateABFile(UnityAction<bool> resultCallBack = null, UnityAction<int, int> prossesCallBack = null)
    {
        //��������ϴ�ʧ�ܲ�����Ϣ
        downloadList.Clear();
        //����Ŀǰ��ab�Ա���Ϣ�����������б�
        UpdateDownloadList( async () =>
        {
            //���������б�,��ʼ����ab����Դ
            //��Ҫ���ص���Դ������
            int maxDownloadCount = downloadList.Count;
            //Ŀǰ�Ѿ����ص���Դ��
            int nowDownloadCount = 0;
            //һ�������ظ����صĴ���
            int maxReDownloadCount = 5;

            //���½���
            prossesCallBack?.Invoke(nowDownloadCount, maxDownloadCount);

            string path =
#if UNITY_IOS
            Application.persistentDataPath + "/AB/IOS/";
#elif UNITY_ANDRIOD
            Application.persistentDataPath + "/AB/Andriod/";
#else
                Application.persistentDataPath + "/AB/PC/";
#endif

            while (downloadList.Count != 0 && maxReDownloadCount > 0)
            {
                //�첽������Դ
                await Task.Run(() =>
                {
                    //�Ӻ���ǰ����������ɾ��һ������ʱ��Ӱ�쵽�����С����������
                    for (int i = downloadList.Count - 1; i >= 0; --i)
                    {
                        //������سɹ��ʹ������б�����ɾ��������
                        if (DownLoadABFile(downloadList[i], path + downloadList[i]))
                        {
                            ++nowDownloadCount;
                            downloadList.RemoveAt(i);
                        }
                        
                    }
                    --maxReDownloadCount;
                });
                //���½���
                prossesCallBack?.Invoke(nowDownloadCount, maxDownloadCount);
            }
            

            //���³ɹ��������µ�ab���Ա��ļ�
            if (downloadList.Count == 0)
            {
                File.WriteAllText(path + "ABCompareFile.txt", cacheCompareStr);
            }

            resultCallBack?.Invoke(downloadList.Count == 0);

            //AssetDatabase.Refresh();

        });
      
    }

    /// <summary>
    /// ����Ŀǰ��ab�Ա���Ϣ�����������б�
    /// </summary>
    private void UpdateDownloadList(UnityAction callback)
    {
        //������ȥ�����µ�ab��Դ���޸��ϵ�ab����Դ�����ԭ������ab������û�еĶ��������ó�ɾ��persistentDataPath������ԭ����,����streamingAssetsPath�е�
        try
        {
            StartCoroutine(IUpdateDownloadList(callback));
        }
        catch(Exception e)
        {
            Debug.Log("���������б�ʧ�ܣ�"+e.Message);
        }
        
    }

    private IEnumerator IUpdateDownloadList(UnityAction callback)
    {
        string abLocalCompareContent = "";
        string[] abStrs;
        string path=
#if UNITY_IOS
            Application.persistentDataPath + "/AB/Andriod/;
#elif UNITY_Andriod
            Application.persistentDataPath + "/AB/IOS/;
#else
            Application.persistentDataPath + "/AB/PC/";
#endif
        //��ʼ����persistentDataPath
        if (File.Exists(path + "ABCompareFile.txt"))
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Get("file:///"+ path + "ABCompareFile.txt");
            yield return unityWebRequest.SendWebRequest();
            if(unityWebRequest.result==UnityWebRequest.Result.Success)
            {
                abLocalCompareContent = unityWebRequest.downloadHandler.text;
                abStrs = abLocalCompareContent.Split('|');
                foreach (string abStr in abStrs)
                {
                    string[] abInfos = abStr.Split(' ');
                    if (abCompareDic.ContainsKey(abInfos[0]))
                    {
                        //�����Դ�������ı䣬ɾ����ǰ�ģ������µ�
                        if (abInfos[2] != abCompareDic[abInfos[0]].md5)
                        {
                            File.Delete(path + abInfos[0]);
                            downloadList.Add(abInfos[0]);
                        }
                        abCompareDic.Remove(abInfos[0]);
                    }
                    else
                    {
                        //ֱ��ɾ��ԭ���Ĳ�����
                        File.Delete(path + abInfos[0]);
                    }
                }
            }
        }
        //��ʼ����streamingAssetsPath
        else if (File.Exists(Application.streamingAssetsPath + "/ABCompareFile.txt"))
        {
            //��ȡ���ص�streamingAssetsPath�ļ��е�ab��Դ�Ա��ļ���Ϣ
            UnityWebRequest unityWebRequest =
#if UNITY_ANDRIOD
            UnityWebRequest.Get(Application.streamingAssetsPath + "/ABCompareFile.txt");
#else
            UnityWebRequest.Get("file:///" + Application.streamingAssetsPath + "/ABCompareFile.txt");
#endif

            yield return unityWebRequest.SendWebRequest();
            if(unityWebRequest.result==UnityWebRequest.Result.Success)
            {
                abLocalCompareContent = unityWebRequest.downloadHandler.text;
                abStrs = abLocalCompareContent.Split('|');
                foreach (string abStr in abStrs)
                {
                    string[] abInfos = abStr.Split(' ');
                    //���Զ�˺ͱ��ض������ab���ļ�,�Ա�MD5�룬������Դ�Ƿ�һ�������Զ��û�����ab�����ļ���ֱ�Ӳ�����
                    if (abCompareDic.ContainsKey(abInfos[0]))
                    {
                        //�����Դ��һ�������ط����������ab��
                        if (abInfos[2] != abCompareDic[abInfos[0]].md5)
                        {
                            downloadList.Add(abInfos[0]);
                        }
                        //�Ƴ�Զ�˵Ĵ�����ab����Դ
                        abCompareDic.Remove(abInfos[0]);
                    }
                    else
                    {
                        //���ԭ��ab�������ab���������µ�ab������û�����ab�����Ͱ�ԭ�����е�ab����Ϣ���䵽��ab����Ϣ���档
                        cacheCompareStr += '|';
                        cacheCompareStr += abStr;
                    }
                }
            }
        }
        //����streamingAssetsPath��persistentDataPath��û�е���Զ���е�ab������������
        foreach (string abStr in abCompareDic.Keys)
        {
            downloadList.Add(abStr);
        }
        abCompareDic.Clear();
        callback();
        StopCoroutine(IUpdateDownloadList(callback));
    }

    /// <summary>
    /// ���ط���������AB���ļ���ab��Ϣ�Ա��ļ�
    /// </summary>
    /// <param name="name">Ҫ�����ļ�������</param>
    /// <param name="path">�ļ�Ҫ���ص����ص�λ��</param>
    /// <returns>�Ƿ����سɹ�</returns>
    public bool DownLoadABFile(string name,string path)
    {
        try
        {

            //����һ��ftp����
            FtpWebRequest ftpWebRequest =
#if UNITY_IOS
            FtpWebRequest.Create(new Uri(serverPath + "/AB/IOS/" + name)) as FtpWebRequest;
#elif UNITY_ANDRIOD
            FtpWebRequest.Create(new Uri(serverPath + "/AB/Andriod/" + name)) as FtpWebRequest;
#else
            FtpWebRequest.Create(new Uri(serverPath + "/AB/PC/" + name)) as FtpWebRequest;
#endif
            //������Կ
            NetworkCredential networkCredential = new NetworkCredential("xyj", "qwe123123");
            ftpWebRequest.Credentials = networkCredential;
            //�����ÿգ���ֹ��http��ͻ
            ftpWebRequest.Proxy = null;
            //ʹ�ö����ƴ���
            ftpWebRequest.UseBinary = true;
            //���غ�ͶϿ�����
            ftpWebRequest.KeepAlive = false;
            //����Ҫ�ɵ�����
            ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            //�õ�Ӧ�����
            FtpWebResponse ftpWebResponse = ftpWebRequest.GetResponse() as FtpWebResponse;
            Stream downloadStream = ftpWebResponse.GetResponseStream();
            //�򿪱����ļ���·����ʼ��Ӧ������д��
            using (FileStream localStream = File.OpenWrite(path))
            {
                byte[] bytes = new byte[2048];
                int length = downloadStream.Read(bytes, 0, bytes.Length);
                //�����Զ������д
                localStream.Write(bytes, 0, length);
                //��ͣ�Ĵ�Ӧ�����ж�ȡ�ֽڣ�ֱ����������
                while(length!=0)
                {
                    length = downloadStream.Read(bytes, 0, bytes.Length);
                    localStream.Write(bytes, 0, length);
                }
                localStream.Dispose();
                downloadStream.Dispose();
            }
            print("�ɹ������ļ�:" + name);
            return true;
        }
        catch(Exception e)
        {
            print("ʧ�������ļ���" + name + " " + e.Message);
            return false;
        }
       

    }

    private void OnDisable()
    {
        instance = null;
    }

    /// <summary>
    /// ab���Ա��ļ����ݽṹ
    /// </summary>
    private class ABCompareClass
    {
        //ab������
        public string abName;
        //ab������
        public long length;
        //MD5��
        public string md5;
        public ABCompareClass(string abName,string length,string md5)
        {
            this.abName = abName;
            this.length = Convert.ToInt64(length);
            this.md5 = md5;
        }

        //���ص��ںͲ��������ڱȽ�ab���Ա��ļ�
        public static bool operator ==(ABCompareClass a,ABCompareClass b)
        {
            return a.md5 == b.md5;
        }
        public static bool operator !=(ABCompareClass a,ABCompareClass b)
        {
            return a.md5 != b.md5;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }

}
