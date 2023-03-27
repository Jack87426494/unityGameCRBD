using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ServerTcp
{
    internal class ServerSocket
    {
        private Socket socket;

        private Dictionary<IPEndPoint, ClientSocket> clientSocketDic = new Dictionary<IPEndPoint, ClientSocket>();
        public ServerSocket(string ipStr, int port, int listenNum)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
                socket.Bind(iPEndPoint);
                socket.Listen(listenNum);
            //Accept();

        }
        /// <summary>
        /// 是否是连接状态
        /// </summary>
        public bool Connected => socket.Connected;


        private Socket tmp_clientSocket;
        /// <summary>
        /// 接受登录
        /// </summary>
        public void Accept()
        {
            try
            {
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    tmp_clientSocket = socket.Accept();
                    ClientSocket clientSocket = new ClientSocket(tmp_clientSocket);
                    clientSocketDic.Add((IPEndPoint)tmp_clientSocket.RemoteEndPoint, clientSocket);
                    clientSocket.Send("欢迎登录服务器");
                    Console.WriteLine(tmp_clientSocket.RemoteEndPoint + ":登录服务器");
                });
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

        }

        /// <summary>
        /// 接收所有消息
        /// </summary>
        public void ReceiveAll()
        {
            
            foreach (ClientSocket clientSocket in clientSocketDic.Values)
            {
                
                clientSocket.Receive();
            }
        }
        /// <summary>
        /// 接收某个客户端发来的消息
        /// </summary>
        public void Receive(IPEndPoint iPEndPoint)
        {
            if (clientSocketDic.ContainsKey(iPEndPoint));
            clientSocketDic[iPEndPoint].Receive();
        }

        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="str">消息的内容</param>
        public void Broadcast(string str)
        {
            foreach (ClientSocket clientSocket in clientSocketDic.Values)
            {
                clientSocket.Send(str);
            }
        }
        /// <summary>
        /// 向某个客户端发送消息
        /// </summary>
        /// <param name="iPEndPoint"></param>
        /// <param name="str"></param>
        public void Send(IPEndPoint iPEndPoint, string str)
        {
            if (clientSocketDic.ContainsKey(iPEndPoint))
                clientSocketDic[iPEndPoint].Send(str);
        }

        public void Close()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            socket = null;
        }
    }
}
