using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;

public class SelectUnitScript : MonoBehaviour
{
    public LayerMask m_LMUnit;
    public LayerMask m_LMBuilding;
    public GameObject m_BuildOK;
   // public bool m_IsSelect;
    static public List<PlayerMove> PM;
    BuildingStatus Bs;  // 현재 선택하는 빌딩
    PlayerMove Pm;  // 현재 선택하는 유닛

    static public SelectUnitScript m_Instance;
    static public SelectUnitScript Instance
    {
        get { return m_Instance; }
    }

    void Start()
    {

    }

    void Awake()
    {
        m_Instance = this;
        PM = new List<PlayerMove>();
        StartCoroutine("SelectRoutine");
    }


    IEnumerator SelectRoutine()
    {
        while (true)
        {
            if(Input.GetMouseButtonDown(0))// && m_IsSelect == false)
            //if (Input.touchCount == 1)//&& m_IsSelect == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //(Input.GetTouch(0).position); 
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LMUnit) && TouchScript.IsOver)
                {
                    if(Bs != null)
                    {
                        Bs.m_IsSelect = false;
                        Bs.imgSelectbar.enabled = false;
                    }

                    Pm = hit.transform.GetComponent<PlayerMove>();
                    if (Pm.m_IsBuild || Pm.m_IsSelect || m_BuildOK.activeSelf) goto CannotOrder;

                    //m_IsSelect = true;
                    

                    if (Pm.m_Team.gameObject.layer == 23)   // 빨강
                        UnitStatusScript.m_Instance.SetUnitImage(hit, 0);
                    else
                        UnitStatusScript.m_Instance.SetUnitImage(hit, 1);

                    if (PhotonNetwork.isMasterClient)
                    {
                        if (Pm.m_Team.gameObject.layer == 22)
                        {
                            UnitStatusScript.m_Instance.SetUnitImage(hit, 1);
                            UnitFuncScript.m_Instance.ClearFunc();
                            goto CannotOrder;
                        }
                            
                        else
                            UnitStatusScript.m_Instance.SetUnitImage(hit, 0);
                    }

                    else
                    {
                        if (Pm.m_Team.gameObject.layer == 23)
                        {
                            UnitStatusScript.m_Instance.SetUnitImage(hit, 0);
                            UnitFuncScript.m_Instance.ClearFunc();
                            goto CannotOrder;
                        }
                            
                        else
                            UnitStatusScript.m_Instance.SetUnitImage(hit, 1);
                    }

                    Pm.m_IsSelect = true;
                    Pm.imgSelectbar.enabled = true;
                    Pm.m_Animator.SetBool("IsPick", false);

                    Pm.StopCoroutine("Picking");
                    Pm.m_IsPick = false;
                    Pm.m_IsMineral = false;
                    Pm.m_IsAttack = false;
                    Pm.StopCoroutine("OrderRoutine");
                    Pm.StartCoroutine("OrderRoutine");
                    //m_IsSelect = true;
                    CannotOrder:;
                }

