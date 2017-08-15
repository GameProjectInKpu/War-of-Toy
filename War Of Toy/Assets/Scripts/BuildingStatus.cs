using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingStatus : MonoBehaviour {

    public Transform m_Team;
    public float m_Hp = 100f;
    public float m_InitHp;
    public bool m_IsAlive;
    public bool m_IsSelect;

    public GameObject m_Particle;
    public bool m_IsParticle;

    public Image imgHpbar;
    public Image imgSelectbar;
    GameObject Obj;

    //public Material 


    void Start()
    {
        imgHpbar.enabled = false;
        imgSelectbar.enabled = false;
        m_IsAlive = true;
    }

    void Awake()
    {
        m_Hp = 100f;
        m_InitHp = m_Hp;
    }

    private void Update()
    {
        if(m_IsSelect == true)
        {
            switch(transform.tag)
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

        if(m_Hp < 50f && !m_IsParticle)
        {
            Vector3 Pos = transform.position;
            Vector3 Rot = Vector3.zero;
            Pos.x -= 3f;
            Pos.z -= 3.5f;
            Rot.x -= 50f;

            Obj = (GameObject)Instantiate(m_Particle, Pos,	Quaternion.Euler(Rot));
            m_IsParticle = true;
        }
        
        if(m_Hp < 0f)
        {
            m_IsAlive = false;
        }

        if(m_IsAlive == false)
        {
            Invoke("Death", 3f);
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

        else if (damage.transform.tag == "AttackArea")
        {
            m_Hp -= 10f;
            if (m_Hp < 0f)
                m_IsAlive = false;
            imgHpbar.enabled = true;
            imgHpbar.fillAmount = (float)m_Hp / (float)m_InitHp;
        }
    }

    public void Death()
    {
        if(transform.tag == "B_ToyCastle")
        {

        }
        Destroy(Obj);
        Destroy(gameObject);
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


