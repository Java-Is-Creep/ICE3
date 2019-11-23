using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ControladorNivel : MonoBehaviourPunCallbacks
{
    int TimeToCreateAmmunition = 20;
    float actualTime = 0;
    Cube Cubo;
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
            if (actualTime < TimeToCreateAmmunition)
            {
                actualTime += Time.deltaTime;
            }
            else
            {
                createAmmunationKit();
                TimeToCreateAmmunition = Random.Range(15, 26);
                actualTime = 0;
            }
        }
       
    }


    public void createAmmunationKit()
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

        GameObject aux = PhotonNetwork.InstantiateSceneObject("KitBalas", ts.AbsolutePos,Quaternion.identity);
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
