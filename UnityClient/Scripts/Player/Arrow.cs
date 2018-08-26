using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Arrow : MonoBehaviour {
    public RoleType type;
    public GameObject brustEffect;
    public ObjectPool effectPool;
    public float speed = 5f;
    private Rigidbody rig;
    public bool isLocal = false;

	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
        rig.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
	}

    private void OnTriggerEnter(Collider other)
    {
        GameFacade.Instance.PlayNormalSound(AudioManager.Sound_ArrowBoom,1f);
        if(other.tag == "Player" && isLocal)
        {
           GameFacade.Instance.SendAttackRequest(10);
        }

        GameObject g;
        effectPool = GameFacade.Instance.GetRoleData(type).brustEffectPool;
        if (effectPool.TryGetNextObject(transform.position, transform.rotation, out g))
        {
            g.GetComponent<BrustEffect>().StartCoroutine("ShowEffect");
        }
        gameObject.SetActive(false);
    }

}
