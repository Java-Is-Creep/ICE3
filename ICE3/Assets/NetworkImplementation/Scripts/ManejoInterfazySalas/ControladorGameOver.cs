using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ControladorGameOver : MonoBehaviourPunCallbacks
{
    Puntuaciones punt;
    // Start is called before the first frame update
    void Start()
    {
        punt = FindObjectOfType<Puntuaciones>();
        foreach(string key in punt.puntuaciones.Keys)
        {
            int aux;
            punt.puntuaciones.TryGetValue(key, out aux);
            Debug.Log("La puntuacion de: " + key + " es: " + aux);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void salir()
    {

        if(punt != null)
        {
            Destroy(punt.gameObject);
        }
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainMenu");
    }


}
