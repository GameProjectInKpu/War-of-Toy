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

    private Vector3 m_Dir;
    private float m_MoveSpeed;
    public float m_Hp = 100f;
    public float m_InitHp;
    public float m_Power;   // 내가 때리는 힘
    public float m_Damage;  // 내가 쳐맞는 힘
    public float m_Heal;
    public int m_Mineral;

    public GameObject m_AttackImage;
    public Transform m_Team;
    public Image imgHpbar;
    public Image imgSelectbar;
    //public Image Orderbar;

    public LayerMask m_LMUnit;
    public LayerMask m_LMGround;
    //public LayerMask m_LMFog;
    public NavMeshPath m_Path;
    //public GameObject m_Canvas;
    public Animator m_Animator;

    //public GameObject UnitButton;
    //GameObject SelectButton;
    public bool m_IsSelect;
    public bool m_IsAlive;
    //public static GameObject bullet;

    //public bool m_IsStartToMove;
    public bool m_Attackstop;

    public bool m_IsBuild;
    public bool m_IsPick;
    public bool m_IsAttack;
    public bool m_IsMineral;
    public bool m_IsHeal;
    public bool m_IsBoard;
    public bool m_IsFull;   // 열기구에 누군가 탔는지
    public bool m_IsPM; // 공격대상이 유닛인지
    public bool m_IsBS; // 공격대상이 건물인지
    public bool m_IsUpgraded;
    public bool m_IsInHealArea;
    public bool m_IsOffensive;

    public GameObject HealEffect;
    public GameObject Building;
    public GameObject Bullet;
    //public BulletRigidbody Brb;
    public GameObject UpgParticle;
    public Transform FireHole;
    public Transform BalloonHeight;
    public Transform CarAttakArea;
    public Transform HealArea;

    public bool m_IsStartDamage;

    public float m_Count;
    public float m_InitCount;

    public NavMeshAgent m_Nav;

    GameObject HitOb;
    //GameObject AttackOb;

    public PlayerMove HitPM;
    //public PlayerMove AttackPM;
    public BuildingStatus HitBS;
    ResourceStatus HitRS;

    GameObject Obj;

    public void HideInFog()
    {
        this.gameObject.layer = 9;  // HideUnit
    }

    public void ShowInFog()
    {
        this.gameObject.layer = 28; // Unit
    }


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
        if (transform.tag != "UnitCupid")
            StartCoroutine("InTheArea");
    }

    void Awake()
    {
        m_Path = new NavMeshPath();
        //if(transform.tag != "UnitAirBalloon")
        //{
        //    m_AttackImage.SetActive(false);
            
        //}
        m_Animator = GetComponentInChildren<Animator>();
        m_Nav = GetComponent<NavMeshAgent>();
        m_Nav.enabled = true;
        m_MoveSpeed = 6f;
        m_Hp = 100f;
        m_InitHp = m_Hp;
        m_Count = 0f;
        m_InitCount = 100f;
        m_Power = 10f;
        m_Heal = 10f;
        m_Mineral = 1;

        //if(Bullet != null)
        //{
        //    Bullet.GetComponent<BulletRigidbody>().m_Owner = GetComponent<PlayerMove>();
        //    //BulletRigidbody Brb = Bullet.GetComponent<BulletRigidbody>();
        //    //Brb .GetComponent<BulletRigidbody>();
        //    //Brb.m_Owner = GetComponent<PlayerMove>(); 
        //    //Brb.m_Power = m_Power;
        //}

    }

    //private void Update()
    //{
    //    RaycastHit hit;
    //    Debug.Log(m_Team.position);
    //    if (Physics.Raycast(m_Team.position, Vector3.up, Mathf.Infinity, m_LMFog))//if (Physics.Raycast(m_Team.position, Vector3.up, out hit, Mathf.Infinity))
    //    {
    //        Debug.DrawRay(m_Team.position, Vector3.up, Color.red);
    //       // Debug.Log(hit.transform.tag);
    //        //Debug.Log(hit.transform.gameObject.GetComponent<Renderer>().material.color.a);

    //    }

      


    //}

    private void TargetPointFalse()
    {
        if (PhotonNetwork.isMasterClient)
        {
            for (int i = 0; i < SelectUnitScript.m_Instance.LivingBlueUnit.Count; ++i)
            {
                if (SelectUnitScript.m_Instance.LivingBlueUnit[i].m_AttackImage.activeSelf == true)
                {
                    SelectUnitScript.m_Instance.LivingBlueUnit[i].m_AttackImage.SetActive(false);
                }
            }
        }

        else
        {
            for (int i = 0; i < SelectUnitScript.m_Instance.LivingRedUnit.Count; ++i)
            {
                if (SelectUnitScript.m_Instance.LivingRedUnit[i].m_AttackImage.activeSelf == true)
                {
                    SelectUnitScript.m_Instance.LivingRedUnit[i].m_AttackImage.SetActive(false);
                }
            }
        }
        
    }



    public bool IsInputRight()
    {
        //return (Input.GetMouseButton(0) && m_IsSelect);
        return ((Input.touchCount == 1) && m_IsSelect);
    }

    public Vector3 InputSpot()
    {
        //return (Input.mousePosition);
        return (Input.GetTouch(0).position);
    }

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
                if (IsInputRight() && TouchScript.m_Instance.IsOver)
                {
                    //m_IsStartToMove = true;
                   // SelectUnitScript.m_Instance.StopCoroutine("SelectRoutine");
                   // SelectUnitScript.m_Instance.StartCoroutine("SelectRoutine");
                    imgSelectbar.enabled = false;
                    imgHpbar.enabled = false;
                    SelectUnitScript.m_Instance.SelectedUnit.Remove(transform.GetComponent<PlayerMove>());
                    Ray ray = Camera.main.ScreenPointToRay(InputSpot());
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        Debug.DrawRay(Camera.main.transform.position, hit.point - Camera.main.transform.position, Color.red);
                        HitOb = hit.collider.gameObject;
                        if (HitOb.layer == 30)   // Ground
                        {
                            SelectUnitScript.m_Instance.m_PickImage.transform.position = hit.point;
                            SelectUnitScript.m_Instance.m_PickImage.SetActive(true);
                            unitState = UnitState.walk;
                            m_Animator.SetBool("IsAttack", false);
                            m_Animator.SetBool("IsMineral", false);
                            m_Animator.SetBool("IsHeal", false);
                            m_Animator.SetBool("IsBuild", false);
                            StopCoroutine("AttackByBullet");
                            StopCoroutine("TraceRoutine");
                            StopCoroutine("MineralRoutine");
                            StopCoroutine("HealRoutine");
                            CancelInvoke("AttackByFlare");
                            yield return StartCoroutine("Picking", hit.point);
                            SelectUnitScript.m_Instance.m_PickImage.SetActive(false);

                            m_IsSelect = false;
                        }
                    }
                }
            }

            else if (m_IsMineral)
            {
                if (IsInputRight())
                {
                    //m_IsStartToMove = true;
                   // SelectUnitScript.m_Instance.StopCoroutine("SelectRoutine");
                    //SelectUnitScript.m_Instance.StartCoroutine("SelectRoutine");
                    imgSelectbar.enabled = false;
                    imgHpbar.enabled = false;
                    SelectUnitScript.m_Instance.SelectedUnit.Remove(transform.GetComponent<PlayerMove>());
                    Ray ray = Camera.main.ScreenPointToRay(InputSpot());
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        Debug.DrawRay(Camera.main.transform.position, hit.point - Camera.main.transform.position, Color.red);
                        HitOb = hit.collider.gameObject;
                        HitRS = HitOb.GetComponent<ResourceStatus>();
                        if (HitOb.layer == 26)   // Resource
                        {
                            if (transform.tag != "UnitLego")
                                StopCoroutine("OrderRoutine");
                            m_Animator.SetBool("IsMineral", false);

                            yield return StartCoroutine("Picking", hit.point);
                            transform.rotation = Quaternion.LookRotation(hit.transform.position - transform.position);
                            unitState = UnitState.mineral;
                            yield return StartCoroutine("MineralRoutine");
                        }
                    }
                }
            }

            else if (m_IsBoard)
            {
                if (IsInputRight() && TouchScript.m_Instance.IsOver)
                {
                    //m_IsStartToMove = true;
                    SelectUnitScript.m_Instance.StartCoroutine("SelectRoutine");
                    imgSelectbar.enabled = false;
                    imgHpbar.enabled = false;
                    SelectUnitScript.m_Instance.SelectedUnit.Remove(transform.GetComponent<PlayerMove>());
                    Ray ray = Camera.main.ScreenPointToRay(InputSpot());
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
                            //yield return HitPM.StartCoroutine("Landing_TakeOff", false);
                            m_Nav.enabled = false;
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
                            //HitPM.m_IsFull = true;
                            UnitFuncScript.m_Instance.IsAirUnitfull = true;
                            //yield return HitPM.StartCoroutine("Landing_TakeOff", true);

                        }
                    }
                }
            }

            else if (m_IsAttack)
            {
                if (IsInputRight() || m_IsOffensive)
                {
                    TargetPointFalse();                 

                    //m_IsStartToMove = true;
                    //SelectUnitScript.m_Instance.StartCoroutine("SelectRoutine");
                    //if (transform.tag == "UnitLego" || transform.tag == "UnitClockMouse")
                    //{
                    //    m_IsSelect = false;
                    //    break;
                    //}

                    Ray ray = Camera.main.ScreenPointToRay(InputSpot());
                    RaycastHit hit;
                   
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity) || m_IsOffensive)
                    {
                        
                        Debug.DrawRay(Camera.main.transform.position, hit.point - Camera.main.transform.position, Color.red);
                        //SelectUnitScript.m_Instance.SelectedUnit.Remove(transform.GetComponent<PlayerMove>());
                        HitOb = hit.collider.gameObject;
                        UnitFuncScript.m_Instance.ClearFunc();
                        if ((HitOb.layer == 28 
                            && !SelectUnitScript.m_Instance.IsUnitMyTeam(HitOb.GetComponent<PlayerMove>())) || m_IsOffensive)   // Unit
                        {
                            m_IsPM = true;
                            m_IsBS = false;
                            if (m_IsOffensive)
                            {
                                hit.point = HitPM.gameObject.transform.position;
                                goto offensive;
                            }
                            HitPM = HitOb.GetComponent<PlayerMove>();
                            offensive:;
                            HitPM.m_IsSelect = false;
                            HitPM.imgSelectbar.enabled = false;
                            HitPM.m_Damage = m_Power;
                            SelectUnitScript.m_Instance.SelectedUnit.Remove(HitPM);
                            SelectUnitScript.m_Instance.SelectedUnit.Remove(transform.GetComponent<PlayerMove>());

                            unitState = UnitState.attack;
                            yield return StartCoroutine("Picking", hit.point);

                            switch (transform.tag)
                            {
                                case "UnitSoldier":
                                    {
                                        InvokeRepeating("AttackByBullet", 0f, 1.0f);
                                        //HitPM.StartCoroutine("DamageRoutine");
                                        //StartCoroutine("AttackByBullet");
                                        yield return StartCoroutine("ConditionForAttack", "no");
                                        CancelInvoke("AttackByBullet");
                                        //StopCoroutine("AttackByBullet");
                                        m_IsAttack = false;
                                        break;
                                    }
                                case "UnitBear":
                                    {
                                        StartCoroutine("BearAttackRoutine");
                                        yield return StartCoroutine("ConditionForAttack", "no");
                                        StopCoroutine("BearAttackRoutine");
                                        m_IsAttack = false;
                                        break;
                                    }
                                case "UnitDinosaur":
                                    {
                                        InvokeRepeating("AttackByFlare", 1.8f, 1.5f);
                                        yield return StartCoroutine("ConditionForAttack", "no");
                                        CancelInvoke("AttackByFlare");
                                        m_IsAttack = false;
                                        break;
                                    }
                                case "UnitRCcar":
                                    {
                                        CarAttakArea.gameObject.SetActive(true);
                                        yield return StartCoroutine("ConditionForAttack", "no");
                                        CarAttakArea.gameObject.SetActive(false);
                                        m_IsAttack = false;
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }
                        
                        else if (hit.transform.gameObject.layer == 27 
                            && !SelectUnitScript.m_Instance.IsBuildingMyTeam(hit.transform.GetComponent<BuildingStatus>()))  // Building
                        {
                            //NoticeScript.m_Instance.Notice("빌딩 타겟 완료\n");
                            m_IsPM = false;
                            m_IsBS = true;
                            HitBS = HitOb.GetComponent<BuildingStatus>();
                            HitBS.m_IsSelect = false;
                            HitBS.imgSelectbar.enabled = false;
                            HitBS.m_Damage = m_Power;
                            unitState = UnitState.attack;
                            yield return StartCoroutine("Picking", hit.point);
                            transform.rotation = Quaternion.LookRotation(hit.transform.position - transform.position);
                            m_Animator.SetBool("IsAttack", true);
                            switch (transform.tag)
                            {
                                case "UnitSoldier":
                                    {
                                        yield return StartCoroutine("ConditionForAttack", "AttackByBullet");
                                        m_IsAttack = false;
                                        break;
                                    }
                                case "UnitBear":
                                    {
                                        yield return StartCoroutine("ConditionForAttack", "BearAttackRoutine");
                                        m_IsAttack = false;
                                        break;
                                    }
                                case "UnitDinosaur":
                                    {
                                        yield return StartCoroutine("ConditionForAttack", "AttackByFlare");
                                        break;
                                    }
                                case "UnitRCcar":
                                    {
                                        CarAttakArea.gameObject.SetActive(true);
                                        yield return StartCoroutine("ConditionForAttack", "AttackByCar");
                                        CarAttakArea.gameObject.SetActive(false);
                                        m_IsAttack = false;
                                        break;
                                    }

                                default:
                                    break;
                            }

                        }

                    }
                }
            }

            else if (m_IsHeal)
            {
                imgSelectbar.enabled = false;
                imgHpbar.enabled = false;
                //SelectUnitScript.m_Instance.SelectedUnit.Remove(transform.GetComponent<PlayerMove>());
                m_IsSelect = false;
                HealEffect.SetActive(true);
                yield return StartCoroutine("HealRoutine");
                HealEffect.SetActive(false);
                HealArea.gameObject.SetActive(false);
                m_IsSelect = false;
            }

            yield return null;
        }
    }

    IEnumerator DamageRoutine()
    {
        while(true)
        {
            m_Hp -= 10f;
            imgHpbar.fillAmount = (float)m_Hp / (float)m_InitHp;
            if (m_Hp <= 0f)
            {
                StopCoroutine("DamageRoutine");
                
            }
            yield return new WaitForSeconds(2.0f);
        }
    }

    IEnumerator ConditionForAttack(string type)
    {
        
        if (m_IsPM && !m_IsBS)
        {
            imgHpbar.enabled = true;
            HitPM.m_IsSelect = false;
            HitPM.imgHpbar.enabled = true;
            HitPM.imgSelectbar.enabled = false;
            yield return StartCoroutine("TraceRoutine");
            //imgHpbar.enabled = false;
            //HitPM.imgHpbar.enabled = false;
            //HitPM.unitState = UnitState.die;
            //HitPM.m_Animator.SetBool("IsDie", true);
            //HitPM.m_IsAlive = false;
            //m_Animator.SetBool("IsAttack", false);
            //unitState = UnitState.idle;
            //m_Animator.SetBool("IsPick", false);
            //m_IsPM = false;
        }

        else if (!m_IsPM && m_IsBS)
        {
            imgHpbar.enabled = true;
            HitBS.imgHpbar.enabled = true;
            HitBS.imgSelectbar.enabled = false;
            HitBS.imgHpbar.enabled = true;
            if (type == "AttackByFlare")
                InvokeRepeating("AttackByFlare", 1.8f, 1.5f);
            else if(type == "AttackByBullet")
                InvokeRepeating("AttackByBullet", 0f, 1.0f);
            else
            {
                yield return StartCoroutine(type);
                imgHpbar.enabled = false;
                //HitBS.imgHpbar.enabled = false;
                //HitBS.m_IsAlive = false;
                m_Animator.SetBool("IsAttack", false);
                unitState = UnitState.idle;
                m_Animator.SetBool("IsPick", false);
                m_IsBS = false;
            }
        }

        else
            StopCoroutine("ConditionForAttack");
        yield return null;
    }

    IEnumerator HealRoutine()
    {
        m_Animator.SetBool("IsHeal", true);
        HealArea.gameObject.SetActive(true);
        while (true)
        {
            transform.Rotate(Vector3.up * 200f * Time.deltaTime);
            yield return null;
        }

    }

    IEnumerator MineralRoutine()
    {
        m_Animator.SetBool("IsMineral", true);
        while (true)
        {
            if (HitOb.tag == "B_Stars")
            {
                if (HitRS.m_Empty)
                {
                    Destroy(HitOb);
                    unitState = UnitState.idle;
                    m_Animator.SetBool("IsMineral", false);
                    StopCoroutine("MineralRoutine");
                }

                StarScript.m_Instance.m_StarNum += (m_Mineral);
                HitRS.m_gage += (m_Mineral);
            }

            else
                BatteryScript.m_Instance.m_BatteryNum += (m_Mineral);

            yield return new WaitForSeconds(1.5f);
        }
    }

    IEnumerator TraceRoutine()
    {
        float tracedis = 0.0f;
        switch (transform.tag)
        {
            case "UnitSoldier":
                tracedis = 5f;
                break;
            case "UnitDinosaur":
                tracedis = 8f;
                break;
            case "UnitBear":
                tracedis = 2f;
                break;
            case "UnitRCcar":
                tracedis = 2f;
                break;
            default: tracedis = 2f;
                break;

        }

        while (HitPM != null)//(HitPM.m_IsAlive)
        {
            if (HitPM.m_Hp <= 0) //(!HitPM.m_IsAlive)
            {
                HitPM.m_Hp = 0f;
                imgHpbar.enabled = false;
                HitPM.imgHpbar.enabled = false;
                HitPM.imgSelectbar.enabled = false;
                HitPM.unitState = UnitState.die;
               // HitPM.m_Animator.SetBool("IsDie", true);
               // HitPM.Invoke("Death",3f);
               // HitPM.m_IsAlive = false;
                m_IsPM = false;
                m_IsAttack = false;
                unitState = UnitState.idle;
                m_Animator.SetBool("IsPick", false);
                m_Animator.SetBool("IsAttack", false);
                StopAllCoroutines();
            }
                
            Debug.Log("추적 루틴 실행중");
            

            if (Vector3.Distance(transform.position, HitPM.transform.position) >= (tracedis + 3f))//8.5f)
            {
                if(transform.tag == "UnitRCcar")
                    CarAttakArea.gameObject.SetActive(false);
                m_Animator.SetBool("IsPick", true);
                m_Animator.SetBool("IsAttack", false);
                m_Attackstop = true;
                //m_Nav.enabled = true;

                NavMesh.CalculatePath(transform.position, HitPM.transform.position, NavMesh.AllAreas, m_Path);
                Vector3[] TraceCorners = m_Path.corners;
                int TraceIndex = 1;
                while (TraceIndex < TraceCorners.Length)
                {
                    m_Dir = (TraceCorners[TraceIndex] - transform.position).normalized;
                    transform.position += m_Dir * m_MoveSpeed * Time.deltaTime;
                    transform.rotation = Quaternion.LookRotation(m_Dir);
                    if (Vector3.Distance(transform.position, TraceCorners[TraceIndex]) < tracedis)
                        TraceIndex++;
                    yield return null;
                }

            }

            else
            {
                m_Animator.SetBool("IsPick", false);
                m_Animator.SetBool("IsAttack", true);
                if (transform.tag == "UnitRCcar")
                    CarAttakArea.gameObject.SetActive(true);
                m_Attackstop = false;
            }


            transform.rotation = Quaternion.LookRotation(HitPM.transform.position - transform.position);
            yield return null;
        }
    }

    public void AttackByBullet()
    {
        if((HitPM != null && HitPM.m_Hp <= 0f) || (HitBS != null && HitBS.m_Hp <= 0f))
        {
            m_Animator.SetBool("IsAttack", false);
            m_IsPM = false;
            m_IsBS = false;
            CancelInvoke("AttackByBullet");
        }

        if (m_Attackstop == false)
        {
            Obj = (GameObject)PhotonNetwork.Instantiate(Bullet.name, FireHole.position, FireHole.rotation, 0);
        }
    }

 

    IEnumerator AttackByCar()
    {
        bool condition = true;
        if (m_IsBS && !m_IsPM) condition = HitBS.m_IsAlive;
        if (m_IsPM && !m_IsBS) condition = HitPM.m_IsAlive;
        if (condition == false)
            StopCoroutine("AttackByCar");
        yield return null;
    }

    public void AttackByFlare()
    {
        bool condition = true;
        if (!m_IsPM && m_IsBS) condition = HitBS.m_IsAlive;
        if (m_IsPM && !m_IsBS) condition = HitPM.m_IsAlive;
        //else condition = false;

        if (condition == false)
        {
            FireHole.gameObject.SetActive(false);
            unitState = UnitState.idle;
            m_Animator.SetBool("IsAttack", false);
            m_IsBS = false;
            m_IsAttack = false;
            imgHpbar.enabled = false;
            CancelInvoke("AttackByFlare");
        }

        if (m_Attackstop == false)
        {
            FireHole.gameObject.SetActive(true);
            Invoke("SetFalse", 0.5f);
        }
    }
    public void SetFalse()
    {
        FireHole.gameObject.SetActive(false);
    }

    IEnumerator BearAttackRoutine()
    {
        bool condition = true;
        while (condition)
        {
            if (m_IsBS && !m_IsPM) condition = HitBS.m_IsAlive;
            if (m_IsPM && !m_IsBS) condition = HitPM.m_IsAlive;

            if (m_Attackstop == false)
            {
                if (m_IsPM)
                {
                    HitPM.m_Hp -= m_Damage;
                    HitPM.imgHpbar.enabled = true;
                    HitPM.imgHpbar.fillAmount = (float)HitPM.m_Hp / (float)HitPM.m_InitHp;
                }

                else if (m_IsBS)
                {
                    HitBS.m_Hp -= m_Damage;
                    HitBS.imgHpbar.enabled = true;
                    HitBS.imgHpbar.fillAmount = (float)HitBS.m_Hp / (float)HitBS.m_InitHp;
                }
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
        unitState = UnitState.walk;
        m_Animator.SetBool("IsPick", true);
        NavMesh.CalculatePath(transform.position, HitPoint, NavMesh.AllAreas, m_Path);
        Vector3[] Corners = m_Path.corners;
        int Index = 1;
        Vector3 m_Pos = transform.position;
        while (Index < Corners.Length)
        {
            Debug.DrawRay(Camera.main.transform.position, HitPoint - Camera.main.transform.position, Color.red);
            m_Dir = (Corners[Index] - transform.position).normalized;
            m_Pos += m_Dir * m_MoveSpeed * Time.deltaTime;
            m_Pos.y = Mathf.Clamp(m_Pos.y, float.MinValue, float.MaxValue - 0.01f);
            transform.position = m_Pos;

            if (transform.tag != "UnitAirballoon")
                transform.rotation = Quaternion.LookRotation(m_Dir);
            if (Vector3.Distance(transform.position, Corners[Index]) < dis)
                Index++;
            yield return null;
        }
        m_Animator.SetBool("IsPick", false);
        //m_Nav.enabled = true;
        unitState = UnitState.idle;
        StopCoroutine("Picking");
    }

    IEnumerator Landing_TakeOff(bool IsLanding)
    {
        Vector3 BallPos = BalloonHeight.localPosition;
        while (true)
        {
            if (IsLanding)
            {
                //Vector3 BallPos = BalloonHeight.position;
                if (BallPos.y <= 5.6f)
                {
                    BallPos.y += 0.01f;// * Time.deltaTime;
                    BalloonHeight.localPosition = BallPos;
                }
                else
                    StopCoroutine("Landing_TakeOff");
            }

            else
            {
                Debug.Log(BallPos);
                //Vector3 BallPos = BalloonHeight.position;
                if (BallPos.y > 0f)
                {
                    BallPos.y -= 0.1f;// 

                }
                else
                    StopCoroutine("Landing_TakeOff");

                BalloonHeight.localPosition += BallPos * Time.deltaTime;
            }
            yield return null;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "MeshLink")
        {
            m_Nav.enabled = false;
        }

        if (other.transform.tag == "HealArea")
        {
            if (SelectUnitScript.m_Instance.IsUnitMyTeam(other.GetComponentInParent<PlayerMove>()))  // 자기팀인지 검사
                m_IsInHealArea = true;
            else
                m_IsInHealArea = false;
        }


    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MeshLink")
        {
            m_Nav.enabled = true;
        }
        if (other.transform.tag == "HealArea")
        {
            m_IsInHealArea = false;
        }
    }

    private void OnCollisionEnter(Collision damage)
    {
        //if (m_Team.gameObject.layer == 23  
        //    && damage.transform.tag == "Bullet_blue")
        //{
        //    //if (!m_IsOffensive)
        //    //{
        //    //    HitPM = damage.gameObject.GetComponent<BulletRigidbody>().m_Owner;
        //    //    m_IsAttack = true;

        //    //    m_IsOffensive = true;
        //    //}


        //    m_Hp -= m_Damage;
        //    //if (m_Hp <= 0f)
        //    //    m_IsAlive = false;
        //    imgHpbar.enabled = true;
        //    imgHpbar.fillAmount = (float)m_Hp / (float)m_InitHp;
        //}

        //else if (m_Team.gameObject.layer == 22 
        //    && damage.transform.tag == "Bullet_red")
        //{
        //    //if (!m_IsOffensive)
        //    //{
        //    //    HitPM = damage.gameObject.GetComponent<BulletRigidbody>().m_Owner;
        //    //    m_IsAttack = true;

        //    //    m_IsOffensive = true;
        //    //}

        //    m_Hp -= m_Damage;
        //    if (m_Hp <= 0f)
        //    {
        //        m_IsAlive = false;
        //        m_Animator.SetBool("IsDie", true);
        //        Invoke("Death", 3f);
        //    }
        //    imgHpbar.enabled = true;
        //    imgHpbar.fillAmount = (float)m_Hp / (float)m_InitHp;
        //}

        //if (damage.transform.tag == "AttackArea")
        //{
        //    m_Hp -= (m_Damage + 5f);
        //    if (m_Hp <= 0f)
        //        m_IsAlive = false;
        //    imgHpbar.enabled = true;
        //    imgHpbar.fillAmount = (float)m_Hp / (float)m_InitHp;
        //}
    }



    IEnumerator InTheArea()
    {
        while (true)
        {
            Vector3 m_Pos = transform.position;
            m_Pos.y = Mathf.Clamp(m_Pos.y, float.MinValue, float.MaxValue - 0.01f);
            transform.position = m_Pos;
            //Debug.Log("area루틴실행중");
            if (m_IsInHealArea)
            {
                if (m_Hp < m_InitHp)
                    m_Hp += m_Heal;
                imgHpbar.enabled = true;
                imgHpbar.fillAmount = (float)m_Hp / (float)m_InitHp;
            }


            yield return new WaitForSeconds(1.5f);
        }

    }

    IEnumerator BuildRoutine()
    {
        UnitFuncScript.m_Instance.ClearFunc();
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
            m_Dir = (Corners[Index] - transform.position).normalized;
            transform.position += m_Dir * m_MoveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(m_Dir);
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
        SelectUnitScript.m_Instance.SelectedUnit.Remove(transform.GetComponent<PlayerMove>());
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

    public void Upgrade()
    {
        if (m_IsUpgraded == true)
            return;

        m_InitHp += 50f;
        m_Hp += 50f;
        m_MoveSpeed += 2f;
        m_Power += 5f;
        m_Mineral += 3;
        if (transform.tag == "UnitCupid")
        {
            HealArea.gameObject.GetComponent<SphereCollider>().radius += 3f;
            HealArea.GetComponentInChildren<Light>().spotAngle += 14f;
        }

        m_IsUpgraded = true;
        Vector3 Rot = Vector3.zero;
        Rot.x = -90f;

        GameObject Obj = (GameObject)Instantiate(UpgParticle,
                                                   transform.position,
                                                   Quaternion.Euler(Rot));

        Destroy(Obj, 3f);

        return;
    }

    public void SelectbyButton()
    {
        m_IsSelect = true;

    }


    public void Death()
    {
        --CurUnitNum.m_Instance.m_UnitNum;
        PhotonNetwork.Destroy(gameObject);
        //Destroy(SelectButton);
    }


    private void OnDestroy()
    {
        StopAllCoroutines();
        m_IsAlive = false;
    }
}
