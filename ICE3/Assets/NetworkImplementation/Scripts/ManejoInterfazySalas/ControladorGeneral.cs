using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField]
    private GameObject joinRandomRoonButton;
    [SerializeField]
    private GameObject salir;


    public Text createRoomPlaceholder;
    public Text numPlayersPlaceholder;
    public Text joinRoomPlaceholder;

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
        salir.SetActive(false);
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
            PhotonNetwork.NickName = PlayerPrefs.GetString("Name");
            createRoomButton.SetActive(false);
            joinRoomButton.SetActive(false);
            joinRandomRoonButton.SetActive(false);
            return;
        } else
        {
            Debug.Log("estoy conectado");
        }

        // si estoy conectdo pero no al lobby, me conecto y espero la llamada on joined lobby
        if (!PhotonNetwork.InLobby)
        {
            Debug.Log("No estoy en lobby");
            PhotonNetwork.JoinLobby();
            createRoomButton.SetActive(false);
            joinRoomButton.SetActive(false);
            joinRandomRoonButton.SetActive(false);
            return;
        } else
        {
            Debug.Log("Estoy en lobby");
        }

        // si todo esta hecho pues activo botones
        if (PhotonNetwork.IsConnected && PhotonNetwork.InLobby)
        {
            salir.SetActive(true);
            Debug.Log("Estamos conectados y lobby ");
            createRoomButton.SetActive(true);
            joinRoomButton.SetActive(true);
            joinRandomRoonButton.SetActive(true);
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
            Debug.Log("Conectado y en lobby update");
            //Debug.Log("Estamos conectados ");
            createRoomButton.SetActive(true);
            joinRoomButton.SetActive(true);
            joinRandomRoonButton.SetActive(true);
        }
        if (Application.isMobilePlatform)
        {
            createRoomPlaceholder.text = roomNameMobileCreate.text;
            joinRoomPlaceholder.text = roomNameMobileJoin.text;
            numPlayersPlaceholder.text = numPlayersMobile.text;
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
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsOpen = true;
            roomOptions.IsVisible = true;
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "modo" };
            //if(int.Parse(numPlayers.text) != int)
            if (Application.isMobilePlatform)
            {
              
                roomOptions.MaxPlayers = byte.Parse(numPlayersMobile.text);
                if (PlayerPrefs.GetInt("Modo") == 1)
                {

                    roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "modo", 1 } };
                    PhotonNetwork.CreateRoom(roomNameMobileCreate.text, roomOptions);
                }
                else
                {
                    roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "modo", 2 } };
                    PhotonNetwork.CreateRoom(roomNameMobileCreate.text, roomOptions);
                }
                
            }
            else
            {
                roomOptions.MaxPlayers = byte.Parse(numPlayers.text);

                if (PlayerPrefs.GetInt("Modo") == 1)
                {
                    roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "modo", 1 } };
                    PhotonNetwork.CreateRoom(roomNameCreate.text, roomOptions);
                }
                else
                {
                    roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "modo", 2 } };
                    PhotonNetwork.CreateRoom(roomNameCreate.text, roomOptions);
                }

            }
            //PhotonNetwork.JoinRoom(roomName.text);

        }
        else
        {
            Debug.Log("Error");
        }

    }

    public void joinRandomRoomMethod()
    {
        if (PhotonNetwork.IsConnected)
        {
            if(PlayerPrefs.GetInt("Modo") == 1)
            {
                ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "modo", 1 } };
                PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties,0);
            } else
            {
                ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "modo", 2 } };
                PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
            }

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

            if (Application.isMobilePlatform)
            {
               PhotonNetwork.JoinRoom(roomNameMobileJoin.text);
                
                

            } else
            {
                PhotonNetwork.JoinRoom(roomNameJoin.text);
            }


            //PhotonNetwork.JoinRoom(roomName.text);

        }
        else
        {
            Debug.Log("Error");
        }
    }

    public void exitMenu()
    {
        SceneManager.LoadScene("MainMenu");
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
            joinRandomRoonButton.SetActive(true);

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
        joinRandomRoonButton.SetActive(true);
        salir.SetActive(true);
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
