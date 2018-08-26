using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class LoginRequest : BaseRequest {
    private LoginPanel loginPanel;
	// Use this for initialization
    public override void Awake ()
    {
        request = RequestCode.User;
        actionCode = ActionCode.Login;
        loginPanel = GetComponent<LoginPanel>();
        base.Awake();
	}

    public void SendRequest(string name,string pwd)
    {
        string data = name + "," + pwd;
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        if(returnCode == ReturnCode.Success)
        {
            string userName = strs[1];
            int total = int.Parse(strs[2]);
            int win = int.Parse(strs[3]);
            UserData userData = new UserData(userName, total, win);
            facade.SetUserData(userData);
        }
        loginPanel.OnLoginResponse(returnCode);
    }

}
