using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameSoundsController : MonoBehaviour
{
    public AudioSource deslizar;
    public AudioSource disparo;
    public AudioSource bolazo;
    public AudioSource sinBolas;
    public AudioSource choque;
    public AudioSource recibirBolazoOof;

    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void playDeslizar()
    {
        deslizar.Play();
    }
    public void playDisparo()
    {
        disparo.Play();
    }
    public void playBolazo()
    {
        bolazo.Play();
    }
    public void playSinBolas()
    {
        sinBolas.Play();
    }
    public void playChoque()
    {
        choque.Play();
    }
    public void playRecibirBolazoOof()
    {
        recibirBolazoOof.Play();
    }

}
