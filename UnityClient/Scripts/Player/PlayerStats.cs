using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Common;

/// <summary>
/// 玩家属性类，维护生命值，速度，暴走点数
/// </summary>
public class PlayerStats : MonoBehaviour 
{

    public RoleType roleType;

    public Animator animator;
    public AnimatorStateInfo animatorStateInfo;
    public Animation anim;
    public AnimationState MoveAnimation;
    public GameObject PowerEffect;
    public GameObject refPowEffect;

    public int MaxHealth = 100;
    public int Health = 100;
    public int AttackSpeed = 1;
    public int MoveSpeed = 1;

    public int SpeedPoint = 0;

    public delegate void StatDelegate(int i);
    public delegate void HealthDelegate(RoleType roleType, int current,int max);
    public delegate void TypeDelegate(RoleType roleType, int value);
    public delegate void RampageDelegate();
    public event RampageDelegate OnRampage;
    public event HealthDelegate OnHealthChange;
    public event TypeDelegate OnRampageChange;
    public event StatDelegate OnMoveSpeedChange;
    public event StatDelegate OnAttackSpeedChange;

   
    private bool isNeedChangeGroundAnimation = false;
    private int tempGroundValue = 0;
    private bool isNeedChangeAttackAnim = false;
    private int tempAttackValue = 0;
    private bool isNeedRampage = false;
    public bool isRampaging = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        refPowEffect = Instantiate(PowerEffect, transform.position, Quaternion.identity, transform);
        refPowEffect.GetComponent<ParticleSystem>().Stop();
    }

    private void Update()
    {
        if(isNeedRampage)
        {
            Rampage();
            isNeedRampage = false;
        }
    }


    public void ChangeGroundAnimationOnMain(int value)
    {
        tempGroundValue = value;
        isNeedChangeGroundAnimation = true;
    }

    public void ChangeAttackAnimationOnMain(int value)
    {
        tempAttackValue = value;
        isNeedChangeAttackAnim = true;
    }

    public void SetMaxHealth(int value)
    {
        this.MaxHealth = value;
    }

    public void AddSpeedPoint(int value)
    {
        this.SpeedPoint += value;
    }

    public void SetHealth(int value)
    {
        this.Health = value;
        OnHealthChange(roleType,value,MaxHealth);
    }
    public void SetAttackSpeed(int value)
    {
        this.AttackSpeed = value;
    }
    public void SetMoveSpeed(int value)
    {
        this.MoveSpeed = value;
    }
    public void SetSpeedPoint(int value)
    {
        this.SpeedPoint = value;
        OnRampageChange(roleType, SpeedPoint);
        if (SpeedPoint >= 100)
        {
            //rampage...
            OnRampage();
        }
    }
    /// <summary>
    /// 进入暴走状态
    /// </summary>
    private void Rampage()
    {
        refPowEffect.GetComponent<ParticleSystem>().Play();
        animator.speed = 5f;
        if(gameObject.GetComponent<PlayerMove>())
        {
            gameObject.GetComponent<PlayerMove>().SetSpeed(100);
        }
        if (gameObject.GetComponent<PlayerAttack>())
        {
            gameObject.GetComponent<PlayerAttack>().isRampaging = true;
        }
        if (gameObject.GetComponent<RemotePlayer>())
        {
            gameObject.GetComponent<RemotePlayer>().isRampaging = true;
        }
        isRampaging = true;
        Invoke("CancelRampage", 10f);
    }

    /// <summary>
    /// 进入暴走状态（子线程调用）
    /// </summary>
    public void RampageOnMain()
    {
        isNeedRampage = true;
    }

    /// <summary>
    /// 取消暴走状态
    /// </summary>
    public void CancelRampage()
    {
        refPowEffect.GetComponent<ParticleSystem>().Stop();
        animator.speed = 1f;
        if (gameObject.GetComponent<PlayerMove>())
        {
            gameObject.GetComponent<PlayerMove>().ResetSpeed();
        }
        if (gameObject.GetComponent<PlayerAttack>())
        {
            gameObject.GetComponent<PlayerAttack>().isRampaging = false;
        }
        if (gameObject.GetComponent<RemotePlayer>())
        {
            gameObject.GetComponent<RemotePlayer>().isRampaging = false;
        }
        isRampaging = false;
    }
}
