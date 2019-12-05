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
    private bool refrescar;
    public float tiempoParaRecargar;
    public float actualTime;
    // Start is called before the first frame update
    void Start()
    {
        jugadores = PhotonNetwork.PlayerList;
        ExitGames.Client.Photon.Hashtable character = new ExitGames.Client.Photon.Hashtable();
        character.Add("skin", PlayerPrefs.GetInt("IndiceEscenario"));
        PhotonNetwork.LocalPlayer.SetCustomProperties(character);
        refrescar = false;
        tiempoParaRecargar = 0.5f;
        actualTime = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (refrescar)
        {
            actualTime += Time.deltaTime;
            if(actualTime > tiempoParaRecargar)
            {
                refrescar = false;
                actualTime = 0;
                refrescarNombres();
            }
        }
    }

    public void refrescarNombres()
    {
        string nombres = " Tu eleccion es: " + PlayerPrefs.GetInt("IndiceEscenario");
        jugadores = PhotonNetwork.PlayerList;
        foreach (Player play in jugadores)
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


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {

        Debug.Log("ALGUIEN SE FUE");
        refrescar = true;
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        Debug.Log("ALGUIEN ENTRA");
        refrescar = true;
      
    }






}
