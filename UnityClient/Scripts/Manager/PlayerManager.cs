using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System;

public class PlayerManager : BaseManager
{
    private UserData userData;
    private Transform playerPositions;
    private RoleType currentRoleType;
    private RoleType remoteRoleType;

    private GameObject currentGO;
    private GameObject remoteGO;
    private GameObject SyncRequestGameObject;

    private ShootRequest shootRequest;
    private AttackRequest attackRequest;
    private UpdatePlayerInfoRequest udInfoRequest;
    private GainInfoRequest gainInfoRequest;
    private RampageRequest rampageRequest;

    private PlayerStats currentPlayerStats;
    public PlayerStats remotePlayerStats;

    private RuntimeAnimatorController currentRTAnimator;
    private RuntimeAnimatorController remoteRTAnimator;

    private RemotePlayer remotePlayer;

    private Animator currentAnimator;
    private Animator remoteAnimator;

    public string blueName;
    public string redName;

    private HealthBarPanel healthBarPanel;

    public void ChangeHealthBar(RoleType role,int current,int max)
    {
        if(role == RoleType.Blue)
        {
            healthBarPanel.ChangeBlueHeathBarOnMain(current, max);

        }else
        {
            healthBarPanel.ChangeRedHeathBarOnMain(current, max);
        }
    }

    public void ChangeRampagePanel(RoleType role, int current)
    {
        if (role == RoleType.Blue)
        {
            healthBarPanel.ChangeBlueRampageOnMain(current);

        }
        else
        {
            healthBarPanel.ChangeRedRampageOnMain(current);
        }
    }

    public void SetHealthBarPanel(HealthBarPanel panel)
    {
        healthBarPanel = panel;
    }

    public PlayerManager(GameFacade facade) : base(facade) { }

    public void SetPlayerName(string host,string guest)
    {
        blueName = host;
        redName = guest;
    }

    //设置双方血条中的姓名
    public void SetHealthBarPanelName()
    {
        healthBarPanel.SetUserNameText(blueName, redName);
    }

    public void SetCurrentRoleType(int i)
    {
        currentRoleType = (RoleType)i;
    }

    public RoleType GetCurrentRoleType()
    {
        return currentRoleType;
    }

    public void SetRemoteRoleType(int i)
    {
        remoteRoleType = (RoleType)i;
    }

    public RoleType GetRemoteRoleType()
    {
        return remoteRoleType;
    }

    public Dictionary<RoleType, RoleData> characterDict = new Dictionary<RoleType, RoleData>();

    public UserData UserData
    { 
        set { userData = value; }
        get { return userData; }
    }

    //初始化操作
    public override void OnInit()
    {
        playerPositions = GameObject.Find("RespawnPosition").transform;
        remoteRTAnimator = Resources.Load("Animator/RemoteAnimator") as RuntimeAnimatorController;
        currentRTAnimator = Resources.Load("Animator/CurrentAnimator") as RuntimeAnimatorController;
        InitCharacterData();
    }

    //初始化双方玩家对应的prefab
    private void InitCharacterData()
    {
        characterDict.Add(RoleType.Blue, new RoleData(RoleType.Blue, "Hunter_BLUE", "Arrow_Blue","Brust_Blue",playerPositions.Find("Pos1").transform));
        characterDict.Add(RoleType.Red, new RoleData(RoleType.Red, "Hunter_RED", "Arrow_Red","Brust_Red",playerPositions.Find("Pos2").transform));
    }


