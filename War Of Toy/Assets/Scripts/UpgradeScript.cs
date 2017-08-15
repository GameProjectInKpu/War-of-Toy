using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void UpgradeByButton(string unitTag)
    {
        bool found = false;
        for (int i = 0; i < SelectUnitScript.m_Instance.LivingUnit.Count; ++i)
        {
            if (SelectUnitScript.m_Instance.IsUnitMyTeam(SelectUnitScript.m_Instance.LivingUnit[i]) 
                && SelectUnitScript.m_Instance.LivingUnit[i].gameObject.tag == unitTag)
            {
                if (SelectUnitScript.m_Instance.LivingUnit[i].m_IsUpgraded)
                {
                    NoticeScript.m_Instance.Notice("이미 강화된 종족입니다\n");
                    return;
                }
                found = true;
                SelectUnitScript.m_Instance.LivingUnit[i].Upgrade();
            }
        }
        if(!found)
            NoticeScript.m_Instance.Notice("아직 존재하지 않습니다\n");
        return;
    }
}
