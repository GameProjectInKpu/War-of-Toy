using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public enum UnitType { lego, soldier, bear, mouse, balloon };
    public enum UnitState { idle, walk, mineral, build, trace, attack, die };
    public UnitState unitState = UnitState.idle;

    private Vector3 m_Pos;
    private float m_MoveSpeed;
    public float m_Hp = 100f;
    public float m_InitHp;

    public Transform m_Team;
    public Image imgHpbar;
    public Image imgSelectbar;
    public Image Orderbar;

    public LayerMask m_LMUnit;
    public LayerMask m_LMGround;
    public NavMeshPath m_Path;
    //public GameObject m_Canvas;
    public Animator m_Animator;

    //public GameObject UnitButton;
    //GameObject SelectButton;
    public bool m_IsSelect;
    public bool m_IsAlive;

    //public bool m_NewOrder;
    //public bool m_IsStartToMove;
    public bool m_Attackstop;

    public bool m_IsBuild;
    public bool m_IsPick;
    public bool m_IsAttack;
    public bool m_IsMineral;
    public bool m_IsBoard;

    public GameObject Building;
    public GameObject Bullet;
    public Transform FireHole;
    public Transform BalloonHeight;
    public bool IsStartParticle;

    public float m_Count;
    public float m_InitCount;

    //public Text m_CurActionText;

    GameObject HitOb;
    PlayerMove HitPM;

    void OnEnable()
    {
        m_IsAlive = true;
        m_IsSelect = false;
        // m_SerialNum = (int)Random.Range(0, 500);
        //SelectButton = Instantiate(UnitButton, transform.position, Quaternion.Euler(Vector2.zero));
        //UnitButton = SelectButton;
        //UnitButton.transform.SetParent(m_Canvas.transform, false);
        //UnitButton.transform.SetSiblingIndex(1);
        imgHpbar.enabled = false;
        imgSelectbar.enabled = false;
        m_Team = transform.Find("MiniMap");
    }

    void Awake()
    {
        m_Path = new NavMeshPath();
        //m_CurActionText = m_CurActionText.GetComponent<Text>();
        m_Animator = GetComponentInChildren<Animator>();
        m_MoveSpeed = 6f;
        m_Hp = 100f;
        m_InitHp = m_Hp;
        m_Count = 0f;
        m_InitCount = 100f;
        //StartCoroutine("UnitStatusRoutine");
    }

    /*IEnumerator UnitStatusRoutine()
    {
        while(m_IsAlive)
        {
            switch (unitState)
            {
                case UnitState.idle:
                    Debug.Log("유닛 아이들 상태");
                    m_Animator.SetBool("IsPick", false);
                    m_Animator.SetBool("IsAttack", false);
                    m_Animator.SetBool("IsMineral", false);
                    m_Animator.SetBool("IsBuild", false);
                    break;

                case UnitState.walk:
                    Debug.Log("유닛 걷기 상태");
                    m_Animator.SetBool("IsPick", true);
                    m_Animator.SetBool("IsAttack", false);
                    m_Animator.SetBool("IsMineral", false);
                    m_Animator.SetBool("IsBuild", false);
                    break;

                case UnitState.attack:
                    m_Animator.SetBool("IsAttack", true);
                    break;

                case UnitState.trace:

                    break;
                case UnitState.mineral:
                    m_Animator.SetBool("IsMineral", false);
                    m_Animator.SetBool("IsMineral", true);
                    m_Animator.SetBool("IsPick", false);
                    m_Animator.SetBool("IsBuild", false);
                    break;

                case UnitState.build:
                    m_Animator.SetBool("IsPick", false);
                    m_Animator.SetBool("IsBuild", true);
                    break;

                case UnitState.die:
                    m_Animator.SetBool("IsDie", true);
                    break;

                default:
                    break;
            }
            yield return null;
        }
    }*/


    IEnumerator OrderRoutine()
    {
        while (true)
        {
            if (BuildScript.m_IsBuild && m_IsSelect == true && transform.tag == "UnitLego")
            {
                m_Animator.SetBool("IsMineral", false);
                yield return StartCoroutine("BuildRoutine");
                StopCoroutine("OrderRoutine");
            }

            else if (m_IsPick)
            {
                if (Input.GetMouseButton(0) && m_IsSelect == true && TouchScript.m_Instance.IsOver)
                //if (Input.touchCount == 1 && m_IsSelect == true)
                {
                    //m_IsStartToMove = true;
                    imgSelectbar.enabled = false;
                    imgHpbar.enabled = false;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //(Input.GetTouch(0).position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        Debug.DrawRay(Camera.main.transform.position, hit.point - Camera.main.transform.position, Color.red);
                        HitOb = hit.collider.gameObject;
                        if (HitOb.layer == 30)   // Ground
                        {
                            unitState = UnitState.walk;
                            m_Animator.SetBool("IsAttack", false);
                            m_Animator.SetBool("IsMineral", false);
                            m_Animator.SetBool("IsBuild", false);
                            StopCoroutine("AttackByBullet");
                            StopCoroutine("TraceRoutine");
                            yield return StartCoroutine("Picking", hit.point);
                            m_IsSelect = false;
                         }
                    }
                }
            }


            else if (m_IsMineral)
            {
                if (Input.GetMouseButton(0) && m_IsSelect == true)
                //if (Input.touchCount == 1 && m_IsSelect == true)
                {
                    //m_IsStartToMove = true;
                    imgSelectbar.enabled = false;
                    imgHpbar.enabled = false;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //(Input.GetTouch(0).position); //
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        Debug.DrawRay(Camera.main.transform.position, hit.point - Camera.main.transform.position, Color.red);
                        HitOb = hit.collider.gameObject;
                        ResourceStatus HitRS = HitOb.GetComponent<ResourceStatus>();
                        if (HitOb.layer == 26)   // Resource
                        {
                            if (transform.tag != "UnitLego")
                                StopCoroutine("OrderRoutine");
                            m_Animator.SetBool("IsMineral", false);

                            yield return StartCoroutine("Picking", hit.point);
                            transform.rotation = Quaternion.LookRotation(hit.transform.position - transform.position);
                            Debug.Log("Resource");
                            unitState = UnitState.mineral;
                            m_Animator.SetBool("IsMineral", true);
                            while (true)
                            {
                                Debug.Log(HitOb.transform.tag);
                                yield return new WaitForSeconds(2.0f);
                                if (HitOb.tag == "B_Stars")
                                {
                                   if (HitRS.m_Empty)
                                    {
                                        Destroy(HitOb);
                                        break;
                                    }
                                       
                                   ++StarScript.m_Instance.m_StarNum;
                                   ++HitRS.m_gage;
                                }
                                    
                                else
                                   ++BatteryScript.m_Instance.m_BatteryNum;
                                    
                            }
                            unitState = UnitState.idle;
                            m_Animator.SetBool("IsMineral", false);
                            StopCoroutine("OrderRoutine");
                        }
                    }
                }
            }

            else if (m_IsBoard)
            {
                if (Input.GetMouseButton(0) && m_IsSelect == true && TouchScript.m_Instance.IsOver)
                //if (Input.touchCount == 1 && m_IsSelect == true)
                {
                    //m_IsStartToMove = true;
                    SelectUnitScript.m_Instance.StartCoroutine("SelectRoutine");
                    imgSelectbar.enabled = false;
                    imgHpbar.enabled = false;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //(Input.GetTouch(0).position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        Debug.DrawRay(Camera.main.transform.position, hit.point - Camera.main.transform.position, Color.red);
                        HitOb = hit.collider.gameObject;
                        HitPM = HitOb.GetComponent<PlayerMove>();
                        if (HitOb.tag == "UnitAirballoon")   // AirUnit
                        {
                            unitState = UnitState.walk;
                            m_Animator.SetBool("IsAttack", false);
                            m_Animator.SetBool("IsMineral", false);
                            m_Animator.SetBool("IsBuild", false);
                            StopCoroutine("AttackByBullet");
                            StopCoroutine("TraceRoutine");
                            yield return StartCoroutine("Picking", HitOb.transform.position);
                            m_IsSelect = false;
                            //yield return HitPM.StartCoroutine("LandingAndTakeOff", false);
                            GetComponent<NavMeshAgent>().enabled = false;
                            transform.SetParent(HitPM.BalloonHeight, false);
                            Vector3 UPos = Vector3.zero;
                            Vector3 URot = Vector3.zero; 
                            Vector3 UScal = Vector3.one;
                            UPos.x -= 5f;
                            URot.z -= 90f;
                            UScal *= 0.6f;
                            transform.localPosition = UPos;
                            transform.localRotation = Quaternion.Euler(URot);
                            transform.localScale = UScal;
                            
                            //yield return HitPM.StartCoroutine("LandingAndTakeOff", true);

                        }
                    }
                }
            }

            else if (m_IsAttack)
            {                
                if (Input.GetMouseButton(0) && m_IsSelect == true)
                //if (Input.touchCount == 1 && m_IsSelect == true)
                {
                    //m_IsStartToMove = true;
                    SelectUnitScript.m_Instance.StartCoroutine("SelectRoutine");
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //(Input.GetTouch(0).position); 
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        Debug.DrawRay(Camera.main.transform.position, hit.point - Camera.main.transform.position, Color.red);
                        HitOb = hit.collider.gameObject;
                        HitPM = HitOb.GetComponent<PlayerMove>();
                       
                        if (HitOb.layer == 28)   // Unit
                        {
                            HitPM.m_IsSelect = false;
                            HitPM.imgSelectbar.enabled = false;
                            if (transform.tag == "UnitLego" || transform.tag == "UnitClockMouse" || HitOb.tag == "UnitAirballoon")
                            {
                                m_IsSelect = false;
                                break;
                            }

                            Debug.Log("타겟 확정");
                            unitState = UnitState.attack;
                            yield return StartCoroutine("Picking", hit.point);
                                
                            switch (transform.tag)
                            {
                                case "UnitSoldier":
                                    {
                                        m_Animator.SetBool("IsAttack", true);
                                        StartCoroutine("AttackByBullet");
                                        imgHpbar.enabled = true;
                                        HitPM.imgHpbar.enabled = true;

                                        yield return StartCoroutine("TraceRoutine");
                                        imgHpbar.enabled = false;
                                        HitPM.imgHpbar.enabled = false;
                                        HitPM.unitState = UnitState.die;
                                    HitPM.m_Animator.SetBool("IsDie", true);
                                    HitPM.m_IsAlive = false;
                                    m_Animator.SetBool("IsAttack", false);
                                    unitState = UnitState.idle;
                                    m_Animator.SetBool("IsPick", false);

                                    StopCoroutine("AttackByBullet");
                                        break;
                                    }
                                case "UnitBear":
                                    {
                                        m_Animator.SetBool("IsAttack", true);
                                        StartCoroutine("BearAttackRoutine");

                                        imgHpbar.enabled = true;
                                        HitPM.imgHpbar.enabled = true;

                                        yield return StartCoroutine("TraceRoutine");
                                        imgHpbar.enabled = false;
                                        HitPM.imgHpbar.enabled = false;
                                        HitPM.m_Animator.SetBool("IsDie", true);
                                        m_Animator.SetBool("IsAttack", false);
                                        StopCoroutine("BearAttackRoutine");
                                        break;
                                    }
                                case "UnitDinosaur":
                                    {
                                        m_Animator.SetBool("IsAttack", true);
                                        imgHpbar.enabled = true;
                                        HitPM.imgHpbar.enabled = true;
                                        StartCoroutine("AttackByFlare");
                                        yield return StartCoroutine("TraceRoutine");
                                        imgHpbar.enabled = false;
                                        HitPM.imgHpbar.enabled = false;
                                        HitPM.unitState = UnitState.die;
                                        HitPM.m_Animator.SetBool("IsDie", true);
                                        HitPM.m_IsAlive = false;
                                        m_Animator.SetBool("IsAttack", false);
                                        unitState = UnitState.idle;
                                        m_Animator.SetBool("IsPick", false);

                                        StopCoroutine("AttackByFlare");
                                        break;
                                    }
                                case "UnitRCcar":
                                    {
                                        m_Animator.SetBool("IsAttack", true);
                                        imgHpbar.enabled = true;
                                        HitPM.imgHpbar.enabled = true;
                                        //StartCoroutine("AttackByFlare");
                                        yield return StartCoroutine("TraceRoutine");
                                        imgHpbar.enabled = false;
                                        HitPM.imgHpbar.enabled = false;
                                        HitPM.unitState = UnitState.die;
                                        HitPM.m_Animator.SetBool("IsDie", true);
                                        HitPM.m_IsAlive = false;
                                        m_Animator.SetBool("IsAttack", false);
                                        unitState = UnitState.idle;
                                        m_Animator.SetBool("IsPick", false);

                                        //StopCoroutine("AttackByFlare");
                                        break;
                                    }

                                default:
                                    break;
                            }
                        }
                    }
                }

            }
            //StopCoroutine("OrderRoutine");
            yield return null;
        }
    }


   

    IEnumerator TraceRoutine()
    {
        while (HitPM.m_IsAlive)
        {
            if (Vector3.Distance(transform.position, HitPM.transform.position) >= 10f)
            {
                float tracedis = 0.0f;
                switch (transform.tag)
                {
                    case "UnitSoldier": tracedis = 5f; break;
                    case "UnitDinosaur": tracedis = 8f; break;
                    case "UnitBear": tracedis = 2f; break;
                    case "UnitRCcar": tracedis = 4f; break;
                    default: tracedis = 2f; break;

                }
                m_Animator.SetBool("IsPick", true);
                m_Animator.SetBool("IsAttack", false);
                m_Attackstop = true;
                IsStartParticle = false;
                NavMesh.CalculatePath(transform.position, HitPM.transform.position, NavMesh.AllAreas, m_Path);
                Vector3[] TraceCorners = m_Path.corners;
                int TraceIndex = 1;
                while (TraceIndex < TraceCorners.Length)
                {
                    m_Pos = (TraceCorners[TraceIndex] - transform.position).normalized;
                    transform.position += m_Pos * m_MoveSpeed * Time.deltaTime;
                    transform.rotation = Quaternion.LookRotation(m_Pos);
                    if (Vector3.Distance(transform.position, TraceCorners[TraceIndex]) < tracedis)
                        TraceIndex++;
                    yield return null;
                }

            }

            else
            {
                m_Animator.SetBool("IsPick", false);
                m_Animator.SetBool("IsAttack", true);
                m_Attackstop = false;             
            }


            transform.rotation = Quaternion.LookRotation(HitPM.transform.position - transform.position);
            yield return null;
        }
    }


    IEnumerator AttackByBullet()
    {
        while (true)
        {
            if (m_IsAttack == false)
            {
                unitState = UnitState.idle;
                StopCoroutine("AttackByBullet");
            }
                

            if (m_Attackstop == false )
            {
                GameObject Obj = (GameObject)PhotonNetwork.Instantiate(Bullet.name, FireHole.position, FireHole.rotation, 0);
            }

            
            yield return new WaitForSeconds(2.5f);
        }

    }

    IEnumerator AttackByFlare()
    {
        while (true)
        {

            if (m_IsAttack == false)
            {
                unitState = UnitState.idle;     
                StopCoroutine("AttackByFlare");
            }

            if (m_Attackstop == false)
            {
                if (IsStartParticle == true)
                {
                    GameObject Obj = (GameObject)Instantiate(Bullet, FireHole.position, FireHole.rotation);
                    Destroy(Obj, 0.5f);
                }
                else
                    IsStartParticle = true;
                    
            }
            
            yield return new WaitForSeconds(1.7f);

        }

    }

    IEnumerator BearAttackRoutine()
    {
        while (true)
        {
            if (HitOb.layer == 28 && m_Attackstop == false)
            {
                HitPM.m_Hp -= 10f;
                if (HitPM.m_Hp < 0f)
                    HitPM.m_IsAlive = false;
                HitPM.imgHpbar.enabled = true;
                HitPM.imgHpbar.fillAmount = (float)HitPM.m_Hp / (float)HitPM.m_InitHp;

            }
            yield return new WaitForSeconds(2.5f);
        }
    }

    IEnumerator Picking(Vector3 HitPoint)
    {
        UnitFuncScript.m_Instance.ClearFunc();
        float dis = 0.0f;
        if (unitState == UnitState.attack)
        {
            switch (transform.tag)
            {
                case "UnitSoldier": dis = 5f; break;
                case "UnitBear": dis = 2f; break;
                case "UnitDinosaur": dis = 9f; break;
                case "UnitRCcar": dis = 2f; break;
                default: dis = 1f; break;

            }
        }

        else dis = 1f;
        Debug.Log("피킹중");
        unitState = UnitState.walk;
        m_Animator.SetBool("IsPick", true);
        NavMesh.CalculatePath(transform.position, HitPoint, NavMesh.AllAreas, m_Path);
        Vector3[] Corners = m_Path.corners;
        int Index = 1;
        while (Index < Corners.Length)
        {
            Debug.DrawRay(Camera.main.transform.position, HitPoint - Camera.main.transform.position, Color.red);
            m_Pos = (Corners[Index] - transform.position).normalized;
            transform.position += m_Pos * m_MoveSpeed * Time.deltaTime;
         
            if (transform.tag != "UnitAirballoon")
                transform.rotation = Quaternion.LookRotation(m_Pos);
            if (Vector3.Distance(transform.position, Corners[Index]) < dis)
                Index++;
            yield return null;
        }
        m_Animator.SetBool("IsPick", false);
        GetComponent<NavMeshAgent>().enabled = true;
        unitState = UnitState.idle;
        StopCoroutine("Picking");
    }

    IEnumerator LandingAndTakeOff(bool IsLanding)
    {
        
        if (IsLanding)
        {
            Vector3 BallPos = BalloonHeight.position;
            if (BallPos.y <= 11f)
            {
                BallPos.y += 0.1f;
                BalloonHeight.position = BallPos;
            }
            else
                StopCoroutine("LandingAndTakeOff");
        }

        else
        {
            Vector3 BallPos = BalloonHeight.position;
            if (BallPos.y > 0f)
            {
                BallPos.y -= 0.1f;
                BalloonHeight.position = BallPos;
            }
            else
                StopCoroutine("LandingAndTakeOff");
        }

        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MeshLink")
        {
            GetComponent<NavMeshAgent>().enabled = false;
        }
    }

    private void OnCollisionEnter(Collision damage)
    {
        if (damage.transform.tag == "Bullet")
        {
            m_Hp -= 10f;
            if (m_Hp < 0f)
                m_IsAlive = false;
            imgHpbar.enabled = true;
            imgHpbar.fillAmount = (float)m_Hp / (float)m_InitHp;
        }

        if (damage.transform.tag == "AttackArea")
        {
            Debug.Log("카에 맞음");
            m_Hp -= 20f;
            if (m_Hp < 0f)
                m_IsAlive = false;
            imgHpbar.enabled = true;
            imgHpbar.fillAmount = (float)m_Hp / (float)m_InitHp;
        }
    }


    IEnumerator BuildRoutine()
    {
        UnitFuncScript.m_Instance.ClearFunc();
        Debug.Log("건물지으러 go");
        m_IsBuild = true;
        imgSelectbar.enabled = false;
        Building = BuildScript.BuildingTemp;
        Building.GetComponentInChildren<Renderer>().material.color = Color.red;
        unitState = UnitState.walk;
        m_Animator.SetBool("IsPick", true);
        BuildScript.m_IsBuild = false;
        float dis = 0.0f;

        switch (Building.tag)
        {
            case "B_ToyFactory": dis = 6f; break;
            default: dis = 4f; break;

        }
        NavMesh.CalculatePath(transform.position, BuildScript.BuildPos, NavMesh.AllAreas, m_Path);
        Vector3[] Corners = m_Path.corners;
        int Index = 1;
        while (Index < Corners.Length)
        {
            Debug.DrawRay(Camera.main.transform.position, BuildScript.BuildPos - Camera.main.transform.position, Color.red);
            m_Pos = (Corners[Index] - transform.position).normalized;
            transform.position += m_Pos * m_MoveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(m_Pos);
            if (Vector3.Distance(transform.position, Corners[Index]) < dis)
                Index++;
            yield return null;
        }

        //Building.GetComponent<NavMeshObstacle>().enabled = true;       
        transform.rotation = Quaternion.LookRotation(BuildScript.BuildPos - transform.position);
        BuildScript.m_IsBuild = false;
        unitState = UnitState.build;
        m_Animator.SetBool("IsPick", false);
        m_Animator.SetBool("IsBuild", true);

        yield return new WaitForSeconds(5.0f);
        Building.GetComponentInChildren<Renderer>().material.color = Color.white;
        unitState = UnitState.idle;
         m_Animator.SetBool("IsBuild", false);
        m_IsSelect = false;
        m_IsBuild = false;

        switch (Building.tag)
        {
            case "B_Zenga":
                BuildScript.AttackArea.gameObject.SetActive(true);
                break;
            case "B_ToyFactory":
                FactoryScript FactoryFunc = Building.GetComponent<FactoryScript>();
                FactoryFunc.enabled = true;
                break;

            default:
                break;

        }

        StopCoroutine("BuildRoutine");
    }



    public void SelectbyButton()
    {
        m_IsSelect = true;

    }


    public void Death()
    {
        --CurUnitNum.m_Instance.m_UnitNum;
        Destroy(gameObject);
        //Destroy(SelectButton);
    }


    private void OnDestroy()
    {
        StopAllCoroutines();
        m_IsAlive = false;
    }
}
