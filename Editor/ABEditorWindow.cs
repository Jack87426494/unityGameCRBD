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
    [MenuItem("AB包工具/打开ab包管理窗口")]
    public static void OpenABEditorWindow()
    {
        //得到编辑器窗口
        ABEditorWindow aBEditorWindow = EditorWindow.GetWindowWithRect(typeof(ABEditorWindow), new Rect(0, 0, 430, 260)) as ABEditorWindow;
        //打开编辑器窗口
        aBEditorWindow.Show();
    }

    //平台编号
    private int platformIndex;
    private string[] platformStrs = { "PC", "IOS", "Andriod" };
    //服务器地址
    private string serverPath = "ftp://127.0.0.1";

    //绘制编辑器窗口
    private void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 100, 30), "选择平台");
        platformIndex=GUI.Toolbar(new Rect(100, 20, 300, 30), platformIndex, platformStrs);

        GUI.Label(new Rect(20, 80, 100, 30), "远端服务器地址");
        serverPath = GUI.TextField(new Rect(120, 85, 300, 20), serverPath);

        if(GUI.Button(new Rect(20, 130, 150, 30), "生成资源对比文件"))
        {
            CreatABCompareFile();
        }
        if (GUI.Button(new Rect(200, 130, 150, 30), "上传AB包"))
        {
            UpLoadAB();
        }
        if (GUI.Button(new Rect(20, 180, 330, 30), "将选中资源移动到StreamingAssets文件夹"))
        {
            MoveABFileToSA();
        }
    }

    /// <summary>
    /// 创建ab包资源对比文件
    /// </summary>
    private void CreatABCompareFile()
    {
        //得到文件夹
        DirectoryInfo directoryInfo = Directory.CreateDirectory(Application.dataPath + "/AB/"+ platformStrs[platformIndex] + "/");
        //得到文件夹里面的所有文件
        FileInfo[] fileInfos = directoryInfo.GetFiles();

        //资源对比文件中的内容，一个字符串：ab包的名字 ab包大小 MD5码|
        string abCompareContent = "";

        foreach (FileInfo fileInfo in fileInfos)
        {
            //只得到ab的信息，因为ab包没有后缀
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
        //删除末尾的"|"符号
        abCompareContent = abCompareContent.Substring(0, abCompareContent.Length - 1);
        //保存该文件到ab包的文件夹中
        File.WriteAllText(Application.dataPath + "/AB/" + platformStrs[platformIndex] + "/ABCompareFile.txt", abCompareContent);
        //刷新编辑器
        AssetDatabase.Refresh();
    }
    private string GetMD5(string path)
    {
        //打开要得到MD5信息的文件流
        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        {
            MD5 d5 = new MD5CryptoServiceProvider();
            //将文件流的信息解析计算为MD5码
            byte[] bytes = d5.ComputeHash(fileStream);
            //释放流
            fileStream.Dispose();
            //使用StringBuilder因为连续修改更节约性能。
            StringBuilder stringBuilder = new StringBuilder();
            //将解析结果的字节信息转化为16位的字符串信息，减少大小。
            foreach (byte b in bytes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }

    /// <summary>
    /// 移动ab包资源文件和ab包资源对比文件到StreammingAssets文件夹
    /// </summary>
    private void MoveABFileToSA()
    {
        //找到选中的物体
        UnityEngine.Object[] objects = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Deep);

        string abCompareContent = "";

        MD5 d5 = new MD5CryptoServiceProvider();
        //遍历选中的物体得到它们的文件信息
        foreach (UnityEngine.Object obj in objects)
        {
            //得到文件的路径
            string filePath = AssetDatabase.GetAssetPath(obj);
            //得到文件名
            string fileName = filePath.Substring(filePath.LastIndexOf('/'));
            FileInfo fileInfo = new FileInfo(Application.dataPath + "/AB/" + platformStrs[platformIndex]+"/"+ fileName);
            //如果是ab包再进行下一步的处理
            if (fileInfo.Extension != "")
            {
                continue;
            }
            //将文件先移动到SreamingAssets文件夹
            AssetDatabase.CopyAsset(filePath, "Assets/StreamingAssets/" + fileName);

            //移动之后再得到它们的信息生成ab包对比文件
            using (FileStream fileStream = File.OpenRead(fileInfo.FullName))
            {
                byte[] bytes = d5.ComputeHash(fileStream);
                fileStream.Close();
                //将byte数组转化为16位字符串
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
    /// 上传ab包文件和ab包资源对比文件
    /// </summary>
    private void UpLoadAB()
    {
        //得到文件夹
        DirectoryInfo directoryInfo = Directory.CreateDirectory(Application.dataPath + "/AB/" + platformStrs[platformIndex] +"/");
        //得到文件夹内的文件
        FileInfo[] fileInfos = directoryInfo.GetFiles();
        foreach (FileInfo fileInfo in fileInfos)
        {
            //如果是ab包或者是资源对比文件才上传
            if (fileInfo.Name == "ABCompareFile.txt" || fileInfo.Extension == "")
            {
                FTPUpLoadFile(fileInfo.FullName, fileInfo.Name);
            }
        }

    }

    /// <summary>
    /// 使用ftp异步上传资源文件
    /// </summary>
    /// <param name="path">本地文件的路径</param>
    /// <param name="name">本地文件的名字</param>
    private async void FTPUpLoadFile(string path, string name)
    {
        await Task.Run(() =>
        {
            try
            {
                FtpWebRequest ftpWebRequest = FtpWebRequest.Create(new Uri(serverPath+"/AB/" + 
                    platformStrs[platformIndex] +"/" + name)) as FtpWebRequest;
                NetworkCredential networkCredential = new NetworkCredential("xyj", "qwe123123");
                //建立秘钥
                ftpWebRequest.Credentials = networkCredential;
                //连接完马上断开
                ftpWebRequest.KeepAlive = false;
                //使用二进制传输
                ftpWebRequest.UseBinary = true;
                //设置要做的事儿
                ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
                //代理设置成空，避免与http协议产生冲突
                ftpWebRequest.Proxy = null;
                //得到上传文件流
                Stream updateStream = ftpWebRequest.GetRequestStream();
                //打开本地文件流开始上传
                using (FileStream localStream = File.OpenRead(path))
                {
                    //读取本地的数据,每次传2k
                    byte[] bytes = new byte[2048];
                    //先本地的字节读出来写入字节数组
                    int count = localStream.Read(bytes, 0, bytes.Length);
                    //再从字节数组写入上传流
                    updateStream.Write(bytes, 0, count);
                    while (count != 0)
                    {
                        //流数据，自动会往后面读
                        count = localStream.Read(bytes, 0, bytes.Length);
                        //再从字节数组写入上传流
                        updateStream.Write(bytes, 0, count);
                    }
                    //用了关闭两个流对象
                    updateStream.Dispose();
                    localStream.Dispose();
                    Debug.Log(name + " 上传成功");
                }
            }
            catch (Exception e)
            {
                Debug.Log(name + " 上传失败 " + e.Message);
            }

        });
    }
}
