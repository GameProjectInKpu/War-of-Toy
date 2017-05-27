using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixHpbarScript : MonoBehaviour {

    Vector3 Rot;


    void Start () {
        Rot = transform.rotation.eulerAngles;

    }
	
	
	void Update () {
        
        Rot.y = 0f;
        Rot.x = 30f;
        //transform.rotation = Quaternion.LookRotation(Rot);
        transform.rotation = Quaternion.Euler(Rot);

    }
}
