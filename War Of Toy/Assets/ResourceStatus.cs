﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStatus : MonoBehaviour {

    public Transform Star_full;
    public Transform Star_little;
    public int m_gage;
    public bool m_Empty;

    void Awake () {
        m_gage = 0;
        StartCoroutine("StateOfStar");
	}

    
    IEnumerator StateOfStar()
    {
        while(true)
        {
            if (m_gage == 10)  // full->little
                Star_full.gameObject.SetActive(false);

            else if (m_gage == 20) // little->empty
            {
                m_Empty = true;
                Star_little.gameObject.SetActive(false);
                StopCoroutine("StateOfStar");
                //Destroy(gameObject);
            }
 
            yield return null;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }


}
