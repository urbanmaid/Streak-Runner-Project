using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPart : MonoBehaviour
{
    [Header("Mechanicals")]
    public GameObject nextPathPoint;

    private Vector3 nextPPLoc;
    private Vector3 nextPPRot;
    public string pathType;
    /*
        사용 가능한 값: Straight, Curve_L, Curve_R, Intersection, Ascend_start, Ascend_mid, Ascend_end, Descend_start, Descend_mid, Descend_end, "그 외"
        "그 외"에 해당하는 pathType은 Straight랑 똑같이 취급함
    */

    public int pathLength;

    [Header("Props")]
    public GameObject streetLightPos;
    public GameObject roadSignPos;
    public GameObject backgroundPosL;
    public GameObject backgroundPosR;

    // Start is called before the first frame update
    void Awake()
    {
        nextPPLoc = nextPathPoint.transform.localPosition;
        nextPPRot = nextPathPoint.transform.rotation.eulerAngles;
        /*
        Debug.Log("Next Path StartPoint Location and Rotation: ("
        + nextPPLoc + "), ("
        + nextPPRot +")"
        );
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Installs Lights On Road (public for other object calls)
    public void InstallLights(GameObject lightsAsset){
        //, GameObject[] roadsignAssetGroup
        if(lightsAsset && streetLightPos){
            Debug.Log("Got Light Asset");

            //Transform lightsUnit;
            Vector3 lightUnitPos;
            Vector3 lightUnitRot;

            int childLength = streetLightPos.transform.childCount;
            for(int i = 0; i < childLength; i++){
                //lightsUnit = streetLightPos.transform.GetChild(i);
                lightUnitPos = streetLightPos.transform.GetChild(i).position;
                lightUnitRot = streetLightPos.transform.GetChild(i).eulerAngles;
                Instantiate(lightsAsset, lightUnitPos, Quaternion.Euler(lightUnitRot));

                //lightsAsset.transform.SetParent(streetLightGroup.transform);
            }
        }

        if(streetLightPos){
            int childLength = streetLightPos.transform.childCount;
            Debug.Log("Light Pos Count: " + childLength);
        }

    }
}
