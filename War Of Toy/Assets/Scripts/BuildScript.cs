using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.AI;

public class BuildScript : MonoBehaviour
{
    public GameObject m_Building;
    public GameObject m_Plane;
    public GameObject m_BuildOK;
    public GameObject m_BuildNO;

    GameObject Building;
    public static GameObject BuildingTemp;
    GameObject Plane;

    public static Vector3 BuildPos;
    Vector3 PlanePos;
    Vector3 OKPos;
    Vector3 NOPos;
    Vector3 OKPosZ;
    Vector3 NOPosZ;


    public bool m_CanBuild = false;
    public bool m_IsClickBuilding = false;
    public static bool m_IsBuild = false;
    //public static bool m_IsFinish = false;

    public LayerMask m_LayerMaskBuild;
    public LayerMask m_LayerMaskGround;
    public LayerMask m_UI;

    private Transform m_Camera;
    private MoveCamera m_CameraMove;
    //private BuildPlaneScript m_PlaneInfo;


    private void Awake()
    {
        m_Camera = Camera.main.transform;
        m_CameraMove = m_Camera.GetComponent<MoveCamera>();
        
    }

    public void Build()
    {
        if (m_BuildOK.activeSelf == true)
            return;

        BuildPos = m_Camera.position;
        PlanePos = m_Camera.position;

        if ( (BuildPos.x > 60f && BuildPos.z < 25f)  || (BuildPos.x < 35f && BuildPos.z > 73f) )
            BuildPos.y = 4.5f;
        else
            BuildPos.y = 0.1f;
        PlanePos.y = BuildPos.y + 0.1f;

        Building = (GameObject)PhotonNetwork.Instantiate(m_Building.name, BuildPos, Quaternion.Euler(Vector3.zero), 0);
        Plane = (GameObject)PhotonNetwork.Instantiate(m_Plane.name, PlanePos, Quaternion.Euler(Vector3.zero), 0);


        m_BuildOK.SetActive(true);
        m_BuildNO.SetActive(true);

        OKPos = Vector3.zero;
        NOPos = Vector3.zero;

        OKPos.x = BuildPos.x - 5f;
        OKPos.z = BuildPos.z + 10f;

        NOPos.x = BuildPos.x + 5f;
        NOPos.z = BuildPos.z + 10f;

        OKPosZ = Camera.main.WorldToScreenPoint(OKPos);
        NOPosZ = Camera.main.WorldToScreenPoint(NOPos);
        OKPosZ.z = 0f;
        NOPosZ.z = 0f;

        m_BuildOK.transform.position = OKPosZ;
        m_BuildNO.transform.position = NOPosZ;

        Plane.transform.SetParent(Building.transform, false);
        Plane.transform.localPosition = Vector3.zero;

        BuildPlaneScript m_PlaneInfo =  Plane.GetComponent<BuildPlaneScript>();

        Building.AddComponent<NavMeshObstacle>();
        Building.GetComponent<NavMeshObstacle>().carving = true;
        Vector3 RebakeSize = Building.GetComponent<NavMeshObstacle>().size;
        Vector3 Scale = transform.localScale;
        switch (m_Building.tag)
        {
            case "B_Zenga":
                Scale.x *= 0.5f;
                Scale.z *= 0.5f;
                break;
            case "B_ToyFactory":
                Scale.z *= 0.6f;
                RebakeSize.x *= 6f;
                RebakeSize.z *= 3f;
                FactoryScript FactoryFunc = Building.GetComponent<FactoryScript>();
                FactoryFunc.enabled = false;
                break;
            case "B_CupCake":
                Scale.x *= 0.5f;
                Scale.z *= 0.5f;
                break;
            default:
                break;

        }
        m_PlaneInfo.transform.localScale = Scale;
        Building.GetComponent<NavMeshObstacle>().size = RebakeSize;
        Building.GetComponent<NavMeshObstacle>().enabled = false;
        StartCoroutine("CheckingCanBuild");
    }


