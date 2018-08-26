using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class ChatPanel : BasePanel
{
    public float offset = 100f;
    public VerticalLayoutGroup layoutGroup;
    public InputField userInput;
    public Text TipText;

    private delegate void OnReturnDown();
    private delegate void OnTime();
    private event OnTime OnTimeFade;
    private event OnReturnDown OnReturn;

    private GameObject chatItem;

    private int maxItemCount = 10;
    private string userData = null;
    private string userName = null;
    private string systemData = null;
    private bool isFade = false;

    private bool isNeedChat = false;
    private bool isNeedSystemChat = false;
    private ChatRequest chatRequest;
    private float originalX;
    private float fadeTime = 5f;
    private float timer = 0f;
    private Tween tween;

    private void Awake()
    {
        chatRequest = GetComponent<ChatRequest>();
        OnReturn += InputAndSendMessage;
        OnTimeFade += FadePanel;
    }



    private void Update()
    {
        if(!isFade) timer += Time.deltaTime;
        if (timer >= fadeTime && !isFade) OnTimeFade();
        if(isNeedChat)
        {
            PlayerChat(userName, userData);
            userName = null;
            userData = null;
            isNeedChat = false;
        }
        if(isNeedSystemChat)
        {
        
            SystemChat(systemData);
            systemData = null;
            isNeedSystemChat = false;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnReturn();
        }

    }

    private void InputAndSendMessage()
    {
        ShowPanel();
        if (EventSystem.current.currentSelectedGameObject != userInput.gameObject)
        {
            userInput.interactable = true;
            EventSystem.current.SetSelectedGameObject(userInput.gameObject);
            isFade = true;
        }
        else
        {
            userInput.interactable = false;
            if (!string.IsNullOrEmpty(userInput.text))
            {
                chatRequest.Send(userInput.text);
            }
            userInput.text = null;
            isFade = false;
        }
    }

    private void Start()
    {
        chatItem = Resources.Load("UIPanel/ChatItem") as GameObject;
        userInput.interactable = false;
        originalX = transform.position.x;
    }

    private void LoadChatItem()
    {
        GameObject obj = Instantiate(chatItem);
        obj.transform.SetParent(layoutGroup.transform);
    }

    private void UpdatePosition()
    {
        layoutGroup.transform.localPosition = new Vector3(0, layoutGroup.transform.localPosition.y + offset, 0);
    }

    public void PlayerChat(string name,string data)
    {
        GameObject obj = Instantiate(chatItem);
        obj.transform.SetParent(layoutGroup.transform);
        obj.GetComponent<ChatItem>().SetUserText(name, data);
        UpdateLayoutSize();
        ShowPanel();
        UpdatePosition();
    }

    public void SystemChat(string data)
    {
        GameObject obj = Instantiate(chatItem);
        obj.transform.SetParent(layoutGroup.transform);
        obj.GetComponent<ChatItem>().SetSystemText(data);
        UpdateLayoutSize();
        ShowPanel();
        UpdatePosition();
    }

    private void UpdateLayoutSize()
    {
        int itemCount = GetComponentsInChildren<ChatItem>().Length;
        Vector2 size = layoutGroup.GetComponent<RectTransform>().sizeDelta;
        layoutGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,
                                                                          itemCount * (chatItem.GetComponent<RectTransform>().sizeDelta.y
                                                                           + layoutGroup.GetComponent<VerticalLayoutGroup>().spacing));
    }

    public void SystemChatOnMain(string data)
    {
        systemData = data;
        isNeedSystemChat = true;
    }

    public void PlayerChatOnMain(string name,string data)
    {
        userData = data;
        userName = name;
        isNeedChat = true;
    }

    private void FadePanel()
    {
        this.transform.DOMoveX(-500, 0.3f);
        isFade = true;
    }

    private void ShowPanel()
    {
        this.transform.DOMoveX(originalX, 0.3f);
        timer = 0f;
        isFade = false;
    }

    public void SetTipsText(string data)
    {
        TipText.text = "当前在线：" + data.ToString();
    }

}
