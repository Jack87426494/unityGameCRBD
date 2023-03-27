using ServerTcp;
using System.Net;

internal class Program
{
    private static void Main(string[] args)
    {
        ServerSocket serverSocket = new ServerSocket("127.0.0.1", 8080, 1024);
        //Console.WriteLine("等待客户端连接");
        serverSocket.Accept();
        
        while (true)
        {
            
            //string input = Console.ReadLine();

            //if (input == "E")
            //{
            //    //serverSocket.Receive(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 60435));

            //}
            //serverSocket.ReceiveAll();
            //if (input == "Q")
            //{
            //    serverSocket.Close();
            //}
            //else if (input.Substring(0, 2) == "B:")
            //{
            //    serverSocket.Broadcast(input.Substring(2));
            //}
            
        }
        
       
    }
}