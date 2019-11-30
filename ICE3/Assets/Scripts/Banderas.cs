using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class Banderas : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Si choco con algun jugador, aviso al master de que tiene que destruir ese objeto
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Choque con algo");
        if (other.tag == "Player")
        {
            Debug.Log("Choque con jugador");
            this.photonView.RPC("DestroyBandera", RpcTarget.MasterClient);
        }
    }

    /// <summary>
    /// Aviso al master de que destruya la bandera
    /// </summary>
    [PunRPC]
    void DestroyBandera()
    {

        Debug.Log("Destruido por master");
        PhotonNetwork.Destroy(this.gameObject);

    }
}
