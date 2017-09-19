using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BulletRigidbody : Photon.PunBehaviour {

    //GameObject TargetOb;
    public PlayerMove TargetPM;
    public BuildingStatus TargetBS;

    private Rigidbody	m_Rigidbody;
	public  GameObject	m_Particle;
    public PlayerMove m_Owner;
    public  float m_Power;
    public bool IsSetTarget;
    

    void Awake(){
	
		m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.AddForce(transform.forward * 3000f);
        //IsSetTarget = false;


    }

    //private void Update()
    //{
    //    //if(TargetPM != null)
    //    //{
    //        //    TargetPM.imgHpbar.enabled = true;

    //        //    TargetPM.imgHpbar.fillAmount = (float)TargetPM.m_Hp / (float)TargetPM.m_InitHp;
    //        //    //IsSetTarget = true;
    //        //if (TargetPM.m_Hp <= 20f) //if (TargetPM.m_IsAlive == false)
    //        //{
    //        //    TargetPM.imgSelectbar.enabled = false;
    //        //    TargetPM.m_Hp = 0f;
    //        //    //TargetPM.m_Animator.SetBool("IsDie", true);
    //        //    //TargetPM.Invoke("Death", 10f);




    //        //    }
    //        //}

    //}

    


    void OnCollisionEnter(Collision Col)
	{
        if (Col.collider.gameObject.layer == 28)
        {
            TargetPM = Col.gameObject.GetComponent<PlayerMove>();
            if (TargetPM.m_IsStartDamage == false)
            {
                TargetPM.imgHpbar.enabled = true;
                TargetPM.StartCoroutine("DamageRoutine");
                TargetPM.m_IsStartDamage = true;
            }
            
            Debug.Log("공격받음!");
            //TargetPM.m_Hp -= 10f;

            //TargetPM.imgHpbar.fillAmount = (float)TargetPM.m_Hp / (float)TargetPM.m_InitHp;
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
            if (TargetBS.m_IsStartDamage == false)
            {
                TargetBS.imgHpbar.enabled = true;
                TargetBS.StartCoroutine("DamageRoutine");
                TargetBS.m_IsStartDamage = true;
            }

            Debug.Log("공격받음!");
            //TargetPM.m_Hp -= 10f;

            //TargetPM.imgHpbar.fillAmount = (float)TargetPM.m_Hp / (float)TargetPM.m_InitHp;
            //Col.gameObject.GetComponent<PlayerMove>().imgHpbar.fillAmount = TargetPM.imgHpbar.fillAmount;
            if (TargetBS.m_Hp <= 0f)
            {
                TargetBS.m_Hp = 0f;
                TargetBS.m_IsAlive = false;
                TargetBS = null;
            }

        }

        GameObject Obj = (GameObject)Instantiate ( m_Particle, 
												   Col.contacts[0].point,
													//transform.position,	// 현재총알의 위치
												   Quaternion.Euler(Vector3.zero));
        
		Destroy (Obj, 1.5f);
		//Destroy (gameObject);   // 총알자신을 삭제

        if (photonView.isMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }

    }
}
