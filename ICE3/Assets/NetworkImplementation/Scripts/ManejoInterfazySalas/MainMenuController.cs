using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    Carrousel miCarrousel;
    public Image imagePersonaje;
    int preIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        miCarrousel = FindObjectOfType<Carrousel>();
        preIndex = 0;
        if (!PlayerPrefs.HasKey("Modo"))
        {
            PlayerPrefs.SetInt("Modo", 1);
        }
        if (!PlayerPrefs.HasKey("IndiceEscenario"))
        {
            miCarrousel.GoToIndex(PlayerPrefs.GetInt("IndiceEscenario"));
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

    public void seleccionarModo1()
    {
        PlayerPrefs.SetInt("Modo", 1);
    }

    public void seleccionarModo2()
    {
        PlayerPrefs.SetInt("Modo", 2);
    }

}
