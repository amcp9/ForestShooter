using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;
using System;

public class RegisterPanel : BasePanel {
    private InputField userNameIf;
    private InputField pwdIF;
    private InputField rePwdIF;
    private RegisterRequest registerRequest;
    private void Start()
    {
        userNameIf = transform.Find("UsernameField").Find("Username_Input").GetComponent<InputField>();
        pwdIF = transform.Find("PwdField").Find("Pwd_Input").GetComponent<InputField>();
        rePwdIF = transform.Find("RePwdField").Find("Pwd_Input").GetComponent<InputField>();
        registerRequest = GetComponent<RegisterRequest>();
        transform.Find("Register_button").GetComponent<Button>().onClick.AddListener(OnRegisterClick);
        transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);
    }
    //public override void OnEnter()
    //{
    //    base.OnEnter();
    //    userNameIf = transform.Find("UsernameField").Find("Username_Input").GetComponent<InputField>();
    //    pwdIF = transform.Find("PwdField").Find("Pwd_Input").GetComponent<InputField>();
    //    rePwdIF = transform.Find("RePwdField").Find("Pwd_Input").GetComponent<InputField>();
    //    registerRequest = GetComponent<RegisterRequest>();
    //    transform.Find("Register_button").GetComponent<Button>().onClick.AddListener(OnRegisterClick);
    //    transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);

    //}


    private void OnRegisterClick()
    {
        PlayClickSound();
        if (string.IsNullOrEmpty(userNameIf.text) || string.IsNullOrEmpty(pwdIF.text) || string.IsNullOrEmpty(rePwdIF.text))
        {
            uIManager.ShowMessage("用户名和密码不能为空");
            return;
        }
        else
        { 
            if (pwdIF.text != rePwdIF.text)
            {
                uIManager.ShowMessage("密码输入不一致");
                return;
            }
        }
        //to register user
        registerRequest.SendRequest(userNameIf.text, pwdIF.text);

    }

    //public override void OnExit()
    //{
    //    transform.DOScale(0, 0.2f);
    //}

    private void OnCloseClick()
    {
        PlayClickSound();
        uIManager.PopPanel();
        uIManager.PushPanel(UIPanelType.Login);
    }

    internal void OnRegisterResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            uIManager.ShowMessageOnMain("注册成功");
        }
        else uIManager.ShowMessageOnMain("用户名已存在");
    }
}
