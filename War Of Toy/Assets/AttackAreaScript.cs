using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaScript : Photon.PunBehaviour
{

    public PlayerMove TargetPM;
    public BuildingStatus TargetBS;

    

    void OnCollisionEnter(Collision Col)
    {
        if (Col.collider.gameObject.layer == 28)
        {
            TargetPM = Col.gameObject.GetComponent<PlayerMove>();
            //if (TargetPM.m_IsStartDamage == false)
            {
                TargetPM.imgHpbar.enabled = true;
                TargetPM.m_Hp -= 15f;
             //   TargetPM.StartCoroutine("DamageRoutine");
             //   TargetPM.m_IsStartDamage = true;
            }
            //TargetPM.m_Hp -= 10f;

            TargetPM.imgHpbar.fillAmount = (float)TargetPM.m_Hp / (float)TargetPM.m_InitHp;
            //Col.gameObject.GetComponent<PlayerMove>().imgHpbar.fillAmount = TargetPM.imgHpbar.fillAmount;
            if (TargetPM.m_Hp <= 0f)
            {
                TargetPM.m_Hp = 0f;
                TargetPM.m_IsAlive = false;
                TargetPM = null;
            }

        }

        else if (Col.collider.gameObject.layer == 27)
        {
            TargetBS = Col.gameObject.GetComponent<BuildingStatus>();
           // if (TargetBS.m_IsStartDamage == false)
            {
                TargetBS.imgHpbar.enabled = true;
                TargetBS.m_Hp -= 15f;
                //   TargetBS.StartCoroutine("DamageRoutine");
                //   TargetBS.m_IsStartDamage = true;
            }

            //TargetPM.m_Hp -= 10f;

            TargetBS.imgHpbar.fillAmount = (float)TargetBS.m_Hp / (float)TargetBS.m_InitHp;
            //Col.gameObject.GetComponent<PlayerMove>().imgHpbar.fillAmount = TargetPM.imgHpbar.fillAmount;
            if (TargetBS.m_Hp <= 0f)
            {
                TargetBS.m_Hp = 0f;
                TargetBS.m_IsAlive = false;
                TargetBS = null;
            }

        }

       

    }
}
