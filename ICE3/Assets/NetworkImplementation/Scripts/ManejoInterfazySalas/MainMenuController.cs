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
    public Sprite sinPulsar;
    public Sprite pulsar;
    int preIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        miCarrousel = FindObjectOfType<Carrousel>();
        /*if (PlayerPrefs.HasKey("IndiceEscenario"))
        {
            miCarrousel.GoToIndexSmooth(PlayerPrefs.GetInt("IndiceEscenario"));
        }*/

        if (PlayerPrefs.HasKey("Modo"))
        {
            if (PlayerPrefs.GetInt("Modo") == 1)
            {
                textModo.text = "Modo 1";
                tituloDescr1.gameObject.SetActive(true);
                contentDescr1.gameObject.SetActive(true);
                tituloDescr2.gameObject.SetActive(false);
                contentDescr2.gameObject.SetActive(false);
                //botonModo1.gameObject.GetComponent<SpriteRenderer>().sprite = 
            }
            else
            {
                textModo.text = "Modo 2";
                tituloDescr1.gameObject.SetActive(false);
                contentDescr1.gameObject.SetActive(false);
                tituloDescr2.gameObject.SetActive(true);
                contentDescr2.gameObject.SetActive(true);
            }
        }

        preIndex = 0;
        if (!PlayerPrefs.HasKey("Modo"))
        {
            PlayerPrefs.SetInt("Modo", 1);
        }

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

    }

    public void modo2()
    {
        PlayerPrefs.SetInt("Modo", 2);
        textModo.text = "Modo 2";
        tituloDescr1.gameObject.SetActive(false);
        contentDescr1.gameObject.SetActive(false);
        tituloDescr2.gameObject.SetActive(true);
        contentDescr2.gameObject.SetActive(true);
    }

}
