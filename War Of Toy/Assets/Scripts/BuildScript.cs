using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.AI;

public class BuildScript : Photon.PunBehaviour
{
    public GameObject m_Building_red;
    public GameObject m_Building_blue;
    public GameObject m_Plane;
    public GameObject m_BuildOK;
    public GameObject m_BuildNO;
    GameObject CheckTag;

    public static GameObject Building;
    public static GameObject BuildingTemp;
    public GameObject Plane;

    public static Vector3 BuildPos;
    Vector3 PlanePos;
    Vector3 OKPos;
    Vector3 NOPos;
    Vector3 OKPosZ;
    Vector3 NOPosZ;

    public static Transform AttackArea;
    public bool IsButtonPressed;

    public bool m_CanBuild = false;
    public bool m_IsClickBuilding = false;
    public static bool m_IsBuild = false;
    //public static bool m_IsFinish = false;

    public LayerMask m_LayerMaskBuild;
    public LayerMask m_LayerMaskGround;

    private Transform m_Camera;
    private MoveCamera m_CameraMove;
    // public BuildPlaneScript m_PlaneInfo;

    public LayerMask m_LMInBase;
    //RaycastHit hit;

    private void Awake()
    {
        
        m_Camera = Camera.main.transform;
        m_CameraMove = m_Camera.GetComponent<MoveCamera>();
        
    }

    public void Build()
    {
        if (m_BuildOK.activeSelf == true)
            return;

        IsButtonPressed = false;
        BuildPos = m_Camera.position;
        PlanePos = m_Camera.position;
        BuildPos.y = 0.1f;
        BuildPos.z += 15f;
        
        if (Physics.Raycast(Camera.main.transform.position, BuildPos - Camera.main.transform.position, Mathf.Infinity, m_LMInBase))
            BuildPos.y = 4.5f;
        Debug.DrawRay(Camera.main.transform.position, BuildPos - Camera.main.transform.position, Color.red);
        
        PlanePos.y = BuildPos.y + 0.1f;

        if (PhotonNetwork.isMasterClient)
        {
            Building = PhotonNetwork.Instantiate(m_Building_red.name, BuildPos, Quaternion.Euler(Vector3.zero), 0);
            Plane = PhotonNetwork.Instantiate(m_Plane.name, PlanePos, Quaternion.Euler(Vector3.zero), 0);
        }
        else
        {
            Building = PhotonNetwork.Instantiate(m_Building_blue.name, BuildPos, Quaternion.Euler(Vector3.zero), 0);
            Plane = PhotonNetwork.Instantiate(m_Plane.name, PlanePos, Quaternion.Euler(Vector3.zero),0);
        }

        //BuildingTemp = Building;
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

        Vector3 Scale = transform.localScale;
        

        if (PhotonNetwork.isMasterClient)
        {
            CheckTag = m_Building_red;
        }
        else
        {
            CheckTag = m_Building_blue;
        }
        switch (CheckTag.tag)
        {
            case "B_Batterys":
                Scale.x *= 0.7f;
                Scale.z *= 0.7f;
                
                break;
            case "B_Zenga":
                Scale.x *= 0.5f;
                Scale.z *= 0.5f;
                AttackArea = Building.transform.Find("AttackArea");
                AttackArea.gameObject.SetActive(false);
                break;
            case "B_ToyFactory":
                Debug.Log("공장 기능 잠시 꺼둠");
                Scale.x *= 0.9f;
                Scale.z *= 0.6f;
                //FactoryScript FactoryFunc = Building.GetComponent<FactoryScript>();
                //FactoryFunc.enabled = false;
                break;
            case "B_CupCake":
                Scale.x *= 0.5f;
                Scale.z *= 0.5f;
                break;
            case "B_Lab":
                Scale.x *= 0.6f;
                Scale.z *= 0.6f;
                break;
            case "B_Hospital":
                Scale.x *= 0.7f;
                Scale.z *= 0.5f;
                break;
            default:
                break;

        }
        m_PlaneInfo.transform.localScale = Scale;
        Building.layer = 0;
        StartCoroutine("CheckingCanBuild");
    }


