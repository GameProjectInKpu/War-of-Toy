using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CamRotByScroll : MonoBehaviour {

    public static Vector2 m_DragTouchDeltha;
    public static Vector2 m_EnterTouchDeltha;

    public Vector3 m_Rot;
    Scrollbar bar;
    private Transform m_Camera;
    private MoveCamera m_CameraMove;
    float TempValue;

    private void Awake () {
        m_Camera = Camera.main.transform;
        m_CameraMove = m_Camera.GetComponent<MoveCamera>();
        m_Rot = m_Camera.rotation.eulerAngles;
        bar = gameObject.GetComponent<Scrollbar>();
        bar.value = 0.3f;
        
    }
	

	private void Update () {
        m_Rot.x = Mathf.Clamp(m_Rot.x, 50f, 90f);
    }

    public void OnScrollEnter(BaseEventData EventData)
    {
        PointerEventData PointEvent = EventData as PointerEventData;
        m_EnterTouchDeltha = PointEvent.delta;
      
    }

    public void OnScrollDrag(BaseEventData EventData)
    {
        PointerEventData PointEvent = EventData as PointerEventData;
        m_DragTouchDeltha = PointEvent.delta;

        if (m_DragTouchDeltha.y < m_EnterTouchDeltha.y)
            m_Rot.x -= (bar.value * 2.0f);
        else
            m_Rot.x += (bar.value * 1.4f);
        m_CameraMove.m_Rot = m_Rot;
    }

    public void OnScrollDrop(BaseEventData EventData)
    {
        m_EnterTouchDeltha = Vector2.zero;
        m_DragTouchDeltha = Vector2.zero;
    }


}
