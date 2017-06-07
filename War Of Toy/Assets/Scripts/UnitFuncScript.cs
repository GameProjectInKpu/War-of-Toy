using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFuncScript : MonoBehaviour {

    static public UnitFuncScript m_Instance;
    static public UnitFuncScript Instance
    {
        get { return m_Instance; }
    }

    public BuildScript[] m_BuildSlotList;   // 건물 생성 버튼
    public InitUnitScript[] m_InitUnitSlotList; // 유닛 생성 버튼
    public AttackByBomb[] m_AttackSlotList;   // 공격 버튼
    //public int[] m_TypeList;
    

    void Awake()
    {
        m_Instance = this;
        m_BuildSlotList = GetComponentsInChildren<BuildScript>();    // BuildScript 가지고 있는 자식들
        m_InitUnitSlotList = GetComponentsInChildren<InitUnitScript>();    // InitUnitScript 가지고 있는 자식들
        m_AttackSlotList = GetComponentsInChildren<AttackByBomb>();
        ClearFunc();

      
    }


    public void SetUnitFunc(int type)
    {
        if(type == 2)   // 레고
        {
            ClearFunc();
            for (int i = 0; i < m_BuildSlotList.Length; ++i)
                m_BuildSlotList[i].gameObject.SetActive(true);
        }

        else if(type == 6)  // 열기구
        {
            ClearFunc();
            for (int i = 0; i < m_AttackSlotList.Length; ++i)
                m_AttackSlotList[i].gameObject.SetActive(true);
        }

        else if(type == 10) // 공장
        {
            ClearFunc();
            for (int i = 0; i < m_InitUnitSlotList.Length; ++i)
            {
                m_InitUnitSlotList[i].gameObject.SetActive(true);                
            }
                
        }



        else
        {
            ClearFunc();
        }
        
    }

    public void ClearFunc()
    {
        for (int i = 0; i < m_BuildSlotList.Length; ++i)
            m_BuildSlotList[i].gameObject.SetActive(false);

        for (int i = 0; i < m_InitUnitSlotList.Length; ++i)
            m_InitUnitSlotList[i].gameObject.SetActive(false);

        for (int i = 0; i < m_AttackSlotList.Length; ++i)
            m_AttackSlotList[i].gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        m_Instance = null;
    }

   
}
