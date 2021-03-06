﻿using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{
    public Vector3 m_Pos;
    public Vector3 m_Rot;
    private float m_CameraSpeed = 0.5f;
    public Transform m_PlayerPos;
    //public bool IsMove;

    //public GameObject AlphaMid;
    //public RenderTexture RT_Mid;

    static public MoveCamera m_Instance;
    static public MoveCamera Instance
    {
        get { return m_Instance; }
    }

    void Awake()
    {
        m_Instance = this;
        m_Rot.x = 70f;
        if(PhotonNetwork.isMasterClient)
        {
            m_Pos.x = 77f;
            m_Pos.y = 40f;
            m_Pos.z = 2.7f;
            transform.position = m_Pos;
        }
        else
        {
            m_Pos.x = 23f;
            m_Pos.y = 40f;
            m_Pos.z = 73f;
            transform.position = m_Pos;
        }

    }

    private void OnEnable()
    {
        //m_Pos = transform.position;
        m_Rot = transform.rotation.eulerAngles;
        StartCoroutine("MoveRoutine");
    }

    private void OnDisable()
    {
        StopAllCoroutines();
       
    }

    //private void Start()
    //{
    //    RT_Mid = AlphaMid.GetComponent<Camera>().targetTexture;

    //}

    private void Update()
    {
       
    }

    IEnumerator MoveRoutine()
    {
        while (true)
        {
            m_Pos.x -= m_CameraSpeed * TouchScript.m_Instance.m_TouchDeltha.x * Time.deltaTime;// * 10f;
            m_Pos.z -= m_CameraSpeed * TouchScript.m_Instance.m_TouchDeltha.y * Time.deltaTime;// * 10f;
            m_Pos.y -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 300f;
            
            m_Pos.x = Mathf.Clamp(m_Pos.x, 20f, 77f);
            m_Pos.z = Mathf.Clamp(m_Pos.z, 0f, 73f);
            m_Pos.y =  Mathf.Clamp(m_Pos.y, 15f, 50f);
            m_Rot.x = Mathf.Clamp(m_Rot.x, 10f, 90f);

            //m_Rot.x = 70f;
            transform.rotation = Quaternion.Euler(m_Rot);
            transform.position = m_Pos;

            //TouchScript.m_Instance.m_TouchDeltha.x

            yield return null;

        }
    }

    void OnDestroy()
    {
        m_Instance = null;
        StopAllCoroutines();
    }

    


    

}
