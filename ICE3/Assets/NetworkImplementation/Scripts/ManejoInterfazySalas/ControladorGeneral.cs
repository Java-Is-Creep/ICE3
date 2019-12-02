﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControladorGeneral : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private InputField roomNameCreate;
    [SerializeField]
    private InputField numPlayers;
    [SerializeField]
    private InputField roomNameMobileCreate;
    [SerializeField]
    private InputField numPlayersMobile;
    [SerializeField]
    private GameObject createRoomButton;
    [SerializeField]
    private InputField roomNameJoin;
    [SerializeField]
    private InputField roomNameMobileJoin;
    [SerializeField]
    private GameObject joinRoomButton;



    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Haciendo init");
        if (Application.isMobilePlatform)
        {
            Debug.Log("Movil");
            // Crear
            roomNameMobileCreate.gameObject.SetActive(true);
            numPlayersMobile.gameObject.SetActive(true);
            roomNameCreate.gameObject.SetActive(false);
            numPlayers.gameObject.SetActive(false);

            // Unirse
            roomNameMobileJoin.gameObject.SetActive(true);
            roomNameJoin.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("no movil");
            // crear
            roomNameMobileCreate.gameObject.SetActive(false);
            numPlayersMobile.gameObject.SetActive(false);
            roomNameCreate.gameObject.SetActive(true);
            numPlayers.gameObject.SetActive(true);
            // unirse
            roomNameMobileJoin.gameObject.SetActive(false);
            roomNameJoin.gameObject.SetActive(true);
        }
        // si no estoy conectado me conecto y espero ya la llamada de on coneccted to master
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("No estoy conectado");
            PhotonNetwork.ConnectUsingSettings();
            createRoomButton.SetActive(false);
            joinRoomButton.SetActive(false);
            return;
        }

        // si estoy conectdo pero no al lobby, me conecto y espero la llamada on joined lobby
        if (!PhotonNetwork.InLobby)
        {
            Debug.Log("No estoy en el lobby");
            PhotonNetwork.JoinLobby();
            createRoomButton.SetActive(false);
            joinRoomButton.SetActive(false);
            return;
        }

        // si todo esta hecho pues activo botones
        if (PhotonNetwork.IsConnected && PhotonNetwork.InLobby)
        {
            Debug.Log("Estamos conectados y lobby ");
            createRoomButton.SetActive(true);
            joinRoomButton.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("Haciendo update");
        //Debug.Log("¿Estoy conectado? " + PhotonNetwork.IsConnected);
        //Debug.Log("Estoy en el lobby?" + PhotonNetwork.InLobby);
        if (PhotonNetwork.IsConnected && PhotonNetwork.InLobby)
        {
            //Debug.Log("Estamos conectados ");
            createRoomButton.SetActive(true);
            joinRoomButton.SetActive(true);
        }
    }

    #region Llamadas Utiles
    /// <summary>
    /// Usando los input text se crea la sala con los datos,
    /// De momento no se hacen comprobaciones en los textos
    /// </summary>
    public void createRoomMethod()
    {

        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Estamos conectados");
            //if(int.Parse(numPlayers.text) != int)
            if (Application.isMobilePlatform)
            {
                PhotonNetwork.CreateRoom(roomNameMobileCreate.text, new RoomOptions { MaxPlayers = byte.Parse(numPlayersMobile.text) });
            }
            else
            {
                PhotonNetwork.CreateRoom(roomNameCreate.text, new RoomOptions { MaxPlayers = byte.Parse(numPlayers.text) });
            }
            //PhotonNetwork.JoinRoom(roomName.text);

        }
        else
        {
            Debug.Log("Error");
        }

    }

    /// <summary>
    /// Nos unimos a la sala con el nombre especificado
    /// </summary>
    public void joinRoomMethod()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Estamos conectados");
            //if(int.Parse(numPlayers.text) != int)

            PhotonNetwork.JoinRoom(roomNameJoin.text);
            //PhotonNetwork.JoinRoom(roomName.text);

        }
        else
        {
            Debug.Log("Error");
        }
    }
    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    /// <summary>
    /// Nos conectamos al server e intentamos conectarnnos al lobby
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado al master");
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        else
        {
            createRoomButton.SetActive(true);
            joinRoomButton.SetActive(true);

        }
    }

    /// <summary>
    /// Una vez cnectados al lobby, habilitamos el boton para crear salas
    /// </summary>
    public override void OnJoinedLobby()
    {
        Debug.Log("Unido al lobby");
        createRoomButton.SetActive(true);
        joinRoomButton.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cause"></param>
    public override void OnDisconnected(DisconnectCause cause)
    {

    }

    /// <summary>
    /// Se llama si no se ha podido crear la sala, normalmente porque el nombr eestará cogido.
    /// Necesitaria notificacion
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("NOMBRE DE SALA YA COGIDO");
    }

    /// <summary>
    /// Se llama cuando se ha creado la sala satisfactoriamente
    /// </summary>
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Sala creada");
    }


    /// <summary>
    /// Se llama si no te puedes unir a la sala, no deberia llamarse, ya que si nos deja crear sala habrá espacio
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No se pudo unir a la sala, no existe o esta llena");
    }



    /// <summary>
    /// Un vez unidos a la sala, pasamos a lapantalla de waiting room para esperar al resto de jugadores.
    /// </summary>
    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
        // Cambiamos de escena cuando halla 2 players
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {

            Debug.Log("We load the Waiting Room ");


            // #Critical
            // Load the Room Level.
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("WaitingRoom");
            }

        }
    }


    #endregion


}