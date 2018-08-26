using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartPanel : BasePanel {
    private Button button;

    private void Start()
    {
        button = transform.Find("LoginButton").GetComponent<Button>();
        button.onClick.AddListener(ClickButton);
    }
    public override void OnEnter()
    {
        base.OnEnter();
    }

    private void ClickButton()
    {
        PlayClickSound();
        uIManager.PopPanel();
        uIManager.PushPanel(UIPanelType.Login);
    }
}
