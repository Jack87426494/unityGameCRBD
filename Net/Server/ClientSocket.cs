using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerTcp
{
    internal class ClientSocket
    {
        private Socket socket;
        //private EndPoint endPoint;

        public ClientSocket(Socket socket/*,EndPoint endPoint*/)
        {
            this.socket = socket;
            //this.endPoint = endPoint;

        }

        /// <summary>
        /// 是否是连接状态
        /// </summary>
        public bool Connected => socket.Connected;

        /// <summary>
        /// 关闭socket
        /// </summary>
        public void Close()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            socket = null;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="str"></param>
        public void Send(string str)
        {
            try
            {
                socket.Send(Encoding.UTF8.GetBytes(str));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Close();
            }

        }

        /// <summary>
        /// 接受信息
        /// </summary>
        public void Receive()
        {
            
            try
            {
                byte[] bytes = new byte[1024 * 10];
                int messageLength = socket.Receive(bytes);
               
                ThreadPool.QueueUserWorkItem((obj) =>
                {
                    (Socket socket, string str) info = ((Socket socket, string str))obj;
                    Console.WriteLine("客户端{0}发送信息：{1}", info.socket.RemoteEndPoint, info.str);
                }, (socket, Encoding.UTF8.GetString(bytes,0,messageLength)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Close();
            }
        }
    }
}
