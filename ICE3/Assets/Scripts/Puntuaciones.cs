using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Puntuaciones : MonoBehaviourPunCallbacks
{

    public Dictionary<int, Puntuacion> puntuaciones;

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
    }

}
