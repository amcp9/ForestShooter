using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMove : MonoBehaviour {

    public float aniForward = 0;

    private Animator animator;

    private float h;
    private float v;

    [SerializeField]
    private float speed = 6f;

    public void SetSpeed(int value)
    {
        float temp = value / 10;
        speed += temp;
    }

    public void ResetSpeed()
    {
        speed = 6f;
    }

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        GetComponent<PlayerStats>().OnMoveSpeedChange += SetSpeed;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        if (animator.GetAnimatorTransitionInfo(0).IsName("Grounded -> Attack")) return;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded")) return;
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if(Mathf.Abs(h) >0 || Mathf.Abs(v) >0 )
        {
            transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime, Space.World);
            transform.rotation = Quaternion.LookRotation(new Vector3(h, 0, v));
            aniForward = Mathf.Max(Mathf.Abs(h), Mathf.Abs(v));
            animator.SetFloat("Forward", Mathf.Max(Mathf.Abs(h), Mathf.Abs(v)));
        }
	}

}
