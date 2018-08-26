using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : BaseManager 
{
    private GameObject cameraGo;
    private Animator cameraAnimator;
    private CameraFollow cameraFollow;

    private Vector3 originalPosition;
    private Vector3 originalRotation;

    public CameraManager(GameFacade facade) : base(facade) { }

    public override void OnInit()
    {
        cameraGo = Camera.main.gameObject;
        cameraAnimator = cameraGo.GetComponent<Animator>();
        cameraFollow = cameraGo.GetComponent<CameraFollow>();
    }

    public void TurnToFollow()
    {
        cameraFollow.target = facade.GetCurrentGO().transform;
        cameraAnimator.enabled = false;
        originalPosition = cameraGo.transform.position;
        originalRotation = cameraGo.transform.eulerAngles;

        Quaternion targetQuaternion = Quaternion.LookRotation(cameraFollow.target.position - cameraGo.transform.position);
        cameraGo.transform.DORotateQuaternion(targetQuaternion, 1f).OnComplete(delegate() 
        {
            cameraFollow.enabled = true;
        });
    }

    public void TurnToAnim()
    {
        cameraFollow.enabled = false;
        cameraGo.transform.DOMove(originalPosition, 1f);
        cameraGo.transform.DORotate(originalRotation, 1f).OnComplete(() =>
         {
             cameraAnimator.enabled = true;
         });
    }

}
