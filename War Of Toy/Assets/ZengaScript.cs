using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ZengaScript : MonoBehaviour {

    public Transform TargetUnit;  // 타겟 유닛

    public Transform[] FireHole;    // 주어진 발사 시작점
    public Transform FirePos;
    public GameObject Bullet;
    private Vector3 Dir;
    private Rigidbody m_Rigidbody;

    void Awake () {

        FirePos = FireHole[0];
    }

    private void OnTriggerEnter(Collider unit)
    {
        if (unit.gameObject.layer != 28)
            return;
        TargetUnit = unit.gameObject.transform;

        for (int i = 0; i < FireHole.Length; ++i)
        {
            if (Vector3.Distance(FirePos.position, unit.transform.position)
                > Vector3.Distance(FireHole[i].position, unit.transform.position))
                FirePos = FireHole[i];
        }


        StartCoroutine("AttackByBullet");
    }

   

    private void OnTriggerExit(Collider unit)
    {
        if (unit.gameObject.layer != 28)
            return;

        StopCoroutine("AttackByBullet");
    }

    IEnumerator AttackByBullet()
    {
        while (true)
        {
            for (int i = 0; i < FireHole.Length; ++i)
            {
                if (Vector3.Distance(FirePos.position, TargetUnit.transform.position)
                    > Vector3.Distance(FireHole[i].position, TargetUnit.transform.position))
                    FirePos = FireHole[i];
            }


            Dir = (TargetUnit.transform.position - FirePos.position);//.normalized;

            GameObject Obj = (GameObject)PhotonNetwork.Instantiate(Bullet.name, FirePos.position, FirePos.rotation, 0);
            m_Rigidbody = Obj.GetComponent<Rigidbody>();
            m_Rigidbody.AddForce(Dir * 250f);

            
            yield return new WaitForSeconds(2.5f);
        }

    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
