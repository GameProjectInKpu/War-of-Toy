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

    // Use this for initialization
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

    private void OnCollisionEnter(Collision bullet)
    {
        if (bullet.transform.tag == "Bullet")
        {
            m_Hp -= 10f;
            if (m_Hp < 0f)
                m_IsAlive = false;
            imgHpbar.enabled = true;
            imgHpbar.fillAmount = (float)m_Hp / (float)m_InitHp;
        }
    }
}
