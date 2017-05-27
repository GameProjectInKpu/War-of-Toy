using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class BuildPlaneScript : MonoBehaviour {

    private NavMeshAgent agent;
    public Transform[] RaycastPos;
    public Vector3[] RayPos;

    void OnEnable()
    {
        
 
    }

    private void Awake()
    {
        StartCoroutine("CheckingCanBuild");
        
        
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
                if (NavMesh.Raycast(RaycastPos[i].position, RaycastPos[i].position + Vector3.down, out hit, NavMesh.AllAreas))
                {
                    GetComponent<Renderer>().material.color = Color.red;
                    break;
                }
                else
                    GetComponent<Renderer>().material.color = Color.blue;

                RaycastPos[i].position = RayPos[i];
            }
            

            yield return null;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        transform.localScale = Vector3.one;
        Debug.Log(transform.localScale);

    }


}
