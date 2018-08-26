using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class UpdatePlayerInfoRequest : BaseRequest
{
    public PlayerManager playerManager;

    public override void Awake()
    {
        request = RequestCode.Game;
        actionCode = ActionCode.UpdatePlayerInfo;
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("asd");
    }

    // 获得服务器应答数据 
    // 数据格式:蓝方数据|红方数据
    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        playerManager.UpdatePlayerRes(strs[0], strs[1]);
    }
}
