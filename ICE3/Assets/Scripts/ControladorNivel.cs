using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ControladorNivel : MonoBehaviourPunCallbacks
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

    private void Awake()
    {
        spawnPersonajeCasillas = new List<TileScript>();
        spawnBazokaCasillas = new List<TileScript>();
        spawnBanderaCasillas = new List<TileScript>();
        bazokasIniciales = 5;
        banderasiniciales = 3;
        actualTimeBandera = 0;
        actualTimeAmmunation = 0;
        TimeToCreateBandera = 15;
        TimeToCreateAmmunition = 10;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Cubo = FindObjectOfType<Cube>();
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


    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
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

            if (hayBanderas)
            {

                if (actualTimeBandera < TimeToCreateBandera)
                {
                    actualTimeBandera += Time.deltaTime;
                }
                else
                {
                    createBandera();
                    //createObject(ObjetosCreables.bandera);
                    TimeToCreateBandera = Random.Range(10, 15);
                    actualTimeBandera = 0;
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
                if(intentos == spawnBanderaCasillas.Count)
                {
                    intentos = 0;
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
        if(miCasilla != null)
        {
            aux = PhotonNetwork.InstantiateSceneObject("BanderaCold", miCasilla.AbsolutePos, Quaternion.identity);
        } else
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

        aux.transform.position += aux.transform.TransformDirection(Vector3.up * 0.75f);
        miCasilla.myObjectType = TileScript.tileObject.BANDERA;
        aux.GetComponent<Banderas>().tile = miCasilla;
        

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
                int indice = Random.Range(0, spawnBanderaCasillas.Count);
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


    // cuando se cree el mapa nuevo no serían válidos
    private void createObject(ObjetosCreables obj)
    {
        int indexX;
        int indexY;
        int cara;
        TileScript ts;

        do
        {
            indexX = Random.Range(0, (int)Cubo.heigth);
            indexY = Random.Range(0, (int)Cubo.width);
            cara = Random.Range(0, 6);
            ts = Cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>();
        } while (ts.myObjectType != TileScript.tileObject.NULL);
        GameObject aux;
        switch (obj)
        {
            case ObjetosCreables.balas:
                aux = PhotonNetwork.InstantiateSceneObject("KitBalas", ts.AbsolutePos, Quaternion.identity);
                break;
            case ObjetosCreables.bandera:
                aux = PhotonNetwork.InstantiateSceneObject("BanderaCold", ts.AbsolutePos, Quaternion.identity);
                break;
            default:
                aux = null;
                break;
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
        switch (obj)
        {
            case ObjetosCreables.balas:
                aux.transform.position += aux.transform.TransformDirection(Vector3.up);
                break;
            case ObjetosCreables.bandera:
                aux.transform.position += aux.transform.TransformDirection(Vector3.up * 0.75f);
                break;
            default:
                aux = null;
                break;
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
