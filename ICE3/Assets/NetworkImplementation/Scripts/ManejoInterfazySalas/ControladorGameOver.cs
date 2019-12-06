using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControladorGameOver : MonoBehaviourPunCallbacks
{
    Puntuaciones punt;
    [SerializeField]
    Text puntuacionText;
    // Start is called before the first frame update
    void Start()
    {
        punt = FindObjectOfType<Puntuaciones>();

        List<Puntuacion> listaOrdenada = new List<Puntuacion>();


        Debug.Log("Antes");
        string puntuaciones = "";
        foreach (int key in punt.puntuaciones.Keys)
        {
            
            Puntuacion aux;
            punt.puntuaciones.TryGetValue(key, out aux);
            Debug.Log("La puntuacion de: " + aux.nombre + " es: " + aux.puntuacion + "\n" );
            listaOrdenada.Add(aux);
        }
        Debug.Log("Despues");

        listaOrdenada.Sort(new ComparadorPuntuacion());
        
        foreach(Puntuacion punt in listaOrdenada)
        {
            Debug.Log("La puntuacion de: " + punt.nombre + " es: " + punt.puntuacion);
            puntuaciones += "La puntuacion de: " + punt.nombre + " es: " + punt.puntuacion + "\n";
            puntuacionText.text = puntuaciones;
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
