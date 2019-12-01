using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class createRoom : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private InputField roomName;
    [SerializeField]
    private InputField numPlayers;
    [SerializeField]
    private InputField roomNameMobile;
    [SerializeField]
    private InputField numPlayersMobile;
    [SerializeField]
    private GameObject createRoomButton;

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
        if (Application.isMobilePlatform)
        {
            Debug.Log("Movil");
            roomNameMobile.gameObject.SetActive(true);
            numPlayersMobile.gameObject.SetActive(true);
            roomName.gameObject.SetActive(false);
            numPlayers.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("no movil");
            roomNameMobile.gameObject.SetActive(false);
            numPlayersMobile.gameObject.SetActive(false);
            roomName.gameObject.SetActive(true);
            numPlayers.gameObject.SetActive(true);
        }
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            createRoomButton.SetActive(false);
            return;
        }

        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            createRoomButton.SetActive(false);
            return;
        }

        if (PhotonNetwork.IsConnected && PhotonNetwork.InLobby)
        {
            createRoomButton.SetActive(true);
        }
        
    }

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
                PhotonNetwork.CreateRoom(roomNameMobile.text, new RoomOptions { MaxPlayers = byte.Parse(numPlayersMobile.text) });
            } else
            {
                PhotonNetwork.CreateRoom(roomName.text, new RoomOptions { MaxPlayers = byte.Parse(numPlayers.text) });
            }
            //PhotonNetwork.JoinRoom(roomName.text);

        } else
        {
            Debug.Log("Error");
        }
      
    }


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
        } else
        {
            createRoomButton.SetActive(true);
            
        }
    }

    /// <summary>
    /// Una vez cnectados al lobby, habilitamos el boton para crear salas
    /// </summary>
    public override void OnJoinedLobby()
    {
        Debug.Log("Unido al lobby");
        createRoomButton.SetActive(true);
        /*
        if (Application.isMobilePlatform)
        {
            Debug.Log("Movil");
            roomNameMobile.gameObject.SetActive(true);
            numPlayersMobile.gameObject.SetActive(true);
            roomName.gameObject.SetActive(false);
            numPlayers.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("no movil");
            roomNameMobile.gameObject.SetActive(false);
            numPlayersMobile.gameObject.SetActive(false);
            roomName.gameObject.SetActive(true);
            numPlayers.gameObject.SetActive(true);
        }*/
    }

    /// <summary>
    /// Abandonamos el lobby y la sala
    /// </summary>
    /// <param name="cause"></param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        //// progressLabel.SetActive(false);
        // controlPanel.SetActive(true);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        Debug.LogWarningFormat("Desconectado del lobby", cause);
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
        Debug.Log("No se pudo unir a la sala");
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
            PhotonNetwork.LoadLevel("WaitingRoom");
        }
    }


    #endregion

}
