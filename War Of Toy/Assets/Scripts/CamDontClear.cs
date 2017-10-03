using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamDontClear : MonoBehaviour
{

    public Camera cam;

    // Use this for initialization
    void Awake()
    {

        if (cam == null)
            cam = this.GetComponent<Camera>();

        Initialize();

    }

    public void Initialize()
    {
        cam.clearFlags = CameraClearFlags.Color;
    }

    private void OnPostRender()
    {
        cam.clearFlags = CameraClearFlags.Nothing;
    }

}
