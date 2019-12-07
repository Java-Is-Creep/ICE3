using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Puntuaciones : MonoBehaviourPunCallbacks
{

    public Dictionary<int, Puntuacion> puntuaciones;

    public Text top1;
    public Text top2;
    public Text top3;

    public Text numerosTop3;
    public Text maxPuntuation;
    string cadena;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        puntuaciones = new Dictionary<int, Puntuacion>();
        if (PlayerPrefs.GetInt ("Idioma") == 0)
        {
            cadena = "Puntos 1º: ";
        }
        else
        {
            cadena = "Best Score: ";
        }
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
        }
        else
        {
            Player play = PhotonNetwork.CurrentRoom.GetPlayer(id);
            Debug.Log(play.NickName + " nombre del player");
            Puntuacion aux2 = new Puntuacion(play.NickName);
            puntuaciones.Add(id,aux2);
        }

        List<Puntuacion> listaOrdenada = new List<Puntuacion>();

        //string puntuacionesString = "";
        foreach (int key in puntuaciones.Keys)
        {
            Puntuacion aux3;
            puntuaciones.TryGetValue(key, out aux3);
            Debug.Log("La puntuacion de: " + aux3.nombre + " es: " + aux3.puntuacion + "\n");
            listaOrdenada.Add(aux3);
        }

        listaOrdenada.Sort(new ComparadorPuntuacion());

        /*foreach (Puntuacion punt in listaOrdenada)
        {
            Debug.Log("La puntuacion de: " + punt.nombre + " es: " + punt.puntuacion);
            puntuacionesString += "La puntuacion de: " + punt.nombre + " es: " + punt.puntuacion + "\n";
            textoPuntuaciones.text = puntuacionesString;
        }*/

        if (listaOrdenada.Count == 2)
        {
            top1.text = "" + listaOrdenada[0].nombre;
            top2.text = "" + listaOrdenada[1].nombre;
            top3.gameObject.SetActive(false);
            numerosTop3.text = "1º\n2º";
            maxPuntuation.text = cadena + listaOrdenada[0].puntuacion;
        }
        else if (listaOrdenada.Count > 2)
        {
            top1.text = "" + listaOrdenada[0].nombre;
            top2.text = "" + listaOrdenada[1].nombre;
            top3.text = "" + listaOrdenada[2].nombre;
            top3.gameObject.SetActive(true);
            numerosTop3.text = "1º\n2º\n3º";

            maxPuntuation.text = cadena + listaOrdenada[0].puntuacion;
        }

        

    }

}
