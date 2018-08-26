using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RoomItem : MonoBehaviour {
    public Text nameText, totalText, winText, winRateText;
    public Button joinButton;

    private RoomListPanel roomListPanel;
    private int id;

    private void Start()
    {
        joinButton.onClick.AddListener(OnJoinButtonClick);
    }
    public void SetRoomItem(RoomListPanel panel,int id,string name,int total,int win)
    {
        roomListPanel = panel;
        this.id = id;
        nameText.text = name;
        totalText.text = "总场次\n" + total.ToString();
        winText.text = "胜率\n" + win.ToString();
        double percent = Convert.ToDouble(win) / Convert.ToDouble(total);
        string result = string.Format("{0:0%}", percent);
        winRateText.text = "胜率\n" + result;

    }
    //调用父级panel的加入请求,以房主userID作为主键
    private void OnJoinButtonClick()
    {
        roomListPanel.OnJoinClickRequest(id);
    }

    public void DestroyItem()
    {
        Debug.Log("Has been Destroy one Item");
        Destroy(this.gameObject);
    }
}
