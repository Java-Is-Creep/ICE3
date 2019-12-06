using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaitingManager : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private Text textoInformativo;

    public bool startDirectly;

    void OnApplicationQuit()
    {
        disconect();
    }
    // Start is called before the first frame update
    void Start()
    {
        textoInformativo.text = PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
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
        if(PlayerPrefs.GetInt("Modo") == 1)
        {
            PhotonNetwork.LoadLevel("MapScene");
        } else
        {
            PhotonNetwork.LoadLevel("MapScene2");
        }

    }

   public  void disconect()
    {
        PhotonNetwork.LeaveRoom();
        //PhotonNetwork.LoadLevel("Launcher");
    }

    #region Photon Callbacks

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("Launcher");
    }

    public override void OnPlayerEnteredRoom(Player other)
    {

        Debug.Log("Ha enntrado el cabron de: " + other.NickName);

        textoInformativo.text = PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

        Debug.Log(PhotonNetwork.CurrentRoom.Name);

        if (PhotonNetwork.IsMasterClient)
        {

            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                goToGameScene();
            }



        }

    }

    public override void OnPlayerLeftRoom(Player other)
    {
        textoInformativo.text = PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

    }

    

    #endregion



}
