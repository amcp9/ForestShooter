using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class GainInfoRequest : BaseRequest 
{
    public override void Awake()
    {
        request = RequestCode.Game;
        actionCode = ActionCode.GainInfo;
        base.Awake();
    }

    public void SendMessage(BaseStatType type,int value)
    {
        string s = ((int)type).ToString() + "-" + value.ToString();
        base.SendRequest(s);
    }
}
