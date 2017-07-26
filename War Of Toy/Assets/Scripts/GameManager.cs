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
        
        public GameObject playerPrefabRed;
        public GameObject playerPrefabBlue;

        GameObject PlayerRed;
        GameObject PlayerBlue;

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
                    PlayerRed =  PhotonNetwork.Instantiate(this.playerPrefabRed.name, new Vector3(75, 5, 15), Quaternion.identity, 0);
                    PlayerMove Pm = PlayerRed.GetComponent<PlayerMove>();
                    //Pm.m_SerialNum = (int)UnityEngine.Random.Range(0, 500);
                    SelectUnitScript.PM.Add(Pm);
                }

                else
                {
                    PlayerBlue =  PhotonNetwork.Instantiate(this.playerPrefabBlue.name, new Vector3(25, 5, 85), Quaternion.identity, 0);
                    PlayerMove Pm = PlayerBlue.GetComponent<PlayerMove>();
                    //Pm.m_SerialNum = (int)UnityEngine.Random.Range(0, 500);
                    SelectUnitScript.PM.Add(Pm);
                }

                // }
                //  else
                //  {
                //      Debug.Log("Ignoring scene load for " + Application.loadedLevelName);
                //  }
            }

        }

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