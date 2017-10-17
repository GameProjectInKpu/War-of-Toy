using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;

public class SelectUnitScript : MonoBehaviour
{
    //public LayerMask m_LMFog;
    public LayerMask m_LMUnit;
    public LayerMask m_LMBuilding;
    public GameObject m_BuildOK;
    public GameObject m_PickImage;
    public GameObject m_Victory;
    public GameObject m_Defeat;

    // public bool m_IsSelect;
    //public List<PlayerMove> LivingUnit; // 현재 살아있는 유닛들
    //public List<PlayerMove> LivingEnemyUnit; // 현재 살아있는 유닛들

    public List<PlayerMove> LivingRedUnit; // 현재 살아있는 red유닛들
    public List<PlayerMove> LivingBlueUnit; // 현재 살아있는 blue유닛들

    public List<BuildingStatus> LivingRedBuilding; // 현재 살아있는 red유닛들
    public List<BuildingStatus> LivingBlueBuilding; // 현재 살아있는 blue유닛들

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
        LivingRedUnit = new List<PlayerMove>();
        LivingBlueUnit = new List<PlayerMove>();

        LivingRedBuilding = new List<BuildingStatus>();
        LivingBlueBuilding = new List<BuildingStatus>();

        AcceptableUnit = 10;
        StartCoroutine("SelectRoutine");
        //InvokeRepeating("SelectRoutine", 0f,0.01f);
        StartCoroutine("IsAliveRoutine");
    }




    IEnumerator SelectRoutine()
    {
        while (true)
        {
            Debug.Log("선택루틴 실행중");
            //if(Input.GetMouseButton(0))
            if (Input.touchCount == 1 )
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position); //(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LMUnit) && TouchScript.m_Instance.IsOver)
                        SelectUnit(hit.transform);

                //else if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LMFog))
                //{
                //    Debug.Log(hit.transform.tag);
                //    Debug.Log(hit.transform.gameObject.GetComponent<Renderer>().material.color.b);
                //}

                else if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LMBuilding)
                    && TouchScript.m_Instance.IsOver && m_BuildOK.activeSelf == false)
                {
                    AllSelectOff();

                    Bs = hit.transform.GetComponent<BuildingStatus>();
                    if (Bs.GetComponentInChildren<Renderer>().material.color == Color.red)
                        goto CannotOrder;
                    
                   

                    if (IsBuildingMyTeam(Bs))
                    {
                        UnitStatusScript.m_Instance.m_IsMyTeam = true;
                        UnitStatusScript.m_Instance.SetUnitImage(hit.transform, 0, Bs.imgHpbar);
                    }
                        
                    else
                    {
                        UnitStatusScript.m_Instance.m_IsMyTeam = false;
                        UnitStatusScript.m_Instance.SetUnitImage(hit.transform, 0, Bs.imgHpbar);
                    }
                        

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
            if (LivingRedUnit != null)
            {
                foreach (PlayerMove unit in LivingRedUnit)
                {
                    Debug.Log("빨간 유닛 :" + unit.name);
                }
            }

            if (LivingBlueUnit != null)
            {
                foreach (PlayerMove unit in LivingBlueUnit)
                {
                    Debug.Log("파란 유닛 :" + unit.name);
                }
            }

            for (int i = 0; i < LivingRedUnit.Count; ++i)
            {
                // 만약 유닛의 위치가 fog가 걷힌 위치에 있다면

                // 해당 유닛의 ShowInFog함수 호출
                // 아니면 HideInFog함수 호출

                if (LivingRedUnit[i].m_Hp <= 0f)
                {
                    LivingRedUnit[i].m_IsSelect = false;
                    LivingRedUnit[i].imgSelectbar.enabled = false;
                    LivingRedUnit[i].m_Animator.SetBool("IsDie", true);
                    LivingRedUnit[i].Invoke("Death", 3f);
                    LivingRedUnit[i].m_IsAlive = false;
                    LivingRedUnit.Remove(LivingRedUnit[i]);
                }
            }

            for (int i = 0; i < LivingBlueUnit.Count; ++i)
            {
                if (LivingBlueUnit[i].m_Hp <= 0f)
                {
                    LivingBlueUnit[i].m_IsSelect = false;
                    LivingBlueUnit[i].imgSelectbar.enabled = false;
                    LivingBlueUnit[i].m_Animator.SetBool("IsDie", true);
                    LivingBlueUnit[i].Invoke("Death", 3f);
                    LivingBlueUnit[i].m_IsAlive = false;
                    LivingBlueUnit.Remove(LivingBlueUnit[i]);
                }
            }

            //foreach (PlayerMove unit in LivingRedUnit)
            //{
            //    if (unit.m_Hp <= 0f)
            //    {
            //        //Vector3 Pos = unit.transform.position;
            //        //Pos.y -= 100f; 

            //        //unit.m_Animator.SetBool("IsDie", true);
            //        //unit.StopAllCoroutines();
            //        //unit.m_IsAlive = false;

            //        //yield return new WaitForSeconds(2.5f);
            //        //unit.transform.position = Pos;
            //        unit.m_IsSelect = false;
            //        unit.imgSelectbar.enabled = false;
            //        unit.m_Animator.SetBool("IsDie", true);
            //        unit.Invoke("Death", 3f);
            //        unit.m_IsAlive = false;
            //        LivingRedUnit.Remove(unit);
            //        //SelectedUnit.Remove(unit);
                    
                    
            //    }

            //    //if (unit.m_IsPM && unit.HitPM == null)    // 타겟 유닛이 죽었을때
            //    //    unit.m_IsPM = false;
            //    //if (unit.m_IsBS && unit.HitBS == null)    // 타겟 건물이 죽었을때
            //    //    unit.m_IsBS = false;

            //}

            //foreach (PlayerMove unit in LivingBlueUnit)
            //{
            //    if (unit.m_Hp <= 0f)
            //    {
            //        //Vector3 Pos = unit.transform.position;
            //        //Pos.y -= 100f; 

            //        //unit.m_Animator.SetBool("IsDie", true);
            //        //unit.StopAllCoroutines();
            //        //unit.m_IsAlive = false;

            //        //yield return new WaitForSeconds(2.5f);
            //        //unit.transform.position = Pos;
            //        unit.m_IsSelect = false;
            //        unit.imgSelectbar.enabled = false;
            //        unit.m_Animator.SetBool("IsDie", true);
            //        unit.Invoke("Death", 3f);
            //        unit.m_IsAlive = false;
            //        LivingBlueUnit.Remove(unit);
            //        //SelectedUnit.Remove(unit);
                    

            //    }

            //    //if (unit.m_IsPM && unit.HitPM == null)    // 타겟 유닛이 죽었을때
            //    //    unit.m_IsPM = false;
            //    //if (unit.m_IsBS && unit.HitBS == null)    // 타겟 건물이 죽었을때
            //    //    unit.m_IsBS = false;

            //}

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



        if(IsUnitMyTeam(Unit) == true)
        {
            UnitStatusScript.m_Instance.m_IsMyTeam = true;
            UnitStatusScript.m_Instance.SetUnitImage(unit, 0, Unit.imgHpbar);
        }
            
            
            
        else
        {
            UnitStatusScript.m_Instance.m_IsMyTeam = false;
            UnitStatusScript.m_Instance.SetUnitImage(unit, 0, Unit.imgHpbar);
            goto CannotOrder;
        }

        


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
        //Debug.Log("체크해제 호출됨");

        UnitFuncScript.m_Instance.ClearFunc();
        foreach (PlayerMove unit in LivingRedUnit)
        {
            unit.m_IsSelect = false;
            unit.imgSelectbar.enabled = false;
        }

        foreach (PlayerMove unit in LivingBlueUnit)
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
            {
                unit.HealArea.gameObject.SetActive(false);
                unit.HealEffect.SetActive(false);
            }
                
            unit.m_IsMineral = false;
            unit.m_IsPick = true;

        }
    }

    public void OrderToMineral()
    {
        foreach (PlayerMove unit in SelectedUnit)
        {
            //if (unit.gameObject.tag != "UnitLego" || unit.m_IsMineral || m_BuildOK.activeSelf)
            //    return;
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
            if (unit.tag == "UnitAirballoon")
            {
                Debug.Log("폭탄공격");
                GameObject Obj = Instantiate(unit.Bullet, unit.FireHole.position, unit.FireHole.rotation);
                unit.m_IsSelect = false;
                return;
            }
            else if (unit.tag != "UnitLego"
                && unit.tag != "UnitCupid" && unit.tag != "UnitClockMouse")
            {
                //StopCoroutine("SelectRoutine");
                unit.m_IsPick = false;
                unit.m_IsMineral = false;
                unit.HitPM = null;
                unit.HitBS = null;
                unit.m_Animator.SetBool("IsAttack", false);
                unit.m_IsAttack = true;
                unit.imgSelectbar.enabled = false;
                unit.imgHpbar.enabled = false;

                if (PhotonNetwork.isMasterClient)
                {
                    foreach (PlayerMove Unit in LivingBlueUnit)
                    {
                        Unit.m_AttackImage.SetActive(true);
                    }
                }

                else
                {
                    foreach (PlayerMove Unit in LivingRedUnit)
                    {
                        Unit.m_AttackImage.SetActive(true);
                    }
                }
                    
            }
        }

        //for(int i = 0; i < SelectedUnit.Count; ++i)
        //{
        //    if(SelectedUnit[i].tag != "UnitLego" 
        //        && SelectedUnit[i].tag != "UnitCupid" && SelectedUnit[i].tag != "UnitClockMouse")
        //    {
        //        StopCoroutine("SelectRoutine");
        //        SelectedUnit[i].m_IsPick = false;
        //        SelectedUnit[i].m_IsMineral = false;
        //        SelectedUnit[i].HitPM = null;
        //        SelectedUnit[i].HitBS = null;
        //        SelectedUnit[i].m_Animator.SetBool("IsAttack", false);
        //        SelectedUnit[i].m_IsAttack = true;
        //        SelectedUnit[i].imgSelectbar.enabled = false;
        //        SelectedUnit[i].imgHpbar.enabled = false;
        //    }
        //}



    }

    public void OrderToBoard()
    {
        foreach (PlayerMove unit in SelectedUnit)
        {
            if (unit.m_IsBoard)
                return;
            //StopCoroutine("SelectRoutine");
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


    public bool IsUnitMyTeam (PlayerMove Unit)//유닛 같은 팀인지 검사
    {
        if (PhotonNetwork.isMasterClient)  
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

    public bool IsBuildingMyTeam(BuildingStatus Unit)//같은 팀인지 검사
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
