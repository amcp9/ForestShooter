using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class ExitRoomRequest : BaseRequest {
    private RoomPanel roomPanel;
    public override void Awake()
    {
        request = RequestCode.Room;
        actionCode = ActionCode.ExitRoom;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("asd");
    }

    public override void OnResponse(string data)
    {
        //string[] s = data.Split('-');
        ReturnCode result = (ReturnCode)int.Parse(data);
        if (result == ReturnCode.Success)
        {
            roomPanel.OnExitResponse();
        }
    }
}
