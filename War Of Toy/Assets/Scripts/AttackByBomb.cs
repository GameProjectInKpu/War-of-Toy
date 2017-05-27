using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackByBomb : MonoBehaviour {

    public GameObject Bomb;
    public static Transform CurBalloon;
    public Transform FireHole;

    void Start () {

    }
	
    public void BombAttack()
    {
        FireHole = CurBalloon.Find("FireHoleBalloon");
        GameObject Obj = (GameObject)Instantiate(Bomb, FireHole.position, FireHole.rotation);
        //GameObject Obj = (GameObject)PhotonNetwork.Instantiate(Bomb.name, FireHole.position, FireHole.rotation, 0);

        CurBalloon.GetComponent<PlayerMove>().m_IsSelect = false;
        CurBalloon.GetComponent<PlayerMove>().imgSelectbar.enabled = false;
        CurBalloon.GetComponent<PlayerMove>().StartCoroutine("OrderRoutine");
    }
}