    IEnumerator CheckingCanBuild()
    {
        while (true)
        {
            if (Input.GetMouseButton(0))    // 버튼이 눌러지는 동안
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LayerMaskBuild))// && !Physics.Raycast(ray, out hit, Mathf.Infinity, m_UI))
                {
                    Debug.Log("BuildingPick");
                    m_IsClickBuilding = true;
                    m_CameraMove.enabled = false;
                }
            }

            else
            {
                m_IsClickBuilding = false;
                m_CameraMove.enabled = true;
            }
            



            if (m_IsClickBuilding == true)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    // 스크린에서 월드방향으로 
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LayerMaskGround))
                {
                    Debug.Log("GroundPick");
                    BuildPos = Building.transform.position;
                    OKPos = Vector3.zero; 
                    NOPos = Vector3.zero;
                    PlanePos = BuildPos;
                    PlanePos.y = BuildPos.y + 0.1f;

                    BuildPos = hit.point;

                    OKPos.x = BuildPos.x - 5f;
                    OKPos.z = BuildPos.z + 10f;
                    NOPos.x = BuildPos.x + 5f;
                    NOPos.z = BuildPos.z + 10f;                

                    OKPos.x = (int)OKPos.x;
                    OKPos.z = (int)OKPos.z;
                    OKPos.x -= (int)OKPos.x % 5;
                    OKPos.z -= (int)OKPos.z % 5;

                    NOPos.x = (int)NOPos.x;
                    NOPos.z = (int)NOPos.z;
                    NOPos.x -= (int)NOPos.x % 5;
                    NOPos.z -= (int)NOPos.z % 5;

                    BuildPos.x = (int)BuildPos.x;
                    BuildPos.z = (int)BuildPos.z;
                    BuildPos.x -= (int)BuildPos.x % 5;
                    BuildPos.z -= (int)BuildPos.z % 5;

                    Plane.transform.position = PlanePos;
                    Building.transform.position = BuildPos;

                    OKPosZ = Camera.main.WorldToScreenPoint(OKPos);
                    NOPosZ = Camera.main.WorldToScreenPoint(NOPos);
                    OKPosZ.z = 0f;
                    NOPosZ.z = 0f;

                    m_BuildOK.transform.position = OKPosZ;
                    m_BuildNO.transform.position = NOPosZ;
                }
            }

            else
            {
                OKPos.x = BuildPos.x - 5f;
                OKPos.z = BuildPos.z + 10f;
                NOPos.x = BuildPos.x + 5f;
                NOPos.z = BuildPos.z + 10f;

                OKPos.x = (int)OKPos.x;
                OKPos.z = (int)OKPos.z;
                OKPos.x -= (int)OKPos.x % 5;
                OKPos.z -= (int)OKPos.z % 5;

                NOPos.x = (int)NOPos.x;
                NOPos.z = (int)NOPos.z;
                NOPos.x -= (int)NOPos.x % 5;
                NOPos.z -= (int)NOPos.z % 5;


                OKPosZ = Camera.main.WorldToScreenPoint(OKPos);
                NOPosZ = Camera.main.WorldToScreenPoint(NOPos);
                OKPosZ.z = 0f;
                NOPosZ.z = 0f;

                m_BuildOK.transform.position = OKPosZ;
                m_BuildNO.transform.position = NOPosZ;

                PlanePos = Vector3.zero;
                PlanePos.y = 0.1f;
                Plane.transform.localPosition = PlanePos;
            }



            if (Plane.GetComponent<Renderer>().material.color == Color.blue)   
                m_CanBuild = true;
            else
                m_CanBuild = false;
            
            yield return null;
        }
    }



    public void SelectbyButton(bool IsBuild)
    {
        if (IsBuild)
        {
            if (!m_CanBuild)
                return;
            if (StarScript.m_Instance.m_StarNum - 50 < 0)
                return;
            StarScript.m_Instance.BuildByStar(50);
            //Building.GetComponent<NavMeshObstacle>().enabled = false;
            BuildingTemp = Building;
            Building = null;
        }

        else
            Destroy(Building);
            
        Destroy(Plane);
        if(m_Building.tag == "B_ToyFactory")
        {
            FactoryScript FactoryFunc = BuildingTemp.GetComponent<FactoryScript>();
            FactoryFunc.enabled = true;
        }

        m_IsBuild = IsBuild;
        m_BuildOK.SetActive(false);
        m_BuildNO.SetActive(false);
        StopCoroutine("CheckingCanBuild");
        //m_IsBuild = false;
        m_CanBuild = false;
    }

    private void Update()
    {

    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

}

