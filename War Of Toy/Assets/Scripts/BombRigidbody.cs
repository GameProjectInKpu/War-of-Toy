using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombRigidbody : MonoBehaviour {

    private Rigidbody m_Rigidbody;
    public GameObject m_Particle;
    private BoxCollider m_Collider;

    GameObject Obj;
    GameObject TargetOb;
    PlayerMove TargetPM;
    private bool IsParticle;

    void Awake()
    {
        m_Collider = GetComponent<BoxCollider>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.AddForce(-transform.up * 500f);

    }


    void OnCollisionEnter(Collision Col)
    {
        if (IsParticle)
            goto skip;
        Obj = (GameObject)Instantiate(m_Particle, Col.contacts[0].point, Quaternion.Euler(Vector3.zero));

        IsParticle = true;
        skip:
        ;

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        

        if (Col.collider.gameObject.layer == 28)
        {
            Debug.Log("공격받음!");
            TargetOb = Col.collider.gameObject;
            TargetPM = TargetOb.GetComponent<PlayerMove>();
            TargetPM.m_Hp -= 30f;
            if (TargetPM.m_Hp < 0f)
                TargetPM.m_IsAlive = false;
            TargetPM.imgHpbar.enabled = true;
            TargetPM.imgHpbar.fillAmount = (float)TargetPM.m_Hp / (float)TargetPM.m_InitHp;
           
        }

        Destroy(Obj, 1f);
        
        Destroy(gameObject, 3f);
    }

    private void OnDestroy()
    {
        IsParticle = false;
    }
}
