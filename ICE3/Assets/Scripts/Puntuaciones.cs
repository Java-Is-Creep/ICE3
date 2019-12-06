using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Puntuaciones : MonoBehaviourPunCallbacks
{

    public Dictionary<int, Puntuacion> puntuaciones;
    [SerializeField]
    Text textoPuntuaciones;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        puntuaciones = new Dictionary<int, Puntuacion>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void anadirPunto(int id)
    {
        Debug.Log("El id que me mandan es: " + id);
        //Debug.Log("Puntuaciones añadiendo punto");
        Puntuacion aux;
        if (puntuaciones.ContainsKey(id))
        {

            puntuaciones.TryGetValue(id, out aux);
            aux.aumentarPuntuacion();
        } else
        {
            
            Player play = PhotonNetwork.CurrentRoom.GetPlayer(id);
            Debug.Log(play.NickName + " nombre del player");
            Puntuacion aux2 = new Puntuacion(play.NickName);
            puntuaciones.Add(id,aux2);
        }


        List<Puntuacion> listaOrdenada = new List<Puntuacion>();


        string puntuacionesString = "";
        foreach (int key in puntuaciones.Keys)
        {

            Puntuacion aux3;
            puntuaciones.TryGetValue(key, out aux3);
            Debug.Log("La puntuacion de: " + aux3.nombre + " es: " + aux3.puntuacion + "\n");
            listaOrdenada.Add(aux3);
        }

        listaOrdenada.Sort(new ComparadorPuntuacion());

        foreach (Puntuacion punt in listaOrdenada)
        {
            Debug.Log("La puntuacion de: " + punt.nombre + " es: " + punt.puntuacion);
            puntuacionesString += "La puntuacion de: " + punt.nombre + " es: " + punt.puntuacion + "\n";
            textoPuntuaciones.text = puntuacionesString;
        }



    }

}
