using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class AttackRequest : BaseRequest 
{
    public override void Awake()
    {
        request = RequestCode.Game;
        actionCode = ActionCode.TakeDamage;
        base.Awake();
    }

    public void SendRequest(int damage)
    {
        base.SendRequest(damage.ToString());
    }
}