    IEnumerator CheckingCanBuild()
    {
        while (true)
        {
            
            Debug.DrawRay(Camera.main.transform.position, BuildPos - Camera.main.transform.position, Color.red);
            if (Input.GetMouseButton(0))    // 버튼이 눌러지는 동안
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LayerMaskBuild))
                {
                    m_IsClickBuilding = true;
                    m_CameraMove.enabled = false;
                }
            }

            else
            {
                m_IsClickBuilding = false;
                m_CameraMove.enabled = true;
            }
            



            if (m_IsClickBuilding == true && IsButtonPressed == false)
            {
                
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    // 스크린에서 월드방향으로 
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LayerMaskGround))
                {
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

                    if (Physics.Raycast(Camera.main.transform.position, BuildPos - Camera.main.transform.position, Mathf.Infinity, m_LMInBase))
                        BuildPos.y = 4.5f;
                    else
                        BuildPos.y = 0.1f;

                    BuildPos.x = Mathf.Clamp(BuildPos.x, 5f, 95f);
                    BuildPos.z = Mathf.Clamp(BuildPos.z, 5f, 95f);
                    BuildPos.y = Mathf.Clamp(BuildPos.y, 0f, 5f);

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

            if (Physics.Raycast(Camera.main.transform.position, BuildPos - Camera.main.transform.position, Mathf.Infinity, m_LMInBase))
                if (BuildPos.y == 0.1f)
                    m_CanBuild = false;



            yield return null;
        }
    }



    public void SelectbyButton(bool IsBuild)
    {
        //StopAllCoroutines();
        IsButtonPressed = true;
        if (Plane == null)
            return;

        if (IsBuild == true)
        {
            Debug.Log(m_CanBuild);
            if(!m_CanBuild )
            {
                NoticeScript.m_Instance.Notice("건물을 지을수 없는 구역입니다\n");
                IsButtonPressed = false;
                return;
            }
            if(StarScript.m_Instance.m_StarNum - 50 < 0)
            {
                NoticeScript.m_Instance.Notice("자원이 부족합니다\n");
                IsButtonPressed = false;
                return;
            }
                
            StarScript.m_Instance.BuildByStar(50);
            //Building.GetComponent<NavMeshObstacle>().enabled = false;
            BuildingTemp = Building;
            //Building = null;
            Destroy(Building);
            Destroy(Plane);
            if (PhotonNetwork.isMasterClient)
            {
                Building = (GameObject)PhotonNetwork.Instantiate(m_Building_red.name, BuildPos, Quaternion.Euler(Vector3.zero), 0);
            }
            else
            {
                Building = (GameObject)PhotonNetwork.Instantiate(m_Building_blue.name, BuildPos, Quaternion.Euler(Vector3.zero), 0);
            }
            Building.transform.position = BuildPos;
            Building.AddComponent<NavMeshObstacle>();
            Building.GetComponent<NavMeshObstacle>().carving = true;
            Vector3 RebakeSize = Building.GetComponent<NavMeshObstacle>().size;
            switch (CheckTag.tag)
            {
                case "B_Batterys":
                    RebakeSize.x *= 4f;
                    RebakeSize.z *= 4f;
                    break;
                case "B_Zenga":
                    AttackArea = Building.transform.Find("AttackArea");
                    AttackArea.gameObject.SetActive(false);
                    break;
                case "B_ToyFactory":
                    RebakeSize.x *= 8f;
                    RebakeSize.z *= 5f;
                    //FactoryScript FactoryFunc = Building.GetComponent<FactoryScript>();
                    //FactoryFunc.enabled = false;
                    break;
                case "B_CupCake":
                    SelectUnitScript.m_Instance.AcceptableUnit += 10;
                    RebakeSize.z *= 2f;
                    break;
                default:
                    break;

            }
            Building.GetComponent<NavMeshObstacle>().size = RebakeSize;
            BuildingTemp = Building;
            Building.layer = 27;
        }

        else
        {
            //StopAllCoroutines();
            Destroy(Building);
            Destroy(Plane);
            Plane = null;
        }
       

        m_IsBuild = IsBuild;
        m_BuildOK.SetActive(false);
        m_BuildNO.SetActive(false);
        IsButtonPressed = false;
        StopAllCoroutines();
        //m_IsBuild = false;
        m_CanBuild = false;
        m_CameraMove.enabled = true;
        return;
    }

    private void Update()
    {

    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

  

}

