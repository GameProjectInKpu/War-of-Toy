using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeScript : MonoBehaviour {

    //public AudioClip soundNotice;
    AudioSource myAudio;

    public Text m_Message0;
    public Text m_Message1;
    public Text m_Message2;
    

    public float timer;
    public float timecheck;

    static public NoticeScript m_Instance;
    static public NoticeScript Instance
    {
        get { return m_Instance; }
    }

    void Awake()
    {
        m_Instance = this;

    }

    void Start () {

        m_Message0.text += " ";
        m_Message1.text += " ";
        m_Message2.text += " ";
        myAudio = GetComponent<AudioSource>();

    }

    public void Notice(string NewMsg)
    {



        m_Message0.text = m_Message1.text;
        m_Message1.text = m_Message2.text;
        m_Message2.text = NewMsg;

        timecheck = timer;



    }

    public void PlaySound(AudioClip soundNotice)
    {
        myAudio.PlayOneShot(soundNotice);
    }

    
	// Update is called once per frame
	void Update () {
        if(timer >= float.MaxValue)
            timer = 0f;

        timer += Time.deltaTime;

        if(timer - timecheck > 5f)
            m_Message0.text = " ";
        if (timer - timecheck > 8f)
            m_Message1.text = " ";
        if (timer - timecheck > 11f)
        {
            m_Message2.text = " ";
        }



    }

    void OnDestroy()
    {
        m_Instance = null;
    }

}
