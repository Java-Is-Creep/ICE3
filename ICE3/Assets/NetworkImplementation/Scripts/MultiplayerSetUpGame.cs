using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class MultiplayerSetUpGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreatePlayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Cube"), Vector3.zero, Quaternion.identity);
        }
        PhotonNetwork.Instantiate( Path.Combine(  "PhotonPrefabs" ,"Character"), Vector3.zero, Quaternion.identity);
    }
}
