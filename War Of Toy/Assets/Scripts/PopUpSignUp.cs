using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpSignUp : MonoBehaviour
{
    public GameObject PopUp;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ShowSignUpWindow()
    {
        PopUp.SetActive(true);
    }

    public void CloseSignUpWindow()
    {
        PopUp.SetActive(false);
    }

}