    //实例化本地玩家与敌方玩家prefab
    public void SpawnPlayers()
    {
        foreach(RoleData rd in characterDict.Values)
        {
            GameObject gameObject = GameObject.Instantiate(rd.RolePrefab, rd.SpawnPosition, Quaternion.identity);
            if (rd.RoleType == currentRoleType)
            {
                currentGO = gameObject;
                currentPlayerStats = currentGO.GetComponent<PlayerStats>();
                currentPlayerStats.OnHealthChange += ChangeHealthBar;
                currentPlayerStats.OnRampageChange += ChangeRampagePanel;
                currentPlayerStats.OnRampage += SendRampageRequest;
                currentPlayerStats.roleType = GetCurrentRoleType();
                currentGO.GetComponent<Animator>().runtimeAnimatorController = currentRTAnimator;
                currentAnimator = currentGO.GetComponent<Animator>();
                currentGO.tag = "Me";
            }
            else 
            {
                gameObject.tag = "Player";
                remoteGO = gameObject;
                remoteGO.GetComponent<Animator>().runtimeAnimatorController = remoteRTAnimator;
                remoteAnimator = remoteGO.GetComponent<Animator>();
                remotePlayer = remoteGO.AddComponent<RemotePlayer>();
                remotePlayerStats = remoteGO.GetComponent<PlayerStats>();
                remotePlayerStats.OnHealthChange += ChangeHealthBar;
                remotePlayerStats.OnRampageChange += ChangeRampagePanel;
                remotePlayerStats.OnRampage += Empty;
                remotePlayerStats.roleType = GetRemoteRoleType();
            }

        }
    }
    public void Empty()
    {
        
    }

    //禁用敌方玩家相关脚本
    public void EnableScripts()
    {
        currentGO.GetComponent<PlayerMove>().enabled = true;
        currentGO.GetComponent<PlayerAttack>().enabled = true;
        currentGO.GetComponent<PlayerAttack>().SetPlayerManager(this);
    }

    public GameObject GetCurrentGO()
    {
        return currentGO;
    }

    public void SendRampageRequest()
    {
        rampageRequest.SendRequest();
    }

    //创建用于同步的游戏物体
    public void CreateSyncRequest()
    {
        SyncRequestGameObject = new GameObject("SyncGameObject");
       
        SyncRequestGameObject.AddComponent<MoveRequest>().SetLocalPlayer(currentGO.transform,currentGO.GetComponent<PlayerMove>()).
                             SetRemotePlayer(remoteGO.transform,remoteGO.GetComponent<Animator>()); //添加用于同步位置信息的脚本，并设置其成员
        
        shootRequest = SyncRequestGameObject.AddComponent<ShootRequest>();                          //添加用于处理攻击行为的脚本，并设置其成员
        shootRequest.playerManager = this;

        attackRequest = SyncRequestGameObject.AddComponent<AttackRequest>();                        //添加用于处理伤害扣血请求的脚本

        udInfoRequest = SyncRequestGameObject.AddComponent<UpdatePlayerInfoRequest>();              //添加用于更新双方属性的脚本
        udInfoRequest.playerManager = this;

        gainInfoRequest = SyncRequestGameObject.AddComponent<GainInfoRequest>();                    //添加用于属性提升请求的脚本

        rampageRequest = SyncRequestGameObject.AddComponent<RampageRequest>();                      //添加暴走请求脚本
        rampageRequest.playerManager = this;
    }

    //本地玩家暴走
    public void CurrentRampage()
    {
        currentPlayerStats.RampageOnMain();
    }

    //同步非本地玩家暴走
    public void RemoteRampage()
    {
        remotePlayerStats.RampageOnMain();
    }

    //本地实例化箭
    public void Shoot(RoleType type,Vector3 pos,Quaternion rotation)
    {
        GameObject g;
        if(characterDict[type].arrowPool.TryGetNextObject(pos, rotation,out g))
        {
            g.GetComponent<SphereCollider>().enabled = true;
            Arrow arrow = g.GetComponent<Arrow>();
            arrow.isLocal = true;
            //if (currentPlayerStats.isRampaging)
            //{
            //    arrow.speed = 8f;
            //    ParticleSystem.MainModule m = arrow.GetComponent<ParticleSystem>().main;
            //    m.startLifetime = 3f;
            //    m.simulationSpeed = 4f;
            //}
        }

        facade.PlayNormalSound(AudioManager.Sound_ArrowFly,1f);
    }

