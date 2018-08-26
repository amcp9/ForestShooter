using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Common;
using System.Threading;
using War_Server.Stats;

namespace War_Server.Servers
{
    enum RoomState
    {
        WaitingJoin,
        WaitingBattle,
        Battling,
        End

    }
    public class Room
    {
        //表示该房间内客户
        private List<Client> clients = new List<Client>();
        private RoomState state = RoomState.WaitingJoin;
        private Server server;
        public delegate void OnClient(Client c);
        public event OnClient OnClientDie;

        private Dictionary<Client, PlayerInfo> PlayerList;

        public void InitPlayerInfo()
        {
            PlayerList = new Dictionary<Client, PlayerInfo>
            {
                { clients[0], new PlayerInfo(100,100, 1, 1) },
                { clients[1], new PlayerInfo(100,100, 1, 1) }
            };
        }

        public bool IsMaxRampagePoint(Client client)
        {
            if (PlayerList.TryGetValue(client, out PlayerInfo info))
            {
                if (info.GetStat(BaseStatType.SpeedPoint).GetFinalValue() < 100)
                {
                    return false;
                }
                else return true;
            }
            else return false;
        }

        public void ClearRampagePoint(Client client)
        {
            if(PlayerList.TryGetValue(client,out PlayerInfo info))
            {
                info.GetStat(BaseStatType.SpeedPoint).ClearAdditive();
            }
        }

        public void TakeDamage(Client client,int damage)
        {
            Client temp = null;
            foreach(Client c in clients)
            {
                if (c != client)
                    temp = c;
            }
            if (PlayerList.TryGetValue(temp, out PlayerInfo info))
            {
                lock (PlayerList)
                {
                    info.GetStat(BaseStatType.Health).SubBaseValue(damage);
                    if(info.GetStat(BaseStatType.Health).GetFinalValue() <= 10)
                    {
                        OnClientDie(temp);
                        state = RoomState.End;
                    }
                }
            }
        }

        public Client GetHost()
        {
            lock (clients)
            {
                return clients[0];
            }
        }

        public string GetAllPlayerInfo()
        {
            lock (PlayerList)
            {
                //maxhealth - health - movespeed - attackspeed - point | maxhealth - health - movespeed - attackspeed - point
                StringBuilder sb = new StringBuilder();
                if (PlayerList.TryGetValue(clients[0], out PlayerInfo hostInfo))
                {
                    sb.Append(hostInfo.GetStat(BaseStatType.MaxHealth).GetFinalValue().ToString());
                    sb.Append("-");
                    sb.Append(hostInfo.GetStat(BaseStatType.Health).GetFinalValue().ToString());
                    sb.Append("-");
                    sb.Append(hostInfo.GetStat(BaseStatType.MoveSpeed).GetFinalValue().ToString());
                    sb.Append("-");
                    sb.Append(hostInfo.GetStat(BaseStatType.AttackSpeed).GetFinalValue().ToString());
                    sb.Append("-");
                    sb.Append(hostInfo.GetStat(BaseStatType.SpeedPoint).GetFinalValue().ToString());
                }
                if (PlayerList.TryGetValue(clients[1], out PlayerInfo info))
                {
                    sb.Append("|");
                    sb.Append(info.GetStat(BaseStatType.MaxHealth).GetFinalValue().ToString());
                    sb.Append("-");
                    sb.Append(info.GetStat(BaseStatType.Health).GetFinalValue().ToString());
                    sb.Append("-");
                    sb.Append(info.GetStat(BaseStatType.MoveSpeed).GetFinalValue().ToString());
                    sb.Append("-");
                    sb.Append(info.GetStat(BaseStatType.AttackSpeed).GetFinalValue().ToString());
                    sb.Append("-");
                    sb.Append(info.GetStat(BaseStatType.SpeedPoint).GetFinalValue().ToString());
                    return sb.ToString();
                }
                return null;
            }
        }

        public void AddOneInfo(Client client,BaseStatType type,int value)
        {
            if (PlayerList.TryGetValue(client, out PlayerInfo info))
            {
                lock (PlayerList)
                {
                    info.GetStat(type).AddExtraValue(new StatBonus(value));
                    Console.WriteLine("增加玩家[" + client.GetUserName() + "]的[" + type.ToString() + "]属性" + value.ToString() + "成功");
                }
            }
            else return;
        }

