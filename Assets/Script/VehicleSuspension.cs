using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSuspension : MonoBehaviour
{
    [Header("Assigned Wheels")]
    public GameObject wheelL;
    public VehicleWheel wheeldataL;
    public GameObject wheelR;
    public VehicleWheel wheeldataR;
    private bool isWheelsAssigned = false;

    /*
    바퀴의 구동 좌표
    x: 구동, y: 조향 및 토, z: (서스펜션에 따라서 달라질 수 있음) 캠버
    */

    [Header("Steering")]
    public bool haveSteer;
    public float steerAngle = 32f;

    private float rotationAmount;

    [Header("Differential")]
    public bool haveDifferential;

    [Header("DriveTrain")]
    public bool allWheelsOnGround = false;

    void Start()
    {
        if(wheelL && wheelR){
            wheeldataL = wheelL.GetComponent<VehicleWheel>();
            wheeldataR = wheelR.GetComponent<VehicleWheel>();

            isWheelsAssigned = true;
        }   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckWheelsOnGround();
    }

    public void Steer(float steerPower){
        if(isWheelsAssigned && haveSteer){
            // steerPower에 따라 Y축으로 회전
            rotationAmount = steerPower * 0.01f * steerAngle; // 회전량 계산

            // 왼쪽 바퀴와 오른쪽 바퀴를 Y축으로 회전
            wheelL.transform.localEulerAngles = new Vector3(0, rotationAmount, 90);
            wheelR.transform.localEulerAngles = new Vector3(0, rotationAmount, 90);
        }
    }

    public void CheckWheelsOnGround(){
        if(wheeldataL.hasGrip && wheeldataR.hasGrip){
            allWheelsOnGround = true;
        }
        else{
            allWheelsOnGround = false;
        }
    }

    public void GivePowerToWheel(){
        if(allWheelsOnGround && haveDifferential){
            //여기에 코드 작성
        }
    }
}
