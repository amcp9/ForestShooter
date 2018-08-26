using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class RegisterRequest : BaseRequest {
    private RegisterPanel registerPanel;
    public override void Awake()
    {
        request = RequestCode.User;
        actionCode = ActionCode.Register;
        registerPanel = GetComponent<RegisterPanel>();
        base.Awake();
    }
    public void SendRequest(string name, string pwd)
    {
        string data = name + "," + pwd;
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        registerPanel.OnRegisterResponse(returnCode);
    }
}
