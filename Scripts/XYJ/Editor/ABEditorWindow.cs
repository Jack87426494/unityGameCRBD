using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System;

public class ABEditorWindow : EditorWindow
{
    [MenuItem("AB������/��ab��������")]
    public static void OpenABEditorWindow()
    {
        //�õ��༭������
        ABEditorWindow aBEditorWindow = EditorWindow.GetWindowWithRect(typeof(ABEditorWindow), new Rect(0, 0, 430, 260)) as ABEditorWindow;
        //�򿪱༭������
        aBEditorWindow.Show();
    }

    //ƽ̨���
    private int platformIndex;
    private string[] platformStrs = { "PC", "IOS", "Andriod" };
    //��������ַ
    private string serverPath = "ftp://127.0.0.1";

    //���Ʊ༭������
    private void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 100, 30), "ѡ��ƽ̨");
        platformIndex=GUI.Toolbar(new Rect(100, 20, 300, 30), platformIndex, platformStrs);

        GUI.Label(new Rect(20, 80, 100, 30), "Զ�˷�������ַ");
        serverPath = GUI.TextField(new Rect(120, 85, 300, 20), serverPath);

        if(GUI.Button(new Rect(20, 130, 150, 30), "������Դ�Ա��ļ�"))
        {
            CreatABCompareFile();
        }
        if (GUI.Button(new Rect(200, 130, 150, 30), "�ϴ�AB��"))
        {
            UpLoadAB();
        }
        if (GUI.Button(new Rect(20, 180, 330, 30), "��ѡ����Դ�ƶ���StreamingAssets�ļ���"))
        {
            MoveABFileToSA();
        }
    }

    /// <summary>
    /// ����ab����Դ�Ա��ļ�
    /// </summary>
    private void CreatABCompareFile()
    {
        //�õ��ļ���
        DirectoryInfo directoryInfo = Directory.CreateDirectory(Application.dataPath + "/AB/"+ platformStrs[platformIndex] + "/");
        //�õ��ļ�������������ļ�
        FileInfo[] fileInfos = directoryInfo.GetFiles();

        //��Դ�Ա��ļ��е����ݣ�һ���ַ�����ab�������� ab����С MD5��|
        string abCompareContent = "";

        foreach (FileInfo fileInfo in fileInfos)
        {
            //ֻ�õ�ab����Ϣ����Ϊab��û�к�׺
            if (fileInfo.Extension == "")
            {
                abCompareContent += fileInfo.Name;
                abCompareContent += " ";
                abCompareContent += fileInfo.Length;
                abCompareContent += " ";
                abCompareContent += GetMD5(Application.dataPath + "/AB/" + platformStrs[platformIndex] + "/" + fileInfo.Name);
                abCompareContent += "|";
            }
        }
        //ɾ��ĩβ��"|"����
        abCompareContent = abCompareContent.Substring(0, abCompareContent.Length - 1);
        //������ļ���ab�����ļ�����
        File.WriteAllText(Application.dataPath + "/AB/" + platformStrs[platformIndex] + "/ABCompareFile.txt", abCompareContent);
        //ˢ�±༭��
        AssetDatabase.Refresh();
    }
    private string GetMD5(string path)
    {
        //��Ҫ�õ�MD5��Ϣ���ļ���
        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        {
            MD5 d5 = new MD5CryptoServiceProvider();
            //���ļ�������Ϣ��������ΪMD5��
            byte[] bytes = d5.ComputeHash(fileStream);
            //�ͷ���
            fileStream.Dispose();
            //ʹ��StringBuilder��Ϊ�����޸ĸ���Լ���ܡ�
            StringBuilder stringBuilder = new StringBuilder();
            //������������ֽ���Ϣת��Ϊ16λ���ַ�����Ϣ�����ٴ�С��
            foreach (byte b in bytes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }

    /// <summary>
    /// �ƶ�ab����Դ�ļ���ab����Դ�Ա��ļ���StreammingAssets�ļ���
    /// </summary>
    private void MoveABFileToSA()
    {
        //�ҵ�ѡ�е�����
        UnityEngine.Object[] objects = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Deep);

        string abCompareContent = "";

        MD5 d5 = new MD5CryptoServiceProvider();
        //����ѡ�е�����õ����ǵ��ļ���Ϣ
        foreach (UnityEngine.Object obj in objects)
        {
            //�õ��ļ���·��
            string filePath = AssetDatabase.GetAssetPath(obj);
            //�õ��ļ���
            string fileName = filePath.Substring(filePath.LastIndexOf('/'));
            FileInfo fileInfo = new FileInfo(Application.dataPath + "/AB/" + platformStrs[platformIndex]+"/"+ fileName);
            //�����ab���ٽ�����һ���Ĵ���
            if (fileInfo.Extension != "")
            {
                continue;
            }
            //���ļ����ƶ���SreamingAssets�ļ���
            AssetDatabase.CopyAsset(filePath, "Assets/StreamingAssets/" + fileName);

            //�ƶ�֮���ٵõ����ǵ���Ϣ����ab���Ա��ļ�
            using (FileStream fileStream = File.OpenRead(fileInfo.FullName))
            {
                byte[] bytes = d5.ComputeHash(fileStream);
                fileStream.Close();
                //��byte����ת��Ϊ16λ�ַ���
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }
                abCompareContent += fileInfo.Name + " " + fileInfo.Length + " " + stringBuilder.ToString();
                abCompareContent += "|";
            }
        }
        abCompareContent.Substring(0, abCompareContent.Length - 1);
        File.WriteAllText(Application.streamingAssetsPath + "/ABCompareFile.txt", abCompareContent);
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// �ϴ�ab���ļ���ab����Դ�Ա��ļ�
    /// </summary>
    private void UpLoadAB()
    {
        //�õ��ļ���
        DirectoryInfo directoryInfo = Directory.CreateDirectory(Application.dataPath + "/AB/" + platformStrs[platformIndex] +"/");
        //�õ��ļ����ڵ��ļ�
        FileInfo[] fileInfos = directoryInfo.GetFiles();
        foreach (FileInfo fileInfo in fileInfos)
        {
            //�����ab����������Դ�Ա��ļ����ϴ�
            if (fileInfo.Name == "ABCompareFile.txt" || fileInfo.Extension == "")
            {
                FTPUpLoadFile(fileInfo.FullName, fileInfo.Name);
            }
        }

    }

    /// <summary>
    /// ʹ��ftp�첽�ϴ���Դ�ļ�
    /// </summary>
    /// <param name="path">�����ļ���·��</param>
    /// <param name="name">�����ļ�������</param>
    private async void FTPUpLoadFile(string path, string name)
    {
        await Task.Run(() =>
        {
            try
            {
                FtpWebRequest ftpWebRequest = FtpWebRequest.Create(new Uri(serverPath+"/AB/" + 
                    platformStrs[platformIndex] +"/" + name)) as FtpWebRequest;
                NetworkCredential networkCredential = new NetworkCredential("xyj", "qwe123123");
                //������Կ
                ftpWebRequest.Credentials = networkCredential;
                //���������϶Ͽ�
                ftpWebRequest.KeepAlive = false;
                //ʹ�ö����ƴ���
                ftpWebRequest.UseBinary = true;
                //����Ҫ�����¶�
                ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
                //�������óɿգ�������httpЭ�������ͻ
                ftpWebRequest.Proxy = null;
                //�õ��ϴ��ļ���
                Stream updateStream = ftpWebRequest.GetRequestStream();
                //�򿪱����ļ�����ʼ�ϴ�
                using (FileStream localStream = File.OpenRead(path))
                {
                    //��ȡ���ص�����,ÿ�δ�2k
                    byte[] bytes = new byte[2048];
                    //�ȱ��ص��ֽڶ�����д���ֽ�����
                    int count = localStream.Read(bytes, 0, bytes.Length);
                    //�ٴ��ֽ�����д���ϴ���
                    updateStream.Write(bytes, 0, count);
                    while (count != 0)
                    {
                        //�����ݣ��Զ����������
                        count = localStream.Read(bytes, 0, bytes.Length);
                        //�ٴ��ֽ�����д���ϴ���
                        updateStream.Write(bytes, 0, count);
                    }
                    //���˹ر�����������
                    updateStream.Dispose();
                    localStream.Dispose();
                    Debug.Log(name + " �ϴ��ɹ�");
                }
            }
            catch (Exception e)
            {
                Debug.Log(name + " �ϴ�ʧ�� " + e.Message);
            }

        });
    }
}
