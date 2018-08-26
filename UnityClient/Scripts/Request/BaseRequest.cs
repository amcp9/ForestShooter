using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class BaseRequest : MonoBehaviour {
    protected RequestCode request = RequestCode.None;
    protected ActionCode actionCode = ActionCode.None;
    protected GameFacade _facade;
    protected GameFacade facade
    {
        get
        {
            if (_facade == null) _facade = GameFacade.Instance;
            return _facade;
        }
    }
    //将自身添加到RequestManager中的字典中(调用Gamefacade)
	public virtual void Awake () {
        GameFacade.Instance.AddRequest(actionCode, this);
	}

    public void SendRequest(string data)
    {
        facade.SendRequest(request, actionCode, data);
    }
    public virtual void SendRequest(){}
    public virtual void OnResponse(string data){}

    public virtual void OnDestroy()
    {
        GameFacade.Instance.RemoveRequest(actionCode);
    }
}
