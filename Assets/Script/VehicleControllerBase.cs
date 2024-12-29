using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VehicleControllerBase : MonoBehaviour
{
    #region Variables
    [Header("Main Asset")]
    public GameObject[] suspensions;
    List<VehicleSuspension> vehicleSus = new List<VehicleSuspension>();
    List<Vector3> vehicleSusLocation = new List<Vector3>();
    List<bool> vehicleSusPropulsion = new List<bool>();
    private Rigidbody rb;
    
    [Header("PowerTrain")]

    public GameObject powertrain;
    public Vector3 driveWheelCenter = new Vector3(0f, 0f, 0f);
    public float wheelbase;
    public float power = 10f; // 힘의 크기

    [Header("Steer Options")]
    public float steerPos = 0f; // 조향 값
    public float steerSensitivity = 100f; // 조향 변화 속도
    public float maxSteerValue = 100f; // 최대 조향 값
    #endregion

    #region Start Func
    // Start is called before the first frame update
    void Start()
    {
        InitializeSuspensions();
        rb = GetComponent<Rigidbody>();
    }

    void InitializeSuspensions()
    {
        bool usedForPropulsion = false;

        foreach(GameObject suspension in suspensions){
            VehicleSuspension sus = suspension.GetComponent<VehicleSuspension>();

            if(sus){
                vehicleSus.Add(sus);
                Debug.Log("Object '" + suspension + "' has VehicleSuspension.");
                usedForPropulsion = sus.haveDifferential;
            }

            // 구동륜 중심 지정하기
            Transform susTransform = suspension.transform;

            if(usedForPropulsion){
                vehicleSusPropulsion.Add(true);
            }
            else{
                vehicleSusPropulsion.Add(false);
            }

            vehicleSusLocation.Add(susTransform.localPosition);
        }

        // vehicleSusPropulsion은 vehicleSusLocation와 길이가 같아야 하며 그렇지 않을 경우 경고
        if(vehicleSusPropulsion.Count != vehicleSusLocation.Count){
            Debug.LogError("Error: The count of axle and axle propulsion list is not fit");
        }

        //구동륜 위치를 통한 구동 중심축 설정하기
        if(vehicleSusPropulsion.Count != 1){
            for(int i = 0; i < vehicleSusLocation.Count; i++){
                if(vehicleSusPropulsion[i]){
                    driveWheelCenter += vehicleSusLocation[i];
                }
            }
            driveWheelCenter /= vehicleSusLocation.Count;
        }

        //휠베이스 지정하기
        wheelbase = vehicleSusLocation[0].z - vehicleSusLocation[vehicleSusLocation.Count - 1].z;
    }

    #endregion

    // Update is called once per frame
    void FixedUpdate()
    {
        
        Steer();
        ApplySteer();
        Accelate();
    }

    // 조향 관리
    void Steer()
    {
        // Steer_L과 Steer_R 버튼 눌림 여부 감지
        bool isSteerLeft = Input.GetButton("Steer_L");
        bool isSteerRight = Input.GetButton("Steer_R");

        // 조향 값 초기화
        if (isSteerLeft)
        {
            steerPos = Mathf.MoveTowards(steerPos, -maxSteerValue, steerSensitivity * Time.deltaTime);
        }
        else if (isSteerRight)
        {
            steerPos = Mathf.MoveTowards(steerPos, maxSteerValue, steerSensitivity * Time.deltaTime);
        }
        else
        {
            // 평상시에는 0으로 자연스럽게 변화
            steerPos = Mathf.MoveTowards(steerPos, 0f, steerSensitivity * Time.deltaTime);
        }
    }

    // 조향 각도를 서스펜션에 적용하기
    void ApplySteer(){
        foreach(VehicleSuspension vehsus in vehicleSus){
            vehsus.Steer(steerPos);
        }
    }

    void Accelate(){
        bool isOnAccel = Input.GetButton("GasPedal");

        if(isOnAccel){
            transform.Rotate(new Vector3(0f, (transform.rotation.y + steerPos * 0.1f), 0f), Space.Self);
            //rb.AddForceAtPosition(transform.forward * power, driveWheelCenter, ForceMode.Force);
        }
    }

}
