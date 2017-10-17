using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingStatus : Photon.PunBehaviour
{

    public Transform m_Team;
    public float m_Hp = 100f;//300f;
    public float m_InitHp;
    public float m_Damage;
    public bool m_IsAlive;
    public bool m_IsSelect;

    public GameObject m_Particle;
    public GameObject m_ExpParticle;
    public bool m_IsParticle;

    public Image imgHpbar;
    public Image imgSelectbar;

    public bool m_IsStartDamage;


    GameObject Obj;
    GameObject Expl;

    //public Material 


    void Start()
    {
        imgHpbar.enabled = false;
        imgSelectbar.enabled = false;
        m_IsAlive = true;
    }

    void Awake()
    {
        m_Hp = 100f;//300f;
        m_InitHp = m_Hp;
    }

    private void Update()
    {
        if (m_IsSelect == true)
        {
            switch (transform.tag)
            {
                case "B_ToyCastle":
                    InitUnitScript.m_Castle = transform;
                    break;
                case "B_ToyFactory":
                    InitUnitScript.m_Factory = transform;
                    break;
                case "B_Hospital":
                    InitUnitScript.m_Hospital = transform;
                    break;
                default:
                    break;
            }
        }

        if (m_Hp < 50f && !m_IsParticle)
        {
            Vector3 Pos = transform.position;
            Vector3 Rot = Vector3.zero;
            Pos.x -= 3f;
            Pos.z -= 3.5f;
            Rot.x -= 50f;

            Obj = (GameObject)PhotonNetwork.Instantiate(m_Particle.name, Pos, Quaternion.Euler(Rot), 0);
            m_IsParticle = true;
        }

        if (m_Hp < 0f)
        {
            m_IsAlive = false;
            
        }

        if (m_IsAlive == false)
        {
            PhotonNetwork.Destroy(Obj);
            Expl = (GameObject)Instantiate(m_ExpParticle, transform.position, Quaternion.Euler(Vector3.zero));
            Invoke("Death", 1.5f);
        }

    }

    IEnumerator DamageRoutine()
    {
        while (true)
        {
            m_Hp -= 10f;
            imgHpbar.fillAmount = (float)m_Hp / (float)m_InitHp;
            if (m_Hp <= 0f)
            {
                m_IsAlive = false;
                StopCoroutine("DamageRoutine");

            }
            yield return new WaitForSeconds(2.0f);
        }
    }


    private void OnCollisionEnter(Collision damage)
    {
        //if (m_Team.gameObject.layer == 23 && damage.transform.tag == "Bullet_blue")
        //{
        //    //Debug.Log("총에 맞음");
        //    m_Hp -= m_Damage;
        //    if (m_Hp <= 0f)
        //        m_IsAlive = false;
        //    imgHpbar.enabled = true;
        //    imgHpbar.fillAmount = (float)m_Hp / (float)m_InitHp;
        //}

        //else if (m_Team.gameObject.layer == 22 && damage.transform.tag == "Bullet_red")
        //{
        //    m_Hp -= m_Damage;
        //    if (m_Hp <= 0f)
        //        m_IsAlive = false;
        //    imgHpbar.enabled = true;
        //    imgHpbar.fillAmount = (float)m_Hp / (float)m_InitHp;
        //}

        if (damage.transform.tag == "AttackArea")
        {
            m_Hp -= m_Damage;
            if (m_Hp < 0f)
                m_IsAlive = false;
            imgHpbar.enabled = true;
            imgHpbar.fillAmount = (float)m_Hp / (float)m_InitHp;
        }
    }

    public void Death()
    {
        if (transform.tag == "B_ToyCastle")
        {
            //if(SelectUnitScript.m_Instance.IsBuildingMyTeam(this) == true)
            //{
            //    SelectUnitScript.m_Instance.m_Victory.SetActive(true);
            //    SelectUnitScript.m_Instance.m_Defeat.SetActive(false);
            //}

            //else
            //{
            //    SelectUnitScript.m_Instance.m_Victory.SetActive(false);
            //    SelectUnitScript.m_Instance.m_Defeat.SetActive(true);
            //}
            if (PhotonNetwork.isMasterClient)   
            {
                if (m_Team.gameObject.layer == 22)
                {
                    SelectUnitScript.m_Instance.m_Victory.SetActive(true);
                    SelectUnitScript.m_Instance.m_Defeat.SetActive(false);
                }

                else
                {
                    SelectUnitScript.m_Instance.m_Victory.SetActive(false);
                    SelectUnitScript.m_Instance.m_Defeat.SetActive(true);
                }
            }

            else
            {
                if (m_Team.gameObject.layer == 23)
                {
                    SelectUnitScript.m_Instance.m_Victory.SetActive(true);
                    SelectUnitScript.m_Instance.m_Defeat.SetActive(false);
                }

                else
                {
                    SelectUnitScript.m_Instance.m_Victory.SetActive(false);
                    SelectUnitScript.m_Instance.m_Defeat.SetActive(true);
                }
            }
        }
        Destroy(Expl);
        PhotonNetwork.Destroy(gameObject);
        //Destroy(SelectButton);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        m_IsAlive = false;
    }

    //public void UpgradeByButton()
    //{
    //    // 유닛 선택
    //    SelectUnitScript.m_Instance.m_IsUpgradeMode = true;
    //    SelectUnitScript.m_Instance.LabPos = transform;



    //    //if (// 선택한 유닛이 자기 팀이면 && unit.m_IsUpgrade == false)
    //    //{
    //    //    unit.m_IsUpgrade = true;

    //    //    //업그레이드
    //    //}
    //    // 업그레이드 내용은 유닛의 체력, 공격력, 미네랄 캐는 능력?
    //}



}

