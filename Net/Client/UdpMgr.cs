using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UdpMgr : MonoBehaviour
{
    private Socket socket;

    //发送消息的队列
    private Queue<string> sendQ=new Queue<string>();
    //接受消息的队列
    private Queue<string> receiveQ=new Queue<string>();
    //接收消息的byte容器
    private byte[] bytes;
    //接收到的消息的byte的长度
    private int byteLength;

    //是否处于连接状态
    private bool isConnected;

    


    private static UdpMgr instance;
    public static UdpMgr Instance => instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    
    /// <summary>
    /// 连接网络
    /// </summary>
    /// <param name="ipStr">ip地址</param>
    /// <param name="portNum">端口号</param>
    public void Connect(string ipStr,int portNum)
    {
        if (isConnected)
            return;

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), portNum);

        try
        {
            socket.Connect(iPEndPoint);
            isConnected = true;
            //发送消息
            ThreadPool.QueueUserWorkItem((o) =>
            {
                
                while (isConnected)
                {
                    
                    if (sendQ.Count>0)
                    {
                        
                        socket.Send(Encoding.UTF8.GetBytes(sendQ.Dequeue()));
                    }
                    
                }

            });

            bytes = new byte[1024 * 1024];
            //接收消息
            ThreadPool.QueueUserWorkItem((o) =>
            {
                while (isConnected)
                {
                    if (socket.Available > 0)
                    {
                        byteLength = socket.Receive(bytes);
                        receiveQ.Enqueue(Encoding.UTF8.GetString(bytes, 0, byteLength));
                    }
                    
                }
            });
        }
        catch (SocketException e)
        {
            if(e.ErrorCode==10061)
            {
                print("服务器拒接连接" + e.Message);
            }
            else
            {
                print(e.Message);
                
            }
            return;
        }
        

       
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="str"></param>
    public void Send(string str)
    {
        sendQ.Enqueue(str);
    }
    /// <summary>
    /// 断开网络连接
    /// </summary>
    public void Close()
    {
        isConnected = false;
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
        socket = null;
    }

    private void Update()
    {
        

        if(receiveQ.Count>0)
        {
            print(receiveQ.Dequeue());
        }
    }
}
