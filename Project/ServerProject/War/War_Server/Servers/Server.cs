using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using War_Server.Controller;
using Common;


namespace War_Server.Servers
{
    public class Server
    {
        private IPEndPoint point;
        private Socket serverSocket;
        private List<Client> clients = new List<Client>();
        private List<Room> rooms = new List<Room>();
        private ControllerManager controllerManager;

        public List<Client> Clients 
        {
            get { return clients; }
        }

        public Server()
        {
        }
        public Server(string s,int p)
        {
            controllerManager = new ControllerManager(this);
            SetIpPoint(s, p);
        }

        public void SetIpPoint(string s,int p)
        {
            point = new IPEndPoint(IPAddress.Parse(s), p);
        }

        //当服务器开启后，应当执行以下操作：
        //1：绑定端点
        //2：设置监听队列
        //3：异步开启答应请求
        public void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(point);
            serverSocket.Listen(10);
            serverSocket.BeginAccept(AcceptCallBack, null);
        }
        private void AcceptCallBack(IAsyncResult ar)
        {
            Socket clientSocket = serverSocket.EndAccept(ar);
            Client client = new Client(clientSocket,this);
            Console.WriteLine(clientSocket.RemoteEndPoint + "has been connected");
            client.Start();
            clients.Add(client);
            client.OnClose += ClientClose;
            serverSocket.BeginAccept(AcceptCallBack, null);
        }

        private void ClientClose(Client client)
        {
            lock(clients)
            {
                foreach(Client c in clients)
                {
                    SystemChatToOneClient("玩家[" + client.GetUserName() + "]已退出游戏", c);
                }
                clients.Remove(client);
            }
        }


        //响应客户端的Request,由Client调用
        public void ResponseMsg(Client client,ActionCode actionCode,string s)
        {
            Console.WriteLine("回应客户请求，action：" + (ActionCode)actionCode + "data:" + s);
            client.Send(actionCode, s);
        }
        public void HandleRequest(RequestCode request, ActionCode action, string data, Client client)
        {
            controllerManager.HandleRequest(request, action, data, client);
        }
        public void CreateRoom(Client client)
        {
            Room room = new Room(this);
            room.AddClient(client);
            rooms.Add(room);
        }
        public List<Room> GetRooms()
        {
            return rooms;
        }
        public void RemoveRoom(Room room)
        {
            if (rooms != null && room != null)
            {
                lock (rooms)
                {
                    rooms.Remove(room);
                }
            }
        }
        public Room GetRoomByHostId(int id)
        {
            foreach(Room r in rooms)
            {
                if (r.GetHostUserId() == id) return r;
            }
            return null;
        }

        public void SystemChat(string data)
        {
            ChatController chatController = controllerManager.GetController(RequestCode.Chat) as ChatController;
            chatController.SystemChat(data, this);
        }

        public void SystemChatToOneClient(string data,Client client)
        {
            ChatController chatController = controllerManager.GetController(RequestCode.Chat) as ChatController;
            chatController.SystemChatToOneClient(client, data, this);
        }
    }
}
