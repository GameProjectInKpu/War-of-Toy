using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFuncScript : MonoBehaviour {

    public GameObject m_ButtonRight;
    public GameObject m_ButtonLeft;

    public GameObject m_ButtonPick;    // 이동명령버튼
    public GameObject m_ButtonMineral;     // 자원캐기버튼
    public GameObject m_ButtonAttack;    // 적군 공격버튼
    public GameObject m_ButtonBoard;     // 공중 유닛 탑승버튼
    public GameObject m_ButtonDrop;      // 열기구에서 유닛 내려주기

    public bool ButtonRight;
    public bool ButtonLeft;
    public bool IsAirUnitfull;
    public int CurUnit;

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
        int start, end = 0;
        if (ButtonRight)
            start = 4;
            
        else
        {
            start = 0;
            end = 4;
        }

        CurUnit = type;

        switch (type)
        {
            case 0:     // 군인
                ClearFunc();
                m_ButtonPick.SetActive(true);
                m_ButtonAttack.SetActive(true);
                m_ButtonBoard.SetActive(true);
                break;

            case 2:     // 레고
                ClearFunc();
                m_ButtonPick.SetActive(true);
                m_ButtonMineral.SetActive(true);
                m_ButtonBoard.SetActive(true);
                m_ButtonRight.SetActive(true);
                m_ButtonLeft.SetActive(true);
                if (start == 4) end = m_BuildSlotList.Length;
                for (int i = start; i < end; ++i)
                    m_BuildSlotList[i].gameObject.SetActive(true);
                break;

            case 4:     // 곰탱
                ClearFunc();
                m_ButtonPick.SetActive(true);
                m_ButtonAttack.SetActive(true);
                m_ButtonBoard.SetActive(true);
                break;

            case 6:     // 열기구
                ClearFunc();
                if(IsAirUnitfull)
                {
                    m_ButtonPick.SetActive(true);
                    m_ButtonDrop.SetActive(true);
                }
                for (int i = start; i < m_AttackSlotList.Length; ++i)
                    m_AttackSlotList[i].gameObject.SetActive(true);
                break;

            case 8:     // 쥐
                ClearFunc();
                m_ButtonPick.SetActive(true);
                break;

            case 10:    // 공장
                ClearFunc();
                m_ButtonRight.SetActive(true);
                m_ButtonLeft.SetActive(true);
                if (start == 4) end = m_InitUnitSlotList.Length;
                for (int i = start; i < end; ++i)
                {
                    if (m_InitUnitSlotList[i].gameObject.tag == "B_ToyFactory")
                        m_InitUnitSlotList[i].gameObject.SetActive(true);
                }
                break;

            case 12:    // 중심건물
                ClearFunc();
                for (int i = start; i < m_InitUnitSlotList.Length; ++i)
                {
                    if (m_InitUnitSlotList[i].gameObject.tag == "B_ToyCastle")
                        m_InitUnitSlotList[i].gameObject.SetActive(true);
                }
                break;
            case 20:    // 공룡
                ClearFunc();
                m_ButtonPick.SetActive(true);
                m_ButtonAttack.SetActive(true);
                m_ButtonBoard.SetActive(true);
                break;
            case 22:    // 카
                ClearFunc();
                m_ButtonPick.SetActive(true);
                m_ButtonAttack.SetActive(true);
                break;

            default:
                ClearFunc();
                break;
        }
       
        
    }

    public void ClearFunc()
    {
        m_ButtonRight.SetActive(false);
        m_ButtonLeft.SetActive(false);
        m_ButtonPick.SetActive(false);
        m_ButtonMineral.SetActive(false);
        m_ButtonAttack.SetActive(false);
        m_ButtonBoard.SetActive(false);

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

    public void ButtonRightIsPressed(bool Right)
    {
        ButtonRight = Right;
        ButtonLeft = false;
        SetUnitFunc(CurUnit);
    }

    public void ButtonLeftIsPressed(bool Left)
    {
        ButtonLeft = Left;
        ButtonRight = false;
        SetUnitFunc(CurUnit);
    }
}
