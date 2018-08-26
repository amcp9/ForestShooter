using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System;

public class ShootRequest : BaseRequest {

    public PlayerManager playerManager;

    private RoleType role;
    private Vector3 pos;
    private Vector3 rotation;
    private bool isNeedSyncRemoteArrow = false;

    public override void Awake()
    {
        request = RequestCode.Game;
        actionCode = ActionCode.Shoot;
        base.Awake();
    }

    private void Update()
    {
        if(isNeedSyncRemoteArrow)
        {
            playerManager.RemoteShoot(role, pos, rotation);
            isNeedSyncRemoteArrow = false;
        }
    }

    public void SendRequest(RoleType roleType,Vector3 pos,Vector3 rotation)
    {
        string data = string.Format("{0}|{1},{2},{3}|{4},{5},{6}", (int)roleType, pos.x, pos.y, pos.z, rotation.x, rotation.y, rotation.z);
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        role = (RoleType)int.Parse(strs[0]);
        pos = Vector3_Parse.Parse(strs[1]);
        rotation = Vector3_Parse.Parse(strs[2]);
        isNeedSyncRemoteArrow = true;
    }
}
