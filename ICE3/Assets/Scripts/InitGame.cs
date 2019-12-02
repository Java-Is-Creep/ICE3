using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitGame : MonoBehaviour
{
    public void onClick()
    {
        SceneManager.LoadScene("InitSesion");
    }
}
