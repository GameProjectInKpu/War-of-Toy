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
    public Image UnitImage;
    public Text  UnitActionText;

    void Awake()
    {
        m_Instance = this;
        UnitImage = GetComponent<Image>();

    }

    public void SetUnitImage(RaycastHit hit, int color)
    {
        switch (hit.transform.tag)
        {
            case "UnitSoldier":
                UnitImage.sprite = m_UnitSpriteList[0+ color];
                UnitFuncScript.m_Instance.SetUnitFunc(0);
                //UnitImage.color = Color.red;
                break;
            case "UnitLego":
                UnitImage.sprite = m_UnitSpriteList[2+ color];
                UnitFuncScript.m_Instance.SetUnitFunc(2);
                break;
            case "UnitBear":
                UnitImage.sprite = m_UnitSpriteList[4+ color];
                UnitFuncScript.m_Instance.SetUnitFunc(4);
                break;
            case "UnitAirballoon":
                AttackByBomb.CurBalloon = hit.collider.gameObject.transform;
                UnitImage.sprite = m_UnitSpriteList[6 + color];
                UnitFuncScript.m_Instance.SetUnitFunc(6);
                break;
            case "UnitClockmouse":
                UnitImage.sprite = m_UnitSpriteList[2 + color];
                UnitFuncScript.m_Instance.SetUnitFunc(8);
                break;
            case "B_ToyFactory":
                UnitImage.sprite = m_UnitSpriteList[10+ color];
                UnitFuncScript.m_Instance.SetUnitFunc(10);
                break;
        }
    }



    void OnDestroy()
    {
        m_Instance = null;
    }

}
