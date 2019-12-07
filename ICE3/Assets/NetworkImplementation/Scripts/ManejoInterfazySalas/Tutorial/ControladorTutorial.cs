﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class ControladorTutorial : MonoBehaviourPunCallbacks
{
    int TimeToCreateAmmunition;
    int TimeToCreateBandera;
    float actualTimeAmmunation;
    float actualTimeBandera;
    Cube Cubo;

    public bool hayBalas;
    public bool hayBanderas;

    public List<TileScript> spawnPersonajeCasillas;
    public List<TileScript> spawnBazokaCasillas;
    public List<TileScript> spawnBanderaCasillas;

    enum ObjetosCreables { bandera, balas }

    public int intentos;
    public int bazokasIniciales;
    public int banderasiniciales;

    //booleans controladores
    public bool [] pasos;
    public int numPasos;
    private bool hecho;

    //Texto de tutorial
    public Text texto;
    public Text loading;

    private void Awake()
    {
        hecho = false;
        spawnPersonajeCasillas = new List<TileScript>();
        spawnBazokaCasillas = new List<TileScript>();
        spawnBanderaCasillas = new List<TileScript>();
        bazokasIniciales = 2;
        banderasiniciales = 3;
        actualTimeBandera = 0;
        actualTimeAmmunation = 0;
        TimeToCreateBandera = 3;
        TimeToCreateAmmunition = 3;
        intentos = 0;
        pasos = new bool[numPasos];
        if (PlayerPrefs.GetInt("Idioma") == 0)
        {
            loading.text = "Cargando...";
            if (Application.isMobilePlatform)
            {
                texto.text = "Usa las flechas direccionales para moverte. Busca 3 banderas.";
            } else
            {
                texto.text = "Usa W, A, S, D para moverte. Busca 3 banderas.";
            }
        }
        else
        {
            loading.text = "Loading...";
            if (Application.isMobilePlatform)
            {
                texto.text = "Use the directional keys to move. Take 3 flags.";
            }
            else
            {
                texto.text = "Use W, A, S, D to move. Take 3 flags.";
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Cubo = FindObjectOfType<Cube>();
        }
        for (int i = 0; i < pasos.Length; i++)
        {
            pasos[i] = false;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if(Cubo == null)
        {
            Cubo = FindObjectOfType<Cube>();
        }


        if (PhotonNetwork.IsMasterClient)
        {
            if (!hecho)
            {
                if (spawnBazokaCasillas.Count != bazokasIniciales)
                {
                    return;
                }
                if (spawnBanderaCasillas.Count != banderasiniciales)
                {
                    return;
                }
                if (hayBalas)
                {
                    for (int i = 0; i < bazokasIniciales; i++)
                    {
                        createBazoka();
                    }
                }


                if (hayBanderas)
                {
                    for (int i = 0; i < banderasiniciales; i++)
                    {
                        createBandera();
                    }


                }
                hecho = true;
            }

            if (hayBalas)
            {
                if (actualTimeAmmunation < TimeToCreateAmmunition)
                {
                    actualTimeAmmunation += Time.deltaTime;
                }
                else
                {
                    //createObject(ObjetosCreables.balas);
                    createBazoka();
                    TimeToCreateAmmunition = Random.Range(7, 12);
                    actualTimeAmmunation = 0;
                }
            }

        }

    }

    private void createBandera()
    {
        int indexX;
        int indexY;
        int cara;
        TileScript miCasilla = null;
        if (spawnBanderaCasillas.Count > 0)
        {
            TileScript casilla;
            do
            {
                if (intentos == spawnBanderaCasillas.Count)
                {
                    intentos = 0;
                    Debug.LogError("ME HE PASADO DE INTENTAR");
                    return;
                }
                int indice = Random.Range(0, spawnBanderaCasillas.Count);
                casilla = spawnBanderaCasillas[indice];
                intentos++;
            } while (casilla.myObjectType != TileScript.tileObject.NULL);
            miCasilla = casilla;
        }

        intentos = 0;

        indexX = miCasilla.indexX;
        indexY = miCasilla.indexY;
        cara = miCasilla.cubeId;
        GameObject aux = null;
        if (miCasilla != null)
        {
            aux = PhotonNetwork.InstantiateSceneObject("BanderaCold", miCasilla.AbsolutePos, Quaternion.identity);
        }
        else
        {
            Debug.LogError("FALTA UNA Casilla");
            return;
        }

        switch (cara)
        {
            case (0):

                break;
            case (1):
                aux.transform.Rotate(new Vector3(0, 0, 90));
                break;
            case (2):
                aux.transform.Rotate(new Vector3(0, 0, -90));
                break;
            case (3):
                aux.transform.Rotate(new Vector3(90, 0, 0));
                break;
            case (4):
                aux.transform.Rotate(new Vector3(-90, 0, 0));
                break;
            case (5):
                aux.transform.Rotate(new Vector3(0, 0, 180));
                break;
        }
        spawnBanderaCasillas.Remove(miCasilla);
        aux.transform.position += aux.transform.TransformDirection(Vector3.up * 0.75f);
        miCasilla.myObjectType = TileScript.tileObject.BANDERA;
        aux.GetComponent<Banderas>().tile = miCasilla;


    }

    /// <summary>
    /// Se hace en funcion del paso que se desbloquee
    /// </summary>
    /// <param name="paso"></param>
    public void desbloquearPaso(int paso)
    {
        switch (paso)
        {
            case 0:
                pasos[0] = true;
                break;
            case 1:
                pasos[1] = true;
                break;
            case 2:
                pasos[2] = true;
                break;
            case 3:
                pasos[3] = true;
                break;
        }
    }

    private void createBazoka()
    {
        int indexX;
        int indexY;
        int cara;
        TileScript miCasilla = null;

        if (spawnBazokaCasillas.Count > 0)
        {
            TileScript casilla;
            do
            {
                if (intentos == spawnBazokaCasillas.Count)
                {
                    intentos = 0;
                    return;
                }
                int indice = Random.Range(0, spawnBazokaCasillas.Count);
                casilla = spawnBazokaCasillas[indice];
                intentos++;
            } while (casilla.myObjectType != TileScript.tileObject.NULL);
            miCasilla = casilla;
        }

        intentos = 0;

        GameObject aux = null;
        if (miCasilla != null)
        {

            indexX = miCasilla.indexX;
            indexY = miCasilla.indexY;
            cara = miCasilla.cubeId;
            aux = PhotonNetwork.InstantiateSceneObject("KitBalas", miCasilla.AbsolutePos, Quaternion.identity);
        }
        else
        {
            return;
        }

        switch (cara)
        {
            case (0):

                break;
            case (1):
                aux.transform.Rotate(new Vector3(0, 0, 90));
                break;
            case (2):
                aux.transform.Rotate(new Vector3(0, 0, -90));
                break;
            case (3):
                aux.transform.Rotate(new Vector3(90, 0, 0));
                break;
            case (4):
                aux.transform.Rotate(new Vector3(-90, 0, 0));
                break;
            case (5):
                aux.transform.Rotate(new Vector3(0, 0, 180));
                break;
        }

        
        aux.transform.position += aux.transform.TransformDirection(Vector3.up);
        miCasilla.myObjectType = TileScript.tileObject.BAZOKA;
        aux.GetComponent<KitBalas>().tile = miCasilla;

    }

    public TileScript getCasillaVacia()
    {
        if (spawnPersonajeCasillas.Count > 0)
        {/*
            TileScript casilla;
            do
            {
                if (intentos == spawnPersonajeCasillas.Count)
                {
                    intentos = 0;
                    return null;
                }
                int indice = Random.Range(0, spawnBanderaCasillas.Count);
                casilla = spawnPersonajeCasillas[indice];
                intentos++;
            } while (casilla.myObjectType != TileScript.tileObject.NULL);
            intentos = 0;
            spawnPersonajeCasillas.Remove(casilla);
            return casilla;
            */
            TileScript casilla;
            int indice = Random.Range(0, spawnPersonajeCasillas.Count);
            casilla = spawnPersonajeCasillas[indice];
            spawnPersonajeCasillas.Remove(casilla);
            return casilla;
        }

        return null;
    }


    public void segundoTexto()
    {
        if (PlayerPrefs.GetInt("Idioma") == 0)
        {
            if (Application.isMobilePlatform)
            {
                texto.text = "Coge un lanzabolas y usa el botón inferior derecho para lanzar bolas de nieve.";
            }
            else
            {
                texto.text = "Coge un lanzabolas y pulsa espacio para lanzar bolas de nieve. Clic izquierdo para cámara libre.";
            }
        }
        else
        {
            if (Application.isMobilePlatform)
            {
                texto.text = "Take one bazooka and use the bottom right button to throw snowballs.";
            }
            else
            {
                texto.text = "Take one bazooka and press Espace to throw snowballs. Left click to free look camera.";
            }
        }
    }

    public void tercerTexto()
    {
        if (PlayerPrefs.GetInt("Idioma") == 0)
        {
            texto.text = "Usa tu lanzabolas para cambiar tu dirección en movimiento.";
        }
        else
        {
            texto.text = "You can change your direction by using your bazooka.";
        }
    }

    #region llenar casillas clave
    public void anadirCasillaPersonaje(TileScript casilla)
    {

        if (PhotonNetwork.IsMasterClient)
        {
            spawnPersonajeCasillas.Add(casilla);
            //Vector3 aux = new Vector3(casilla.indexX, casilla.indexY, casilla.cubeId);
            /*
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add("pos" + spawnPersonajeCasillas.Count, aux);
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
            */
        }


    }

    public void anadirCasillaBazoka(TileScript casilla)
    {
        spawnBazokaCasillas.Add(casilla);
    }

    public void anadirCasillaBandera(TileScript casilla)
    {
        spawnBanderaCasillas.Add(casilla);
    }
    #endregion
}
