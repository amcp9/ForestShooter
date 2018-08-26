using System;
using System.Collections.Generic;
using System.Collections;
using War_Server.Servers;

namespace War_Server
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Server server = new Server("192.168.192.107", 12345);
            server.Start();
            Console.WriteLine("服务器已启动...");

            Console.ReadKey();
        }
    }
}
