using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class KitBalas : MonoBehaviourPunCallbacks
{
    public TileScript tile;
    public float velocityRot;
    public float velocityPos;
    public float counter;

    // Start is called before the first frame update
    void Start()
    {
        velocityRot = Random.Range(40f, 60f);
        velocityPos = 0.4f;
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        this.transform.Rotate(Vector3.up, velocityRot*Time.deltaTime);

        if (counter >= 4f)
        {
            counter = 0f;
        }
        if (counter <= 2)
        {
            this.transform.Translate(0, -velocityPos * Time.deltaTime, 0);
        } else
        {
            this.transform.Translate(0, velocityPos * Time.deltaTime, 0);

        }
    }

    /// <summary>
    /// Aviso al master de que destruya las balas
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Choque con algo");
        //Debug.Log(other.tag);
        if (other.tag == "CharacterCollider")
        {
            //Debug.Log("Choque con jugador");
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
