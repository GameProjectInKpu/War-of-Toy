using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchScript : MonoBehaviour {

    public bool IsOver;
    public Vector2 m_TouchDeltha;
    public Vector2[] m_CurTouches = new Vector2[2];
    public Vector2[] m_PrevTouches = new Vector2[2];

    public static TouchScript m_Instance;
    public static TouchScript Instance
    {
        get { return m_Instance; }
    }

    void Awake()
    {
        m_Instance = this;
        StartCoroutine("MultiTouchesRoutine");
    }


    IEnumerator MultiTouchesRoutine()
    {
        while (true)
        {
            if (Input.touchCount == 2)
            {
                m_CurTouches[0] = Input.GetTouch(0).position;
                m_CurTouches[1] = Input.GetTouch(1).position;

                if (m_PrevTouches[0] != Vector2.zero && m_PrevTouches[1] != Vector2.zero)
                {
                    float CurDistance = Vector2.Distance(m_CurTouches[0], m_CurTouches[1]);
                    float PrevDistance = Vector2.Distance(m_PrevTouches[0], m_PrevTouches[1]);
                    MoveCamera.m_Instance.m_Pos.y -= (CurDistance - PrevDistance) * Time.deltaTime;
                }
                m_PrevTouches[0] = m_CurTouches[0];
                m_PrevTouches[1] = m_CurTouches[1];
            }

            else
                m_PrevTouches[0] = m_PrevTouches[1] = Vector2.zero;

            yield return null;
        }
    }



    

    public void OnTouch(BaseEventData EventData)
    {
        PointerEventData PointEvent = EventData as PointerEventData;
        m_TouchDeltha = PointEvent.delta;
        IsOver = false;
    }

    public void EndTouch()
    {
        m_TouchDeltha = Vector2.zero;
        IsOver = true;
    }

    public void PointerEnter(bool isover)
    {
        IsOver = isover;
    }

    public void PointerExit(bool isover)
    {
        IsOver = isover;
    }

    void OnDestroy()
    {
        m_Instance = null;
    }

}
