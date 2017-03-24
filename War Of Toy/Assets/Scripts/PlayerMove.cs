using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    private Vector3 m_Pos;
    public float m_MoveSpeed;
    public LayerMask m_LayerMask;
    public NavMeshPath m_Path;


    void OnEnable()
    {
        StartCoroutine("Picking");
    }

    void Awake()
    {
        m_Path = new NavMeshPath();
        m_MoveSpeed = 20f;
    }


    IEnumerator Picking()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LayerMask))
                {
                    Debug.Log("Pick");
                    NavMesh.CalculatePath(transform.position, hit.point, NavMesh.AllAreas, m_Path);     // m_Path = 경로
                    StopCoroutine("MoveRoutine");
                    StartCoroutine("MoveRoutine");
                }
            }

            yield return null;
        }
    }

    IEnumerator MoveRoutine()   // 경로로 이동
    {
        Vector3[] Corners = m_Path.corners;
        int Index = 1;  // 1부터 시작
        while (Index < Corners.Length)
        {
            m_Pos = (Corners[Index] - transform.position).normalized;   // m_Pos 방향
            transform.position += m_Pos * m_MoveSpeed * Time.deltaTime; // 이동
            //transform.rotation.SetLookRotation(m_Pos);  // 해당 방향으로 회전
            transform.rotation = Quaternion.LookRotation(m_Pos);    // 해당 방향으로 회전
            if (Vector3.Distance(transform.position, Corners[Index]) < 1f)
                Index++;

            yield return null;
        }
    }

    //public Rigidbody m_Rigidbody;
    //public float m_MoveSpeed;
    //public float m_RotationSpeed;


    //void Awake()
    //{
    //    m_Rigidbody = GetComponent<Rigidbody>();
    //    m_MoveSpeed = 100;
    //    m_RotationSpeed = 100;


    //}

    //void FixedUpdate()
    //{
    //    float Vertical = Input.GetAxis("Vertical");
    //    float Horizontal = Input.GetAxis("Horizontal");

    //    m_Rigidbody.MovePosition(transform.position + (transform.forward * Vertical * Time.fixedDeltaTime * m_MoveSpeed));
    //    m_Rigidbody.MoveRotation(Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, Horizontal * Time.fixedDeltaTime * m_RotationSpeed, 0f)));

    //}

}
