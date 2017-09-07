using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxCamScript : MonoBehaviour {

    public bool IsClick;
    public bool IsMainCamMove;
    public Vector3 m_BoxPos;
    public Vector3 MainCam;
    public float MainCamDist;
    public Transform StartBox;
    public Transform EndBox;

    public float m_AxisX;   // 박스캠의 가로길이
    public float m_AxisY;   // 박스캠의 세로길이


    void Start () {

        m_AxisX = EndBox.position.x - StartBox.position.x;
        m_AxisY = EndBox.position.y - StartBox.position.y;

        m_BoxPos = transform.position;
        //MainCam = MoveCamera.m_Instance.m_Pos;
    }

    private void Update()
    {
       // IsMainCamMove = MoveCamera.m_Instance.IsMove;
        if(IsMainCamMove == true)
        {
            StopCoroutine("MovingCamWithMainCam");
            StartCoroutine("MovingCamWithMainCam");
        }

    }

    IEnumerator MovingCamWithMainCam()  // 메인카메라 움직임에따라 박스카메라가 움직임
    {
        while (true)
        {
            if (IsMainCamMove == false)
                break;
          //  MainCam = MoveCamera.m_Instance.m_Pos;
            
            m_BoxPos.x = (MainCam.x - 10f) / m_AxisX * 900f + StartBox.position.x; //* 150f + StartBox.position.x; //* 20f + StartBox.position.x; // 현재카메라x위치 - 처음지형위치 /  
            m_BoxPos.y = (MainCam.z - 4f) / m_AxisY * 1000f + StartBox.position.z; //* 190f + StartBox.position.z; //* 20f + StartBox.position.z; // 현재카메라 z위치 - 처음지형위치 /  

            m_BoxPos.x = Mathf.Clamp(m_BoxPos.x, StartBox.position.x, EndBox.position.x);
            m_BoxPos.y = Mathf.Clamp(m_BoxPos.y, StartBox.position.y, EndBox.position.y);

            //m_BoxPos.Scale = MainCamDist.

            transform.position = m_BoxPos;
            yield return null;
        }
    }

    IEnumerator MovingCamWithMouse()    // 마우스가 움직일 때마다 박스카메라가 움직임
    {
        while (true)
        {
            if (IsClick == false)
                break;

            m_BoxPos = Input.mousePosition;
            m_BoxPos.x = Mathf.Clamp(m_BoxPos.x, StartBox.position.x, EndBox.position.x);
            m_BoxPos.y = Mathf.Clamp(m_BoxPos.y, StartBox.position.y, EndBox.position.y);

            MainCam.x = (m_BoxPos.x - StartBox.position.x) / m_AxisX * 100f;
            MainCam.z = (m_BoxPos.y - StartBox.position.y) / m_AxisY * 100f;

            transform.position = m_BoxPos;
         //   MoveCamera.m_Instance.m_Pos = MainCam;

            yield return null;
        }
    } 

    public void SetClick(bool Click)
    {
        
        IsClick = Click;
        if(IsClick == true)
        {
            StartCoroutine("MovingCamWithMouse");
        }
    }

}
