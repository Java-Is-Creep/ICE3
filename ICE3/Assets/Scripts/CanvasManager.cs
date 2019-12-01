using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Button up;
    public Button down;
    public Button right;
    public Button left;

    public Button fire;

    public bool bindeado;
    public CharacterController personaje = null; /*= FindObjectOfType<CharacterController>();*/
    // Start is called before the first frame update
    void Start()
    {
        bindeado = false;
        //if (Application.isMobilePlatform)
        //{
        Debug.Log("Haciendo start de canvas manager");
            up.gameObject.SetActive(true);
            down.gameObject.SetActive(true);
            right.gameObject.SetActive(true);
            left.gameObject.SetActive(true);
            fire.gameObject.SetActive(true);
            

       // }
    }

    private void Update()
    {
        Debug.Log("Update canvas manager");
        if (!bindeado)
        {
            Debug.Log("Intento de bindear");
            CharacterController[] cc = FindObjectsOfType<CharacterController>();
            
            foreach (CharacterController c in cc)
            {
                if (c.photonView.IsMine)
                {
                    personaje = c;
                }
            }
            if (personaje != null)
            {
                Debug.Log("Personaje que soy yo no es null");
                if (personaje.photonView.IsMine)
                {
                    Debug.Log("Bindeando las funciones");
                    up.onClick.AddListener(personaje.w);
                    down.onClick.AddListener(personaje.s);
                    left.onClick.AddListener(personaje.a);
                    right.onClick.AddListener(personaje.d);
                    fire.onClick.AddListener(personaje.ComprobarDisparo);
                    bindeado = true;
                }
            }
        } else
        {
            Debug.Log("Bindeado es true");
        }
    }
}
