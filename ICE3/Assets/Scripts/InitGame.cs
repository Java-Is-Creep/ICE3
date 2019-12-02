using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitGame : MonoBehaviour
{

    public AudioSource a;

    public void onClick()
    {
        a.Play();
        SceneManager.LoadScene("InitSesion");
    }
}
