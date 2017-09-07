using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CamRotByScroll : MonoBehaviour
{

    public static Vector2 m_DragTouchDeltha;
    public static Vector2 m_EnterTouchDeltha;

    public Vector3 m_Rot;
    public Vector3 m_Pos;
    Scrollbar bar;
    private Transform m_Camera;
    private MoveCamera m_CameraMove;
    float TempValue;

    private void Start()
    {
        m_Camera = Camera.main.transform;
        m_CameraMove = m_Camera.GetComponent<MoveCamera>();
        bar = gameObject.GetComponent<Scrollbar>();
        bar.value = 0.7f;
        StartCoroutine("ScrollRoutine");

    }


   

    

    public void OnScrollDrag(BaseEventData EventData)
    {
        m_Rot = m_CameraMove.m_Rot;
        m_Pos = m_Camera.position;
        m_Rot.x = Mathf.Clamp(m_Rot.x, 10f, 90f);

        m_Rot.x = bar.value * 100f;        
        m_Pos.y = m_Rot.x / 2f;



        m_CameraMove.m_Rot = Vector3.Lerp(m_CameraMove.m_Rot, m_Rot, 1f);//m_Rot;
        m_CameraMove.m_Pos = Vector3.Lerp(m_CameraMove.m_Pos, m_Pos, 1f);// m_Pos;
    }

    


}
