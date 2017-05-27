using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BulletRigidbody : Photon.PunBehaviour {

	private Rigidbody	m_Rigidbody;
	public  GameObject	m_Particle;

    GameObject TargetOb;
    PlayerMove TargetPM;
    //public Image imgHpbar;

    void Awake(){
	
		m_Rigidbody = GetComponent<Rigidbody> ();
		m_Rigidbody.AddForce(transform.forward * 800f);
	
	}

	void OnCollisionEnter(Collision Col)
	{
		GameObject Obj = (GameObject)Instantiate ( m_Particle, 
												   Col.contacts[0].point,
													//transform.position,	// 현재총알의 위치
												   Quaternion.Euler(Vector3.zero));
        if(Col.collider.gameObject.layer == 28)
        {
            //Debug.Log("공격받음!");
            TargetOb = Col.collider.gameObject;
            TargetPM = TargetOb.GetComponent<PlayerMove>();
            TargetPM.m_Hp -= 10f;
            if (TargetPM.m_Hp < 0f)
                TargetPM.m_IsAlive = false;
            TargetPM.imgHpbar.enabled = true;
            TargetPM.imgHpbar.fillAmount = (float)TargetPM.m_Hp / (float)TargetPM.m_InitHp;
        }

		Destroy (Obj, 1f);
		//Destroy (gameObject);   // 총알자신을 삭제

        if (photonView.isMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }

    }
}
