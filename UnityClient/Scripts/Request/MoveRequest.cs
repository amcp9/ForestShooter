using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class MoveRequest : BaseRequest {

    private Transform localPlayerTransform;
    private PlayerMove localPlayerMove;
    private Transform remotePlayerTransform;
    private Animator remoteAnimatior;

    private bool isNeedSyncRemote = false;
    private Vector3 pos;
    private Vector3 rotation;
    private float forward;

    public int rate = 15;

    public override void Awake()
    {
        request = RequestCode.Game;
        actionCode = ActionCode.Move;
        base.Awake();
    }

    private void Start()
    {
        InvokeRepeating("SendLocalPlayerSyncInfo",1f,1f/rate);
    }

    private void FixedUpdate()
    {
        if(isNeedSyncRemote)
        {
            SyncRemotePlayerInfo();
            isNeedSyncRemote = false;
        }
    }

    private void SyncRemotePlayerInfo()
    {
        remotePlayerTransform.position = pos;
        remotePlayerTransform.eulerAngles = rotation;
        remoteAnimatior.SetFloat("Forward", forward);
    }

    private void SendLocalPlayerSyncInfo()
    {
        SendMessage(localPlayerTransform.position.x, localPlayerTransform.position.y, localPlayerTransform.position.z,
                    localPlayerTransform.eulerAngles.x, localPlayerTransform.eulerAngles.y, localPlayerTransform.eulerAngles.z, localPlayerMove.aniForward);
    }

    public MoveRequest SetLocalPlayer(Transform localPT,PlayerMove localPM)
    {
        this.localPlayerTransform = localPT;
        this.localPlayerMove = localPM;
        return this;
    }

    public MoveRequest SetRemotePlayer(Transform remotePT,Animator remoteAnimator)
    {
        this.remotePlayerTransform = remotePT;
        this.remoteAnimatior = remoteAnimator;
        return this;
    }

    public void SendMessage(float x,float y,float z,float rotationX,float rotationY,float rotationZ,float forward)
    {
        string data = string.Format("{0},{1},{2}|{3},{4},{5}|{6}", x, y, z, rotationX, rotationY, rotationZ, forward);
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        pos = Vector3_Parse.Parse(strs[0]);
        rotation = Vector3_Parse.Parse(strs[1]);
        forward = float.Parse(strs[2]);
        isNeedSyncRemote = true;
    }

}
