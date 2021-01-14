using System;
using System.IO;
using System.Drawing.Imaging;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*************************************************서버를 시작합니다*************************************************");
            mSocket server = new mSocket();
            server.StartServer();
            Console.ReadLine();
        }
    }
}
