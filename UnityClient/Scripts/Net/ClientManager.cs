using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using Common;
using War_Server.Servers;

public class ClientManager :BaseManager
{
    public ClientManager(GameFacade facade) : base(facade) { }
    public const string IP = "192.168.192.107";
    private const int Port = 12345;
    private Socket clientSocket;
    private Message msg = new Message();

    public override void OnInit()
    {
        base.OnInit();

        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientSocket.Connect(IP, Port);
            Start();
        }
        catch(Exception e)
        {
            Debug.LogWarning("无法连接到服务器:" + e.Message);
        }
    }

    private void Start()
    {
        //异步接受数据
        clientSocket.BeginReceive(msg.Data,msg.StartIndex,msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
    }

    /// <summary>
    /// 接收数据后的回调
    /// </summary>
    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            if(clientSocket==null || !clientSocket.Connected)
            {
                facade.ShowMessageOnMain("网络连接中断");
                return;
            }
            int count = clientSocket.EndReceive(ar);
            if(count == 0)
            {
                facade.ShowMessageOnMain("网络连接中断");
            }
            msg.ReadMessage(count, OnProcessDataCallback);
            Start();
        }
        catch(Exception e)
        {
            Debug.Log("接受服务器数据时间发生错误：" + e.Message);
            
        }
    }
    /// <summary>
    /// 解析数据完成后执行的回调，由对应的Request执行
    /// </summary>
    private void OnProcessDataCallback(ActionCode actionCode,string s)
    {
        //RequestManager to do...
        facade.HandleResponse(actionCode, s);
    }

    /// <summary>
    /// 发送请求至服务器
    /// </summary>
    /// <param name="request">请求码,服务器端以请求码区分不同的Request</param>
    /// <param name="action">当服务器找到相应的Request后通过ActionCode通过反射获得对应的函数</param>
    /// <param name="data">这条请求除去RequestCode和ActionCode后，所包含的数据</param>
    public void SendRequest(RequestCode request,ActionCode action,string data)
    {
        byte[] needSend = Message.PackData(request, action, data);
        try
        {
            clientSocket.Send(needSend);
        }
        catch(Exception e)
        {
            facade.ShowMessage("asd未打开服务器，错误码：" + e.Message);
        }
    }

    //游戏结束时回收资源
    public override void OnDestroy()
    {
        base.OnDestroy();
        try
        {
            clientSocket.Close();
        }
        catch(Exception e)
        {
            Debug.LogWarning("关闭Socket失败:" + e.Message);
        }
    }
}
