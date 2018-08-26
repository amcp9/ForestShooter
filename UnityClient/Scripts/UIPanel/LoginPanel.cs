using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Common;

public class LoginPanel : BasePanel {
    public Button btn;
    public InputField usernameField;
    public InputField pwdField;
    public Button loginBtn;
    public Button registerBtn;
    private LoginRequest loginRequest;
    private void Start()
    {
        loginRequest = GetComponent<LoginRequest>();

        btn.onClick.AddListener(OnCloseButtonClick);
        loginBtn.onClick.AddListener(OnLoginBtnClick);
        registerBtn.onClick.AddListener(OnRegisterBtnClick);
    }
    //public override void OnEnter()
    //{
    //    base.OnEnter();
    //    btn = transform.Find("CloseButton").GetComponent<Button>();
    //    loginBtn = transform.Find("Login_button").GetComponent<Button>();
    //    registerBtn = transform.Find("Register_button").GetComponent<Button>();
    //    usernameField = transform.Find("UsernameField").Find("Username_Input").GetComponent<InputField>();
    //    pwdField = transform.Find("PwdField").Find("Pwd_Input").GetComponent<InputField>();
    //    loginRequest = GetComponent<LoginRequest>();

    //    btn.onClick.AddListener(OnCloseButtonClick);
    //    loginBtn.onClick.AddListener(OnLoginBtnClick);
    //    registerBtn.onClick.AddListener(OnRegisterBtnClick);
    //}
    private void OnLoginBtnClick()
    {
        PlayClickSound();
        string msg = "";
        if(string.IsNullOrEmpty(usernameField.text)||string.IsNullOrEmpty(pwdField.text))
        {
            msg = "用户名和密码不能为空";
        }
        if(msg != "")
        {
            GameFacade.Instance.ShowMessage(msg);
            return;
        }
        //发送请求至服务器
        loginRequest.SendRequest(usernameField.text, pwdField.text);
    }
    public void OnLoginResponse(ReturnCode returnCode)
    {

        if (returnCode == ReturnCode.Success)
        {
            //todo...登陆成功，进入房间列表
            uIManager.PopPanelOnMain();
            uIManager.ShowPanelOnMain(UIPanelType.Chat);
            uIManager.PushPanelOnMain(UIPanelType.RoomList);

        }
        else
        {
            uIManager.ShowMessageOnMain("用户名或密码错误");
        }
    }
    private void OnRegisterBtnClick()
    {
        PlayClickSound();
        uIManager.PopPanelOnMain();
       uIManager.PushPanel(UIPanelType.Register);
    }
    //public override void OnExit()
    //{
    //    transform.DOScale(0, 0.2f);
    //}
    private void OnCloseButtonClick()
    {
        PlayClickSound();
        //transform.DOScale(0, 0.2f).OnComplete(() => uIManager.PopPanel());
        uIManager.PopPanel();
        uIManager.PushPanel(UIPanelType.Start);
    }

}