    //发送攻击请求
    public void SendShootRequest(RoleType type, Vector3 pos, Quaternion rotation)
    {
        //GameObject.Instantiate(arrowPrefab, pos, rotation).GetComponent<Arrow>().isLocal = true;
        shootRequest.SendRequest(type, pos, rotation.eulerAngles);
        //facade.PlayNormalSound(AudioManager.Sound_ArrowFly, 1f);
    }

    //发送属性提升请求
    public void SendGainRequest(BaseStatType type,int value)
    {
        gainInfoRequest.SendMessage(type, value);
    }

    //接收到敌方玩家攻击行为
    public void RemoteShoot(RoleType rt, Vector3 pos, Vector3 rotation)
    {
        remotePlayer.SetArrowSetting(characterDict[rt].arrowPool, pos, rotation);//传递对应的箭prefab对象及位置信息
        remoteAnimator.SetTrigger("Attack");                                    //触发动画控制变量
    }

    //发送攻击请求
    public void SendAttackRequest(int damage)
    {
        attackRequest.SendRequest(damage);
    }

    //设置本地玩家属性
    public void SetCurrentStat(int cMaxHealth,int cHealth,int cMoveSpeed,int cAttackSpeed,int cRampPoint)
    {
        lock (currentPlayerStats)
        {
            currentPlayerStats.SetMaxHealth(cMaxHealth);
            currentPlayerStats.SetHealth(cHealth);
            currentPlayerStats.SetAttackSpeed(cAttackSpeed);
            currentPlayerStats.SetMoveSpeed(cMoveSpeed);
            currentPlayerStats.SetSpeedPoint(cRampPoint);
        }
    }

    //设置对手属性
    public void SetRemoteStat(int rMaxHealth,int rHealth,int rMoveSpeed,int rAttackSpeed,int rRampPoint)
    {
        lock (remotePlayerStats)
        {
            remotePlayerStats.SetMaxHealth(rMaxHealth);
            remotePlayerStats.SetHealth(rHealth);
            remotePlayerStats.SetAttackSpeed(rAttackSpeed);
            remotePlayerStats.SetMoveSpeed(rMoveSpeed);
            remotePlayerStats.SetSpeedPoint(rRampPoint);
        }
    }

    /// <summary>
    /// 第一步解包完成后由PlayerManager完成第二步解包,数据格式:MaxHealth-CurrentHealth-MoveSpeed-AttackSpeed-RampagePoint
    /// </summary>
    public void UpdatePlayerRes(string host,string guest)
    {
        switch(currentRoleType)
        {
            case RoleType.Blue:
                string[] mine = host.Split('-');
                string[] remote = guest.Split('-');
                SetCurrentStat(int.Parse(mine[0]),int.Parse(mine[1]), int.Parse(mine[2]), int.Parse(mine[3]), int.Parse(mine[4]));
                SetRemoteStat(int.Parse(remote[0]),int.Parse(remote[1]), int.Parse(remote[2]), int.Parse(remote[3]), int.Parse(remote[4]));
                break;
            case RoleType.Red:
                string[] m = guest.Split('-');
                string[] r = host.Split('-');
                SetCurrentStat(int.Parse(m[0]),int.Parse(m[1]), int.Parse(m[2]), int.Parse(m[3]), int.Parse(m[4]));
                SetRemoteStat(int.Parse(r[0]),int.Parse(r[1]), int.Parse(r[2]), int.Parse(r[3]), int.Parse(r[4]));
                break;
        }
    }

    public void GameOver()
    {
        currentGO.GetComponent<PlayerMove>().enabled = false;
        currentGO.GetComponent<PlayerAttack>().enabled = false;
    }

    public void Restart()
    {
        GameObject.Destroy(currentGO);
        GameObject.Destroy(remoteGO);
        GameObject.Destroy(SyncRequestGameObject);
    }
}
