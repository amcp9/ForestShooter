using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using Common;

public class RoomPanel : BasePanel {
    public Text localUserName;
    public Text localTotalCount;
    public Text localWinCount;
    public Text localWinRate;

    public Text remoteUserName;
    public Text remoteTotalCount;
    public Text remoteWinCount;
    public Text remoteWinRate;

    public Button starButton;
    public Button exitButton;

    public Transform bluePanel;
    public Transform redpanel;
    public Transform startBtn;
    public Transform exitBtn;

    private UserData userData1 = null;
    private UserData userData2 = null;

    private ExitRoomRequest exitRoomRequest;
    private StartGameRequest startGameRequest;

    private bool isNeedStart = false;
    private bool isNeedAddScripts = false;

	void Start () {
        starButton.onClick.AddListener(OnStartButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
        exitRoomRequest = GetComponent<ExitRoomRequest>();
        startGameRequest = GetComponent<StartGameRequest>();
	}
	void Update () {
        if(userData1!=null && userData2 == null)
        {
            ClearRemoteUserInfo();
            SetLocalUserInfo(userData1.userName, userData1.totalCount, userData1.winCount);
            startBtn.gameObject.SetActive(true);
            userData1 = null;
        }

        if(userData1 != null && userData2 != null)
        {
            ClearRemoteUserInfo();
            SetLocalUserInfo(userData1.userName, userData1.totalCount, userData1.winCount);
            SetRemoteUserInfo(userData2.userName, userData2.totalCount, userData2.winCount);
            userData1 = null;
            userData2 = null;
        }
        if(isNeedStart)
        {
            facade.InsPlayer();
            facade.EnableScripts();
            facade.CreateSyncGO();
            isNeedStart = false;
        }
        if(isNeedAddScripts)
        {
            OnGoBattleResponse();
            isNeedAddScripts = false;
        }
	}

    public void SetAllUserDataOnMain(UserData user1,UserData user2)
    {
        this.userData1 = user1;
        this.userData2 = user2;
    }

    //处理创建房间
    public void SetLocalUserInfoOnMain(UserData user)
    {
        userData1 = user;
    }

    public void OnOneClientExit(UserData data)
    {
        
        userData1 = data;
        userData2 = null;
    }

    private void SetLocalUserInfo(string username,int total,int win)
    {
        localUserName.text = username;
        localTotalCount.text = "总场次:"+ total.ToString();
        localWinCount.text = "胜利:" + win.ToString();
        double percent = Convert.ToDouble(win) / Convert.ToDouble(total);
        localWinRate.text = string.Format("{0:0%}", percent);
    }
    private void SetRemoteUserInfo(string username, int total, int win)
    {
        remoteUserName.text = username;
        remoteTotalCount.text = "总场次:" + total.ToString();
        remoteWinCount.text = "胜利:" + win.ToString();
        double percent = Convert.ToDouble(win) / Convert.ToDouble(total);
        remoteWinRate.text = string.Format("{0:0%}", percent);
    }
    private void ClearRemoteUserInfo()
    {
        remoteUserName.text = "Waiting...";
        remoteTotalCount.text = "";
        remoteWinCount.text = "";
        remoteWinRate.text = "";
    }
    private void OnStartButtonClick()
    {
        startGameRequest.SendRequest();
    }
    private void OnExitButtonClick()
    {
        exitRoomRequest.SendRequest();
    }

    public override void OnEnter()
    {
        //gameObject.SetActive(true);
        bluePanel.localPosition = new Vector3(-1000, 20, 0);
        redpanel.localPosition = new Vector3(1000, 20, 0);
        bluePanel.DOLocalMoveX(-120, 0.4f, true);
        redpanel.DOLocalMoveX(90, 0.4f, true);
        startBtn.localScale = Vector3.zero;
        exitBtn.localScale = Vector3.zero;
        startBtn.DOScale(1, 0.4f);
        exitBtn.DOScale(1, 0.4f);
    }

    public void OnExitResponse()
    {
        uIManager.PopPanelOnMain();
        uIManager.PushPanelOnMain(UIPanelType.RoomList);
    }

    public void OnStartRequestResponse(ReturnCode returnCode)
    {
        if(returnCode == ReturnCode.Fail)
        {
            uIManager.ShowMessageOnMain("开始游戏失败");
        }
        else
        {
            uIManager.PopPanel();
            uIManager.PushPanelOnMain(UIPanelType.Game);
            isNeedStart = true;
        }
    }

    public override void OnExit()
    {
        bluePanel.DOLocalMoveX(-1000, 0.4f);
        redpanel.DOLocalMoveX(-1000,0.4f);
        startBtn.DOScale(0, 0.4f);
        exitBtn.DOScale(0, 0.4f);
       // gameObject.SetActive(false);
    }

    public void OnGoBattleResponseOnMain()
    {
        isNeedAddScripts = true;
    }

    public void OnGoBattleResponse()
    {
        //facade.EnableScripts();
        //facade.CreateSyncGO();
        uIManager.PushPanel(UIPanelType.HealthBar);
        HealthBarPanel panel = uIManager.PeekStack() as HealthBarPanel;
        GameFacade.Instance.SetHealthBarPanel(panel);
    }
}
