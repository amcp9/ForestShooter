using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class RoleData 
{
    public RoleType RoleType { get; private set; }
    public GameObject RolePrefab{ get; private set; }
    public GameObject ArrowPrefab{ get; private set; }
    public Vector3 SpawnPosition { get; private set; }
    public GameObject BrustEffect { get; private set; }

    public ObjectPool arrowPool, brustEffectPool;

    public RoleData(RoleType roleType,string rolePath,string arrowPath,string effectPath,Transform spawnPos)
    {
        this.RoleType = roleType;
        this.RolePrefab = Resources.Load(rolePath) as GameObject;
        this.ArrowPrefab = Resources.Load(arrowPath) as GameObject;
        this.BrustEffect = Resources.Load(effectPath) as GameObject;

        arrowPool = ObjectPool.CreateObjectPool(ArrowPrefab, ArrowPrefab.name + "Pool",20, true, true, true);
        brustEffectPool = ObjectPool.CreateObjectPool(BrustEffect, BrustEffect.name + "Pool", 20, true, true, true);
        ArrowPrefab.GetComponent<Arrow>().brustEffect = BrustEffect;
        ArrowPrefab.GetComponent<Arrow>().effectPool = brustEffectPool;
        ArrowPrefab.GetComponent<Arrow>().type = RoleType;
        this.SpawnPosition = spawnPos.position;
    }

}