                else if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LMBuilding)
                    && TouchScript.IsOver && m_BuildOK.activeSelf == false)
                {
                    AllSelectOff();

                    Bs = hit.transform.GetComponent<BuildingStatus>();
                    if (Bs.GetComponentInChildren<Renderer>().material.color == Color.red)
                        goto CannotOrder;
                    //m_IsSelect = true;
                    Bs.m_IsSelect = true;
                    if (Bs.m_Team.gameObject.layer == 23)
                        UnitStatusScript.m_Instance.SetUnitImage(hit, 0);
                    else
                        UnitStatusScript.m_Instance.SetUnitImage(hit, 1);

                    Bs.imgSelectbar.enabled = true;
                    CannotOrder:;
                }

                else if (TouchScript.IsOver
                     && !Physics.Raycast(ray, out hit, Mathf.Infinity, m_LMUnit)
                     && !Physics.Raycast(ray, out hit, Mathf.Infinity, m_LMBuilding)
                     && m_BuildOK.activeSelf == false)
                {
                    AllSelectOff();
                }
            }

            yield return null;
        }
    }

    /*IEnumerator SelectRoutine()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))    // 유닛 선택
            //if (Input.touchCount == 1 && m_IsSelect == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //(Input.GetTouch(0).position); 
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LMUnit))
                {
                    PlayerMove Pm = hit.transform.GetComponent<PlayerMove>();
                    
                    //foreach (PlayerMove unit in PM)
                    //{
                    //    if (unit.m_SerialNum == Pm.m_SerialNum)
                    //    {
                    //        //Debug.Log("리스트에서 선택한 유닛 찾음.");
                    //        Pm = unit;
                    //    }
                    //}
                    PM.FindIndex()

                    if (Pm.m_IsBuild) goto CannotOrder;
                    
                    m_IsSelect = true;
                    Pm.m_IsSelect = true;
                    if (Pm.m_Team.gameObject.layer == 23)
                        UnitStatusScript.m_Instance.SetUnitImage(hit, 0);
                    else
                        UnitStatusScript.m_Instance.SetUnitImage(hit, 1);

                    Pm.imgSelectbar.enabled = true;
                    Pm.m_Animator.SetBool("IsPick", false);

                    Pm.StopCoroutine("Picking");
                    Pm.StopCoroutine("OrderRoutine");
                    Pm.StartCoroutine("OrderRoutine");
                    CannotOrder:;

                   
                }

                if (TouchScript.IsOver
                    && !Physics.Raycast(ray, out hit, Mathf.Infinity, m_LMUnit)
                    && !Physics.Raycast(ray, out hit, Mathf.Infinity, m_LMBuilding)
                    && m_BuildOK.activeSelf == false) 
                {
                    AllSelectOff();
                }
            }

            yield return null;
        }
    }*/

    IEnumerator IsAliveRoutine()
    {
        while (true)
        {
            for (int i = 0; i < PM.Count; i++)
            {
                if (PM[i].m_IsAlive == false)
                {
                    PM[i].Invoke("Death", 3f);
                    PM.Remove(PM[i]);
                }

            }

            yield return null;
        }
    }

    private void AllSelectOff()
    {
        Debug.Log("체크해제 호출됨");
        UnitFuncScript.m_Instance.ClearFunc();
        foreach (PlayerMove unit in PM)
        {
            //m_IsSelect = false;
            unit.m_IsSelect = false;
            unit.imgSelectbar.enabled = false;
        }

        if (Bs != null)
        {
            Bs.m_IsSelect = false;
            Bs.imgSelectbar.enabled = false;
        }

    }

    public void OrderToPick()
    {
        if (Pm.m_IsPick || m_BuildOK.activeSelf)
            return;
        Pm.m_IsAttack = false;
        Pm.m_IsMineral = false;
        Pm.m_IsPick = true;
        //Pm.imgSelectbar.enabled = false;
        //Pm.imgHpbar.enabled = false;
    }

    public void OrderToMineral()
    {
        if (Pm.m_IsMineral || m_BuildOK.activeSelf)
            return;
        Pm.m_IsPick = false;
        Pm.m_IsAttack = false;
        Pm.m_IsMineral = true;
        //Pm.imgSelectbar.enabled = false;
        //Pm.imgHpbar.enabled = false;
    }

    public void OrderToAttack()
    {
        if (Pm.m_IsAttack)
            return;
        StopCoroutine("SelectRoutine");
        Pm.m_IsPick = false;
        Pm.m_IsMineral = false;
        Pm.m_IsAttack = true;
        Pm.imgSelectbar.enabled = false;
        Pm.imgHpbar.enabled = false;
    }

    void OnDestroy()
    {
        m_Instance = null;
        StopAllCoroutines();
    }

}
