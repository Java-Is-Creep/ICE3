using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class joinRoom : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private InputField roomName;
    [SerializeField]
    private InputField roomNameMobile;
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

    // Start is called before the first frame update
    void Start()
    {
        if (Application.isMobilePlatform)
        {
            Debug.Log("Movil");
            roomNameMobile.gameObject.SetActive(true);
            roomName.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("no movil");
            roomNameMobile.gameObject.SetActive(false);
            roomName.gameObject.SetActive(true);
        }

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            joinRoomButton.SetActive(false);
            return;
        }

        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            joinRoomButton.SetActive(false);
            return;
        }

        if (PhotonNetwork.IsConnected && PhotonNetwork.InLobby)
        {
            joinRoomButton.SetActive(true);
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

            PhotonNetwork.JoinRoom(roomName.text);
            //PhotonNetwork.JoinRoom(roomName.text);

        }
        else
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
        }
        else
        {
            joinRoomButton.SetActive(true);
        }
    }

    /// <summary>
    /// Una vez cnectados al lobby, habilitamos el boton para crear salas
    /// </summary>
    public override void OnJoinedLobby()
    {
        Debug.Log("Unido al lobby");
        joinRoomButton.SetActive(true);
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
    /// Se llama si no te puedes unir a la sala, no deberia llamarse, ya que si nos deja crear sala habrá espacio
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No se pudo unir a la sala, debido a: " + message);
    }

    /// <summary>
    /// Un vez unidos a la sala, pasamos a lapantalla de waiting room para esperar al resto de jugadores.
    /// </summary>
    public override void OnJoinedRoom()
    {
        Debug.Log("Nos hemos unido a la sala");
        // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
        // Cambiamos de escena cuando halla 2 players
        Debug.Log(PhotonNetwork.CurrentRoom.Name);

        Debug.Log("We load the Waiting Room ");


            // #Critical
            // Load the Room Level.
            //PhotonNetwork.LoadLevel("WaitingRoom");
        
    }

    #endregion

}
