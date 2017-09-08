using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitUnitScript : MonoBehaviour {

    public AudioClip soundAddHouse;

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
            NoticeScript.m_Instance.Notice("숙소를 늘려주십시오\n");
            NoticeScript.m_Instance.PlaySound(soundAddHouse);
            return;
        }
        switch (unit)
        {
            case 0: // 성에서 생성
                //CastleScript.m_Animator.SetBool("IsWork", true);
                UnitPos = m_Castle.position;
                UnitPos.x += Random.Range(-1.5f, 1.5f);
                UnitPos.y = 4.5f;

                if(UnitPos.z < 6f)
                    UnitPos.z += 5f;
                else
                    UnitPos.z -= 5f;
                break;

            case 1: // 공장에서 생성
                //FactoryScript.m_Animator.SetBool("IsWork", true);
                UnitPos = m_Factory.position;
                UnitPos.x += Random.Range(-1.5f, 1.5f);
                UnitPos.z -= 5f;
                UnitPos.y += 0.05f;
                break;

            case 2: // 병원에서 생성
                UnitPos = m_Hospital.position;

                UnitPos.z -= 5f;
                break;


        }


        if (PhotonNetwork.isMasterClient)
        {
            Obj = (GameObject)PhotonNetwork.Instantiate(this.InitUnitRed.name, UnitPos, Quaternion.Euler(Vector3.zero), 0);
            //if (Obj.transform.tag != "UnitAirballoon")
                Obj.GetComponent<PlayerMove>().enabled = true;
            if (Obj.transform.tag == "UnitAirballoon")
                UnitFuncScript.m_Instance.IsInitAirUnit = true;
            PlayerMove unitRed = Obj.GetComponent<PlayerMove>();
            SelectUnitScript.m_Instance.LivingUnit.Add(unitRed);
            //SelectUnitScript.m_Instance.LivingEnemyUnit.Add(unitBlue);
            ++CurUnitNum.m_Instance.m_UnitNum;
        }
        else
        {
            Obj = (GameObject)PhotonNetwork.Instantiate(this.InitUnitBlue.name, UnitPos, Quaternion.Euler(Vector3.zero), 0);
            //if (Obj.transform.tag != "UnitAirballoon")
                Obj.GetComponent<PlayerMove>().enabled = true;
            if (Obj.transform.tag == "UnitAirballoon")
                UnitFuncScript.m_Instance.IsInitAirUnit = true;
            PlayerMove unitBlue = Obj.GetComponent<PlayerMove>();
            SelectUnitScript.m_Instance.LivingUnit.Add(unitBlue);
            //SelectUnitScript.m_Instance.LivingEnemyUnit.Add(unitRed);
            ++CurUnitNum.m_Instance.m_UnitNum;
        }

        //GameObject Obj = (GameObject)Instantiate(InitUnit, UnitPos, Quaternion.Euler(Vector3.zero));
        //Obj.GetComponent<PlayerMove>().enabled = true;
        //Invoke("IsWorkFalse", 5f);
    }

    //void IsWorkFalse()
    //{
        //FactoryScript.m_Animator.SetBool("IsWork", false);
    //}
}
