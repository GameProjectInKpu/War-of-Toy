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
        if (PhotonNetwork.isMasterClient)
        {
            for (int i = 0; i < SelectUnitScript.m_Instance.LivingRedUnit.Count; ++i)
            {
                if (SelectUnitScript.m_Instance.IsUnitMyTeam(SelectUnitScript.m_Instance.LivingRedUnit[i])
                    && SelectUnitScript.m_Instance.LivingRedUnit[i].gameObject.tag == unitTag)
                {
                    if (SelectUnitScript.m_Instance.LivingRedUnit[i].m_IsUpgraded)
                    {
                        NoticeScript.m_Instance.Notice("이미 강화된 종족입니다\n");
                        return;
                    }
                    found = true;
                    SelectUnitScript.m_Instance.LivingRedUnit[i].Upgrade();
                }
            }
        }

        else
        {
            for (int i = 0; i < SelectUnitScript.m_Instance.LivingBlueUnit.Count; ++i)
            {
                if (SelectUnitScript.m_Instance.IsUnitMyTeam(SelectUnitScript.m_Instance.LivingBlueUnit[i])
                    && SelectUnitScript.m_Instance.LivingBlueUnit[i].gameObject.tag == unitTag)
                {
                    if (SelectUnitScript.m_Instance.LivingBlueUnit[i].m_IsUpgraded)
                    {
                        NoticeScript.m_Instance.Notice("이미 강화된 종족입니다\n");
                        return;
                    }
                    found = true;
                    SelectUnitScript.m_Instance.LivingBlueUnit[i].Upgrade();
                }
            }
        }

        
        if(!found)
            NoticeScript.m_Instance.Notice("아직 존재하지 않습니다\n");
        return;
    }
}
