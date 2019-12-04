using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameOverManejador : MonoBehaviourPunCallbacks
{
    private int timeToChoose;
    public float actualTime;
    private int numPlayers;
    private int numPlayersRepeat;
    private int actualNumPlayers;
    private bool repetir;
    // Start is called before the first frame update
    void Start()
    {
        numPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        actualTime = 0;
        timeToChoose = 15;
        repetir = false;
    }

    // Update is called once per frame
    void Update()
    {
        actualTime += Time.deltaTime;
        if(actualTime >= timeToChoose)
        {
            if (!repetir)
            {
                Debug.Log("Mando RPC por tiempo");
                this.photonView.RPC("MandarDecision", RpcTarget.All, false);
            }

        }
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
            repetir = true;
        } else
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.LeaveRoom();
                SceneManager.LoadScene("MainMenu");
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if (actualNumPlayers == numPlayers)
            {
                if(numPlayersRepeat == numPlayers)
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
                    PhotonNetwork.LoadLevel("WaitingRoom");
                }
            }
        }
    }


}
