using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControladorGameOver : MonoBehaviourPunCallbacks
{
    Puntuaciones punt;

    public Text puestoText;
    public Text botonSalir;
    [Header ("Podio")]
    public Image personaje1;
    public Image personaje2;
    public Image personaje3;
    public Text personaje1Text;
    public Text personaje2Text;
    public Text personaje3Text;

    [Header("Conjuntos")]
    public GameObject[] players;
    public Text[] name2Jugadores;
    public Text[] score2Jugadores;
    public Text[] name3Jugadores;
    public Text[] score3Jugadores;
    public Text[] name4Jugadores;
    public Text[] score4Jugadores;
    public Text[] name5Jugadores;
    public Text[] score5Jugadores;
    public Text[] name6Jugadores;
    public Text[] score6Jugadores;

    [Header("SpritesPersonaje")]
    public Sprite[] p;

    // Start is called before the first frame update
    void Start()
    {
        string[] puestosEsp = { "Primer Puesto", "Segundo Puesto",
            "Tercer Puesto", "Cuarto Puesto", "Quinto Puesto", "Sexto Puesto" };
        string[] puestosEng = { "First Place", "Second Place",
            "Third Place", "Fourth Place", "Fifth Place", "Sixth Place"};

        // Idioma 0 espanol
        if (PlayerPrefs.GetInt ("Idioma") == 0)
        {
            botonSalir.text = "Salir";
        }
        else
        {
            botonSalir.text = "Exit";
        }

        Player[] jugadores = PhotonNetwork.PlayerList;
        punt = FindObjectOfType<Puntuaciones>();
        List<Puntuacion> listaOrdenada = new List<Puntuacion>();
        foreach (int key in punt.puntuaciones.Keys)
        {
            Puntuacion aux;
            punt.puntuaciones.TryGetValue(key, out aux);
            Debug.Log("La puntuacion de: " + aux.nombre + " es: " + aux.puntuacion + "\n" );
            listaOrdenada.Add(aux);
        }

        listaOrdenada.Sort(new ComparadorPuntuacion());
        players[listaOrdenada.Count - 2].SetActive(true);

        Text[] auxTexto;
        Text[] auxScore;
        auxTexto = name6Jugadores;
        auxScore = score6Jugadores;
        switch (listaOrdenada.Count)
        {
            case 2:
                auxTexto = name2Jugadores;
                auxScore = score2Jugadores;
                personaje3.gameObject.SetActive(false);
                break;
            case 3:
                auxTexto = name3Jugadores;
                auxScore = score3Jugadores;
                break;
            case 4:
                auxTexto = name4Jugadores;
                auxScore = score4Jugadores;
                break;
            case 5:
                auxTexto = name5Jugadores;
                auxScore = score5Jugadores;
                break;
            case 6:
                auxTexto = name6Jugadores;
                auxScore = score6Jugadores;
                break;
        }

        int i = 0;
        foreach (Puntuacion punt in listaOrdenada)
        {
            // Seteo nombre y texto de cada uno de los jugadores
            auxTexto[i].text = punt.nombre;
            auxScore[i].text = "" + punt.puntuacion;

            if (punt.nombre.CompareTo(PhotonNetwork.LocalPlayer.NickName) == 0)
            {
                if (PlayerPrefs.GetInt("Idioma") == 0)
                {
                    puestoText.text = puestosEsp[i];
                }
                else
                {
                    puestoText.text = puestosEng[i];
                }
            }

            // Podium
            if (i == 0)
            {
                for (int n = 0; n < PhotonNetwork.CurrentRoom.PlayerCount; n++)
                {
                    Debug.Log("Jugador " + n + " es: " + jugadores[n].NickName);
                    if (punt.nombre.CompareTo(jugadores[n].NickName) == 0)
                    {
                        object indice;
                        jugadores[n].CustomProperties.TryGetValue("skin", out indice);
                        int index = (int)indice;
                        personaje1.sprite = p[index];
                        personaje1Text.text = punt.nombre;
                        break;
                    }
                }
            }
            else if (i == 1)
            {
                for (int n = 0; n < PhotonNetwork.CurrentRoom.PlayerCount; n++)
                {
                    if (punt.nombre.CompareTo(jugadores[n].NickName) == 0)
                    {
                        object indice;
                        jugadores[n].CustomProperties.TryGetValue("skin", out indice);
                        int index = (int)indice;
                        personaje2.sprite = p[index];
                        personaje2Text.text = punt.nombre;
                    }
                }
            }
            else if (i == 2)
            {
                for (int n = 0; n < PhotonNetwork.CurrentRoom.PlayerCount; n++)
                {
                    if (punt.nombre.CompareTo(jugadores[n].NickName) == 0)
                    {
                        object indice;
                        jugadores[n].CustomProperties.TryGetValue("skin", out indice);
                        int index = (int)indice;
                        personaje3.sprite = p[index];
                        personaje3Text.text = punt.nombre;
                    }
                }
            }
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void salir()
    {

        if(punt != null)
        {
            Destroy(punt.gameObject);
        }
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainMenu");
    }

}
