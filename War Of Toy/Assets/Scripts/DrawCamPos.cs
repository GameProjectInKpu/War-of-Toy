using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawCamPos : MonoBehaviour {

    public Text m_CamPosText;
    //public float m_Pos;


    IEnumerator DrawCamPosRoutine()
    {

        while (true)
        {

            m_CamPosText.text = "Pos " + MoveCamera.m_Instance.transform.position + "";
         
            yield return null;
        }

    }


    void Start()
    {

        m_CamPosText = GetComponent<Text>();
        StartCoroutine("DrawCamPosRoutine");
    }
}
