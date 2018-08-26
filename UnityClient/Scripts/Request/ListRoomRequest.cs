using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class ListRoomRequest : BaseRequest {
    private RoomListPanel roomListPanel;
    public override void Awake()
    {
        request = RequestCode.Room;
        actionCode = ActionCode.RoomList;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }
    public override void SendRequest()
    {
        base.SendRequest("null");
    }
    public override void OnResponse(string data)
    {
        if (data == "0")
        {
            roomListPanel.EmptyRoomItemOnMain();
            return; 
        }
        List<UserData> userDatas = new List<UserData>();
        string[] userHosts = data.Split('|');
        foreach(string host in userHosts)
        {
            string[] strs = host.Split(',');
            userDatas.Add(new UserData(int.Parse(strs[0]),strs[1], int.Parse(strs[2]), int.Parse(strs[3])));
        }
        roomListPanel.LoadRoomItemAsync(userDatas);
    }
}
