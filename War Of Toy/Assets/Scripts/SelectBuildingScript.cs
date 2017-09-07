//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.AI;
//using UnityEngine;
//using UnityEngine.UI;

//public class SelectBuildingScript : MonoBehaviour
//{

//    public LayerMask m_LMUnit;
//    public LayerMask m_LMBuilding;
//    public GameObject m_BuildOK;
//    public bool m_IsSelect;
//    BuildingStatus CurBuilding;

//    void Start()
//    {

//    }

//    void Awake()
//    {
//        StartCoroutine("SelectRoutine");
//    }

//    IEnumerator SelectRoutine()
//    {
//        while (true)
//        {
//            if (Input.GetMouseButtonDown(0))    // 빌딩 선택
//            //if (Input.touchCount == 1 && m_IsSelect == false)
//            {
//                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //(Input.GetTouch(0).position); //
//                RaycastHit hit;
//                if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LMBuilding))
//                {
//                    CurBuilding = hit.transform.GetComponent<BuildingStatus>();

                    

//                    m_IsSelect = true;
//                    CurBuilding.m_IsSelect = true;
//                    if (CurBuilding.m_Team.gameObject.layer == 23)
//                        UnitStatusScript.m_Instance.SetUnitImage(hit.transform, 0);
//                    else
//                        UnitStatusScript.m_Instance.SetUnitImage(hit.transform, 1);

//                    CurBuilding.imgSelectbar.enabled = true;
                    
//                }

//                if (TouchScript.m_Instance.IsOver
//                    && !Physics.Raycast(ray, out hit, Mathf.Infinity, m_LMUnit)
//                    && !Physics.Raycast(ray, out hit, Mathf.Infinity, m_LMBuilding)
//                    && m_BuildOK.activeSelf == false)
//                {
//                    AllSelectOff();
//                }
//            }

//            yield return null;
//        }
//    }

    

//    private void AllSelectOff()
//    {
//        Debug.Log("체크해제 호출됨");
//        UnitFuncScript.m_Instance.ClearFunc();
        
//        m_IsSelect = false;
//        CurBuilding.m_IsSelect = false;
//        CurBuilding.imgSelectbar.enabled = false;

//    }

   
//}
