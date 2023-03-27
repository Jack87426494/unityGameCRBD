using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using System.Net.Http.Headers;
using System.Threading;


internal class Program
{
   
    private static void Main(string[] args)
    {

        UseTcpServer();
        
    }
    static List<Socket> socketsList=new List<Socket>();

    static Socket clientSocket;

    static Socket socket;

    static bool isClose;

    public static void UseTcpServer()
    {
        
        //建立套接字
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //绑定ip和端口号
        try
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            socket.Bind(iPEndPoint);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return;
        }
        //用listen方法监听
        socket.Listen(1024);
       
        Thread acceptTread = new Thread(() =>
        {

            while(!isClose)
            {
                clientSocket = socket.Accept();
                //等待客户连接
                socketsList.Add(clientSocket);
                clientSocket.Send(Encoding.UTF8.GetBytes("欢迎连接服务器"+socket.LocalEndPoint?.ToString()));
            }
           
        });
        acceptTread.Start();

        Thread receiveTread = new Thread(() =>
        {
            int i;
            byte[] bytes = new byte[1024 * 1024];
            int messageLength;
            while(!isClose)
            {
                for(i=0;i<socketsList.Count;++i)
                {
                    if(socketsList[i].Available>0)
                    {
                        messageLength = socketsList[i].Receive(bytes);
                        ThreadPool.QueueUserWorkItem((message) =>
                        {
                            (Socket socket, string str) info = ((Socket socket, string str))message;
                            Console.WriteLine("收到{0}:消息{1}", info.socket.RemoteEndPoint, info.str);
                        },(socketsList[i], Encoding.UTF8.GetString(bytes, 0, messageLength)));
                    }
                }
            }
        });
        receiveTread.Start();

        while(true)
        {
            string input=Console.ReadLine();
            //关闭socket
            if (input == "Q")
            {
                for (int i = 0; i < socketsList.Count; ++i)
                {
                    socketsList[i].Shutdown(SocketShutdown.Both);
                    socketsList[i].Close();
                    socketsList.Clear();

                }
            }
            else if(input.Substring(0,2)=="B:")
            {
                for (int i = 0; i < socketsList.Count; ++i)
                {
                    socketsList[i].Send(Encoding.UTF8.GetBytes(input.Substring(2)));
                }
            }
        }
        
    }
}