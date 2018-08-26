using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class PlayerAttack : MonoBehaviour {

    public GameObject arrowPrefab;
    public Animator animator;
    private Transform leftHandTrans;
    private Vector3 dir;
    private PlayerManager playerManager;
    public bool isRampaging = false;

    public RoleType roleType;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        roleType = GetComponent<PlayerStats>().roleType;
        leftHandTrans = transform.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand");
	}
	
	// Update is called once per frame
	void Update () {
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool isCollider = Physics.Raycast(ray, out hit);
                if(isCollider)
                {
                    Vector3 target = hit.point;
                    target.y = transform.position.y;
                    dir = target - transform.position;
                    transform.rotation = Quaternion.LookRotation(dir);
                    //当玩家处于非暴走状态时，射箭积攒暴走条
                    if(isRampaging == false)
                    {
                        GameFacade.Instance.SendGainRequest(BaseStatType.SpeedPoint, 20);
                    }
                    playerManager.SendShootRequest(roleType, leftHandTrans.position, Quaternion.LookRotation(dir));
                    animator.SetTrigger("Attack");

                }
            }
        }

	}

    public void InsArrow()
    {
        animator.SetFloat("Forward", 0.1f);
        playerManager.Shoot(roleType, leftHandTrans.position, Quaternion.LookRotation(dir));
    }

    public void SetPlayerManager(PlayerManager manager)
    {
        playerManager = manager;
    }
}
