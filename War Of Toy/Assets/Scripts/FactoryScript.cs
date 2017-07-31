using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryScript : MonoBehaviour {

    //public LayerMask m_LMBuilding;
   // FactoryScript Fs;
    //public bool m_IsSelect;
    public bool m_IsAlive;

    public static Animator m_Animator;

    private void OnEnable()
    {
        
    }
    void Awake () {
        m_IsAlive = true;
        m_Animator = GetComponentInChildren<Animator>();
        //Fs = transform.GetComponent<FactoryScript>();
    }



    void Update () {
        if (m_IsAlive == false)
        {
            Invoke("Death", 3f);
        }


        //if (Input.GetMouseButtonDown(0) && m_IsSelect == false)     // 유닛 선택
        //if (Input.touchCount == 1 && m_IsSelect == false)
        /*{
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LMBuilding))
            {
                //Fm = hit.transform.GetComponent<FactoryScript>();
                //Fm.m_IsSelect = true;// m_IsSelect = true; //
                //UnitStatusScript.m_Instance.SetUnitImage(hit, 0);
                //InitUnitScript.m_Factory = Fm.transform;
                //Fm.m_IsSelect = false;

                GetComponent<FactoryScript>();
                m_IsSelect = true;// 
                UnitStatusScript.m_Instance.SetUnitImage(hit, 0);
                InitUnitScript.m_Factory = transform;
                m_IsSelect = false;
            }

        }*/
        //InitUnitScript.m_Factory = Fs.transform;
    }

    public void Death()
    {
        PhotonNetwork.Destroy(gameObject);
        
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
