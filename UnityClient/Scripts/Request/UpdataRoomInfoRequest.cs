using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class UpdataRoomInfoRequest : BaseRequest {
    private RoomPanel roomPanel;
    public override void Awake()
    {
        request = RequestCode.Room;
        actionCode = ActionCode.UpdateRoom;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        //string[] s = data.Split('-');
        //ReturnCode result = (ReturnCode)int.Parse(s[0]);

        if ((ReturnCode)int.Parse(data) == ReturnCode.Success)
        {
            roomPanel.OnOneClientExit(facade.GetUserData());
        }
    }
}
