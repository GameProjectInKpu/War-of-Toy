using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildScript : MonoBehaviour {

    public GameObject building;
    public Transform CamAxis;
	

    public void Build()
    {
        Vector3 Pos = CamAxis.position;
        Quaternion Rot = CamAxis.rotation;
        Pos.z += 20;
        Pos.y = 0f;
        Rot.x = 0f;

        GameObject Obj = (GameObject)Instantiate(building, Pos, Rot);
    }
}
