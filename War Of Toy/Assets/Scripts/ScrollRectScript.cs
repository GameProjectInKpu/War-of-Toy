using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectScript : MonoBehaviour {

    public ScrollRect ScrollRect;
    public bool ButtonRight;
    public bool ButtonLeft;

    // Use this for initialization
    void Start () {
        ScrollRect = GetComponent<ScrollRect>();
	}
	
	// Update is called once per frame
	void Update () {
        if (ButtonRight)
            ScrollRect.horizontalNormalizedPosition += 10f;
        else if (ButtonLeft)
            ScrollRect.horizontalNormalizedPosition -= 10f;	
	}

    public void ButtonRightIsPressed(bool Right)
    {
        ButtonRight = Right;
    }

    public void ButtonLeftIsPressed(bool Left)
    {
        ButtonLeft = Left;
    }


}
