using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;


public class StartGameRequest : BaseRequest {
    private RoomPanel roomPanel;
    public override void Awake()
    {
        request = RequestCode.Game;
        actionCode = ActionCode.StartGame;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest(" ");
    }

    public override void OnResponse(string data)
    {
        string[] s = data.Split('-');
        ReturnCode returnCode = (ReturnCode)int.Parse(s[0]);
        if (returnCode == ReturnCode.Fail) return;
        string type = s[1];
        switch(type)
        {
            case "Guest":
                facade.SetCurrentRoleType(1);
                facade.SetRemoteRoleType(0);
                break;
            case "Host":
                facade.SetCurrentRoleType(0);
                facade.SetRemoteRoleType(1);
                break;
        }
        roomPanel.OnStartRequestResponse(returnCode);
    }
}
