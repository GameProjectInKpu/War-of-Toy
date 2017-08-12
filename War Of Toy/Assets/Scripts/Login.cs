using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    int CountPasswordChar = 0;
    static int MaxPassword = 15;

    string LoginURL = "http://www.unitykds.tk/Login.php";
    string[] MaskArray = new string[MaxPassword];

    public Text StarPassword;
    public InputField EnterID;
    public InputField EnterPassword;
    public GameObject WarningPasswordCount;
    public GameObject AccessToLauncher;

    private void Start()
    {
        MaskArray[0] = "";
        string mask = "";
        for (int count = 0; count <= MaxPassword - 1; count++)
        {
            if (count != 0)
            {
                MaskArray[count] = mask + "*";
                mask = mask + "*";
            }
        }
    }

    private void Update()
    {
        // 
    }

    public void PasswordToStar()
    {
        int CountPassword = EnterPassword.text.Length;
        if (CountPassword >= 15)
        {
            WarningPasswordCount.SetActive(true);
        }
        StarPassword.text = MaskArray[CountPassword];
        Debug.Log(EnterPassword.text);

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

        if (www.text == "LoginOK")
        {
            Debug.Log("Login OK");
            AccessToLauncher.SetActive(true);
        }

        else if (www.text == "PasswordError")
        {
            Debug.Log("Password is Error");
        }

        else if (www.text == "UserNotFound")
        {
            Debug.Log("User Not Found");
        }

        StopCoroutine(LoginToDB(EnterID.text, EnterPassword.text));
    }
}
