using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{
    public Vector3 m_Pos;
    private float m_CameraSpeed = 7f;
    public Transform m_PlayerPos;
    public float m_Distance;
    public bool IsMove;


    static public MoveCamera m_Instance;
    static public MoveCamera Instance
    {
        get { return m_Instance; }
    }

    void Awake()
    {
        m_Instance = this;
        m_Pos = transform.position;
        m_Distance = 20f;
        StartCoroutine("MoveRoutine");
    }

    IEnumerator MoveRoutine()
    {
        while (true)
        {
            //m_Pos.x += m_CameraSpeed * Input.GetAxis("Mouse X") * Time.deltaTime * 10f;
            //m_Pos.z += m_CameraSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime * 10f;

            m_Pos.x += m_CameraSpeed * Input.GetAxis("Horizontal") * Time.deltaTime * 10f;
            m_Pos.z += m_CameraSpeed * Input.GetAxis("Vertical") * Time.deltaTime * 10f;
            m_Pos.y -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 300f;

            m_Pos.x = Mathf.Clamp(m_Pos.x, 25f, 1000f);
            m_Pos.z = Mathf.Clamp(m_Pos.z, 0f, 990f);
            m_Pos.y = Mathf.Clamp(m_Pos.y, 10f, 200f);

            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)     IsMove = false;
            else    IsMove = true;


            transform.position = m_Pos;


            yield return null;

        }
    }

    void OnDestroy()
    {
        m_Instance = null;
    }



}
