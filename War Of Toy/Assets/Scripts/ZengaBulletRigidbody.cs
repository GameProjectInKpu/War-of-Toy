
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ZengaBulletRigidbody : Photon.PunBehaviour
{
    //private Rigidbody m_Rigidbody;
    public GameObject m_Particle;

    void Awake()
    {
        //m_Rigidbody = GetComponent<Rigidbody>();
        //m_Rigidbody.AddForce(transform.forward * 800f);
    }

    private void Update()
    {

    }

    void OnCollisionEnter(Collision Col)
    {
        GameObject Obj = (GameObject)PhotonNetwork.Instantiate(m_Particle.name,
                                                   Col.contacts[0].point,
                                                   //transform.position,   // 현재총알의 위치
                                                   Quaternion.Euler(Vector3.zero), 0);

        Destroy(Obj, 1f);
        //Destroy (gameObject);   // 총알자신을 삭제

        if (photonView.isMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }

    }
}
