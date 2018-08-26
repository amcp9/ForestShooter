using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class ChatRequest : BaseRequest 
{
    private ChatPanel chatPanel;
    public override void Awake()
    {
        request = RequestCode.Chat;
        actionCode = ActionCode.ChatPublic;
        chatPanel = GetComponent<ChatPanel>();
        base.Awake();
    }

    public void Send(string data)
    {
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split('-');
        ChatCode chatCode = (ChatCode)int.Parse(strs[0]);
        switch(chatCode)
        {
            case ChatCode.Player:
                chatPanel.PlayerChatOnMain(strs[1], strs[2]);
                break;
            case ChatCode.System:
                chatPanel.SystemChatOnMain(strs[1]);
                break;
        }
    }
}
