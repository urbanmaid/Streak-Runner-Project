using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleWheel : MonoBehaviour
{
    [Header("Options")]
    public bool hasGrip;
    public string groundMaterial; //a

    void Start()
    {
 
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void OnTriggerEnter(Collider other){
        hasGrip = true;
        groundMaterial = other.transform.tag;
        Debug.Log(this + " has Grip.");
    }

    private void OnTriggerExit(Collider other){
        hasGrip = false;
        groundMaterial = "";
        Debug.Log(this + " lost Grip.");
    }
}
