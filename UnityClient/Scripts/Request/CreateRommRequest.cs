using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class CreateRommRequest : BaseRequest {

    private RoomListPanel roomListPanel;

    public override void Awake()
    {
        request = RequestCode.Room;
        actionCode = ActionCode.CreateRoom;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }
    public override void SendRequest()
    {
        base.SendRequest("sad");
    }

    public override void OnResponse(string data)
    {
        if((ReturnCode)int.Parse(data) == ReturnCode.Success)
        {
            UserData user = facade.GetUserData();
            roomListPanel.OnCreateRequestResponse(user);
        }
    }
}
