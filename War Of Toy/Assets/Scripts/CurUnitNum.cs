using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurUnitNum : MonoBehaviour {

    public Text m_UnitText;
    public int m_UnitNum;

    static public CurUnitNum m_Instance;
    static public CurUnitNum Instance
    {
        get { return m_Instance; }
    }

    void Awake()
    {
        m_Instance = this;

    }

    IEnumerator AmountUnitRoutine()
    {
        while (true)
        {
            m_UnitText.text = " " + m_UnitNum + "";

            yield return null;
        }

    }


    void Start()
    {
        m_UnitNum = 0;
        m_UnitText = GetComponent<Text>();
        StartCoroutine("AmountUnitRoutine");
    }

    void OnDestroy()
    {
        m_Instance = null;
        StopAllCoroutines();
    }

}
