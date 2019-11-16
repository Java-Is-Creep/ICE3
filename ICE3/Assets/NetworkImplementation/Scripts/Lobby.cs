using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Lobby : MonoBehaviourPunCallbacks
{


    public Button connectBtn;
    public Button joinRandomBtn;
    public Text Log;


    public byte maxPlayersInRoom = 4;
    public byte minPlayersInRoom = 2;

    public int playerCounter;
    public Text PlayerCounter;



    public void connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.ConnectUsingSettings())
            {
               Log.text += " \n Estamos conectados al server";
            } else
            {
                Log.text += " \n Ha habido un error";
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("We are now connected to the " + PhotonNetwork.CloudRegion + " server!");
        joinRandomBtn.interactable = true;
        connectBtn.interactable = false;

    }

    public void joinRandom()
    {
        if (!PhotonNetwork.JoinRandomRoom())
        {
            Log.text += "\n Fallo al unirse a la sala";
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Log.text += "\n No existen salas a las que unirse.. Creando una nueva";
        if (PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions() { MaxPlayers = maxPlayersInRoom }))
        {
            Log.text += "\n Sala creada con exito";
        } else
        {
            Log.text += "\n Fallo al crear la sala";
        }
    }



    public void FixedUpdate()
    {
        if(PhotonNetwork.CurrentRoom != null)
        {
            playerCounter = PhotonNetwork.CurrentRoom.PlayerCount;
        }
        PlayerCounter.text = playerCounter + "/" + maxPlayersInRoom;
    }







}
