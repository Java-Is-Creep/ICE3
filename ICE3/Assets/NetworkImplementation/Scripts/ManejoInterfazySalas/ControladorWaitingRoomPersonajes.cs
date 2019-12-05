using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ControladorWaitingRoomPersonajes : MonoBehaviourPunCallbacks 
{
    Player[] jugadores;
    // Start is called before the first frame update
    void Start()
    {
        jugadores = PhotonNetwork.PlayerList;
        ExitGames.Client.Photon.Hashtable character = new ExitGames.Client.Photon.Hashtable();
        character.Add("skin", PlayerPrefs.GetInt("IndiceEscenario"));
        PhotonNetwork.LocalPlayer.SetCustomProperties(character);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("ALGUIEN SE FUE");
        jugadores = PhotonNetwork.PlayerList;
        foreach(Player play in jugadores)
        {
            object indice;
            play.CustomProperties.TryGetValue("skin", out indice);
            if (indice != null)
            {
                int index = (int)indice;
                Debug.Log("Jugador: " + play.NickName + " lleva la skin de: " + index);
            }

        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("ALGUIEN ENTRA");
        jugadores = PhotonNetwork.PlayerList;
        foreach (Player play in jugadores)
        {
            object indice;
            play.CustomProperties.TryGetValue("skin", out indice);
            if(indice != null)
            {
                int index = (int)indice;
                Debug.Log("Jugador: " + play.NickName + " lleva la skin de: " + index);
            }

        }
    }






}
