using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour 
{
    public Transform target;
    private Vector3 offSet = new Vector3(0,15f,-13f);
    private float smoothLevel = 2f;



    private void LateUpdate()
    {
        Vector3 targetPos = target.position + offSet;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothLevel * Time.deltaTime);
        transform.LookAt(target);
        //ScrollView();
    }

    public float maxScroll;
    public float minScroll;
    public float maxYRotateAngles = 70;
    public float minYRotateAngles = 5;
    public float rotateSpeed = 5;
    public float scrollSpeed = 10;

    private float distance;
    private Quaternion currentRotation;

    //void ScrollView()
    //{
    //    //限制最大缩放
    //    if (Input.GetAxis("Mouse ScrollWheel") < 0 && offSet.magnitude < maxScroll)
    //    {
    //        //获取偏移量的模
    //        distance = offSet.magnitude;
    //        distance += System.Math.Abs(Input.GetAxis("Mouse ScrollWheel")) * scrollSpeed;
    //        //通过获得单位向量*新的偏移模更新偏移信息
    //        offSet = offSet.normalized * distance;
    //    }
    //    //限制最小缩放
    //    if (Input.GetAxis("Mouse ScrollWheel") > 0 && offSet.magnitude > minScroll)
    //    {
    //        //获取偏移量的模
    //        distance = offSet.magnitude;
    //        distance += -System.Math.Abs(Input.GetAxis("Mouse ScrollWheel")) * scrollSpeed;
    //        //通过获得单位向量*新的偏移模更新偏移信息
    //        offSet = offSet.normalized * distance;
    //    }
    //}
}






