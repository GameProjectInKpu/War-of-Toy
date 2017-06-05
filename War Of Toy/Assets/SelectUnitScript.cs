using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;

public class SelectUnitScript : MonoBehaviour {

    public LayerMask m_LMUnit;
    public GameObject m_BuildOK;
    public bool m_IsSelect;
    static public List<PlayerMove> PM;


    void Start () {
		
	}

    void Awake()
    {
        PM = new List<PlayerMove>();

        StartCoroutine("SelectRoutine");
    }

    IEnumerator SelectRoutine()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))    // 유닛 선택
            //if (Input.touchCount == 1 && m_IsSelect == false)
            {
                
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //(Input.GetTouch(0).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LMUnit))
                {
                    PlayerMove Pm = hit.transform.GetComponent<PlayerMove>();
                    if (Pm.m_IsBuild) goto CannotOrder;
                    if (Pm.m_IsSelect) goto AlreadyAdd; //if (Pm.m_IsAlive) goto AlreadyAdd;

                    //Pm.m_IsAlive = true;
                    PM.Add(Pm);

                    AlreadyAdd:;
                    m_IsSelect = true;
                    Pm.m_IsSelect = true;
                    if (Pm.m_Team.gameObject.layer == 23)
                        UnitStatusScript.m_Instance.SetUnitImage(hit, 0);
                    else
                        UnitStatusScript.m_Instance.SetUnitImage(hit, 1);

                    Pm.imgSelectbar.enabled = true;
                    Pm.m_Animator.SetBool("IsPick", false);

                    Pm.StopCoroutine("Picking");
                    Pm.StopCoroutine("OrderRoutine");
                    Pm.StartCoroutine("OrderRoutine");
                    CannotOrder:;
                }

                if (TouchScript.IsOver
                    && !Physics.Raycast(ray, out hit, Mathf.Infinity, m_LMUnit)
                    && m_BuildOK.activeSelf == false) 
                {
                    AllSelectOff();
                }

            }


            yield return null;
        }
    }

    IEnumerator IsAliveRoutine()
    {
        while(true)
        {
            for (int i = 0; i < PM.Count; i++) 
            {
                if(PM[i].m_IsAlive == false)
                    PM[i].Invoke("Death", 3f);
            }

            yield return null;
        }
    }

    private void AllSelectOff()
    {
        Debug.Log("체크해제 호출됨");
        UnitFuncScript.m_Instance.ClearFunc();
        foreach (PlayerMove unit in PM)
        {
            m_IsSelect = false;
            unit.m_IsSelect = false;
            unit.imgSelectbar.enabled = false;
        }

    }
}
