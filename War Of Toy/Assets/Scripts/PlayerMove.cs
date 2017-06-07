using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour {

    public enum UnitType { lego, soldier, bear, mouse };
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
    public GameObject m_Canvas;
    public Animator m_Animator;

    public GameObject UnitButton;
    GameObject SelectButton;
    public bool m_IsSelect;
    public bool m_IsAlive;
    public bool m_NewOrder;
    public bool m_Attackstop;
    public bool m_IsBuild;

    public GameObject Building;
    public GameObject Bullet;
    public Transform FireHole;

    public float m_Count;
    public float m_InitCount;

    public Text m_CurActionText;

    GameObject HitOb;
    PlayerMove HitPM;

    void OnEnable()
    {
        m_IsAlive = true;
        //SelectButton = Instantiate(UnitButton, transform.position, Quaternion.Euler(Vector2.zero));
        UnitButton = SelectButton;
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
        m_MoveSpeed = 4f;
        m_Hp = 100f;
        m_InitHp = m_Hp;
        m_Count = 0f;
        m_InitCount = 100f;
    }


    IEnumerator OrderRoutine()
    {
        while (true)
        {
            if (BuildScript.m_IsBuild && m_IsSelect == true && transform.tag == "UnitLego")
            {
                yield return StartCoroutine("BuildRoutine");
                StopCoroutine("OrderRoutine");
            }

            //if (Input.touchCount == 1 && m_IsSelect == true)
            if (Input.GetMouseButton(1) && m_IsSelect == true && BuildScript.Building == null)
            {
                if (m_Count < m_InitCount)
                {
                    m_Count += 2f;
                }
                
                if (m_Count == m_InitCount && m_IsSelect == true)
                {
                    imgSelectbar.enabled = false;
                    imgHpbar.enabled = false;
                    Debug.Log("명령 받음");

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //(Input.GetTouch(0).position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        Debug.DrawRay(Camera.main.transform.position, hit.point - Camera.main.transform.position, Color.red);
                        HitOb = hit.collider.gameObject;
                        HitPM = HitOb.GetComponent<PlayerMove>();
                        
                        switch (HitOb.layer)
                        {
                            case 30:    // 피킹
                                {    
                                    Debug.Log("Picked");
                                    m_Animator.SetBool("IsAttack", false);
                                    m_Animator.SetBool("IsMineral", false);
                                    m_Animator.SetBool("IsBuild", false);
                                    StopCoroutine("AttackByBullet");
                                    yield return StartCoroutine("Picking", hit.point);
                                    m_IsSelect = false;
                                    break;
                                }

                            case 26:    // 자원 획득
                                {                                   
                                    if (transform.tag != "UnitLego")
                                        StopCoroutine("OrderRoutine");
                                    m_Animator.SetBool("IsMineral", false);

                                    yield return StartCoroutine("Picking", hit.point);
                                    transform.rotation = Quaternion.LookRotation(hit.transform.position - transform.position);
                                    Debug.Log("Resource");
                                    m_Animator.SetBool("IsMineral", true);
                                    while (m_NewOrder == false)
                                    {    
                                        yield return new WaitForSeconds(2.0f);
                                        if (hit.transform.tag == "B_Batterys")
                                            ++BatteryScript.m_Instance.m_BatteryNum;
                                        else
                                            ++StarScript.m_Instance.m_StarNum;
                                    }
                                    m_Animator.SetBool("IsMineral", false);
                                    break;
                                }

                            case 28:    // 다른 유닛 공격or 힐링
                                {
                                    Debug.DrawRay(Camera.main.transform.position, hit.point - Camera.main.transform.position, Color.red);
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
                                                HitPM.m_Animator.SetBool("IsDie", true);
                                                m_Animator.SetBool("IsAttack", false);
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
                                        default:
                                            break;
                                    }
                                    break;
                                }
                            default:
                                break;
                        }
                        StopCoroutine("OrderRoutine");
                    }
                }
                
            }

            else
            {
                if (m_Count  > 0)
                {
                    m_Count -= 2f;
                }

            }

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
                    case "UnitBear": tracedis = 2f; break;
                    default: tracedis = 2f; break;

                }
                m_Animator.SetBool("IsPick", true);
                m_Animator.SetBool("IsAttack", false);
                m_Attackstop = true;
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
        while(true)
        {
            if(m_Attackstop == false)
            {
                GameObject Obj = (GameObject)PhotonNetwork.Instantiate(Bullet.name, FireHole.position, FireHole.rotation, 0);
            }

            yield return new WaitForSeconds(2.5f);
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
                default: dis = 1f; break;

            }
        }

        else dis = 1f;
        Debug.Log("피킹중");
        m_Animator.SetBool("IsPick", true);
        NavMesh.CalculatePath(transform.position, HitPoint, NavMesh.AllAreas, m_Path);
        Vector3[] Corners = m_Path.corners;
        int Index = 1;
        while (Index < Corners.Length)
        {
            Debug.DrawRay(Camera.main.transform.position, HitPoint - Camera.main.transform.position, Color.red);
            m_Pos = (Corners[Index] - transform.position).normalized;
            transform.position += m_Pos * m_MoveSpeed * Time.deltaTime;
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "MeshLink")
        {
            GetComponent<NavMeshAgent>().enabled = false;
        }
    }

    private void OnCollisionEnter(Collision bullet)
    {
        if(bullet.transform.tag == "Bullet")
        {
            m_Hp -= 10f;
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

        Building.GetComponent<NavMeshObstacle>().enabled = true;       
        transform.rotation = Quaternion.LookRotation(BuildScript.BuildPos - transform.position);
        BuildScript.m_IsBuild = false;
        m_Animator.SetBool("IsPick", false);
        m_Animator.SetBool("IsBuild", true);

        yield return new WaitForSeconds(5.0f);
        Building.GetComponentInChildren<Renderer>().material.color = Color.white;
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
        Destroy(gameObject);
        Destroy(SelectButton);
    }


    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
