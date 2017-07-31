using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class BuildPlaneScript : MonoBehaviour {

    private NavMeshAgent agent;
    public Transform[] RaycastPos;
    public Vector3[] RayPos;
    public Color m_Color;
    public bool Overlaped;

    void OnEnable()
    {
        
 
    }

    private void Awake()
    {
        StartCoroutine("CheckingCanBuild");
        m_Color = GetComponent<Renderer>().material.color;


    }


    IEnumerator CheckingCanBuild()
    {
        while (true)
        {
            NavMeshHit hit;
            for (int i = 0; i < RaycastPos.Length; ++i)
            {
                RayPos[i] = RaycastPos[i].position;

                Debug.DrawRay(RaycastPos[i].position, Vector3.down, Color.red);
                if (NavMesh.Raycast(RaycastPos[i].position, RaycastPos[i].position + Vector3.down, out hit, NavMesh.AllAreas)
                    || Overlaped == true)
                {
                    m_Color = Color.red;
                    break;
                }
                else
                    m_Color = Color.blue;

               
                if(BuildScript.BuildPos.y > 5 || (BuildScript.BuildPos.y < 4 && BuildScript.BuildPos.y > 0.5f))
                    m_Color = Color.red;


                RaycastPos[i].position = RayPos[i];
            }

            GetComponent<Renderer>().material.color = m_Color;


            //if(RaycastPos[i].position)
            yield return null;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        transform.localScale = Vector3.one;
        Debug.Log(transform.localScale);

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 28 || other.gameObject.layer == 27)
        {
            Debug.Log("유닛과 플레인 겹침");
            Overlaped = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 28 || other.gameObject.layer == 27)
        {
            Debug.Log("유닛과 플레인 분리됨");
            Overlaped = false;
        }
    }

}
