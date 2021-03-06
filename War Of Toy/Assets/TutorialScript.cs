﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TutorialScript : MonoBehaviour {

    public GameObject[] Videos;
    public GameObject TutorialCanvas;
    public GameObject BackButton;
    public GameObject SkipButton;

    int CurVideo;

	// Use this for initialization
	void Awake () {
        CurVideo = 0;
        BackButton.SetActive(false);
        Videos[CurVideo].SetActive(true);
        for(int i = CurVideo + 1; i < Videos.Length ; ++i)
        {
            Videos[i].SetActive(false);
        }
        TutorialCanvas.SetActive(true);

    }

    public void SkipByButton()
    {
        if (CurVideo == Videos.Length - 1)
        {
            TutorialCanvas.SetActive(false);
            return;
        }
            

        ++CurVideo;
        Videos[CurVideo].SetActive(true);
        for (int i = 0; i < Videos.Length; ++i)
        {
            if (i == CurVideo)
                continue;
            Videos[i].SetActive(false);
        }
        //if (CurVideo == Videos.Length - 1)
        //    SkipButton.SetActive(false);

        BackButton.SetActive(true);
    }

    public void BackByButton()
    {
        //if (CurVideo == 0)
        //    return;

        --CurVideo;
        Videos[CurVideo].SetActive(true);
        for (int i = 0; i < Videos.Length; ++i)
        {
            if (i == CurVideo)
                continue;
            Videos[i].SetActive(false);
        }
        if (CurVideo == 0)
            BackButton.SetActive(false);

        SkipButton.SetActive(true);
    }

    public void TutorialByButton()
    {
        CurVideo = 0;
        BackButton.SetActive(false);
        Videos[CurVideo].SetActive(true);
        for (int i = CurVideo + 1; i < Videos.Length ; ++i)
        {
            Videos[i].SetActive(false);
        }
        TutorialCanvas.SetActive(true);
    }
}
