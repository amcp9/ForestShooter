using System;
using Common;
using War_Server.Servers;
using System.Text;

namespace War_Server.Controller
{
    public class ChatController:BaseController
    {
        public ChatController()
        {
            request = RequestCode.Chat;
        }

        public void ChatPublic(string data, Client client, Server server)
        {
            //"Player-name-data"
            string newData = ((int)ChatCode.Player).ToString()+"-"+client.GetUserName() + "-" + data;
            //转为房间聊天
            if(client.Room != null)
            {
                client.Room.BoardCastMessage(null, ActionCode.ChatPublic, newData);
                return;
            }
            //转为公共聊天
            foreach(Client c in server.Clients)
            {
                server.ResponseMsg(c, ActionCode.ChatPublic, newData);
            }
        }

        public void SystemChat(string data,Server server)
        {
            //"System-data"
            string newdata = ((int)ChatCode.System).ToString() +"-"+data;
            foreach (Client c in server.Clients)
            {
                server.ResponseMsg(c, ActionCode.ChatPublic, newdata);
            }
        }

        public void SystemChatToOneClient(Client client,string data,Server server)
        {
            string newData = ((int)ChatCode.System).ToString() + "-" + data;
            server.ResponseMsg(client, ActionCode.ChatPublic, newData);
        }

    }
}
