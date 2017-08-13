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

    public Image imgHpbar;
    public Image imgSelectbar;

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
                    
                default:
                    break;
            }
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
        Destroy(gameObject);
        //Destroy(SelectButton);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        m_IsAlive = false;
    }

    public void UpgradeByButton()
    {
        // 유닛 선택
        // 선택한 유닛이 자기 팀인지 
        // 자기 팀이면 업그레이드
        // 업그레이드 내용은 유닛의 체력, 공격력, 미네랄 캐는 능력?
    }
}


