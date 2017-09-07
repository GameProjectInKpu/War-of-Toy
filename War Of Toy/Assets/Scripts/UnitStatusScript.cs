using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UnitStatusScript : MonoBehaviour {

    static public UnitStatusScript m_Instance;

    static public UnitStatusScript Instance
    {
        get { return m_Instance; }
    }

    //public List<Sprite> m_SpriteList = new List<Sprite>();
    // 이미지 관리
    public List<Sprite> m_UnitSpriteList;
    public Image UnitHp;
    public Image CurHp;
    public Image UnitImage;
    //public Text  UnitActionText;

    public bool m_IsMyTeam;

    void Awake()
    {
        m_Instance = this;
        UnitImage = GetComponent<Image>();
        CurHp.enabled = false;

    }

    public void SetUnitImage(Transform unit, int color, Image hp)//(RaycastHit hit, int color)
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (m_IsMyTeam)
                color = 0;
            else
                color = 1;
        }
            
        else
        {
            if (m_IsMyTeam)
                color = 1;
            else
                color = 0;
        }

        CurHp.enabled = true;
        UnitHp = hp;
        StopCoroutine("HPRoutine");
        StartCoroutine("HPRoutine");


        switch (unit.tag)
        {
            case "UnitSoldier":
                UnitImage.sprite = m_UnitSpriteList[0+ color];
                if(m_IsMyTeam)
                    UnitFuncScript.m_Instance.SetUnitFunc(0);
                break;
            case "UnitLego":
                UnitImage.sprite = m_UnitSpriteList[2+ color];
                if (m_IsMyTeam)
                    UnitFuncScript.m_Instance.SetUnitFunc(2);
                break;
            case "UnitBear":
                UnitImage.sprite = m_UnitSpriteList[4+ color];
                if (m_IsMyTeam)
                    UnitFuncScript.m_Instance.SetUnitFunc(4);
                break;
            case "UnitAirballoon":
                AttackByBomb.CurBalloon = unit;// hit.collider.gameObject.transform;
                UnitImage.sprite = m_UnitSpriteList[6 + color];
                if (m_IsMyTeam)
                    UnitFuncScript.m_Instance.SetUnitFunc(6);
                break;
            case "UnitClockmouse":
                UnitImage.sprite = m_UnitSpriteList[8 + color];
                if (m_IsMyTeam)
                    UnitFuncScript.m_Instance.SetUnitFunc(8);
                break;
            case "B_ToyFactory":
                UnitImage.sprite = m_UnitSpriteList[10+ color];
                if (m_IsMyTeam)
                    UnitFuncScript.m_Instance.SetUnitFunc(10);
                break;
            case "B_ToyCastle":
                UnitImage.sprite = m_UnitSpriteList[12 + color];
                if (m_IsMyTeam)
                    UnitFuncScript.m_Instance.SetUnitFunc(12);
                break;
            case "B_Batterys":
                UnitImage.sprite = m_UnitSpriteList[14 + color];
                if (m_IsMyTeam)
                    UnitFuncScript.m_Instance.SetUnitFunc(14);
                break;
            case "B_CupCake":
                UnitImage.sprite = m_UnitSpriteList[16 + color];
                if (m_IsMyTeam)
                    UnitFuncScript.m_Instance.SetUnitFunc(16);
                break;
            case "B_Zenga":
                UnitImage.sprite = m_UnitSpriteList[18 + color];
                if (m_IsMyTeam)
                    UnitFuncScript.m_Instance.SetUnitFunc(18);
                break;
            case "UnitDinosaur":
                UnitImage.sprite = m_UnitSpriteList[20 + color];
                if (m_IsMyTeam)
                    UnitFuncScript.m_Instance.SetUnitFunc(20);
                break;
            case "UnitRCcar":
                Debug.Log("카 이미지 출력");
                UnitImage.sprite = m_UnitSpriteList[22 + color];
                if (m_IsMyTeam)
                    UnitFuncScript.m_Instance.SetUnitFunc(22);
                break;
            case "B_Lab":
                UnitImage.sprite = m_UnitSpriteList[24 + color];
                if (m_IsMyTeam)
                    UnitFuncScript.m_Instance.SetUnitFunc(24);
                break;
            case "B_Hospital":
                UnitImage.sprite = m_UnitSpriteList[26 + color];
                if (m_IsMyTeam)
                    UnitFuncScript.m_Instance.SetUnitFunc(26);
                break;
            case "UnitCupid":
                UnitImage.sprite = m_UnitSpriteList[28 + color];
                if (m_IsMyTeam)
                    UnitFuncScript.m_Instance.SetUnitFunc(28);
                break;
            default:
                break;
        }
    }

    IEnumerator HPRoutine()
    {
        while(true)
        {
            if (CurHp.fillAmount <= 0)
                StopCoroutine("HPRoutine");
            CurHp.fillAmount = UnitHp.fillAmount;
            //Debug.Log(CurHp.fillAmount);// = hp.fillAmount;
            yield return null;
        }
    }
    

    void OnDestroy()
    {
        StopAllCoroutines();
        m_Instance = null;
    }

}
