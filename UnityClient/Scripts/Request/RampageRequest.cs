using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class RampageRequest : BaseRequest 
{
    public PlayerManager playerManager;

    public override void Awake()
    {
        request = RequestCode.Game;
        actionCode = ActionCode.Rampage;
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("asd");
    }

    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        switch(returnCode)
        {
            case ReturnCode.Remote:
                playerManager.RemoteRampage();
                break;
            case ReturnCode.Success:
                playerManager.CurrentRampage();
                break;
            case ReturnCode.Fail:
                break;
        }
    }
}
