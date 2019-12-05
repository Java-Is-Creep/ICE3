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

    // Textos
    [Header("FRONT")]
    public Text creditosFront;
    public Text personajeFront;
    public Text modoJuegoFront;
    public Text opcionesFront;
    public Text modoTextFront;

    [Header("TOP")]
    public Text menuTop;
    public Text titleTop;
    public Text contentScrollTop;
    public Text donarTextTop;

    [Header("BOTTOM")]
    public Text menuBot;
    public Text titleBot;
    public Text sonidoTextBot;
    public Text idiomaTextBot;

    [Header("LEFT")]
    public Text infoLeft;
    public Text menuLeft;
    public Text titleLeft;
    public Text modo1TextLeft;
    public Text modo2TextLeft;

    [Header("BACK")]
    public Text personajeBack;
    public Text modoJuegoBack;
    public Text titleBack;
    public Text titleGameMode1Back;
    public Text contentGameMode1Back;
    public Text titleGameMode2Back;
    public Text contentGameMode2Back;

    [Header("RIGHT")]
    public Text infoRight;
    public Text menuRight;
    public Text titleRight;

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
                textSpanish();
            }
            else
            {
                botonEspanol.gameObject.GetComponent<Image>().sprite = desactivadoCopo;
                botonIngles.gameObject.GetComponent<Image>().sprite = activadoCopo;
                textEnglish();
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
            AudioListener.volume = 0;
        }
        else
        {
            PlayerPrefs.SetInt("Sonido", 1);
            botonSonido.gameObject.GetComponent<Image>().sprite = activadoCopo;
            AudioListener.volume = 1;
        }
    }

    public void espanolButton()
    {
        PlayerPrefs.SetInt("Idioma", 0);
        botonEspanol.gameObject.GetComponent<Image>().sprite = activadoCopo;
        botonIngles.gameObject.GetComponent<Image>().sprite = desactivadoCopo;
        textSpanish();
    }

    public void englishButton()
    {
        PlayerPrefs.SetInt("Idioma", 1);
        botonEspanol.gameObject.GetComponent<Image>().sprite = desactivadoCopo;
        botonIngles.gameObject.GetComponent<Image>().sprite = activadoCopo;
        textEnglish();
    }

    public void textSpanish()
    {
        // FRONT
        creditosFront.text = "Créditos";
        personajeFront.text = "Personaje";
        modoJuegoFront.text = "Modo\nJuego";
        opcionesFront.text = "Opciones";
        if (PlayerPrefs.GetInt("Modo") == 0)
        {
            modoTextFront.text = "Modo 1";
        }
        else
        {
            modoTextFront.text = "Modo 2";
        }
        

        // TOP
        menuTop.text = "Menú";
        titleTop.text = "Créditos";
        donarTextTop.text = "Donar";
        contentScrollTop.text = "PATATA\nPATATA VIVA ESPAÑA";

        // BOT
        menuBot.text = "Menú";
        titleBot.text = "Opciones";
        sonidoTextBot.text = "Sonido";
        idiomaTextBot.text = "Idioma";

        // LEFT
        infoLeft.text = "Info";
        menuLeft.text = "Menú";
        titleLeft.text = "Modos De Juego";
        modo1TextLeft.text = "Modo 1";
        modo2TextLeft.text = "Modo 2";

        // BACK
        personajeBack.text = "Personaje";
        modoJuegoBack.text = "Modo\nJuego";
        titleBack.text = "Información Modo";
        titleGameMode1Back.text = "Pelea bolas de nieve";
        contentGameMode1Back.text = "En este modo, deberás hacer uso de tu puntería para acertar 5 bolazos antes que los demás";
        titleGameMode2Back.text = "Congélate";
        contentGameMode2Back.text = "Se el más rápido en el cubo, consigiuiendo coger las banderas";

        // RIGHT
        infoRight.text = "Info";
        menuRight.text = "Menú";
        titleRight.text = "Selector Personaje";
    }

    public void textEnglish()
    {
        // FRONT
        creditosFront.text = "Credits";
        personajeFront.text = "Character";
        modoJuegoFront.text = "Game\nMode";
        opcionesFront.text = "Options";
        if (PlayerPrefs.GetInt("Modo") == 0)
        {
            modoTextFront.text = "Mode 1";
        }
        else
        {
            modoTextFront.text = "Mode 2";
        }


        // TOP
        menuTop.text = "Menu";
        titleTop.text = "Credits";
        donarTextTop.text = "Donate";
        contentScrollTop.text = "PATATA\nPATATA BEBO TE PORQUE SOY PIJO";

        // BOT
        menuBot.text = "Menu";
        titleBot.text = "Options";
        sonidoTextBot.text = "Sound";
        idiomaTextBot.text = "Language";

        // LEFT
        infoLeft.text = "Info";
        menuLeft.text = "Menu";
        titleLeft.text = "Game Mode";
        modo1TextLeft.text = "Mode 1";
        modo2TextLeft.text = "Mode 2";

        // BACK
        personajeBack.text = "Character";
        modoJuegoBack.text = "Modo\nJuego";
        titleBack.text = "Mode Information";
        titleGameMode1Back.text = "Fight snowballs";
        contentGameMode1Back.text = "In this mode, you must use your aim to hit 5 balls before the others";
        titleGameMode2Back.text = "Freeze";
        contentGameMode2Back.text = "Be the fastest in the cube, getting the flags";

        // RIGHT
        infoRight.text = "Info";
        menuRight.text = "Menu";
        titleRight.text = "Choose Character";
    }

    public void instaButton()
    {
        Application.OpenURL("https://www.instagram.com/java_is_creep/");
    }

    public void twitterButton()
    {
        Application.OpenURL("https://twitter.com/Java_Is_Creep");
    }
    
    public void youtubeButton()
    {
        Application.OpenURL("https://www.youtube.com/channel/UCPiuFVLDn7vtVCGowXJ7CVw");
    }

    public void javaIsCreepButton()
    {
        Application.OpenURL("https://java-is-creep.github.io/Portfolio/index.html");
    }
}
