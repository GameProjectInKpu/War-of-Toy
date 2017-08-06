using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{

    string LoginURL = "http://www.unitykds.tk/Login.php";

    public InputField EnterID;
    public InputField EnterPassword;
    public GameObject AccessToLauncher;

    void Start()
    {

    }

    void Update()
    {


    }

    public void ClickLogin()
    {
        StartCoroutine(LoginToDB(EnterID.text, EnterPassword.text));
    }

    IEnumerator LoginToDB(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("passwordPost", password);

        WWW www = new WWW(LoginURL, form);

        yield return www;

        if (www.text == "1")
        {
            Debug.Log("if Login Success...! ");
            AccessToLauncher.SetActive(true);
            // Application.LoadLevel("next");
        }
        else
        {
            Debug.Log("Login Failed...! ");
        }

        StopCoroutine(LoginToDB(EnterID.text, EnterPassword.text));
    }
}
