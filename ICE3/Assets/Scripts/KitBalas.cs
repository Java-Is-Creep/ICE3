using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class KitBalas : MonoBehaviourPunCallbacks
{
    public TileScript tile;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Aviso al master de que destruya las balas
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Choque con algo");
        if (other.tag == "Player")
        {
            Debug.Log("Choque con jugador");
            this.photonView.RPC("Destroy", RpcTarget.MasterClient);
        }
    }

    /// <summary>
    /// Llamada remota para que el master destruya las balas
    /// </summary>
    [PunRPC]
    void Destroy()
    {
        
        Debug.Log("Destruido por master");
        tile.myObjectType = TileScript.tileObject.NULL;
        PhotonNetwork.Destroy(this.gameObject);

    }


}
