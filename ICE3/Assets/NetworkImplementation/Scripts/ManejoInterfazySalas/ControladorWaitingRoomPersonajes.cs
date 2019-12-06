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
    private bool refrescar;
    public float tiempoParaRecargar;
    public float actualTime;

    [Header("Textos")]
    public Text[] text2Jugadores;
    public Text[] text3Jugadores;
    public Text[] text4Jugadores;
    public Text[] text5Jugadores;
    public Text[] text6Jugadores;

    public Image[] image2Jugadores;
    public Image[] image3Jugadores;
    public Image[] image4Jugadores;
    public Image[] image5Jugadores;
    public Image[] image6Jugadores;

    public Sprite[] p;
    public GameObject[] g;
    public Sprite interrogacion;

    string cadena;

    [Header("Textos Idioma")]
    public Text titleText;
    public Text numberPlayersText;
    public Text waitingPlayersText;
    public Text exitText;

    //public GameObject loadingAnimation;

    // Start is called before the first frame update
    void Start()
    {
        //g[PhotonNetwork.CurrentRoom.MaxPlayers - 2].SetActive(true);

        if (PlayerPrefs.GetInt ("Idioma") == 0)
        {
            cadena = "Desconocido";
            titleText.text = "Sala Espera";
            numberPlayersText.text = "Número Jugadores";
            waitingPlayersText.text = "Esperando más jugadores...";
            exitText.text = "Salir";
        }
        else
        {
            cadena = "Unknown";
            titleText.text = "Waiting Room";
            numberPlayersText.text = "Number of Players";
            waitingPlayersText.text = "Waiting more players...";
            exitText.text = "Exit";
        }

        jugadores = PhotonNetwork.PlayerList;
        ExitGames.Client.Photon.Hashtable character = new ExitGames.Client.Photon.Hashtable();
        character.Add("skin", PlayerPrefs.GetInt("IndiceEscenario"));
        PhotonNetwork.LocalPlayer.SetCustomProperties(character);
        refrescar = true;
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
        g[PhotonNetwork.CurrentRoom.MaxPlayers - 2].SetActive(true);
        jugadores = PhotonNetwork.PlayerList;
        switch (PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            case 2:
                int i = 0;
                for (i = 0; i < jugadores.Length; i++)
                {
                    object indice;
                    jugadores[i].CustomProperties.TryGetValue("skin", out indice);
                    if (indice != null)
                    {
                        int index = (int)indice;
                        text2Jugadores[i].text = jugadores[i].NickName;
                        image2Jugadores[i].sprite = p[index];
                        //Debug.Log("Jugador: " + jugadores[i].NickName + " lleva la skin de: " + index);
                    }
                }
                for (int j = i; j < text2Jugadores.Length; j++)
                {
                    text2Jugadores[j].text = cadena;
                    image2Jugadores[j].sprite = interrogacion;
                }
                break;
            case 3:
                for (i = 0; i < jugadores.Length; i++)
                {
                    object indice;
                    jugadores[i].CustomProperties.TryGetValue("skin", out indice);
                    if (indice != null)
                    {
                        int index = (int)indice;
                        text3Jugadores[i].text = jugadores[i].NickName;
                        image3Jugadores[i].sprite = p[index];
                        //Debug.Log("Jugador: " + jugadores[i].NickName + " lleva la skin de: " + index);
                    }
                }
                for (int j = i; j < text3Jugadores.Length; j++)
                {
                    text3Jugadores[j].text = cadena;
                    image3Jugadores[j].sprite = interrogacion;
                }
                break;
            case 4:
                for (i = 0; i < jugadores.Length; i++)
                {
                    object indice;
                    jugadores[i].CustomProperties.TryGetValue("skin", out indice);
                    if (indice != null)
                    {
                        int index = (int)indice;
                        text4Jugadores[i].text = jugadores[i].NickName;
                        image4Jugadores[i].sprite = p[index];
                        //Debug.Log("Jugador: " + jugadores[i].NickName + " lleva la skin de: " + index);
                    }
                }
                for (int j = i; j < text4Jugadores.Length; j++)
                {
                    text4Jugadores[j].text = cadena;
                    image4Jugadores[j].sprite = interrogacion;
                }
                break;
            case 5:
                for (i = 0; i < jugadores.Length; i++)
                {
                    object indice;
                    jugadores[i].CustomProperties.TryGetValue("skin", out indice);
                    if (indice != null)
                    {
                        int index = (int)indice;
                        text5Jugadores[i].text = jugadores[i].NickName;
                        image5Jugadores[i].sprite = p[index];
                        //Debug.Log("Jugador: " + jugadores[i].NickName + " lleva la skin de: " + index);
                    }
                }
                for (int j = i; j < text5Jugadores.Length; j++)
                {
                    text5Jugadores[j].text = cadena;
                    image5Jugadores[j].sprite = interrogacion;
                }
                break;
            case 6:
                for (i = 0; i < jugadores.Length; i++)
                {
                    object indice;
                    jugadores[i].CustomProperties.TryGetValue("skin", out indice);
                    if (indice != null)
                    {
                        int index = (int)indice;
                        text6Jugadores[i].text = jugadores[i].NickName;
                        image6Jugadores[i].sprite = p[index];
                        //Debug.Log("Jugador: " + jugadores[i].NickName + " lleva la skin de: " + index);
                    }
                }
                for (int j = i; j < text6Jugadores.Length; j++)
                {
                    text6Jugadores[j].text = cadena;
                    image6Jugadores[j].sprite = interrogacion;
                }
                break;
        }




        /*string nombres = " Tu eleccion es: " + PlayerPrefs.GetInt("IndiceEscenario");
        jugadores = PhotonNetwork.PlayerList;
        foreach (Player play in jugadores)
        {
            object indice;
            play.CustomProperties.TryGetValue("skin", out indice);
            if (indice != null)
            {
                int index = (int)indice;
                Debug.Log("Jugador: " + play.NickName + " lleva la skin de: " + index);
                nombres += "Jugador: " + play.NickName + " lleva la skin de: " + index + "\n";
            }

        }
        texto.text = nombres;*/
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
