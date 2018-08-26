using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class GameFacade : MonoBehaviour {
    private static GameFacade _instance;
    public static GameFacade Instance{ get { return _instance; }}
    public Loom loom;

    private UIManager uIManager;
    private CameraManager cameraManager;
    private AudioManager audioManager;
    private RequestManager requestManager;
    private PlayerManager playerManager;
    private ClientManager clientManager;

    private void Awake()
    {
        if(_instance!=null)
        {
            Destroy(this.gameObject);
        }
        _instance = this;
    }
    // Use this for initialization
    void Start () {
        InitManager();
        loom = Loom.Current;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateManager();
	}
    private void UpdateManager()
    {
        uIManager.UPdate();
        audioManager.UPdate();
        playerManager.UPdate();
        requestManager.UPdate();
        cameraManager.UPdate();
        clientManager.UPdate();
    }
    private void OnDestroy()
    {
        DestroyManager();
    }

    private void InitManager()
    {
        uIManager = new UIManager(this);
        audioManager = new AudioManager(this);
        playerManager = new PlayerManager(this);
        requestManager = new RequestManager(this);
        cameraManager = new CameraManager(this);
        clientManager = new ClientManager(this);

        uIManager.OnInit();
        audioManager.OnInit();
        playerManager.OnInit();
        requestManager.OnInit();
        cameraManager.OnInit();
        clientManager.OnInit();
    }

    private void DestroyManager()
    {
        uIManager.OnDestroy();
        audioManager.OnDestroy();
        playerManager.OnDestroy();
        requestManager.OnDestroy();
        cameraManager.OnDestroy();
        clientManager.OnDestroy();
    }

    public void SetPlayerName(string b,string r)
    {
        playerManager.SetPlayerName(b, r);
    }

    public void SetHealthBarPanel(HealthBarPanel panel)
    {
        playerManager.SetHealthBarPanel(panel);
        playerManager.SetHealthBarPanelName();
    }

    public void AddRequest(ActionCode actionCode, BaseRequest requestMethod)
    {
        requestManager.AddRequest(actionCode, requestMethod);
    }

    public void RemoveRequest(ActionCode actionCode)
    {
        requestManager.RemoveRequest(actionCode);
    }

    public void HandleResponse(ActionCode actionCode, string data)
    {
        requestManager.HandleResponse(actionCode, data);
    }

    public void ShowMessage(string s)
    {
        uIManager.ShowMessage(s);
    }

    public void ShowMessageOnMain(string s)
    {
        uIManager.ShowMessageOnMain(s);
    }

    //发送请求至服务器
    public void SendRequest(RequestCode request, ActionCode action, string data)
    {
        clientManager.SendRequest(request, action, data);
    }

    public void PlayBGSound(string name)
    {
        audioManager.PlayBGSound(name);
    }

    public void PlayNormalSound(string name)
    {
        audioManager.PlayNormalSound(name);
    }

    public void PlayNormalSound(string name,float spatialBlend)
    {
        audioManager.PlayNormalSound(name,spatialBlend);
    }

    public void SetUserData(UserData data)
    {
        playerManager.UserData = data;
    }
    public UserData GetUserData()
    {
        return playerManager.UserData;
    }

    private void OnApplicationQuit()
    {
        clientManager.OnDestroy();
        DestroyManager();
    }

    public void SetCurrentRoleType(int i)
    {
        playerManager.SetCurrentRoleType(i);
    }

    public void SetRemoteRoleType(int i)
    {
        playerManager.SetRemoteRoleType(i);
    }

    public GameObject GetCurrentGO()
    {
        return playerManager.GetCurrentGO();
    }

    public void EnableScripts()
    {
        playerManager.EnableScripts();
    }

    public void CreateSyncGO()
    {
        playerManager.CreateSyncRequest();
    }

    public void InsPlayer()
    {
        playerManager.SpawnPlayers();
        cameraManager.TurnToFollow();
    }

    public void SendAttackRequest(int damage)
    {
        playerManager.SendAttackRequest(damage);
    }

    public void SendGainRequest(BaseStatType type, int value)
    {
        playerManager.SendGainRequest(type, value);
    }

    public RoleData GetRoleData(RoleType roleType)
    {
        return playerManager.characterDict[roleType];
    }

    public void Restart()
    {
        playerManager.Restart();
        cameraManager.TurnToAnim();
        uIManager.PushPanelOnMain(UIPanelType.RoomList);
    }

    public void GameOver()
    {
        uIManager.PopPanel();
        playerManager.GameOver();
    }
}
