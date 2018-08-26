using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using MySql.Data.MySqlClient;
using War_Server.Tools;
using Common;
using War_Server.Model;


namespace War_Server.Servers
{
// 服务器处理收发数据
    public class Client
    {

        private Socket clientSocket;//持有与服务器的Socket的引用
        private Server server;//持有Server对象的引用
        private Message msg = new Message();
        private MySqlConnection mySqlConnection;//每个Client持有一个MySQL连接
        private User user;
        private Result result;
        private Room room;//持有一个当前Client所在的房间 

        public MySqlConnection GetMySqlConnection
        {
            get { return mySqlConnection; }
        }

        public void SetUserData(User user,Result result)
        {
            this.user = user;
            this.result = result;
        }
        public string GetUserData()
        {
            return user.Id + "," + user.Username + "," + result.TotalCount + "," + result.WinCount;
        }
        public string GetUserName()
        {
            return user.Username;
        }
        public int GetUserId()
        {
            return user.Id;
        }
        public Room Room
        {
            set { room = value; }
            get { return room; }
        }

        public delegate void closeDel(Client client);
        public event closeDel OnClose;

        public Client() { }

        public Client(Socket clientSocket, Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
            mySqlConnection = ConnectionHelper.Connect();//完成一个MySQL连接
        }

        //开始接受数据
        public void Start()
        {
            if (clientSocket == null || !clientSocket.Connected)
            {
                Console.WriteLine(clientSocket.RemoteEndPoint + "已关闭连接");
                Close();
                return;
            }
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
        }

        //异步接受Server发过来的数据
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                if (clientSocket == null || !clientSocket.Connected)
                {
                    Console.WriteLine(clientSocket.RemoteEndPoint + "已关闭连接");
                    Close();
                    return;
                }
                int receiveCount = clientSocket.EndReceive(ar);
                Console.WriteLine("已经接受字节数：" + receiveCount);
                if (receiveCount == 0)
                {
                    //处理非正常关闭
                    Close();
                    return;
                }
                //处理正确接受数据,回调响应函数
                msg.ReadMessage(receiveCount, OnProcessMessage);
                Console.WriteLine("已经正确ReadMessage");
                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("ReceiveCallback Error: " + e.Message);
                Close();
            }
        }
        //解析出来数据的时候调用对应的Controller
        private void OnProcessMessage(RequestCode request, ActionCode action, string data)
        {
            server.HandleRequest(request, action, data, this);
        }
        private void Close()
        {
            ConnectionHelper.CloseConnection(mySqlConnection);//关闭MySQL连接

            //关闭房间
            if (room != null)
            {
                room.CloseRoom(this);
            }
            if (clientSocket != null)
            {
                //发布事件
                OnClose(this);
                clientSocket.Close();
            }

        }

        public void Send(ActionCode actionCode, string data)
        {
            try
            {
                byte[] bytes = Message.PackData(actionCode, data);
                clientSocket.Send(bytes);
            }
            catch(Exception e)
            {
                Console.WriteLine("cannot send message:" + e.Message);
            }

        }

        public bool IsHostClient()
        {
            return room.isHostClient(this);
        }

    }
}

