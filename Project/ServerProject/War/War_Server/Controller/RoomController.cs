using System;
using Common;
using War_Server.Servers;
using System.Text;
namespace War_Server.Controller
{
    public class RoomController : BaseController
    {
        public RoomController()
        {
            request = RequestCode.Room;
        }

        //处理客户端创建房间请求
        public string CreateRoom(string data,Client client,Server server)
        {
            try
            {
                server.CreateRoom(client);
            }
            catch(Exception e)
            {
                Console.WriteLine("CreateRoom Exception:" + e.Message);
                return ((int)ReturnCode.Fail).ToString();
            }
            Console.WriteLine("已调用CreatRoom 当前已存房间总数为：" + server.GetRooms().Count);
            return ((int)ReturnCode.Success).ToString();
        }

        //处理客户端获取房间列表请求
        public string RoomList(string data,Client client,Server server)
        {
            StringBuilder stringBuilder = new StringBuilder();
            //遍历Server中的所有等待加入中的room
            foreach(var room in server.GetRooms())
            {
                if(room.IsWaitingJoinRoom())
                {
                    stringBuilder.Append(room.GetHostInfo() + "|");
                }
            }
            if (stringBuilder.Length == 0)
            {
                stringBuilder.Append("0");
            }
            else
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }
            return stringBuilder.ToString();
        }

        //处理客户端加入某房间请求
        public string JoinRoom(string data,Client client,Server server)
        {
            int id = int.Parse(data);
            Room room = server.GetRoomByHostId(id);

            if (room == null)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            if (room.IsWaitingJoinRoom() == false)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                //成功通过ID获取房间
                lock (room)
                {
                    room.AddClient(client);
                    string roomData = room.GetRoomData();                                                       //格式"ReturnCode-id,name,tc,wc|id,name,tc,wc"
                    server.SystemChatToOneClient("玩家[" + client.GetUserName() + "]已加入房间", room.GetHost());//给房主发送提示
                    room.BoardCastRoomInfo(client, ((int)ReturnCode.Success).ToString() + "-" + roomData);    //给其他玩家广播加入应答
                    return ((int)ReturnCode.Success).ToString() + "-" + roomData;
                }

            }
        }

        //处理退出房间请求
        public string ExitRoom(string data,Client client,Server server)
        {
            //退出者为房主时
            if(client.IsHostClient())
            {
                if(client.Room.IsWaitingJoinRoom())
                {
                    //当房主退出无玩家的房间
                    server.RemoveRoom(client.Room);
                    client.Room = null;
                }else
                {
                    //当房主退出满员房间
                    client.Room.BoardCastOneExit(client, ((int)ReturnCode.Success).ToString());
                    client.Room.RemoveClient(client);
                }
                return (((int)ReturnCode.Success).ToString());
            }
            else//退出者不是房主时
            {
                //向其他玩家更新房间信息
                client.Room.BoardCastOneExit(client,((int)ReturnCode.Success).ToString());
                client.Room.RemoveClient(client);
                //返回该玩家的退房应答
                return (((int)ReturnCode.Success).ToString());
            }
        }
    }

}
