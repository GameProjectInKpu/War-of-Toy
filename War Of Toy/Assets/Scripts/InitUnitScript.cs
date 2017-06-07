using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitUnitScript : MonoBehaviour {

    GameObject Obj;
    public GameObject InitUnitBlue;
    public GameObject InitUnitRed;

    Vector3 UnitPos;
    public static Transform m_Factory;//m_Camera;
    //private MoveCamera m_CameraMove;

    private void Awake()
    {
        //m_Camera = Camera.main.transform;
        //m_CameraMove = m_Camera.GetComponent<MoveCamera>();
    }

    void Update () {
		
	}

    public void InitUnitByButton()
    {
        FactoryScript.m_Animator.SetBool("IsWork", true);
        UnitPos = m_Factory.position;
        if ((UnitPos.x > 60f && UnitPos.z < 25f) || (UnitPos.x < 35f && UnitPos.z > 73f))
            UnitPos.y = 4.5f;
        else
            UnitPos.y = 0.15f;
        UnitPos.z -= 5f;
        //UnitPos.y = 0.15f;

        if (PhotonNetwork.isMasterClient)
        {
            Obj = (GameObject)PhotonNetwork.Instantiate(this.InitUnitRed.name, UnitPos, Quaternion.Euler(Vector3.zero), 0);
            if(Obj.transform.tag != "UnitAirballoon")
                Obj.GetComponent<PlayerMove>().enabled = true;
        }
        else
        {
            Obj = (GameObject)PhotonNetwork.Instantiate(this.InitUnitBlue.name, UnitPos, Quaternion.Euler(Vector3.zero), 0);
            if (Obj.transform.tag != "UnitAirballoon")
                Obj.GetComponent<PlayerMove>().enabled = true;
        }

        //GameObject Obj = (GameObject)Instantiate(InitUnit, UnitPos, Quaternion.Euler(Vector3.zero));
        //Obj.GetComponent<PlayerMove>().enabled = true;
        Invoke("IsWorkFalse", 5f);
    }

    void IsWorkFalse()
    {
        FactoryScript.m_Animator.SetBool("IsWork", false);
    }
}
