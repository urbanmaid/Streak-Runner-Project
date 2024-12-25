using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.WSA;
using UnityEngine.AI;

using Random = UnityEngine.Random;

public class TrackGenerator : MonoBehaviour
{
    public string pathPartDirectory = "Prefabs/Tracks";
    public int trackLength;
    private int trackLengthCur = 0;

    [Header("Path Parts")]
    public GameObject[] pathParts;
    private string[] pathPartsDir;
    public GameObject pathPartFinishing;


    public PathPart path;
    public GameObject objTest;
    private Vector3 nextPPLoc = new Vector3(0, 0, 0);
    private Vector3 nextPPRot = new Vector3(0, 0, 0);

    [Header("Props")]
    public GameObject streetLightObj;
    public bool hasStreetLightGroup = false;

    // Start is called before the first frame update
    void Start()
    {
        if(path){
            nextPPLoc = path.nextPathPoint.transform.localPosition;
            nextPPRot = path.nextPathPoint.transform.rotation.eulerAngles;
            Debug.Log("Declared Track Part Type: (" + path.pathType + "), StartPoint Location and Rotation: ("
            + nextPPLoc + "), ("
            + nextPPRot + ")" );
        }

        //LoadPathPartData(UnityEngine.Application.dataPath + pathPartDirectory);
        //CheckRoadSignInstall();

        GenerateTrack();
    }

    private void LoadPathPartData(string path) //Load Path Parts From Directory (미사용)
    {
        if(Directory.Exists(path)) {
            //Debug.Log("지정한 경로에서 가져오기 성공: " + path);

            pathPartsDir = Directory.GetFiles(path, "*.prefab");
            foreach(string dir in pathPartsDir){
                // .prefab 파일의 경로에서 "Assets/"를 제거하고, "/"를 대신 "\\"로 전환
                string assetPath = dir.Replace("\\", "/");

                // 프리팹 로드
                objTest = Resources.Load<GameObject>(assetPath);
                Debug.Log(assetPath);
                //path = prefab;
            }
        }
        else {
            Debug.LogError("지정한 경로가 존재하지 않습니다: " + path);
        }
    }

    private void GenerateTrack()
    {
        string trackTypeCur = "";
        GameObject trackPart;

        while (trackLengthCur < trackLength)
        {
            // pathParts에서 랜덤으로 하나를 선택 (조건 포함)
            GameObject part = pathParts[Random.Range(0, pathParts.Length)];

            // 부품을 인스턴스화
            path = part.GetComponent<PathPart>();

            // 부품을 인스턴스화하기: 인스턴스 위치와 각도는 각각 nextPPLoc, nextPPRot으로 지정
            trackPart = Instantiate(part, nextPPLoc, Quaternion.Euler(nextPPRot));

            // trackPart의 nextPathPoint를 가져옵니다.
            PathPart pathInfo = trackPart.GetComponent<PathPart>();

            // 어셋 설치
            if(hasStreetLightGroup){
                pathInfo.InstallLights(streetLightObj);
            }

            // 다음 위치와 회전을 업데이트: nextPathPoint의 월드 위치와 회전 사용
            nextPPLoc = pathInfo.nextPathPoint.transform.position; // 월드 위치로 업데이트
            nextPPRot = pathInfo.nextPathPoint.transform.eulerAngles; // 월드 회전으로 업데이트

            // 트랙 길이 계산하기
            trackLengthCur += pathInfo.pathLength;
            trackTypeCur = path.pathType;

            Debug.Log("Path Name: " + path.name
                + ", L: " + trackLengthCur
                + ", T: " + path.pathType
                //+ ", \tStartPoint Location and Rotation: ("
                //+ nextPPLoc + "\t), ("
                //+ nextPPRot + ")"
                ); // 로그 남기기용, 실제화 시 미사용예정임

            
        }

        // 마지막 트랙 파트 추가(결승선 구현)
        if(pathPartFinishing) {
            trackPart = Instantiate(pathPartFinishing, nextPPLoc, Quaternion.Euler(nextPPRot));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
