using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MultiSelectScript : MonoBehaviour {

    public Vector2 m_StartPoint;
    public Vector2 m_EndPoint;
    public Vector2 DrawingPoint;
    public Vector2 Pivot;
    public Vector2 m_Size;
    public Rect rect;
    public bool IsOnMulti;
    public GameObject m_SelectImage;
    //public bool IsStartPoint;

    // Use this for initialization
    void Start () {
        Pivot = m_SelectImage.GetComponent<RectTransform>().pivot;
    }
	
    private void MultiSelect()
    {
        if (m_StartPoint.y > m_EndPoint.y)
        {
            if (m_StartPoint.x < m_EndPoint.x)
            {
                float tmp = m_StartPoint.y;
                m_StartPoint.y = m_EndPoint.y;
                m_EndPoint.y = tmp;
            }
                
            else
            {
                Vector2 tmp = m_StartPoint;
                m_StartPoint = m_EndPoint;
                m_EndPoint = tmp;
            }
        }

        else
        {
            if(m_StartPoint.x > m_EndPoint.x)
            {
                float tmp = m_StartPoint.x;
                m_StartPoint.x = m_EndPoint.x;
                m_EndPoint.x = tmp;
            }
        }
            
        m_Size = (m_EndPoint - m_StartPoint);
        rect = new Rect(m_StartPoint, m_Size); 

        if (m_Size.x < 0)
            m_Size.x = -m_Size.x;
        if (m_Size.y < 0)
            m_Size.y = -m_Size.y;

        rect = new Rect(m_StartPoint, m_Size);
        m_SelectImage.GetComponent<RectTransform>().sizeDelta = m_Size * 2.5f;

        foreach (PlayerMove unit in SelectUnitScript.m_Instance.LivingUnit)
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
            Debug.DrawRay(Camera.main.transform.position, unit.transform.position - Camera.main.transform.position, Color.red);
            Debug.Log(screenPos);
            if (rect.Contains(screenPos))
            {
                Debug.Log(unit.name);
                SelectUnitScript.m_Instance.SelectUnit(unit.transform);
            }
        }
        m_Size = Vector2.zero;
        m_SelectImage.SetActive(false);
        m_SelectImage.GetComponent<RectTransform>().pivot = Vector2.one;
        m_SelectImage.GetComponent<RectTransform>().sizeDelta = Vector2.one;

    }

   

    public void StartPointToSelect()
    {
        if (!IsOnMulti ) return;
        
        m_StartPoint = Input.mousePosition;
        m_SelectImage.SetActive(true);
        m_SelectImage.transform.position = m_StartPoint;
    }
    public void DrawRect()
    {
        DrawingPoint = Input.mousePosition;
        
        if (m_StartPoint.y > DrawingPoint.y)
        {
            if (m_StartPoint.x < DrawingPoint.x)
            {
                Pivot.x = 0;
                Pivot.y = 1;
                float tmp = m_StartPoint.y;
                m_StartPoint.y = DrawingPoint.y;
                DrawingPoint.y = tmp;
            }

            else
            {
                Pivot.x = 1;
                Pivot.y = 1;
                Vector2 tmp = m_StartPoint;
                m_StartPoint = DrawingPoint;
                DrawingPoint = tmp;
            }
        }

        else
        {
            if (m_StartPoint.x > DrawingPoint.x)
            {
                Pivot.x = 1;
                Pivot.y = 0;
                float tmp = m_StartPoint.x;
                m_StartPoint.x = DrawingPoint.x;
                DrawingPoint.x = tmp;
            }
        }
        m_SelectImage.GetComponent<RectTransform>().pivot = Pivot;
        m_Size = (DrawingPoint - m_StartPoint);

        if (m_Size.x < 0)
            m_Size.x = -m_Size.x;
        if (m_Size.y < 0)
            m_Size.y = -m_Size.y;

        
        m_SelectImage.GetComponent<RectTransform>().localScale = m_Size * 2.5f;
    }

    public void EndPointToSelect()
    {
        if (!IsOnMulti) return;
        m_EndPoint = Input.mousePosition;
        MultiSelect();
        MoveCamera.m_Instance.enabled = true;
        IsOnMulti = false;
        
    }

    public void OnMultiSelectMode()
    {
        IsOnMulti = true;
        MoveCamera.m_Instance.enabled = false;
        
    }
}
