using System;
using Common;
using System.Collections;
using System.Collections.Generic;
using War_Server.Servers;

namespace War_Server.Controller
{
    public class GameController:BaseController
    {
        public GameController()
        {
            request = RequestCode.Game;
        }



        //start game request
        public string StartGame(string data, Client client, Server server)
        {
            if (!client.IsHostClient()) 
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            if(client.Room.IsWaitingJoinRoom())
            {
                server.ResponseMsg(client, ActionCode.Tips, "人数不足无法开始游戏");
                return null;
            }
            client.Room.InitPlayerInfo();
            client.Room.OnClientDie += ClientDie;
            //for other client
            client.Room.BoardCastMessage(client, ActionCode.StartGame,((int)ReturnCode.Success).ToString() + "-Guest");
            //start timer
            client.Room.ShowTimer();
            //for start client
            return ((int)ReturnCode.Success).ToString() + "-Host";
        }

        public string Move(string data, Client client, Server server)
        {
            Room room = client.Room;
            if(room != null) room.BoardCastMessage(client, ActionCode.Move, data);
            return null;
        }
        public string Shoot(string data, Client client, Server server)
        {
            Room room = client.Room;
            if (room != null) room.BoardCastMessage(client, ActionCode.Shoot, data);
            return null;
        }


        public void ClientDie(Client client)
        {
            //((int)ReturnCode.Success).ToString();
            client.Room.BoardCastMessage(client, ActionCode.GameOver, ((int)ReturnCode.Success).ToString());
            client.Room.SendMessage(client, ActionCode.GameOver, ((int)ReturnCode.Fail).ToString());
            client.Room.ClearRoom();
        }

        public string TakeDamage(string data,Client client,Server server)
        {
            int damage = int.Parse(data);
            client.Room.TakeDamage(client, damage);
            if(!client.Room.IsEndRoom()) SendAllPlayerInfo(client);
            return null;
        }

        public void UpdatePlayerInfo(string data, Client client, Server server)
        {
            SendAllPlayerInfo(client);
        }

        private void SendAllPlayerInfo(Client client)
        {
            Room room = client.Room;
            string newData = room.GetAllPlayerInfo();
            if (room != null) room.BoardCastMessage(null, ActionCode.UpdatePlayerInfo, newData);
        }

        public void GainInfo(string data,Client client,Server server)
        {
            //basetype-value
            string[] strs = data.Split('-');
            BaseStatType type = (BaseStatType)int.Parse(strs[0]);
            client.Room.AddOneInfo(client, type, int.Parse(strs[1]));
            SendAllPlayerInfo(client);
        }

        public string Rampage(string data, Client client, Server server)
        {
            if(client.Room.IsMaxRampagePoint(client))
            {
                if (client.Room != null)
                {
                    client.Room.BoardCastMessage(client, ActionCode.Rampage, ((int)ReturnCode.Remote).ToString());
                }
                client.Room.ClearRampagePoint(client);
                return ((int)ReturnCode.Success).ToString();
            }
            return ((int)ReturnCode.Fail).ToString();
        }
    }


}
