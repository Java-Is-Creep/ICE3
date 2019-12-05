using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    Carrousel miCarrousel;
    public Image imagePersonaje;
    public Text textModo;
    public Text tituloDescr1;
    public Text contentDescr1;
    public Text tituloDescr2;
    public Text contentDescr2;
    public Button botonModo1;
    public Button botonModo2;
    public Button botonSonido;
    public Button botonEspanol;
    public Button botonIngles;

    // Sprites Modos (boton)
    public Sprite desactivadoBoton;
    public Sprite activadoBoton;

    // Sprites Sonido (boton)
    public Sprite activadoCopo;
    public Sprite desactivadoCopo;

    int preIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        miCarrousel = FindObjectOfType<Carrousel>();

        ///////// MODO
        // 1 --> Batalla bolas de nieve (por defecto)
        // 2 --> Hazte con los objetivos


        // Set del modo por defecto
        if (!PlayerPrefs.HasKey("Modo"))
        {
            PlayerPrefs.SetInt("Modo", 1);
        }
        // Pintado del modo de juego
        if (PlayerPrefs.HasKey("Modo"))
        {
            if (PlayerPrefs.GetInt("Modo") == 1)
            {
                textModo.text = "Modo 1";
                tituloDescr1.gameObject.SetActive(true);
                contentDescr1.gameObject.SetActive(true);
                tituloDescr2.gameObject.SetActive(false);
                contentDescr2.gameObject.SetActive(false);
                botonModo1.gameObject.GetComponent<Image>().sprite = activadoBoton;
                botonModo2.gameObject.GetComponent<Image>().sprite = desactivadoBoton;
            }
            else
            {
                textModo.text = "Modo 2";
                tituloDescr1.gameObject.SetActive(false);
                contentDescr1.gameObject.SetActive(false);
                tituloDescr2.gameObject.SetActive(true);
                contentDescr2.gameObject.SetActive(true);
                botonModo1.gameObject.GetComponent<Image>().sprite = desactivadoBoton;
                botonModo2.gameObject.GetComponent<Image>().sprite = activadoBoton;
            }
        }


        ///////// SONIDO
        // 0 --> Desactivado
        // 1 --> Activado (por defecto)

        // Set del sonido por defecto
        if (!PlayerPrefs.HasKey("Sonido"))
        {
            PlayerPrefs.SetInt("Sonido", 1);
        }
        // Pintado del boton de sonido
        if (PlayerPrefs.HasKey("Sonido"))
        {
            if (PlayerPrefs.GetInt("Sonido") == 1)
            {
                botonSonido.gameObject.GetComponent<Image>().sprite = activadoCopo;
            }
            else
            {
                botonSonido.gameObject.GetComponent<Image>().sprite = desactivadoCopo;
            }
        }


        ///////// IDIOMA
        // 0 --> Español (por defecto)
        // 1 --> Ingles

        // Set del idioma por defecto
        if (!PlayerPrefs.HasKey("Idioma"))
        {
            PlayerPrefs.SetInt("Idioma", 0);
        }
        // Pintado de los botones de idioma
        if (PlayerPrefs.HasKey("Idioma"))
        {
            if (PlayerPrefs.GetInt("Idioma") == 0)
            {
                botonEspanol.gameObject.GetComponent<Image>().sprite = activadoCopo;
                botonIngles.gameObject.GetComponent<Image>().sprite = desactivadoCopo;
            }
            else
            {
                botonEspanol.gameObject.GetComponent<Image>().sprite = desactivadoCopo;
                botonIngles.gameObject.GetComponent<Image>().sprite = activadoCopo;
            }
        }

        preIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (preIndex != miCarrousel.current_index)
        {
            preIndex = miCarrousel.current_index;
            imagePersonaje.sprite = miCarrousel.images[preIndex].GetComponent<Image>().sprite;
        }
    }


    public void jugar()
    {
        PlayerPrefs.SetInt("IndiceEscenario", miCarrousel.current_index);
        SceneManager.LoadScene("Launcher");
    }

    public void modo1()
    {
        PlayerPrefs.SetInt("Modo", 1);
        textModo.text = "Modo 1";
        tituloDescr1.gameObject.SetActive(true);
        contentDescr1.gameObject.SetActive(true);
        tituloDescr2.gameObject.SetActive(false);
        contentDescr2.gameObject.SetActive(false);
        botonModo1.gameObject.GetComponent<Image>().sprite = activadoBoton;
        botonModo2.gameObject.GetComponent<Image>().sprite = desactivadoBoton;

    }

    public void modo2()
    {
        PlayerPrefs.SetInt("Modo", 2);
        textModo.text = "Modo 2";
        tituloDescr1.gameObject.SetActive(false);
        contentDescr1.gameObject.SetActive(false);
        tituloDescr2.gameObject.SetActive(true);
        contentDescr2.gameObject.SetActive(true);
        botonModo1.gameObject.GetComponent<Image>().sprite = desactivadoBoton;
        botonModo2.gameObject.GetComponent<Image>().sprite = activadoBoton;
    }

    public void soundButton()
    {
        if (PlayerPrefs.GetInt("Sonido") == 1)
        {
            PlayerPrefs.SetInt("Sonido", 0);
            botonSonido.gameObject.GetComponent<Image>().sprite = desactivadoCopo;
        }
        else
        {
            PlayerPrefs.SetInt("Sonido", 1);
            botonSonido.gameObject.GetComponent<Image>().sprite = activadoCopo;
        }
    }

    public void espanolButton()
    {
        PlayerPrefs.SetInt("Idioma", 0);
        botonEspanol.gameObject.GetComponent<Image>().sprite = activadoCopo;
        botonIngles.gameObject.GetComponent<Image>().sprite = desactivadoCopo;
    }

    public void englishButton()
    {
        PlayerPrefs.SetInt("Idioma", 1);
        botonEspanol.gameObject.GetComponent<Image>().sprite = desactivadoCopo;
        botonIngles.gameObject.GetComponent<Image>().sprite = activadoCopo;
    }
}
