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
    public Text NowLoginState;
    public InputField EnterID;
    public InputField EnterPassword;
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

    public void PasswordToStar()
    {
        int CountPassword = EnterPassword.text.Length;
        if (CountPassword >= 15)
        {
            //WarningCount.SetActive(true);
        }

        StarPassword.text = MaskArray[CountPassword];

    }

    public void ClickLogin()
    {
        StartCoroutine(LoginToDB(EnterID.text, EnterPassword.text));
    }

    public void AdminLogin()
    {
        AccessToLauncher.SetActive(true);
    }

    IEnumerator LoginToDB(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("passwordPost", password);
        WWW www = new WWW(LoginURL, form);

        yield return www;

      // Debug.Log("Last Login Date is " + www.text.Substring(0, www.text.Length - 3));

        if (www.text == "PE")
        {
            Debug.Log("Password is Error");
           NowLoginState.text = "Password is Error";
        }

        else if (www.text == "NF")
        {
            Debug.Log("User Not Found");
           NowLoginState.text = "User Not Found";
        }

        else
        {
            Debug.Log("Login OK-> " + www.text.Substring(www.text.Length - 1, 1));
          NowLoginState.text = "LoginOK";

            PlayerPrefs.SetString("UserID", username);
            PlayerPrefs.SetString("UserLastLogin", www.text.Substring(0, www.text.Length - 3));
            PlayerPrefs.SetString("UserLevel", www.text.Substring(www.text.Length - 1, 1));

            Application.LoadLevel("Waiting");
        }

        StopCoroutine(LoginToDB(EnterID.text, EnterPassword.text));
    }
}


