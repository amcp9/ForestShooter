using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemotePlayer : MonoBehaviour 
{
    public PlayerManager PlayerManager { private get; set; }

    public GameObject arrowPrefab;
    public Vector3 position;
    public Vector3 rotation;
    private ObjectPool Pool;
    public bool isRampaging = false;

    public void SetArrowSetting(ObjectPool pool,Vector3 pos,Vector3 rot)
    {
        Pool = pool;
        position = pos;
        rotation = rot;
    }

    //由动画状态机"Attack"状态的事件触发
    public void InsRemoteArrow()
    {
        GameObject g;
        if(Pool.TryGetNextObject(position,Quaternion.Euler(rotation),out g))
        {
            g.GetComponent<SphereCollider>().enabled = true;
            Arrow arrow = g.GetComponent<Arrow>();
            arrow.isLocal = true;
            //if (isRampaging)
            //{
            //    arrow.speed = 8f;
            //    ParticleSystem.MainModule m = arrow.GetComponent<ParticleSystem>().main;
            //    m.startLifetime = 3f;
            //    m.simulationSpeed = 4f;
            //}
        }

        GameFacade.Instance.PlayNormalSound(AudioManager.Sound_ArrowFly,1f);
    }
}
