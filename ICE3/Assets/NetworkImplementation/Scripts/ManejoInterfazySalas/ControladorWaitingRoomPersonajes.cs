using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ControladorWaitingRoomPersonajes : MonoBehaviourPunCallbacks 
{
    Player[] jugadores;
    [SerializeField]
    private Text texto;
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
        string nombres = " Tu eleccion es: " + PlayerPrefs.GetInt("IndiceEscenario");
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
                nombres.Insert(nombres.Length, "Jugador: " + play.NickName + " lleva la skin de: " + index + "\n");
            }

        }
        texto.text = nombres;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        string nombres = " Tu eleccion es: " + PlayerPrefs.GetInt("IndiceEscenario");
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
                nombres.Insert(nombres.Length, "Jugador: " + play.NickName + " lleva la skin de: " + index + "\n");
            }

            texto.text = nombres;

        }
    }






}
