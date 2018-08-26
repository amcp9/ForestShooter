using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIManager:BaseManager
{
    public UIManager(GameFacade facade) : base(facade) { ParseUIPanelTypeJson();}
    private UIPanelType uIPanel = UIPanelType.None;
    private UIPanelType needShowPanel = UIPanelType.None;

    /// 管理所有UI界面
    /// 单例模式的核心
    /// 1，定义一个静态的对象 在外界访问 在内部构造
    /// 2，构造方法私有化
    public override void OnInit()
    {
        base.OnInit();
        PushPanel(UIPanelType.Message);
        PushPanel(UIPanelType.Start);

    }
    //private static UIManager _instance;

    //public static UIManager Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = new UIManager();
    //        }
    //        return _instance;
    //    }
    //}

    private Transform canvasTransform;
    private Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }
            return canvasTransform;
        }
    }
    private Dictionary<UIPanelType, string> panelPathDict;//存储所有面板Prefab的路径
    private Dictionary<UIPanelType, BasePanel> panelDict;//保存所有实例化面板的游戏物体身上的BasePanel组件
    private Stack<BasePanel> panelStack;
    private MessagePanel msgPanel;

    public override void UPdate()
    {
        if(uIPanel != UIPanelType.None)
        {
            PushPanel(uIPanel);
            uIPanel = UIPanelType.None;
        }
        if(needShowPanel != UIPanelType.None)
        {
            ShowPanel(needShowPanel);
            needShowPanel = UIPanelType.None;
        }
    }

    /// <summary>
    /// 把某个页面入栈，  把某个页面显示在界面上
    /// </summary>
    public void PushPanel(UIPanelType panelType)
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        //判断一下栈里面是否有页面
        if (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }

        BasePanel panel = GetPanel(panelType);
        panel.OnEnter();
        panelStack.Push(panel);
    }
    public void PushPanelOnMain(UIPanelType panelType)
    {
        uIPanel = panelType;
    }
    //public void PushPanelAsync(UIPanelType panelType)
    //{
    //    if (panelStack == null)
    //        panelStack = new Stack<BasePanel>();

    //    //判断一下栈里面是否有页面
    //    if (panelStack.Count > 0)
    //    {
    //        BasePanel topPanel = panelStack.Peek();
    //        Loom.QueueOnMainThread(() => { topPanel.OnPause(); });
    //    }

    //    BasePanel panel = GetPanelASync(panelType);
    //    Loom.QueueOnMainThread(() => { panel.OnEnter(); });
    //    Loom.QueueOnMainThread(() => { panelStack.Push(panel); });
    //}


    /// <summary>
    /// 出栈 ，把页面从界面上移除
    /// </summary>
    public void PopPanel()
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        if (panelStack.Count <= 0) return;

        //关闭栈顶页面的显示
        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();

        if (panelStack.Count <= 0) return;
        BasePanel topPanel2 = panelStack.Peek();
        topPanel2.OnResume();

    }
    public void PopPanelOnMain()
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        if (panelStack.Count <= 0) return;

        //关闭栈顶页面的显示
        BasePanel topPanel = panelStack.Pop();
        Loom.QueueOnMainThread(() => { topPanel.OnExit(); });

        if (panelStack.Count <= 0) return;
        BasePanel topPanel2 = panelStack.Peek();
        Loom.QueueOnMainThread(() => { topPanel2.OnResume(); });

    }

    public BasePanel PeekStack()
    {
        return panelStack.Peek();
    }

    /// <summary>
    /// 根据面板类型 得到实例化的面板
    /// </summary>
    /// <returns></returns>
    private BasePanel GetPanel(UIPanelType panelType)
    {
        if (panelDict == null)
        {
            panelDict = new Dictionary<UIPanelType, BasePanel>();
        }

        BasePanel panel = panelDict.TryGet(panelType);

        if (panel == null)
        {
            //如果找不到，那么就找这个面板的prefab的路径，然后去根据prefab去实例化面板
            //string path;
            //panelPathDict.TryGetValue(panelType, out path);
            string path = panelPathDict.TryGet(panelType);
            GameObject instPanel = GameObject.Instantiate(Resources.Load(path),CanvasTransform) as GameObject;
            //instPanel.transform.SetParent(CanvasTransform,false);
            instPanel.GetComponent<BasePanel>().UIManager = this;
            instPanel.GetComponent<BasePanel>().Facade = facade;
            panelDict.Add(panelType, instPanel.GetComponent<BasePanel>());
            return instPanel.GetComponent<BasePanel>();
        }
        else
        {
            return panel;
        }

    }

    public void ShowPanel(UIPanelType panelType)
    {
        string path = panelPathDict.TryGet(panelType);
        GameObject instPanel = GameObject.Instantiate(Resources.Load(path)) as GameObject;
        instPanel.transform.SetParent(CanvasTransform, false);
        instPanel.GetComponent<BasePanel>().UIManager = this;
        instPanel.GetComponent<BasePanel>().Facade = facade;
    }

    public void ShowPanelOnMain(UIPanelType panelType)
    {
        needShowPanel = panelType;
    }

    [Serializable]
    class UIPanelTypeJson
    {
        public List<UIPanelInfo> infoList;
    }
    private void ParseUIPanelTypeJson()
    {
        panelPathDict = new Dictionary<UIPanelType, string>();
        //读取Resources目录下的UIPanelType文件
        TextAsset ta = Resources.Load<TextAsset>("UIPanelType");
        UIPanelTypeJson jsonObject = JsonUtility.FromJson<UIPanelTypeJson>(ta.text);

        foreach (UIPanelInfo info in jsonObject.infoList) 
        {
            //Debug.Log(info.panelType);
            panelPathDict.Add(info.panelType, info.path);
        }
    }
    //获得messagepanel的引用
    public void InjectMsgPanel(MessagePanel messagePanel)
    {
        msgPanel = messagePanel;
    }
    //控制MessagePanel显示信息
    public void ShowMessage(string s)
    {
        if(msgPanel == null)
        {
            Debug.Log("无法显示信息,msgPanel为空");
        }
        msgPanel.ShowMessage(s);
    }

    public void ShowMessageOnMain(string msg)
    {
        if (msgPanel == null)
        {
            Debug.Log("无法显示信息,msgPanel为空");
        }
        msgPanel.ShowMessageOnMain(msg);
    }



}
