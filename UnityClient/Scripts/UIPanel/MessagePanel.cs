using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : BasePanel {

    private Text text;
    private string message = null;
    private void Update()
    {
        if(message != null)
        {
            ShowMessage(message);
            message = null;
        }
    }
    public override void OnEnter()
    {
        base.OnEnter();
        text = GetComponent<Text>();
        text.enabled = false;
        //让UImanager获得对自己的引用
        uIManager.InjectMsgPanel(this);
    }
    public void ShowMessageOnMain(string msg)
    {
        message = msg;
    }
    public void ShowMessage(string s)
    {
        text.CrossFadeAlpha(1, 0.2f, false);
        text.text = s;
        text.enabled = true;
        Invoke("Hide", 1f);
    }

    private void Hide()
    {
        text.CrossFadeAlpha(0, 1, false);
    }

    public void OnResponse(string data)
    {
        uIManager.ShowMessageOnMain(data);
    }
}
