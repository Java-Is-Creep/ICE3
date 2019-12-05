using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puntuaciones : MonoBehaviour
{

    public Dictionary<string, int> puntuaciones;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        puntuaciones = new Dictionary<string, int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void anadirPunto(string nombre)
    {
        Debug.Log("Puntuaciones añadiendo punto");
        int aux = 0;
        if (puntuaciones.ContainsKey(nombre))
        {

            puntuaciones.TryGetValue(nombre, out aux);
            puntuaciones.Remove(nombre);
        }
        Debug.Log("La puntuacion de :" + nombre + " era: " + aux);
        aux++;
        Debug.Log(" lA PUNTUACION DE: " + nombre + " ahora es: " + aux);
        puntuaciones.Add(nombre, aux);
    }

}
