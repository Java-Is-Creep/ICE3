﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class QuickStartRoomController : MonoBehaviourPunCallbacks
{
    public int multiplayerSceneIndex;



    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("No shemos unido a la sala");
        StartGame();
    }

    private void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log(" somos el manda mas");
            PhotonNetwork.LoadLevel(multiplayerSceneIndex);
        }

    }

}
