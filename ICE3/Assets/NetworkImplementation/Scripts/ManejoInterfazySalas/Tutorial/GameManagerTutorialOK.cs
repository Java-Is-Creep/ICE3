using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


public class GameManagerTutorialOK : MonoBehaviourPunCallbacks
{

    public static GameManagerTutorialOK instance;
    public ControladorTutorial controlador;

    [Tooltip("The prefab to use for representing the player")]
    public GameObject renoPlayer;
    public GameObject pinguinoPlayer;
    public GameObject morsaPlayer;
    public GameObject teapotPlayer;
    public GameObject osoPlayer;
    public GameObject zorraPlayer;
    public GameObject munecoPlayer;
    public GameObject cubePrefab;
    public Vector3 posIniTutorial;

    [HideInInspector]
    public GameObject musicMenu;

    void OnApplicationQuit()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Launcher");
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Haciendo el start");
        musicMenu = GameObject.Find("MusicaFondoMenu");
        if (musicMenu != null)
        {
            Destroy(musicMenu);
        }

        instance = this;


        GameObject a = PhotonNetwork.InstantiateSceneObject(this.cubePrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);


        GameObject aux;
        switch (PlayerPrefs.GetInt("IndiceEscenario"))
        {
            // pinguino
            case 0:
                aux = PhotonNetwork.Instantiate(this.pinguinoPlayer.name, Vector3.zero, Quaternion.identity, 0);
                break;
            // oso
            case 1:
                aux = PhotonNetwork.Instantiate(this.osoPlayer.name, Vector3.zero, Quaternion.identity, 0);
                break;

            //zorro
            case 2:
                aux = PhotonNetwork.Instantiate(this.zorraPlayer.name, Vector3.zero, Quaternion.identity, 0);
                break;
            //muñeco
            case 3:
                aux = PhotonNetwork.Instantiate(this.munecoPlayer.name, Vector3.zero, Quaternion.identity, 0);
                break;
            //reno
            case 4:
                aux = PhotonNetwork.Instantiate(this.renoPlayer.name, Vector3.zero, Quaternion.identity, 0);
                break;
            //morsa
            case 5:
                aux = PhotonNetwork.Instantiate(this.morsaPlayer.name, Vector3.zero, Quaternion.identity, 0);
                break;
            //teapot
            case 6:
                aux = PhotonNetwork.Instantiate(this.teapotPlayer.name, Vector3.zero, Quaternion.identity, 0);
                break;
            default:
                aux = PhotonNetwork.Instantiate(this.pinguinoPlayer.name, Vector3.zero, Quaternion.identity, 0);
                break;
        }



    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void leaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #region RPCs

    #endregion

    #region Private methods


    void goToGameScene()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.Log("Loading game scene");
        PhotonNetwork.LoadLevel("MapScene");
    }

    void goToWaitingScene()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.Log("Loading game scene");
        PhotonNetwork.LoadLevel("WaitingRoom");
    }

    void goToFinishScene()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.Log("Loading game scene");
        PhotonNetwork.LoadLevel("GameOver");
    }


    #endregion


    #region Photon Callbacks

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


            if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
            {
                Debug.Log("Nos hemos quedado sin jugadores suficientes");
                goToFinishScene();
            }
        }
    }

    #endregion



}
