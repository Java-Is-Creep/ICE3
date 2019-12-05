using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitGame : MonoBehaviour
{

    public AudioSource a;

    public Button botonIdioma;

    public Sprite espanolSprite;
    public Sprite englishSprite;

    public Text playText;

    public void Start()
    {
        if (!PlayerPrefs.HasKey("Idioma"))
        {
            PlayerPrefs.SetInt("Idioma", 0);
        }
        // Pintado de los botones de idioma
        if (PlayerPrefs.HasKey("Idioma"))
        {
            if (PlayerPrefs.GetInt("Idioma") == 0)
            {
                botonIdioma.gameObject.GetComponent<Image>().sprite = espanolSprite;
                playText.text = "Jugar";
            }
            else
            {
                botonIdioma.gameObject.GetComponent<Image>().sprite = englishSprite;
                playText.text = "Play";
            }
        }

        if (!PlayerPrefs.HasKey("Sonido"))
        {
            PlayerPrefs.SetInt("Sonido", 1);
        }
    }

    public void onClick()
    {
        a.Play();
        SceneManager.LoadScene("InitSesion");
    }

    public void pulsarIdioma()
    {
        if (PlayerPrefs.GetInt("Idioma") == 0)
        {
            PlayerPrefs.SetInt("Idioma", 1);
            botonIdioma.gameObject.GetComponent<Image>().sprite = englishSprite;
            playText.text = "Play";
        }
        else
        {
            PlayerPrefs.SetInt("Idioma", 0);
            botonIdioma.gameObject.GetComponent<Image>().sprite = espanolSprite;
            playText.text = "Jugar";
        }
    }
}
