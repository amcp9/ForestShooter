using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class CountTimeRequest : BaseRequest {
    
    private GamePanel gamePanel;

    public override void Awake()
    {
        actionCode = ActionCode.CountTime;
        gamePanel = GetComponent<GamePanel>();
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        int time = int.Parse(data);
        gamePanel.ShowTimerOnMain(time);
    }
}
