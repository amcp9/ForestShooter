using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System;

public class RequestManager : BaseManager
{
    //管理所有request
    public RequestManager(GameFacade facade) : base(facade) { }
    private Dictionary<ActionCode, BaseRequest> requestDict = new Dictionary<ActionCode, BaseRequest>();

    public void AddRequest(ActionCode actionCode,BaseRequest requestMethod)
    {
        requestDict.Add(actionCode, requestMethod);
    }

    public void RemoveRequest(ActionCode actionCode)
    {
        requestDict.Remove(actionCode);
    }


    //当服务器请求发来时，由Requestmanager根据Requestcode找到对应的baseRequest
    //该方法由gamefacade调用
    public void HandleResponse(ActionCode actionCode,string data)
    {
        //BaseRequest request = requestDict.TryGet<ActionCode, BaseRequest>(actionCode);

        BaseRequest request = requestDict.TryGet<ActionCode, BaseRequest>(actionCode);
        if(request == null)
        {
            Debug.LogWarning("无法得到ActionCode:[" + actionCode + "]对应的Request");
            return;
        }
        request.OnResponse(data);

    }
}
