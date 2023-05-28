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

    //������Ϣ�Ķ���
    private Queue<string> sendQ=new Queue<string>();
    //������Ϣ�Ķ���
    private Queue<string> receiveQ=new Queue<string>();
    //������Ϣ��byte����
    private byte[] bytes;
    //���յ�����Ϣ��byte�ĳ���
    private int byteLength;

    //�Ƿ�������״̬
    private bool isConnected;

    


    private static UdpMgr instance;
    public static UdpMgr Instance => instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="ipStr">ip��ַ</param>
    /// <param name="portNum">�˿ں�</param>
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
            //������Ϣ
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
            //������Ϣ
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
                print("�������ܽ�����" + e.Message);
            }
            else
            {
                print(e.Message);
                
            }
            return;
        }
        

       
    }

    /// <summary>
    /// ������Ϣ
    /// </summary>
    /// <param name="str"></param>
    public void Send(string str)
    {
        sendQ.Enqueue(str);
    }
    /// <summary>
    /// �Ͽ���������
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
