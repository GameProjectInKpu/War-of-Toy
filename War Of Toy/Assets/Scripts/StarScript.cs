using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarScript : MonoBehaviour
{
    public Text m_StarText; 
    public int m_StarNum;

    static public StarScript m_Instance;
    static public StarScript Instance
    {
        get { return m_Instance; }
    }

    void Awake()
    {
        m_Instance = this;

    }

    IEnumerator AmountStarRoutine()
    {
        while (true)
        {
            m_StarText.text = " " + m_StarNum + "";

            yield return null;
        }

    }

    public void BuildByStar(int cost)
    {
        m_StarNum -= cost;
    }

    public void AddStarByButton()
    {
        m_StarNum += 10;
    }

    void Start()
    {
        m_StarNum = 0;
        m_StarText = GetComponent<Text>();
        StartCoroutine("AmountStarRoutine");
    }

    void OnDestroy()
    {
        m_Instance = null;
        StopAllCoroutines();
    }


}
