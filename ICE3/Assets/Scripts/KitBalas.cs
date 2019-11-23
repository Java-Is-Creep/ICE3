using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class KitBalas : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Choque con algo");
        if (other.tag == "Player")
        {
            Debug.Log("Choque con jugador");
            this.photonView.RPC("Destroy", RpcTarget.MasterClient);
        }
    }

    public void crash()
    {
        Debug.Log("Choque con jugador mandado por el");
        this.photonView.RPC("Destroy", RpcTarget.MasterClient);
    }

    [PunRPC]
    void Destroy()
    {
        
        Debug.Log("Destruido por master");
        PhotonNetwork.Destroy(this.gameObject);

    }

    /*
    public void init( TileScript ts,int cara)
    {
        this.transform.position = ts.AbsolutePos;
        switch (cara)
        {
            case (0):
                break;
            case (1):
                this.transform.Rotate(new Vector3(0, 0, 90));
                break;
            case (2):
                this.transform.Rotate(new Vector3(0, 0, -90));
                break;
            case (3):
                this.transform.Rotate(new Vector3(90, 0, 0));
                break;
            case (4):
                this.transform.Rotate(new Vector3(-90, 0, 0));
                break;
            case (5):
                Debug.Log("Cambiando de cara");
                this.transform.Rotate(new Vector3(0, 0, 180));
                break;
        }
        this.transform.position += this.transform.TransformDirection(Vector3.up*1.5f);
    }
    */
}
