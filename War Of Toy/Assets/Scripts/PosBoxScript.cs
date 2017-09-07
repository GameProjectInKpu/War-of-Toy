using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosBoxScript : MonoBehaviour {

    public Vector3 StartPos;
    public Vector3 EndPos;
    private Transform m_Camera;
    private MoveCamera m_CameraMove;
    public bool IsMainCamMove;
    
    //private BuildScript m_BuildMove;


    public float m_AxisX;
    public float m_AxisY;

    void Start()
    {
        m_Camera = Camera.main.transform;
        m_CameraMove = m_Camera.GetComponent<MoveCamera>();
        //m_BuildMove = GameObject.Find("build").GetComponent<BuildScript>();
        StartPos = GameObject.Find("PosBoxStart").transform.position;
        EndPos = GameObject.Find("PosBoxEnd").transform.position;

        m_AxisX = EndPos.x - StartPos.x;
        m_AxisY = EndPos.y - StartPos.y;
        
    }

    public void PointerDown()
    {
        StartCoroutine("PosBoxMovedWithMouse");
    }


    public void PointerUp()
    {
        StopCoroutine("PosBoxMovedWithMouse");
        //if (m_BuildMove.enabled == true)
        //    m_BuildMove.m_IsClickBuilding = true;
    }

    private void Update()
    {
        if (TouchScript.m_Instance.m_TouchDeltha == Vector2.zero)
            IsMainCamMove = false;
        else
            IsMainCamMove = true;

        if (IsMainCamMove == true)
        {
            StopCoroutine("PosBoxMovedWithMainCam");
            StartCoroutine("PosBoxMovedWithMainCam");
        }
        
    }

    IEnumerator PosBoxMovedWithMainCam()  // 카메라 움직일때 박스 움직임
    {
        while (true)
        {
            if (IsMainCamMove == false)    break;

            Vector3 CameraPos = m_Camera.position;
            float X = CameraPos.x - 17f;    // 메인카메라가 움직인 거리
            float Z = CameraPos.z - 0f;// 4f;

            X /= 61f;   // 메인카메라가 움직일 수 있는 거리
            Z /= 72f;// 96f;   

            X *= (EndPos.x + 10f - StartPos.x); // 박스가 움직일수 있는 실제 거리
            Z *= (EndPos.y + 10f - StartPos.y); 

            Vector3 Pos = Vector3.zero;
            Pos.x = StartPos.x + X;
            CameraPos.y = m_CameraMove.m_Pos.y;
            //CameraPos.y = 40;
            Pos.y = StartPos.y + Z ;

            transform.position = Pos;
            yield return null;
        }
    }



    IEnumerator PosBoxMovedWithMouse()  // 클릭으로 박스 움직임
    {
        while (true)
        {
            Vector3 Pos = Input.mousePosition;

            Pos.x = Mathf.Clamp(Pos.x, StartPos.x, EndPos.x);
            Pos.y = Mathf.Clamp(Pos.y, StartPos.y, EndPos.y);
            Pos.z = 0f;

            transform.position = Pos;

            //if (m_BuildMove.enabled == true)
            //    m_BuildMove.m_IsClickBuilding = false;

            float X = Pos.x - StartPos.x;
            float Y = Pos.y - StartPos.y;

            X /= m_AxisX;
            Y /= m_AxisY;

            X *= 100f; // 카메라가 움직일수 있는 실제 거리
            Y *= 100f; // 카메라가 움직일수 있는 실제 거리

            Vector3 CameraPos = Vector3.zero;
            CameraPos.x = X;
            CameraPos.y = m_CameraMove.m_Pos.y;
            //CameraPos.y = 40f;
            CameraPos.z = Y;// - 20f;

            //m_Camera.position = CameraPos;
            m_CameraMove.m_Pos = CameraPos;
            yield return null;
        }
    }
}
