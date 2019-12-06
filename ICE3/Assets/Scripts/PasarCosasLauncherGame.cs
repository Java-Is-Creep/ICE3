using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasarCosasLauncherGame : MonoBehaviour
{
    public bool tutorial;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        tutorial = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activarTutorial()
    {
        tutorial = true;
    }
}