        public void RemoveOneInfo(Client client, BaseStatType type, int value)
        {
            if (PlayerList.TryGetValue(client, out PlayerInfo info))
            {
                lock (PlayerList)
                {
                    info.GetStat(type).RemoveExtraValue(new StatBonus(value));
                    Console.WriteLine("移除玩家[" + client.GetUserName() + "]的[" + type.ToString() + "]属性" + value.ToString() + "成功");
                }
            }
            else return;
        }

        public bool IsEndRoom()
        {
            return state == RoomState.End;
        }


        public Room(Server server)
        {
            this.server = server;
        }
        public bool IsWaitingJoinRoom()
        {
            return (state == RoomState.WaitingJoin);
        }

        //往Room里面添加Client
        public void AddClient(Client client)
        {
            //thie first one is the host
            lock(clients)
            {
                clients.Add(client);
            }
            client.Room = this;
            if(clients.Count >= 2)
            {
                state = RoomState.WaitingBattle;
            }
        }

        public void RemoveClient(Client client)
        {
            client.Room = null;
            lock(clients)
            {
                clients.Remove(client);
            }
            if (clients.Count >= 2)
            {
                state = RoomState.Battling;
            }
            else
            {
                state = RoomState.WaitingJoin;
            }
        }

        //获得房主信息
        public string GetHostInfo()
        {
            return clients[0].GetUserData();
        }

        //获得房主id，依据id分辨房间
        public int GetHostUserId()
        {
            if (clients.Count > 0)
            {
                return clients[0].GetUserId();
            }
            else { return -1; }
        }

        //获得房间内所有人的信息
        public string GetRoomData()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach(Client c in clients)
            {
                stringBuilder.Append(c.GetUserData() + "|");
            }
            if(stringBuilder.Length >0 )
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }
            return stringBuilder.ToString();
        }

        //从Server里面移除这个对象，关闭房间
        public void CloseRoom(Client client)
        {
            //判断是否是房主退出房间
            if (client == clients[0])
            {
                if(state == RoomState.Battling)
                {
                    BoardCastMessage(client, ActionCode.Tips, "玩家:" + client.GetUserName() + "已退出房间");
                    foreach(Client c in clients)
                    {
                        c.Room = null;
                    }
                    clients.Clear();
                }
                server.RemoveRoom(this);

            }else
            {
                clients.Remove(client);
                client.Room = null;
            }
            Console.WriteLine("已调用CloseRoom 当前已存房间总数为：" + server.GetRooms().Count);
        }

        public void SendMessage(Client client,ActionCode action,string data)
        {
            server.ResponseMsg(client, action, data);
        }

        public void BoardCastRoomInfo(Client client,string data)
        {
            foreach(Client c in clients)
            {
                if(c != client)
                {
                    server.ResponseMsg(c, ActionCode.JoinRoom, data);
                }
            }
        }
        public void BoardCastMessage(Client client,ActionCode action,string data)
        {
            foreach(Client c in clients)
            {
                if(c != client)
                {
                    server.ResponseMsg(c, action, data);
                }
            }
        }
        public void BoardCastOneExit(Client client,string data)
        {
            foreach(Client c in clients)
            {
                if (c != client)
                {
                    server.SystemChatToOneClient("玩家[" + client.GetUserName() + "]已退出房间", c);//给剩余玩家发送提示
                    server.ResponseMsg(c, ActionCode.UpdateRoom, data);
                }
            }
        }

        public bool isHostClient(Client client)
        {
            if (client != clients[0])
            {
                return false;
            }
            else return true;
        }

        public void ShowTimer()
        {
            Thread r = new Thread(RunTime);
            r.Start();
        }

        private void RunTime()
        {
            Thread.Sleep(1000);
            for (int i = 3; i > 0;i--)
            {
                BoardCastMessage(null, ActionCode.CountTime, i.ToString());
                Thread.Sleep(1000);
            }
            BoardCastMessage(null, ActionCode.GoBattle, clients[0].GetUserName()+"-"+clients[1].GetUserName());
            //BoardCastMessage(null, ActionCode.UpdatePlayerInfo, GetAllPlayerInfo());
            state = RoomState.Battling;
        }

        public void ClearRoom()
        {
            foreach(Client c in clients)
            {
                c.Room = null;
            }
            clients.Clear();
            server.RemoveRoom(this);
        }
    }
}
