using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameOverPropio : MonoBehaviourPunCallbacks
{
    private int timeToChoose;
    public float actualTime;
    private bool repetir;
    GameOverManejador manejadorGeneral;
    // Start is called before the first frame update
    void Start()
    {
        actualTime = 0;
        timeToChoose = 15;
        repetir = false;
        manejadorGeneral = FindObjectOfType<GameOverManejador>();
    }

    // Update is called once per frame
    void Update()
    {
        actualTime += Time.deltaTime;
        if (actualTime >= timeToChoose)
        {
            if (!repetir)
            {
                manejadorGeneral.salirse();
                Debug.Log("Mando RPC por tiempo");
                PhotonNetwork.LeaveRoom();
                SceneManager.LoadScene("MainMenu");
            }

        }
    }

    public void volverAJugar()
    {
        manejadorGeneral.volverAjugar();
        repetir = true;

    }

    public void salir()
    {
        manejadorGeneral.salirse();
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainMenu");
    }

}
