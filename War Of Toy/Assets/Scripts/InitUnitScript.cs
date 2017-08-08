﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitUnitScript : MonoBehaviour {

    GameObject Obj;
    public GameObject InitUnitBlue;
    public GameObject InitUnitRed;

    Vector3 UnitPos;
    public static Transform m_Castle;
    public static Transform m_Factory;//m_Camera;
    public static Transform m_Hospital;
    //private MoveCamera m_CameraMove;

    private void Awake()
    {
        //m_Camera = Camera.main.transform;
        //m_CameraMove = m_Camera.GetComponent<MoveCamera>();
    }

    void Update () {
		
	}

    public void InitUnitByButton(int unit)
    {
        if(CurUnitNum.m_Instance.m_UnitNum + 1 > SelectUnitScript.m_Instance.AcceptableUnit)
        {
            Debug.Log("유닛을 더이상 수용할 수 없습니다. 숙소를 늘려주세요");
            return;
        }
        switch (unit)
        {
            case 0: // 성에서 생성
                //CastleScript.m_Animator.SetBool("IsWork", true);
                UnitPos = m_Castle.position;
                UnitPos.y = 4.5f;

                UnitPos.z -= 5f;
                break;

            case 1: // 공장에서 생성
                FactoryScript.m_Animator.SetBool("IsWork", true);
                UnitPos = m_Factory.position;
                if ((UnitPos.x > 60f && UnitPos.z < 25f) || (UnitPos.x < 35f && UnitPos.z > 73f))
                    UnitPos.y = 4.5f;
                else
                    UnitPos.y = 0.15f;
                UnitPos.z -= 5f;
                //UnitPos.y = 0.15f;
                break;

            case 2: // 병원에서 생성
                
                break;


        }


        if (PhotonNetwork.isMasterClient)
        {
            Obj = (GameObject)PhotonNetwork.Instantiate(this.InitUnitRed.name, UnitPos, Quaternion.Euler(Vector3.zero), 0);
            Debug.Log(Obj.transform.tag);
            if (Obj.transform.tag != "UnitAirballoon")
                Obj.GetComponent<PlayerMove>().enabled = true;
            PlayerMove Pm = Obj.GetComponent<PlayerMove>();
            SelectUnitScript.m_Instance.LivingUnit.Add(Pm);
            ++CurUnitNum.m_Instance.m_UnitNum;
        }
        else
        {
            Obj = (GameObject)PhotonNetwork.Instantiate(this.InitUnitBlue.name, UnitPos, Quaternion.Euler(Vector3.zero), 0);
            if (Obj.transform.tag != "UnitAirballoon")
                Obj.GetComponent<PlayerMove>().enabled = true;
            PlayerMove Pm = Obj.GetComponent<PlayerMove>();
            SelectUnitScript.m_Instance.LivingUnit.Add(Pm);
            ++CurUnitNum.m_Instance.m_UnitNum;
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
