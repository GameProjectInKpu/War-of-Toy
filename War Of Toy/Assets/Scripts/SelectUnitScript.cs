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
    public List<PlayerMove> LivingUnit; // 현재 살아있는 유닛들
    BuildingStatus Bs;  // 현재 선택하는 빌딩
    public List<PlayerMove> SelectedUnit;  // 현재 선택한 유닛들

    public int AcceptableUnit;
    public bool IsSRrunning;

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
        LivingUnit = new List<PlayerMove>();
        AcceptableUnit = 10;
        StartCoroutine("SelectRoutine");
        StartCoroutine("IsAliveRoutine");
    }


    IEnumerator SelectRoutine()
    {
        IsSRrunning = true;
        while (true)
        {
            
            Debug.Log("선택루틴 실행중");
            if(Input.GetMouseButtonDown(0))// && m_IsSelect == false)
            //if (Input.touchCount == 1 && TouchScript.IsOver)//&& m_IsSelect == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //(Input.GetTouch(0).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LMUnit) && TouchScript.m_Instance.IsOver)
                    SelectUnit(hit.transform);

                else if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LMBuilding)
                    && TouchScript.m_Instance.IsOver && m_BuildOK.activeSelf == false)
                {
                    AllSelectOff();

                    Bs = hit.transform.GetComponent<BuildingStatus>();
                    if (Bs.GetComponentInChildren<Renderer>().material.color == Color.red)
                        goto CannotOrder;
                    Bs.m_IsSelect = true;

                    if (Bs.m_Team.gameObject.layer == 23)
                        UnitStatusScript.m_Instance.SetUnitImage(hit.transform, 0);
                    else
                        UnitStatusScript.m_Instance.SetUnitImage(hit.transform, 1);

                    Bs.imgSelectbar.enabled = true;
                    CannotOrder:;
                }

                else if (TouchScript.m_Instance.IsOver
                     && !Physics.Raycast(ray, out hit, Mathf.Infinity, m_LMUnit)
                     && !Physics.Raycast(ray, out hit, Mathf.Infinity, m_LMBuilding)
                     && m_BuildOK.activeSelf == false)
                {
                    AllSelectOff();
                }
            }

            yield return null;
            
        }
        IsSRrunning = false;
    }

  

    IEnumerator IsAliveRoutine()
    {
        while (true)
        {
            foreach (PlayerMove unit in LivingUnit)
            {
                if (unit.m_IsAlive == false)
                {
                    unit.Invoke("Death", 3f);
                    LivingUnit.Remove(unit);
                    SelectedUnit.Remove(unit);
                }

            }

            //if (IsSRrunning == false)
            //    StartCoroutine("SelectedRoutine");

            yield return null;
        }
    }

    public void SelectUnit(Transform unit)
    {
        if (Bs != null)
        {
            Bs.m_IsSelect = false;
            Bs.imgSelectbar.enabled = false;
        }

        PlayerMove Unit = unit.GetComponent<PlayerMove>();

        if (Unit.m_IsBuild || Unit.m_IsSelect || m_BuildOK.activeSelf) goto CannotOrder;

        if (PhotonNetwork.isMasterClient)
        {
            if (Unit.m_Team.gameObject.layer == 22)
            {
                UnitStatusScript.m_Instance.SetUnitImage(unit, 1);
                UnitFuncScript.m_Instance.ClearFunc();
                goto CannotOrder;
            }

            else
                UnitStatusScript.m_Instance.SetUnitImage(unit, 0);
        }

        else
        {
            if (Unit.m_Team.gameObject.layer == 23)
            {
                UnitStatusScript.m_Instance.SetUnitImage(unit, 0);
                UnitFuncScript.m_Instance.ClearFunc();
                goto CannotOrder;
            }

            else
                UnitStatusScript.m_Instance.SetUnitImage(unit, 1);
        }

        SelectedUnit.Add(Unit);

        Unit.m_IsSelect = true;
        Unit.imgSelectbar.enabled = true;
        Unit.m_Animator.SetBool("IsPick", false);

        Unit.StopCoroutine("Picking");
        Unit.m_IsPick = false;
        Unit.m_IsMineral = false;
        Unit.m_IsAttack = false;
        Unit.StopCoroutine("OrderRoutine");
        Unit.StartCoroutine("OrderRoutine");
        CannotOrder:;
    }


    private void AllSelectOff()
    {
        Debug.Log("체크해제 호출됨");
        UnitFuncScript.m_Instance.ClearFunc();
        foreach (PlayerMove unit in LivingUnit)
        {
            unit.m_IsSelect = false;
            unit.imgSelectbar.enabled = false;
        }


        SelectedUnit.Clear();

        if (Bs != null)
        {
            Bs.m_IsSelect = false;
            Bs.imgSelectbar.enabled = false;
        }

    }

    public void OrderToPick()
    {
        foreach (PlayerMove unit in SelectedUnit)
        {
            if (unit.m_IsPick || m_BuildOK.activeSelf)
                return;
            unit.m_IsAttack = false;
            unit.m_IsMineral = false;
            unit.m_IsPick = true;

        }
        //SelectedUnit.imgSelectbar.enabled = false;
        //SelectedUnit.imgHpbar.enabled = false;
    }

    public void OrderToMineral()
    {
        foreach (PlayerMove unit in SelectedUnit)
        {
            if (unit.m_IsMineral || m_BuildOK.activeSelf)
                return;
            unit.m_IsPick = false;
            unit.m_IsAttack = false;
            unit.m_IsMineral = true;
        }
        //SelectedUnit.imgSelectbar.enabled = false;
        //SelectedUnit.imgHpbar.enabled = false;
    }

    public void OrderToAttack()
    {
        foreach (PlayerMove unit in SelectedUnit)
        {
            if (unit.m_IsAttack)
                return;
            StopCoroutine("SelectRoutine");
            unit.m_IsPick = false;
            unit.m_IsMineral = false;
            unit.m_IsAttack = true;
            unit.imgSelectbar.enabled = false;
            unit.imgHpbar.enabled = false;
        }
        
    }

    public void OrderToBoard()
    {
        foreach (PlayerMove unit in SelectedUnit)
        {
            if (unit.m_IsBoard)
                return;
            StopCoroutine("SelectRoutine");
            unit.m_IsPick = false;
            unit.m_IsMineral = false;
            unit.m_IsAttack = false;
            unit.m_IsBoard = true;
            unit.imgSelectbar.enabled = false;
            unit.imgHpbar.enabled = false;
        }

    }

    void OnDestroy()
    {
        m_Instance = null;
        StopAllCoroutines();
    }

}
