using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Common;

public class JoinRoomRequest : BaseRequest {
    private RoomListPanel roomListPanel;
    public override void Awake()
    {
        request = RequestCode.Room;
        actionCode = ActionCode.JoinRoom;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }

    public void SendRequest(int id)
    {
        base.SendRequest(id.ToString());
    }

    public override void OnResponse(string data)
    {
        string[] s = data.Split('-');
        ReturnCode result = (ReturnCode)int.Parse(s[0]);
        UserData u1 = null, u2 = null;
        if (result == ReturnCode.Success)
        {
            string[] users = s[1].Split('|');
            u1 = new UserData(users[0]);
            u2 = new UserData(users[1]);
        }
        else return ;
        roomListPanel.OnJoinResponse(result, u1, u2);
    }

}
