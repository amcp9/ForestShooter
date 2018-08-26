using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager 
{
    protected GameFacade facade;
    public BaseManager(GameFacade facade)
    {
        this.facade = facade;
    }

    //涉及生命周期
    public virtual void OnInit(){}
    public virtual void UPdate(){}
    public virtual void OnDestroy(){}
} 
