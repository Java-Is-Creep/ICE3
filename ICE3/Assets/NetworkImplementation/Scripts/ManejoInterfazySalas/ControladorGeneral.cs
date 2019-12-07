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
    private Text numPlayers;
    [SerializeField]
    private InputField roomNameMobileCreate;
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
    [SerializeField]
    private GameObject botonTutorial;

    public Text createRoomPlaceholder;
    public Text joinRoomPlaceholder;

    [Header("Errores")]
    public Text textoErrorCrearSala;
    public Text textoErrorUnirseSala;
    public Text textoErrorBuscarSala;

    [Header("Front")]
    public Text connectingText;
    public Text buscarFront;
    public Text salirFront;
    public Text unirseFront;
    public Text crearFront;
    public GameObject buscarFrontButton;
    public GameObject unirseFrontButton;
    public GameObject crearFrontButton;
    public Text modoSeleccionado;
    public Image imagePersonaje;
    public Image movimiento;
    public Image disparo;
    public Sprite p0;
    public Sprite p1;
    public Sprite p2;
    public Sprite p3;
    public Sprite p4;
    public Sprite p5;
    public Sprite p6;
    public Sprite movimientoPC;
    public Sprite disparoPC;
    public Sprite movimientoMobile;
    public Sprite disparoMobile;

    [Header ("Bot")]
    public Text titleBot;
    public Text insertarNombreSalaHolderBot;
    public Text insertarNombreSalaHolderMobileBot;
    public Text numJugadoresBot;
    public Text botonCrearBot;
    public Text botonInicioBot;

    [Header("Left")]
    public Text botonInicioLeft;
    public Text botonUnirseLeft;
    public Text titleLeft;

    [Header("Right")]
    public Text botonInicioRight;
    public Text titleRight;
    public Text insertarNombreSalaHolderRight;
    public Text insertarNombreSalaHolderMobileRight;
    public Text botonUnirseRight;

    //tutorial
    bool tutorial;
    float timeError;

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
        timeError = 0;
        // Idioma 0 espanol
        if (PlayerPrefs.GetInt("Idioma") == 0)
        {
            // Front
            connectingText.text = "Conectando...";
            buscarFront.text = "Buscar";
            crearFront.text = "Crear";
            salirFront.text = "Salir";
            unirseFront.text = "Unirse";
            if (PlayerPrefs.GetInt ("Modo") == 1)
            {
                modoSeleccionado.text = "Modo 1";
            }
            else
            {
                modoSeleccionado.text = "Modo 2";
            }
            

            // Bot
            titleBot.text = "Crear Sala";
            insertarNombreSalaHolderBot.text = "Inserta Nombre Sala";
            insertarNombreSalaHolderMobileBot.text = "Inserta Nombre Sala";
            numJugadoresBot.text = "Número De Jugadores";
            botonCrearBot.text = "Crear";
            botonInicioBot.text = "Lobby";

            // Left
            botonInicioLeft.text = "Lobby";
            botonUnirseLeft.text = "Unirse";
            titleLeft.text = "Buscar Sala";

            // Right
            botonInicioRight.text = "Lobby";
            botonUnirseRight.text = "Unirse";
            insertarNombreSalaHolderRight.text = "Inserta Nombre Sala";
            insertarNombreSalaHolderMobileRight.text = "Inserta Nombre Sala";
            titleRight.text = "Unirse a Sala";
        }
        else
        {
            // Front
            connectingText.text = "Connecting...";
            buscarFront.text = "Search";
            crearFront.text = "Create";
            salirFront.text = "Exit";
            unirseFront.text = "Join";
            if (PlayerPrefs.GetInt("Modo") == 1)
            {
                modoSeleccionado.text = "Mode 1";
            }
            else
            {
                modoSeleccionado.text = "Mode 2";
            }

            // Bot
            titleBot.text = "Create Room";
            insertarNombreSalaHolderBot.text = "Insert Room Name";
            insertarNombreSalaHolderMobileBot.text = "Insert Room Name";
            numJugadoresBot.text = "Number of Players";
            botonCrearBot.text = "Create";
            botonInicioBot.text = "Lobby";

            // Left
            botonInicioLeft.text = "Lobby";
            botonUnirseLeft.text = "Search";
            titleLeft.text = "Search Room";

            // Right
            botonInicioRight.text = "Lobby";
            botonUnirseRight.text = "Join";
            insertarNombreSalaHolderRight.text = "Insert Room Name";
            insertarNombreSalaHolderMobileRight.text = "Insert Room Name";
            titleRight.text = "Join Room";
        }

        // Pantalla Inicial personaje
        switch (PlayerPrefs.GetInt("IndiceEscenario"))
        {
            case 0:
                imagePersonaje.sprite = p0;
                break;
            case 1:
                imagePersonaje.sprite = p1;
                break;
            case 2:
                imagePersonaje.sprite = p2;
                break;
            case 3:
                imagePersonaje.sprite = p3;
                break;
            case 4:
                imagePersonaje.sprite = p4;
                break;
            case 5:
                imagePersonaje.sprite = p5;
                break;
            case 6:
                imagePersonaje.sprite = p6;
                break;
        }

        tutorial = false;
        salir.SetActive(false);
        buscarFrontButton.SetActive(false);
        crearFrontButton.SetActive(false);
        unirseFrontButton.SetActive(false);
        if (Application.isMobilePlatform)
        {
            // Crear
            roomNameMobileCreate.gameObject.SetActive(true);
            roomNameCreate.gameObject.SetActive(false);
            //numPlayers.gameObject.SetActive(false);

            // Unirse
            roomNameMobileJoin.gameObject.SetActive(true);
            roomNameJoin.gameObject.SetActive(false);

            movimiento.sprite = movimientoMobile;
            disparo.sprite = disparoMobile;
        }
        else
        {
            // crear
            roomNameMobileCreate.gameObject.SetActive(false);
            //numPlayersMobile.gameObject.SetActive(false);
            roomNameCreate.gameObject.SetActive(true);
            //numPlayers.gameObject.SetActive(true);
            // unirse
            roomNameMobileJoin.gameObject.SetActive(false);
            roomNameJoin.gameObject.SetActive(true);

            movimiento.sprite = movimientoPC;
            disparo.sprite = disparoPC;
        }

        // si no estoy conectado me conecto y espero ya la llamada de on coneccted to master
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.NickName = PlayerPrefs.GetString("Name");
            createRoomButton.SetActive(false);
            joinRoomButton.SetActive(false);
            joinRandomRoonButton.SetActive(false);
            botonTutorial.SetActive(false);
            return;
        }
        else
        {
            Debug.Log("estoy conectado");
        }

        // si estoy conectdo pero no al lobby, me conecto y espero la llamada on joined lobby
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            createRoomButton.SetActive(false);
            joinRoomButton.SetActive(false);
            joinRandomRoonButton.SetActive(false);
            botonTutorial.SetActive(false);
            return;
        }
        else
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
            botonTutorial.SetActive(true);
            buscarFrontButton.SetActive(true);
            crearFrontButton.SetActive(true);
            unirseFrontButton.SetActive(true);
            connectingText.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeError += Time.deltaTime;
        if (timeError > 3)
        {
            textoErrorBuscarSala.text = "";
            textoErrorUnirseSala.text = "";
            textoErrorCrearSala.text = "";
        }
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
            connectingText.gameObject.SetActive(false);
        }
        if (Application.isMobilePlatform)
        {
            createRoomPlaceholder.text = roomNameMobileCreate.text;
            joinRoomPlaceholder.text = roomNameMobileJoin.text;
        }
    }

    #region tutorial

    public void irAlTutorial()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 1 });
        tutorial = true;
    }

    #endregion

    #region Llamadas Utiles
    /// <summary>
    /// Usando los input text se crea la sala con los datos,
    /// De momento no se hacen comprobaciones en los textos
    /// </summary>
    public void createRoomMethod()
    {
        if (roomNameMobileCreate.text != "" || roomNameCreate.text != "")
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

                    roomOptions.MaxPlayers = byte.Parse(numPlayers.text);
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
            }
            else
            {
                Debug.Log("Error");
            }
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
            }
            else
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
            botonTutorial.SetActive(true);
            connectingText.gameObject.SetActive(false);

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
        buscarFrontButton.SetActive(true);
        crearFrontButton.SetActive(true);
        unirseFrontButton.SetActive(true);
        botonTutorial.SetActive(true);
        connectingText.gameObject.SetActive(false);
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
        if (PlayerPrefs.GetInt("Idioma") == 0)
        {
            textoErrorCrearSala.text = "Nombre de sala ya cogido";
        }
        else
        {
            textoErrorCrearSala.text = "Room name already taken";
        }
        timeError = 0;
    }

    /// <summary>
    /// Se llama cuando se ha creado la sala satisfactoriamente
    /// </summary>
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Sala creada");
        if (tutorial)
        {
            ExitGames.Client.Photon.Hashtable tutorial = new ExitGames.Client.Photon.Hashtable();
            tutorial.Add("tutorial", true);
            PhotonNetwork.CurrentRoom.SetCustomProperties(tutorial);
            PhotonNetwork.LoadLevel("Tutorial");
        }
    }


    /// <summary>
    /// Se llama si no te puedes unir a la sala, no deberia llamarse, ya que si nos deja crear sala habrá espacio
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        if (PlayerPrefs.GetInt("Idioma") == 0)
        {
            textoErrorBuscarSala.text = "No se encontró sala";
        }
        else
        {
            textoErrorBuscarSala.text = "Room not found";
        }
        timeError = 0;
        Debug.Log("No se pudo unir a la sala, no existe o esta llena");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (PlayerPrefs.GetInt("Idioma") == 0)
        {
            textoErrorUnirseSala.text = "No se pudo unir a la sala";
        }
        else
        {
            textoErrorUnirseSala.text = "Room not found";
        }
        timeError = 0;
    }

    /// <summary>
    /// Un vez unidos a la sala, pasamos a lapantalla de waiting room para esperar al resto de jugadores.
    /// </summary>
    public override void OnJoinedRoom()
    {
        if (tutorial)
        {
            return;
        }
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

    public void aumentarNum()
    {
        int num = int.Parse(numPlayers.text) + 1;
        if (num != 7)
        {
            //numPlayersMobile.text = "" + num;
            numPlayers.text = "" + num;
        }
    }

    public void disminuirNum()
    {
        int num = int.Parse(numPlayers.text) - 1;
        if (num != 1)
        {
            //numPlayersMobile.text = "" + num;
            numPlayers.text = "" + num;
        }
    }
}
