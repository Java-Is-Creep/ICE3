using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingManager : MonoBehaviourPunCallbacks
{

    public int minNumPlayers;

    public bool startDirectly;

    void OnApplicationQuit()
    {
        disconect();
    }
    // Start is called before the first frame update
    void Start()
    {
 
        if (startDirectly)
        {
            goToGameScene();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void goToGameScene()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log(PhotonNetwork.CurrentRoom.Name);
            Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.Log("Loading game scene");
        PhotonNetwork.LoadLevel("MapScene");
    }


   public  void disconect()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Launcher");
    }

    #region Photon Callbacks

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

        Debug.Log(PhotonNetwork.CurrentRoom.Name);

        if (PhotonNetwork.IsMasterClient)
        {

            if (PhotonNetwork.CurrentRoom.PlayerCount == minNumPlayers)
            {

                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                goToGameScene();
            }



        }

    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

    }

    

    #endregion



}
