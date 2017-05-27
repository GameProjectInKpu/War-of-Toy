using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryScript : MonoBehaviour {

    public Text m_BatteryText;
    public int m_BatteryNum;

    static public BatteryScript m_Instance;
    static public BatteryScript Instance
    {
        get { return m_Instance; }
    }

    void Awake()
    {
        m_Instance = this;

    }

    IEnumerator AmountBatteryRoutine()
    {
        while (true)
        {
            m_BatteryText.text = "Batterty " + m_BatteryNum + "";

            yield return null;
        }

    }


    void Start()
    {
        m_BatteryNum = 0;
        m_BatteryText = GetComponent<Text>();
        StartCoroutine("AmountBatteryRoutine");
    }

    void OnDestroy()
    {
        m_Instance = null;
        StopAllCoroutines();
    }


}
