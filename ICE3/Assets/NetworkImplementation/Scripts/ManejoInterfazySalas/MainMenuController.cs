using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    Carrousel miCarrousel;
    // Start is called before the first frame update
    void Start()
    {
        miCarrousel = FindObjectOfType<Carrousel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void jugar()
    {
        PlayerPrefs.SetInt("IndiceEscenario", miCarrousel.current_index);
        SceneManager.LoadScene("Launcher");
    }

}
