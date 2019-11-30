using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{

    public static GameManager instance;
    public ControladorNivel controlador;

    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;
    public GameObject cubePrefab;



    void OnApplicationQuit()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Launcher");
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        // si eres el master, creas el cubo
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject a = PhotonNetwork.InstantiateSceneObject(this.cubePrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
            //a.transform.position = new Vector3(-4, 4, -3.5f);
        } else
        {
            Debug.Log("GirandoCaras");

        }

        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            TileScript casilla = controlador.getCasillaVacia();
             GameObject aux = PhotonNetwork.Instantiate(this.playerPrefab.name, casilla.AbsolutePos, Quaternion.identity, 0);
            CharacterController characterCont = aux.GetComponent<CharacterController>();
            characterCont.indexX = casilla.indexX;
            characterCont.indexY = casilla.indexY;
            characterCont.cara = casilla.cubeId;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
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


            if(PhotonNetwork.CurrentRoom.PlayerCount < 2)
            {
                Debug.Log("Nos hemos quedado sin jugadores suficientes");
                goToWaitingScene();
            }
        }
    }

    #endregion



}
