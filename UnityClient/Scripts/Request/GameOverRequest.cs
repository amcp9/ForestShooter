using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class GameOverRequest : BaseRequest 
{
    private GamePanel gamePanel;

    public override void Awake()
    {
        actionCode = ActionCode.GameOver;
        gamePanel = GetComponent<GamePanel>();
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        gamePanel.OnGameOverResponse(returnCode);
    }
}
