using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class ShowMessageRequest : BaseRequest {
    private MessagePanel messagePanel;
    public override void Awake()
    {
        actionCode = ActionCode.Tips;
        messagePanel = GetComponent<MessagePanel>();
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        messagePanel.OnResponse(data);
    }
}
