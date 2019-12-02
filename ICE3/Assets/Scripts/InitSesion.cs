using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitSesion : MonoBehaviour
{
    [SerializeField]
    private InputField playerName;
    [SerializeField]
    private InputField playerNameMobile;
    

    // Start is called before the first frame update
    void Start()
    {
        if (Application.isMobilePlatform)
        {
            // Crear
            playerNameMobile.gameObject.SetActive(true);

        }
        else
        {
            Debug.Log("no movil");
            // crear
            playerName.gameObject.SetActive(true);
        }
    }

    public void onClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
