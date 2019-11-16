using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks
{

    public float multiplayer;

    // Start is called before the first frame update
    void Start()
    {

        //se hace para que la camara te siga a ti;
        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();


        if (_cameraWork != null)
        {
            if (photonView.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }
    }

    // Update is called once per frame
    void Update()
    {

        // Se comprueba si estas conectado para evitar teenr que conectarte al probar;
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        v *= multiplayer;
        h *= multiplayer;


        this.transform.position = new Vector3(this.transform.position.x + h, this.transform.position.y, this.transform.position.z + v);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.instance.leaveRoom();
        }

    }
}
