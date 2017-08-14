using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelloToUser : MonoBehaviour
{
    public Text IntroInfo;
    public Text LineInfo;
    public string UserID;
    public string UserLevel;
    public string UserLastLogin;
    public GameObject WindowHello;
    public GameObject WindowUserInfo;
    public GameObject RandomMatch;
    public GameObject AccessToRandomMatch;
    
    void Start()
    {
        UserID = PlayerPrefs.GetString("UserID");
        UserLevel = PlayerPrefs.GetString("UserLevel");
        UserLastLogin = PlayerPrefs.GetString("UserLastLogin");
        ShowIntroInfo();
    }

    public void ShowIntroInfo()
    {
        IntroInfo.text = "Hello " + UserID + "!\nYour Level is " + UserLevel + "\nand Last Login is " + UserLastLogin + "\n";
    }

    public void ShowLineInfo()
    {
        LineInfo.text = UserID + " / " + UserLevel;
        WindowUserInfo.SetActive(true);
        RandomMatch.SetActive(true);

    }

    public void CloseWindowHello()
    {
        WindowHello.SetActive(false);
        ShowLineInfo();
    }

    public void StartLauncher()
    {
        AccessToRandomMatch.SetActive(true);
    }

}
