using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.SceneManagement;
using System.Collections;
using System;


namespace Com.MyCompany.MyGame
{
    public class GameManager : Photon.PunBehaviour
    {
        static public GameManager Instance;

        public GameObject AlphaMid;
        public GameObject AlphaZero;
        public GameObject AlphaZeroBuilding;

        public RenderTexture RT_MidRed;
        public RenderTexture RT_ZeroRed;
        public RenderTexture RT_ZeroBuildingRed;

        public RenderTexture RT_MidBlue;
        public RenderTexture RT_ZeroBlue;
        public RenderTexture RT_ZeroBuildingBlue;

        public GameObject Fog;

        public GameObject playerPrefabRed;
        public GameObject playerPrefabBlue;

        GameObject PlayerRed;
        GameObject PlayerBlue;


    

        private void Awake()
        {
            if(PhotonNetwork.isMasterClient)
            {
                Fog.GetComponent<Renderer>().material.SetTexture("_MainTex", RT_ZeroRed);
                Fog.GetComponent<Renderer>().material.SetTexture("_MainTex2", RT_MidRed);
                Fog.GetComponent<Renderer>().material.SetTexture("_MainTex3", RT_ZeroBuildingRed);

                AlphaMid.GetComponent<Camera>().cullingMask = LayerMask.GetMask("FOWUnit");//29;   // FOWUnit
                AlphaMid.GetComponent<Camera>().targetTexture = RT_MidRed;

                AlphaZero.GetComponent<Camera>().cullingMask = LayerMask.GetMask("FOWUnit");//29;   // FOWUnit
                AlphaZero.GetComponent<Camera>().targetTexture = RT_ZeroRed;

                AlphaZeroBuilding.GetComponent<Camera>().cullingMask = LayerMask.GetMask("FOWBuilding");//17;   // FOWBuilding
                AlphaZeroBuilding.GetComponent<Camera>().targetTexture = RT_ZeroBuildingRed;
               
            }

            else
            {
                Fog.GetComponent<Renderer>().material.SetTexture("_MainTex", RT_ZeroBlue);
                Fog.GetComponent<Renderer>().material.SetTexture("_MainTex2", RT_MidBlue);
                Fog.GetComponent<Renderer>().material.SetTexture("_MainTex3", RT_ZeroBuildingBlue);

                AlphaMid.GetComponent<Camera>().cullingMask = LayerMask.GetMask("FOWUnitBlue");//16;   // FOWUnitBlue
                AlphaMid.GetComponent<Camera>().targetTexture = RT_MidBlue;

                AlphaZero.GetComponent<Camera>().cullingMask = LayerMask.GetMask("FOWUnitBlue");//16;   // FOWUnitBlue
                AlphaZero.GetComponent<Camera>().targetTexture = RT_ZeroBlue;

                AlphaZeroBuilding.GetComponent<Camera>().cullingMask = LayerMask.GetMask("FOWBuildingBlue");//15;   // FOWBuildingBlue
                AlphaZeroBuilding.GetComponent<Camera>().targetTexture = RT_ZeroBuildingBlue;
               
            }
        }



        // Use this for initialization
        void Start()
        {
            Instance = this;

            if (playerPrefabRed == null || playerPrefabBlue == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                // if (PlayerManager.LocalPlayerInstance == null)
                // {
                Debug.Log("We are Instantiating LocalPlayer from " + Application.loadedLevelName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate

                if (PhotonNetwork.isMasterClient)
                {
                    //Destroy(PlayerRed);
                    PlayerRed =  PhotonNetwork.Instantiate(this.playerPrefabRed.name, new Vector3(75, 5, 15), Quaternion.identity, 0);
                    PlayerMove unitRed = PlayerRed.GetComponent<PlayerMove>();
                    SelectUnitScript.m_Instance.LivingRedUnit.Add(unitRed);
                    ++CurUnitNum.m_Instance.m_UnitNum;
                    //SelectUnitScript.m_Instance.LivingEnemyUnit.Add(PlayerBlue.GetComponent<PlayerMove>());
                }

                else
                {
                    //Destroy(PlayerBlue);
                    PlayerBlue =  PhotonNetwork.Instantiate(this.playerPrefabBlue.name, new Vector3(25, 5, 85), Quaternion.identity, 0);
                    PlayerMove unitBlue = PlayerBlue.GetComponent<PlayerMove>();
                    SelectUnitScript.m_Instance.LivingBlueUnit.Add(unitBlue);
                    ++CurUnitNum.m_Instance.m_UnitNum;
                    //SelectUnitScript.m_Instance.LivingEnemyUnit.Add(PlayerRed.GetComponent<PlayerMove>());
                }


                //SelectUnitScript.m_Instance.LivingRedUnit.Add(PlayerRed.GetComponent<PlayerMove>());
                //SelectUnitScript.m_Instance.LivingBlueUnit.Add(PlayerBlue.GetComponent<PlayerMove>());

                // }
                //  else
                //  {
                //      Debug.Log("Ignoring scene load for " + Application.loadedLevelName);
                //  }
            }

        }

        //public void AddUnit()
        //{
        //    if (PhotonNetwork.isMasterClient)
        //    {
        //        //Destroy(PlayerRed);
        //        PlayerRed = PhotonNetwork.Instantiate(this.playerPrefabRed.name, new Vector3(75, 5, 15), Quaternion.identity, 0);
        //        PlayerMove unitRed = PlayerRed.GetComponent<PlayerMove>();
        //        //SelectUnitScript.m_Instance.LivingRedUnit.Add(unitRed);
        //        ++CurUnitNum.m_Instance.m_UnitNum;
        //        //SelectUnitScript.m_Instance.LivingEnemyUnit.Add(PlayerBlue.GetComponent<PlayerMove>());
        //    }

        //    else
        //    {
        //        //Destroy(PlayerBlue);
        //        PlayerBlue = PhotonNetwork.Instantiate(this.playerPrefabBlue.name, new Vector3(25, 5, 85), Quaternion.identity, 0);
        //        PlayerMove unitBlue = PlayerBlue.GetComponent<PlayerMove>();
        //        //SelectUnitScript.m_Instance.LivingBlueUnit.Add(unitBlue);
        //        ++CurUnitNum.m_Instance.m_UnitNum;
        //        //SelectUnitScript.m_Instance.LivingEnemyUnit.Add(PlayerRed.GetComponent<PlayerMove>());
        //    }
        //}

        void OnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }
        }

        #region Photon Messages

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        #endregion

        #region Private Methods

        void LoadArena()
        {
            if (!PhotonNetwork.isMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.Log("PhotonNetwork : Loading Level : " + PhotonNetwork.room.PlayerCount);
            // PhotonNetwork.LoadLevel("Mechanic");
        }

        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region Photon Messages


        public override void OnPhotonPlayerConnected(PhotonPlayer other)
        {
            Debug.Log("OnPhotonPlayerConnected() " + other.NickName); // not seen if you're the player connecting

            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected

                LoadArena();
            }
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
        {
            Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName); // seen when other disconnects

            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected

                LoadArena();
            }
        }

        #endregion
    }
    // Update is called once per frame


}