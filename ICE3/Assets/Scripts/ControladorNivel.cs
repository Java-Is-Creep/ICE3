using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ControladorNivel : MonoBehaviourPunCallbacks
{
    int TimeToCreateAmmunition = 10;
    int TimeToCreateBandera = 15;
    float actualTimeAmmunation = 0;
    float actualTimeBandera = 0;
    Cube Cubo;

    public bool hayBalas;
    public bool hayBanderas;

    enum ObjetosCreables { bandera, balas }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Cubo = FindObjectOfType<Cube>();
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
                    createObject(ObjetosCreables.balas);
                    TimeToCreateAmmunition = Random.Range(15, 26);
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
                    createObject(ObjetosCreables.bandera);
                    TimeToCreateBandera = Random.Range(20, 30);
                    actualTimeBandera = 0;
                }

            }
        }

    }


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
                Debug.Log("Cambiando de cara");
                aux.transform.Rotate(new Vector3(0, 0, 180));
                break;
        }
        aux.transform.position += aux.transform.TransformDirection(Vector3.up);
    }


}
