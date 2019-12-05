using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameOverManejador : MonoBehaviourPunCallbacks
{
    private int numPlayers;
    private int numPlayersRepeat;
    private int actualNumPlayers;
    // Start is called before the first frame update
    void Start()
    {
        numPlayers = PhotonNetwork.CurrentRoom.PlayerCount;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void volverAjugar()
    {
        this.photonView.RPC("MandarDecision", RpcTarget.All,true);
    }

    public void salirse()
    {
        this.photonView.RPC("MandarDecision", RpcTarget.All, false);

    }

    [PunRPC]
    void MandarDecision(bool decision)
    {
        Debug.Log("RPC recibida");
        actualNumPlayers++;
        
        if (decision)
        {
            numPlayersRepeat++;
        } else
        {

        }

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Soy el server y la rcp es mia");
            if (actualNumPlayers == numPlayers)
            {
                if (numPlayersRepeat == numPlayers)
                {
                    if (PlayerPrefs.GetInt("Modo") == 1)
                    {
                        PhotonNetwork.LoadLevel("MapScene");
                    }
                    else
                    {
                        PhotonNetwork.LoadLevel("MapScene2");
                    }
                } else
                {
                    PhotonNetwork.CurrentRoom.IsVisible = true;
                    PhotonNetwork.CurrentRoom.IsOpen = true;
                    PhotonNetwork.LoadLevel("WaitingRoom");
                }
            }
        }
    }


}
