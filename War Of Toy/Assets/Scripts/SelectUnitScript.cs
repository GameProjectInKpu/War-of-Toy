﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;

public class SelectUnitScript : MonoBehaviour
{
    public LayerMask m_LMUnit;
    public LayerMask m_LMBuilding;
    public GameObject m_BuildOK;
    public GameObject m_PickImage;
   // public bool m_IsSelect;
    public List<PlayerMove> LivingUnit; // 현재 살아있는 유닛들
    BuildingStatus Bs;  // 현재 선택하는 빌딩
    public List<PlayerMove> SelectedUnit;  // 현재 선택한 유닛들

    public int AcceptableUnit;
    public bool IsSRrunning;
    //public bool m_IsUpgradeMode;
    //public Transform LabPos;

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
        //InvokeRepeating("SelectRoutine", 1f,0.01f);
        StartCoroutine("IsAliveRoutine");
    }




    IEnumerator SelectRoutine()
    {
        while (true)
        {
            Debug.Log("선택루틴 실행중");
            if(Input.GetMouseButton(0))
            //if (Input.touchCount == 1 )
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
                    
                   

                    if (Bs.m_Team.gameObject.layer == 23)
                        UnitStatusScript.m_Instance.SetUnitImage(hit.transform, 0, Bs.imgHpbar);
                    else
                        UnitStatusScript.m_Instance.SetUnitImage(hit.transform, 1, Bs.imgHpbar);

                    Bs.m_IsSelect = true;
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
    }

  

    IEnumerator IsAliveRoutine()
    {
        while (true)
        {
            foreach (PlayerMove unit in SelectedUnit)
            {
                Debug.Log(unit.tag);
            }

            foreach (PlayerMove unit in LivingUnit)
            {
                if(unit.m_Hp <= 0f)
                {
                    unit.StopAllCoroutines();
                    unit.m_IsAlive = false;
                    unit.Invoke("Death", 3f);
                    LivingUnit.Remove(unit);
                    SelectedUnit.Remove(unit);
                }

                if (unit.m_IsPM && unit.HitPM == null)    // 타겟 유닛이 죽었을때
                    unit.m_IsPM = false;
                if (unit.m_IsBS && unit.HitBS == null)    // 타겟 건물이 죽었을때
                    unit.m_IsBS = false;

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



        if(IsMyTeam(Unit) == true)
            UnitStatusScript.m_Instance.m_IsMayTeam = true;
            
            
        else
        {
            UnitStatusScript.m_Instance.m_IsMayTeam = false;
            goto CannotOrder;
        }

        UnitStatusScript.m_Instance.SetUnitImage(unit, 0, Unit.imgHpbar);


        SelectedUnit.Add(Unit);

        m_PickImage.SetActive(false);
        Unit.m_IsSelect = true;
        Unit.imgSelectbar.enabled = true;
        Unit.m_Animator.SetBool("IsPick", false);

        Unit.StopCoroutine("Picking");
        Unit.m_IsPick = false;
        Unit.m_IsMineral = false;
        Unit.m_IsAttack = false;
        Unit.m_IsHeal = false;
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
            unit.m_IsHeal = false;
            unit.HitPM = null;
            unit.HitBS = null;
            if (unit.gameObject.tag == "UnitCupid")
                unit.HealArea.gameObject.SetActive(false);
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
            if (unit.gameObject.tag != "UnitLego" || unit.m_IsMineral || m_BuildOK.activeSelf)
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
            if (unit.gameObject.tag == "UnitCupid" || unit.gameObject.tag == "UnitLego" || unit.m_IsAttack)
                return;
            StopCoroutine("SelectRoutine");
            unit.m_IsPick = false;
            unit.m_IsMineral = false;
            unit.HitPM = null;
            unit.HitBS = null;
            unit.m_Animator.SetBool("IsAttack", false);
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
            unit.HitPM = null;
            unit.HitBS = null;
            unit.m_IsBoard = true;
            unit.imgSelectbar.enabled = false;
            unit.imgHpbar.enabled = false;
        }

    }

    public void OrderToHeal()
    {
        foreach (PlayerMove unit in SelectedUnit)
        {
            if (unit.gameObject.tag != "UnitCupid" || unit.m_IsHeal)
                return;
            unit.m_IsPick = false;
            unit.m_IsMineral = false;
            unit.m_IsAttack = false;
            unit.m_IsHeal= true;
            unit.imgSelectbar.enabled = false;
            unit.imgHpbar.enabled = false;
        }

    }

    //public void OrderToDrop()
    //{
    //    foreach (PlayerMove unit in SelectedUnit)
    //    {
    //        if (unit.m_IsBoard)
    //            return;
    //        StopCoroutine("SelectRoutine");
    //        unit.m_IsPick = false;
    //        unit.m_IsMineral = false;
    //        unit.m_IsAttack = false;
    //        unit.m_IsBoard = true;
    //        unit.imgSelectbar.enabled = false;
    //        unit.imgHpbar.enabled = false;
    //    }

    //}


    public bool IsMyTeam (PlayerMove Unit)//같은 팀인지 검사
    {
        if (PhotonNetwork.isMasterClient)   // 같은 편인지 검사
        {
            if (Unit.m_Team.gameObject.layer == 22)
                return false;

            else
                return true;
        }

        else
        {
            if (Unit.m_Team.gameObject.layer == 23)
                return false;

            else
                return true;
        }
    }

    void OnDestroy()
    {
        m_Instance = null;
        StopAllCoroutines();
    }

}
