using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class GoBattleRequest : BaseRequest {
    private RoomPanel roomPanel;
    public override void Awake()
    {
        roomPanel = GetComponent<RoomPanel>();
        actionCode = ActionCode.GoBattle;
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        base.OnResponse(data);
        string[] s = data.Split('-');
        GameFacade.Instance.SetPlayerName(s[0], s[1]);
        roomPanel.OnGoBattleResponseOnMain();
    }

}
