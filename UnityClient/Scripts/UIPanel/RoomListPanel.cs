using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;

public class RoomListPanel : BasePanel
{
    public Button closeButton, refleshButton, createRoomButton;
    public Text userName, total, win, winRate;
    public VerticalLayoutGroup layoutGroup;

    private GameObject roomItem;
    private List<UserData> u = null;
    private bool needEmpty = false;

    private ListRoomRequest listRequest;
    private JoinRoomRequest joinRoomRequest;
    private CreateRommRequest createRommRequest;

    private RoomPanel roomPanel;

    private void Awake()
    {
        listRequest = GetComponent<ListRoomRequest>();
        joinRoomRequest = GetComponent<JoinRoomRequest>();
        createRommRequest = GetComponent<CreateRommRequest>();
    }
    void Start()
    {
        closeButton.onClick.AddListener(OnCloseButtonClick);
        refleshButton.onClick.AddListener(OnRefleshBtnClick);
        createRoomButton.onClick.AddListener(OnCreateBtnClick);
        roomItem = Resources.Load("UIPanel/RoomItem") as GameObject;
    }
    private void Update()
    {
        if (u != null)
        {
            LoadRoomItem(u);
            u = null;
        }
        if(needEmpty)
        {
            EmptyRoomItem();
            needEmpty = false;
        }
    }
    public override void OnEnter()
    {
        //closeButton = transform.Find("CloseButton").GetComponent<Button>();
        //userName = transform.Find("UserInfo/UserName").GetComponent<Text>();
        //total = transform.Find("UserInfo/TotalCount").GetComponent<Text>();
        //win = transform.Find("UserInfo/WinCount").GetComponent<Text>();
        //winRate = transform.Find("UserInfo/WinRate").GetComponent<Text>();


        //transform.gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.2f).OnComplete(listRequest.SendRequest);

        //request.SendRequest();
        UserData user = facade.GetUserData();
        userName.text = user.userName;
        total.text = "总场次:" + user.totalCount.ToString();
        win.text = "胜利:" + user.winCount.ToString();
        double percent = Convert.ToDouble(user.winCount) / Convert.ToDouble(user.totalCount);
        string result = string.Format("{0:0%}", percent);
        winRate.text = "胜率:" + result;
        transform.DOScale(1, 0.2f);

    }
    public override void OnExit()
    {
        //transform.DOScale(0, 0.2f).OnComplete(() => transform.gameObject.SetActive(false));
        transform.DOScale(0, 0.2f);
    }
    private void OnCloseButtonClick()
    {
        PlayClickSound();
        uIManager.PopPanel();
        uIManager.PushPanel(UIPanelType.Login);
    }


    public void LoadRoomItemAsync(List<UserData> userDatas)
    {
        u = userDatas;
    }

    public void EmptyRoomItemOnMain()
    {
        needEmpty = true;
    }

    private void EmptyRoomItem()
    {
        //清空原有房间
        RoomItem[] roomItems = layoutGroup.GetComponentsInChildren<RoomItem>();
        foreach (RoomItem r in roomItems)
        {
            r.DestroyItem();
        }
    }

    private void LoadRoomItem(List<UserData> userDatas)
    {
        //清空原有房间
        RoomItem[] roomItems = layoutGroup.GetComponentsInChildren<RoomItem>();
        foreach (RoomItem r in roomItems)
        {
            r.DestroyItem();
        }

        int count = userDatas.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject room = Instantiate(roomItem);
            room.transform.SetParent(layoutGroup.transform);
            room.GetComponent<RoomItem>().SetRoomItem(this, userDatas[i].userId, userDatas[i].userName, userDatas[i].totalCount, userDatas[i].winCount);
        }
        int roomCount = GetComponentsInChildren<RoomItem>().Length;
        Vector2 size = layoutGroup.GetComponent<RectTransform>().sizeDelta;
        layoutGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,
                                                                          roomCount * (roomItem.GetComponent<RectTransform>().sizeDelta.y
                                                                           + layoutGroup.GetComponent<VerticalLayoutGroup>().spacing));
    }

    private void OnRefleshBtnClick()
    {
        listRequest.SendRequest();
    }
    private void OnCreateBtnClick()
    {
        uIManager.PopPanel();
        uIManager.PushPanel(UIPanelType.Room);
        roomPanel = uIManager.PeekStack().GetComponent<RoomPanel>();
        createRommRequest.SendRequest();
    }
    public void OnCreateRequestResponse(UserData user)
    {
        roomPanel.SetLocalUserInfoOnMain(user);
    }
    //请求加入房主ID为ID的房间
    public void OnJoinClickRequest(int id)
    {
        uIManager.PopPanel();
        uIManager.PushPanel(UIPanelType.Room);
        roomPanel = uIManager.PeekStack().GetComponent<RoomPanel>();
        roomPanel.startBtn.gameObject.SetActive(false);
        joinRoomRequest.SendRequest(id);
    }

    public void OnJoinResponse(ReturnCode returnCode, UserData data1, UserData data2)
    {

        if (returnCode == ReturnCode.Success)
        {
            roomPanel.SetAllUserDataOnMain(data1, data2);
        }
        else
        {
            uIManager.ShowMessageOnMain("加入房间失败");
            uIManager.PopPanelOnMain();
            uIManager.PushPanelOnMain(UIPanelType.RoomList);
        }

    }
}



