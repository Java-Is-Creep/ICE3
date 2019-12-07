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

    public Text placeholderNombre;
    public Text textoError;

    public Text title;
    public Text button;
    public Text nombreJugadorMobile;
    public Text nombreJugadorPC;
    public Text sonido;

    public GameObject on;
    public GameObject off;

    public Sprite activate;
    public Sprite desactivate;

    float timeError;

    
    // Start is called before the first frame update
    void Start()
    {
        timeError = 0;
        if (Application.isMobilePlatform)
        {
            // Crear
            playerNameMobile.gameObject.SetActive(true);
        }
        else
        {
            // crear
            playerName.gameObject.SetActive(true);
        }

        // Idioma 0 = Espanol
        if (PlayerPrefs.GetInt("Idioma") == 0)
        {
            title.text = "Elige un nombre";
            button.text = "Iniciar Juego";
            nombreJugadorMobile.text = "Nombre Jugador";
            nombreJugadorPC.text = "Nombre Jugador";
            sonido.text = "Sonido";
        }
        else
        {
            title.text = "Choose your name";
            button.text = "Init Game";
            nombreJugadorMobile.text = "Player Name";
            nombreJugadorPC.text = "Player Name";
            sonido.text = "Sound";
        }

        if (PlayerPrefs.GetInt("Sonido") == 0)
        {
            on.gameObject.GetComponent<Image>().sprite = desactivate;
            off.gameObject.GetComponent<Image>().sprite = activate;
            AudioListener.volume = 0;
        }
        else
        {
            on.gameObject.GetComponent<Image>().sprite = activate;
            off.gameObject.GetComponent<Image>().sprite = desactivate;
            AudioListener.volume = 1;
        }
        
    }

    public void Update()
    {
        if (Application.isMobilePlatform)
        {
            // Crear
            placeholderNombre.text = playerNameMobile.text;
        }
        timeError += Time.deltaTime;
        if (timeError > 3)
        {
            textoError.text = "";
        }
    }

    public void onClick()
    {

        if (Application.isMobilePlatform)
        {
            if (playerNameMobile.text != "")
            {
                PlayerPrefs.SetString("Name", playerNameMobile.text);
            }
        }
        else
        {
            if (playerName.text != "")
            {
                PlayerPrefs.SetString("Name", playerName.text);
            }
        }

        if (playerName.text != "" || playerNameMobile.text != "")
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            if (PlayerPrefs.GetInt("Idioma") == 0)
            {
                textoError.text = "Escribe un nombre";
            }
            else
            {
                textoError.text = "Type a name";
            }
            timeError = 0;
                
        }
    }

    public void botonOn()
    {
        on.gameObject.GetComponent<Image>().sprite = activate;
        off.gameObject.GetComponent<Image>().sprite = desactivate;
        PlayerPrefs.SetInt("Sonido", 1);
        AudioListener.volume = 1;
    }

    public void botonOff()
    {
        on.gameObject.GetComponent<Image>().sprite = desactivate;
        off.gameObject.GetComponent<Image>().sprite = activate;
        PlayerPrefs.SetInt("Sonido", 0);
        AudioListener.volume = 0;
    }

    public void back()
    {
        SceneManager.LoadScene("InitGame");
    }
}
