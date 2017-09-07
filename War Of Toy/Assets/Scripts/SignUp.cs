using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignUp : MonoBehaviour
{
    string SignUpURL = "http://www.unitykds.tk/insert_user.php";

    public GameObject SignUpWindow;
    public GameObject SignUpFinish;
    public GameObject UsingIDWindow;
    public InputField NewID;
    public InputField NewPassword;
    public InputField NewEmail;

    public void ClickSignUp()
    {
        StartCoroutine(SignUpToDB());
    }
    IEnumerator SignUpToDB()
    {
        // DB연결 후
        // 아이디와 비밀번호 입력받고
        // 중복되는지 확인 후

        WWWForm Signform = new WWWForm();
        Signform.AddField("usernamePost", NewID.text);
        Signform.AddField("passwordPost", NewPassword.text);
        Signform.AddField("emailPost", NewEmail.text);

        WWW www = new WWW(SignUpURL, Signform);

        yield return www;

        if (www.text == "IDIsAlreadyExist")
        {
            Debug.Log("ID is already Exist");
            UsingIDWindow.SetActive(true);
        }

        else if (www.text == "EmailIsAlreadyExist")
        {
            Debug.Log("Email Is Already Exist");
        }
        else
        {
            Debug.Log("Sign Up Complete");
            StopCoroutine(SignUpToDB());
            SignUpFinish.SetActive(true);
        }

    }

    public void ShowSignUpWindow()
    {
        SignUpWindow.SetActive(true);
    }

    public void CloseSignUpWindow()
    {
        SignUpWindow.SetActive(false);
    }

    public void CloseSignUpFinish()
    {
        SignUpFinish.SetActive(false);
        SignUpWindow.SetActive(false);
    }

    public void CloseUsingIDWindow()
    {
        UsingIDWindow.SetActive(false);
    }

}
